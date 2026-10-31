
using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;

namespace MovieTicketBooking.Repositories
{
    public class ShowRepository : IShowRepository
    {
        private readonly ApplicationDbContext _context;

        public ShowRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Show>> GetPagedAsync(
            int pageNumber, int pageSize, int? movieId, int? screenId)
        {
            var query = _context.Shows.AsQueryable();

            if (movieId.HasValue)
                query = query.Where(s => s.MovieId == movieId.Value);

            if (screenId.HasValue)
                query = query.Where(s => s.ScreenId == screenId.Value);

            return await query
                .OrderBy(s => s.StartTime)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Show?> GetByIdAsync(int id)
        {
            return await _context.Shows
                .FirstOrDefaultAsync(s => s.ShowId == id);
        }

        public async Task<Show> AddAsync(Show show)
        {
            _context.Shows.Add(show);
            await _context.SaveChangesAsync();
            return show;
        }

        public async Task UpdateAsync(Show show)
        {
            _context.Shows.Update(show);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Shows
                .AnyAsync(s => s.ShowId == id);
        }

        public async Task<bool> HasOverlappingShowAsync(
            int screenId,
            DateTime startTime,
            DateTime endTime,
            int? excludeShowId = null)
        {
            var query = _context.Shows.Where(s =>
                s.ScreenId == screenId &&
                s.StartTime < endTime &&
                s.EndTime > startTime);

            if (excludeShowId.HasValue)
                query = query.Where(s => s.ShowId != excludeShowId.Value);

            return await query.AnyAsync();
        }

        public async Task<List<Seat>> GetSeatsByScreenIdAsync(int screenId)
        {
            return await _context.Seats
                .Where(s => s.ScreenId == screenId)
                .ToListAsync();
        }

        public async Task AddShowSeatsAsync(List<ShowSeat> showSeats)
        {
            _context.ShowSeats.AddRange(showSeats);
            await _context.SaveChangesAsync();
        }
    }
}
