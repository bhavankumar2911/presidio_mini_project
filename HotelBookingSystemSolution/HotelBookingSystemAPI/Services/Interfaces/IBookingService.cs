using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Models.DTOs.BookingDTOs;
using HotelBookingSystemAPI.Models.DTOs.PaymentDTOs;

namespace HotelBookingSystemAPI.Services.Interfaces
{
    public interface IBookingService
    {
        public Task<Booking> BookRoom(BookingInputDTO bookingInputDTO, int bookingGuestId);

        public Task<IEnumerable<Booking>> ViewGuestBookings(int guestId);

        public Task<IEnumerable<Booking>> ViewHotelBookings(int hotelId);

        public Task<AmountReturnDTO> CalculateBookingAmount(BookingInputDTO bookingInputDTO);

        public Task<PaymentOrderIdReturnDTO> GivePaymentOrderId(BookingInputDTO bookingInputDTO);
    }
}
