namespace HotelBookingSystemAPI.Models.DTOs.RoomDTOs
{
    public class RoomFilterDTO
    {
        public double MinPrice { get; set; } = -1;
        public double MaxPrice { get; set; } = -1;
        public RoomSize Size { get; set; } = RoomSize.Default;
        public int HotelId { get; set; } = -1;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
    }
}
