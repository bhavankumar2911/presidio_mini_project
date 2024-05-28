using System.ComponentModel.DataAnnotations;

namespace HotelBookingSystemAPI.Models.DTOs.RoomDTOs
{
    public class RoomInputDTO
    {
        [Required(ErrorMessage = "Room price per day is required")]
        public double PricePerDay { get; set; }

        [Required(ErrorMessage = "Room size is required")]
        public RoomSize Size { get; set; }

        [Required(ErrorMessage = "Room number is required")]
        public int RoomNumber { get; set; }

        [Required(ErrorMessage = "Floor number is required")]
        public int FloorNumber { get; set; }

        [Required(ErrorMessage = "Room availability is required")]
        public bool IsAvailable { get; set; }
    }
}
