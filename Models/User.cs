
using MovieTicketBooking.Models;
using System.ComponentModel.DataAnnotations;

namespace MovieBookingSystem.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [MaxLength(30)]
        public string Role { get; set; } = "Customer";

        public ICollection<Booking> Bookings { get; set; }
            = new List<Booking>();
    }
}
