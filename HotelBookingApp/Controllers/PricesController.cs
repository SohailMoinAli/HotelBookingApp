using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelBookingApp.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging; // Make sure you have logging

//public class PricesController : Controller
//{
//    private readonly HotelDbContext _context;
//    private readonly ILogger<PricesController> _logger; // Add logger

//    public PricesController(HotelDbContext context, ILogger<PricesController> logger)
//    {
//        _context = context;
//        _logger = logger;
//    }

//    // GET: Prices/Edit
//    public async Task<IActionResult> Edit()
//    {
//        var rooms = await _context.Rooms.ToListAsync();
//        var additionalServices = await _context.AdditionalBookings.ToListAsync();
//        var model = new PricesViewModel
//        {
//            Rooms = rooms ?? new List<Room>(),  // Ensure list is not null
//            AdditionalServicesList = additionalServices ?? new List<AdditionalBooking>()  // Ensure list is not null
//        };
//        return View(model);
//    }

//    // POST: Prices/Edit
//    [HttpPost]
//    [ValidateAntiForgeryToken]
//    public async Task<IActionResult> Edit(PricesViewModel model)
//    {
//        if (ModelState.IsValid)
//        {
//            _context.ChangeTracker.Clear(); // Clear the change tracker to avoid any tracking issues

//            foreach (var room in model.Rooms)
//            {
//                var dbRoom = await _context.Rooms.FindAsync(room.RoomId);
//                if (dbRoom != null)
//                {
//                    dbRoom.Price = room.Price;
//                    _context.Entry(dbRoom).State = EntityState.Modified; // Explicitly mark as modified
//                }
//                else
//                {
//                    _logger.LogWarning($"Room not found with ID {room.RoomId}");
//                }
//            }

//            foreach (var service in model.AdditionalServicesList)
//            {
//                var dbService = await _context.AdditionalBookings.FindAsync(service.AdditionalBookingId);
//                if (dbService != null)
//                {
//                    dbService.DinnerPrice = service.DinnerPrice;
//                    dbService.TourPrice = service.TourPrice;
//                    dbService.GolfPackagePrice = service.GolfPackagePrice;
//                    _context.Entry(dbService).State = EntityState.Modified; // Explicitly mark as modified
//                }
//                else
//                {
//                    _logger.LogWarning($"Service not found with ID {service.AdditionalBookingId}");
//                }
//            }

//            try
//            {
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateException ex)
//            {
//                _logger.LogError($"Database update failed: {ex.Message}");
//                ModelState.AddModelError("", "Unable to save changes. Check the log for more information.");
//                return View(model);
//            }

//            return RedirectToAction(nameof(Edit));
//        }
//        else
//        {
//            var errorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage));
//            foreach (var errorMessage in errorMessages)
//            {
//                _logger.LogError($"Model state error: {errorMessage}");
//            }
//        }
//        return View(model);
//    }
//}
