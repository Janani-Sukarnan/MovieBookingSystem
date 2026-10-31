
using MovieTicketBooking.DTOs.Seats;

namespace MovieTicketBooking.Services.Interfaces
{
    public interface ISeatService
    {
        Task<List<SeatResponse>> GetAllAsync(
            int pageNumber, int pageSize, int? screenId);

        Task<SeatResponse?> GetByIdAsync(int id);

        Task<SeatResponse> CreateAsync(CreateSeatRequest request);

        Task<SeatResponse?> UpdateAsync(int id, UpdateSeatRequest request);
    }
}
