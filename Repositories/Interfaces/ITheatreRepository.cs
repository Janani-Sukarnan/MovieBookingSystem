
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces
{
    public interface ITheatreRepository
    {
        Task<List<Theatre>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? name,
            string? city);

        Task<Theatre?> GetByIdAsync(int id);

        Task<Theatre> AddAsync(Theatre theatre);

        Task UpdateAsync(Theatre theatre);

        Task DeleteAsync(Theatre theatre);

        Task<bool> ExistsAsync(int id);
    }
}
