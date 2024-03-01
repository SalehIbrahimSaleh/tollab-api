using SuperKotob.Admin.Core;
using SuperKotob.Admin.Data.Models;
using SuperKotob.Admin.Data.TransferObjects;
using SuperKotob.Admin.Utils.Payment;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using SuperKotob.Admin.Utils.Configuration;
using SuperKotob.Admin.BusinessServices;
using Newtonsoft.Json;
using System.Text;
using System.Security.Cryptography;

namespace SuperKotob.Admin.Payment
{
    public class PayfortPaymentService : IPaymentService
    {
        CustomerCardService _customerCardService;
        PayfortHttpClient _payfortClient;
        IAppConfigurations _appConfigurations;
        IBusinessService<Customer, CustomerDTO> _customerService;
        IBusinessService<Order, OrderDTO> _orderService;

        public PayfortPaymentService(
            IBusinessService<Customer, CustomerDTO> customerService,
            CustomerCardService customerCardService,
            PayfortHttpClient payfortClient,
            IBusinessService<Order, OrderDTO> orderService,
            IAppConfigurations appConfigurations
            )
        {
            _customerService = customerService;
            _customerCardService = customerCardService;
            _payfortClient = payfortClient;
            _appConfigurations = appConfigurations;
            _orderService = orderService;
        }

        public async Task<DataResponse<CustomerCard>> GetDefaultCardAsync(RequestInputs requestInputs)
        {
            var strCustomerId = requestInputs.GetValue<string>("customer_id");
            long customerId;
            if (!long.TryParse(strCustomerId, out customerId))
            {
                return null;
            }

            var response = await _customerCardService.GetDefaultCardForCustomerAsync(customerId);
            return response;
        }

        public async Task<ApiResponse<PaymentHistoryDTO>> History(RequestInputs inputs)
        {
            var orders = await _orderService.GetDtoAsync(inputs);
            var list = orders.Model.Select(item => new PaymentHistoryDTO()
            {
                OrderId = item.Id,
                Amount = item.TotalPrice,
                DateTime = item.Date,
                CardNumber = item.Card.Name,
                //TypeId = item.Card.
            }).ToList();
            var response = new ApiResponse<PaymentHistoryDTO>(list);

            return response;
        }

        async Task<CustomerDTO> GetCustomer(RequestInputs requestInputs)
        {
            var strCustomerId = requestInputs.GetValue<string>("customer_id");
            long customerId;
            if (long.TryParse(strCustomerId, out customerId))
            {
                var customerResponse = await _customerService.GetDtoAsync(customerId);
                if (customerResponse.HasErrors || !customerResponse.HasModel)
                    return null;
                return customerResponse.Model.First();
            }
            return null;
        }

        public async Task<ApiResponse<KeyValuePair<string, string>>> Purchase(RequestInputs requestInputs)
        {
            var customerId = requestInputs.GetValue<string>("customer_id");
            var customer = await GetCustomer(requestInputs);
            if (customer == null)
            {
                return new ApiResponse<KeyValuePair<string, string>>(null, "Can not find customer with provided id");
            }

            var response = await GetDefaultCardAsync(requestInputs);
            if (response.HasErrors || !response.HasData)
                return new ApiResponse<KeyValuePair<string, string>>(null, response.Errors);


            var card = response.Data.First();
            var amount = requestInputs.GetValue<string>("amount");
            var dictionary = await BuildParamsDictionary(requestInputs, amount, customer, card);
            var data = await SendPurchaseRequest(dictionary);
            return new ApiResponse<KeyValuePair<string, string>>(data.ToList());
        }

        private async Task<Dictionary<string, string>> SendPurchaseRequest(Dictionary<string, string> dictionary)
        {
            var payfortResponse = await _payfortClient.PostJsonAsync(dictionary);
            var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(payfortResponse);

            string responseCode;
            string url_3ds;

            if (data.TryGetValue("response_code", out responseCode) &&
                data.TryGetValue("3ds_url", out url_3ds) &&
                responseCode == "20064")
            {
                Task.Run(() =>
                {
                    _payfortClient.Make3dsUrl(url_3ds);
                });
            }

            return data;
        }

