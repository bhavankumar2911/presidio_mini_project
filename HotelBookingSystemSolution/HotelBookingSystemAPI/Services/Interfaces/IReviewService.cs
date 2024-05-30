using HotelBookingSystemAPI.Models.DTOs.ReviewDTOs;

namespace HotelBookingSystemAPI.Services.Interfaces
{
    public interface IReviewService
    {
        public Task ReviewAHotel(int guestId, ReviewInputDTO reviewInputDTO);
    }
}
