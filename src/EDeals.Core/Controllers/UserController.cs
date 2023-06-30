using EDeals.Core.API.Extensions;
using EDeals.Core.Application.Interfaces.UserServices;
using EDeals.Core.Application.Models.UserProfile;
using EDeals.Core.Application.User.Commands.DeleteUser;
using EDeals.Core.Application.User.Queries.UserInfo;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EDeals.Core.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IUserService _userService;

        public UserController(IMediator mediator, IUserService userService)
        {
            _mediator = mediator;
            _userService = userService;
        }

        [HttpGet("info")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<UserInfoResponse>> GetUserInfo()
        {
            var command = new UserInfoQuery();

            return ControllerExtension.Map(await _mediator.Send(command));
        }

        [HttpDelete("account")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<UserInfoResponse>> DeleteUser()
        {
            var command = new DeleteUserCommand();

            return ControllerExtension.Map(await _mediator.Send(command));
        }

        [HttpGet("all")]
        [Produces("application/json")]
        public async Task<ActionResult<List<UserInfoResponse>>> GetUsers([FromQuery] string? userName)
        {
            return ControllerExtension.Map(await _userService.GetUsers(userName));
        }
    }
}
