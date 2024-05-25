using HotelBookingSystemAPI.Context;
using HotelBookingSystemAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using RoleBasedAuthenticationAPI.Services.Interfaces;
using RoleBasedAuthenticationAPI.Services;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Repository;
using HotelBookingSystemAPI.Services.Interfaces;
using HotelBookingSystemAPI.Services;

namespace HotelBookingSystemAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            #region context
            builder.Services.AddDbContext<HotelBookingSystemContext>(
           options => options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection"))
           );
            #endregion

            #region repositories
            builder.Services.AddScoped<IRepository<int, Guest>, GuestRepository>();
            builder.Services.AddScoped<IRepository<int, User>, UserRepository>();
            #endregion

            #region services
            builder.Services.AddScoped<IGuestService, GuestService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
