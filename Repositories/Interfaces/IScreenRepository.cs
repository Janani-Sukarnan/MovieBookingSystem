
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces
{
    public interface IScreenRepository
    {
        Task<List<Screen>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            int? theatreId);

        Task<Screen?> GetByIdAsync(int id);

        Task<Screen> AddAsync(Screen screen);
        Task<int> GetTotalScreensAsync(int? theatreId);

        Task UpdateAsync(Screen screen);

        Task<bool> ExistsAsync(int id);
    }
}
