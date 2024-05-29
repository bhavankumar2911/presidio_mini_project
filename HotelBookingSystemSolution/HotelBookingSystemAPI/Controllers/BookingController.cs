using HotelBookingSystemAPI.Exceptions;
using HotelBookingSystemAPI.Exceptions.Booking;
using HotelBookingSystemAPI.Exceptions.Room;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Models.DTOs.BookingDTOs;
using HotelBookingSystemAPI.Models.DTOs.RoomDTOs;
using HotelBookingSystemAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [Authorize(Roles = "guest")]
        [HttpPost("/book")]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<SuccessResponse>> BookRoom(BookingInputDTO bookingInputDTO)
        {
            try
            {
                int guestId = -1;

                foreach (var claim in HttpContext.User.Claims)
                {
                    if (claim.Type == "id") guestId = Convert.ToInt32(claim.Value);
                }

                Booking booking = await _bookingService.BookRoom(bookingInputDTO, guestId);

                return Ok(new SuccessResponse("Room booked", booking));

            }
            catch (InvalidCheckinAndCheckoutException ex)
            {
                return BadRequest(new ErrorResponse(StatusCodes.Status400BadRequest, ex.Message));
            }
            catch (LessBookingTimeException ex)
            {
                return BadRequest(new ErrorResponse(StatusCodes.Status422UnprocessableEntity, ex.Message));
            }
            catch (RoomNotAvailableException ex)
            {
                return BadRequest(new ErrorResponse(StatusCodes.Status404NotFound, ex.Message));
            }
            catch (RoomAlreadyBookedException ex)
            {
                return BadRequest(new ErrorResponse(StatusCodes.Status409Conflict, ex.Message));
            }
            catch (MaxGuestsLimitException ex)
            {
                return BadRequest(new ErrorResponse(StatusCodes.Status400BadRequest, ex.Message));
            }
            catch (GuestsAgeRestrictionException ex)
            {
                return BadRequest(new ErrorResponse(StatusCodes.Status400BadRequest, ex.Message));
            }
            catch (RoomNotFoundException ex)
            {
                return BadRequest(new ErrorResponse(StatusCodes.Status404NotFound, ex.Message));
            }
        }

        [Authorize(Roles = "guest")]
        [HttpGet("/guest/bookings")]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SuccessResponse>> GetGuestBookings ()
        {
            try
            {
                int guestId = -1;

                foreach (var claim in HttpContext.User.Claims)
                {
                    if (claim.Type == "id") guestId = Convert.ToInt32(claim.Value);
                }

                IEnumerable<Booking> bookings = await _bookingService.ViewGuestBookings(guestId);

                return Ok(new SuccessResponse(bookings));
            }
            catch (NoBookingsAvailableException ex)
            {
                return NotFound(new ErrorResponse(StatusCodes.Status404NotFound, ex.Message));
            }
        }
    }
}
