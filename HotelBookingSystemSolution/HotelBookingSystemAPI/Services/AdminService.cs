using HotelBookingSystemAPI.Exceptions.Guest;
using HotelBookingSystemAPI.Exceptions;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Models.DTOs;
using HotelBookingSystemAPI.Services.Interfaces;
using RoleBasedAuthenticationAPI.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;
using HotelBookingSystemAPI.Repository;
using HotelBookingSystemAPI.Repository.Interfaces;
using HotelBookingSystemAPI.Models.DTOs.Hotel;
using HotelBookingSystemAPI.Exceptions.Hotel;

namespace HotelBookingSystemAPI.Services
{
    public class AdminService : IAdminService
    {
        private readonly IRepository<int, User> _userRepository;
        private readonly IAuthenticationService _authenticationService;
        private readonly ITokenService _tokenService;

        public AdminService(IRepository<int, User> userRepository, IAuthenticationService authenticationService, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _authenticationService = authenticationService;
            _tokenService = tokenService;
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

        public async Task<LoginAdminReturnDTO> Login(LoginAdminInputDTO loginAdminInputDTO)
        {
            User user = await CheckIfEmailAlreadyExists(loginAdminInputDTO.Email);

            if (user == null) throw new WrongLoginCredentialsException();

            // check role
            if (user.Role != "admin") throw new UnauthorizedException();

            // check password
            HMACSHA512 hMACSHA512 = new HMACSHA512(user.PasswordHashKey);
            byte[] hashedPassword = _authenticationService.GetHashedPassword(hMACSHA512, loginAdminInputDTO.PlainTextPassword);

            if (_authenticationService.ComparePassword(hashedPassword, user.HashedPassword))
            {
                return new LoginAdminReturnDTO { Token = _tokenService.GenerateToken(user.Id, user.Role) };
            }

            throw new WrongLoginCredentialsException();
        }
    }
}
