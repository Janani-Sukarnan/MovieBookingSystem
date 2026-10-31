
using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Movies;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        // GET: api/movies
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? title = null)
        {
            try
            {
                var movies = await _movieService.GetAllAsync(
                    pageNumber, pageSize, title);

                return Ok(movies);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/movies/1
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var movie = await _movieService.GetByIdAsync(id);

            if (movie == null)
                return NotFound(new { message = "Movie not found." });

            return Ok(movie);
        }

        // POST: api/movies
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateMovieRequest request)
        {
            try
            {
                var movie = await _movieService.CreateAsync(request);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = movie.MovieId },
                    movie);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/movies/1
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateMovieRequest request)
        {
            try
            {
                var movie = await _movieService.UpdateAsync(id, request);

                if (movie == null)
                    return NotFound(new { message = "Movie not found." });

                return Ok(movie);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/movies/1
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _movieService.DeleteAsync(id);

            if (!deleted)
                return NotFound(new { message = "Movie not found." });

            return Ok(new { message = "Movie deleted successfully." });
        }

        // GET: api/movies/search?title=Avatar
        [HttpGet("search")]
        public async Task<IActionResult> SearchByTitle(
            [FromQuery] string title)
        {
            try
            {
                var movies = await _movieService.SearchByTitleAsync(title);
                return Ok(movies);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
