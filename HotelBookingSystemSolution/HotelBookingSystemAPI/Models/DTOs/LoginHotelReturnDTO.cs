namespace HotelBookingSystemAPI.Models.DTOs
{
    public class LoginHotelReturnDTO
    {
        public RegisterHotelReturnDTO? Hotel { get; set; }
        public string Token { get; set; } = string.Empty;
    }
}
