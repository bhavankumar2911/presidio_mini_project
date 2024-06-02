using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBookingSystemAPI.Models
{
    public class BookingGuest
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public double Age { get; set; }

        //public Booking Booking { get; set; } = null!;
        //[ForeignKey("Booking")]
        public int BookingId { get; set; }
    }
}
