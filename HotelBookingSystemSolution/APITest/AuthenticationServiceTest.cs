using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Services;
using HotelBookingSystemAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace APITest
{
    internal class AuthenticationServiceTest
    {
        private IAuthenticationService _authenticationService;

        [SetUp]
        public void SetUp ()
        {
            _authenticationService = new AuthenticationService();
        }

        [Test]
        public void GetHashKeyPassTest ()
        {
            HMACSHA512 hMACSHA512 = new HMACSHA512();
            byte[] hashKey = _authenticationService.GetHashKey(hMACSHA512);
            Assert.IsNotNull(hashKey);
        }

        [Test]
        public void GetHashedPasswordPassTest ()
        {
            HMACSHA512 hmacSHA512 = new HMACSHA512();
            byte[] hashedPassword = _authenticationService.GetHashedPassword(hmacSHA512, "password");

            Assert.IsNotNull(hashedPassword);
        }

        [Test]
        public void ComparePasswordPassTest ()
        {
            HMACSHA512 hMACSHA512 = new HMACSHA512();
            byte[] passwordHashkey = _authenticationService.GetHashKey(hMACSHA512);
            byte[] hashedPassword = _authenticationService.GetHashedPassword(hMACSHA512, "password");

            byte[] newPassword = _authenticationService.GetHashedPassword(hMACSHA512, "password");

            Assert.That(newPassword, Is.EqualTo(hashedPassword));
        }

        [Test]
        public void ComparePasswordFailTest()
        {
            HMACSHA512 hMACSHA512 = new HMACSHA512();
            byte[] passwordHashkey = _authenticationService.GetHashKey(hMACSHA512);
            byte[] hashedPassword = _authenticationService.GetHashedPassword(hMACSHA512, "password");

            byte[] newPassword = _authenticationService.GetHashedPassword(hMACSHA512, "pass");

            Assert.That(hashedPassword, Is.Not.EqualTo(newPassword));
        }

        [Test]
        public void PrepareUserForRegisterTest ()
        {
            HMACSHA512 hMACSHA512 = new HMACSHA512();
            byte[] passwordHashkey = _authenticationService.GetHashKey(hMACSHA512);
            byte[] hashedPassword = _authenticationService.GetHashedPassword(hMACSHA512, "password");

            User user = _authenticationService.PrepareUserForRegister("bhavan@gmail.com", "guest", passwordHashkey, hashedPassword);

            Assert.IsInstanceOf(typeof(User), user);
        }
    }
}
