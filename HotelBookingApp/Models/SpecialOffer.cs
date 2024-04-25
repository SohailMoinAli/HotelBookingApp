using System.ComponentModel.DataAnnotations;

namespace HotelBookingApp.Models
{
    public class SpecialOffer
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        [Required]
        [Range(1, 10000, ErrorMessage = "Price must be between 1 and 10,000.")]
        public decimal Price { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Valid from date is required.")]
        public DateTime ValidFrom { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Valid until date is required.")]
        public DateTime ValidUntil { get; set; }

        public bool IsActive { get; set; }
    }

}
