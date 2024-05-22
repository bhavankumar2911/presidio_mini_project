using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBookingSystemAPI.Models
{
    public class Room
    {
        [Key]
        public int Id { get; set; }
        public double PricePerDay { get; set; }
        public string Size { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }

        public Hotel? Hotel { get; set; }
        [ForeignKey("Hotel")]
        public int HotelId { get; set; }

        public ICollection<Booking>? Bookings { get; set; }
    }
}
