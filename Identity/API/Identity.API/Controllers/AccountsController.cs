using Identity.BLL.Abstractions.Internals.Services;
using Identity.DTO.Accounts;
using Identity.DTO.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    public class AccountsController : BaseController
    {
        private readonly IAccountService _accountService;
       
        public AccountsController(IAccountService accountService) => _accountService = accountService;

        [HttpPut("{id:guid}/confirm-email")]
        public async Task<ActionResult> ConfirmEmail(ConfirmEmailDto request)=> Ok(await _accountService.ConfirmEmail(request));
    
        [Authorize]
        [HttpPatch("{userId:guid}/active")]
        public async Task<ActionResult<bool>> ActivateUser([FromRoute] Guid userId)=> Ok(await _accountService.UserActive(userId));
      
        [Authorize]
        [HttpPatch("{userId:guid}/deactive")]
        public async Task<ActionResult<bool>> DeactivateUser([FromRoute] Guid userId)=> Ok(await _accountService.UserDeactive(userId));

        [HttpPost("email-verification-code")]
        public async Task<ActionResult<Guid>> SendEmailVerificationCode([FromQuery] string email) => Ok(await _accountService.SendEmailVerificationCode(email));
        [HttpPost("register")]
        public async Task<ActionResult<Guid>> Register([FromForm] CreateUserDto request) => Ok(await _accountService.Register(request));

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto request)=> Ok(await _accountService.LoginAsync(request));

        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult<bool>> Logout() => Ok(await _accountService.LogoutAsync());
        [HttpPost("forget-password")]
        public async Task<ActionResult> ForgetPassword([FromBody] ForgetPasswordDto dto) => Ok(await _accountService.ForgetPassword(dto));
        
    }
}
