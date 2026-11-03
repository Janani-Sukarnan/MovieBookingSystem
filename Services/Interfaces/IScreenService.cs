
using MovieTicketBooking.DTOs.Screens;
using MovieTicketBooking.DTOs.Common;
namespace MovieTicketBooking.Services.Interfaces
{
    public interface IScreenService
    {
        Task<PagedResponse<ScreenResponse>> GetAllAsync(
    int pageNumber, int pageSize, int? theatreId);

        Task<ScreenResponse?> GetByIdAsync(int id);

        Task<ScreenResponse> CreateAsync(CreateScreenRequest request);

        Task<ScreenResponse?> UpdateAsync(
            int id, UpdateScreenRequest request);
    }
}
