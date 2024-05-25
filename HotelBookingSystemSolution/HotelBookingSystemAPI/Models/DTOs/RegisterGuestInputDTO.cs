using System.ComponentModel.DataAnnotations;

namespace HotelBookingSystemAPI.Models.DTOs
{
    public class RegisterGuestInputDTO : RegisterGuestInputBaseDTO
    {
        [Required(ErrorMessage = "Password is required")]
        public string PlainTextPassword { get; set; } = string.Empty;
    }
}
