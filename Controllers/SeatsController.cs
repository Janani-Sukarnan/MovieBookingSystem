
using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Seats;
using MovieTicketBooking.Services.Interfaces;

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

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var seat = await _seatService.GetByIdAsync(id);

            if (seat == null)
                return NotFound(new { message = "Seat not found." });

            return Ok(seat);
        }

        [HttpPost]
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

        [HttpPut("{id:int}")]
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
