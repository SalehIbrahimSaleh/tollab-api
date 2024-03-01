//using SuperKotob.Admin.Core;
//using SuperKotob.Admin.Core.CustomerUseCases;
//using SuperKotob.Admin.Core.Utils.Validation;
//using SuperKotob.Admin.Data;
//using SuperKotob.Admin.Data.Models;
//using SuperKotob.Admin.Data.TransferObjects;
//using SuperKotob.Admin.UseCases.Base;
//using SuperKotob.Admin.Utils.DI;
//using SuperKotob.Admin.Utils.Mapping;
//using SuperKotob.Admin.WebProtector;
//using SuperKotob.Admin.WebProtector.Core;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Web.Security;

//namespace SuperKotob.Admin.BusinessServices
//{
//    public class SignUpService : BaseService<SignUp, SignUpDTO>
//    {

//        IProtectedUserRepository _protectedUserRepository;
//        IDependencyContainerScope _containerScope;
//        IBusinessService<Customer, CustomerDTO> _customersService;

//        public SignUpService(
//            IDependencyContainerScope containerScope,
//            IProtectedUserRepository protectedUserRepository,
//            IBusinessService<Customer, CustomerDTO> customersService,
//            IRepository<SignUp> repository,
//            IValidator<SignUpDTO> validator,
//            IDataMapper dataMapper)
//            : base(repository, validator, dataMapper)
//        {
//            _protectedUserRepository = protectedUserRepository;
//            _containerScope = containerScope;
//            _customersService = customersService;
//        }

//        public virtual async Task<ApiResponse<SignUpConfirmResponseDTO>> Confirm(SignUpConfirmDTO model)
//        {
//            var requestInputs = new RequestInputs(new Dictionary<string, string>()
//            {
//                ["PhoneNumber"] = model.PhoneNumber,
//                ["ConfirmationCode"] = model.ConfirmationCode
//            });

//            var signupResponse = await Repository.GetAsync(requestInputs);
//            var isNumberConfirmed = signupResponse != null && signupResponse.Data.FirstOrDefault() != null;

//            if (isNumberConfirmed)
//            {
//                return await OnNumberConfirmed(signupResponse);
//            }

//            return new ApiResponse<SignUpConfirmResponseDTO>()
//            {
//                Errors = new List<string>(){
//                     "Can't confirm with provided values"
//                }
//            };
//        }

//        async Task<ApiResponse<CustomerDTO>> CreateNewCustomer(SignUp signup)
//        {
//            var dataResponse = await _customersService.CreateAsync(new CustomerDTO()
//            {
//                Name = signup.Name,
//                Email = signup.Email,
//                Phone = signup.PhoneNumber,
//            });

//            var customers = DataMapper.Map<IList<CustomerDTO>>(dataResponse.Model);
//            var customer = customers.FirstOrDefault();
//            return new ApiResponse<CustomerDTO>(customer, dataResponse.Errors);
//        }

//        /// <summary>
//        /// Returns password of the user just created or updated
//        /// </summary>
//        /// <param name="signup"></param>
//        /// <returns>Returns password of the user just created or updated</returns>
//        async Task<ApiResponse<string>> CreateOrUpdateUser(SignUp signup)
//        {
//            IList<string> errors = null;
//            var password = Membership.GeneratePassword(10, 3);

//            var user = await _protectedUserRepository.FindUserByUserName(signup.PhoneNumber);
//            if (user == null)
//            {
//                user = new ProtectedUser()
//                {
//                    FirstName = signup.Name,
//                    UserName = signup.PhoneNumber,
//                    Email = signup.Email,
//                    PhoneNumber = signup.PhoneNumber
//                };
//                var result = await _protectedUserRepository.CreateUserAsync(user, password);
//                errors = result.Errors;
//            }
//            else
//            {
//                var protectedUserResponse = await _protectedUserRepository.ChangeUserPassword(user, password);
//                errors = protectedUserResponse.Errors;
//            }
//            return new ApiResponse<string>(password, errors);
//        }
//        private async Task<ApiResponse<SignUpConfirmResponseDTO>> OnNumberConfirmed(DataResponse<SignUp> signupResponse)
//        {
//            var signup = signupResponse.Data.FirstOrDefault();
//            var createUserResponse = await CreateOrUpdateUser(signup);
//            if (!createUserResponse.HasErrors)
//            {
//                return new ApiResponse<SignUpConfirmResponseDTO>()
//                {
//                    Errors = createUserResponse.Errors
//                };
//            }

//            var password = createUserResponse.Model.First();


//            var customerResponse = await CreateNewCustomer(signup);
//            await DeleteAsync(signup.Id);


//            var customer = customerResponse.Model?.FirstOrDefault();
//            var model = new SignUpConfirmResponseDTO()
//            {
//                CustomerId = customer == null ? 0 : customer.Id,
//                Password = password,
//                Name = customer?.Name,
//                Phone = customer?.Phone,
//                Email = customer?.Email
//            };

//            return new ApiResponse<SignUpConfirmResponseDTO>(model, customerResponse.Errors);
//        }

//        public override async Task<ApiResponse<SignUpDTO>> CreateAsync(SignUpDTO model)
//        {
//            var interactor = _containerScope.GetInstance<RegisterNewDeviceInteractor>();

//            var result = await interactor.RunAsync(new RegisterNewDeviceRequest()
//            {
//                To = model.PhoneNumber
//            });

//            if (result.IsSmsSent)
//            {
//                var dataModel = DataMapper.Map<SignUp>(model);
//                dataModel.ConfirmationCode = result.ConfirmationCode;

//                var repoResult = await this.Repository.CreateAsync(dataModel);
//                var apiModel = DataMapper.Map<IList<SignUpDTO>>(repoResult.Data);

//                var apiResponse = new ApiResponse<SignUpDTO>()
//                {
//                    Model = apiModel
//                };
//                return apiResponse;
//            }
//            return new ApiResponse<SignUpDTO>()
//            {
//                Errors = new List<string>()
//                {
//                    result.ResponseText
//                }
//            };
//        }
//    }
//}
