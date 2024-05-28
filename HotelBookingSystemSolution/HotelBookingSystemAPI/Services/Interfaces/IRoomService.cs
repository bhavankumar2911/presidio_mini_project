using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Models.DTOs.RoomDTOs;

namespace HotelBookingSystemAPI.Services.Interfaces
{
    public interface IRoomService
    {
        public Task<Room> AddNewRoom(RoomInputDTO roomInputDTO, int hotelId);

        public Task<IEnumerable<Room>> ListRoomsForBooking(RoomFilterDTO roomFilterDTO);
    }
}
