
namespace MovieTicketBooking.DTOs.Reviews
{
    public class UpdateReviewRequest
    {
        public string CustomerName { get; set; } = string.Empty;

        public int Rating { get; set; }

        public string Comment { get; set; } = string.Empty;
    }
}
