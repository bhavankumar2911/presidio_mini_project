namespace HotelBookingSystemAPI.Exceptions.Room
{
    public class RoomNotAvailableException : Exception
    {
        public RoomNotAvailableException() : base("The room is not available at the moment.") { }
    }
}
