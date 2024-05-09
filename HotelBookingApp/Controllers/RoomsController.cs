using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelBookingApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering; // Ensure this matches your namespace

public class RoomsController : Controller
{
    private readonly HotelDbContext _context;

    public RoomsController(HotelDbContext context)
    {
        _context = context;
    }

    // Lists all rooms
    public async Task<IActionResult> Index()
    {
        var roomList = await _context.Rooms
            .Select(room => new RoomViewModel
            {
                RoomId = room.RoomId,
                RoomNumber = room.RoomNumber,
                RoomType = room.RoomType,
                Status = room.Status
            })
            .ToListAsync();

        return View(roomList);
    }


    // Shows form to create a new room
    public IActionResult Create()
    {
        ViewBag.RoomTypeList = new SelectList(_context.RoomTypes, "RoomTypeId", "Name");
        ViewBag.StatusList = new SelectList(new List<string>() { "Available", "Booked", "Under-maintenance" });
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Room room)
    {
        ModelState.Remove("RoomType");
        ModelState.Remove("RoomReservations");
        if (ModelState.IsValid)
        {
            _context.Add(room);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Room created successfully.";
            return RedirectToAction("Index");
        }

        ViewBag.RoomTypeList = new SelectList(_context.RoomTypes, "RoomTypeId", "Name", room.RoomTypeId);
        ViewBag.StatusList = new SelectList(new List<string>() { "Available", "Booked", "Under-maintenance" }, room.Status);

        return View(room);
    }


    // Shows form to edit a room
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var room = await _context.Rooms.FindAsync(id);
        if (room == null)
        {
            return NotFound();
        }

        ViewBag.RoomTypeList = new SelectList(_context.RoomTypes, "RoomTypeId", "Name", room.RoomTypeId);
        ViewBag.StatusList = new SelectList(new List<string>() { "Available", "Booked", "Under-maintenance" }, room.Status);

        return View(room);
    }

    // Handles room updates
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Room room)
    {
        ModelState.Remove("RoomType");
        ModelState.Remove("RoomReservations");
        if (!ModelState.IsValid)
        {
            ViewBag.RoomTypeList = new SelectList(_context.RoomTypes, "RoomTypeId", "Name", room.RoomTypeId);
            ViewBag.StatusList = new SelectList(new List<string>() { "Available", "Booked", "Under-maintenance" }, room.Status);
            return View(room);
        }

        var roomToUpdate = await _context.Rooms.FindAsync(room.RoomId);
        if (roomToUpdate == null)
        {
            return NotFound();
        }

        roomToUpdate.RoomNumber = room.RoomNumber;
        roomToUpdate.RoomTypeId = room.RoomTypeId; // Ensuring the foreign key is updated
        roomToUpdate.Status = room.Status;

        _context.Update(roomToUpdate);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public ActionResult Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        var roomFromDb = _context.Rooms
        .Include(r => r.RoomType) 
        .FirstOrDefault(rt => rt.RoomId == id);


        if (roomFromDb == null)
        {
            return NotFound();
        }
        var room = new Room
        {
            RoomId = roomFromDb.RoomId,
            RoomNumber=roomFromDb.RoomNumber,
            Status = roomFromDb.Status,
            RoomTypeId = roomFromDb.RoomTypeId,
            RoomType = roomFromDb.RoomType
        };

        return View(room);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult DeletePOST(int? id)
    {
        var obj = _context.Rooms.Find(id);
        if (obj == null)
        {
            return NotFound();
        }

        _context.Rooms.Remove(obj);
        _context.SaveChanges();
        TempData["SuccessMessage"] = "Room deleted successfully";
        return RedirectToAction("Index");
    }

    //public async Task<IActionResult> RoomIndex()
    //{
    //    var availableRooms = await _context.Rooms.Where(r => r.IsAvailable).ToListAsync();
    //    return View("RoomIndex", availableRooms);
    //}
    //public async Task<IActionResult> BookARoom(int roomId)
    //{
    //    var room = await _context.Rooms.FindAsync(roomId);
    //    if (room == null)
    //    {
    //        return NotFound("Room not found.");
    //    }

    //    if (!room.IsAvailable)
    //    {
    //        return BadRequest("Room is not available for booking.");
    //    }

    //    return View(room);
    //}
}

//    // Displays available rooms to book
//    [HttpPost]
//    [ValidateAntiForgeryToken]
//    public async Task<IActionResult> BookARoom(int roomId, DateTime checkInDate, DateTime checkOutDate)
//    {
//        Console.WriteLine($"Received roomId: {roomId}, checkInDate: {checkInDate}, checkOutDate: {checkOutDate}");
//        var room = await _context.Rooms.FirstOrDefaultAsync(r => r.RoomId == roomId && r.IsAvailable);
//        if (room == null)
//        {
//            return NotFound("Room not available.");
//        }

//        // Check ModelState.IsValid
//        if (!ModelState.IsValid)
//        {
//            // Log ModelState errors to diagnose validation issues
//            Console.WriteLine("ModelState is invalid:");
//            foreach (var modelState in ModelState.Values)
//            {
//                foreach (var error in modelState.Errors)
//                {
//                    Console.WriteLine(error.ErrorMessage);
//                }
//            }

//            return BadRequest(ModelState);
//        }

//        // Calculate the number of nights and total price
//        int totalNights = (checkOutDate - checkInDate).Days;
//        if (totalNights <= 0)
//        {
//            totalNights = 0;  // Ensure totalNights is not negative or zero
//        }
//        decimal totalPrice = totalNights * room.Price;

//        var customerName = User.Identity.IsAuthenticated ? User.Identity.Name : "Guest Customer";

