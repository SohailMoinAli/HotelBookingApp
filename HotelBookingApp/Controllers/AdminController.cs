using HotelBookingApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

public class AdminController : Controller
{
    private readonly HotelDbContext _context;
    private readonly ILogger<AdminController> _logger;

    public AdminController(HotelDbContext context, ILogger<AdminController> logger)
    {
        _context = context;
        _logger = logger;
    }

    public IActionResult Home()
    {
        return View();
    }
    public async Task<IActionResult> ListOffers()
    {
        var offers = await _context.SpecialOffers.ToListAsync();
        return View(offers);
    }

    public IActionResult CreateOffer()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateOffer(SpecialOffer offer)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("CreateOffer: ModelState is invalid.");
            return View(offer);
        }

        try
        {
            _context.SpecialOffers.Add(offer);
            await _context.SaveChangesAsync();
            _logger.LogInformation("CreateOffer: New offer added successfully.");
            return RedirectToAction(nameof(ListOffers));
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "CreateOffer: Error adding offer to database.");
            ModelState.AddModelError("", "An error occurred saving the offer to the database.");
            return View(offer);
        }
    }

    public async Task<IActionResult> EditOffer(int id)
    {
        var offer = await _context.SpecialOffers.FindAsync(id);
        if (offer == null)
        {
            _logger.LogWarning($"EditOffer: Offer with ID {id} not found.");
            return NotFound();
        }
        return View(offer);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditOffer(int id, SpecialOffer offer)
    {
        if (id != offer.Id)
        {
            _logger.LogWarning("EditOffer: Offer ID mismatch.");
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("EditOffer: ModelState is invalid.");
            return View(offer);
        }

        try
        {
            _context.Update(offer);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"EditOffer: Offer with ID {id} updated successfully.");
            return RedirectToAction(nameof(ListOffers));
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, $"EditOffer: Error updating offer with ID {id}.");
            ModelState.AddModelError("", "An error occurred updating the offer.");
            return View(offer);
        }
    }

    public async Task<IActionResult> DeleteOffer(int id)
    {
        var offer = await _context.SpecialOffers.FindAsync(id);
        if (offer == null)
        {
            _logger.LogWarning($"DeleteOffer: Offer with ID {id} not found.");
            return NotFound();
        }

        try
        {
            _context.SpecialOffers.Remove(offer);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"DeleteOffer: Offer with ID {id} removed successfully.");
            return RedirectToAction(nameof(ListOffers));
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, $"DeleteOffer: Error removing offer with ID {id}.");
            return View("Error", new ErrorViewModel { Message = "An error occurred removing the offer." });
        }
    }
}
