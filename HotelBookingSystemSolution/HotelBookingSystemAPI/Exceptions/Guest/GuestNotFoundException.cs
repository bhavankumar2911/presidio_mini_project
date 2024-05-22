namespace HotelBookingSystemAPI.Exceptions.Guest
{
    public class GuestNotFoundException : Exception
    {
        public GuestNotFoundException(int guestId) : base($"No guest was found with this id {guestId}") { }
    }
}
