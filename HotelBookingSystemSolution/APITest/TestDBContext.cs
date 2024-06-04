using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using HotelBookingSystemAPI;
using HotelBookingSystemAPI.Context;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Services;
using HotelBookingSystemAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace APITest
{
    internal class TestDBContext : HotelBookingSystemContext
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly HMACSHA512 _hmacSHA512;
        public TestDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
            _authenticationService = new AuthenticationService();
            _hmacSHA512 = new HMACSHA512();
        }

        static public DbContextOptions GetDBContextOptions ()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder().UseInMemoryDatabase("dbTest");

            return optionsBuilder.Options;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasData(
                    new User
                    {
                        Id = 1,
                        Email = "john@gmail.com",
                        Role = "guest",
                        PasswordHashKey = _authenticationService.GetHashKey(_hmacSHA512),
                        HashedPassword = _authenticationService.GetHashedPassword(_hmacSHA512, "pass"),
                    },
                    new User
                    {
                        Id = 2,
                        Email = "kim@gmail.com",
                        Role = "guest",
                        PasswordHashKey = _authenticationService.GetHashKey(_hmacSHA512),
                        HashedPassword = _authenticationService.GetHashedPassword(_hmacSHA512, "pass"),
                    }
                );

            modelBuilder.Entity<Guest>()
                .HasData(
                    new Guest
                    {
                        Id=1,
                        Name = "john",
                        Age = 21,
                        Phone = "0123456789",
                        Gender = "male",
                        IsBlocked = false,
                        UserId = 1,
                    },
                    new Guest
                    {
                        Id = 2,
                        Name = "kim",
                        Age = 21,
                        Phone = "6789012345",
                        Gender = "female",
                        IsBlocked = true,
                        UserId = 2,
                    }
                );
        }
    }
}
