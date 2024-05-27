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
            var hotels = await _context.Hotels.ToListAsync();

            //if (hotels.Count == 0) throw new NoHotelsFoundException();

            return hotels;
        }

        async public Task<IEnumerable<Hotel>> GetAllWithAddress()
        {
            var hotels = await _context.Hotels.Include(h => h.Address).ToListAsync();

            //if (hotels.Count == 0) throw new NoHotelsFoundException();

            return hotels;
        }

        public async Task<Hotel> GetByKey(int key)
        {
            var hotel = await _context.Hotels.FirstOrDefaultAsync(e => e.Id == key);

            if (hotel != null) return hotel;

            throw new HotelNotFoundException(key);
        }

        public async Task<Hotel> GetByKeyWithAddress(int key)
        {
            var hotel = await _context.Hotels.Include(h => h.Address).FirstOrDefaultAsync(e => e.Id == key);

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
