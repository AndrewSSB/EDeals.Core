using EDeals.Core.Application.Interfaces;
using EDeals.Core.Application.Interfaces.Mediator;
using EDeals.Core.Domain.Common.GenericResponses.ServiceResponse;

namespace EDeals.Core.Application.Authentication.Commands.ConfirmEmail
{
    public class ConfirmEmailCommandHandler : IMediatRCommandHandler<ConfirmEmailCommand>
    {
        private readonly IIdentityBaseRepository _identityBase;

        public ConfirmEmailCommandHandler(IIdentityBaseRepository identityBase)
        {
            _identityBase = identityBase;
        }

        public async Task<ResultResponse> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken) =>
            await _identityBase.ConfirmUserEmail(request.Token);
    }
}
