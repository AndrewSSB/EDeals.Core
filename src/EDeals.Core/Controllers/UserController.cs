using EDeals.Core.API.Extensions;
using EDeals.Core.Application.Authentication.Commands.Register;
using EDeals.Core.Application.Authentication.Commands.UserInfo;
using EDeals.Core.Domain.Models.Authentiation.Register;
using EDeals.Core.Domain.Models.User;
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

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("info")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<UserInfoResponse>> Register()
        {
            var command = new UserInfoQuery();

            return ControllerExtension.Map(await _mediator.Send(command));
        }
    }
}
