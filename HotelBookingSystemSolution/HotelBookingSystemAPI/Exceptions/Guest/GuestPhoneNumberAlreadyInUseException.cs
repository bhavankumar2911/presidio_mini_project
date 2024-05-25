namespace HotelBookingSystemAPI.Exceptions.Guest
{
    public class GuestPhoneNumberAlreadyInUseException : Exception
    {
        public GuestPhoneNumberAlreadyInUseException(string phone) : base($"The phone number({phone}) is already used by another guest.") { }
    }
}
