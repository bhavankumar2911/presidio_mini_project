using HotelBookingSystemAPI.Context;
using HotelBookingSystemAPI.Exceptions.Hotel;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystemAPI.Repository
{
    public class HotelWithAddressRepository : HotelRepository, IHotelWithAddressRepository
    {
        private readonly HotelBookingSystemContext _context;

        public HotelWithAddressRepository(HotelBookingSystemContext context) : base(context) 
        {
            _context = context;
        }

        public async Task<IEnumerable<Hotel>> GetAllWithAddress()
        {
            var hotels = await _context.Hotels.Include(h => h.Address).ToListAsync();

            //if (hotels.Count == 0) throw new NoHotelsFoundException();

            return hotels;
        }

        public async Task<Hotel> GetByKeyWithAddress(int key)
        {
            var hotel = await _context.Hotels.Include(h => h.Address).FirstOrDefaultAsync(e => e.Id == key);

            if (hotel != null) return hotel;

            throw new HotelNotFoundException(key);
        }
    }
}
