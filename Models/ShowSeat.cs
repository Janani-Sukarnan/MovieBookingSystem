
namespace MovieTicketBooking.Models
{
    public class ShowSeat
    {
        public int ShowSeatId { get; set; }

        public int ShowId { get; set; }

        public int SeatId { get; set; }

        public bool IsBooked { get; set; }

        public Show? Show { get; set; }

        public Seat? Seat { get; set; }
    }
}
