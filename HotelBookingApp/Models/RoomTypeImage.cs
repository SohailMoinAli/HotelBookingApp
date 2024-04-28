using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotelBookingApp.Models
{
    public class RoomTypeImage
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("RoomType")]
        public int RoomTypeId { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }

        [StringLength(200)]
        public string? Description { get; set; }

        public int? Order { get; set; }  

        public virtual RoomType RoomType { get; set; }
    }
}
