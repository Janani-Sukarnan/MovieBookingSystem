using MovieBookingSystem.Models;

namespace MovieBookingSystem.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}