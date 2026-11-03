
using MovieTicketBooking.DTOs.Seats;
using MovieTicketBooking.DTOs.Common;
namespace MovieTicketBooking.Services.Interfaces
{
    public interface ISeatService
    {
        Task<PagedResponse<SeatResponse>> GetAllAsync(
    int pageNumber, int pageSize, int? screenId);

        Task<SeatResponse?> GetByIdAsync(int id);

        Task<SeatResponse> CreateAsync(CreateSeatRequest request);

        Task<SeatResponse?> UpdateAsync(int id, UpdateSeatRequest request);
    }
}
