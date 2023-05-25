using EDeals.Core.Application.Interfaces;
using EDeals.Core.Infrastructure.Identity.Auth;

namespace EDeals.Core.Infrastructure.Identity.Repository
{
    public interface IIdentityRepository : IIdentityBaseRepository
    {
        /// <summary>
        /// Returns the application user.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="email"></param>
        /// <param name="userName"></param>
        /// <returns> ApplicationUser if there is a user with the specified UserId, Email, UserName </returns>
        Task<ApplicationUser?> FindUser(Guid? userId, string? email, string? userName);
    }
}
