using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Models.DTOs;

namespace HotelBookingSystemAPI.Services.Interfaces
{
    public interface IAddressService
    {
        public Task<Address> SaveHotelAddress(AddressInputDTO addressInputDTO);
    }
}
