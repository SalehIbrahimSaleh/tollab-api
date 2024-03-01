using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SuperKotob.Admin.Core;
using SuperKotob.Admin.Utils.Logging;
using SuperMatjar.WebProtector.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMatjar.WebProtector
{
    public class AspnetIdentityUserRepository : IProtectedUserRepository
    {
        private ProtectorDbContext _dbContext;

        private ProtectorUserManager _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public AspnetIdentityUserRepository(ILogger logger)
        {
            try
            {
                _dbContext = new ProtectorDbContext();
                _userManager = new ProtectorUserManager(new ProtectorUserStore(_dbContext));
                _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_dbContext));
            }
            catch (Exception ex)
            {
                logger.LogAsync(ex);
                throw;
            }
        }
       
        public async Task<DataResponse<IProtectedUser>> ChangeUserPassword(IProtectedUser user, string newPassword)
        {
            var foundUser = _userManager.FindByEmail(user.Email);
            if (foundUser == null)
            {
                return new DataResponse<IProtectedUser>()
                {
                    Errors = new List<string>() { "User not found" }
                };
            }
            var token = _userManager.GeneratePasswordResetToken(foundUser.Id);
            var result = _userManager.ResetPassword(foundUser.Id, token, newPassword);
            return new DataResponse<IProtectedUser>()
            {
                Errors = result.Errors.ToList()
            };
        }


        public async Task<ProtectedUserResult> UpdateUserAsync(IProtectedUser user)
        {
            try
            {
                _userManager.UserValidator = new UserValidator<ProtectedUser>(_userManager)
                {
                    AllowOnlyAlphanumericUserNames = false,
                    RequireUniqueEmail = true
                };

                //var user1 = _dbContext.Users.Where(c =>c.BusinessUserId==user.BusinessUserId)
                //    .FirstOrDefault();
                var user1 = await _userManager.FindByIdAsync(user.IdentityId);
                //user1.CustomerId = user.CustomerId;
                user1.BusinessRoleId = user.BusinessRoleId;
                user1.BusinessUserId = user.BusinessUserId;
                user1.FirstName = user.FirstName;
                user1.LastName = user.LastName;
                user1.Email = user.Email;
                user1.UserName = user.UserName;
                user1.PhoneNumber = user.PhoneNumber;

                var identityResult = await _userManager.UpdateAsync(user1);
                var result = identityResult.ToProtectedUserResult();

                return result;
            }
            catch (DbEntityValidationException ex)
            {
                var errors = GetAllErrors(ex);
                return new ProtectedUserResult(false, errors);
            }
        }
      

        private static string[] GetAllErrors(DbEntityValidationException ex)
        {
            var errors = new List<string>();
            foreach (var err in ex.EntityValidationErrors)
            {
                foreach (var verror in err.ValidationErrors)
                    errors.Add(verror.PropertyName + ": " + verror.ErrorMessage);
            }
            return errors.ToArray();
        }
        public async Task<ProtectedUserResult> CreateUserAsync(IProtectedUser user, string password)
        {
            try
            {
                _userManager.UserValidator = new UserValidator<ProtectedUser>(_userManager)
                {
                    AllowOnlyAlphanumericUserNames = false,
                    RequireUniqueEmail = true
                };
                var protectorUser = new ProtectedUser
                {
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    JoinDate = DateTime.Now,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    BusinessRoleId = user.BusinessRoleId,
                    BusinessUserId = user.BusinessUserId
                    
                    
                };
               
                var identityResult = await _userManager.CreateAsync(protectorUser, password);
                var loadedUser = _userManager.FindByName(protectorUser.UserName);
                var result = identityResult.ToProtectedUserResult();
                return result;
            }
            catch (DbEntityValidationException ex)
            {
                var errors = GetAllErrors(ex);
                return new ProtectedUserResult(false, errors);
            }
        }
        public async Task<ProtectedRefreshToken> FindRefreshToken(string refreshTokenId)
        {
            var refreshToken = await _dbContext.RefreshTokens.FindAsync(refreshTokenId);

            return refreshToken;
        }

        public async Task<bool> AddRefreshTokenAsync(ProtectedRefreshToken token)
        {

            var existingToken = _dbContext.RefreshTokens
                .Where(r => r.UserId == token.UserId)
                .Where(r => r.ClientId == token.ClientId)
                .FirstOrDefault();

            if (existingToken != null)
            {
                var result = await RemoveRefreshToken(existingToken);
            }

            _dbContext.RefreshTokens.Add(token);

            return await _dbContext.SaveChangesAsync() > 0;
        }
        public async Task<bool> RemoveRefreshToken(string refreshTokenId)
        {
            var refreshToken = await _dbContext.RefreshTokens.FindAsync(refreshTokenId);

            if (refreshToken != null)
            {
                _dbContext.RefreshTokens.Remove(refreshToken);
                return await _dbContext.SaveChangesAsync() > 0;
            }

            return false;
        }

        public async Task<bool> RemoveRefreshToken(ProtectedRefreshToken refreshToken)
        {
            _dbContext.RefreshTokens.Remove(refreshToken);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public List<ProtectedRefreshToken> GetAllRefreshTokens()
        {
            return _dbContext.RefreshTokens.ToList();
        }
        public async Task<IProtectedUser> FindUserByUserName(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            return user;
        }
        //FindUserById method not used any where till now
        public async Task<IProtectedUser> FindUserById(string id)
        {
             var user = await _userManager.FindByIdAsync( id);
            //var user = _dbContext.Users.Where(c => Guid.Parse(c.Id)  == Guid.Parse(id)).FirstOrDefault();
            if (user == null)
                return null;
            // return user;
            return new ProtectedUser()
            {
                IdentityId = user.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                FirstName = user.FirstName
            };
        }
        public async Task<IProtectedUser> FindUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user;
        }

        public async Task<IProtectedUser> FindUserAsync(string userName, string password)
        {

            var user = await _userManager.FindAsync(userName, password);
            return user;
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            _userManager.Dispose();

        }

        public async Task<ProtectedClient> FindClientAsync(string clientId)
        {
            var client = _dbContext.Clients.FirstOrDefault(item => item.Id == clientId);
            return client;
        }

        public async Task<ProtectedUserResult> CreateUserAsync(IProtectedUser user, string password, string[] roles)
        {
            var result = await CreateUserAsync(user, password);
            if (result.Succeeded)
            {
                var usr = _userManager.FindByName(user.UserName);

                foreach (var r in roles)
                {
                    var foundRole = _roleManager.FindByName(r);
                    if (foundRole == null)
                    {
                        foundRole = new IdentityRole()
                        {
                            Name = r
                        };
                        await _roleManager.CreateAsync(foundRole);
                    }
                    _userManager.AddToRole(usr.Id, r);
                }
            }
            return result;
        }
        // sholkany comment this method to modify it 13-03-2018
        //public async Task DeleteUser(string username)
        //{
        //    var user = await _userManager.FindByIdAsync(username);
        //    if (user != null)
        //        await _userManager.DeleteAsync(user);
        //}
        public async Task DeleteUser(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
           
            if (user != null)
                await _userManager.DeleteAsync(user);
        }
        public async Task<IList<IProtectedUser>> GetUsersByRole(string roleName)
        {
            var r = _dbContext.Roles
                .FirstOrDefault(item => item.Name == roleName);
            if (r == null)
                return new List<IProtectedUser>();

            return _dbContext.Database.SqlQuery<ProtectedUser>(
                $"select * from AspNetUsers where Id in (select UserId from AspNetUserRoles where RoleId='{r.Id}')")
                    .ToList()
                    .Select(item => item as IProtectedUser)
                    .ToList();


        }

      
    }
}
