using Microsoft.EntityFrameworkCore;
using TaskManagement.Api.Data;
using TaskManagement.Api.DTOs.Auth;
using TaskManagement.Api.Entities;
using TaskManagement.Api.Security;
using TaskManagement.Api.Services.Interfaces;
using TaskManagement.Services.Interfaces;

namespace TaskManagement.Api.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthService(AppDbContext context, IJwtTokenService jwtTokenService)
        {
            _context = context;
            _jwtTokenService = jwtTokenService;
        }

        public async Task SignupAsync(SignupRequestDto request)
        {
            // Check duplicates
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email || u.Username == request.Username);

            if (existingUser != null)
            {
                throw new ApplicationException("Username or Email already exists.");
            }

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                HashedPassword = PasswordHasher.HashPassword(request.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                throw new ApplicationException("Invalid email or password.");
            }

            var isPasswordValid = PasswordHasher.VerifyPassword(user.HashedPassword, request.Password);
            if (!isPasswordValid)
            {
                throw new ApplicationException("Invalid email or password.");
            }

            var token = _jwtTokenService.GenerateToken(user);

            return new LoginResponseDto
            {
                Token = token,
                Username = user.Username,
                Email = user.Email
            };
        }
    }
}
