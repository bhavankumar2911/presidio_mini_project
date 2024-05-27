namespace HotelBookingSystemAPI.Exceptions.Address
{
    public class NoAddressesFoundException : Exception
    {
        public NoAddressesFoundException() : base("No addresses are available") { }
    }
}
