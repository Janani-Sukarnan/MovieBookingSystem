
using MovieTicketBooking.DTOs.Seats;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;
using MovieTicketBooking.DTOs.Common;

namespace MovieTicketBooking.Services
{
    public class SeatService : ISeatService
    {
        private readonly ISeatRepository _seatRepository;
        private readonly IScreenRepository _screenRepository;

        public SeatService(
            ISeatRepository seatRepository,
            IScreenRepository screenRepository)
        {
            _seatRepository = seatRepository;
            _screenRepository = screenRepository;
        }

        public async Task<PagedResponse<SeatResponse>> GetAllAsync(
    int pageNumber, int pageSize, int? screenId)
        {
            if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
                throw new ArgumentException("Invalid pagination values.");

            var totalRecords = await _seatRepository.GetTotalSeatsAsync(screenId);

            var seats = await _seatRepository.GetPagedAsync(
                pageNumber, pageSize, screenId);

            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            return new PagedResponse<SeatResponse>
            {
                Items = seats.Select(s => MapToResponse(s)).ToList(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = totalPages
            };
        }

        public async Task<SeatResponse?> GetByIdAsync(int id)
        {
            var seat = await _seatRepository.GetByIdAsync(id);

            if (seat == null)
                return null;

            return MapToResponse(seat);
        }

        public async Task<SeatResponse> CreateAsync(CreateSeatRequest request)
        {
            ValidateSeat(request.SeatNumber, request.SeatType, request.Price);

            if (!await _screenRepository.ExistsAsync(request.ScreenId))
                throw new ArgumentException("Screen does not exist.");

            var seat = new Seat
            {
                ScreenId = request.ScreenId,
                SeatNumber = request.SeatNumber.Trim(),
                SeatType = request.SeatType.Trim(),
                Price = request.Price
            };

            var createdSeat = await _seatRepository.AddAsync(seat);

            return MapToResponse(createdSeat);
        }

        public async Task<SeatResponse?> UpdateAsync(
            int id, UpdateSeatRequest request)
        {
            var seat = await _seatRepository.GetByIdAsync(id);

            if (seat == null)
                return null;

            ValidateSeat(request.SeatNumber, request.SeatType, request.Price);

            if (!await _screenRepository.ExistsAsync(request.ScreenId))
                throw new ArgumentException("Screen does not exist.");

            seat.ScreenId = request.ScreenId;
            seat.SeatNumber = request.SeatNumber.Trim();
            seat.SeatType = request.SeatType.Trim();
            seat.Price = request.Price;

            await _seatRepository.UpdateAsync(seat);

            return MapToResponse(seat);
        }

        private static void ValidateSeat(
            string seatNumber, string seatType, decimal price)
        {
            if (string.IsNullOrWhiteSpace(seatNumber))
                throw new ArgumentException("Seat number is required.");

            if (seatNumber.Trim().Length > 10)
                throw new ArgumentException("Seat number cannot exceed 10 characters.");

            if (string.IsNullOrWhiteSpace(seatType))
                throw new ArgumentException("Seat type is required.");

            if (seatType.Trim().Length > 30)
                throw new ArgumentException("Seat type cannot exceed 30 characters.");

            if (price <= 0)
                throw new ArgumentException("Price must be greater than zero.");
        }

        private static SeatResponse MapToResponse(Seat seat)
        {
            return new SeatResponse
            {
                SeatId = seat.SeatId,
                ScreenId = seat.ScreenId,
                SeatNumber = seat.SeatNumber,
                SeatType = seat.SeatType,
                Price = seat.Price
            };
        }
    }
}
