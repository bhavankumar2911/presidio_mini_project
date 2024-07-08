using HotelBookingSystemAPI.Models.DTOs.PaymentDTOs;
using HotelBookingSystemAPI.Services.Interfaces;
using Razorpay.Api;

namespace HotelBookingSystemAPI.Services
{
    public class PaymentService : IPaymentService
    {
        public string GetPaymentOrderId(double price)
        {
            string key = "rzp_test_RhAepz4Wv2n7V7";
            string secret = "MWtn74btPDkF31be3Xp1Rfi9";

            Dictionary<string, object> input = new Dictionary<string, object>();
            input.Add("amount", (int)price * 100);
            input.Add("currency", "INR");
            input.Add("receipt", Guid.NewGuid().ToString());

            RazorpayClient client = new RazorpayClient(key, secret);

            Razorpay.Api.Order order = client.Order.Create(input);
            string orderId = order["id"].ToString();

            return orderId;
        }

        public void VerifyPayment(PaymentVerifyInputDTO paymentVerifyInputDTO)
        {
            RazorpayClient client = new RazorpayClient("rzp_test_RhAepz4Wv2n7V7", "MWtn74btPDkF31be3Xp1Rfi9");

            Dictionary<string, string> attributes = new Dictionary<string, string>();

            attributes.Add("razorpay_payment_id", paymentVerifyInputDTO.PaymentId);
            attributes.Add("razorpay_order_id", paymentVerifyInputDTO.OrderId);
            attributes.Add("razorpay_signature", paymentVerifyInputDTO.Signature);

            Utils.verifyPaymentSignature(attributes);
        }
    }
}
