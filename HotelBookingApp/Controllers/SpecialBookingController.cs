using Microsoft.AspNetCore.Mvc;
using HotelBookingApp.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

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

    // POST: Handle the booking submission
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> BookSpecial(SpecialBooking booking)
    {
        _logger.LogInformation("Received booking attempt with SpecialOfferId: {SpecialOfferId}, BookingDate: {BookingDate}, NumberOfGuests: {NumberOfGuests}",
                               booking.SpecialOfferId, booking.BookingDate, booking.NumberOfGuests);

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Model state is invalid. Error details follow:");
            foreach (var entry in ModelState)
            {
                foreach (var error in entry.Value.Errors)
                {
                    _logger.LogWarning("{FieldName} - Error: {ErrorMessage}", entry.Key, error.ErrorMessage);
                }
            }
            return View(booking);
        }

        _context.SpecialBookings.Add(booking);
        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Booking saved successfully with ID: {BookingId}", booking.Id);
            return RedirectToAction("ConfirmationSpecial", new { id = booking.Id });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving booking to the database.");
            ModelState.AddModelError("", "An error occurred while saving the booking.");
            return View(booking);
        }
    }

    // GET: Show confirmation details after booking
    public async Task<IActionResult> ConfirmationSpecial(int id)
    {
        var booking = await _context.SpecialBookings.FindAsync(id);
        if (booking == null)
        {
            _logger.LogWarning("Booking with ID {BookingId} not found for confirmation.", id);
            return NotFound($"Booking with ID {id} not found.");
        }

        return View(booking);
    }
}
