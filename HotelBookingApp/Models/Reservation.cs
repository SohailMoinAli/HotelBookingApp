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

        [Required]
        [StringLength(100, ErrorMessage = "Customer name must be under 100 characters.")]
        public string CustomerName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime CheckInDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime CheckOutDate { get; set; }

        public int NumberOfRooms { get; set; }
        public int NumberOfGuests { get; set; }

        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = "Pending";
        
        public virtual ICollection<RoomReservation> RoomReservations { get; set; }
        public List<AdditionalBooking>? AdditionalBookings { get; set; }

    }
}
