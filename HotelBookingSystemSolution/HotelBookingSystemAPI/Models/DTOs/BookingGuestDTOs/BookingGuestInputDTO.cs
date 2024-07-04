using System.ComponentModel.DataAnnotations;

namespace HotelBookingSystemAPI.Models.DTOs.BookingGuestDTOs
{
    public class BookingGuestInputDTO
    {
        public string Name { get; set; } = string.Empty;

        public string Gender { get; set; } = string.Empty;

        public double Age { get; set; }
    }
}
