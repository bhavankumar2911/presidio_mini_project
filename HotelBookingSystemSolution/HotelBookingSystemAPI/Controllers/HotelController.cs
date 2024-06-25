using HotelBookingSystemAPI.Exceptions.Guest;
using HotelBookingSystemAPI.Exceptions;
using HotelBookingSystemAPI.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HotelBookingSystemAPI.Services.Interfaces;
using HotelBookingSystemAPI.Exceptions.Hotel;
using HotelBookingSystemAPI.Services;
using HotelBookingSystemAPI.Models.DTOs.Hotel;
using Microsoft.AspNetCore.Authorization;
using HotelBookingSystemAPI.Models;
using Microsoft.AspNetCore.Cors;

namespace HotelBookingSystemAPI.Controllers
{
    [EnableCors("DefaultPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly IHotelService _hotelService;
        public HotelController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        [HttpPost("/hotel/register")]
        [ProducesResponseType(typeof(RegisterHotelReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<RegisterHotelReturnDTO>> Register(RegisterHotelInputDTO registerHotelInputDTO)
        {
            try
            {
                RegisterHotelReturnDTO registerHotelReturnDTO = await _hotelService.RegisterNewHotel(registerHotelInputDTO);

                return Ok(registerHotelReturnDTO);
            }
            catch (HotelEmailAlreadyInUseException ex)
            {
                return Conflict(new ErrorResponse(409, ex.Message));
            }
            catch (HotelPhoneNumberAlreadyInUseException ex)
            {
                return Conflict(new ErrorResponse(409, ex.Message));
            }
        }

        [HttpPost("/hotel/login")]
        [ProducesResponseType(typeof(LoginHotelReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<LoginHotelReturnDTO>> Login(LoginHotelInputDTO loginHotelInputDTO)
        {
            try
            {
                LoginHotelReturnDTO loginHotelReturnDTO = await _hotelService.LoginHotel(loginHotelInputDTO);

                return Ok(loginHotelReturnDTO);
            }
            catch (WrongGuestLoginCredentialsException ex)
            {
                return Unauthorized(new ErrorResponse(401, ex.Message));
            }
            catch (UnauthorizedException ex)
            {
                return Unauthorized(new ErrorResponse(401, ex.Message));
            }
        }

        [Authorize(Roles = "admin")]
        [HttpGet("/hotels")]
        [ProducesResponseType(typeof(IEnumerable<Hotel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Hotel>>> ListHotels ()
        {
            try
            {
                IEnumerable<Hotel> hotels = await _hotelService.ListAllHotels();

                return Ok(hotels);
            } catch (NoHotelsFoundException ex)
            {
                return NotFound(new ErrorResponse(404, ex.Message));
            }
        }

        [Authorize(Roles = "admin")]
        [HttpGet("/hotels/status")]
        [ProducesResponseType(typeof(IEnumerable<Hotel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Hotel>>> ListHotelsByStatus(bool isApproved)
        {
            try
            {
                IEnumerable<Hotel> hotels = await _hotelService.ListAllHotelsByApprovalStatus(isApproved);

                return Ok(hotels);
            }
            catch (NoHotelsFoundException ex)
            {
                return NotFound(new ErrorResponse(404, ex.Message));
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPut("/hotel/status_update")]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<SuccessResponse>> UpdateHotelApprovalStatus(int hotelId, bool newApprovalStatus)
        {
            try
            {
                await _hotelService.ChangeHotelApprovalStatus(hotelId, newApprovalStatus);

                return Ok(new SuccessResponse("Hotel approval status updated."));
            } catch (HotelNotFoundException ex)
            {
                return NotFound(new ErrorResponse(StatusCodes.Status404NotFound, ex.Message));
            } catch (HotelApprovalException ex)
            {
                return Conflict(new ErrorResponse(StatusCodes.Status409Conflict, ex.Message));
            }
        }
    }
}
