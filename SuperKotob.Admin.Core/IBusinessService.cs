using SuperKotob.Admin.Models;
using SuperKotob.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Admin.Core
{
    public interface IBusinessService<TDataModel, TApiModel> 
        where TDataModel : class, IDataModel
        where TApiModel : class
    {


        Task<DataResponse<TDataModel>> GetAsync(long id);
        Task<DataResponse<TDataModel>> GetAsync(RequestInputs requestInputs = null);

        Task<DataResponse<TDataModel>> CreateAsync(TDataModel model);
        Task<DataResponse<TDataModel>> UpdateAsync(TDataModel model);


        Task<ApiResponse<TApiModel>> GetDtoAsync(long id);
        Task<ApiResponse<TApiModel>> GetDtoAsync(IList<long> ids);
        Task<ApiResponse<TApiModel>> GetDtoAsync(RequestInputs requestInputs = null);
        Task<ApiResponse<TApiModel>> CreateDtoAsync(TApiModel model);
        Task<ApiResponse<TApiModel>> UpdateDtoAsync(TApiModel model);
        Task<DataResponse<TDataModel>> DeleteAsync(long id);
        Task<DataResponse<LookupItem>> AutoCompleteAsync(string text, bool isIdList, RequestInputs requestInputs, string field);
        Task<DataResponse<LookupItem>> AutoCompleteAsync(string text, bool isIdList, RequestInputs requestInputs, string field, string wherefield);
       // Task<DataResponse<LookupItem>> AutoCompleteAreaMangersOnly(string text, bool isIdList, RequestInputs requestInputs);
        //    Task<DataResponse<LookupItem>> AutoCompleteNameArabicAsync(string text, bool isIdList, RequestInputs requestInputs);
    }

}
