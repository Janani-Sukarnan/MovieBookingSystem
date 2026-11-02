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
        private readonly PasswordHasher<User> _passwordHasher;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<User> RegisterAsync(RegisterDto registerDto)
        {
            var existingUser = await _userRepository
                .GetUserByEmailAsync(registerDto.Email);

            if (existingUser != null)
            {
                throw new Exception("Email already registered.");
            }

            var user = new User
            {
                Name = registerDto.Name,
                Email = registerDto.Email,
                Role = "Customer"
            };

            user.PasswordHash = _passwordHasher.HashPassword(
                user,
                registerDto.Password
            );

            await _userRepository.AddUserAsync(user);
            await _userRepository.SaveChangesAsync();

            return user;
        }

        public async Task<User?> LoginAsync(LoginDto loginDto)
        {
            var user = await _userRepository
                .GetUserByEmailAsync(loginDto.Email);

            if (user == null)
            {
                return null;
            }

            var result = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                loginDto.Password
            );

            if (result == PasswordVerificationResult.Failed)
            {
                return null;
            }

            return user;
        }
    }
}