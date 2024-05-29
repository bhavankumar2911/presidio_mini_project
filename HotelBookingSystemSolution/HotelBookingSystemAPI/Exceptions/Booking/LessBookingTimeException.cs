namespace HotelBookingSystemAPI.Exceptions.Booking
{
    public class LessBookingTimeException : Exception
    {
        public LessBookingTimeException() : base ("You cannot book a room for less than 3 hours.") { }
    }
}
