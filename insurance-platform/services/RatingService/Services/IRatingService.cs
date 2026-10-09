using RatingService.Models;

namespace RatingService.Services;

public interface IRatingService
{
    Task<RatingResponse> CalculateAutoRatingAsync(
        RatingRequest request);
}