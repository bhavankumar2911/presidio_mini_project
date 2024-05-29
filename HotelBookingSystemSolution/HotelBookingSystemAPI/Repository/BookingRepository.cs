using HotelBookingSystemAPI.Context;
using HotelBookingSystemAPI.Exceptions.Booking;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystemAPI.Repository
{
    public class BookingRepository : IRepository<int, Booking>
    {
        private readonly HotelBookingSystemContext _context;

        public BookingRepository(HotelBookingSystemContext context)
        {
            _context = context;
        }

        async public Task<Booking> Add(Booking booking)
        {
            _context.Add(booking);
            await _context.SaveChangesAsync();
            return booking;
        }

        async public Task<Booking> Delete(int key)
        {
            var booking = await GetByKey(key);

            if (booking != null)
            {
                _context.Remove(booking);
                await _context.SaveChangesAsync(true);
                return booking;
            }

            throw new BookingNotFoundException(key);
        }

        async public Task<IEnumerable<Booking>> GetAll()
        {
            var bookings = await _context.Bookings.ToListAsync();

            //if (bookings.Count == 0) throw new NoBookingsFoundException();

            return bookings;
        }

        public async Task<Booking> GetByKey(int key)
        {
            var booking = await _context.Bookings.FirstOrDefaultAsync(e => e.Id == key);

            if (booking != null) return booking;

            throw new BookingNotFoundException(key);
        }

        async public Task<Booking> Update(Booking newBooking)
        {
            var booking = await GetByKey(newBooking.Id);

            if (booking != null)
            {
                _context.Update(newBooking);
                await _context.SaveChangesAsync(true);
                return newBooking;
            }

            throw new BookingNotFoundException(newBooking.Id);
        }
    }
}
