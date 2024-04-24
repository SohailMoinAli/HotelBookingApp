using HotelBookingApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class FeedbackController : Controller
{
    private readonly HotelDbContext _context; // Change this to use HotelDbContext

    public FeedbackController(HotelDbContext context) // Update constructor parameter
    {
        _context = context;
    }

    // GET: Feedback/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Feedback/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,Email,Message")] Feedback feedback)
    {
        if (ModelState.IsValid)
        {
            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Thanks)); // Redirect to a thank you page
        }
        return View(feedback);
    }

    // Thanks page after submitting feedback
    public IActionResult Thanks()
    {
        return View();
    }

    // GET: Feedback/AdminView
    public async Task<IActionResult> AdminView()
    {
        return View(await _context.Feedbacks.ToListAsync());
    }
}
