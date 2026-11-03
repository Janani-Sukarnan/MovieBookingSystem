
using MovieTicketBooking.DTOs.Screens;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;
using MovieTicketBooking.DTOs.Common;

namespace MovieTicketBooking.Services
{
    public class ScreenService : IScreenService
    {
        private readonly IScreenRepository _screenRepository;
        private readonly ITheatreRepository _theatreRepository;

        public ScreenService(
            IScreenRepository screenRepository,
            ITheatreRepository theatreRepository)
        {
            _screenRepository = screenRepository;
            _theatreRepository = theatreRepository;
        }

        public async Task<PagedResponse<ScreenResponse>> GetAllAsync(
    int pageNumber, int pageSize, int? theatreId)
        {
            if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
                throw new ArgumentException("Invalid pagination values.");

            var totalRecords = await _screenRepository.GetTotalScreensAsync(theatreId);

            var screens = await _screenRepository.GetPagedAsync(
                pageNumber, pageSize, theatreId);

            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            return new PagedResponse<ScreenResponse>
            {
                Items = screens.Select(s => MapToResponse(s)).ToList(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = totalPages
            };
        }

        public async Task<ScreenResponse?> GetByIdAsync(int id)
        {
            var screen = await _screenRepository.GetByIdAsync(id);

            if (screen == null)
                return null;

            return MapToResponse(screen);
        }

        public async Task<ScreenResponse> CreateAsync(
            CreateScreenRequest request)
        {
            ValidateScreen(
                request.ScreenName, request.TotalSeats, request.ScreenType);

            if (!await _theatreRepository.ExistsAsync(request.TheatreId))
                throw new ArgumentException("Theatre does not exist.");

            var screen = new Screen
            {
                TheatreId = request.TheatreId,
                ScreenName = request.ScreenName.Trim(),
                TotalSeats = request.TotalSeats,
                ScreenType = request.ScreenType.Trim()
            };

            var createdScreen = await _screenRepository.AddAsync(screen);

            return MapToResponse(createdScreen);
        }

        public async Task<ScreenResponse?> UpdateAsync(
            int id, UpdateScreenRequest request)
        {
            var screen = await _screenRepository.GetByIdAsync(id);

            if (screen == null)
                return null;

            ValidateScreen(
                request.ScreenName, request.TotalSeats, request.ScreenType);

            if (!await _theatreRepository.ExistsAsync(request.TheatreId))
                throw new ArgumentException("Theatre does not exist.");

            screen.TheatreId = request.TheatreId;
            screen.ScreenName = request.ScreenName.Trim();
            screen.TotalSeats = request.TotalSeats;
            screen.ScreenType = request.ScreenType.Trim();

            await _screenRepository.UpdateAsync(screen);

            return MapToResponse(screen);
        }

        private static void ValidateScreen(
            string name, int totalSeats, string screenType)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Screen name is required.");

            if (name.Trim().Length > 50)
                throw new ArgumentException("Screen name cannot exceed 50 characters.");

            if (totalSeats <= 0)
                throw new ArgumentException("Total seats must be greater than zero.");

            if (string.IsNullOrWhiteSpace(screenType))
                throw new ArgumentException("Screen type is required.");

            if (screenType.Trim().Length > 30)
                throw new ArgumentException("Screen type cannot exceed 30 characters.");
        }

        private static ScreenResponse MapToResponse(Screen screen)
        {
            return new ScreenResponse
            {
                ScreenId = screen.ScreenId,
                TheatreId = screen.TheatreId,
                ScreenName = screen.ScreenName,
                TotalSeats = screen.TotalSeats,
                ScreenType = screen.ScreenType
            };
        }
    }
}

