using SuperKotob.Admin.Core;
using SuperKotob.Admin.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperKotob.Admin.Models;
using SuperKotob.Models;

namespace SuperKotob.Admin.Data
{
    public interface IRepository<T> where T : class,  SuperKotob.Admin.Models.IDataModel
    {
        Task<DataResponse<T>> GetAsync(long id);
        Task<DataResponse<T>> GetAsync(RequestInputs requestInputs = null);
        Task<DataResponse<T>> CreateAsync(T model);
        Task<DataResponse<T>> UpdateAsync(T model);
        Task<DataResponse<T>> DeleteAsync(long id);
        Task<DataResponse<T>> DeleteAsync(IList<long> ids);
        Task<DataResponse<LookupItem>> AutoCompleteAsync(string text, bool isIdList, RequestInputs requestInputs, string field);
        Task<DataResponse<LookupItem>> AutoCompleteAsync(string text, bool isIdList, RequestInputs requestInputs, string field, string wherefield);
        //Task<DataResponse<LookupItem>> AutoCompleteAsync(string text, bool isIdList, RequestInputs requestInputs, string field, string whereField);
       // Task<DataResponse<LookupItem>> AutoCompleteAreaMangersOnly(string text, bool isIdList, RequestInputs requestInputs);
    }
}
