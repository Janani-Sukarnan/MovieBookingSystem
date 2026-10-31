
namespace MovieTicketBooking.DTOs.Bookings
{
    public class UpdateBookingRequest
    {
        public string CustomerName { get; set; } = string.Empty;

        public string CustomerEmail { get; set; } = string.Empty;

        public string ContactNumber { get; set; } = string.Empty;
    }
}
