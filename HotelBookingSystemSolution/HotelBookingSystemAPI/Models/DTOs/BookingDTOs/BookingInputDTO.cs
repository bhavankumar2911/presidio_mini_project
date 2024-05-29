using HotelBookingSystemAPI.Models.DTOs.BookingGuestDTOs;
using System.ComponentModel.DataAnnotations;

namespace HotelBookingSystemAPI.Models.DTOs.BookingDTOs
{
    public class BookingInputDTO
    {
        [Required(ErrorMessage = "Checkin date and time is required.")]
        public DateTime CheckinDateTime { get; set; }

        [Required(ErrorMessage = "Checkout date and time is required.")]
        public DateTime CheckoutDateTime { get; set; }

        [Required(ErrorMessage = "Room id is required for booking")]
        public int RoomID { get; set; }

        [Required(ErrorMessage = "Cannot book rooms without guests")]
        public IList<BookingGuestInputDTO> Guests { get; set; }
    }
}
