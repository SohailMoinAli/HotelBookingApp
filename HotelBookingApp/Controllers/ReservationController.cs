using HotelBookingApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization;

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
                        .Where(room => room.Status == "Available" &&
                                        !room.RoomReservations.Any(res =>
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
            var availableRoomTypes = JsonConvert.DeserializeObject<List<RoomTypeAvailabilityViewModel>>(availableRoomTypesSerialized);

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

        [HttpPost]
        public IActionResult BookRooms(AvailableRoomTypesViewModel model)
        {
            int numberOfNights = Convert.ToInt32(Request.Form["NumberOfNights"]);
            model.NumberOfNights = numberOfNights;

            if (!ModelState.IsValid)
            {
                return View("ShowAvailableRooms", model);
            }

            var reservationDetails = new List<RoomReservationDetailViewModel>();
            foreach (var roomType in model.RoomTypes.Where(rt => rt.SelectedRooms > 0))
            {
                var detail = new RoomReservationDetailViewModel
                {
                    RoomTypeId = roomType.RoomTypeId,
                    RoomTypeName = roomType.RoomTypeName,
                    NumberOfRooms = roomType.SelectedRooms,
                    PricePerNight = roomType.PricePerNight,
                    TotalPrice = roomType.SelectedRooms * roomType.PricePerNight
                };
                reservationDetails.Add(detail);
            }

            decimal grandTotal = reservationDetails.Sum(rd => rd.TotalPrice * numberOfNights);

            TempData["ReservationDetails"] = JsonConvert.SerializeObject(reservationDetails);
            TempData["NumberOfNights"] = numberOfNights.ToString();
            TempData["GrandTotal"] = grandTotal.ToString("F2");
            TempData["CheckInDate"] = model.CheckInDate.ToString("dd/MM/yyyy");
            TempData["CheckOutDate"] = model.CheckOutDate.ToString("dd/MM/yyyy");
            TempData["TotalGuests"] = model.NumberOfGuests.ToString();

            return RedirectToAction("ConfirmBooking");
        }


        public IActionResult ConfirmBooking()
        {
            var reservationDetailsJson = TempData.Peek("ReservationDetails") as string; // Use Peek here
            var reservationDetails = JsonConvert.DeserializeObject<List<RoomReservationDetailViewModel>>(reservationDetailsJson);

            if (reservationDetails == null)
            {
                return RedirectToAction("Index");
            }

            var numberOfNights = Convert.ToInt32(TempData.Peek("NumberOfNights")); // Use Peek here
            var grandTotal = Convert.ToDecimal(TempData.Peek("GrandTotal")); // Use Peek here

            var viewModel = new ConfirmBookingViewModel
            {
                ReservationDetails = reservationDetails,
                NumberOfNights = numberOfNights,
                GrandTotal = grandTotal
            };

            return View(viewModel);
        }

        public List<int> GetAvailableRoomIds(int roomTypeId, DateTime checkInDate, DateTime checkOutDate)
        {
            // Fetch all rooms of the specified type that are available for the entire duration
            var availableRoomIds = _context.Rooms
                .Where(r => r.RoomTypeId == roomTypeId && r.Status == "Available")
                .Select(r => r.RoomId)
                .ToList();

            // Fetch reserved room IDs that conflict with the desired dates
            var reservedRoomIds = _context.RoomReservations
                .Where(rr => availableRoomIds.Contains(rr.RoomId) &&
                             rr.Reservation.CheckInDate < checkOutDate &&
                             rr.Reservation.CheckOutDate > checkInDate)
                .Select(rr => rr.RoomId)
                .Distinct()
                .ToList();

            // Exclude reserved rooms from the available room IDs
            availableRoomIds = availableRoomIds.Except(reservedRoomIds).ToList();

            return availableRoomIds;
        }

        [HttpPost]
        public IActionResult ProceedToCheckout()
        {

            if (TempData["ReservationDetails"] is not string reservationDetailsJson)
            {
                return RedirectToAction("Index");
            }
            var reservationDetails = JsonConvert.DeserializeObject<List<RoomReservationDetailViewModel>>(reservationDetailsJson);
            if (reservationDetails == null || !DateTime.TryParse(TempData["CheckInDate"]?.ToString(), out var checkInDate) ||
                !DateTime.TryParse(TempData["CheckOutDate"]?.ToString(), out var checkOutDate) ||
                !int.TryParse(TempData["TotalGuests"]?.ToString(), out var totalGuests))
            {
                // Log the error or handle it as needed
                return RedirectToAction("Index"); // Redirect to start if critical TempData is missing
            }

            var numberOfNights = Convert.ToInt32(TempData["NumberOfNights"]);
            TempData["NumberOfNights"] = numberOfNights;  // Re-store the data for future use
            var grandTotal = Convert.ToDecimal(TempData["GrandTotal"]);

            var reservation = new Reservation
            {
                CustomerName = "Guest User", // Assuming the user is logged in
                CheckInDate = checkInDate,
                CheckOutDate = checkOutDate,
                NumberOfRooms = reservationDetails.Count,
                NumberOfGuests = totalGuests,
                TotalPrice = grandTotal,
                Status = "Awaiting Payment"
            };

            _context.Reservations.Add(reservation);
            _context.SaveChanges(); // Save to get the ReservationId

            foreach (var detail in reservationDetails)
            {
                var availableRoomIds = GetAvailableRoomIds(detail.RoomTypeId, checkInDate, checkOutDate);
                if (availableRoomIds.Count < detail.NumberOfRooms)
                {
                    TempData["BookingError"] = "Not enough rooms available.";
                    return RedirectToAction("BookingError");
                }
                for (int i = 0; i < detail.NumberOfRooms; i++)
                {
                    var roomReservation = new RoomReservation
                    {
                        RoomId = availableRoomIds[i],
                        ReservationId = reservation.ReservationId
                    };
                    _context.RoomReservations.Add(roomReservation);
                }
            }

            _context.SaveChanges();
            return RedirectToAction("Confirmation", new { reservationId = reservation.ReservationId });
        }


        public IActionResult Confirmation(int reservationId)
        {
            var reservation = _context.Reservations
                .Include(r => r.RoomReservations)
                    .ThenInclude(rr => rr.Room)
                        .ThenInclude(room => room.RoomType) // Ensure RoomType is loaded
                .FirstOrDefault(r => r.ReservationId == reservationId);

            if (reservation == null)
            {
                return RedirectToAction("Error"); // Redirect to an error page if the reservation is not found
            }

            var numberOfNights = Convert.ToInt32(TempData["NumberOfNights"]);

            var viewModel = new ConfirmationViewModel
            {
                ReservationId = reservationId,
                CustomerName = reservation.CustomerName,
                CheckInDate = reservation.CheckInDate,
                CheckOutDate = reservation.CheckOutDate,
                NumberOfNights = numberOfNights,
                TotalPrice = reservation.TotalPrice,
                Rooms = reservation.RoomReservations.GroupBy(rr => rr.Room.RoomType)
                    .Select(group => new RoomDetailViewModel
                    {
                        RoomTypeName = group.Key.Name,
                        NumberOfRooms = group.Count(), // Count the number of room reservations for this room type
                        PricePerNight = group.Key.BasePrice
                    }).ToList()
            };

            return View(viewModel);
        }

    }
}

