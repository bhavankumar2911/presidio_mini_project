using HotelBookingSystemAPI.Exceptions.Review;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Models.DTOs.RatingDTOs;
using HotelBookingSystemAPI.Models.DTOs.ReviewDTOs;
using HotelBookingSystemAPI.Repository;
using HotelBookingSystemAPI.Repository.Interfaces;
using HotelBookingSystemAPI.Services.Interfaces;

namespace HotelBookingSystemAPI.Services
{
    public class RatingService : IRatingService
    {
        private readonly IRepository<int, Rating> _ratingRepository;
        private readonly IRepository<int, Booking> _bookingRepository;
        private readonly IRepository<int, Hotel> _hotelRepository;

        public RatingService(IRepository<int, Rating> ratingRepository, IRepository<int, Booking> bookingRepository, IRepository<int, Hotel> hotelRepository)
        {
            _bookingRepository = bookingRepository;
            _ratingRepository = ratingRepository;
            _hotelRepository = hotelRepository;
        }

        public async Task CheckIfGuestBookedTheHotel(int guestId, int hotelId)
        {
            IEnumerable<Booking> bookings = await _bookingRepository.GetAll();

            if (!bookings.Any(b => b.Guest.Id == guestId && b.Room.Hotel.Id == hotelId)) throw new GuestNotBookedException();
        }

        private float CalculateHotelRating (ICollection<Rating> ratings)
        {
            float oneStar = 0;
            float twoStar = 0;
            float threeStar = 0;
            float fourStar = 0;
            float fiveStar = 0;

            foreach (var rating in ratings)
            {
                switch (rating.StarRating)
                {
                    case 1: oneStar += 1; break;
                    case 2: twoStar += 1; break;
                    case 3: threeStar += 1; break;
                    case 4: fourStar += 1; break;
                    case 5: fiveStar += 1; break;
                }
            }
            
            return (oneStar + (2 * twoStar) + (3 * threeStar) + (4 * fourStar) + (5 * fiveStar)) / ratings.Count;
        }

        private async Task<Rating> CheckIfAlreadyRatedTheHotel(int guestId, int hotelId)
        {
            IEnumerable<Rating> ratings = await _ratingRepository.GetAll();
            ratings = ratings.Where(r => r.HotelId == hotelId && r.GuestId == guestId);

            if (ratings.Any())
                return ratings.First();

            return null;
        }

        public async Task RateAHotel(int guestId, RatingInputDTO ratingInputDTO)
        {
            Hotel hotel = await _hotelRepository.GetByKey(ratingInputDTO.HotelId);

            await CheckIfGuestBookedTheHotel(guestId, ratingInputDTO.HotelId);

            Rating rating = await CheckIfAlreadyRatedTheHotel(guestId, hotel.Id);

            if (rating != null)
            {
                rating.StarRating = ratingInputDTO.StarRating;
                await _ratingRepository.Update(rating);
                hotel.Ratings.Add(rating);
            }
            else
            {
                Rating newRating = new Rating
                {
                    StarRating = ratingInputDTO.StarRating,
                    HotelId = ratingInputDTO.HotelId,
                    GuestId = guestId,
                };

                await _ratingRepository.Add(newRating);
                hotel.Ratings.Add(newRating);
            }

            hotel.StarRating = CalculateHotelRating(hotel.Ratings);
            await _hotelRepository.Update(hotel);
        }
    }
}
