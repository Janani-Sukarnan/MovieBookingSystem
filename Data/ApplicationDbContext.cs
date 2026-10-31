
using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Movie> Movies => Set<Movie>();
        public DbSet<Theatre> Theatres => Set<Theatre>();
        public DbSet<Screen> Screens => Set<Screen>();
        public DbSet<Seat> Seats => Set<Seat>();
        public DbSet<Show> Shows => Set<Show>();
        public DbSet<ShowSeat> ShowSeats => Set<ShowSeat>();
        public DbSet<Booking> Bookings => Set<Booking>();
        public DbSet<BookingSeat> BookingSeats => Set<BookingSeat>();
        public DbSet<Review> Reviews => Set<Review>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.HasKey(m => m.MovieId);

                entity.Property(m => m.Title)
                    .HasMaxLength(150)
                    .IsRequired();

                entity.Property(m => m.Description)
                    .HasMaxLength(500);

                entity.Property(m => m.DurationMinutes)
                    .IsRequired();

                entity.Property(m => m.ReleaseDate)
                    .HasColumnType("date")
                    .IsRequired();

                entity.Property(m => m.Language)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(m => m.Genre)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.ToTable(t => t.HasCheckConstraint(
                    "CK_Movies_DurationMinutes",
                    "[DurationMinutes] > 0"));
            });

            modelBuilder.Entity<Theatre>(entity =>
            {
                entity.HasKey(t => t.TheatreId);

                entity.Property(t => t.Name)
                    .HasMaxLength(150)
                    .IsRequired();

                entity.Property(t => t.Address)
                    .HasMaxLength(250)
                    .IsRequired();

                entity.Property(t => t.City)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(t => t.ContactNumber)
                    .HasMaxLength(20)
                    .IsRequired();
            });

            modelBuilder.Entity<Screen>(entity =>
            {
                entity.HasKey(s => s.ScreenId);

                entity.Property(s => s.ScreenName)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(s => s.TotalSeats)
                    .IsRequired();

                entity.Property(s => s.ScreenType)
                    .HasMaxLength(30)
                    .IsRequired();

                entity.HasOne(s => s.Theatre)
                    .WithMany()
                    .HasForeignKey(s => s.TheatreId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.ToTable(t => t.HasCheckConstraint(
                    "CK_Screens_TotalSeats",
                    "[TotalSeats] > 0"));
            });

            modelBuilder.Entity<Seat>(entity =>
            {
                entity.HasKey(s => s.SeatId);

                entity.Property(s => s.SeatNumber)
                    .HasMaxLength(10)
                    .IsRequired();

                entity.Property(s => s.SeatType)
                    .HasMaxLength(30)
                    .IsRequired();

                entity.Property(s => s.Price)
                    .HasColumnType("decimal(10,2)")
                    .IsRequired();

                entity.HasOne(s => s.Screen)
                    .WithMany()
                    .HasForeignKey(s => s.ScreenId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.ToTable(t => t.HasCheckConstraint(
                    "CK_Seats_Price",
                    "[Price] > 0"));
            });

            modelBuilder.Entity<Show>(entity =>
            {
                entity.HasKey(s => s.ShowId);

                entity.Property(s => s.StartTime).IsRequired();

                entity.Property(s => s.EndTime).IsRequired();

                entity.Property(s => s.TicketPrice)
                    .HasColumnType("decimal(10,2)")
                    .IsRequired();

                entity.HasOne(s => s.Movie)
                    .WithMany()
                    .HasForeignKey(s => s.MovieId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(s => s.Screen)
                    .WithMany()
                    .HasForeignKey(s => s.ScreenId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.ToTable(t =>
                {
                    t.HasCheckConstraint(
                        "CK_Shows_EndTime",
                        "[EndTime] > [StartTime]");

                    t.HasCheckConstraint(
                        "CK_Shows_TicketPrice",
                        "[TicketPrice] > 0");
                });
            });

            modelBuilder.Entity<ShowSeat>(entity =>
            {
                entity.HasKey(ss => ss.ShowSeatId);

                entity.HasOne(ss => ss.Show)
                    .WithMany(s => s.ShowSeats)
                    .HasForeignKey(ss => ss.ShowId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ss => ss.Seat)
                    .WithMany()
                    .HasForeignKey(ss => ss.SeatId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(ss => new { ss.ShowId, ss.SeatId })
                    .IsUnique();
            });

            // Booking configuration
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(b => b.BookingId);

                entity.Property(b => b.CustomerName)
                    .HasMaxLength(150)
                    .IsRequired();

                entity.Property(b => b.CustomerEmail)
                    .HasMaxLength(150)
                    .IsRequired();

                entity.Property(b => b.ContactNumber)
                    .HasMaxLength(20)
                    .IsRequired();

                entity.Property(b => b.BookingDate)
                    .IsRequired();

                entity.Property(b => b.TotalAmount)
                    .HasColumnType("decimal(10,2)")
                    .IsRequired();

                entity.Property(b => b.Status)
                    .HasMaxLength(30)
                    .IsRequired();

                entity.HasMany(b => b.BookingSeats)
                    .WithOne(bs => bs.Booking)
                    .HasForeignKey(bs => bs.BookingId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.ToTable(t => t.HasCheckConstraint(
                    "CK_Bookings_TotalAmount",
                    "[TotalAmount] > 0"));
            });

            // BookingSeat configuration
            modelBuilder.Entity<BookingSeat>(entity =>
            {
                entity.HasKey(bs => bs.BookingSeatId);

                entity.Property(bs => bs.Price)
                    .HasColumnType("decimal(10,2)")
                    .IsRequired();

                entity.HasOne(bs => bs.ShowSeat)
                    .WithMany()
                    .HasForeignKey(bs => bs.ShowSeatId)
                    .OnDelete(DeleteBehavior.Restrict);

                // One show seat can belong to only one booking.
                entity.HasIndex(bs => bs.ShowSeatId)
                    .IsUnique();
            });
            modelBuilder.Entity<Review>(entity =>
            {
                entity.HasKey(r => r.ReviewId);

                entity.Property(r => r.CustomerName)
                    .HasMaxLength(150)
                    .IsRequired();

                entity.Property(r => r.Rating)
                    .IsRequired();

                entity.Property(r => r.Comment)
                    .HasMaxLength(1000)
                    .IsRequired();

                entity.Property(r => r.ReviewDate)
                    .IsRequired();

                entity.HasOne(r => r.Movie)
                    .WithMany()
                    .HasForeignKey(r => r.MovieId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.ToTable(t => t.HasCheckConstraint(
                    "CK_Reviews_Rating",
                    "[Rating] >= 1 AND [Rating] <= 5"));
            });
        }
    }
}
