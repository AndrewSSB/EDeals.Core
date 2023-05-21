using EDeals.Core.Infrastructure.Identity.Repository;
using MediatR;

namespace EDeals.Core.Application.Authentication.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, string?>
    {
        private readonly IIdentityBaseRepository _identityBase;

        public LoginCommandHandler(IIdentityBaseRepository identityBase)
        {
            _identityBase = identityBase;
        }

        public async Task<string?> Handle(LoginCommand request, CancellationToken cancellationToken) =>
            await _identityBase.SignInUserAsync(request.Password, email: request.Email, userName: request.UserName);
    }
}
