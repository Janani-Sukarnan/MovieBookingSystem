using MovieTicketBooking.Models;
using MovieTicketBooking.DTOs.Common;
namespace MovieTicketBooking.Repositories.Interfaces
{
    public interface IMovieRepository
    {
        Task<List<Movie>> GetAllAsync();

        Task<int> GetTotalMoviesAsync(string? title);

        Task<List<Movie>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? title);

        Task<List<Movie>> SearchByTitleAsync(string title);

        Task<Movie?> GetByIdAsync(int id);

        Task<Movie> AddAsync(Movie movie);

        Task UpdateAsync(Movie movie);

        Task DeleteAsync(Movie movie);

        Task<bool> ExistsAsync(int id);
    }
}