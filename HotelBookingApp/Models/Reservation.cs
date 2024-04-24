using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBookingApp.Models
{
    public class Reservation
    {
        [Key]
        public int ReservationId { get; set; }

        [ForeignKey("Room")]
        public int RoomId { get; set; }
        public Room Room { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Customer name must be under 100 characters.")]
        public string CustomerName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime CheckInDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime CheckOutDate { get; set; }

        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = "Pending";
        public List<AdditionalBooking>? AdditionalBookings { get; set; }

    }
}
