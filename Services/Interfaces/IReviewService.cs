using MovieTicketBooking.DTOs.Reviews;
using MovieTicketBooking.DTOs.Common;
namespace MovieTicketBooking.Services.Interfaces
{
    public interface IReviewService
    {
        //Task<List<ReviewResponse>> GetAllAsync();
        Task<ReviewResponse?> GetByIdAsync(int id);
        Task<PagedResponse<ReviewResponse>> GetAllAsync(
    int pageNumber, int pageSize);

        Task<PagedResponse<ReviewResponse>> GetByMovieIdAsync(
            int movieId, int pageNumber, int pageSize);
        //Task<List<ReviewResponse>> GetByMovieIdAsync(int movieId);
        Task<ReviewResponse> CreateAsync(CreateReviewRequest request);
        Task<ReviewResponse?> UpdateAsync(int id, UpdateReviewRequest request);
        Task<bool> DeleteAsync(int id);
        Task<object> GetMovieRatingSummaryAsync(int movieId);
    }
}