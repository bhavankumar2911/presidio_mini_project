using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Repository.Interfaces;
using HotelBookingSystemAPI.Repository;
using HotelBookingSystemAPI.Services.Interfaces;
using HotelBookingSystemAPI.Services;
using RoleBasedAuthenticationAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBookingSystemAPI.Models.DTOs;
using HotelBookingSystemAPI.Exceptions.Guest;
using HotelBookingSystemAPI.Exceptions.Hotel;
using System.Numerics;
using HotelBookingSystemAPI.Exceptions.Address;
using HotelBookingSystemAPI.Exceptions;

namespace APITest
{
    internal class HotelServiceTest
    {
        private TestDBContext _dbContext;
        private IHotelService _hotelService;
        private IAuthenticationService _authenticationService;
        private IAddressService _addressService;
        private IRepository<int, Address> _addressRepository;
        private IRepository<int, User> _userRepository;
        private IRepository<int, Hotel> _hotelRepository;
        private IHotelWithAddressRepository _hotelWithAddressRepository;
        private ITokenService _tokenService;


        [SetUp]
        public void SetUp()
        {
            _dbContext = new TestDBContext(TestDBContext.GetDBContextOptions());
            _dbContext.Database.EnsureCreated();
            _userRepository = new UserRepository(_dbContext);
            _hotelRepository = new HotelRepository(_dbContext);
            _addressRepository = new AddressRepository(_dbContext);
            _hotelWithAddressRepository = new HotelWithAddressRepository(_dbContext);
            _addressService = new AddressService(_addressRepository);
            _tokenService = new TokenService(TestConfiguration.GetConfiguration());
            _authenticationService = new AuthenticationService();
            _hotelService = new HotelService(_userRepository, _hotelRepository, _tokenService, _authenticationService, _addressService, _addressRepository, _hotelWithAddressRepository);
        }

        [Test]
        public async Task RegisterHotelPassTest()
        {
            RegisterHotelInputDTO registerHotelInputDTO = new RegisterHotelInputDTO
            {
                PlainTextPassword = "pass",
                Email = "lemontree@gmail.com",
                Name = "Lemon Tree",
                Phone = "1234567890",
                Description = "best hotel",
                Address = new AddressInputDTO
                {
                    BuildingNoAndName = "1",
                    StreetNoAndName = "1 some street",
                    City = "some city",
                    State = "some state",
                    Pincode = "123456",
                }
            };

            RegisterHotelReturnDTO registerHotelReturnDTO = await _hotelService.RegisterNewHotel(registerHotelInputDTO);

            var hotels = await _hotelRepository.GetAll();
            var users = await _userRepository.GetAll();
            var addresses = await _addressRepository.GetAll();

            Assert.Multiple(() =>
            {
                Assert.That(_dbContext.Hotels.ToList().Count, Is.EqualTo(3));
                Assert.That(_dbContext.Users.ToList().Count, Is.EqualTo(5));
                Assert.That(_dbContext.Addresses.ToList().Count, Is.EqualTo(3));
            });
        }

        [Test]
        public void EmailExistTest ()
        {
            RegisterHotelInputDTO registerHotelInputDTO = new RegisterHotelInputDTO
            {
                Email = "hyatt@gmail.com"
            };

            var ex = Assert.ThrowsAsync<HotelEmailAlreadyInUseException>(async () =>
            {
                await _hotelService.RegisterNewHotel(registerHotelInputDTO);
            });

            Assert.That(ex.Message, Is.EqualTo($"The email({registerHotelInputDTO.Email}) is already used by another Hotel."));
        }

        [Test]
        public void PhoneNumberExistTest ()
        {
            RegisterHotelInputDTO registerHotelInputDTO = new RegisterHotelInputDTO
            {
                Email = "newhotel@gmail.com",
                Phone = "0987654321",
            };

            var ex = Assert.ThrowsAsync<HotelPhoneNumberAlreadyInUseException>(async () =>
            {
                await _hotelService.RegisterNewHotel(registerHotelInputDTO);
            });

            Assert.That(ex.Message, Is.EqualTo($"The phone number({registerHotelInputDTO.Phone}) is already used by another hotel."));
        }

        [Test]
        public void AddressExistTest()
        {
            RegisterHotelInputDTO registerHotelInputDTO = new RegisterHotelInputDTO
            {
                Address = new AddressInputDTO
                {
                    BuildingNoAndName = "2",
                    StreetNoAndName = "3 some street",
                    City = "some city",
                    State = "some state",
                    Pincode = "987654"
                }
            };

            var ex = Assert.ThrowsAsync<AddressAlreadyExistsException>(async () =>
            {
                await _hotelService.RegisterNewHotel(registerHotelInputDTO);
            });

            Assert.That(ex.Message, Is.EqualTo("Another hotel is registered in this address. Kindly check."));
        }

        [Test]
        public async Task LoginHotelPassTest ()
        {
            LoginHotelInputDTO input = new LoginHotelInputDTO
            {
                Email = "hyatt@gmail.com",
                PlainTextPassword = "pass"
            };

            LoginHotelReturnDTO loginResponse = await _hotelService.LoginHotel(input);

            Assert.That(loginResponse.Token, Is.Not.Null);
        }

        [Test]
        public void WrongEmailTest ()
        {
            LoginHotelInputDTO input = new LoginHotelInputDTO
            {
                Email = "somehotel@gmail.com",
                PlainTextPassword = "pass"
            };

            var exp = Assert.ThrowsAsync<WrongHotelLoginCredentialsException>(async () =>
            {
                await _hotelService.LoginHotel(input);
            });

            Assert.That(exp.Message, Is.EqualTo("Email or password is wrong. Try again"));
        }

        [Test]
        public void WrongPasswordTest()
        {
            LoginHotelInputDTO input = new LoginHotelInputDTO
            {
                Email = "sb@gmail.com",
                PlainTextPassword = "wrongpass"
            };

            var exp = Assert.ThrowsAsync<WrongHotelLoginCredentialsException>(async () =>
            {
                await _hotelService.LoginHotel(input);
            });

            Assert.That(exp.Message, Is.EqualTo("Email or password is wrong. Try again"));
        }

        [Test]
        public void HotelNotApprovedTest ()
        {
            LoginHotelInputDTO input = new LoginHotelInputDTO
            {
                Email = "sb@gmail.com",
                PlainTextPassword = "pass"
            };

            var exp = Assert.ThrowsAsync<UnauthorizedException>(async () =>
            {
                await _hotelService.LoginHotel(input);
            });

            Assert.That(exp.Message, Is.EqualTo("You are unauthorized"));
        }

        [Test]
        public async Task ListHotelsTest ()
        {
            var hotels = await _hotelService.ListAllHotels();

            Assert.That(hotels.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task ListHotelsByStatusTest()
        {
            var hotels = await _hotelService.ListAllHotelsByApprovalStatus(true);

            Assert.That(hotels.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task ChangeApprovalStatusPassTest ()
        {
            await _hotelService.ChangeHotelApprovalStatus(2, true);

            Hotel hotel = await _hotelRepository.GetByKey(2);

            Assert.That(hotel.IsApproved, Is.True);
        }

        [Test]
        public async Task ChangeApprovalStatusFailTest()
        {
            var ex = Assert.ThrowsAsync<HotelApprovalException>(
                    async () => await _hotelService.ChangeHotelApprovalStatus(2, false)
                );
            

            Hotel hotel = await _hotelRepository.GetByKey(2);

            Assert.That(ex.Message, Is.EqualTo("Hotel is already pending approval."));
        }
    }
}
