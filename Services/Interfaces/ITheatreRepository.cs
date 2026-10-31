using MovieTicketBooking.DTOs.Theatres;


namespace MovieTicketBooking.Services.Interfaces
{
    public interface ITheatreService
    {
        Task<List<TheatreResponse>> GetAllAsync(
            int pageNumber, int pageSize, string? name, string? city);

        Task<TheatreResponse?> GetByIdAsync(int id);

        Task<TheatreResponse> CreateAsync(CreateTheatreRequest request);

        Task<TheatreResponse?> UpdateAsync(int id, UpdateTheatreRequest request);

        Task<bool> DeleteAsync(int id);
    }
}
