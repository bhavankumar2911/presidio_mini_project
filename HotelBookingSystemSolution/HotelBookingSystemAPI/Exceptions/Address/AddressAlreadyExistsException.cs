namespace HotelBookingSystemAPI.Exceptions.Address
{
    public class AddressAlreadyExistsException : Exception
    {
        public AddressAlreadyExistsException() : base("Another hotel is registered in this address. Kindly check.") { }
    }
}
