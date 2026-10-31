
namespace MovieTicketBooking.DTOs.Reviews
{
    public class ReviewResponse
    {
        public int ReviewId { get; set; }

        public int MovieId { get; set; }

        public string MovieTitle { get; set; } = string.Empty;

        public string CustomerName { get; set; } = string.Empty;

        public int Rating { get; set; }

        public string Comment { get; set; } = string.Empty;

        public DateTime ReviewDate { get; set; }
    }
}
