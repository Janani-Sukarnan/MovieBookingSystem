
using Microsoft.AspNetCore.Identity;
using MovieBookingSystem.DTOs.Auth;
using MovieBookingSystem.Models;
using MovieBookingSystem.Services.Interfaces;
using MovieTicketBooking.Repositories.Interfaces;

namespace MovieBookingSystem.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IJwtService _jwtService;

        public UserService(
            IUserRepository userRepository,
            IPasswordHasher<User> passwordHasher,
            IJwtService jwtService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
        }

        public async Task<(bool Success, string Message)>
            RegisterAsync(RegisterDto dto)
        {
            string email = dto.Email.Trim().ToLowerInvariant();

            var existingUser =
                await _userRepository.GetUserByEmailAsync(email);

            if (existingUser != null)
            {
                return (false, "Email is already registered.");
            }

            var user = new User
            {
                Name = dto.Name.Trim(),
                Email = email,
                Role = "Customer"
            };

            user.PasswordHash = _passwordHasher.HashPassword(
                user,
                dto.Password
            );

            await _userRepository.AddUserAsync(user);
            await _userRepository.SaveChangesAsync();

            return (true, "Registration successful.");
        }

        public async Task<(bool Success, string Message, string? Token)>
            LoginAsync(LoginDto dto)
        {
            string email = dto.Email.Trim().ToLowerInvariant();

            var user =
                await _userRepository.GetUserByEmailAsync(email);

            if (user == null)
            {
                return (false, "Invalid email or password.", null);
            }

            var result = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                dto.Password
            );

            if (result == PasswordVerificationResult.Failed)
            {
                return (false, "Invalid email or password.", null);
            }

            string token = _jwtService.GenerateToken(user);

            return (true, "Login successful.", token);
        }
    }
}
