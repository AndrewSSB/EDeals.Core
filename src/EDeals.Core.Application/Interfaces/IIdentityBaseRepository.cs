using EDeals.Core.Domain.Common.GenericResponses.ServiceResponse;
using EDeals.Core.Domain.Models.Authentiation.Login;
using EDeals.Core.Domain.Models.Authentiation.Register;

namespace EDeals.Core.Application.Interfaces
{
    public interface IIdentityBaseRepository
    {
        Task<ResultResponse> GetUserNameAsync(Guid userId);

        Task<ResultResponse> GetUserEmailAsync(Guid userId);

        Task<ResultResponse> GetUserPhoneNumberAsync(Guid userId);

        Task<bool> IsInRoleAsync(Guid userId, string role);

        Task<ResultResponse<RegisterResponse>> CreateUserAsync(string firstName, string lastName, string userName, string email, string phoneNumber, string password);

        Task<ResultResponse<LoginResponse>> SignInUserAsync(string password, Guid? userId = null, string? email = null, string? userName = null);

        Task<ResultResponse> SignOutUserAsync();

        Task<ResultResponse> DeleteUserAsync(Guid? userId, string? email, string? userName);

        Task<ResultResponse> ConfirmUserEmail(string token);

        Task<ResultResponse> ConfirmUserPhone(string digitCode);

        Task<ResultResponse> SendPhoneCode();

        Task<ResultResponse> SendEmailToken();
    }
}