        private async Task<Dictionary<string, string>> BuildParamsDictionary(RequestInputs requestInputs, string amount, CustomerDTO customer, CustomerCard card)
        {
            var appUrl = await _appConfigurations.AppUrl.GetValueAsync();
            appUrl = appUrl.TrimEnd('/');

            var dic = new Dictionary<string, string>()
            {
                ["command"] = "PURCHASE",
                ["access_code"] = PayfortHttpClient.access_code,
                ["merchant_identifier"] = PayfortHttpClient.merchant_identifier,
                ["return_url"] = appUrl + $"/api/payments/after-purchase/{customer.Id}",
                ["merchant_reference"] = Guid.NewGuid().ToString(),
                ["language"] = "en",
                ["currency"] = "QAR",
                ["customer_email"] = customer.Email,
                ["amount"] = amount
            };
            AddCardDetails(dic, requestInputs, card);

            AddIfHasValue(dic, requestInputs, "merchant_reference");
            if (card.StatusId != null && card.StatusId > 2)
                AddIfHasValue(dic, requestInputs, "card_security_code");


            dic.Remove("signature");
            AddSignature(dic, card.StatusId == 3);
            return dic;
        }

        List<string> ignore = new List<string>() {
                "signature",
                "card_security_code",
                "card_number",
                "expiry_date",
                "card_holder_name",
                "remember_me"
            };
        List<string> secondTimePurchaseIgnore = new List<string>() {
                "signature",
                "card_number",
                "expiry_date",
                "card_holder_name",
                "remember_me"
            };

        public IDictionary<string, string> AddSignature(IDictionary<string, string> dic, bool includeCardSecurityCode = true)
        {
            var args = dic.ToList();
            var orderedList = args.OrderBy(item => item.Key).ToList();
            var sb = new StringBuilder();
            sb.Append(PayfortHttpClient.RequestSHAKey);
            var ignoreList = includeCardSecurityCode ? secondTimePurchaseIgnore : ignore;
            foreach (var item in orderedList)
            {

                if (ignoreList.Contains(item.Key))
                    continue;

                sb.Append(item.Key);
                sb.Append("=");
                sb.Append(item.Value);

            }
            sb.Append(PayfortHttpClient.RequestSHAKey);

            var text = sb.ToString();
            var hashValue = Sha256(text);
            dic.Add("signature", hashValue);
            return dic;
        }
        string Sha256(string text)
        {
            var crypt = SHA256.Create();
            System.Text.StringBuilder hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(text), 0, Encoding.UTF8.GetByteCount(text));
            //return Encoding.UTF8.GetString(crypto);
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }
        //public async Task<ApiResponse<KeyValuePair<string, string>>> Purchase(RequestInputs requestInputs)
        //{
        //    var appUrl = await _appConfigurations.AppUrl.GetValueAsync();
        //    appUrl = appUrl.TrimEnd('/');

        //    var customerId = requestInputs.GetValue<string>("customer_id");
        //    var amount = requestInputs.GetValue<string>("amount");

        //    var response = await GetDefaultCardAsync(requestInputs);
        //    if (response.HasErrors || !response.HasData)
        //        return new ApiResponse<KeyValuePair<string, string>>(null, response.Errors);

        //    var dic = new Dictionary<string, string>()
        //    {
        //        ["command"] = "PURCHASE",
        //        ["access_code"] = PayfortHttpClient.access_code,
        //        ["merchant_identifier"] = PayfortHttpClient.merchant_identifier,
        //        ["return_url"] = appUrl + $"/api/payments/after-purchase/{customerId}",
        //        ["merchant_reference"] = Guid.NewGuid().ToString(),
        //        ["language"] = "en",
        //        ["currency"] = "QAR",
        //        ["amount"] = amount
        //    };

        //    AddCardDetails(dic, requestInputs, response.Data.First());

        //    AddIfHasValue(dic, requestInputs,
        //        "command",
        //        "access_code",
        //        "merchant_identifier",
        //        "return_url",
        //        "check_3ds",
        //        "merchant_reference",
        //        "language",
        //        "currency",
        //        "amount",
        //        "token_name",
        //        "customer_email",
        //        "card_security_code");

        //    var payfortResponse = await _payfortClient.PostJsonAsync(dic);
        //    var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(payfortResponse);

        //    return new ApiResponse<KeyValuePair<string, string>>(data.ToList());
        //}

