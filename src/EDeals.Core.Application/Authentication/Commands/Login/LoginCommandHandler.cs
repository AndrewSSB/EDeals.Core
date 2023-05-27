using EDeals.Core.Application.Interfaces.IIdentityRepository;
using EDeals.Core.Application.Interfaces.Mediator;
using EDeals.Core.Application.Models.Authentication.Login;
using EDeals.Core.Domain.Common.GenericResponses.ServiceResponse;

namespace EDeals.Core.Application.Authentication.Commands.Login
{
    public class LoginCommandHandler : IMediatRCommandHandler<LoginCommand, LoginResponse>
    {
        private readonly IIdentityRepository _identityBase;

        public LoginCommandHandler(IIdentityRepository identityBase)
        {
            _identityBase = identityBase;
        }

        public async Task<ResultResponse<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken) =>
            await _identityBase.SignInUserAsync(request.Password, email: request.Email, userName: request.UserName);
    }
}
