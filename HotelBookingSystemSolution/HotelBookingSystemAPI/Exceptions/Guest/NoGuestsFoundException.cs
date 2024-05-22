namespace HotelBookingSystemAPI.Exceptions.Guest
{
    public class NoGuestsFoundException : Exception
    {
        public NoGuestsFoundException() : base("No guests are available") { }
    }
}
