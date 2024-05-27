using HotelBookingSystemAPI.Exceptions.Address;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Models.DTOs;
using HotelBookingSystemAPI.Repository.Interfaces;
using HotelBookingSystemAPI.Services.Interfaces;

namespace HotelBookingSystemAPI.Services
{
    public class AddressService : IAddressService
    {
        private readonly IRepository<int, Address> _addressRepository;

        public AddressService(IRepository<int, Address> addressRepository)
        {
            _addressRepository = addressRepository;
        }

        private async Task<bool> CheckAddressAlreadyInUse (AddressInputDTO addressInputDTO)
        {
            IEnumerable<Address> addresses = await _addressRepository.GetAll ();

            foreach (var address in addresses)
            {
                if (address.BuildingNoAndName == addressInputDTO.BuildingNoAndName)
                {
                    if (address.StreetNoAndName == addressInputDTO.StreetNoAndName && address.City == addressInputDTO.City && address.State == addressInputDTO.State && address.Pincode == addressInputDTO.Pincode)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public async Task<Address> SaveHotelAddress(AddressInputDTO addressInputDTO)
        {
            if (await CheckAddressAlreadyInUse(addressInputDTO))
                throw new AddressAlreadyExistsException();

            Address address = await _addressRepository.Add(new Address
            {
                BuildingNoAndName = addressInputDTO.BuildingNoAndName,
                StreetNoAndName = addressInputDTO.StreetNoAndName,
                City = addressInputDTO.City,
                State = addressInputDTO.State,
                Pincode = addressInputDTO.Pincode,
            });

            return address;
        }
    }
}
