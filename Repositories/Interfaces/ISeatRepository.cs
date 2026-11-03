
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces
{
    public interface ISeatRepository
    {
        Task<List<Seat>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            int? screenId);

        Task<Seat?> GetByIdAsync(int id);
        Task<int> GetTotalSeatsAsync(int? screenId);

        Task<Seat> AddAsync(Seat seat);

        Task UpdateAsync(Seat seat);

        Task<bool> ExistsAsync(int id);
    }
}
