namespace HotelBookingSystemAPI.Exceptions.Booking
{
    public class MaxGuestsLimitException : Exception
    {
        public MaxGuestsLimitException(int maxGuest) : base($"This room can accommodate only upto {maxGuest} guests.") { }
    }
}
