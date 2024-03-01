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
        Task<ApiResponse<TApiModel>> GetDtoAsync(long id);
        Task<ApiResponse<TApiModel>> GetDtoAsync(RequestInputs actionParams = null);


        //Task<ApiResponse<TApiModel>> GetDtoAsync(long id);
        //Task<ApiResponse<TApiModel>> GetDtoAsync(RequestInputs actionParams = null);


        Task<ApiResponse<TApiModel>> CreateAsync(TApiModel model);
        Task<ApiResponse<TApiModel>> UpdateAsync(TApiModel model);
        Task<ApiResponse<TApiModel>> DeleteAsync(long id);
    }
}
