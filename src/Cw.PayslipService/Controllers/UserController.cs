using Cw.PayslipService.Dtos;
using Cw.PayslipService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cw.PayslipService.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpGet("authenticate")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult AuthenticateUser(UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var token = _userService.ValidateCredentials(userDto.Username, userDto.Password);

            if (token == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }
            
            return Ok(new { Token = token });
        }
    }
}
