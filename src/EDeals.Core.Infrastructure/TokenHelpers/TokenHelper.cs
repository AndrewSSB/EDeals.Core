using EDeals.Core.Infrastructure.Identity.Auth;
using EDeals.Core.Infrastructure.Settings;
using EDeals.Core.Infrastructure.Shared.TokenExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace EDeals.Core.Infrastructure.TokenHelpers
{
    public class TokenHelper : ITokenHelper
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtSettings _settings;

        public TokenHelper(IOptions<JwtSettings> settings, UserManager<ApplicationUser> userManager)
        {
            _settings = settings.Value;
            _userManager = userManager;
        }

        public string CreateToken(List<Claim> claims) => TokenManager.CreateSecurityToken(_settings, claims);

        public async Task<string?> SetAuthenticationToken(ApplicationUser user, string loginProvider, string name)
        {
            if (user == null || string.IsNullOrEmpty(name)) { return null; }

            var newToken = await _userManager.GenerateUserTokenAsync(user, loginProvider, name);

            if (newToken == null) return null;

            var setAuthToken = await _userManager.SetAuthenticationTokenAsync(user, loginProvider, name, newToken);

            if (!setAuthToken.Succeeded) return null;

            return newToken;
        }
    }
}
