using EDeals.Core.Infrastructure.Identity.Repository;
using MediatR;

namespace EDeals.Core.Application.Authentication.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, string?>
    {
        private readonly IIdentityBaseRepository _identityBase;

        public RegisterCommandHandler(IIdentityBaseRepository identityBase)
        {
            _identityBase = identityBase;
        }

        public async Task<string?> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityBase.CreateUserAsync(
                request.FirstName,
                request.LastName,
                request.UserName,
                request.Email,
                request.PhoneNumber,
                request.Password);

            return result;
        }
    }
}
