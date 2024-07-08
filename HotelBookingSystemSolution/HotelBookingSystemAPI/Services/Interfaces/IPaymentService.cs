using HotelBookingSystemAPI.Controllers;
using HotelBookingSystemAPI.Models.DTOs.PaymentDTOs;

namespace HotelBookingSystemAPI.Services.Interfaces
{
    public interface IPaymentService
    {
        public string GetPaymentOrderId(double price);

        public void VerifyPayment(PaymentVerifyInputDTO paymentVerifyInputDTO);
    }
}
