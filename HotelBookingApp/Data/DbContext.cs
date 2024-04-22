using HotelBookingApp.Models;
using Microsoft.EntityFrameworkCore;

public class HotelDbContext : DbContext
{
    public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options) { }

    public DbSet<Room> Rooms { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<AdditionalBooking> AdditionalBookings { get; set; }
    public DbSet<Hotel> Hotels { get; set; }  // Ensure this line is correctly included
}
