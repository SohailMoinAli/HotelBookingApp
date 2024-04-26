using Microsoft.AspNetCore.Mvc;
using HotelBookingApp.Models;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

public class SpecialBookingController : Controller
{
    private readonly HotelDbContext _context;
    private readonly ILogger<SpecialBookingController> _logger;

    public SpecialBookingController(HotelDbContext context, ILogger<SpecialBookingController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: Initialize the booking form with a valid offer
    [HttpGet]
    public async Task<IActionResult> BookSpecial(int offerId)
    {
        var offer = await _context.SpecialOffers.FindAsync(offerId);
        if (offer == null)
        {
            _logger.LogWarning("Special offer with ID {OfferId} not found.", offerId);
            return NotFound($"Special offer with ID {offerId} not found.");
        }

        var model = new SpecialBooking
        {
            SpecialOfferId = offerId,
            BookingDate = DateTime.Today // Default booking date to today, can adjust as needed
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> BookSpecial(SpecialBooking booking)
    {
        _logger.LogInformation("Booking attempt: SpecialOfferId={SpecialOfferId}, Date={BookingDate}, Guests={NumberOfGuests}",
            booking.SpecialOfferId, booking.BookingDate, booking.NumberOfGuests);

        // Ensure the SpecialOffer exists
        booking.SpecialOffer = await _context.SpecialOffers.FindAsync(booking.SpecialOfferId);
        if (booking.SpecialOffer == null)
        {
            ModelState.AddModelError("SpecialOfferId", "The specified special offer does not exist.");
        }

        // Remove the erroneous ModelState error manually if it's known to be incorrect
        ModelState.Remove("SpecialOffer");

        if (!ModelState.IsValid)
        {
            foreach (var entry in ModelState)
            {
                foreach (var error in entry.Value.Errors)
                {
                    _logger.LogWarning("Validation error - Field: {FieldName}, Error: {ErrorMessage}", entry.Key, error.ErrorMessage);
                }
            }
            return View(booking);
        }

        _context.SpecialBookings.Add(booking);
        try
        {
            await _context.SaveChangesAsync();
            return RedirectToAction("ConfirmationSpecial", new { id = booking.Id });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving booking.");
            ModelState.AddModelError("", "An error occurred while saving the booking.");
            return View(booking);
        }
    }


    // GET: Show confirmation details after booking
    public async Task<IActionResult> ConfirmationSpecial(int id)
    {
        var booking = await _context.SpecialBookings
            .Include(b => b.SpecialOffer) // Ensure the SpecialOffer data is loaded for display
            .FirstOrDefaultAsync(b => b.Id == id);

        if (booking == null)
        {
            _logger.LogWarning("Booking with ID {BookingId} not found for confirmation.", id);
            return NotFound($"Booking with ID {id} not found.");
        }

        return View(booking);
    }
}
