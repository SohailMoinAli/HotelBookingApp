namespace HotelBookingApp.Models
{
    public class ConfirmationViewModel
    {
        public int ReservationId { get; set; }
        public string CustomerName { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int NumberOfNights { get; set; }
        public decimal TotalPrice { get; set; }
        public List<RoomDetailViewModel> Rooms { get; set; }

        public ConfirmationViewModel()
        {
            Rooms = new List<RoomDetailViewModel>();
        }
    }
}
