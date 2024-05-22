using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBookingSystemAPI.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public Guest? Guest { get; set; }
        [ForeignKey("Guest")]
        public int GuestId { get; set; }
        public Hotel? Hotel { get; set; }
        [ForeignKey("Hotel")]
        public int HotelId { get; set; }
    }
}
