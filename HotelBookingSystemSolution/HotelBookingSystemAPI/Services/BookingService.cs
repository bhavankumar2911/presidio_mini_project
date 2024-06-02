using HotelBookingSystemAPI.Exceptions.Booking;
using HotelBookingSystemAPI.Exceptions.Room;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Models.DTOs.BookingDTOs;
using HotelBookingSystemAPI.Models.DTOs.BookingGuestDTOs;
using HotelBookingSystemAPI.Repository.Interfaces;
using HotelBookingSystemAPI.Services.Interfaces;

namespace HotelBookingSystemAPI.Services
{
    public class BookingService : IBookingService
    {
        private readonly IRepository<int, Room> _roomRepository;
        private readonly IRepository<int, Booking> _bookingRepository;
        private readonly IRepository<int, BookingGuest> _bookingGuestRepository;

        public BookingService (IRepository<int, Room> roomRepository, IRepository<int, Booking> bookingRepository, IRepository<int, BookingGuest> bookingGuestRepository)
        {
            _roomRepository = roomRepository;
            _bookingRepository = bookingRepository;
            _bookingGuestRepository = bookingGuestRepository;
        }

        private async Task CheckRoomAvailability (BookingInputDTO bookingInputDTO, Room room)
        {
            if (!room.IsAvailable) throw new RoomNotAvailableException();

            // check other bookings for the room
            IEnumerable<Booking> bookings = await _bookingRepository.GetAll();
            bookings = bookings.Where(b => b.Room.Id == bookingInputDTO.RoomID);

            int c = bookings.Count();

            if (bookings.Count() > 0)
            {
                //bookings = bookings.OrderBy(b => b.CheckoutDateTime).ToList();

                //Booking latestBooking = bookings.Last();

                //Console.WriteLine("\ndate time: \n" + bookingInputDTO.CheckinDateTime);
                //Console.WriteLine ("\ndate time: \n" + latestBooking.CheckoutDateTime);

                //if (bookingInputDTO.CheckinDateTime < latestBooking.CheckoutDateTime) throw new RoomAlreadyBookedException();

                //DateTime.TryParse(bookingInputDTO.CheckinDateTime.ToString(), out DateTime newIn);
                //DateTime.TryParse(bookingInputDTO.CheckoutDateTime.ToString(), out DateTime newOut);
                foreach (var booking in bookings)
                {
                    //DateTime.TryParse(booking.CheckinDateTime.ToString(), out DateTime oldIn);
                    //DateTime.TryParse(booking.CheckoutDateTime.ToString(), out DateTime oldOut);

                    //if ((newOut > oldIn && newOut < oldOut) ||
                    //    (newIn > oldIn && newIn < oldOut) ||
                    //    (newIn < oldIn && newOut > oldOut) ||
                    //    (newIn == oldIn && newOut == oldOut) ||
                    //    (newIn < oldIn && newOut == oldOut) ||
                    //    (newIn == oldIn && newOut > oldOut))
                    //        {
                    //    throw new RoomAlreadyBookedException();
                    //        }

                    if ((bookingInputDTO.CheckoutDateTime > booking.CheckinDateTime &&
                        bookingInputDTO.CheckoutDateTime < booking.CheckoutDateTime) ||
                        (bookingInputDTO.CheckinDateTime > booking.CheckinDateTime &&
                        bookingInputDTO.CheckinDateTime < booking.CheckoutDateTime) ||
                        (bookingInputDTO.CheckinDateTime < booking.CheckinDateTime &&
                        bookingInputDTO.CheckoutDateTime > booking.CheckoutDateTime) ||
                        (bookingInputDTO.CheckinDateTime == booking.CheckinDateTime &&
                        bookingInputDTO.CheckoutDateTime == booking.CheckoutDateTime) ||
                        (bookingInputDTO.CheckinDateTime < booking.CheckinDateTime && bookingInputDTO.CheckoutDateTime == booking.CheckoutDateTime) || (bookingInputDTO.CheckinDateTime == booking.CheckinDateTime && bookingInputDTO.CheckoutDateTime > booking.CheckoutDateTime))
                    {
                        throw new RoomAlreadyBookedException();
                    }

                }
            }
        }

