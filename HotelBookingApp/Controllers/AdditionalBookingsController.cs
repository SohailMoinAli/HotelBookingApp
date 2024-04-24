using Microsoft.AspNetCore.Mvc;
using HotelBookingApp.Models;

public class AdditionalBookingsController : Controller
{
    private readonly HotelDbContext _context;

    public AdditionalBookingsController(HotelDbContext context)
    {
        _context = context;
    }

    public IActionResult Create(int reservationId)
    {
        var model = new AdditionalBooking { ReservationId = reservationId };
        return View(model);
    }

    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> Create(AdditionalBooking additionalBooking)
    //{
    //    if (!ModelState.IsValid)
    //    {
    //        ViewBag.ReservationId = additionalBooking.ReservationId;
    //        return View(additionalBooking);
    //    }

    //    _context.AdditionalBookings.Add(additionalBooking);
    //    await _context.SaveChangesAsync();
    //    return RedirectToAction("Confirmation", "Reservations", new { reservationId = additionalBooking.ReservationId });
    //}
}
