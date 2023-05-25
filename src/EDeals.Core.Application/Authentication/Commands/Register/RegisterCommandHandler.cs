using EDeals.Core.Application.Interfaces;
using EDeals.Core.Application.Interfaces.Mediator;
using EDeals.Core.Domain.Common.GenericResponses.ServiceResponse;
using EDeals.Core.Domain.Models.Authentiation.Register;

namespace EDeals.Core.Application.Authentication.Commands.Register
{
    public class RegisterCommandHandler : IMediatRCommandHandler<RegisterCommand, RegisterResponse>
    {
        private readonly IIdentityBaseRepository _identityBase;

        public RegisterCommandHandler(IIdentityBaseRepository identityBase)
        {
            _identityBase = identityBase;
        }

        public async Task<ResultResponse<RegisterResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
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
