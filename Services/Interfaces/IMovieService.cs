using MovieTicketBooking.DTOs.Movies;
using MovieTicketBooking.DTOs.Common;
namespace MovieTicketBooking.Services.Interfaces
{
    public interface IMovieService
    {
        Task<PagedResponse<MovieResponse>> GetAllAsync(
    int pageNumber, int pageSize, string? title);

        Task<MovieResponse?> GetByIdAsync(int id);

        Task<MovieResponse> CreateAsync(CreateMovieRequest request);

        Task<MovieResponse?> UpdateAsync(
            int id, UpdateMovieRequest request);

        Task<bool> DeleteAsync(int id);

        Task<List<MovieResponse>> SearchByTitleAsync(string title);
    }
}