        private void ValidateCheckinAndCheckout (BookingInputDTO bookingInputDTO)
        {
            if (bookingInputDTO.CheckoutDateTime < bookingInputDTO.CheckinDateTime) throw new InvalidCheckinAndCheckoutException();

            if (bookingInputDTO.CheckoutDateTime.Subtract(bookingInputDTO.CheckinDateTime).Hours < 3) throw new LessBookingTimeException();
        }

        private void ValidateGuests (IList<BookingGuestInputDTO> guests, Room room)
        {
            if (guests.Count() > room.MaxGuests) throw new MaxGuestsLimitException(room.MaxGuests);

            if (guests.All(g => g.Age < 18))
                throw new GuestsAgeRestrictionException();
        }

        private double CalculateTotalPrice (BookingInputDTO bookingInputDTO, Room room)
        {
            int stayingHours = bookingInputDTO.CheckoutDateTime.Subtract(bookingInputDTO.CheckinDateTime).Hours;

            double price = stayingHours * (room.PricePerDay / 24);

            return price;
        }

        private double IncludeTaxes (double totalPrice, Room room)
        {
            if (room.PricePerDay > 7500)
                return totalPrice + (0.18 * totalPrice);

            return totalPrice + (0.12 * totalPrice);
        }

        private async Task SaveBookingGuests (IList<BookingGuestInputDTO> guests, int bookingId)
        {
            foreach (var guest in guests)
            {
                BookingGuest bookingGuest = new BookingGuest()
                { 
                    Name = guest.Name,
                    Gender = guest.Gender,
                    Age = guest.Age,
                    BookingId = bookingId,
                };

                await _bookingGuestRepository.Add(bookingGuest);
            }
        }

        public async Task<Booking> BookRoom(BookingInputDTO bookingInputDTO, int bookingGuestId)
        {
            Room room = await _roomRepository.GetByKey(bookingInputDTO.RoomID);

            ValidateCheckinAndCheckout(bookingInputDTO);

            await CheckRoomAvailability(bookingInputDTO, room);

            ValidateGuests(bookingInputDTO.Guests, room);

            double totalPrice = CalculateTotalPrice(bookingInputDTO, room);

            double amountWithTax = IncludeTaxes(totalPrice, room);

            Booking booking = new Booking()
            {
                DateOfBooking = DateTime.Now,
                CheckinDateTime = bookingInputDTO.CheckinDateTime,
                CheckoutDateTime = bookingInputDTO.CheckoutDateTime,
                Amount = amountWithTax,
                RoomID = room.Id,
                GuestId = bookingGuestId
            };

            Booking newBooking = await _bookingRepository.Add(booking);

            await SaveBookingGuests(bookingInputDTO.Guests, booking.Id);

            return newBooking;
        }

        public async Task<IEnumerable<Booking>> ViewGuestBookings(int guestId)
        {
            IEnumerable<Booking> bookings = await _bookingRepository.GetAll();
            bookings = bookings.Where(b => b.Guest.Id == guestId).OrderByDescending(b => b.DateOfBooking);

            if (bookings.Count() == 0) throw new NoBookingsAvailableException();

            return bookings;
        }

        public async Task<IEnumerable<Booking>> ViewHotelBookings(int hotelId)
        {
            IEnumerable<Booking> bookings = await _bookingRepository.GetAll();

            bookings = bookings.Where(b => b.Room.Hotel.Id == hotelId).OrderByDescending(b => b.DateOfBooking);

            if (bookings.Count() == 0) throw new NoBookingsAvailableException();
            
            return bookings;
        }
    }
}
