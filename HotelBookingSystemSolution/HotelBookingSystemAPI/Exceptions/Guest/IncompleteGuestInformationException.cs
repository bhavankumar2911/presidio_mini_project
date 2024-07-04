namespace HotelBookingSystemAPI.Exceptions.Guest
{
    public class IncompleteGuestInformationException : Exception
    {
        public IncompleteGuestInformationException() : base("Name, age, and gender is required for all the guests.") { }
    }
}
