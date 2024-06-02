using HotelBookingSystemAPI;
using HotelBookingSystemAPI.Context;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Repository;
using HotelBookingSystemAPI.Repository.Interfaces;
using HotelBookingSystemAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using RoleBasedAuthenticationAPI.Services.Interfaces;

namespace APITest
{
    public class UserRepoTest
    {
        private HotelBookingSystemContext _context;
        private IRepository<int, User> userRepo;

        [SetUp]
        public void Setup()
        {
            

            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder().UseInMemoryDatabase("dummyDB");
            _context = new HotelBookingSystemContext(optionsBuilder.Options);
            userRepo = new UserRepository(_context);
        }

        [Test]
        public async Task AddUserPassTest()
        {
            User user = await userRepo.Add(new User
            {

            });
        }
    }
}