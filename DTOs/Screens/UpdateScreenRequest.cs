
namespace MovieTicketBooking.DTOs.Screens
{
    public class UpdateScreenRequest
    {
        public int TheatreId { get; set; }

        public string ScreenName { get; set; } = string.Empty;

        public int TotalSeats { get; set; }

        public string ScreenType { get; set; } = string.Empty;
    }
}
