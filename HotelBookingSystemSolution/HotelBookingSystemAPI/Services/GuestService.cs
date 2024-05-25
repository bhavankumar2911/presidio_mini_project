using HotelBookingSystemAPI.Exceptions.Guest;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Models.DTOs;
using HotelBookingSystemAPI.Repository.Interfaces;
using HotelBookingSystemAPI.Services.Interfaces;
using RoleBasedAuthenticationAPI.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace HotelBookingSystemAPI.Services
{
    public class GuestService : IGuestService
    {
        private readonly IRepository<int, User> _userRepository;
        private readonly IRepository<int, Guest> _guestRepository;
        private readonly ITokenService _tokenService;

        public GuestService(IRepository<int, User> userRepository, IRepository<int, Guest> guestRepository, ITokenService tokenService)
        {
            _guestRepository = guestRepository;
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        private async Task<User> CheckIfEmailAlreadyExists (string email)
        {
            IEnumerable<User> users = await _userRepository.GetAll();

            if (users.Count() == 0) return null;

            foreach (var user in users)
            {
                if (user.Email == email) return user;
            }

            return null;
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
            //IEnumerable<User> users = await _userRepository.GetAll();
            IEnumerable<Guest> guests = await _guestRepository.GetAll();

            User existingUser = await CheckIfEmailAlreadyExists(registerGuestInputDTO.Email);

            if (existingUser != null) throw new GuestEmailAlreadyInUseException(registerGuestInputDTO.Email);

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

        private bool ComparePassword(byte[] passwordFromUser, byte[] passwordInDB)
        {
            if (passwordFromUser.Length != passwordInDB.Length) return false;

            for (int i = 0; i < passwordFromUser.Length; i++)
            {
                if (passwordFromUser[i] != passwordInDB[i])
                {
                    return false;
                }
            }
            return true;
        }

        private async Task<Guest> GetGuestFromUser (int userId)
        {
            IEnumerable<Guest> guests = await _guestRepository.GetAll();

            if (guests.Count() == 0) throw new NoGuestsFoundException();

            foreach (Guest guest in guests)
            {
                if (guest.UserId == userId) return guest;
            }

            throw new WrongGuestLoginCredentialsException();
        }

        private LoginGuestReturnDTO CreateGuestLoginReturn (User user, Guest guest, string token)
        {
            RegisterGuestReturnDTO returnUser = new RegisterGuestReturnDTO()
            {
                Id = guest.Id,
                Name = guest.Name,
                Email = user.Email,
                Phone = guest.Phone,
                Age = guest.Age,
                Gender = guest.Gender,
                Role = user.Role,
                IsBlocked = guest.IsBlocked
            };

            return new LoginGuestReturnDTO
            {
                User = returnUser,
                Token = token
            };
        }

        public async Task<LoginGuestReturnDTO> LoginGuest(LoginGuestInputDTO loginGuestInputDTO)
        {
            User user = await CheckIfEmailAlreadyExists(loginGuestInputDTO.Email);

            if (user == null) throw new WrongGuestLoginCredentialsException();

            // check password
            HMACSHA512 hMACSHA512 = new HMACSHA512(user.PasswordHashKey);
            byte[] hashedPassword = hMACSHA512.ComputeHash(Encoding.UTF8.GetBytes(loginGuestInputDTO.PlainTextPassword));

            if (ComparePassword(hashedPassword, user.HashedPassword))
            {
                Guest guest = await GetGuestFromUser(user.Id);

                if (guest.IsBlocked) throw new Exception("You are unauthorized");

                return CreateGuestLoginReturn(user, guest, _tokenService.GenerateToken(guest.Id, user.Role));
            }

            throw new WrongGuestLoginCredentialsException();
        }
    }
}
