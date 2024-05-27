namespace HotelBookingSystemAPI.Exceptions.Hotel
{
    public class HotelEmailAlreadyInUseException : Exception
    {
        public HotelEmailAlreadyInUseException(string email) : base($"The email({email}) is already used by another Hotel.") { }
    }
}
