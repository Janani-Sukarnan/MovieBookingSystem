
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using MovieTicketBooking.DTOs.Bookings;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        private bool TryGetCurrentUser(
            out int userId, out bool canManageAll)
        {
            userId = 0;

            canManageAll =
                User.IsInRole("Admin") ||
                User.IsInRole("TheatreManager");

            return int.TryParse(
                User.FindFirstValue(ClaimTypes.NameIdentifier),
                out userId);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,TheatreManager")]
        public async Task<IActionResult> GetAll(
            int pageNumber = 1,
            int pageSize = 10)
        {
            try
            {
                var bookings = await _bookingService.GetAllAsync(
                    pageNumber, pageSize);

                return Ok(bookings);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (!TryGetCurrentUser(out int userId, out bool canManageAll))
                return Unauthorized();

            var booking = await _bookingService.GetByIdAsync(
                id, userId, canManageAll);

            if (booking == null)
                return NotFound(new { message = "Booking not found." });

            return Ok(booking);
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Create(
            CreateBookingRequest request)
        {
            if (!TryGetCurrentUser(out int userId, out _))
                return Unauthorized();

            try
            {
                var booking = await _bookingService.CreateAsync(
                    request, userId);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = booking.BookingId },
                    booking);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id, UpdateBookingRequest request)
        {
            if (!TryGetCurrentUser(out int userId, out bool canManageAll))
                return Unauthorized();

            try
            {
                var booking = await _bookingService.UpdateAsync(
                    id, request, userId, canManageAll);

                if (booking == null)
                    return NotFound(new { message = "Booking not found." });

                return Ok(booking);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> Cancel(int id)
        {
            if (!TryGetCurrentUser(out int userId, out bool canManageAll))
                return Unauthorized();

            try
            {
                var cancelled = await _bookingService.CancelAsync(
                    id, userId, canManageAll);

                if (!cancelled)
                    return NotFound(new { message = "Booking not found." });

                return Ok(new
                {
                    message = "Booking cancelled successfully."
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
