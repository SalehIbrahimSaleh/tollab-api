using SuperKotob.Admin;
using SuperKotob.Admin.Core;
using SuperKotob.Admin.Core.Utils.Validation;
using SuperKotob.Admin.UseCases.Base;
using SuperKotob.Admin.Utils.Mapping;
using SuperKotob.Data.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.UseCases.PushTokens
{
    public class PushTokenService : BaseService<PushToken, PushTokenDTO>
    {
        private IPushManager _pushManager;
        private PushTokenRepository _repository;

        public PushTokenService(
           PushTokenRepository repository,
            IValidator<PushTokenDTO> validator,
            IDataMapper mapper,
            IPushManager pushManager)
            : base(repository, validator, mapper)
        {
            _pushManager = pushManager;
            _repository = repository;
        }
        public async Task<ApiResponse<PushTokenDTO>> SendPushNotification(long customerId, string message,string title,string url)
        {
            return await this.SendPushNotification(new PushTokenDTO()
            {
                CustomerId = customerId,
                Message = message,
                Title=title,
                Url=url
                

            });
        }
        public async Task<ApiResponse<PushTokenDTO>> SendPushNotification(IList<long> customerIds, string message, string title, string url)
        {
            foreach (var id in customerIds)
            {
                return await this.SendPushNotification(new PushTokenDTO()
                {
                    CustomerId = id,
                    Message = message,
                    Title = title,
                    Url = url
                });
            }
            return new ApiResponse<PushTokenDTO>();
        }
        public async Task<ApiResponse<PushTokenDTO>> SendPushNotification(PushTokenDTO message)
        {
            var strValue = ConfigurationManager.AppSettings["push_enabled"];
            bool enabled = true;
            bool.TryParse(strValue, out enabled);
            if (!enabled)
                return new ApiResponse<PushTokenDTO>();

            var apiResponse = new ApiResponse<PushTokenDTO>(new List<PushTokenDTO>());
            var response = await _repository.GetAsync(Convert.ToInt64(message.CustomerId));
            foreach (var item in response.Data)
            {
                _pushManager.Push(item.Token, message.Message, message.OS,message.Title,message.Url);
                apiResponse.Model.Add(new PushTokenDTO()
                {
                    CustomerId = item.CustomerId,
                    CustomerTypeId = item.CustomerTypeId,
                    OS = message.OS,
                    Title=message.Title,
                    Url=message.Url

                });
            }
            return apiResponse;
        }
        protected override PushToken MapToDataModel(PushTokenDTO model)
        {
            if (model == null)
                return null;

            return new PushToken()
            {
                Id = model.Id,
                OS = model.OS,
                Token = model.Token,
                CustomerId = model.CustomerId,
                CustomerTypeId = model.CustomerTypeId,
            };
        }
        protected override IList<PushTokenDTO> MapToApiModel(params PushToken[] dataModel)
        {
            var list = new List<PushTokenDTO>();
            foreach (var dataitem in dataModel)
            {
                if (dataitem == null)
                    continue;

                list.Add(new PushTokenDTO()
                {
                    OS = dataitem.OS,
                    Token = dataitem.Token,
                    CustomerId = dataitem.CustomerId,
                    Id = dataitem.Id,
                    CustomerTypeId = dataitem.CustomerTypeId
                });
            }
            return list;
        }
    }
}