//        // Create a reservation object to pass to the confirmation view
//        var reservation = new
//        {
//            RoomId = roomId,
//            RoomNumber = room.RoomNumber,
//            RoomType = room.RoomType,
//            CheckInDate = checkInDate,
//            CheckOutDate = checkOutDate,
//            TotalNights = totalNights,
//            TotalPrice = totalPrice,
//            CustomerName = customerName
//        };

//        // Pass reservation details to the confirmation view
//        return View("ConfirmBooking", reservation);
//    }

//    // Shows form for additional reservations
//    public async Task<IActionResult> AdditionalReservations(int reservationId)
//    {
//        var reservation = await _context.Reservations.Include(r => r.Room)
//            .Include(r => r.AdditionalBookings) // Make sure this includes AdditionalBookings
//            .FirstOrDefaultAsync(r => r.ReservationId == reservationId);

//        if (reservation == null)
//        {
//            return NotFound("Reservation not found.");
//        }

//        // Initialize additional bookings if none exist
//        if (!reservation.AdditionalBookings.Any())
//        {
//            reservation.AdditionalBookings.Add(new AdditionalBooking
//            {
//                DinnerReservation = false,
//                TourBooking = false,
//                GolfPackage = false
//            });
//            _context.SaveChanges();  // Save changes if we modify the data
//        }

//        return View(reservation);
//    }

//    [HttpPost]
//    [ValidateAntiForgeryToken]
//    public async Task<IActionResult> AdditionalReservations(Reservation reservationModel)
//    {
//        var reservation = await _context.Reservations
//            .Include(r => r.AdditionalBookings)
//            .FirstOrDefaultAsync(r => r.ReservationId == reservationModel.ReservationId);

//        if (reservation == null)
//        {
//            return NotFound("Reservation not found.");
//        }

//        // Update additional bookings based on the form submission
//        foreach (var booking in reservation.AdditionalBookings)
//        {
//            var formBooking = reservationModel.AdditionalBookings.FirstOrDefault(ab => ab.AdditionalBookingId == booking.AdditionalBookingId);
//            if (formBooking != null)
//            {
//                booking.DinnerReservation = formBooking.DinnerReservation;
//                booking.TourBooking = formBooking.TourBooking;
//                booking.GolfPackage = formBooking.GolfPackage;
//            }
//        }

//        await _context.SaveChangesAsync();

//        return RedirectToAction("BookingConfirmation", new { reservationId = reservation.ReservationId });
//    }

//    // Final booking confirmation after selecting additional services
//    public async Task<IActionResult> BookingConfirmation(int reservationId)
//    {
//        var reservation = await _context.Reservations
//            .Include(r => r.Room)
//            .Include(r => r.AdditionalBookings) // Ensure this relation is handled in DbContext
//            .FirstOrDefaultAsync(r => r.ReservationId == reservationId);

//        if (reservation == null)
//        {
//            return NotFound("The reservation could not be found.");
//        }

//        return View(reservation);
//    }

//    [HttpPost]
//    [ValidateAntiForgeryToken]
//    public async Task<IActionResult> ProceedToCheckout(int roomId, DateTime checkInDate, DateTime checkOutDate, decimal totalPrice, string customerName)
//    {
//        var room = await _context.Rooms.FirstOrDefaultAsync(r => r.RoomId == roomId);
//        if (room == null || !room.IsAvailable)
//        {
//            return NotFound("Room not available.");
//        }

//        // Mark the room as unavailable
//        room.IsAvailable = false;

//        var reservation = new Reservation
//        {
//            RoomId = roomId,
//            CheckInDate = checkInDate,
//            CheckOutDate = checkOutDate,
//            CustomerName = customerName,
//            TotalPrice = totalPrice,
//            Status = "Pending Payment"
//        };

//        _context.Reservations.Add(reservation);
//        await _context.SaveChangesAsync();

//        // Redirect to a dummy checkout view with the reservation ID
//        return RedirectToAction("Checkout", new { reservationId = reservation.ReservationId });
//    }

//    [HttpPost]
//    [ValidateAntiForgeryToken]
//    public async Task<IActionResult> FinalizeBooking(int reservationId)
//    {
//        var reservation = await _context.Reservations.FirstOrDefaultAsync(r => r.ReservationId == reservationId);
//        if (reservation == null)
//        {
//            return NotFound("Reservation not found.");
//        }

//        // Finalize the reservation
//        reservation.Status = "Confirmed";
//        await _context.SaveChangesAsync();

//        // Redirect to a confirmation view or similar
//        return RedirectToAction("ReservationComplete", new { reservationId = reservation.ReservationId });
//    }

//    public async Task<IActionResult> ReservationComplete(int reservationId)
//    {
//        var reservation = await _context.Reservations
//            .Include(r => r.Room) // Load room information if needed for display
//            .FirstOrDefaultAsync(r => r.ReservationId == reservationId);

//        if (reservation == null)
//        {
//            return NotFound("The reservation could not be found.");
//        }

//        // Ensure the reservation is actually confirmed before showing the complete view
//        if (reservation.Status != "Confirmed")
//        {
//            return RedirectToAction("Error", new { message = "Reservation is not confirmed yet." });
//        }

//        return View(reservation);
//    }

//    [HttpGet]
//    public async Task<IActionResult> Checkout(int reservationId)
//    {
//        var reservation = await _context.Reservations
//            .Include(r => r.Room)
//            .FirstOrDefaultAsync(r => r.ReservationId == reservationId);

//        if (reservation == null)
//        {
//            return NotFound("Reservation not found.");
//        }

//        return View(reservation);  // Make sure you have a Checkout.cshtml view ready in the correct view folder
//    }

//}
