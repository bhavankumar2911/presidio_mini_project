using HotelBookingSystemAPI.Models;
using System.Security.Cryptography;

namespace HotelBookingSystemAPI.Services.Interfaces
{
    public interface IAuthenticationService
    {
        public byte[] GetHashKey(HMACSHA512 hMACSHA);

        public byte[] GetHashedPassword(HMACSHA512 hMACSHA, string plainTextPassword);

        public User PrepareUserForRegister(string email, string role, byte[] passwordHashKey, byte[] hashedPassword);
    }
}
