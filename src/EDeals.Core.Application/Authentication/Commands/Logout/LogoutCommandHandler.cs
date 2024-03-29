﻿using EDeals.Core.Application.Interfaces.IIdentityRepository;
using EDeals.Core.Application.Interfaces.Mediator;
using EDeals.Core.Domain.Common.GenericResponses.ServiceResponse;

namespace EDeals.Core.Application.Authentication.Commands.Logout
{
    public class LogoutCommandHandler : IMediatRCommandHandler<LogoutCommand>
    {
        private readonly IIdentityRepository _identityBase;

        public LogoutCommandHandler(IIdentityRepository identityBase)
        {
            _identityBase = identityBase;
        }

        public async Task<ResultResponse> Handle(LogoutCommand request, CancellationToken cancellationToken) =>
            await _identityBase.SignOutUserAsync();
    }
}
