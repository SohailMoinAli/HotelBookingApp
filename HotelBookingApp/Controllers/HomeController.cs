using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class HomeController : Controller
{
    private readonly HotelDbContext _context;

    public HomeController(HotelDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        // Test database connection
        bool dbConnected = _context.Database.CanConnect();

        ViewData["DbConnected"] = dbConnected ? "Database Connection: Successful" : "Database Connection: Failed";
        return View();
    }
}
