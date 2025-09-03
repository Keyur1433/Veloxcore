using APZMS.DTOs;
using APZMS.Models;

namespace APZMS.Services
{
    public interface IUserService
    {
        Task<User> RegisterUserAsync(UserRegistrationDto dto);
        Task<bool> EmailExistsAsync(string email);
    }
}