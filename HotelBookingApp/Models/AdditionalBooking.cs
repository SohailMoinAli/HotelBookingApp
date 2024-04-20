using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBookingApp.Models
{
    public class AdditionalBooking
    {
        [Key]
        public int AdditionalBookingId { get; set; }

        [ForeignKey("Reservation")]
        public int ReservationId { get; set; }
        public Reservation Reservation { get; set; }

        public bool DinnerReservation { get; set; }
        public bool TourBooking { get; set; }
        public bool GolfPackage { get; set; }
    }
}
