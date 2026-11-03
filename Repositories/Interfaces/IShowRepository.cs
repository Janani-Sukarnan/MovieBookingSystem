
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces
{
    public interface IShowRepository
    {
        Task<List<Show>> GetPagedAsync(
            int pageNumber, int pageSize, int? movieId, int? screenId);

        Task<Show?> GetByIdAsync(int id);

        Task<Show> AddAsync(Show show);
        Task<int> GetTotalShowsAsync(int? movieId, int? screenId);

        Task UpdateAsync(Show show);

        Task<bool> ExistsAsync(int id);

        Task<bool> HasOverlappingShowAsync(
            int screenId,
            DateTime startTime,
            DateTime endTime,
            int? excludeShowId = null);

        Task<List<Seat>> GetSeatsByScreenIdAsync(int screenId);

        Task AddShowSeatsAsync(List<ShowSeat> showSeats);
    }
}
