using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;

namespace MovieTicketBooking.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _context;

        public ReviewRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Review>> GetAllAsync()
        {
            return await _context.Reviews
                .Include(r => r.Movie)
                .OrderByDescending(r => r.ReviewDate)
                .ToListAsync();
        }

        public async Task<Review?> GetByIdAsync(int id)
        {
            return await _context.Reviews
                .Include(r => r.Movie)
                .FirstOrDefaultAsync(r => r.ReviewId == id);
        }

        public async Task<List<Review>> GetByMovieIdAsync(int movieId)
        {
            return await _context.Reviews
                .Include(r => r.Movie)
                .Where(r => r.MovieId == movieId)
                .OrderByDescending(r => r.ReviewDate)
                .ToListAsync();
        }

        public async Task<Review> AddAsync(Review review)
        {
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(review.ReviewId) ?? review;
        }

        public async Task UpdateAsync(Review review)
        {
            _context.Reviews.Update(review);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Review review)
        {
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> MovieExistsAsync(int movieId)
        {
            return await _context.Movies
                .AnyAsync(m => m.MovieId == movieId);
        }

        public async Task<double> GetAverageRatingAsync(int movieId)
        {
            var average = await _context.Reviews
                .Where(r => r.MovieId == movieId)
                .Select(r => (double?)r.Rating)
                .AverageAsync();

            return average ?? 0;
        }

        public async Task<int> GetReviewCountAsync(int movieId)
        {
            return await _context.Reviews
                .CountAsync(r => r.MovieId == movieId);
        }
    }
}