using SuperMatjar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMatjar.Core
{
    public interface IBusinessService<TDataModel, TApiModel> where TDataModel : class, IDataModel
    {
<<<<<<< HEAD
        Task<ApiResponse<TApiModel>> GetDtoAsync(long id);
        Task<ApiResponse<TApiModel>> GetDtoAsync(RequestInputs actionParams = null);


        //Task<ApiResponse<TApiModel>> GetDtoAsync(long id);
        //Task<ApiResponse<TApiModel>> GetDtoAsync(RequestInputs actionParams = null);


=======
        Task<ApiResponse<TApiModel>> GetAsync(long id);
        Task<ApiResponse<TApiModel>> GetAsync(IList<long> ids);
        Task<ApiResponse<TApiModel>> GetAsync(RequestInputs actionParams = null);
>>>>>>> 6c0d577ace8c7debbc95cbef8b2ac25788f8a85a
        Task<ApiResponse<TApiModel>> CreateAsync(TApiModel model);
        Task<ApiResponse<TApiModel>> UpdateAsync(TApiModel model);
        Task<ApiResponse<TApiModel>> DeleteAsync(long id);
    }
}
