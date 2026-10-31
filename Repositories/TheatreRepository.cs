
using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;

namespace MovieTicketBooking.Repositories
{
    public class TheatreRepository : ITheatreRepository
    {
        private readonly ApplicationDbContext _context;

        public TheatreRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Theatre>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? name,
            string? city)
        {
            var query = _context.Theatres.AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(t => t.Name.Contains(name));
            }

            if (!string.IsNullOrWhiteSpace(city))
            {
                query = query.Where(t => t.City.Contains(city));
            }

            return await query
                .OrderBy(t => t.TheatreId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Theatre?> GetByIdAsync(int id)
        {
            return await _context.Theatres.FindAsync(id);
        }

        public async Task<Theatre> AddAsync(Theatre theatre)
        {
            await _context.Theatres.AddAsync(theatre);
            await _context.SaveChangesAsync();

            return theatre;
        }

        public async Task UpdateAsync(Theatre theatre)
        {
            _context.Theatres.Update(theatre);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Theatre theatre)
        {
            _context.Theatres.Remove(theatre);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Theatres
                .AnyAsync(t => t.TheatreId == id);
        }
    }
}
