
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

        }
    }
}
