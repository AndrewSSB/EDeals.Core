using EDeals.Core.Infrastructure.Identity.Auth;

namespace EDeals.Core.Infrastructure.Identity.Repository
{
    public interface IIdentityRepository
    {
        Task<string?> GetUserNameAsync(Guid userId);

        Task<string?> GetUserEmailAsync(Guid userId);

        Task<string?> GetUserPhoneNumberAsync(Guid userId);

        /// <summary>
        /// Returns the application user.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="email"></param>
        /// <param name="userName"></param>
        /// <returns> ApplicationUser if there is a user with the specified UserId, Email, UserName </returns>
        Task<ApplicationUser?> FindUser(Guid? userId, string? email, string? userName);

        Task<bool> IsInRoleAsync(Guid userId, string role);

        // TODO: change the string response with the generic response
        Task<string?> CreateUserAsync(string firstName, string lastName, string userName, string email, string phoneNumber, string password);

        Task<string?> SignInUserAsync(Guid? userId, string? email, string? userName, string password);
        
        Task<string?> SignOutUserAsync(Guid userId);

        // TODO: change the string response with the generic response
        public Task<string?> DeleteUserAsync(Guid? userId, string? email, string? userName);
    }
}
