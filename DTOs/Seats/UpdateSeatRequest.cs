
namespace MovieTicketBooking.DTOs.Seats
{
    public class UpdateSeatRequest
    {
        public int ScreenId { get; set; }

        public string SeatNumber { get; set; } = string.Empty;

        public string SeatType { get; set; } = string.Empty;

        public decimal Price { get; set; }
    }
}
