using HotelBookingSystemAPI.Exceptions;
using HotelBookingSystemAPI.Exceptions.Review;
using HotelBookingSystemAPI.Models.DTOs.BookingDTOs;
using HotelBookingSystemAPI.Models.DTOs.ReviewDTOs;
using HotelBookingSystemAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController (IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [Authorize(Roles = "guest")]
        [HttpPost("/review")]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SuccessResponse>> ReviewHotel(ReviewInputDTO reviewInputDTO)
        {
            int userId = -1;

            foreach (var claim in HttpContext.User.Claims)
            {
                if (claim.Type == "id") userId = Convert.ToInt32(claim.Value);
            }

            try
            {
                await _reviewService.ReviewAHotel(userId, reviewInputDTO);

                return Ok(new SuccessResponse("Review added"));
            } catch (GuestNotBookedException ex)
            {
                return BadRequest(new ErrorResponse(StatusCodes.Status400BadRequest, ex.Message));
            }
        }
    }
}
