using MediatR;
using Microsoft.AspNetCore.Mvc;
using MiniApp.BLL.Features.Commands.Accounts.ConfirmEmail;
using MiniApp.BLL.Features.Commands.Accounts.SendEmailVerificationCode;
using MiniApp.BLL.Features.Commands.Accounts.ToggleUserStatus;
using MiniApp.BLL.Features.Commands.Accounts.UserDeactive;
using MiniApp.DTOs.Accounts;

namespace MiniApp.API.Controllers
{
    public class AccountsController : BaseController
    {
        public AccountsController(IMediator mediator) : base(mediator) { }

        [HttpPut("confirm-email/{userId:guid}")]
        public async Task<ActionResult> EmailConfirm([FromRoute] Guid userId, [FromBody] ConfirmEmailRequest request)
        {
            await _mediator.Send(new ConfirmEmailCommand(userId,request.Code));
            return Ok();
        }
        [HttpPatch("{userId:guid}/active")]
        public async Task<ActionResult> ActivateUser([FromRoute] Guid userId)
        {
            bool result = await _mediator.Send(new UserActiveCommand(userId));
            return Ok(result);
        }
        [HttpPatch("{userId:guid}/deactive")]
        public async Task<ActionResult> DeactivateUser([FromRoute] Guid userId)
        {
            bool result = await _mediator.Send(new UserDeactiveCommand(userId));
            return Ok(result);
        }
        [HttpPost("{userId:guid}/verification-code")]
        public async Task<ActionResult> SendEmailVerificationCode([FromRoute] Guid userId)
        {
            bool result = await _mediator.Send(new SendEmailVerificationCodeCommand(userId));
            return Ok(result);
        }
    }
}
