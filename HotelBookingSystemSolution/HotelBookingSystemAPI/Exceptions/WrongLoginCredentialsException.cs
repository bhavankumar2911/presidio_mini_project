namespace HotelBookingSystemAPI.Exceptions
{
    public class WrongLoginCredentialsException : Exception
    {
        public WrongLoginCredentialsException() : base("Email or password is wrong. Try again") { }
    }
}
