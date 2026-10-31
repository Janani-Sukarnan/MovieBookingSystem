
using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;

namespace MovieTicketBooking.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext _context;

        public BookingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Booking>> GetPagedAsync(
            int pageNumber, int pageSize)
        {
            return await _context.Bookings
                .Include(b => b.BookingSeats)
                    .ThenInclude(bs => bs.ShowSeat)
                        .ThenInclude(ss => ss!.Show)
                .OrderByDescending(b => b.BookingDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Booking?> GetByIdAsync(int id)
        {
            return await _context.Bookings
                .Include(b => b.BookingSeats)
                    .ThenInclude(bs => bs.ShowSeat)
                        .ThenInclude(ss => ss!.Show)
                .FirstOrDefaultAsync(b => b.BookingId == id);
        }

        public async Task<List<ShowSeat>> GetShowSeatsForBookingAsync(
            int showId, List<int> showSeatIds)
        {
            return await _context.ShowSeats
                .Include(ss => ss.Show)
                .Where(ss => ss.ShowId == showId &&
                             showSeatIds.Contains(ss.ShowSeatId))
                .ToListAsync();
        }

        public async Task<Booking> AddAsync(
            Booking booking, List<ShowSeat> showSeats)
        {
            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                foreach (var showSeat in showSeats)
                {
                    showSeat.IsBooked = true;

                    booking.BookingSeats.Add(new BookingSeat
                    {
                        ShowSeatId = showSeat.ShowSeatId,
                        Price = showSeat.Show!.TicketPrice
                    });
                }

                _context.Bookings.Add(booking);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return booking;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task UpdateAsync(Booking booking)
        {
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Bookings
                .AnyAsync(b => b.BookingId == id);
        }

        public async Task CancelAsync(Booking booking)
        {
            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                foreach (var bookingSeat in booking.BookingSeats)
                {
                    if (bookingSeat.ShowSeat != null)
                    {
                        bookingSeat.ShowSeat.IsBooked = false;
                    }
                }

                booking.Status = "Cancelled";

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
