namespace HotelBookingApp.Models
{
    public class RoomDetailViewModel
    {
        public string RoomTypeName { get; set; } // Type of the room booked
        public int NumberOfRooms { get; set; } // Number of rooms booked of this type
        public decimal PricePerNight { get; set; } // Price per night for this type of room
        public decimal TotalPrice { get; set; } // Total price for the rooms of this type
    }
}
