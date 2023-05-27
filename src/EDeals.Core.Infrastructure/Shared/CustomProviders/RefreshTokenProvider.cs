using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;

namespace EDeals.Core.Infrastructure.Shared.CustomProviders
{
    public class RefreshTokenProvider<TUser> : IUserTwoFactorTokenProvider<TUser>
        where TUser : class
    {
        public Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<TUser> manager, TUser user)
        {
            return Task.FromResult(true);
        }

        public async Task<string> GenerateAsync(string purpose, UserManager<TUser> manager, TUser user)
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            var refreshToken = Convert.ToBase64String(randomNumber);


            await manager.SetAuthenticationTokenAsync(user, "RefreshTokenProvider", "RefreshToken", refreshToken);

            return refreshToken;
        }

        public async Task<bool> ValidateAsync(string purpose, string token, UserManager<TUser> manager, TUser user)
        {
            var refreshToken = await manager.GetAuthenticationTokenAsync(user, "RefreshTokenProvider", "RefreshToken");

            return token == refreshToken;
        }
    }
}
