using HotelBookingSystemAPI.Exceptions.Guest;
using HotelBookingSystemAPI.Exceptions;
using HotelBookingSystemAPI.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HotelBookingSystemAPI.Services.Interfaces;
using HotelBookingSystemAPI.Exceptions.Hotel;

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
    }
}
