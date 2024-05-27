using HotelBookingSystemAPI.Context;
using HotelBookingSystemAPI.Exceptions.Address;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystemAPI.Repository
{
    public class AddressRepository : IRepository<int, Address>
    {
        private readonly HotelBookingSystemContext _context;

        public AddressRepository(HotelBookingSystemContext context)
        {
            _context = context;
        }

        async public Task<Address> Add(Address address)
        {
            _context.Add(address);
            await _context.SaveChangesAsync();
            return address;
        }

        async public Task<Address> Delete(int key)
        {
            var address = await GetByKey(key);

            if (address != null)
            {
                _context.Remove(address);
                await _context.SaveChangesAsync(true);
                return address;
            }

            throw new AddressNotFoundException(key);
        }

        async public Task<IEnumerable<Address>> GetAll()
        {
            var addresses = await _context.Addresses.ToListAsync();

            //if (addresses.Count == 0) throw new NoAddressesFoundException();

            return addresses;
        }

        public async Task<Address> GetByKey(int key)
        {
            var address = await _context.Addresses.FirstOrDefaultAsync(e => e.Id == key);

            if (address != null) return address;

            throw new AddressNotFoundException(key);
        }

        async public Task<Address> Update(Address newAddress)
        {
            var address = await GetByKey(newAddress.Id);

            if (address != null)
            {
                _context.Update(newAddress);
                await _context.SaveChangesAsync(true);
                return newAddress;
            }

            throw new AddressNotFoundException(newAddress.Id);
        }
    }

}
