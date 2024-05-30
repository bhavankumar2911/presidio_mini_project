namespace HotelBookingSystemAPI.Exceptions.Review
{
    public class ReviewNotFoundException : Exception
    {
        public ReviewNotFoundException(int reviewId) : base($"Review with id {reviewId} not found") { }
    }
}
