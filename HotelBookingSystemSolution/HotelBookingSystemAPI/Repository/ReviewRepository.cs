using HotelBookingSystemAPI.Context;
using HotelBookingSystemAPI.Exceptions.Review;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystemAPI.Repository
{
    public class ReviewRepository : IRepository<int, Review>
    {
        private readonly HotelBookingSystemContext _context;

        public ReviewRepository(HotelBookingSystemContext context)
        {
            _context = context;
        }

        async public Task<Review> Add(Review review)
        {
            _context.Add(review);
            await _context.SaveChangesAsync();
            return review;
        }

        async public Task<Review> Delete(int key)
        {
            var review = await GetByKey(key);

            if (review != null)
            {
                _context.Remove(review);
                await _context.SaveChangesAsync(true);
                return review;
            }

            throw new ReviewNotFoundException(key);
        }

        async public Task<IEnumerable<Review>> GetAll()
        {
            var reviews = await _context.Reviews.ToListAsync();

            //if (reviews.Count == 0) throw new NoReviewsFoundException();

            return reviews;
        }

        public async Task<Review> GetByKey(int key)
        {
            var review = await _context.Reviews.FirstOrDefaultAsync(e => e.Id == key);

            if (review != null) return review;

            throw new ReviewNotFoundException(key);
        }

        async public Task<Review> Update(Review newReview)
        {
            var review = await GetByKey(newReview.Id);

            if (review != null)
            {
                _context.Update(newReview);
                await _context.SaveChangesAsync(true);
                return newReview;
            }

            throw new ReviewNotFoundException(newReview.Id);
        }

    }
}
