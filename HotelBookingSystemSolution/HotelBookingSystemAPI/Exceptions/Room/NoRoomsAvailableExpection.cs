namespace HotelBookingSystemAPI.Exceptions.Room
{
    public class NoRoomsAvailableExpection : Exception
    {
        public NoRoomsAvailableExpection() : base("No rooms are available currently.") { }
    }
}
