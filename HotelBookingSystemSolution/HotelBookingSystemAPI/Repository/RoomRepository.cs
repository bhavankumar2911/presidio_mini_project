﻿using HotelBookingSystemAPI.Context;
using HotelBookingSystemAPI.Exceptions.Room;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystemAPI.Repository
{
    public class RoomRepository : IRepository<int, Room>
    {
        private readonly HotelBookingSystemContext _context;

        public RoomRepository(HotelBookingSystemContext context)
        {
            _context = context;
        }

        async public Task<Room> Add(Room room)
        {
            _context.Add(room);
            await _context.SaveChangesAsync();
            return room;
        }

        async public Task<Room> Delete(int key)
        {
            var room = await GetByKey(key);

            if (room != null)
            {
                _context.Remove(room);
                await _context.SaveChangesAsync(true);
                return room;
            }

            throw new RoomNotFoundException(key);
        }

        async public Task<IEnumerable<Room>> GetAll()
        {
            var rooms = await _context.Rooms.Include(r => r.Hotel).ThenInclude(h => h.Address).ToListAsync();

            //if (rooms.Count == 0) throw new NoRoomsFoundException();

            return rooms;
        }

        public async Task<Room> GetByKey(int key)
        {
            var room = await _context.Rooms.FirstOrDefaultAsync(e => e.Id == key);

            if (room != null) return room;

            throw new RoomNotFoundException(key);
        }

        async public Task<Room> Update(Room newRoom)
        {
            var room = await GetByKey(newRoom.Id);

            if (room != null)
            {
                _context.Update(newRoom);
                await _context.SaveChangesAsync(true);
                return newRoom;
            }

            throw new RoomNotFoundException(newRoom.Id);
        }

    }
}
