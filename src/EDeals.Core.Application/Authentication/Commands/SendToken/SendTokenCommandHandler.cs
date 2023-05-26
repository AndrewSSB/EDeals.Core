using EDeals.Core.Application.Interfaces;
using EDeals.Core.Application.Interfaces.Mediator;
using EDeals.Core.Domain.Common.GenericResponses.ServiceResponse;

namespace EDeals.Core.Application.Authentication.Commands.SendToken
{
    public class SendTokenCommandHandler : IMediatRCommandHandler<SendTokenCommand>
    {
        private readonly IIdentityBaseRepository _identityBase;

        public SendTokenCommandHandler(IIdentityBaseRepository identityBase)
        {
            _identityBase = identityBase;
        }

        public async Task<ResultResponse> Handle(SendTokenCommand request, CancellationToken cancellationToken) =>
            await _identityBase.SendEmailToken();
    }
}
