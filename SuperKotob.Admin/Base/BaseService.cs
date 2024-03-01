using SuperKotob.Admin.Core;
using SuperKotob.Admin.Core.Utils.Validation;
using SuperKotob.Admin.Data;
using SuperKotob.Admin.Models;
using SuperKotob.Admin.Utils.Mapping;
using SuperKotob.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Admin.UseCases.Base
{
    public class BaseService<TDataModel, TApiModel> : IBusinessService<TDataModel, TApiModel>
        where TDataModel : class, IDataModel
        where TApiModel : class
    {
        protected IRepository<TDataModel> Repository { get; private set; }
        protected IDataMapper DataMapper { get; private set; }
        public IValidator<TApiModel> Validator { get; }

        public BaseService(
            IRepository<TDataModel> repository,
            IValidator<TApiModel> validator,
            IDataMapper mapper)
        {
            this.Repository = repository;
            this.DataMapper = mapper;
            this.Validator = validator;
        }




        public virtual async Task<DataResponse<TDataModel>> GetAsync(long id)
        {
            if (id < 0)
                return new DataResponse<TDataModel>((TDataModel)null, "Id should be more than zero");

            return await GetAsync(new RequestInputs(new Dictionary<string, string>()
            {
                ["id"] = id.ToString()
            }));

        }

        public virtual async Task<DataResponse<TDataModel>> GetAsync(RequestInputs requestInputs = null)
        {
            return await Repository.GetAsync(requestInputs);
        }

        
        protected ApiMeta GetApiMeta(PagingMeta pagingMeta)
        {
            if (pagingMeta == null)
                return null;

            var metas = new ApiMeta()
            {
                Page = pagingMeta.Page,
                TotalCount = pagingMeta.TotalCount,
                MaxPage = pagingMeta.MaxPage
            };
            return metas;
        }




        public virtual async Task<DataResponse<TDataModel>> CreateAsync(TDataModel model)
        {
            var response = await Repository.CreateAsync(model);
            return await AfterCreateOrUpdate(model, response);//fghfkf
        }

        public virtual async Task<DataResponse<TDataModel>> UpdateAsync(TDataModel model)
        {
            var response = await Repository.UpdateAsync(model);
            return await AfterCreateOrUpdate(model, response);
        }

        public virtual async Task<DataResponse<LookupItem>> AutoCompleteAsync(string text, bool isIdList, RequestInputs requestInputs, string field)
        {
            return await Repository.AutoCompleteAsync(text, isIdList, requestInputs,field);
        }
        public virtual async Task<DataResponse<LookupItem>> AutoCompleteAsync(string text, bool isIdList, RequestInputs requestInputs, string field, string Wherefield)
        {
            return await Repository.AutoCompleteAsync(text, isIdList, requestInputs, field,Wherefield);
        }
        //public virtual async Task<DataResponse<LookupItem>> AutoCompleteAreaMangersOnly(string text, bool isIdList, RequestInputs requestInputs)
        //{
        //    return await Repository.AutoCompleteAreaMangersOnly(text, isIdList, requestInputs);
        //}





        public virtual async Task<ApiResponse<TApiModel>> GetDtoAsync(long id)
        {
            if (id < 0)
                return new ApiResponse<TApiModel>((TApiModel)null, "Id should be more than zero");

            return await GetDtoAsync(new RequestInputs(new Dictionary<string, string>()
            {
                ["id"] = id.ToString()
            }));
        }
        public virtual async Task<ApiResponse<TApiModel>> GetDtoAsync(IList<long> ids)
        {
            if (ids == null || ids.Where(item => item > 0).Count() < 1)
                return new ApiResponse<TApiModel>((TApiModel)null);

            return await GetDtoAsync(new RequestInputs(new Dictionary<string, string>()
            {
                ["ids"] = string.Join(", ", ids)
            }));
        }

        public virtual async Task<ApiResponse<TApiModel>> GetDtoAsync(RequestInputs requestInputs)
        {
            requestInputs.Set("AsNoTracking", "True");
            var dataResponse = await this.Repository.GetAsync(requestInputs);
            var apiModel = MapToApiModel(dataResponse.Data?.ToArray());
            var metas = GetApiMeta(dataResponse.Paging);

            var response = new ApiResponse<TApiModel>()
            {
                Model = apiModel,
                Errors = dataResponse.Errors,
                Metas = metas
            };
            return response;
        }
        public virtual async Task<ApiResponse<TApiModel>> CreateDtoAsync(TApiModel model)
        {
            return await CreateOrUpdate(CrudAction.Create, model);
        }
        enum CrudAction
        {
            Create,
            Update
        }
        private async Task<ApiResponse<TApiModel>> CreateOrUpdate(CrudAction action, TApiModel model)
        {
            var validationContext = new ValidatorContext<TApiModel>(nameof(model), model);
            var validationResult = await this.Validator.ValidateAsync(validationContext);
            if (!validationResult.IsValid)
            {
                return new ApiResponse<TApiModel>()
                {
                    Errors = validationResult.Messages
                };
            }

            var dataModel = MapToDataModel(model);
            DataResponse<TDataModel> response;

            if (action == CrudAction.Update || dataModel.Id > 0)
                response = await this.Repository.UpdateAsync(dataModel);
            else
                response = await this.Repository.CreateAsync(dataModel);

            return await AfterCreateOrUpdateDto(dataModel, response);
        }

        protected virtual async Task<ApiResponse<TApiModel>> AfterCreateOrUpdateDto(TDataModel dataModel, DataResponse<TDataModel> response)
        {
            var getResponse = await this.Repository.GetAsync(dataModel.Id);
            var apiModel = MapToApiModel(getResponse.Data?.ToArray());

            var apiResponse = new ApiResponse<TApiModel>()
            {
                Errors = response.Errors,
                Model = apiModel
            };

            return apiResponse;
        }
        protected virtual async Task<DataResponse<TDataModel>> AfterCreateOrUpdate(TDataModel dataModel, DataResponse<TDataModel> response)
        {
            return response;
        }

        public virtual async Task<ApiResponse<TApiModel>> UpdateDtoAsync(TApiModel model)
        {
            return await CreateOrUpdate(CrudAction.Update, model);
        }
        protected virtual TDataModel MapToDataModel(TApiModel model)
        {
            return DataMapper.Map<TDataModel>(model);
        }
        protected virtual IList<TApiModel> MapToApiModel(params TDataModel[] dataModel)
        {
            return DataMapper.Map<IList<TApiModel>>(dataModel);
        }
        public virtual async Task<DataResponse<TDataModel>> DeleteAsync(long id)
        {
            var response = await this.Repository.DeleteAsync(id);
            return response;
        }

    }
}
