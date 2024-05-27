namespace HotelBookingSystemAPI.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException() : base("You are unauthorized") { }
    }
}
