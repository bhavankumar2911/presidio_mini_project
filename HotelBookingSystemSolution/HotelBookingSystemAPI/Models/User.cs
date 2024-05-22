using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBookingSystemAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public byte[] PasswordHashKey { get; set; } = new byte[0];
        public byte[] HashedPassword { get; set; } = new byte[0];

        public Guest? Guest { get; set; }
        //[ForeignKey("Guest")]
        //public int GuestId { get; set; }

        public Hotel? Hotel { get; set; }
        //[ForeignKey("Hotel")]
        //public int HotelId { get; set; }
    }
}
