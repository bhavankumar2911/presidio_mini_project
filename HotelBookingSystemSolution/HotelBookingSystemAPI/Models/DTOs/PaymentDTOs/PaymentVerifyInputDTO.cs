namespace HotelBookingSystemAPI.Models.DTOs.PaymentDTOs
{
    public class PaymentVerifyInputDTO
    {
        public string PaymentId { get; set; } = "";
        public string OrderId { get; set; } = "";
        public string Signature { get; set; } = "";
    }
}
