using Amazon.Runtime.Internal.Auth;
using Identity.BLL.Abstractions.Internals.Services;
using Identity.DTO.Users;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost]
        public async Task<ActionResult> Add([FromForm]CreateUserDto request)
        {
            Guid id=await _userService.Add(request);
            return CreatedAtAction(nameof(Add), GetUserDetail, id);
        }
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<UserDto>> GetUserDetail([FromRoute] Guid id)
        {
            UserDto response=await _userService.GetUserDetailByIdAsync(id);
            return Ok(response);
        }
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete([FromRoute] Guid id)
        {
            await _userService.Remove(id);
            return NoContent();
        }
        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update([FromRoute] Guid id,[FromForm]UpdateUserDto request)
        {
            if(id != request.Id)
                return BadRequest();
            await _userService.Update(request);
            return Ok();
        }

    }
}
