using MovieTicketBooking.DTOs.Movies;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;

        public MovieService(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        // Get all movies with pagination
        public async Task<List<MovieResponse>> GetAllAsync(
            int pageNumber, int pageSize, string? title)
        {
            if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
            {
                throw new ArgumentException("Invalid pagination values.");
            }

            var movies = await _movieRepository.GetPagedAsync(
                pageNumber, pageSize, title);

            return movies.Select(m => MapToResponse(m)).ToList();
        }

        // Get movie by ID
        public async Task<MovieResponse?> GetByIdAsync(int id)
        {
            var movie = await _movieRepository.GetByIdAsync(id);

            if (movie == null)
                return null;

            return MapToResponse(movie);
        }

        // Create movie
        public async Task<MovieResponse> CreateAsync(
            CreateMovieRequest request)
        {
            ValidateMovie(
                request.Title,
                request.DurationMinutes,
                request.Language,
                request.Genre);

            var movie = new Movie
            {
                Title = request.Title.Trim(),
                Description = request.Description,
                DurationMinutes = request.DurationMinutes,
                ReleaseDate = request.ReleaseDate,
                Language = request.Language.Trim(),
                Genre = request.Genre.Trim()
            };

            var createdMovie = await _movieRepository.AddAsync(movie);

            return MapToResponse(createdMovie);
        }

        // Update movie
        public async Task<MovieResponse?> UpdateAsync(
            int id, UpdateMovieRequest request)
        {
            var movie = await _movieRepository.GetByIdAsync(id);

            if (movie == null)
                return null;

            ValidateMovie(
                request.Title,
                request.DurationMinutes,
                request.Language,
                request.Genre);

            movie.Title = request.Title.Trim();
            movie.Description = request.Description;
            movie.DurationMinutes = request.DurationMinutes;
            movie.ReleaseDate = request.ReleaseDate;
            movie.Language = request.Language.Trim();
            movie.Genre = request.Genre.Trim();

            await _movieRepository.UpdateAsync(movie);

            return MapToResponse(movie);
        }

        // Delete movie
        public async Task<bool> DeleteAsync(int id)
        {
            var movie = await _movieRepository.GetByIdAsync(id);

            if (movie == null)
                return false;

            await _movieRepository.DeleteAsync(movie);

            return true;
        }

        // Search movie by title
        public async Task<List<MovieResponse>> SearchByTitleAsync(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Movie title is required.");
            }

            var movies = await _movieRepository.SearchByTitleAsync(title.Trim());

            return movies.Select(m => MapToResponse(m)).ToList();
        }

        // Validate movie details
        private static void ValidateMovie(
            string title,
            int duration,
            string language,
            string genre)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Movie title is required.");

            if (title.Trim().Length > 150)
                throw new ArgumentException("Movie title cannot exceed 150 characters.");

            if (duration <= 0)
                throw new ArgumentException("Duration must be greater than zero.");

            if (string.IsNullOrWhiteSpace(language))
                throw new ArgumentException("Language is required.");

            if (language.Trim().Length > 50)
                throw new ArgumentException("Language cannot exceed 50 characters.");

            if (string.IsNullOrWhiteSpace(genre))
                throw new ArgumentException("Genre is required.");

            if (genre.Trim().Length > 50)
                throw new ArgumentException("Genre cannot exceed 50 characters.");
        }

        // Convert Model into Response DTO
        private static MovieResponse MapToResponse(Movie movie)
        {
            return new MovieResponse
            {
                MovieId = movie.MovieId,
                Title = movie.Title,
                Description = movie.Description,
                DurationMinutes = movie.DurationMinutes,
                ReleaseDate = movie.ReleaseDate,
                Language = movie.Language,
                Genre = movie.Genre
            };
        }
    }
}