using EDeals.Core.Application.Interfaces.Mediator;
using EDeals.Core.Application.Interfaces.UserServices;
using EDeals.Core.Domain.Common.GenericResponses.ServiceResponse;

namespace EDeals.Core.Application.User.Commands.DeleteUser
{
    public class DeleteUserCommandHandler : IMediatRCommandHandler<DeleteUserCommand>
    {
        private readonly IUserService _userService;

        public DeleteUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<ResultResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken) =>
            await _userService.DeleteUserAsync();
    }
}
