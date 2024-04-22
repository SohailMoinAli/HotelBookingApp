using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelBookingApp.Models;
using System.Threading.Tasks;

public class PricesController : Controller
{
    private readonly HotelDbContext _context;

    public PricesController(HotelDbContext context)
    {
        _context = context;
    }

    // GET: Prices/Edit
    public async Task<IActionResult> Edit()
    {
        var rooms = await _context.Rooms.ToListAsync();
        var additionalServices = await _context.AdditionalBookings.ToListAsync();
        var model = new PricesViewModel
        {
            Rooms = rooms,
            AdditionalServicesList = additionalServices
        };
        return View(model);
    }

    // POST: Prices/Edit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(PricesViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Update Rooms
            _context.Rooms.UpdateRange(model.Rooms);

            // Update Additional Services
            _context.AdditionalBookings.UpdateRange(model.AdditionalServicesList);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Log the error (uncomment ex variable name and write a log.)
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.");
                return View(model);
            }

            return RedirectToAction(nameof(Edit));
        }
        return View(model);
    }
}
