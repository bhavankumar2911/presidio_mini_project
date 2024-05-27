using HotelBookingSystemAPI.Exceptions.Guest;
using HotelBookingSystemAPI.Exceptions;
using HotelBookingSystemAPI.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HotelBookingSystemAPI.Services.Interfaces;

namespace HotelBookingSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost("/admin/login")]
        [ProducesResponseType(typeof(LoginAdminReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<LoginAdminReturnDTO>> Login(LoginAdminInputDTO loginAdminInputDTO)
        {
            try
            {
                LoginAdminReturnDTO loginAdminReturn = await _adminService.Login(loginAdminInputDTO);

                return Ok(loginAdminReturn);
            }
            catch (WrongLoginCredentialsException ex)
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
