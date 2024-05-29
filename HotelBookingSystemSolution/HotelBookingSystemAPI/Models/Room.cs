using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBookingSystemAPI.Models
{
    public enum RoomSize
    {
        Small, Medium, Large, Default
    }

    public class Room
    {
        [Key]
        public int Id { get; set; }
        public double PricePerDay { get; set; }
        public int RoomNumber { get; set; }
        public int FloorNumber { get; set; }
        public int MaxGuests { get; set; }
        public RoomSize Size { get; set; }
        public bool IsAvailable { get; set; }

        public Hotel Hotel { get; set; } = null!;
        //[ForeignKey("Hotel")]
        public int HotelId { get; set; }

        public ICollection<Booking>? Bookings { get; set; }
    }
}
