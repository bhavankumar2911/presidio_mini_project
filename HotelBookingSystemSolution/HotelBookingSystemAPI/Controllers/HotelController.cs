using HotelBookingSystemAPI.Exceptions.Guest;
using HotelBookingSystemAPI.Exceptions;
using HotelBookingSystemAPI.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HotelBookingSystemAPI.Services.Interfaces;
using HotelBookingSystemAPI.Exceptions.Hotel;
using HotelBookingSystemAPI.Services;

namespace HotelBookingSystemAPI.Controllers
{
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
    }
}
