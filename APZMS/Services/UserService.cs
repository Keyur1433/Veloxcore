using APZMS.Data;
using APZMS.DTOs;
using APZMS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace APZMS.Services
{
    public class UserService: IUserService
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(AppDbContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
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

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }


        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }
    }
}
