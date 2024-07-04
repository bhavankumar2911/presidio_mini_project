namespace HotelBookingSystemAPI.Exceptions.Booking
{
    public class NoGuestException : Exception
    {
        public NoGuestException() : base("Atleast one guest is required to book a room") { }
    }
}
