namespace HotelBookingSystemAPI.Exceptions.Rating
{
    public class RatingNotFoundException : Exception
    {
        public RatingNotFoundException() : base("Rating not found") { }
    }
}
