using HotelBookingSystemAPI.Models.DTOs;

namespace HotelBookingSystemAPI.Services.Interfaces
{
    public interface IGuestService
    {
        public Task<RegisterGuestReturnDTO> RegisterNewGuest(RegisterGuestInputDTO registerGuestInputDTO);

        public Task<LoginGuestReturnDTO> LoginGuest(LoginGuestInputDTO loginGuestInputDTO);
    }
}
