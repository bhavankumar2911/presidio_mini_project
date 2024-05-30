using System.ComponentModel.DataAnnotations;

namespace HotelBookingSystemAPI.Models.DTOs.ReviewDTOs
{
    public class ReviewInputDTO
    {
        [Required(ErrorMessage = "Empty review cannot be posted")]
        public string Content { get; set; }

        [Required(ErrorMessage = "Hotel id is required")]
        public int HotelId { get; set; }
    }
}