        private void AddCardDetails(Dictionary<string, string> dic, RequestInputs requestInputs, CustomerCard customerCard)
        {
            dic["token_name"] = customerCard.PaymentProviderToken;
            if (customerCard.StatusId == 3) // used before
                dic["card_security_code"] = requestInputs.GetValue<string>("card_security_code");
        }

        private void AddIfHasValue(Dictionary<string, string> dic, RequestInputs requestInputs, params string[] keys)
        {
            foreach (var key in keys)
            {
                var value = requestInputs.GetValue(key, "");
                if (!string.IsNullOrWhiteSpace(value))
                    dic[key] = value;
            }
        }

        public async Task<string> AfterPurchase(RequestInputs requestInputs)
        {
            var customerId = requestInputs.GetValue<long>("customerid");
            var cardNumber = requestInputs.GetValue<string>("card_number");

            var response = await _customerCardService.GetByCardNumberAsync(cardNumber);
            if (response.HasErrors || !response.HasModel)
                return null;

            var card = response.Model.First();
            if (card.StatusId == null || card.StatusId < 3)
            {
                card.StatusId = 3;
                await _customerCardService.UpdateDtoAsync(card);
            }

            // var dic = requestInputs.ToDictionary();
            // var response = await _payfortClient.PostJsonAsync(dic);
            return "";
        }

        public async Task<string> Register(CustomerCardRegisterDTO model)
        {
            var appUrl = await _appConfigurations.AppUrl.GetValueAsync();
            appUrl = appUrl.TrimEnd('/');


            var dictionary = new Dictionary<string, string>()
            {
                ["service_command"] = "TOKENIZATION",
                ["access_code"] = PayfortHttpClient.access_code,
                ["merchant_identifier"] = PayfortHttpClient.merchant_identifier,
                ["merchant_reference"] = Guid.NewGuid().ToString(),
                ["language"] = "en",
                ["expiry_date"] = model.ExpiryDate,
                ["card_number"] = model.CardNumber,
                ["card_security_code"] = model.SecurityCode,
                ["card_holder_name"] = model.HolderName,
                ["remember_me"] = "YES",
                ["return_url"] = appUrl + $"/api/customercards/after-card-registration/{model.CustomerId}"
            };

            dictionary.Remove("signature");
            AddSignature(dictionary, false);

            var x = await _payfortClient.PostFormsAsync(dictionary);

            return x;
        }

        public async Task AfterCardRegistration(RequestInputs requestInputs)
        {
            //var dic = await Purchase(requestInputs);
            var registerationDTO = new CustomerCardRegisterDTO()
            {
                CardNumber = requestInputs.GetValue<string>("card_number"),
                CustomerId = requestInputs.GetValue("customer_id", default(long)),
                ExpiryDate = requestInputs.GetValue<string>("expiry_date"),
                SecurityCode = requestInputs.GetValue<string>("card_security_code"),
                CardToken = requestInputs.GetValue<string>("token_name")
            };
            if (requestInputs.GetValue("response_code", "") != "18000")
            {
                registerationDTO.ErrorText = JsonConvert.SerializeObject(requestInputs.ToDictionary());
            }
          
            var response = await _customerCardService.Create(registerationDTO);

            //    [0]: {[response_code, 18000]    
            //    [1]: {[card_number, 400555******0001]
            //    [2]: {[card_holder_name, ahmed ali]}
            //    [3]: {[signature, 6d9580a53badebf95291e0bd30f09c4598b6178bef28e54e4a5576fd7c4afef4]}
            //    [4]: {[merchant_identifier, RCosLRqQ]}
            //    [5]: {[expiry_date, 1705]}
            //    [6]: {[access_code, LcDyBFuLpBRASYrIcVjh]}
            //    [7]: {[language, en]}
            //    [8]: {[service_command, TOKENIZATION]}
            //    [9]: {[response_message, Success]}
            //    [10]: {[merchant_reference, dcfa60e5-49df-4a86-935a-a0bcf94609e9]}
            //    [11]: {[token_name, 4CD1008D3D1C6A6CE053321E320A4814]}
            //    [12]: {[return_url, http://localhost:10723/api/payments/messages/123]}
            //    [13]: {[remember_me, YES]}
            //    [14]: {[card_bin, 400555]}
            //    [15]: {[status, 18]}{
        }

    }
}
