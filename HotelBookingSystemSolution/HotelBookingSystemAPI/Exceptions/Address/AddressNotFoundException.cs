namespace HotelBookingSystemAPI.Exceptions.Address
{
    public class AddressNotFoundException : Exception
    {
        public AddressNotFoundException(int guestId) : base($"No address was found with this id {guestId}") { }
    }
}
