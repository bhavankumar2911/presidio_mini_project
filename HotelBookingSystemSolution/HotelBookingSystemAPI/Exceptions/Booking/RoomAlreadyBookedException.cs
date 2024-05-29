namespace HotelBookingSystemAPI.Exceptions.Booking
{
    public class RoomAlreadyBookedException : Exception
    {
        public RoomAlreadyBookedException(DateTime checkoutDateTime) : base($"This room is available after {checkoutDateTime}") { }
    }
}
