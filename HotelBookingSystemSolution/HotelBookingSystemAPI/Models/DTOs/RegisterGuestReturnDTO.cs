namespace HotelBookingSystemAPI.Models.DTOs
{
    public class RegisterGuestReturnDTO : RegisterGuestInputBaseDTO
    {
        public int Id { get; set; }
        public string Role { get; set; } = string.Empty;
        public bool IsBlocked { get; set; } = false;
    }
}
