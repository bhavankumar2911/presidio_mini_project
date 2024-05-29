using System.ComponentModel.DataAnnotations;

namespace HotelBookingSystemAPI.Models.DTOs.BookingGuestDTOs
{
    public class BookingGuestInputDTO
    {
        [Required(ErrorMessage = "Name of the guest is required.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Gender of the guest is required.")]
        public string Gender { get; set; } = string.Empty;

        [Required(ErrorMessage = "Age of the guest is required")]
        public double Age { get; set; }
    }
}
