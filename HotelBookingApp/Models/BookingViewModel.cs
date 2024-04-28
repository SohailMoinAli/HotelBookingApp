using System.ComponentModel.DataAnnotations;

namespace HotelBookingApp.Models
{
    public class BookingViewModel
    {
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Check-in Date")]
        public DateTime CheckInDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Check-out Date")]
        public DateTime CheckOutDate { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a valid number of guests.")]
        [Display(Name = "Number of Guests")]
        public int NumberOfGuests { get; set; }
    }
}
