namespace HotelBookingSystemAPI.Models.DTOs
{
    public class RegisterHotelReturnDTO : RegisterHotelInputBaseDTO
    {
        public int Id { get; set; }
        public bool IsApproved { get; set; } = false;
    }
}
