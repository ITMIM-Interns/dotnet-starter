using Identity.BLL.Abstractions.Externals;
using Identity.BLL.Abstractions.Internals.Services;
using Identity.BLL.ServiceImplementation;
using Identity.DTO.Accounts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    public class AccountsController : BaseController
    {
        private readonly IAccountService _accountService;
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;
        public AccountsController(IAccountService accountService, ITokenService tokenService, IUserService userService)
        {
            _accountService = accountService;
            _tokenService = tokenService;
            _userService = userService;
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
        [HttpPost("login")]
        public async Task<IActionResult> Login(Guid id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user is null)
                return Unauthorized();

            string accessToken =
                _tokenService.CreateAccessToken(user);

            return Ok(new
            {
                accessToken
            });
        }
    }
}
