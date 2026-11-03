
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces
{
    public interface IBookingRepository
    {
        Task<List<Booking>> GetPagedAsync(int pageNumber, int pageSize);
        Task<Booking?> GetByIdAsync(int id);

        Task<List<ShowSeat>> GetShowSeatsForBookingAsync(
            int showId, List<int> showSeatIds);
        Task<int> GetTotalBookingsAsync();

        Task<Booking> AddAsync(Booking booking, List<ShowSeat> showSeats);
        Task UpdateAsync(Booking booking);
        Task<bool> ExistsAsync(int id);

        Task CancelAsync(Booking booking);
    }
}
