namespace HotelBookingSystemAPI.Exceptions.Hotel
{
    public class NoHotelsFoundException : Exception
    {
        public NoHotelsFoundException() : base("No hotels are available") { } 
    }
}
