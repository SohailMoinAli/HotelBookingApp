using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelBookingApp.Models; // Ensure this matches your namespace

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
        var rooms = await _context.Rooms.Select(r => new RoomViewModel
        {
            RoomId = r.RoomId,
            Number = r.Number,
            Type = r.Type,
            IsAvailable = r.IsAvailable
        }).ToListAsync();
        return View("Index", rooms);
    }

    // Shows form to create a new room
    public IActionResult Create()
    {
        return View();
    }

    // Handles room creation
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Room room)
    {
        if (ModelState.IsValid)
        {
            _context.Add(room);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
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

        var roomViewModel = new RoomViewModel
        {
            RoomId = room.RoomId,
            Number = room.Number,
            Type = room.Type,
            Price = room.Price,
            IsAvailable = room.IsAvailable
        };
        return View(roomViewModel);
    }

    // Handles room updates
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, RoomViewModel model)
    {
        if (id != model.RoomId)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var room = await _context.Rooms.FindAsync(id);
        if (room == null)
        {
            return NotFound();
        }

        room.Number = model.Number;
        room.Type = model.Type;
        room.IsAvailable = model.IsAvailable;
        room.Price = model.Price;
        _context.Update(room);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // Displays available rooms to book
    public async Task<IActionResult> BookARoom()
    {
        var availableRooms = await _context.Rooms.Where(r => r.IsAvailable).ToListAsync();
        return View("BookARoom", availableRooms);
    }

    // Handles room booking and redirects to additional reservations
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> BookARoom(int roomId, DateTime checkInDate, DateTime checkOutDate)
    {
        var room = await _context.Rooms.FirstOrDefaultAsync(r => r.RoomId == roomId && r.IsAvailable);
        if (room == null)
        {
            return NotFound("Room not available.");
        }

        var customerName = User.Identity.IsAuthenticated ? User.Identity.Name : "Guest Customer";

        var reservation = new Reservation
        {
            RoomId = roomId,
            CheckInDate = checkInDate,
            CheckOutDate = checkOutDate,
            CustomerName = customerName,
            AdditionalBookings = new List<AdditionalBooking>()
        };

        _context.Reservations.Add(reservation);
        room.IsAvailable = false; // Optionally leave room as available until final confirmation if needed
        await _context.SaveChangesAsync();

        // Redirect to the page for additional reservations
        return RedirectToAction("AdditionalReservations", new { reservationId = reservation.ReservationId });
    }

    // Shows form for additional reservations
    public async Task<IActionResult> AdditionalReservations(int reservationId)
    {
        var reservation = await _context.Reservations.Include(r => r.Room)
            .Include(r => r.AdditionalBookings) // Make sure this includes AdditionalBookings
            .FirstOrDefaultAsync(r => r.ReservationId == reservationId);

        if (reservation == null)
        {
            return NotFound("Reservation not found.");
        }

        // Initialize additional bookings if none exist
        if (!reservation.AdditionalBookings.Any())
        {
            reservation.AdditionalBookings.Add(new AdditionalBooking
            {
                DinnerReservation = false,
                TourBooking = false,
                GolfPackage = false
            });
            _context.SaveChanges();  // Save changes if we modify the data
        }

        return View(reservation);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AdditionalReservations(Reservation reservationModel)
    {
        var reservation = await _context.Reservations
            .Include(r => r.AdditionalBookings)
            .FirstOrDefaultAsync(r => r.ReservationId == reservationModel.ReservationId);

        if (reservation == null)
        {
            return NotFound("Reservation not found.");
        }

        // Update additional bookings based on the form submission
        foreach (var booking in reservation.AdditionalBookings)
        {
            var formBooking = reservationModel.AdditionalBookings.FirstOrDefault(ab => ab.AdditionalBookingId == booking.AdditionalBookingId);
            if (formBooking != null)
            {
                booking.DinnerReservation = formBooking.DinnerReservation;
                booking.TourBooking = formBooking.TourBooking;
                booking.GolfPackage = formBooking.GolfPackage;
            }
        }

        await _context.SaveChangesAsync();

        return RedirectToAction("BookingConfirmation", new { reservationId = reservation.ReservationId });
    }

    // Final booking confirmation after selecting additional services
    public async Task<IActionResult> BookingConfirmation(int reservationId)
    {
        var reservation = await _context.Reservations
            .Include(r => r.Room)
            .Include(r => r.AdditionalBookings) // Ensure this relation is handled in DbContext
            .FirstOrDefaultAsync(r => r.ReservationId == reservationId);

        if (reservation == null)
        {
            return NotFound("The reservation could not be found.");
        }

        return View(reservation);
    }
}
