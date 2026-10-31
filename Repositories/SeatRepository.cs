
using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;

namespace MovieTicketBooking.Repositories
{
    public class SeatRepository : ISeatRepository
    {
        private readonly ApplicationDbContext _context;

        public SeatRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Seat>> GetPagedAsync(
            int pageNumber, int pageSize, int? screenId)
        {
            var query = _context.Seats.AsQueryable();

            if (screenId.HasValue)
            {
                query = query.Where(s => s.ScreenId == screenId.Value);
            }

            return await query
                .OrderBy(s => s.SeatId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Seat?> GetByIdAsync(int id)
        {
            return await _context.Seats.FindAsync(id);
        }

        public async Task<Seat> AddAsync(Seat seat)
        {
            await _context.Seats.AddAsync(seat);
            await _context.SaveChangesAsync();

            return seat;
        }

        public async Task UpdateAsync(Seat seat)
        {
            _context.Seats.Update(seat);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Seats
                .AnyAsync(s => s.SeatId == id);
        }
    }
}
