using HotelBookingSystemAPI.Exceptions;
using HotelBookingSystemAPI.Exceptions.Guest;
using HotelBookingSystemAPI.Exceptions.Hotel;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Models.DTOs;
using HotelBookingSystemAPI.Models.DTOs.Hotel;
using HotelBookingSystemAPI.Repository;
using HotelBookingSystemAPI.Repository.Interfaces;
using HotelBookingSystemAPI.Services.Interfaces;
using RoleBasedAuthenticationAPI.Services.Interfaces;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace HotelBookingSystemAPI.Services
{
    public class HotelService : IHotelService
    {
        private readonly IRepository<int, User> _userRepository;
        private readonly IRepository<int, Hotel> _hotelRepository;
        private readonly IHotelWithAddressRepository _hotelAddressRepository;
        private readonly IRepository<int, Address> _addressRepository;
        private readonly ITokenService _tokenService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IAddressService _addressService;

        public HotelService(IRepository<int, User> userRepository, IRepository<int, Hotel> hotelRepository, ITokenService tokenService, IAuthenticationService authenticationService, IAddressService addressService, IRepository<int, Address> addressRepository, IHotelWithAddressRepository hotelAddressRepository)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
            _hotelRepository = hotelRepository;
            _authenticationService = authenticationService;
            _addressService = addressService;
            _addressRepository = addressRepository;
            _hotelAddressRepository = hotelAddressRepository;
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

        private async Task<Hotel> GetHotelFromUser(int userId)
        {
            IEnumerable<Hotel> hotels = await _hotelRepository.GetAll();

            if (hotels.Count() == 0) throw new NoHotelsFoundException();

            foreach (Hotel hotel in hotels)
            {
                if (hotel.UserId == userId) return hotel;
            }

            throw new WrongHotelLoginCredentialsException();
        }

        private LoginHotelReturnDTO CreateHotelLoginReturn(User user, Hotel hotel, Address address, string token)
        {
            RegisterHotelReturnDTO returnHotel = new RegisterHotelReturnDTO()
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

            return new LoginHotelReturnDTO
            {
                Hotel = returnHotel,
                Token = token
            };
        }

        public async Task<LoginHotelReturnDTO> LoginHotel(LoginHotelInputDTO loginHotelInputDTO)
        {
            User user = await CheckIfEmailAlreadyExists(loginHotelInputDTO.Email);

            if (user == null) throw new WrongHotelLoginCredentialsException();

            // check role
            if (user.Role != "hotel") throw new UnauthorizedException();

            // check password
            HMACSHA512 hMACSHA512 = new HMACSHA512(user.PasswordHashKey);
            byte[] hashedPassword = _authenticationService.GetHashedPassword(hMACSHA512, loginHotelInputDTO.PlainTextPassword);

            if (_authenticationService.ComparePassword(hashedPassword, user.HashedPassword))
            {
                Hotel hotel = await GetHotelFromUser(user.Id);

                if (!hotel.IsApproved) throw new UnauthorizedException();

                // fetching address
                Address address = await _addressRepository.GetByKey(hotel.AddressId);

                return CreateHotelLoginReturn(user, hotel, address, _tokenService.GenerateToken(hotel.Id, user.Role));
            }

            throw new WrongGuestLoginCredentialsException();
        }

        public async Task<IEnumerable<Hotel>> ListAllHotels()
        {
            IEnumerable<Hotel> hotels = await _hotelAddressRepository.GetAllWithAddress();
            //IEnumerable<Hotel> hotels = await _hotelRepository.GetAll();

            if (hotels.Count() == 0) throw new NoHotelsFoundException();

            return hotels;
        }

        public async Task<IEnumerable<Hotel>> ListAllHotelsByApprovalStatus(bool isApproved)
        {
            IEnumerable<Hotel> hotels = await _hotelAddressRepository.GetAllWithAddress();

            if (hotels.Count() == 0) throw new NoHotelsFoundException();

            IList<Hotel> result = new List<Hotel>();

            foreach (var hotel in hotels)
            {
                if (hotel.IsApproved == isApproved) result.Add(hotel);
            }

            if (result.Count == 0) throw new NoHotelsFoundException();

            return result;
        }
    }
}
