// Models/RoomViewModel.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBookingApp.Models
{
    public class RoomViewModel
    {
        public int RoomId { get; set; }
        [Required(ErrorMessage = "Room number is required.")]
        public string RoomNumber { get; set; }
        public int RoomTypeID { get; set; }
        public virtual RoomType? RoomType { get; set; }
        public string Status { get; set; }

    }
}
