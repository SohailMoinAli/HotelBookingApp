using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBookingApp.Models
{
    public class Room
    {
        public int RoomId { get; set; }
        public string Number { get; set; }
        public string Type { get; set; }
        public bool IsAvailable { get; set; }

        [NotMapped]
        public DateTime? CheckInDate { get; set; }
        [NotMapped]
        public DateTime? CheckOutDate { get; set; }
    }
}
    