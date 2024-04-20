using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using HotelBookingApp.Models;

public class HotelDbContext : DbContext
{
    public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options)
    {
    }

    public DbSet<Room> Rooms { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<AdditionalBooking> AdditionalBookings { get; set; }
}
