using HotelBookingSystemAPI.Models;

namespace HotelBookingSystemAPI.Models.DTOs.Hotel
{
    public class ListHotelsDTO
    {
        public IEnumerable<Models.Hotel>? Hotels { get; set; }
    }
}
