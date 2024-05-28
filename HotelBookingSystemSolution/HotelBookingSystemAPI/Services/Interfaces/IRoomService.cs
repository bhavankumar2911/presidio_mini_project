using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Models.DTOs.RoomDTOs;

namespace HotelBookingSystemAPI.Services.Interfaces
{
    public interface IRoomService
    {
        public Task<Room> AddNewRoom(RoomInputDTO roomInputDTO, int hotelId);
    }
}
