using EDeals.Core.Application.Interfaces;
using EDeals.Core.Application.Interfaces.Mediator;
using EDeals.Core.Domain.Common.GenericResponses.ServiceResponse;
using EDeals.Core.Domain.Models.Authentiation.Login;

namespace EDeals.Core.Application.Authentication.Commands.Login
{
    public class LoginCommandHandler : IMediatRCommandHandler<LoginCommand, LoginResponse>
    {
        private readonly IIdentityBaseRepository _identityBase;

        public LoginCommandHandler(IIdentityBaseRepository identityBase)
        {
            _identityBase = identityBase;
        }

        public async Task<ResultResponse<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken) =>
            await _identityBase.SignInUserAsync(request.Password, email: request.Email, userName: request.UserName);
    }
}
