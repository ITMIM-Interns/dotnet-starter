using Identity.BLL.Abstractions.Internals.Services;
using Identity.DTO.Accounts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    public class AccountsController : BaseController
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [HttpPut("{id:guid}/confirm-email")]
        public async Task<ActionResult> ConfirmEmail(ConfirmEmailDto request)
        {
            await _accountService.ConfirmEmail(request);
            return Ok();
        }
        [HttpPatch("{userId:guid}/active")]
        public async Task<ActionResult<bool>> ActivateUser([FromRoute] Guid userId)
        {
            bool result = await _accountService.UserActive(userId);
            return Ok(result);
        }
        [HttpPatch("{userId:guid}/deactive")]
        public async Task<ActionResult<bool>> DeactivateUser([FromRoute] Guid userId)
        {
            bool result = await _accountService.UserDeactive(userId);
            return Ok(result);
        }
        [HttpPost("{userId:guid}/verification-code")]
        public async Task<ActionResult<bool>> SendEmailVerificationCode([FromRoute] Guid userId)
        {
            bool result = await _accountService.SendEmailVerificationCode(userId);
            return Ok(result);
        }
    }
}
