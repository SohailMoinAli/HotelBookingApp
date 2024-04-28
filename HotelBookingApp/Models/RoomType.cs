using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelBookingApp.Models
{
    public class RoomType
    {
        [Key]
        public int RoomTypeId { get; set; }
        [Required]
        [StringLength(50)]
        public string Name {  get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal BasePrice { get; set; }

        [Required]
        public int Capacity { get; set; }
        public virtual ICollection<Room> Rooms { get; set; }
        public virtual ICollection<RoomTypeImage> Images { get; set; }

        public RoomType()
        {
            Rooms = new HashSet<Room> ();
            Images = new HashSet<RoomTypeImage> ();
        }
    }
}

