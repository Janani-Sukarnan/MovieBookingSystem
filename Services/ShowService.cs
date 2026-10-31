
using MovieTicketBooking.DTOs.Shows;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services
{
    public class ShowService : IShowService
    {
        private readonly IShowRepository _showRepository;
        private readonly IMovieRepository _movieRepository;
        private readonly IScreenRepository _screenRepository;

        public ShowService(
            IShowRepository showRepository,
            IMovieRepository movieRepository,
            IScreenRepository screenRepository)
        {
            _showRepository = showRepository;
            _movieRepository = movieRepository;
            _screenRepository = screenRepository;
        }

        public async Task<List<ShowResponse>> GetAllAsync(
            int pageNumber, int pageSize, int? movieId, int? screenId)
        {
            if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
                throw new ArgumentException("Invalid pagination values.");

            var shows = await _showRepository.GetPagedAsync(
                pageNumber, pageSize, movieId, screenId);

            return shows.Select(MapToResponse).ToList();
        }

        public async Task<ShowResponse?> GetByIdAsync(int id)
        {
            var show = await _showRepository.GetByIdAsync(id);

            return show == null ? null : MapToResponse(show);
        }

        public async Task<ShowResponse> CreateAsync(CreateShowRequest request)
        {
            ValidateShow(
                request.StartTime,
                request.EndTime,
                request.TicketPrice);

            if (!await _movieRepository.ExistsAsync(request.MovieId))
                throw new ArgumentException("Movie does not exist.");

            if (!await _screenRepository.ExistsAsync(request.ScreenId))
                throw new ArgumentException("Screen does not exist.");

            if (await _showRepository.HasOverlappingShowAsync(
                request.ScreenId,
                request.StartTime,
                request.EndTime))
            {
                throw new ArgumentException(
                    "Another show is already scheduled on this screen.");
            }

            var seats = await _showRepository.GetSeatsByScreenIdAsync(
                request.ScreenId);

            if (seats.Count == 0)
                throw new ArgumentException(
                    "No seats are configured for this screen.");

            var show = new Show
            {
                MovieId = request.MovieId,
                ScreenId = request.ScreenId,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                TicketPrice = request.TicketPrice
            };

            await _showRepository.AddAsync(show);

            var showSeats = seats.Select(seat => new ShowSeat
            {
                ShowId = show.ShowId,
                SeatId = seat.SeatId,
                IsBooked = false
            }).ToList();

            await _showRepository.AddShowSeatsAsync(showSeats);

            return MapToResponse(show);
        }

        public async Task<ShowResponse?> UpdateAsync(
            int id, UpdateShowRequest request)
        {
            var show = await _showRepository.GetByIdAsync(id);

            if (show == null)
                return null;

            ValidateShow(
                request.StartTime,
                request.EndTime,
                request.TicketPrice);

            if (!await _movieRepository.ExistsAsync(request.MovieId))
                throw new ArgumentException("Movie does not exist.");

            if (!await _screenRepository.ExistsAsync(request.ScreenId))
                throw new ArgumentException("Screen does not exist.");

            if (await _showRepository.HasOverlappingShowAsync(
                request.ScreenId,
                request.StartTime,
                request.EndTime,
                id))
            {
                throw new ArgumentException(
                    "Another show is already scheduled on this screen.");
            }

            if (request.ScreenId != show.ScreenId)
            {
                throw new ArgumentException(
                    "The screen cannot be changed after show seats are generated.");
            }

            show.MovieId = request.MovieId;
            show.StartTime = request.StartTime;
            show.EndTime = request.EndTime;
            show.TicketPrice = request.TicketPrice;

            await _showRepository.UpdateAsync(show);

            return MapToResponse(show);
        }

        private static void ValidateShow(
            DateTime startTime,
            DateTime endTime,
            decimal ticketPrice)
        {
            if (endTime <= startTime)
                throw new ArgumentException(
                    "End time must be later than start time.");

            if (ticketPrice <= 0)
                throw new ArgumentException(
                    "Ticket price must be greater than zero.");
        }

        private static ShowResponse MapToResponse(Show show)
        {
            return new ShowResponse
            {
                ShowId = show.ShowId,
                MovieId = show.MovieId,
                ScreenId = show.ScreenId,
                StartTime = show.StartTime,
                EndTime = show.EndTime,
                TicketPrice = show.TicketPrice
            };
        }
    }
}
