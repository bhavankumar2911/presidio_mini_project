using HotelBookingSystemAPI.Exceptions.Guest;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Models.DTOs;
using HotelBookingSystemAPI.Repository.Interfaces;
using HotelBookingSystemAPI.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace HotelBookingSystemAPI.Services
{
    public class GuestService : IGuestService
    {
        private readonly IRepository<int, User> _userRepository;
        private readonly IRepository<int, Guest> _guestRepository;
        public GuestService(IRepository<int, User> userRepository, IRepository<int, Guest> guestRepository)
        {
            _guestRepository = guestRepository;
            _userRepository = userRepository;
        }

        private bool CheckIfEmailAlreadyExists (IEnumerable<User> users, string email)
        {
            foreach (var user in users)
            {
                if (user.Email == email) return true;
            }

            return false;
        }

        private bool CheckIfPhoneNumberAlreadyExists(IEnumerable<Guest> guests, string phone)
        {
            foreach (var guest in guests)
            {
                if (guest.Phone == phone) return true;
            }

            return false;
        }

        private RegisterGuestReturnDTO CreateGuestRegisterReturn (User user, Guest guest)
        {
            return new RegisterGuestReturnDTO
            {
                Id = guest.Id,
                Name = guest.Name,
                Email = user.Email,
                Phone = guest.Phone,
                Age = guest.Age,
                Gender = guest.Gender,
                Role = user.Role,
                IsBlocked = guest.IsBlocked,
            };
        }

        private byte[] GetHashKey(HMACSHA512 hMACSHA)
        {
            return hMACSHA.Key;
        }

        private byte[] GetHashedPassword(HMACSHA512 hMACSHA, string plainTextPassword)
        {
            return hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(plainTextPassword));
        }

        private User PrepareUserForRegister (string email, byte[] passwordHashKey, byte[] hashedPassword)
        {
            return new User
            {
                Email = email,
                Role = "guest",
                PasswordHashKey = passwordHashKey,
                HashedPassword = hashedPassword
            };
        }

        private Guest PrepareGuestForRegister (RegisterGuestInputDTO guestInputDTO, int userId)
        {
            return new Guest
            {
                Name = guestInputDTO.Name,
                Age = guestInputDTO.Age,
                Phone = guestInputDTO.Phone,
                Gender = guestInputDTO.Gender,
                IsBlocked = false,
                UserId = userId
            };
        }

        public async Task<RegisterGuestReturnDTO> RegisterNewGuest(RegisterGuestInputDTO registerGuestInputDTO)
        {
            IEnumerable<User> users = await _userRepository.GetAll();
            IEnumerable<Guest> guests = await _guestRepository.GetAll();

            if (users.Count() > 0)
            {
                if (CheckIfEmailAlreadyExists(users, registerGuestInputDTO.Email)) throw new GuestEmailAlreadyInUseException(registerGuestInputDTO.Email);
            }

            if (guests.Count() > 0)
            {
                if (CheckIfPhoneNumberAlreadyExists(guests, registerGuestInputDTO.Phone)) throw new GuestPhoneNumberAlreadyInUseException(registerGuestInputDTO.Phone);
            }

            // create password hash key
            HMACSHA512 hMACSHA512 = new HMACSHA512();
            byte[] key = GetHashKey(hMACSHA512);

            // compute hashed password
            byte[] hashedPassword = GetHashedPassword(hMACSHA512, registerGuestInputDTO.PlainTextPassword);

            // insert user
            User user = PrepareUserForRegister(registerGuestInputDTO.Email,key,hashedPassword);

            User newUser = await _userRepository.Add(user);

            // insert guest
            Guest guest = PrepareGuestForRegister(registerGuestInputDTO, newUser.Id);
            Guest newGuest = await _guestRepository.Add(guest);

            return CreateGuestRegisterReturn(newUser, newGuest);
        }
    }
}
