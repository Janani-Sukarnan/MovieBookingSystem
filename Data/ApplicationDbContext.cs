
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



        }
    }
}
