

using HotelBookingSystemAPI.Models.DTOs;

namespace RoleBasedAuthenticationAPI.Services.Interfaces
{
    public interface ITokenService
    {
        public string GenerateToken(RegisterGuestReturnDTO registerGuestReturnDTO);
    }
}