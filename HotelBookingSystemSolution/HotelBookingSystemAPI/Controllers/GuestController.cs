using HotelBookingSystemAPI.Services;
using HotelBookingSystemAPI.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HotelBookingSystemAPI.Exceptions;
using HotelBookingSystemAPI.Exceptions.Guest;
using HotelBookingSystemAPI.Services.Interfaces;

namespace HotelBookingSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuestController : ControllerBase
    {
        private readonly IGuestService _guestService;
        public GuestController(IGuestService guestService)
        {
            _guestService = guestService;
        }

        [HttpPost("/guest/register")]
        [ProducesResponseType(typeof(RegisterGuestReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<RegisterGuestReturnDTO>> Register(RegisterGuestInputDTO registerGuestInputDTO)
        {
            try
            {
                RegisterGuestReturnDTO registerGuestReturnDTO = await _guestService.RegisterNewGuest(registerGuestInputDTO);

                return Ok(registerGuestReturnDTO);
            }
            catch (GuestEmailAlreadyInUseException ex)
            {
                return Conflict(new ErrorResponse(409, ex.Message));
            }
            catch (GuestPhoneNumberAlreadyInUseException ex)
            {
                return Conflict(new ErrorResponse(409, ex.Message));
            }
        }
    }
}
