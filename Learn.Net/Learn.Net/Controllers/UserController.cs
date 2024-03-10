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
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(string username, string oldPassword, string newPassword)
        {
            var response = await _userService.ResetPassword(username, oldPassword,newPassword);
            return Ok(response);
        }

        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(string username)
        {
            var response = await _userService.ForgetPassword(username);
            return Ok(response);
        }

        [HttpPost("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword(string username, string password, string optText)
        {
            var response = await _userService.UpdatePassword(username, password, optText);
            return Ok(response);
        }

        [HttpPost("UpdateStatus")]
        public async Task<IActionResult> UpdateStatus(string username, bool status)
        {
            var response = await _userService.UpdateStatus(username, status);
            return Ok(response);
        }

        [HttpPost("UpdateRole")]
        public async Task<IActionResult> UpdateRole(string username, string role)
        {
            var response = await _userService.UpdateRole(username, role);
            return Ok(response);
        }
    }
}
