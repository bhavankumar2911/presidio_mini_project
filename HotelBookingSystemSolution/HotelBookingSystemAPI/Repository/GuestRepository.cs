using HotelBookingSystemAPI.Context;
using HotelBookingSystemAPI.Exceptions.Guest;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystemAPI.Repository
{
    public class GuestRepository : IRepository<int, Guest>
    {
        private readonly HotelBookingSystemContext _context;

        public GuestRepository(HotelBookingSystemContext context)
        {
            _context = context;
        }

        async public Task<Guest> Add(Guest guest)
        {
            _context.Add(guest);
            await _context.SaveChangesAsync();
            return guest;
        }

        async public Task<Guest> Delete(int key)
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

        async public Task<IEnumerable<Guest>> GetAll()
        {
            var guests = await _context.Guests.ToListAsync();

            if (guests.Count == 0) throw new NoGuestsFoundException();

            return guests;
        }

        public async Task<Guest> GetByKey(int key)
        {
            var guest = await _context.Guests.FirstOrDefaultAsync(e => e.Id == key);

            if (guest != null) return guest;

            throw new GuestNotFoundException(key);
        }

        async public Task<Guest> Update(Guest newGuest)
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
