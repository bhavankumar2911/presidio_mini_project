namespace HotelBookingSystemAPI.Exceptions.Room
{
    public class RoomNumberAlreadyInUseException : Exception
    {
        public RoomNumberAlreadyInUseException(int roomNumber) : base($"Room number - {roomNumber} is already assigned to some other room") { }
    }
}
