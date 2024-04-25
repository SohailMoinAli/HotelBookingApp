using HotelBookingApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

public class HomeController : Controller
{
    private readonly HotelDbContext _context;

    public HomeController(HotelDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        string dbStatusMessage;
        try
        {
            // Test database connection
            bool dbConnected = _context.Database.CanConnect();
            dbStatusMessage = dbConnected ? "Database Connection: Successful" : "Database Connection: Failed";
        }
        catch (Exception ex)
        {
            dbStatusMessage = $"Database Connection: Failed with exception {ex.Message}";
        }

        ViewData["DbConnected"] = dbStatusMessage;

        // Fetch special offers that have not expired
        var specialOffers = await _context.SpecialOffers
                                         .Where(o => o.ValidUntil > DateTime.Now)
                                         .ToListAsync();

        return View(specialOffers);
    }
}
