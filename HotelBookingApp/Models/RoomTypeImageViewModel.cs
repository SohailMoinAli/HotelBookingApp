namespace HotelBookingApp.Models
{
    public class RoomTypeImageViewModel
    {
     
            public int Id { get; set; }
            public string FileName { get; set; } // Assuming you store the file name
            public string ImageUrl { get; set; } // Full path might be used in some cases
        
    }
}
