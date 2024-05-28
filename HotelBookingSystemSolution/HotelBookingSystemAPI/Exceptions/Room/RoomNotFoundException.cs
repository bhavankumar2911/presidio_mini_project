namespace HotelBookingSystemAPI.Exceptions.Room
{
    public class RoomNotFoundException : Exception
    {
        public RoomNotFoundException(int roomId) : base($"No room was found with this id {roomId}") { }
    }
}
