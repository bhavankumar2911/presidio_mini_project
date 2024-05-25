namespace HotelBookingSystemAPI.Exceptions.Guest
{
    public class WrongGuestLoginCredentialsException : Exception
    {
        public WrongGuestLoginCredentialsException() : base("Email or password is wrong. Try again") { }
    }
}
