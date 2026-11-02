
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MovieTicketBooking.Data;
using MovieTicketBooking.Repositories;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services;
using MovieTicketBooking.Services.Interfaces;
using System.Text;

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

// Seat Repository
builder.Services.AddScoped<ISeatRepository, SeatRepository>();

// Seat Service
builder.Services.AddScoped<ISeatService, SeatService>();

// Show Repository and Service
builder.Services.AddScoped<IShowRepository, ShowRepository>();
builder.Services.AddScoped<IShowService, ShowService>();

// Booking Repository and Service
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IBookingService, BookingService>();

// Review Repository and Service
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IReviewService, ReviewService>();

// User Repository
builder.Services.AddScoped<IUserRepository, UserRepository>();

// User Service
builder.Services.AddScoped<
    MovieBookingSystem.Services.Interfaces.IUserService,
    MovieBookingSystem.Services.UserService>();

// JWT Service
builder.Services.AddScoped<
    MovieBookingSystem.Services.Interfaces.IJwtService,
    MovieBookingSystem.Services.JwtService>();

// JWT Authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");

var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
        JwtBearerDefaults.AuthenticationScheme;

    options.DefaultChallengeScheme =
        JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],

        IssuerSigningKey = new SymmetricSecurityKey(key),

        ClockSkew = TimeSpan.Zero
    };
});

// Authorization
builder.Services.AddAuthorization();
builder.Services.AddScoped<IPasswordHasher<MovieBookingSystem.Models.User>,
    PasswordHasher<MovieBookingSystem.Models.User>>();

var app = builder.Build();
//await RoleSeeder.SeedAsync(
  //  app.Services,
    //app.Configuration);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Authentication must come before Authorization
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
