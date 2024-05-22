namespace HotelBookingSystemAPI.Exceptions.User
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(int userId) : base($"No user was found with this id {userId}") { }
    }
}
