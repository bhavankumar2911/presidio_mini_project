namespace HotelBookingSystemAPI.Exceptions.Booking
{
    public class RoomAlreadyBookedException : Exception
    {
        public RoomAlreadyBookedException() : base("The room is not available at this time.") { }
    }
}
