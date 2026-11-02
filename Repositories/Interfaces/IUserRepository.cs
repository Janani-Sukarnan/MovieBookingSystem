using MovieBookingSystem.Models;
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmailAsync(string email);

        Task<User?> GetUserByIdAsync(int userId);

        Task AddUserAsync(User user);

        Task SaveChangesAsync();
    }
}