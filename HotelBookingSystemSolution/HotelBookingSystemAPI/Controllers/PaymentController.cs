using HotelBookingSystemAPI.Exceptions;
using HotelBookingSystemAPI.Exceptions.Booking;
using HotelBookingSystemAPI.Exceptions.Guest;
using HotelBookingSystemAPI.Exceptions.Room;
using HotelBookingSystemAPI.Models.DTOs.BookingDTOs;
using HotelBookingSystemAPI.Models.DTOs.PaymentDTOs;
using HotelBookingSystemAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Razorpay.Api;

namespace HotelBookingSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IBookingService _bookingService;

        public PaymentController(IPaymentService paymentService, IBookingService bookingService)
        {
            _paymentService = paymentService;
            _bookingService = bookingService;
        }

        [HttpPost("/payment/order")]
        [Authorize(Roles = "guest")]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<SuccessResponse>> GetOrderId(BookingInputDTO bookingInputDTO)
        {
            try
            {
                PaymentOrderIdReturnDTO orderIdReturnDTO = await _bookingService.GivePaymentOrderId(bookingInputDTO);

                return Ok(new SuccessResponse(orderIdReturnDTO));
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
            catch (NoGuestException ex)
            {
                return BadRequest(new ErrorResponse(StatusCodes.Status400BadRequest, ex.Message));
            }
            catch (IncompleteGuestInformationException ex)
            {
                return BadRequest(new ErrorResponse(StatusCodes.Status400BadRequest, ex.Message));
            }
        }

        [Authorize(Roles = "guest")]
        [HttpPost("/payment/verify")]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public ActionResult<SuccessResponse> VerifyPayment(PaymentVerifyInputDTO paymentVerifyInputDTO)
        {
            try
            {
                _paymentService.VerifyPayment(paymentVerifyInputDTO);

                return Ok(new SuccessResponse("Payment verified"));
            } catch (Exception ex)
            {
                return BadRequest(new ErrorResponse(StatusCodes.Status400BadRequest, "Payment failed."));
            }
        }
    }
}
