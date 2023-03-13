using BookingWebApiV1.Api.Requests;
using BookingWebApiV1.Services.LoginService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingWebApiV1.Controllers
{
    [ApiController]
    [Route("api/login")]
    public class LoginController : ControllerBase
    {
        private ILoginService LoginService { get; }


        public LoginController(ILoginService loginService)
        {
            LoginService = loginService;
        }

        [AllowAnonymous]
        [HttpPost("loginUser")]
        public async Task<IActionResult> Login([FromBody] LoginUserRequest user)
        {
            bool isCorrectPassword = await LoginService.LoginUser(user);

            if (!isCorrectPassword)
            {
                return Unauthorized("username or password is incorrect");
            }

            string token = await LoginService.GenerateToken(user.Username);

            return Ok(new { token });
        }

        [AllowAnonymous]
        [HttpPost("registerNewUser")]
        public async Task<IActionResult> RegisterNewUser([FromBody] CreateNewUserRequest userRequest)
        {
            bool userCreated = await LoginService.RegisterNewUser(userRequest);

            if (!userCreated)
            {
                return BadRequest("username already exists.");
            }

            return Created("result", userCreated.ToString());
        }
    }
}