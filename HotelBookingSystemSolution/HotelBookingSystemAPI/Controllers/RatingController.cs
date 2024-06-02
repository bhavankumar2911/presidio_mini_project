using HotelBookingSystemAPI.Exceptions;
using HotelBookingSystemAPI.Exceptions.Hotel;
using HotelBookingSystemAPI.Exceptions.Review;
using HotelBookingSystemAPI.Models.DTOs.RatingDTOs;
using HotelBookingSystemAPI.Models.DTOs.ReviewDTOs;
using HotelBookingSystemAPI.Services;
using HotelBookingSystemAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;

        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [Authorize(Roles = "guest")]
        [HttpPost("/rating")]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SuccessResponse>> RateHotel(RatingInputDTO ratingInputDTO)
        {
            int userId = -1;

            foreach (var claim in HttpContext.User.Claims)
            {
                if (claim.Type == "id") userId = Convert.ToInt32(claim.Value);
            }

            try
            {
                await _ratingService.RateAHotel(userId, ratingInputDTO);

                return Ok(new SuccessResponse("Rating added"));
            }
            catch (GuestNotBookedException ex)
            {
                return BadRequest(new ErrorResponse(StatusCodes.Status400BadRequest, ex.Message));
            }
            catch (HotelNotFoundException ex)
            {
                return NotFound(new ErrorResponse(StatusCodes.Status404NotFound, ex.Message));
            }
        }
    }
}
