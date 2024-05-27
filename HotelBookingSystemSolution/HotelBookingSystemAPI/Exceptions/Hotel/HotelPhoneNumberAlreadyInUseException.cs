namespace HotelBookingSystemAPI.Exceptions.Hotel
{
    public class HotelPhoneNumberAlreadyInUseException : Exception
    {
        public HotelPhoneNumberAlreadyInUseException(string phone) : base($"The phone number({phone}) is already used by another hotel.") { }
    }
}
