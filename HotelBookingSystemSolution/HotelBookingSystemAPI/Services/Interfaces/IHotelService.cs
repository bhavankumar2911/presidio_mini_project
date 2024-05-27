using HotelBookingSystemAPI.Models.DTOs;

namespace HotelBookingSystemAPI.Services.Interfaces
{
    public interface IHotelService
    {
        public Task<RegisterHotelReturnDTO> RegisterNewHotel(RegisterHotelInputDTO registerHotelInputDTO);

        //public Task<LoginGuestReturnDTO> LoginGuest(LoginGuestInputDTO loginGuestInputDTO);
    }
}
