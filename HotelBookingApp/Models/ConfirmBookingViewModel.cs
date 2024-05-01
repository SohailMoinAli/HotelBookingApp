namespace HotelBookingApp.Models
{
    public class ConfirmBookingViewModel
    {
        public string CustomerName { get; set; }  // To display the customer's name on the confirmation page
        public DateTime CheckInDate { get; set; } // Display check-in date
        public DateTime CheckOutDate { get; set; } // Display check-out date
        public int NumberOfNights { get; set; } // Total nights of stay
        public decimal GrandTotal { get; set; } // Total cost of the reservation

        // Detailed list of each room type reserved, if you need to display a breakdown
        public List<RoomDetailViewModel> Rooms { get; set; }

        // Additional field for payment status if needed
        public string Status { get; set; }
        public List<RoomReservationDetailViewModel> ReservationDetails { get; internal set; }

        public ConfirmBookingViewModel()
        {
            Rooms = new List<RoomDetailViewModel>();
        }
    }


}
