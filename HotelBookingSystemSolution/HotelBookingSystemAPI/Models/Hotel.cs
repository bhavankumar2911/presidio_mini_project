using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBookingSystemAPI.Models
{
    public class Hotel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsApproved { get; set; } = false;
        public string Phone { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        
        public Address? Address { get; set; }
        public int AddressId { get; set; }

        public User? User { get; set; }
        //[ForeignKey("User")]
        public int UserId { get; set; }

        public ICollection<Review>? Reviews { get; set; }

        public ICollection<Rating>? Ratings { get; set; }
    }
}
