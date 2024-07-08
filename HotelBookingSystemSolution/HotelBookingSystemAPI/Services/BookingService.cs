using HotelBookingSystemAPI.Exceptions.Booking;
using HotelBookingSystemAPI.Exceptions.Guest;
using HotelBookingSystemAPI.Exceptions.Room;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Models.DTOs.BookingDTOs;
using HotelBookingSystemAPI.Models.DTOs.BookingGuestDTOs;
using HotelBookingSystemAPI.Models.DTOs.PaymentDTOs;
using HotelBookingSystemAPI.Repository.Interfaces;
using HotelBookingSystemAPI.Services.Interfaces;

namespace HotelBookingSystemAPI.Services
{
    public class BookingService : IBookingService
    {
        private readonly IRepository<int, Room> _roomRepository;
        private readonly IRepository<int, Booking> _bookingRepository;
        private readonly IRepository<int, BookingGuest> _bookingGuestRepository;
        private readonly IPaymentService _paymentService;

        public BookingService (IRepository<int, Room> roomRepository, IRepository<int, Booking> bookingRepository, IRepository<int, BookingGuest> bookingGuestRepository, IPaymentService paymentService)
        {
            _roomRepository = roomRepository;
            _bookingRepository = bookingRepository;
            _bookingGuestRepository = bookingGuestRepository;
            _paymentService = paymentService;
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
                foreach (var booking in bookings)
                {

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

            if (bookingInputDTO.CheckoutDateTime.Subtract(bookingInputDTO.CheckinDateTime).TotalHours < 3) throw new LessBookingTimeException();
        }

        private void ValidateGuests (IList<BookingGuestInputDTO> guests, Room room)
        {
            if (guests.Count() == 0) throw new NoGuestException();

            if (guests.Count() > room.MaxGuests) throw new MaxGuestsLimitException(room.MaxGuests);

            foreach (var guest in guests)
            {
                if (guest.Name == "" || guest.Age == 0 || guest.Gender == "")
                    throw new IncompleteGuestInformationException();
            }

            if (guests.All(g => g.Age < 18))
                throw new GuestsAgeRestrictionException();
        }

        private double CalculateTotalPrice (BookingInputDTO bookingInputDTO, Room room)
        {
            double stayingHours = bookingInputDTO.CheckoutDateTime.Subtract(bookingInputDTO.CheckinDateTime).TotalHours;

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

        public async Task<AmountReturnDTO> CalculateBookingAmount(BookingInputDTO bookingInputDTO) {
            Room room = await _roomRepository.GetByKey(bookingInputDTO.RoomID);

            ValidateCheckinAndCheckout(bookingInputDTO);

            await CheckRoomAvailability(bookingInputDTO, room);

            // ValidateGuests(bookingInputDTO.Guests, room);

            double totalPrice = Math.Round(CalculateTotalPrice(bookingInputDTO, room), 2);

            double amountWithTax = Math.Round(IncludeTaxes(totalPrice, room), 2);

            return new AmountReturnDTO {
                WithTax = amountWithTax,
                WithoutTax = totalPrice
            };
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

            if (bookings.Count() == 0) throw new NoBookingsAvailableException("You don't have any bookings!");
            
            return bookings;
        }

        public async Task<PaymentOrderIdReturnDTO> GivePaymentOrderId(BookingInputDTO bookingInputDTO)
        {
            Room room = await _roomRepository.GetByKey(bookingInputDTO.RoomID);

            ValidateCheckinAndCheckout(bookingInputDTO);

            await CheckRoomAvailability(bookingInputDTO, room);

            ValidateGuests(bookingInputDTO.Guests, room);

            double totalPrice = CalculateTotalPrice(bookingInputDTO, room);

            double amountWithTax = IncludeTaxes(totalPrice, room);

            string orderId = _paymentService.GetPaymentOrderId(amountWithTax);

            return new PaymentOrderIdReturnDTO { OrderId = orderId };
        }
    }
}
