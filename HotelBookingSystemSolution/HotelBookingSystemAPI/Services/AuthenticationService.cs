using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Repository;
using HotelBookingSystemAPI.Repository.Interfaces;
using HotelBookingSystemAPI.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace HotelBookingSystemAPI.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        public byte[] GetHashedPassword(HMACSHA512 hMACSHA, string plainTextPassword)
        {
            return hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(plainTextPassword));
        }

        public byte[] GetHashKey(HMACSHA512 hMACSHA)
        {
            return hMACSHA.Key;
        }

        public User PrepareUserForRegister(string email, string role, byte[] passwordHashKey, byte[] hashedPassword)
        {
            return new User
            {
                Email = email,
                Role = role,
                PasswordHashKey = passwordHashKey,
                HashedPassword = hashedPassword
            };
        }
    }
}
