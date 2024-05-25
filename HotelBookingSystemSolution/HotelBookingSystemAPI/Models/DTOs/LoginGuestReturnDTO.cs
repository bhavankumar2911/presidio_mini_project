namespace HotelBookingSystemAPI.Models.DTOs
{
    public class LoginGuestReturnDTO
    {
        public RegisterGuestReturnDTO? User { get; set; }
        public string Token { get; set; } = string.Empty;
    }
}
