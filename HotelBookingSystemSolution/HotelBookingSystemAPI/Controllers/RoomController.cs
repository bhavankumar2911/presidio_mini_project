using HotelBookingSystemAPI.Exceptions;
using HotelBookingSystemAPI.Exceptions.Hotel;
using HotelBookingSystemAPI.Exceptions.Room;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Models.DTOs.RoomDTOs;
using HotelBookingSystemAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;
        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [Authorize(Roles = "hotel")]
        [HttpPost("/room")]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<SuccessResponse>> AddRoom(RoomInputDTO roomInputDTO)
        {
            try
            {
                int hotelId = -1;

                foreach (var claim in HttpContext.User.Claims)
                {
                    if (claim.Type == "id") hotelId = Convert.ToInt32(claim.Value);
                }

                Room room = await _roomService.AddNewRoom(roomInputDTO, hotelId);

                return Ok(new SuccessResponse("Room added.", room));
            } catch(HotelNotFoundException ex)
            {
                return NotFound(new ErrorResponse(StatusCodes.Status404NotFound, ex.Message));
            } catch(RoomNumberAlreadyInUseException ex)
            {
                return Conflict(new ErrorResponse(StatusCodes.Status409Conflict, ex.Message));
            }
        }

        [HttpGet("/room")]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SuccessResponse>> ListRooms([FromQuery]RoomFilterDTO roomFilterDTO)
        {
            try
            {
                IEnumerable<Room> rooms = await _roomService.ListRoomsForBooking(roomFilterDTO);

                return Ok(new SuccessResponse(rooms));
            } catch (NoRoomsAvailableExpection ex)
            {
                return NotFound(new ErrorResponse(StatusCodes.Status404NotFound, ex.Message));
            }
        }

        [HttpGet("/room/{roomId}")]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SuccessResponse>> GetRoom (int roomId)
        {
            try
            {
                Room room = await _roomService.ViewSingleRoom(roomId);
                return Ok(new SuccessResponse(room));
            } catch (RoomNotFoundException ex)
            {
                return NotFound(new ErrorResponse(StatusCodes.Status404NotFound, ex.Message));
            }
        }
    }
}
