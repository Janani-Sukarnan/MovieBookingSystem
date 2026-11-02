
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MovieTicketBooking.DTOs.Shows;
using MovieTicketBooking.Services.Interfaces;
using MovieBookingSystemApi.Constants;

namespace MovieTicketBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowsController : ControllerBase
    {
        private readonly IShowService _showService;

        public ShowsController(IShowService showService)
        {
            _showService = showService;
        }

        // GET: api/shows
        // Public access
        [HttpGet]
        public async Task<IActionResult> GetAll(
            int pageNumber = 1,
            int pageSize = 10,
            int? movieId = null,
            int? screenId = null)
        {
            try
            {
                var shows = await _showService.GetAllAsync(
                    pageNumber, pageSize, movieId, screenId);

                return Ok(shows);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/shows/1
        // Public access
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var show = await _showService.GetByIdAsync(id);

            if (show == null)
                return NotFound(new { message = "Show not found." });

            return Ok(show);
        }

        // POST: api/shows
        // Admin only
        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Create(CreateShowRequest request)
        {
            try
            {
                var show = await _showService.CreateAsync(request);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = show.ShowId },
                    show);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/shows/1
        // Admin only
        [HttpPut("{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Update(
            int id, UpdateShowRequest request)
        {
            try
            {
                var show = await _showService.UpdateAsync(id, request);

                if (show == null)
                    return NotFound(new { message = "Show not found." });

                return Ok(show);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
