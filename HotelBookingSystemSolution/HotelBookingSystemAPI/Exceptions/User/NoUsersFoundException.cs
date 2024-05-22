namespace HotelBookingSystemAPI.Exceptions.User
{
    public class NoUsersFoundException : Exception
    {
        public NoUsersFoundException() : base("No users are available") { }
    }
}
