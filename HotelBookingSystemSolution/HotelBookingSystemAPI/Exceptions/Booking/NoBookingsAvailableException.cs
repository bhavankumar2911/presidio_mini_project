﻿namespace HotelBookingSystemAPI.Exceptions.Booking
{
    public class NoBookingsAvailableException : Exception
    {
        public NoBookingsAvailableException() : base("You didn't book any rooms.") { }
    }
}
