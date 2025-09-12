using APZMS.Application.DTOs;
using APZMS.Application.Interfaces;
using APZMS.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace APZMS.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegistrationDto dto)
        {
            try
            {
                var user = await _userService.RegisterUserAsync(dto);

                return StatusCode(201, new
                {
                    customerId = user.Id,
                    customerName = user.Name,
                    ageGroup = AgeGroupHelper.GetAgeGroup(dto.BirthDate),
                    message = "User registered successfully."
                });
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("Email already exists"))
            {
                return StatusCode(409, new
                {
                    message = "User with this email already exists."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An unexpected error occurred.",
                    details = ex.Message
                });
            }
        }
    }
}