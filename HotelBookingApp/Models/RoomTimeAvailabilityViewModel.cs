namespace HotelBookingApp.Models
{
    public class RoomTypeAvailabilityViewModel
    {
        public int RoomTypeId { get; set; }
        public string RoomTypeName { get; set; }
        public int AvailableCount { get; set; } 
        public decimal PricePerNight { get; set; }
        public string Description { get; set; }
        public int SelectedRooms { get; set; }
        public List<string> ImageUrls { get; set; } // List of URLs to images of the room type

        // Constructor to initialize the ImageUrls list
        public RoomTypeAvailabilityViewModel()
        {
            ImageUrls = new List<string>();
        }
    }
}
