using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBookingSystemAPI.Models
{
    public class Rating
    {
        [Key]
        public int Id { get; set; }
        public int StarRating { get; set; }

        //public Hotel? Hotel { get; set; }
        //[ForeignKey("Hotel")]
        public int HotelId { get; set; }
        public Guest? Guest { get; set; }
        [ForeignKey("Guest")]
        public int GuestId { get; set; }
    }
}
