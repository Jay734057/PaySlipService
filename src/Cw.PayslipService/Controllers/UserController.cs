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
        public IActionResult AuthenticateUser(string userName, string password)
        {
            var token = _userService.ValidateCredentials(userName, password);

            if (token == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }
            
            return Ok(new { Token = token });
        }
    }
}
