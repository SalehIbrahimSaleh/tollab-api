using SuperKotob.Admin.Core;
using SuperKotob.Admin.Data.Repositories;
using SuperKotob.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.UseCases.PushTokens
{
    public class PushTokenRepository : BaseRepository<PushToken>
    {
        public async override Task<DataResponse<PushToken>> CreateAsync(PushToken model)
        {
            var pushToken = DataContext.PushTokens
                .Where(item => item.CustomerId == model.CustomerId)
                .Where(item => item.CustomerTypeId == model.CustomerTypeId)
                .Where(item => item.OS == model.OS)
                .FirstOrDefault();

            if (pushToken != null)
            {
                pushToken.Token = model.Token;
                return await UpdateAsync(pushToken);
            }

            return await base.CreateAsync(model);
        }

        public async Task<DataResponse<PushToken>> GetAsync(long customerId)
        {
            var list = DataContext.PushTokens
                .Where(item => item.CustomerId == customerId)
                .ToList();
            return new DataResponse<PushToken>(list);
        }
        public async Task<DataResponse<PushToken>> GetAsync(long customerId, long? customerTypeId, string os)
        {
            var list = DataContext.PushTokens
                .Where(item => item.CustomerId == customerId)
                .Where(item => item.CustomerTypeId == customerTypeId)
                .Where(item => item.OS == os)
                .ToList();
            return new DataResponse<PushToken>(list);
        }
    }
}
