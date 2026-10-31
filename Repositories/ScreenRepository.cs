
using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;

namespace MovieTicketBooking.Repositories
{
    public class ScreenRepository : IScreenRepository
    {
        private readonly ApplicationDbContext _context;

        public ScreenRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Screen>> GetPagedAsync(
            int pageNumber, int pageSize, int? theatreId)
        {
            var query = _context.Screens.AsQueryable();

            if (theatreId.HasValue)
            {
                query = query.Where(s => s.TheatreId == theatreId.Value);
            }

            return await query
                .OrderBy(s => s.ScreenId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Screen?> GetByIdAsync(int id)
        {
            return await _context.Screens.FindAsync(id);
        }

        public async Task<Screen> AddAsync(Screen screen)
        {
            await _context.Screens.AddAsync(screen);
            await _context.SaveChangesAsync();

            return screen;
        }

        public async Task UpdateAsync(Screen screen)
        {
            _context.Screens.Update(screen);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Screens
                .AnyAsync(s => s.ScreenId == id);
        }
    }
}
