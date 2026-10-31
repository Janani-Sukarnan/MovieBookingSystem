
using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;
using MovieTicketBooking.Repositories;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services;
using MovieTicketBooking.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddOpenApi();

// Database connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Movie Repository
builder.Services.AddScoped<IMovieRepository, MovieRepository>();

// Movie Service
builder.Services.AddScoped<IMovieService, MovieService>();
// Theatre Repository
builder.Services.AddScoped<ITheatreRepository, TheatreRepository>();

// Theatre Service
builder.Services.AddScoped<ITheatreService, TheatreService>();
// Screen Repository
builder.Services.AddScoped<IScreenRepository, ScreenRepository>();

// Screen Service
builder.Services.AddScoped<IScreenService, ScreenService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
