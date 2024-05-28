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
    }
}
