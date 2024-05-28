using HotelBookingSystemAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystemAPI.Context
{
    public class HotelBookingSystemContext : DbContext
    {
        public HotelBookingSystemContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingGuest> BookingGuests { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // primary keys
            //modelBuilder.Entity<User>().HasKey(p => p.Id);
            //modelBuilder.Entity<Hotel>().HasKey(h => h.Id);
            //modelBuilder.Entity<Address>().HasKey(a => a.Id);
            //modelBuilder.Entity<Review>().HasKey(r => r.Id);
            //modelBuilder.Entity<Booking>().HasKey(b => b.Id);
            //modelBuilder.Entity<BookingGuest>().HasKey(bg => bg.Id);
            //modelBuilder.Entity<Rating>().HasKey(r => r.Id);
            //modelBuilder.Entity<Room>().HasKey(r => r.Id);

            //modelBuilder.Entity<Hotel>()
            //    .HasOne(a => a.Address)
            //    .WithOne(h => h.Hotel)
            //    .HasForeignKey<Address>(a => a.HotelId)
            //    .IsRequired()
            //    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Hotel>()
                .HasOne(h => h.Address);

            modelBuilder.Entity<Room>()
                .HasOne(r => r.Hotel);

            //modelBuilder.Entity<User>()
            //    .HasOne(u => u.Guest)
            //    .WithOne(g => g.User)
            //    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Guest>()
                .HasOne(g => g.User)
                .WithOne(u => u.Guest)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Hotel>()
                .HasOne(h => h.User)
                .WithOne(u => u.Hotel)
                .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<User>()
            //    .HasOne(u => u.Hotel)
            //    .WithOne(h => h.User)
            //    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
