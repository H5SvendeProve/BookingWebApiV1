using BookingWebApiV1.Api.RequestDTOs;
using BookingWebApiV1.Exceptions;
using BookingWebApiV1.Services.LoginService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UnauthorizedAccessException = BookingWebApiV1.Exceptions.UnauthorizedAccessException;

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
            string token = await LoginService.LoginUser(user);
            
            if (token.Length < 1)
            {
                throw new BadRequestException("error happened when generating a jwt token");
            }
            
            return Ok(new {token});
        }

        [AllowAnonymous]
        [HttpPost("registerNewUser")]
        public async Task<IActionResult> RegisterNewUser([FromBody] CreateNewUserRequest userRequest)
        {
            bool userCreated = await LoginService.RegisterNewUser(userRequest);

            if (!userCreated)
            {
                throw new BadRequestException($"the user {userRequest.Username} has not been created");
            }

            return Created("result", userCreated.ToString());
        }
    }
}