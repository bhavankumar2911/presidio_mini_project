using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBookingSystemAPI.Models
{
    public class Guest
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public float Age { get; set; }
        public string Phone { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public bool IsBlocked { get; set; } = false;

        public User? User { get; set; }
        //[ForeignKey("User")]
        public int UserId { get; set; }

        //public ICollection<Review> Reviews { get; set; }
        public ICollection<Rating>? Ratings { get; set; }
    }
}
