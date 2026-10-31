
using MovieTicketBooking.DTOs.Screens;

namespace MovieTicketBooking.Services.Interfaces
{
    public interface IScreenService
    {
        Task<List<ScreenResponse>> GetAllAsync(
            int pageNumber, int pageSize, int? theatreId);

        Task<ScreenResponse?> GetByIdAsync(int id);

        Task<ScreenResponse> CreateAsync(CreateScreenRequest request);

        Task<ScreenResponse?> UpdateAsync(
            int id, UpdateScreenRequest request);
    }
}
