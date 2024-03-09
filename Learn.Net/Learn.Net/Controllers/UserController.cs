using Learn.Net.Modal;
using Learn.Net.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Learn.Net.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        //constructor
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("UserRegistration")]
        public async Task<IActionResult> UserRegistration(UserRegister userRegister)
        {
            var response = await _userService.UserRegistration(userRegister);
            return Ok(response);
        }
        [HttpPost("ConfirmRegistration")]
        public async Task<IActionResult> ConfirmRegistration(int userId, string username, string outputText)
        {
            var response = await _userService.ConfirmRegistration(userId, username, outputText);
            return Ok(response);
        }

    }
}
