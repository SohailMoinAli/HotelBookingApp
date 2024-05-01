namespace HotelBookingApp.Models
{
    public class RoomReservationDetailViewModel
    {
        public int RoomTypeId { get; set; }
        public string RoomTypeName { get; set; }
        public decimal PricePerNight { get; set; }
        public int NumberOfRooms { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
