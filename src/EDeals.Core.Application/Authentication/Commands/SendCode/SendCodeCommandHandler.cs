using EDeals.Core.Application.Interfaces.IIdentityRepository;
using EDeals.Core.Application.Interfaces.Mediator;
using EDeals.Core.Domain.Common.GenericResponses.ServiceResponse;

namespace EDeals.Core.Application.Authentication.Commands.SendCode
{
    public class SendCodeCommandHandler : IMediatRCommandHandler<SendCodeCommand>
    {
        private readonly IIdentityRepository _identityBase;

        public SendCodeCommandHandler(IIdentityRepository identityBase)
        {
            _identityBase = identityBase;
        }

        public async Task<ResultResponse> Handle(SendCodeCommand request, CancellationToken cancellationToken) =>
            await _identityBase.SendPhoneCode();
    }
}
