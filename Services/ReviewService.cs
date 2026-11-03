using MovieTicketBooking.DTOs.Reviews;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;
using MovieTicketBooking.DTOs.Common;

namespace MovieTicketBooking.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewService(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<PagedResponse<ReviewResponse>> GetAllAsync(
    int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
                throw new ArgumentException("Invalid pagination values.");

            var totalRecords =
                await _reviewRepository.GetTotalReviewsAsync(null);

            var reviews = await _reviewRepository.GetPagedAsync(
                pageNumber, pageSize, null);

            var totalPages =
                (int)Math.Ceiling((double)totalRecords / pageSize);

            return new PagedResponse<ReviewResponse>
            {
                Items = reviews.Select(MapToResponse).ToList(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = totalPages
            };
        }

        public async Task<ReviewResponse?> GetByIdAsync(int id)
        {
            var review = await _reviewRepository.GetByIdAsync(id);
            return review == null ? null : MapToResponse(review);
        }


        public async Task<PagedResponse<ReviewResponse>> GetByMovieIdAsync(
    int movieId, int pageNumber, int pageSize)
        {
            if (movieId <= 0)
                throw new ArgumentException("Invalid MovieId.");

            if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
                throw new ArgumentException("Invalid pagination values.");

            if (!await _reviewRepository.MovieExistsAsync(movieId))
                throw new ArgumentException("Movie not found.");

            var totalRecords =
                await _reviewRepository.GetTotalReviewsAsync(movieId);

            var reviews = await _reviewRepository.GetPagedAsync(
                pageNumber, pageSize, movieId);

            var totalPages =
                (int)Math.Ceiling((double)totalRecords / pageSize);

            return new PagedResponse<ReviewResponse>
            {
                Items = reviews.Select(MapToResponse).ToList(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = totalPages
            };
        }

        public async Task<ReviewResponse> CreateAsync(CreateReviewRequest request)
        {
            ValidateReview(request.CustomerName, request.Rating, request.Comment);

            if (request.MovieId <= 0)
                throw new ArgumentException("Invalid MovieId.");

            if (!await _reviewRepository.MovieExistsAsync(request.MovieId))
                throw new ArgumentException("Movie not found.");

            var review = new Review
            {
                MovieId = request.MovieId,
                CustomerName = request.CustomerName.Trim(),
                Rating = request.Rating,
                Comment = request.Comment.Trim(),
                ReviewDate = DateTime.Now
            };

            var savedReview = await _reviewRepository.AddAsync(review);
            return MapToResponse(savedReview);
        }

        public async Task<ReviewResponse?> UpdateAsync(
            int id,
            UpdateReviewRequest request)
        {
            var review = await _reviewRepository.GetByIdAsync(id);

            if (review == null)
                return null;

            ValidateReview(request.CustomerName, request.Rating, request.Comment);

            review.CustomerName = request.CustomerName.Trim();
            review.Rating = request.Rating;
            review.Comment = request.Comment.Trim();

            await _reviewRepository.UpdateAsync(review);

            return MapToResponse(review);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var review = await _reviewRepository.GetByIdAsync(id);

            if (review == null)
                return false;

            await _reviewRepository.DeleteAsync(review);
            return true;
        }

        public async Task<object> GetMovieRatingSummaryAsync(int movieId)
        {
            if (movieId <= 0)
                throw new ArgumentException("Invalid MovieId.");

            if (!await _reviewRepository.MovieExistsAsync(movieId))
                throw new ArgumentException("Movie not found.");

            var averageRating =
                await _reviewRepository.GetAverageRatingAsync(movieId);

            var reviewCount =
                await _reviewRepository.GetReviewCountAsync(movieId);

            return new
            {
                MovieId = movieId,
                AverageRating = Math.Round(averageRating, 2),
                TotalReviews = reviewCount
            };
        }

        private static void ValidateReview(
            string customerName,
            int rating,
            string comment)
        {
            if (string.IsNullOrWhiteSpace(customerName) ||
                customerName.Length > 150)
            {
                throw new ArgumentException(
                    "Customer name is required and must not exceed 150 characters.");
            }

            if (rating < 1 || rating > 5)
                throw new ArgumentException("Rating must be between 1 and 5.");

            if (string.IsNullOrWhiteSpace(comment) ||
                comment.Length > 1000)
            {
                throw new ArgumentException(
                    "Comment is required and must not exceed 1000 characters.");
            }
        }

        private static ReviewResponse MapToResponse(Review review)
        {
            return new ReviewResponse
            {
                ReviewId = review.ReviewId,
                MovieId = review.MovieId,
                MovieTitle = review.Movie?.Title ?? string.Empty,
                CustomerName = review.CustomerName,
                Rating = review.Rating,
                Comment = review.Comment,
                ReviewDate = review.ReviewDate
            };
        }
    }
}