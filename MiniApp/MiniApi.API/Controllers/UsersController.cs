using MediatR;
using Microsoft.AspNetCore.Mvc;
using MiniApp.BLL.Features.Commands.Users.Delete;
using MiniApp.BLL.Features.Queries.Users.GetById;
using MiniApp.DTOs.Users;

namespace MiniApp.API.Controllers
{
    public sealed class UsersController : BaseController
    {
        public UsersController(IMediator mediator) : base(mediator) { }



        [HttpGet("{id:guid}")]
        public async Task<ActionResult<UserDto>> GetById([FromRoute] Guid id)
        {
            UserDto response = await _mediator.Send(new GetByIdUserQuery(id));
            return Ok(response);
        }
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete([FromRoute] Guid id)
        {
            await _mediator.Send(new DeleteUserCommand(id));
           return NoContent();
        }
    }
}
