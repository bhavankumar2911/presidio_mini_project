using HotelBookingSystemAPI.Exceptions.Review;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Models.DTOs.ReviewDTOs;
using HotelBookingSystemAPI.Repository.Interfaces;
using HotelBookingSystemAPI.Services.Interfaces;

namespace HotelBookingSystemAPI.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IRepository<int, Review> _reviewRepository;
        private readonly IRepository<int, Guest> _guestRepository;
        private readonly IRepository<int, Booking> _bookingRepository;

        public ReviewService (IRepository<int, Review> reviewRepository, IRepository<int, Guest> guestRepository, IRepository<int, Booking> bookingRepository)
        {
            _reviewRepository = reviewRepository;
            _guestRepository = guestRepository;
            _bookingRepository = bookingRepository;
        }

        private async Task CheckIfGuestBookedTheHotel (int guestId, int hotelId)
        {
            IEnumerable<Booking> bookings = await _bookingRepository.GetAll();

            if (!bookings.Any(b => b.Guest.Id == guestId && b.Room.Hotel.Id == hotelId)) throw new GuestNotBookedException();
        }

        public async Task ReviewAHotel(int guestId, ReviewInputDTO reviewInputDTO)
        {
            await CheckIfGuestBookedTheHotel(guestId, reviewInputDTO.HotelId);

            Review review = new Review
            {
                Content = reviewInputDTO.Content,
                HotelId = reviewInputDTO.HotelId,
                GuestId = guestId,
            };

            await _reviewRepository.Add(review);
        }
    }
}
