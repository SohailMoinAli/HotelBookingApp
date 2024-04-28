using HotelBookingApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace HotelBookingApp.Controllers
{
    public class ReservationController : Controller
    {
        private readonly HotelDbContext _context; 

        public ReservationController(HotelDbContext context)
        {
            _context = context;
        }

        // GET: Displays the booking form
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CheckAvailability(BookingViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Retrieve room types and their available room counts for the given dates
                var availableRoomTypes = _context.RoomTypes
                    .Select(rt => new RoomTypeAvailabilityViewModel
                    {
                        RoomTypeId = rt.RoomTypeId,
                        RoomTypeName = rt.Name,
                        AvailableCount = rt.Rooms
                            .Where(room => !room.RoomReservations.Any(res =>
                                res.Reservation.CheckInDate < model.CheckOutDate &&
                                res.Reservation.CheckOutDate > model.CheckInDate))
                            .Count(),
                        PricePerNight = rt.BasePrice,
                        Description = rt.Description,
                    }).ToList();


                if (availableRoomTypes.All(rt => rt.AvailableCount == 0))
                {
                    TempData["Error"] = "No rooms available for the selected dates.";
                    return View("Index", model);
                }

                // Pass available room types to the next step
                TempData["AvailableRoomTypesSerialized"] = JsonConvert.SerializeObject(availableRoomTypes);
                return RedirectToAction("ShowAvailableRooms", new { checkInDate = model.CheckInDate, checkOutDate = model.CheckOutDate, numberOfGuests = model.NumberOfGuests });
            }

            // If model state is not valid, return to the form with the existing model to display errors
            return View("Index", model);
        }

        // GET: Displays available rooms (Assuming room details are to be shown)
        public IActionResult ShowAvailableRooms(DateTime checkInDate, DateTime checkOutDate, int numberOfGuests)
        {
            // Retrieve available room IDs from TempData or session
            var availableRoomTypesSerialized = TempData["AvailableRoomTypesSerialized"] as string;

            if (string.IsNullOrEmpty(availableRoomTypesSerialized))
            {
                // Redirect to index or error page since TempData may be lost after redirect or session expiration
                return RedirectToAction("Index");
            }

            // Deserialize available room types
            var availableRoomTypes = JsonConvert.DeserializeObject < List<RoomTypeAvailabilityViewModel>> (availableRoomTypesSerialized);

            foreach (var roomType in availableRoomTypes)
            {
                var roomTypeImages = _context.RoomTypeImages
                    .Where(rt => rt.RoomTypeId == roomType.RoomTypeId)
                    .Select(rt => rt.ImageUrl)
                    .ToList();

                roomType.ImageUrls.AddRange(roomTypeImages);
            }

            // Create a view model to pass to the view
            var viewModel = new AvailableRoomTypesViewModel
            {
                CheckInDate = checkInDate,
                CheckOutDate = checkOutDate,
                NumberOfGuests = numberOfGuests,
                RoomTypes = availableRoomTypes
            };

            return View(viewModel);
        }
    }
}

