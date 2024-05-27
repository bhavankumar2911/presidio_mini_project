namespace HotelBookingSystemAPI.Exceptions.Hotel
{
    public class HotelNotFoundException : Exception
    {
        public HotelNotFoundException(int hotelId) : base($"No hotel was found with this id {hotelId}") { }
    }
}
