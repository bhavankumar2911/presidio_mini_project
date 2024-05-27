using HotelBookingSystemAPI.Exceptions.Guest;
using HotelBookingSystemAPI.Exceptions.Hotel;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Models.DTOs;
using HotelBookingSystemAPI.Repository;
using HotelBookingSystemAPI.Repository.Interfaces;
using HotelBookingSystemAPI.Services.Interfaces;
using RoleBasedAuthenticationAPI.Services.Interfaces;
using System.Security.Cryptography;

namespace HotelBookingSystemAPI.Services
{
    public class HotelService : IHotelService
    {
        private readonly IRepository<int, User> _userRepository;
        private readonly IRepository<int, Hotel> _hotelRepository;
        private readonly ITokenService _tokenService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IAddressService _addressService;

        public HotelService(IRepository<int, User> userRepository, IRepository<int, Hotel> hotelRepository, ITokenService tokenService, IAuthenticationService authenticationService, IAddressService addressService)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
            _hotelRepository = hotelRepository;
            _authenticationService = authenticationService;
            _addressService = addressService;
        }

        private async Task<User> CheckIfEmailAlreadyExists(string email)
        {
            IEnumerable<User> users = await _userRepository.GetAll();

            if (users.Count() == 0) return null;

            foreach (var user in users)
            {
                if (user.Email == email) return user;
            }

            return null;
        }

        private bool CheckIfPhoneNumberAlreadyExists(IEnumerable<Hotel> hotels, string phone)
        {
            foreach (var hotel in hotels)
            {
                if (hotel.Phone == phone) return true;
            }

            return false;
        }

        private Hotel PrepareHotelForRegister(RegisterHotelInputDTO registerHotelInputDTO, int userId, int addressId)
        {
            return new Hotel
            {
                Name = registerHotelInputDTO.Name,
                Phone = registerHotelInputDTO.Phone,
                Description = registerHotelInputDTO.Description,
                IsApproved = false,
                UserId = userId,
                AddressId = addressId
            };
        }

        private RegisterHotelReturnDTO CreateHotelRegisterReturn(User user, Hotel hotel, Address address)
        {
            return new RegisterHotelReturnDTO
            {
                Id = hotel.Id,
                Name = hotel.Name,
                Email = user.Email,
                Phone = hotel.Phone,
                Description = hotel.Description,
                IsApproved = false,
                Address = new AddressInputDTO
                {
                    BuildingNoAndName = address.BuildingNoAndName,
                    StreetNoAndName = address.StreetNoAndName,
                    City = address.City,
                    State = address.State,
                    Pincode = address.Pincode,
                }
            };
        }

        public async Task<RegisterHotelReturnDTO> RegisterNewHotel(RegisterHotelInputDTO registerHotelInputDTO)
        {
            IEnumerable<Hotel> hotels = await _hotelRepository.GetAll();

            User existingUser = await CheckIfEmailAlreadyExists(registerHotelInputDTO.Email);

            if (existingUser != null) throw new HotelEmailAlreadyInUseException(registerHotelInputDTO.Email);

            if (hotels.Count() > 0)
            {
                if (CheckIfPhoneNumberAlreadyExists(hotels, registerHotelInputDTO.Phone)) throw new GuestPhoneNumberAlreadyInUseException(registerHotelInputDTO.Phone);
            }

            // create password hash key
            HMACSHA512 hMACSHA512 = new HMACSHA512();
            byte[] key = _authenticationService.GetHashKey(hMACSHA512);

            // compute hashed password
            byte[] hashedPassword = _authenticationService.GetHashedPassword(hMACSHA512, registerHotelInputDTO.PlainTextPassword);

            // insert user
            User user = _authenticationService.PrepareUserForRegister(registerHotelInputDTO.Email, "hotel", key, hashedPassword);

            User newUser = await _userRepository.Add(user);

            // insert hotel
            Address address = await _addressService.SaveHotelAddress(registerHotelInputDTO.Address);

            // insert hotel
            Hotel hotel = PrepareHotelForRegister(registerHotelInputDTO, newUser.Id, address.Id);
            Hotel newHotel = await _hotelRepository.Add(hotel);

            return CreateHotelRegisterReturn(newUser, newHotel, address);
        }
    }
}
