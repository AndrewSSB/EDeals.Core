using EDeals.Core.Infrastructure.Identity.Auth;
using System.Security.Claims;

namespace EDeals.Core.Infrastructure.TokenHelpers
{
    public interface ITokenHelper
    {
        string CreateToken(List<Claim> claims);
        Task<string?> SetAuthenticationToken(ApplicationUser user, string loginProvider, string name);
    }
}
