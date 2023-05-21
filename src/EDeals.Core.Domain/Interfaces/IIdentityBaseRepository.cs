namespace EDeals.Core.Infrastructure.Identity.Repository
{
    public interface IIdentityBaseRepository
    {
        Task<string?> GetUserNameAsync(Guid userId);

        Task<string?> GetUserEmailAsync(Guid userId);

        Task<string?> GetUserPhoneNumberAsync(Guid userId);

        Task<bool> IsInRoleAsync(Guid userId, string role);

        // TODO: change the string response with the generic response
        Task<string?> CreateUserAsync(string firstName, string lastName, string userName, string email, string phoneNumber, string password);

        Task<string?> SignInUserAsync(string password, Guid? userId = null, string? email = null, string? userName = null);
        
        Task<string?> SignOutUserAsync(Guid userId);

        // TODO: change the string response with the generic response
        public Task<string?> DeleteUserAsync(Guid? userId, string? email, string? userName);
    }
}
