using System.ComponentModel.DataAnnotations;

namespace HotelBookingSystemAPI.Models.DTOs.RatingDTOs
{
    public class RatingInputDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "Hotel id is required.")]
        public int HotelId { get; set; }

        [Range(1, 5, ErrorMessage = "You can rate the hotel in a scale of 5 stars")]
        public int StarRating { get; set; }
    }
}
