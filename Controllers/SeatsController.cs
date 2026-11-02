
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MovieTicketBooking.DTOs.Seats;
using MovieTicketBooking.Services.Interfaces;
using MovieBookingSystemApi.Constants;

namespace MovieTicketBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatsController : ControllerBase
    {
        private readonly ISeatService _seatService;

        public SeatsController(ISeatService seatService)
        {
            _seatService = seatService;
        }

        // GET: api/seats
        // Public access
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] int? screenId = null)
        {
            try
            {
                var seats = await _seatService.GetAllAsync(
                    pageNumber, pageSize, screenId);

                return Ok(seats);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/seats/1
        // Public access
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var seat = await _seatService.GetByIdAsync(id);

            if (seat == null)
                return NotFound(new { message = "Seat not found." });

            return Ok(seat);
        }

        // POST: api/seats
        // Admin only
        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Create(
            [FromBody] CreateSeatRequest request)
        {
            try
            {
                var seat = await _seatService.CreateAsync(request);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = seat.SeatId },
                    seat);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/seats/1
        // Admin only
        [HttpPut("{id:int}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateSeatRequest request)
        {
            try
            {
                var seat = await _seatService.UpdateAsync(id, request);

                if (seat == null)
                    return NotFound(new { message = "Seat not found." });

                return Ok(seat);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
