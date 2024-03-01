using SuperKotob.Admin.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperKotob.Admin.Core;

namespace SuperKotob.Admin.Data.Repositories
{
    public class SignUpsRepository : BaseRepository<SignUp>
    {
        public override async Task<DataResponse<SignUp>> GetAsync(RequestInputs requestInputs)
        {
            var phoneNumber = requestInputs.GetValue<string>("PhoneNumber");
            var confirmationCode = requestInputs.GetValue<string>("ConfirmationCode");



            if (!string.IsNullOrWhiteSpace(phoneNumber)) //mansour && !string.IsNullOrWhiteSpace(confirmationCode))
            {
               var model = DataContext.SignUps
                      .Where(item => item.PhoneNumber == phoneNumber)
                      .Where(item => item.ConfirmationCode == confirmationCode)
                      .FirstOrDefault();

                return new DataResponse<SignUp>(new List<SignUp>() { model });
            }

            return await base.GetAsync(requestInputs);
        }
        public override async Task<DataResponse<SignUp>> CreateAsync(SignUp model)
        {
            DataContext.Database.ExecuteSqlCommand($"delete SignUps where PhoneNumber = '{model.PhoneNumber}'");
            return await base.CreateAsync(model);
        }
    }
}
