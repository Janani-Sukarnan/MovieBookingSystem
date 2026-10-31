
namespace MovieTicketBooking.DTOs.Bookings
{
    public class CreateBookingRequest
    {
        public int ShowId { get; set; }

        public string CustomerName { get; set; } = string.Empty;

        public string CustomerEmail { get; set; } = string.Empty;

        public string ContactNumber { get; set; } = string.Empty;

        public List<int> ShowSeatIds { get; set; } = new();
    }
}
