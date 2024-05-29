using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBookingSystemAPI.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }
        public DateTime DateOfBooking { get; set; } = DateTime.Today;
        public DateTime CheckinDateTime { get; set; }
        public DateTime CheckoutDateTime { get; set; }
        public double Amount { get; set; }

        public Room? Room { get; set; }
        [ForeignKey("Room")]
        public int RoomID { get; set; }

        public Guest? Guest { get; set; }
        [ForeignKey("Guest")]
        public int GuestId { get; set; }

        public ICollection<BookingGuest>? Guests { get; set; }
    }
}
