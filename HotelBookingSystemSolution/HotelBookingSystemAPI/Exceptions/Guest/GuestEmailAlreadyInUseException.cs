namespace HotelBookingSystemAPI.Exceptions.Guest
{
    public class GuestEmailAlreadyInUseException : Exception
    {
        public GuestEmailAlreadyInUseException(string email) : base($"The email({email}) is already used by another guest.") { }
    }
}
