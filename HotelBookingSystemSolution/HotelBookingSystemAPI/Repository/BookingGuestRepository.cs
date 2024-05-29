using HotelBookingSystemAPI.Context;
using HotelBookingSystemAPI.Exceptions.Guest;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystemAPI.Repository
{
    public class BookingGuestRepository : IRepository<int, BookingGuest>
    {
        private readonly HotelBookingSystemContext _context;

        public BookingGuestRepository(HotelBookingSystemContext context)
        {
            _context = context;
        }

        async public Task<BookingGuest> Add(BookingGuest guest)
        {
            _context.Add(guest);
            await _context.SaveChangesAsync();
            return guest;
        }

        async public Task<BookingGuest> Delete(int key)
        {
            var guest = await GetByKey(key);

            if (guest != null)
            {
                _context.Remove(guest);
                await _context.SaveChangesAsync(true);
                return guest;
            }

            throw new GuestNotFoundException(key);
        }

        async public Task<IEnumerable<BookingGuest>> GetAll()
        {
            var guests = await _context.BookingGuests.ToListAsync();

            //if (guests.Count == 0) throw new NoBookingGuestsFoundException();

            return guests;
        }

        public async Task<BookingGuest> GetByKey(int key)
        {
            var guest = await _context.BookingGuests.FirstOrDefaultAsync(e => e.Id == key);

            if (guest != null) return guest;

            throw new GuestNotFoundException(key);
        }

        async public Task<BookingGuest> Update(BookingGuest newGuest)
        {
            var guest = await GetByKey(newGuest.Id);

            if (guest != null)
            {
                _context.Update(newGuest);
                await _context.SaveChangesAsync(true);
                return newGuest;
            }

            throw new GuestNotFoundException(newGuest.Id);
        }

    }
}
