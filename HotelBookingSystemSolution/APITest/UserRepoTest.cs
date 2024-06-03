using HotelBookingSystemAPI;
using HotelBookingSystemAPI.Context;
using HotelBookingSystemAPI.Exceptions.User;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Repository;
using HotelBookingSystemAPI.Repository.Interfaces;
using HotelBookingSystemAPI.Services;
using HotelBookingSystemAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using RoleBasedAuthenticationAPI.Services.Interfaces;
using System.Security.Cryptography;

namespace APITest
{
    public class UserRepoTest
    {
        private HotelBookingSystemContext _context;
        private IRepository<int, User> userRepo;
        private IAuthenticationService _authenticationService;

        [SetUp]
        public void Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder().UseInMemoryDatabase("dummyDB");
            _context = new HotelBookingSystemContext(optionsBuilder.Options);
            userRepo = new UserRepository(_context);
            _authenticationService = new AuthenticationService();
        }

        [Test]
        public async Task AddUserPassTest()
        {
            HMACSHA512 hMACSHA512 = new HMACSHA512();

            User user = await userRepo.Add(new User
            {
                Email = "bhavan@gmail.com",
                Role = "guest",
                PasswordHashKey = _authenticationService.GetHashKey(hMACSHA512),
                HashedPassword = _authenticationService.GetHashedPassword(hMACSHA512, "password")
            });

            Assert.IsNotNull(user.Id);
        }

        [Test]
        public async Task GetUserByKeyPassTest()
        {
            // add one user
            HMACSHA512 hMACSHA512 = new HMACSHA512();

            User newUser = await userRepo.Add(new User
            {
                Email = "john@gmail.com",
                Role = "guest",
                PasswordHashKey = _authenticationService.GetHashKey(hMACSHA512),
                HashedPassword = _authenticationService.GetHashedPassword(hMACSHA512, "password")
            });

            User user = await userRepo.GetByKey(newUser.Id);

            Assert.IsNotNull(user);
        }

        [Test]
        public void GetUserByKeyFailTest ()
        {
            var ex = Assert.ThrowsAsync<UserNotFoundException>(async () => await userRepo.GetByKey(98));

            Assert.That(ex.Message, Is.EqualTo($"No user was found with this id {98}"));
        }

        [Test]
        public async Task UpdateUserPassTest ()
        {
            HMACSHA512 hMACSHA512 = new HMACSHA512();

            User user = await userRepo.Add(new User
            {
                Email = "bhavan@gmail.com",
                Role = "guest",
                PasswordHashKey = _authenticationService.GetHashKey(hMACSHA512),
                HashedPassword = _authenticationService.GetHashedPassword(hMACSHA512, "password")
            });

            user.Email = "bhavankumar@gmail.com";

            User updatedUser = await userRepo.Update(user);

            Assert.That(updatedUser.Email, Is.EqualTo("bhavankumar@gmail.com"));
        }

        [Test]
        public async Task UpdateUserFailTest ()
        {
            HMACSHA512 hMACSHA512 = new HMACSHA512();

            User user = new User
            {
                Id = 100,
                Email = "bhavan@gmail.com",
                Role = "guest",
                PasswordHashKey = _authenticationService.GetHashKey(hMACSHA512),
                HashedPassword = _authenticationService.GetHashedPassword(hMACSHA512, "password")
            };

            var ex = Assert.ThrowsAsync<UserNotFoundException>(async () => await userRepo.Update(user));

            Assert.That(ex.Message, Is.EqualTo($"No user was found with this id {100}"));
        }

        [Test]
        public async Task DeleteUserPassTest()
        {
            HMACSHA512 hMACSHA512 = new HMACSHA512();

            User user = await userRepo.Add(new User
            {
                Email = "bhavan@gmail.com",
                Role = "guest",
                PasswordHashKey = _authenticationService.GetHashKey(hMACSHA512),
                HashedPassword = _authenticationService.GetHashedPassword(hMACSHA512, "password")
            });

            int userId = user.Id;

            await userRepo.Delete(userId);

            var ex = Assert.ThrowsAsync<UserNotFoundException>(async () => await userRepo.GetByKey(userId));

            Assert.That(ex.Message, Is.EqualTo($"No user was found with this id {userId}"));
        }

        [Test]
        public async Task DeleteUserFailTest()
        {
            var ex = Assert.ThrowsAsync<UserNotFoundException>(async () => await userRepo.Delete(100));

            Assert.That(ex.Message, Is.EqualTo($"No user was found with this id {100}"));
        }
    }
}