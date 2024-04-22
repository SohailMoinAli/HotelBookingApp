using Microsoft.AspNetCore.Mvc;
using System.Linq;
using HotelBookingApp.Models;

public class MapsController : Controller
{
    private readonly HotelDbContext _context;  // Change DbContext to HotelDbContext

    public MapsController(HotelDbContext context)  // Constructor should take HotelDbContext
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetHotels()
    {
        var hotels = _context.Hotels.Select(h => new
        {
            Name = h.Name,
            Latitude = h.Latitude,
            Longitude = h.Longitude,
            Description = h.Description
        }).ToList();

        return Json(hotels);
    }
}
