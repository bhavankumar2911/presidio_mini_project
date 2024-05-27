using System.ComponentModel.DataAnnotations;

namespace HotelBookingSystemAPI.Models.DTOs
{
    public class RegisterHotelInputDTO : RegisterHotelInputBaseDTO
    {
        [Required(ErrorMessage = "Password is required")]
        public string PlainTextPassword { get; set; } = string.Empty;
    }
}
