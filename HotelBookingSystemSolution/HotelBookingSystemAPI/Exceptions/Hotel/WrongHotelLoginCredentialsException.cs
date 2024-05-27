namespace HotelBookingSystemAPI.Exceptions.Hotel
{
    public class WrongHotelLoginCredentialsException : Exception
    {
        public WrongHotelLoginCredentialsException() : base("Email or password is wrong. Try again") { }
    }
}
