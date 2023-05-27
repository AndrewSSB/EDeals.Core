using EDeals.Core.Application.Models.Authentication.Login;
using EDeals.Core.Application.Models.Authentication.Register;
using EDeals.Core.Domain.Common.GenericResponses.ServiceResponse;

namespace EDeals.Core.Application.Interfaces.IIdentityRepository
{
    public interface IIdentityRepository
    {
        Task<ResultResponse> GetUserNameAsync(Guid userId);

        Task<ResultResponse> GetUserEmailAsync(Guid userId);

        Task<ResultResponse> GetUserPhoneNumberAsync(Guid userId);

        Task<bool> IsInRoleAsync(Guid userId, string role);

        Task<ResultResponse<RegisterResponse>> CreateUserAsync(string firstName, string lastName, string userName, string email, string phoneNumber, string password);

        Task<ResultResponse<LoginResponse>> SignInUserAsync(string password, string? email = null, string? userName = null);

        Task<ResultResponse> SignOutUserAsync();

        Task<ResultResponse> ConfirmUserEmail(string token);

        Task<ResultResponse> ConfirmUserPhone(string digitCode);

        Task<ResultResponse> SendPhoneCode();

        Task<ResultResponse> SendEmailToken();
    }
}
