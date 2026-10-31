
namespace MovieTicketBooking.Models
{
    public class Review
    {
        public int ReviewId { get; set; }

        public int MovieId { get; set; }

        public string CustomerName { get; set; } = string.Empty;

        public int Rating { get; set; }

        public string Comment { get; set; } = string.Empty;

        public DateTime ReviewDate { get; set; }

        public Movie? Movie { get; set; }
    }
}
