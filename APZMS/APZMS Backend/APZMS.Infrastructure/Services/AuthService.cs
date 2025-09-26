using APZMS.Application.DTOs;
using APZMS.Domain.Models;
using APZMS.Application.Interfaces;
using APZMS.Application.Common;
using Microsoft.AspNetCore.Identity;
using APZMS.Infrastructure.Repositories.Interfaces;

namespace APZMS.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserRepository _userRepository;

        public AuthService(IPasswordHasher<User> passwordHasher, IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
        {
            //_context = context;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
            _userRepository = userRepository;
        }

        public async Task<ServiceResult<LoginResponseDto>> LoginAsync(UserLoginDto dto)
        {
            //var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            var user = await _userRepository.GetByEmailAsync(dto.Email);

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
