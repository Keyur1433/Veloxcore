using APZMS.Application.DTOs;
using APZMS.Domain.Models;
using APZMS.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using APZMS.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace APZMS.Infrastructure.Services
{
    public class UserService: IUserService
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IUserRepository _userRepository;

        public UserService(AppDbContext context, IPasswordHasher<User> passwordHasher, IUserRepository userRepository)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
        }

        public async Task<User> RegisterUserAsync(UserRegistrationDto dto)
        {
            if (await EmailExistsAsync(dto.Email!))
            {
                throw new InvalidOperationException("Email already exists");
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

            //_context.Users.Add(user);
            await _userRepository.AddAsync(user);
            //await _context.SaveChangesAsync();
            await _userRepository.SaveChangesAsync();
            return user;
        }


        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }
    }
}
