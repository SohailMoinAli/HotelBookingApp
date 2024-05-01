using System.Collections.Generic;

namespace HotelBookingApp.Models
{
    public class AvailableRoomTypesViewModel
    {
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int NumberOfGuests { get; set; }

        // A list of available room types with details
        public List<RoomTypeAvailabilityViewModel> RoomTypes { get; set; }
        public int NumberOfNights
        {
            get
            {
                TimeSpan duration = CheckOutDate - CheckInDate;
                return duration.Days;
            }
            set { } // Remove the private setter to allow external assignment
        }
    }
}
