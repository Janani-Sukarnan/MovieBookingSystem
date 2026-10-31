using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;

namespace MovieTicketBooking.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly ApplicationDbContext _context;

        public MovieRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Movie>> GetAllAsync()
        {
            return await _context.Movies.ToListAsync();
        }

        public async Task<Movie?> GetByIdAsync(int id)
        {
            return await _context.Movies.FindAsync(id);
        }

        public async Task<Movie> AddAsync(Movie movie)
        {
            await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync();

            return movie;
        }

        public async Task UpdateAsync(Movie movie)
        {
            _context.Movies.Update(movie);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Movie movie)
        {
            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Movies.AnyAsync(m => m.MovieId == id);
        }

        public async Task<List<Movie>> GetPagedAsync(
        int pageNumber, int pageSize, string? title)
        {
            var query = _context.Movies.AsQueryable();

            if (!string.IsNullOrWhiteSpace(title))
            {
                query = query.Where(m => m.Title.Contains(title));
            }

            return await query
                .OrderBy(m => m.MovieId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<Movie>> SearchByTitleAsync(string title)
        {
            return await _context.Movies
                .Where(m => m.Title.Contains(title))
                .OrderBy(m => m.Title)
                .ToListAsync();
        }
    }
}