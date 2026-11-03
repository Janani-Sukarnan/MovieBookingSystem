
using MovieTicketBooking.DTOs.Common;
using MovieTicketBooking.DTOs.Shows;

namespace MovieTicketBooking.Services.Interfaces
{
    public interface IShowService
    {
        Task<PagedResponse<ShowResponse>> GetAllAsync(
    int pageNumber, int pageSize, int? movieId, int? screenId);

        Task<ShowResponse?> GetByIdAsync(int id);

        Task<ShowResponse> CreateAsync(CreateShowRequest request);

        Task<ShowResponse?> UpdateAsync(int id, UpdateShowRequest request);
    }
}

