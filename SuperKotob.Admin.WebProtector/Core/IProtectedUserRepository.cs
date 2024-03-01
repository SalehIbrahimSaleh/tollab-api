using SuperKotob.Admin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMatjar.WebProtector.Core
{
    public interface IProtectedUserRepository : IDisposable
    {
        Task<DataResponse<IProtectedUser>> ChangeUserPassword(IProtectedUser user, string newPassword);

        Task<ProtectedRefreshToken> FindRefreshToken(string refreshTokenId);
        Task<ProtectedUserResult> UpdateUserAsync(IProtectedUser user);
        Task<ProtectedUserResult> CreateUserAsync(IProtectedUser user, string password);
        Task<ProtectedUserResult> CreateUserAsync(IProtectedUser user, string password, string[] roles);
        //Task<IProtectedUser> FindUserByEmail(string email);
        Task<IProtectedUser> FindUserByUserName(string username);
        Task<IProtectedUser> FindUserAsync(string userName, string password);
        Task<ProtectedClient> FindClientAsync(string clientId);
        Task<bool> AddRefreshTokenAsync(ProtectedRefreshToken token);
        Task<bool> RemoveRefreshToken(string refreshTokenId);
        Task DeleteUser(string username);
        //Task DeleteUser(long? id);
        Task<IList<IProtectedUser>> GetUsersByRole(string roleName);
        Task<IProtectedUser> FindUserById(string id);
      


    }
}
