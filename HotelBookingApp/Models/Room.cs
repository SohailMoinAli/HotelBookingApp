using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBookingApp.Models
{
    public class Room
    {
        [Key]
        public int RoomId { get; set; }
        [Required]
        public string RoomNumber { get; set; }

        public string? Type { get; set; }
        [ForeignKey("RoomType")]
        public int RoomTypeId { get; set; }
        [Required]
        public string Status { get; set; }
        public virtual RoomType RoomType { get; set; }

        public virtual ICollection<RoomReservation> RoomReservations { get; set; }

    }
}
