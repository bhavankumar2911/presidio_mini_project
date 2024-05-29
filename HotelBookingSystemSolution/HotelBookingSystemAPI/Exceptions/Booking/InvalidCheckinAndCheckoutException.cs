namespace HotelBookingSystemAPI.Exceptions.Booking
{
    public class InvalidCheckinAndCheckoutException : Exception
    {
        public InvalidCheckinAndCheckoutException() : base ("Invalid checkin and checkout details.") { }
    }
}
