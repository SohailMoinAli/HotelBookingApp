using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotelBookingApp.Models
{
    public class RoomReservation
    {
        [Key]
        public int RoomReservationId { get; set; }

        [ForeignKey("Room")]
        public int RoomId { get; set; }
        public virtual Room Room { get; set; }

        [ForeignKey("Reservation")]
        public int ReservationId { get; set; }
        public virtual Reservation Reservation { get; set; }
    }

}
