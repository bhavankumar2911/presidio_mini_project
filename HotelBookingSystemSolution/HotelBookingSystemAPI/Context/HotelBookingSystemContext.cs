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

            modelBuilder.Entity<Hotel>()
                .HasOne(h => h.Address);

            modelBuilder.Entity<Room>()
                .HasOne(r => r.Hotel);

            modelBuilder.Entity<Guest>()
                .HasOne(g => g.User)
                .WithOne(u => u.Guest)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Hotel>()
                .HasOne(h => h.User)
                .WithOne(u => u.Hotel)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Room);

            modelBuilder.Entity<BookingGuest>()
                .HasOne(bg => bg.Booking)
                .WithMany(b => b.BookingGuests)
                .HasForeignKey(bg => bg.BookingId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Hotel)
                .WithMany(h => h.Reviews)
                .HasForeignKey(r => r.HotelId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Guest)
                .WithMany(g => g.Reviews)
                .HasForeignKey(r => r.GuestId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
