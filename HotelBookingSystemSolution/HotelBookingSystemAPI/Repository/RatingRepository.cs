using HotelBookingSystemAPI.Context;
using HotelBookingSystemAPI.Exceptions.Rating;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystemAPI.Repository
{
    public class RatingRepository : IRepository<int, Rating>
    {
        private readonly HotelBookingSystemContext _context;

        public RatingRepository(HotelBookingSystemContext context)
        {
            _context = context;
        }

        async public Task<Rating> Add(Rating rating)
        {
            _context.Add(rating);
            await _context.SaveChangesAsync();
            return rating;
        }

        async public Task<Rating> Delete(int key)
        {
            var rating = await GetByKey(key);

            if (rating != null)
            {
                _context.Remove(rating);
                await _context.SaveChangesAsync(true);
                return rating;
            }

            throw new RatingNotFoundException();
        }

        async public Task<IEnumerable<Rating>> GetAll()
        {
            var ratings = await _context.Ratings.ToListAsync();

            //if (ratings.Count == 0) throw new NoRatingsFoundException();

            return ratings;
        }

        public async Task<Rating> GetByKey(int key)
        {
            var rating = await _context.Ratings.FirstOrDefaultAsync(e => e.Id == key);

            if (rating != null) return rating;

            throw new RatingNotFoundException();
        }

        async public Task<Rating> Update(Rating newRating)
        {
            var rating = await GetByKey(newRating.Id);

            if (rating != null)
            {
                _context.Update(newRating);
                await _context.SaveChangesAsync(true);
                return newRating;
            }

            throw new RatingNotFoundException();
        }
    }

}
