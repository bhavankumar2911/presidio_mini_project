using HotelBookingSystemAPI.Models.DTOs;

namespace HotelBookingSystemAPI.Services.Interfaces
{
    public interface IAdminService
    {
        public Task<LoginAdminReturnDTO> Login(LoginAdminInputDTO loginAdminInputDTO);
    }
}
