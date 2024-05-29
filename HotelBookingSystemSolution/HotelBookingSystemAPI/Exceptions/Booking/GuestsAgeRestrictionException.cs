namespace HotelBookingSystemAPI.Exceptions.Booking
{
    public class GuestsAgeRestrictionException : Exception
    {
        public GuestsAgeRestrictionException() : base ("Atleast one guest must be 18 years old.") { }
    }
}
