using EDeals.Core.Application.Interfaces.IIdentityRepository;
using EDeals.Core.Application.Interfaces.Mediator;
using EDeals.Core.Application.Models.Authentication.Register;
using EDeals.Core.Domain.Common.GenericResponses.ServiceResponse;

namespace EDeals.Core.Application.Authentication.Commands.Register
{
    public class RegisterCommandHandler : IMediatRCommandHandler<RegisterCommand, RegisterResponse>
    {
        private readonly IIdentityRepository _identityBase;

        public RegisterCommandHandler(IIdentityRepository identityBase)
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
