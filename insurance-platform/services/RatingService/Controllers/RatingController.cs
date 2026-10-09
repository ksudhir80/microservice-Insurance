using Microsoft.AspNetCore.Mvc;
using RatingService.Models;
using RatingService.Services;

namespace RatingService.Controllers;

[ApiController]
[Route("api/rating")]
public class RatingController : ControllerBase
{
    private readonly IRatingService _ratingService;

    public RatingController(IRatingService ratingService)
    {
        _ratingService = ratingService;
    }

    [HttpPost("auto")]
    public async Task<IActionResult> CalculateAutoRating(
        [FromBody] RatingRequest request)
    {
        if (request.VehicleYear < 1900)
        {
            return BadRequest(new
            {
                message = "Invalid vehicle year."
            });
        }

        if (request.DriverAge < 16)
        {
            return BadRequest(new
            {
                message = "Driver age must be at least 16."
            });
        }

        if (request.Accidents < 0 ||
            request.Violations < 0)
        {
            return BadRequest(new
            {
                message = "Accidents and violations cannot be negative."
            });
        }

        var result =
            await _ratingService.CalculateAutoRatingAsync(request);

        return Ok(result);
    }
}