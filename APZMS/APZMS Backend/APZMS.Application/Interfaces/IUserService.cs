using APZMS.Application.DTOs;
using APZMS.Domain.Models;

namespace APZMS.Application.Interfaces
{
    public interface IUserService
    {
        Task<User> RegisterUserAsync(UserRegistrationDto dto);
    }
}