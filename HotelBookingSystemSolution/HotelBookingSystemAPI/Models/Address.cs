using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBookingSystemAPI.Models
{
    public class Address
    {
        [Key]
        public int Id { get; set; }
        public string BuildingNoAndName { get; set; } = string.Empty;
        public string StreetNoAndName { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Pincode { get; set; } = string.Empty;

        public Hotel? Hotel { get; set; }
        //public int? HotelId { get; set; }
    }
}
