namespace HotelBookingSystemAPI.Exceptions.Review
{
    public class GuestNotBookedException : Exception
    {
        public GuestNotBookedException(): base("You have not booked this hotel before to give a review.") { }
    }
}
