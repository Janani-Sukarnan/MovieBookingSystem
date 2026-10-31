
namespace MovieTicketBooking.DTOs.Bookings
{
    public class BookingResponse
    {
        public int BookingId { get; set; }

        public string CustomerName { get; set; } = string.Empty;

        public string CustomerEmail { get; set; } = string.Empty;

        public string ContactNumber { get; set; } = string.Empty;

        public int ShowId { get; set; }

        public List<int> ShowSeatIds { get; set; } = new();

        public DateTime BookingDate { get; set; }

        public decimal TotalAmount { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}
