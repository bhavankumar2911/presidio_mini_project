using HotelBookingSystemAPI.Context;
using HotelBookingSystemAPI.Exceptions.Hotel;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystemAPI.Repository
{
    public class HotelRepository : IRepository<int, Hotel>
    {
        private readonly HotelBookingSystemContext _context;

        public HotelRepository(HotelBookingSystemContext context)
        {
            _context = context;
        }

        async public Task<Hotel> Add(Hotel hotel)
        {
            _context.Add(hotel);
            await _context.SaveChangesAsync();
            return hotel;
        }

        async public Task<Hotel> Delete(int key)
        {
            var hotel = await GetByKey(key);

            if (hotel != null)
            {
                _context.Remove(hotel);
                await _context.SaveChangesAsync(true);
                return hotel;
            }

            throw new HotelNotFoundException(key);
        }

        async public Task<IEnumerable<Hotel>> GetAll()
        {
            var hotels = await _context.Hotels.Select(h => new Hotel
            {
                Id = h.Id,
                Name = h.Name,
                IsApproved = h.IsApproved,
                Phone = h.Phone,
                Description = h.Description,
                Ratings = h.Ratings,
                StarRating = h.StarRating,
                UserId = h.UserId,
                AddressId = h.AddressId,
            }).ToListAsync();

            //if (hotels.Count == 0) throw new NoHotelsFoundException();

            return hotels;
        }

        public async Task<Hotel> GetByKey(int key)
        {
            var hotel = await _context.Hotels.FirstOrDefaultAsync(e => e.Id == key);

            if (hotel != null) return hotel;

            throw new HotelNotFoundException(key);
        }

        async public Task<Hotel> Update(Hotel newHotel)
        {
            var hotel = await GetByKey(newHotel.Id);

            if (hotel != null)
            {
                _context.Update(newHotel);
                await _context.SaveChangesAsync(true);
                return newHotel;
            }

            throw new HotelNotFoundException(newHotel.Id);
        }
    }

}
