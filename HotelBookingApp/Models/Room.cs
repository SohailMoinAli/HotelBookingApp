using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBookingApp.Models
{
    public class Room
    {
        [Key]
        public int RoomId { get; set; }
        public string Number { get; set; }
        public string Type { get; set; }
        public bool IsAvailable { get; set; }

        [NotMapped]
        public DateTime? CheckInDate { get; set; }
        [NotMapped]
        public DateTime? CheckOutDate { get; set; }

        // Adding a price property to be stored in the database
        public decimal Price { get; set; }
    }
}
