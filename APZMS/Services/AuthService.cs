using APZMS.Data;
using APZMS.DTOs;
using APZMS.Models;
using APZMS.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace APZMS.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(AppDbContext context, IPasswordHasher<User> passwordHasher, IJwtTokenGenerator jwtTokenGenerator)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<ServiceResult<LoginResponseDto>> LoginAsync(UserLoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null || _passwordHasher.VerifyHashedPassword(user, user.PasswordHash!, dto.Password) == PasswordVerificationResult.Failed)
            {
                return ServiceResult<LoginResponseDto>.Fail("Invalid credentials");
            }



            var token = _jwtTokenGenerator.GenerateToken(user);

            var response = new LoginResponseDto
            {
                CustomerId = user.Id,
                CustomerName = user.Name!,
                AgeGroup = AgeGroupHelper.GetAgeGroup(user.BirthDate),
                AccessToken = token,
                Role = user.Role!.ToLower()
            };

            return ServiceResult<LoginResponseDto>.Ok(response);
        }
    }
}
