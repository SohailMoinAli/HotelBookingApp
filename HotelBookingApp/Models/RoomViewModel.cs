// Models/RoomViewModel.cs
namespace HotelBookingApp.Models
{
    public class RoomViewModel
    {
        public int RoomId { get; set; }
        public string Number { get; set; }
        public string Type { get; set; }
        public bool IsAvailable { get; set; }
        public string AvailabilityStatus => IsAvailable ? "Available" : "Booked";
    }
}
