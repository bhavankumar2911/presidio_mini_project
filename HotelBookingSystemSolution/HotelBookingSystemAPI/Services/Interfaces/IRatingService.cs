using HotelBookingSystemAPI.Models.DTOs.RatingDTOs;
using HotelBookingSystemAPI.Models.DTOs.ReviewDTOs;

namespace HotelBookingSystemAPI.Services.Interfaces
{
    public interface IRatingService
    {
        public Task RateAHotel(int guestId, RatingInputDTO ratingInputDTO);
    }
}
