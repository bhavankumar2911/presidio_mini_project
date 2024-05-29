using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Models.DTOs.BookingDTOs;

namespace HotelBookingSystemAPI.Services.Interfaces
{
    public interface IBookingService
    {
        public Booking BookRoom(BookingInputDTO bookingInputDTO, int bookingGuestId);
    }
}
