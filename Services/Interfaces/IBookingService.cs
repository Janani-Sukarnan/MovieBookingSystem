
using MovieTicketBooking.DTOs.Bookings;

namespace MovieTicketBooking.Services.Interfaces
{
    public interface IBookingService
    {
        Task<List<BookingResponse>> GetAllAsync(
            int pageNumber, int pageSize);

        Task<BookingResponse?> GetByIdAsync(
            int id, int userId, bool canManageAll);

        Task<BookingResponse> CreateAsync(
            CreateBookingRequest request, int userId);

        Task<BookingResponse?> UpdateAsync(
            int id, UpdateBookingRequest request,
            int userId, bool canManageAll);

        Task<bool> CancelAsync(
            int id, int userId, bool canManageAll);
    }
}
