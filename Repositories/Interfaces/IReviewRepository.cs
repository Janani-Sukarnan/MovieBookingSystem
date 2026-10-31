using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces
{
    public interface IReviewRepository
    {
        Task<List<Review>> GetAllAsync();
        Task<Review?> GetByIdAsync(int id);
        Task<List<Review>> GetByMovieIdAsync(int movieId);
        Task<Review> AddAsync(Review review);
        Task UpdateAsync(Review review);
        Task DeleteAsync(Review review);
        Task<bool> MovieExistsAsync(int movieId);
        Task<double> GetAverageRatingAsync(int movieId);
        Task<int> GetReviewCountAsync(int movieId);
    }
}