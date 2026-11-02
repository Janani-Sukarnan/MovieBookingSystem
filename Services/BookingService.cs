
using System.Net.Mail;
using MovieTicketBooking.DTOs.Bookings;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingService(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<List<BookingResponse>> GetAllAsync(
            int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
                throw new ArgumentException("Invalid pagination values.");

            var bookings = await _bookingRepository.GetPagedAsync(
                pageNumber, pageSize);

            return bookings.Select(MapToResponse).ToList();
        }

        public async Task<BookingResponse?> GetByIdAsync(
            int id, int userId, bool canManageAll)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);

            if (booking == null)
                return null;

            if (!canManageAll && booking.UserId != userId)
                return null;

            return MapToResponse(booking);
        }

        public async Task<BookingResponse> CreateAsync(
            CreateBookingRequest request, int userId)
        {
            ValidateCustomer(request);

            if (request.ShowId <= 0)
                throw new ArgumentException("Invalid ShowId.");

            if (request.ShowSeatIds == null ||
                request.ShowSeatIds.Count == 0)
                throw new ArgumentException("Select at least one seat.");

            if (request.ShowSeatIds.Any(id => id <= 0))
                throw new ArgumentException("Invalid ShowSeatId.");

            if (request.ShowSeatIds.Distinct().Count() !=
                request.ShowSeatIds.Count)
                throw new ArgumentException("Duplicate seats are not allowed.");

            var showSeats =
                await _bookingRepository.GetShowSeatsForBookingAsync(
                    request.ShowId, request.ShowSeatIds);

            if (showSeats.Count != request.ShowSeatIds.Count)
                throw new ArgumentException(
                    "One or more selected seats do not belong to this show.");

            if (showSeats.Any(s => s.IsBooked))
                throw new ArgumentException(
                    "One or more selected seats are already booked.");

            if (showSeats.Any(s =>
                s.Show == null || s.Show.StartTime <= DateTime.Now))
                throw new ArgumentException(
                    "Seats cannot be booked for a show that has already started.");

            decimal totalAmount =
                showSeats.Sum(s => s.Show!.TicketPrice);

            var booking = new Booking
            {
                CustomerName = request.CustomerName.Trim(),
                CustomerEmail = request.CustomerEmail.Trim(),
                ContactNumber = request.ContactNumber.Trim(),
                BookingDate = DateTime.Now,
                TotalAmount = totalAmount,
                Status = "Confirmed",

                // Link the booking to the authenticated user.
                UserId = userId,

                BookingSeats = new List<BookingSeat>()
            };

            var savedBooking =
                await _bookingRepository.AddAsync(booking, showSeats);

            return MapToResponse(savedBooking);
        }

        public async Task<BookingResponse?> UpdateAsync(
            int id,
            UpdateBookingRequest request,
            int userId,
            bool canManageAll)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);

            if (booking == null)
                return null;

            if (!canManageAll && booking.UserId != userId)
                return null;

            if (booking.Status != "Confirmed")
                throw new ArgumentException(
                    "Only confirmed bookings can be updated.");

            ValidateCustomer(
                request.CustomerName,
                request.CustomerEmail,
                request.ContactNumber);

            booking.CustomerName = request.CustomerName.Trim();
            booking.CustomerEmail = request.CustomerEmail.Trim();
            booking.ContactNumber = request.ContactNumber.Trim();

            await _bookingRepository.UpdateAsync(booking);

            return MapToResponse(booking);
        }

        public async Task<bool> CancelAsync(
            int id, int userId, bool canManageAll)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);

            if (booking == null)
                return false;

            if (!canManageAll && booking.UserId != userId)
                return false;

            if (booking.Status != "Confirmed")
                throw new ArgumentException(
                    "Only confirmed bookings can be cancelled.");

            if (booking.BookingSeats.Any(bs => bs.ShowSeat == null))
                throw new InvalidOperationException(
                    "Booking seat information could not be loaded.");

            await _bookingRepository.CancelAsync(booking);

            return true;
        }

        private static void ValidateCustomer(CreateBookingRequest request)
        {
            ValidateCustomer(
                request.CustomerName,
                request.CustomerEmail,
                request.ContactNumber);
        }

        private static void ValidateCustomer(
            string name, string email, string contactNumber)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length > 150)
                throw new ArgumentException(
                    "Customer name is required and must not exceed 150 characters.");

            if (string.IsNullOrWhiteSpace(email) || email.Length > 150)
                throw new ArgumentException("A valid email is required.");

            try
            {
                var address = new MailAddress(email);

                if (address.Address != email.Trim())
                    throw new ArgumentException("Invalid email address.");
            }
            catch (FormatException)
            {
                throw new ArgumentException("Invalid email address.");
            }

            if (string.IsNullOrWhiteSpace(contactNumber) ||
                contactNumber.Length > 20)
                throw new ArgumentException(
                    "Contact number is required and must not exceed 20 characters.");
        }

        private static BookingResponse MapToResponse(Booking booking)
        {
            return new BookingResponse
            {
                BookingId = booking.BookingId,
                CustomerName = booking.CustomerName,
                CustomerEmail = booking.CustomerEmail,
                ContactNumber = booking.ContactNumber,

                ShowId = booking.BookingSeats
                    .Select(bs => bs.ShowSeat?.ShowId ?? 0)
                    .FirstOrDefault(),

                ShowSeatIds = booking.BookingSeats
                    .Select(bs => bs.ShowSeatId)
                    .ToList(),

                BookingDate = booking.BookingDate,
                TotalAmount = booking.TotalAmount,
                Status = booking.Status
            };
        }
    }
}
