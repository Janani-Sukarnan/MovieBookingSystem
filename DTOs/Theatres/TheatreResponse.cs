
namespace MovieTicketBooking.DTOs.Theatres
{
    public class TheatreResponse
    {
        public int TheatreId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string ContactNumber { get; set; } = string.Empty;
    }
}
