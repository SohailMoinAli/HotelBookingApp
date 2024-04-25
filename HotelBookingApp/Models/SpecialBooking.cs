using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBookingApp.Models
{
    public class SpecialBooking
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Special Offer")]
        public int SpecialOfferId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Booking Date")]
        public DateTime BookingDate { get; set; }

        [Display(Name = "Number of Guests")]
        public int NumberOfGuests { get; set; }

        // Navigation property for related SpecialOffer
        [ForeignKey("SpecialOfferId")]
        public virtual SpecialOffer SpecialOffer { get; set; }
    }


}
