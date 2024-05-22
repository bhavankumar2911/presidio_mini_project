using HotelBookingSystemAPI.Context;
using HotelBookingSystemAPI.Exceptions.User;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace HotelBookingSystemAPI.Repository
{
    public abstract class UserRepository : IRepository<int, User>
    {
        private readonly HotelBookingSystemContext _context;

        public UserRepository(HotelBookingSystemContext context)
        {
            _context = context;
        }

        async public Task<User> Add(User user)
        {
            _context.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        async public Task<User> Delete(int key)
        {
            var user = await GetByKey(key);

            if (user != null)
            {
                _context.Remove(user);
                await _context.SaveChangesAsync(true);
                return user;
            }

            throw new UserNotFoundException(key);
        }

        async public Task<IEnumerable<User>> GetAll()
        {
            var users = await _context.Users.ToListAsync();

            if (users.Count == 0) throw new NoUsersFoundException();

            return users;
        }

        public async Task<User> GetByKey(int key)
        {
            var user = await _context.Users.FirstOrDefaultAsync(e => e.Id == key);

            if (user != null) return user;

            throw new UserNotFoundException(key);
        }

        async public Task<User> Update(User newUser)
        {
            var user = await GetByKey(newUser.Id);

            if (user != null)
            {
                _context.Update(newUser);
                await _context.SaveChangesAsync(true);
                return newUser;
            }

            throw new UserNotFoundException(newUser.Id);
        }
    }
}
