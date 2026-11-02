using MovieBookingSystem.DTOs.Auth;
using MovieBookingSystem.Models;

namespace MovieBookingSystem.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> RegisterAsync(RegisterDto registerDto);

        Task<User?> LoginAsync(LoginDto loginDto);
    }
}