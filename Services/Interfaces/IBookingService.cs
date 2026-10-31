
using MovieTicketBooking.DTOs.Bookings;

namespace MovieTicketBooking.Services.Interfaces
{
    public interface IBookingService
    {
        Task<List<BookingResponse>> GetAllAsync(int pageNumber, int pageSize);
        Task<BookingResponse?> GetByIdAsync(int id);
        Task<BookingResponse> CreateAsync(CreateBookingRequest request);
        Task<BookingResponse?> UpdateAsync(int id, UpdateBookingRequest request);
        Task<bool> CancelAsync(int id);
    }
}
