using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Models.DTOs.BookingDTOs;

namespace HotelBookingSystemAPI.Services.Interfaces
{
    public interface IBookingService
    {
        public Task<Booking> BookRoom(BookingInputDTO bookingInputDTO, int bookingGuestId);

        public Task<IEnumerable<Booking>> ViewGuestBookings(int guestId);

        public Task<IEnumerable<Booking>> ViewHotelBookings(int hotelId);
    }
}
