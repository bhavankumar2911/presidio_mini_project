using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Models.DTOs;
using HotelBookingSystemAPI.Models.DTOs.Hotel;

namespace HotelBookingSystemAPI.Services.Interfaces
{
    public interface IAdminService
    {
        public Task<LoginAdminReturnDTO> Login(LoginAdminInputDTO loginAdminInputDTO);
    }
}
