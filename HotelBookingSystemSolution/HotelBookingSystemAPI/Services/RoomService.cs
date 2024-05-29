using HotelBookingSystemAPI.Exceptions.Room;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Models.DTOs.RoomDTOs;
using HotelBookingSystemAPI.Repository.Interfaces;
using HotelBookingSystemAPI.Services.Interfaces;
using System.Security.Claims;

namespace HotelBookingSystemAPI.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRepository<int, Hotel> _hotelRepository;
        private readonly IRepository<int, Room> _roomRepository;

        public RoomService(IRepository<int, Hotel> hotelRepository, IRepository<int, Room> roomRepository)
        {
            _hotelRepository = hotelRepository;
            _roomRepository = roomRepository;
        }

        public async Task<Room> AddNewRoom(RoomInputDTO roomInputDTO, int hotelId)
        {
            Hotel hotel = await _hotelRepository.GetByKey(hotelId);

            // check room number
            IEnumerable<Room> rooms = await _roomRepository.GetAll();

            foreach (var r in rooms)
            {
                if (r.HotelId == hotelId && roomInputDTO.RoomNumber == r.RoomNumber) throw new RoomNumberAlreadyInUseException(roomInputDTO.RoomNumber);
            }

            Room room = new Room()
            {
                RoomNumber = roomInputDTO.RoomNumber,
                Size = roomInputDTO.Size,
                IsAvailable = roomInputDTO.IsAvailable,
                FloorNumber = roomInputDTO.FloorNumber,
                PricePerDay = roomInputDTO.PricePerDay,
                HotelId = hotelId
            };

            return await _roomRepository.Add(room);
        }

        public async Task<IEnumerable<Room>> ListRoomsForBooking(RoomFilterDTO roomFilterDTO)
        {
            IEnumerable<Room> rooms = await _roomRepository.GetAll();

            if (roomFilterDTO.MinPrice != -1)
                rooms = rooms.Where(room => room.PricePerDay >= roomFilterDTO.MinPrice);

            if (roomFilterDTO.MaxPrice != -1)
                rooms = rooms.Where(room => room.PricePerDay <= roomFilterDTO.MaxPrice);

            if (roomFilterDTO.Size != RoomSize.Default)
                rooms = rooms.Where(room => room.Size == roomFilterDTO.Size);

            if (roomFilterDTO.HotelId != -1)
                rooms = rooms.Where(room => room.HotelId == roomFilterDTO.HotelId);

            if (roomFilterDTO.MaxGuests != -1)
                rooms = rooms.Where(room => room.MaxGuests == roomFilterDTO.MaxGuests);

            if (!string.IsNullOrEmpty(roomFilterDTO.City))
                rooms = rooms.Where(room => room.Hotel.Address.City.ToLower() == roomFilterDTO.City.ToLower());

            if (!string.IsNullOrEmpty(roomFilterDTO.State))
                rooms = rooms.Where(room => room.Hotel.Address.State.ToLower() == roomFilterDTO.State.ToLower());

            if (rooms.Count() == 0) throw new NoRoomsAvailableExpection();

            return rooms;
        }
    }
}
