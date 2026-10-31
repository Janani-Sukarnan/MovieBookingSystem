
namespace MovieTicketBooking.DTOs.Reviews
{
    public class CreateReviewRequest
    {
        public int MovieId { get; set; }

        public string CustomerName { get; set; } = string.Empty;

        public int Rating { get; set; }

        public string Comment { get; set; } = string.Empty;
    }
}
