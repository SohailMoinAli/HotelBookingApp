using Microsoft.AspNetCore.Mvc;
using System.Linq;
using HotelBookingApp.Models;

public class MapsController : Controller
{
    private readonly HotelDbContext _context;

    public MapsController(HotelDbContext context)
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
