using Microsoft.Owin.Security.Infrastructure;
using SuperMatjar.WebProtector.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMatjar.WebProtector
{
    public class RefreshTokenProvider : IAuthenticationTokenProvider
    {
        public RefreshTokenProvider(IProtectedUserRepository usersRepo)
        {
            this.UsersRepository = usersRepo;
        }

        IProtectedUserRepository UsersRepository { get; set; }

        public void Create(AuthenticationTokenCreateContext context)
        {
            var task = CreateAsync(context);
            task.Wait();
        }

        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var clientid = context.Ticket.Properties.Dictionary["as:client_id"];

            if (string.IsNullOrEmpty(clientid))
            {
                return;
            }

            var refreshTokenId = Guid.NewGuid().ToString("n");


            var refreshTokenLifeTime = context.OwinContext.Get<string>("as:clientRefreshTokenLifeTime");

            var token = new ProtectedRefreshToken()
            {
                Id = Helpers.SecurityHelper.ComputeHash(refreshTokenId),
                ClientId = clientid,
                UserId = context.Ticket.Identity.Name,
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(Convert.ToDouble(refreshTokenLifeTime)),

            };

            context.Ticket.Properties.IssuedUtc = token.IssuedUtc;
            context.Ticket.Properties.ExpiresUtc = token.ExpiresUtc;

            token.ProtectedTicket = context.SerializeTicket();

            var result = await UsersRepository.AddRefreshTokenAsync(token);

            if (result)
            {
                context.SetToken(refreshTokenId);
            }


        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            string hashedTokenId = Helpers.SecurityHelper.ComputeHash(context.Token);
            var refreshToken = await UsersRepository.FindRefreshToken(hashedTokenId);

            if (refreshToken != null)
            {
                //Get protectedTicket from refreshToken class
                context.DeserializeTicket(refreshToken.ProtectedTicket);
                var result = await UsersRepository.RemoveRefreshToken(hashedTokenId);
            }

        }
    }
}
