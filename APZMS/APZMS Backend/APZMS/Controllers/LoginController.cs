using APZMS.Application.DTOs;
using APZMS.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace APZMS.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class LoginController : ControllerBase
    {
        private readonly IAuthService _authService;

        public LoginController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto dto)
        {
           var result = await _authService.LoginAsync(dto);

            if (!result.Success)
            {
                return StatusCode(401, new
                {
                    message = result.ErrorMessage
                });
            }

            return Ok(result);
        }

    }
}
