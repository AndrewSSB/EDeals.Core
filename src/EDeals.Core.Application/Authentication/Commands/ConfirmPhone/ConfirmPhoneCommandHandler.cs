using EDeals.Core.Application.Interfaces.IIdentityRepository;
using EDeals.Core.Application.Interfaces.Mediator;
using EDeals.Core.Domain.Common.GenericResponses.ServiceResponse;

namespace EDeals.Core.Application.Authentication.Commands.ConfirmPhone
{
    public class ConfirmPhoneCommandHandler : IMediatRCommandHandler<ConfirmPhoneCommand>
    {
        private readonly IIdentityRepository _identityBase;

        public ConfirmPhoneCommandHandler(IIdentityRepository identityBase)
        {
            _identityBase = identityBase;
        }

        public async Task<ResultResponse> Handle(ConfirmPhoneCommand request, CancellationToken cancellationToken) =>
            await _identityBase.ConfirmUserPhone(request.DigitCode);
    }
}
