using System;

namespace HotelBookingApp.Models
{
    public class RoomTypeViewModel
    {
        public int RoomTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal BasePrice { get; set; }
        public int Capacity { get; set; }
        public List<IFormFile> NewImages { get; set; }
        public List<int> ImagesToDelete { get; set; } // IDs of images to delete
        public List <RoomTypeImageViewModel> Images{ get; set; } // Existing images
    public RoomTypeViewModel()
        {
            NewImages = new List<IFormFile>();
            ImagesToDelete = new List<int>();
            Images = new List<RoomTypeImageViewModel>(); 
        }
    }
}

