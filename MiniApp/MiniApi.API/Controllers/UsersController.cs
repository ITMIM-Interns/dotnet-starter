using MediatR;
using Microsoft.AspNetCore.Mvc;
using MiniApp.BLL.Abstractions.Externals;
using MiniApp.BLL.Features.Commands.Users.Create;
using MiniApp.BLL.Features.Commands.Users.Delete;
using MiniApp.BLL.Features.Commands.Users.Update;
using MiniApp.BLL.Features.Queries.Users.GetById;
using MiniApp.DTOs.Users;

namespace MiniApp.API.Controllers
{
    public sealed class UsersController : BaseController
    {

        public UsersController(IMediator mediator, IEmailService emailService) : base(mediator) { }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<UserDto>> UserDetailById([FromRoute] Guid id)
        {
            UserDto response = await _mediator.Send(new GetUserDetailByIdQuery(id));
            return Ok(response);
        }
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete([FromRoute] Guid id)
        {
            await _mediator.Send(new DeleteUserCommand(id));
            return NoContent();
        }
        [HttpPost]
        public async Task<ActionResult> Create([FromForm] CreateUserCommand request)
        {
            Guid newId = await _mediator.Send(request);
            return CreatedAtAction(nameof(Create), UserDetailById, newId);
        }
        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update([FromForm] UpdateUserCommand request, [FromRoute] Guid id)
        {
            if (id != request.Id)
                return BadRequest();
            await _mediator.Send(request);
            return Ok();
        }
        
    }
}
