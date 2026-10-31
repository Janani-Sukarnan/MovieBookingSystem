
using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Screens;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScreensController : ControllerBase
    {
        private readonly IScreenService _screenService;

        public ScreensController(IScreenService screenService)
        {
            _screenService = screenService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] int? theatreId = null)
        {
            try
            {
                var screens = await _screenService.GetAllAsync(
                    pageNumber, pageSize, theatreId);

                return Ok(screens);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var screen = await _screenService.GetByIdAsync(id);

            if (screen == null)
                return NotFound(new { message = "Screen not found." });

            return Ok(screen);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateScreenRequest request)
        {
            try
            {
                var screen = await _screenService.CreateAsync(request);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = screen.ScreenId },
                    screen);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateScreenRequest request)
        {
            try
            {
                var screen = await _screenService.UpdateAsync(id, request);

                if (screen == null)
                    return NotFound(new { message = "Screen not found." });

                return Ok(screen);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
