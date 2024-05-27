using System.ComponentModel.DataAnnotations;

namespace HotelBookingSystemAPI.Models.DTOs
{
    public class AddressInputDTO
    {
        [Required(ErrorMessage = "Building number is required")]
        public string BuildingNoAndName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Street number and name is required")]
        public string StreetNoAndName { get; set; } = string.Empty;

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "State is required")]
        public string State { get; set; } = string.Empty;

        [Required(ErrorMessage = "Pincode is required")]
        public string Pincode { get; set; } = string.Empty;
    }
}
