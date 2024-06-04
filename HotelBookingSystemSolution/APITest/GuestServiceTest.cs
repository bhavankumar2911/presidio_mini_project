using HotelBookingSystemAPI.Exceptions;
using HotelBookingSystemAPI.Exceptions.Guest;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Models.DTOs;
using HotelBookingSystemAPI.Repository;
using HotelBookingSystemAPI.Repository.Interfaces;
using HotelBookingSystemAPI.Services;
using HotelBookingSystemAPI.Services.Interfaces;
using RoleBasedAuthenticationAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace APITest
{
    internal class GuestServiceTest
    {
        private TestDBContext _dbContext;
        private IGuestService _guestService;
        private IRepository<int, User> _userRepository;
        private IRepository<int, Guest> _guestRepository;
        private ITokenService _tokenService;


        [SetUp]
        public void SetUp ()
        {
            _dbContext = new TestDBContext (TestDBContext.GetDBContextOptions());
            _dbContext.Database.EnsureCreated ();
            _userRepository = new UserRepository (_dbContext);
            _guestRepository = new GuestRepository (_dbContext);
            _tokenService = new TokenService(TestConfiguration.GetConfiguration());
            _guestService = new GuestService(_userRepository, _guestRepository, _tokenService);
        }

        [Test]
        public async Task RegisterGuestPassTest ()
        {
            RegisterGuestInputDTO registerGuestInputDTO = new RegisterGuestInputDTO
            {
                Email = "bhavan@gmail.com",
                Name = "bhavan",
                Age = 21,
                Phone = "1234567890",
                Gender = "male",
                PlainTextPassword = "pass",
            };

            RegisterGuestReturnDTO registerGuestReturnDTO = await _guestService.RegisterNewGuest(registerGuestInputDTO);

            var guests = await _guestRepository.GetAll();
            var users = await _userRepository.GetAll();

            Assert.Multiple(() =>
            {
                Assert.That(_dbContext.Guests.ToList().Count, Is.EqualTo(2));
                Assert.That(_dbContext.Users.ToList().Count, Is.EqualTo(2));
            });
        }

        [Test]
        public void EmailExistTest ()
        {
            RegisterGuestInputDTO registerGuestInputDTO = new RegisterGuestInputDTO
            {
                Email = "john@gmail.com",
                Name = "john",
                Age = 21,
                Phone = "1234567890",
                Gender = "male",
                PlainTextPassword = "pass",
            };

            var ex = Assert.ThrowsAsync<GuestEmailAlreadyInUseException>(async () =>
            {
                await _guestService.RegisterNewGuest(registerGuestInputDTO);
            });

            Assert.That(ex.Message, Is.EqualTo($"The email({registerGuestInputDTO.Email}) is already used by another guest."));
        }

        [Test]
        public void PhoneNumberExistTest ()
        {
            RegisterGuestInputDTO registerGuestInputDTO = new RegisterGuestInputDTO
            {
                Email = "mike@gmail.com",
                Name = "mike",
                Age = 21,
                Phone = "0123456789",
                Gender = "male",
                PlainTextPassword = "pass",
            };

            var ex = Assert.ThrowsAsync<GuestPhoneNumberAlreadyInUseException>(async () =>
            {
                await _guestService.RegisterNewGuest(registerGuestInputDTO);
            });

            Assert.That(ex.Message, Is.EqualTo($"The phone number({registerGuestInputDTO.Phone}) is already used by another guest."));
        }

        [Test]
        public async Task LoginGuestPassTest ()
        {
            LoginGuestInputDTO loginGuestInputDTO = new LoginGuestInputDTO
            {
                Email = "john@gmail.com",
                PlainTextPassword = "pass"
            };

            LoginGuestReturnDTO loginGuestReturnDTO = await _guestService.LoginGuest(loginGuestInputDTO);

            Assert.That(loginGuestReturnDTO.Token, Is.Not.Null );
        }

        [Test]
        public void LoginWithNotExistingEmailTest ()
        {
            LoginGuestInputDTO loginGuestInputDTO = new LoginGuestInputDTO
            {
                Email = "mike@gmail.com",
                PlainTextPassword = "pass"
            };

            var ex = Assert.ThrowsAsync<WrongGuestLoginCredentialsException>(async () =>
            {
                await _guestService.LoginGuest(loginGuestInputDTO);
            });

            Assert.That(ex.Message, Is.EqualTo("Email or password is wrong. Try again"));
        }

        [Test]
        public void LoginGuestWithWrongPasswordTest()
        {
            LoginGuestInputDTO loginGuestInputDTO = new LoginGuestInputDTO
            {
                Email = "john@gmail.com",
                PlainTextPassword = "wrong"
            };

            var ex = Assert.ThrowsAsync<WrongGuestLoginCredentialsException>(async () =>
            {
                await _guestService.LoginGuest(loginGuestInputDTO);
            });

            Assert.That(ex.Message, Is.EqualTo("Email or password is wrong. Try again"));
        }

        [Test]
        public void BlockedGuestTest ()
        {
            LoginGuestInputDTO loginGuestInputDTO = new LoginGuestInputDTO
            {
                Email = "kim@gmail.com",
                PlainTextPassword = "pass"
            };

            var ex = Assert.ThrowsAsync<UnauthorizedException>(async () =>
            {
                await _guestService.LoginGuest(loginGuestInputDTO);
            });

            Assert.That(ex.Message, Is.EqualTo("You are unauthorized"));
        }
    }
}
