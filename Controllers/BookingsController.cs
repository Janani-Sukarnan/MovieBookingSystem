
using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Bookings;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet]
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
            var booking = await _bookingService.GetByIdAsync(id);

            if (booking == null)
                return NotFound(new { message = "Booking not found." });

            return Ok(booking);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            CreateBookingRequest request)
        {
            try
            {
                var booking = await _bookingService.CreateAsync(request);

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
            try
            {
                var booking = await _bookingService.UpdateAsync(
                    id, request);

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
            try
            {
                var cancelled = await _bookingService.CancelAsync(id);

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
