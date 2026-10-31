using MovieTicketBooking.DTOs.Reviews;

namespace MovieTicketBooking.Services.Interfaces
{
    public interface IReviewService
    {
        Task<List<ReviewResponse>> GetAllAsync();
        Task<ReviewResponse?> GetByIdAsync(int id);
        Task<List<ReviewResponse>> GetByMovieIdAsync(int movieId);
        Task<ReviewResponse> CreateAsync(CreateReviewRequest request);
        Task<ReviewResponse?> UpdateAsync(int id, UpdateReviewRequest request);
        Task<bool> DeleteAsync(int id);
        Task<object> GetMovieRatingSummaryAsync(int movieId);
    }
}