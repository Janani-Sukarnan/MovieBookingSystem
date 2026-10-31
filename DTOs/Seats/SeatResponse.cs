
namespace MovieTicketBooking.DTOs.Seats
{
    public class SeatResponse
    {
        public int SeatId { get; set; }

        public int ScreenId { get; set; }

        public string SeatNumber { get; set; } = string.Empty;

        public string SeatType { get; set; } = string.Empty;

        public decimal Price { get; set; }
    }
}
