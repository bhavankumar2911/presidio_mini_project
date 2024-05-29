namespace HotelBookingSystemAPI.Exceptions.Booking
{
    public class BookingNotFoundException : Exception
    {
        public BookingNotFoundException(int bookingId) : base ($"No booking was found with the id {bookingId}") { }
    }
}
