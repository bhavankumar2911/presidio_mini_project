using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Models.DTOs;
using HotelBookingSystemAPI.Models.DTOs.Hotel;

namespace HotelBookingSystemAPI.Services.Interfaces
{
    public interface IHotelService
    {
        public Task<RegisterHotelReturnDTO> RegisterNewHotel(RegisterHotelInputDTO registerHotelInputDTO);

        public Task<LoginHotelReturnDTO> LoginHotel(LoginHotelInputDTO loginHotelInputDTO);

        public Task<IEnumerable<Hotel>> ListAllHotels();

        public Task<IEnumerable<Hotel>> ListAllHotelsByApprovalStatus(bool isApproved);

        public Task ChangeHotelApprovalStatus(int hotelId, bool newApprovalStatus);
    }
}
