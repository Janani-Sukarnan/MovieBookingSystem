
using MovieTicketBooking.DTOs.Theatres;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;
using MovieTicketBooking.DTOs.Common;
namespace MovieTicketBooking.Services
{
    public class TheatreService : ITheatreService
    {
        private readonly ITheatreRepository _theatreRepository;

        public TheatreService(ITheatreRepository theatreRepository)
        {
            _theatreRepository = theatreRepository;
        }

        public async Task<PagedResponse<TheatreResponse>> GetAllAsync(
    int pageNumber,
    int pageSize,
    string? name,
    string? city)
        {
            if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
                throw new ArgumentException("Invalid pagination values.");

            var totalRecords = await _theatreRepository.GetTotalTheatresAsync(
                name, city);

            var theatres = await _theatreRepository.GetPagedAsync(
                pageNumber, pageSize, name, city);

            var totalPages = (int)Math.Ceiling(
                (double)totalRecords / pageSize);

            return new PagedResponse<TheatreResponse>
            {
                Items = theatres.Select(t => MapToResponse(t)).ToList(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = totalPages
            };
        }

        public async Task<TheatreResponse?> GetByIdAsync(int id)
        {
            var theatre = await _theatreRepository.GetByIdAsync(id);

            if (theatre == null)
                return null;

            return MapToResponse(theatre);
        }

        public async Task<TheatreResponse> CreateAsync(CreateTheatreRequest request)
        {
            ValidateTheatre(
                request.Name, request.Address, request.City, request.ContactNumber);
           

            var theatre = new Theatre
            {
                Name = request.Name.Trim(),
                Address = request.Address.Trim(),
                City = request.City.Trim(),
                ContactNumber = request.ContactNumber.Trim()
            };

            var createdTheatre = await _theatreRepository.AddAsync(theatre);

            return MapToResponse(createdTheatre);
        }

        public async Task<TheatreResponse?> UpdateAsync(
            int id, UpdateTheatreRequest request)
        {
            var theatre = await _theatreRepository.GetByIdAsync(id);


            if (theatre == null)
                return null;

            ValidateTheatre(
                request.Name, request.Address, request.City, request.ContactNumber);

            theatre.Name = request.Name.Trim();
            theatre.Address = request.Address.Trim();
            theatre.City = request.City.Trim();
            theatre.ContactNumber = request.ContactNumber.Trim();

            await _theatreRepository.UpdateAsync(theatre);

            return MapToResponse(theatre);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var theatre = await _theatreRepository.GetByIdAsync(id);

            if (theatre == null)
                return false;

            await _theatreRepository.DeleteAsync(theatre);

            return true;
        }

        private static void ValidateTheatre(
            string name, string address, string city, string contactNumber)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Theatre name is required.");

            if (name.Trim().Length > 150)
                throw new ArgumentException("Theatre name cannot exceed 150 characters.");

            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentException("Address is required.");

            if (address.Trim().Length > 250)
                throw new ArgumentException("Address cannot exceed 250 characters.");

            if (string.IsNullOrWhiteSpace(city))
                throw new ArgumentException("City is required.");

            if (city.Trim().Length > 100)
                throw new ArgumentException("City cannot exceed 100 characters.");

            if (string.IsNullOrWhiteSpace(contactNumber))
                throw new ArgumentException("Contact number is required.");

            if (contactNumber.Trim().Length > 20)
                throw new ArgumentException("Contact number cannot exceed 20 characters.");
        }

        private static TheatreResponse MapToResponse(Theatre theatre)
        {
            return new TheatreResponse
            {
                TheatreId = theatre.TheatreId,
                Name = theatre.Name,
                Address = theatre.Address,
                City = theatre.City,
                ContactNumber = theatre.ContactNumber
            };
        }
    }
}
