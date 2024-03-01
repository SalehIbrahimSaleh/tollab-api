using SuperKotob.Admin.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperKotob.Admin.Utils.Sms;
using Nexmo.Api;
using SuperKotob.Admin.Utils.Http;
using Jil;

namespace SuperKotob
{
    public class NexmoSmsService : ISmsService
    {
        IHttpClient HttpClient { get; set; }
        public string ApiUrl { get; private set; }

        public NexmoSmsService(IHttpClient httpClient)
        {
            this.HttpClient = httpClient;
            this.ApiUrl = "https://rest.nexmo.com/sms/json";
        }


        public async Task<SmsServiceResponse> SendAsync(SmsMessage message)
        {
            var data = new
            {
                api_key = "28dd9f07",
                api_secret = "56a0de4a97c1af32",
                to = message.To,
                from = message.From,
                text = message.Text
            };


            var response = await this.HttpClient.PostJsonAsync(ApiUrl, data);
            var responseText = await response.Content.ReadAsStringAsync();
            var serviceResponse = ConverToSuperKotobResponse(responseText);
            return serviceResponse;
        }
        private SmsServiceResponse ConverToSuperKotobResponse(string responseText)
        {
            responseText = responseText.Replace("\"error-text\"", "\"error_text\"");

            var smsResponse = JSON.Deserialize<SMS.SMSResponse>(responseText);
            var serviceResponse = ConverToSuperKotobResponse(smsResponse);
            return serviceResponse;
        }
        private SmsServiceResponse ConverToSuperKotobResponse(SMS.SMSResponse nexmoResponse)
        {
            var items = nexmoResponse.messages.Select(item => new SmsServiceResponseItem()
            {
                MessageId = item.message_id,
                ClientRef = item.client_ref,
                ErrorText = item.error_text,
                MessagePrice = item.message_price,
                Network = item.network,
                RemainingBalance = item.remaining_balance,
                Status = item.status,
                To = item.to
            }).ToList();

            return new SmsServiceResponse()
            {
                Items = items
            };
        }

        private SMS.SMSRequest ConvertToNexmoRequestMessage(SmsMessage message)
        {
            return new SMS.SMSRequest
            {
                from = message.From,
                to = message.To,
                text = message.Text
            };

        }
    }
}
