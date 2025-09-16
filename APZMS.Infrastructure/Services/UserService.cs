using APZMS.Application.DTOs;
using APZMS.Application.Interfaces;
using APZMS.Domain.Exceptions;
using APZMS.Domain.Models;
using APZMS.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace APZMS.Infrastructure.Services
{
    public class UserService: IUserService
    {
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IUserRepository _userRepository;

        public UserService(IPasswordHasher<User> passwordHasher, IUserRepository userRepository)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
        }

        public async Task<User> RegisterUserAsync(UserRegistrationDto dto)
        {
            if (await _userRepository.GetByEmailAsync(dto.Email) != null)
            {
                throw new EmailAlreadyExistsException(dto.Email);
            }

            var user = new User
            {
                Email = dto.Email,
                Name = dto.Name,
                Phone = dto.Phone,
                Role = "customer",
                BirthDate = dto.BirthDate,
            };
 
            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password!);

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();
            return user;
        }
    }
}
