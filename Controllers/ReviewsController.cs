using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Reviews;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        // GET: api/reviews
        [HttpGet]
        public async Task<IActionResult> GetAll(
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10)
        {
            try
            {
                var reviews = await _reviewService.GetAllAsync(
                    pageNumber, pageSize);

                return Ok(reviews);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/reviews/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var review = await _reviewService.GetByIdAsync(id);

            if (review == null)
                return NotFound("Review not found.");

            return Ok(review);
        }

        // GET: api/reviews/movie/1
        [HttpGet("movie/{movieId}")]
        public async Task<IActionResult> GetByMovieId(
    int movieId,
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10)
        {
            try
            {
                var reviews = await _reviewService.GetByMovieIdAsync(
                    movieId, pageNumber, pageSize);

                return Ok(reviews);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/reviews
        [HttpPost]
        public async Task<IActionResult> Create(CreateReviewRequest request)
        {
            try
            {
                var review = await _reviewService.CreateAsync(request);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = review.ReviewId },
                    review);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/reviews/1
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateReviewRequest request)
        {
            try
            {
                var review = await _reviewService.UpdateAsync(id, request);

                if (review == null)
                    return NotFound("Review not found.");

                return Ok(review);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/reviews/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _reviewService.DeleteAsync(id);

            if (!deleted)
                return NotFound("Review not found.");

            return Ok("Review deleted successfully.");
        }
        // GET: api/reviews/movie/1/summary
        [HttpGet("movie/{movieId}/summary")]
        public async Task<IActionResult> GetMovieRatingSummary(int movieId)
        {
            try
            {
                var summary =
                    await _reviewService.GetMovieRatingSummaryAsync(movieId);

                return Ok(summary);
            }
            catch (ArgumentException ex)
            {


                return BadRequest(ex.Message);
            }
        }
    }
}
