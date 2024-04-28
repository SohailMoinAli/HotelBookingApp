using HotelBookingApp.Models;
using Microsoft.EntityFrameworkCore;

public class HotelDbContext : DbContext
{
    public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options) { }

    public DbSet<Room> Rooms { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<AdditionalBooking> AdditionalBookings { get; set; }
    public DbSet<Hotel> Hotels { get; set; }  
    public DbSet<Feedback> Feedbacks { get; set; }

    public DbSet<SpecialOffer> SpecialOffers { get; set; }
    public DbSet<SpecialBooking> SpecialBookings { get; set; }
    public DbSet<RoomType> RoomTypes { get; set; }
    public DbSet<RoomTypeImage> RoomTypeImages { get; set; }
    public DbSet<RoomReservation> RoomReservations { get; set; }
}
