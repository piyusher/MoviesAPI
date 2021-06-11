using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using MoviesAPI.Domain;
using MoviesAPI.Services.Interfaces;

namespace MoviesAPI.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}")]
    public class RatingsController : ControllerBase
    {
        private readonly IRatingsControllerService _ratingsService;
        public RatingsController(IRatingsControllerService ratingsService)
        {
            _ratingsService = ratingsService;
        }

        /// <summary> 
        /// Adds or update rating given by a user for a movie
        /// </summary>
        /// <param name="rating">All fields in the payload are mandatory.
        /// Rating should be numeric value between 1 and 5</param>
        /// /// <response code="200">Successfully added/updated rating</response>
        /// <response code="404">Either movie or user with given id doesn't exist</response>
        /// <response code="400">Input validation error.</response>
        /// <response code="500">Exception while processing the request.Log a support ticket.</response>
        [ProducesResponseType(typeof(RatingModel), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        [HttpPost]
        [Route("rating")]
        public async Task<IActionResult> AddUpdateRating(RatingModel rating)
        {

            var (isValid, message) = await _ratingsService.ValidateMovieAndUserId(rating.MovieId, rating.UserId);

            if (!isValid)
            {
                return new NotFoundObjectResult(Problem(
                     statusCode:404,
                     title: MessageStrings.NotFound,
                     detail:message
                ));
            }

            var ratingResult = await _ratingsService.AddUpdateMovieRatingAsync(rating);
            return new OkObjectResult(ratingResult);
        }
    }
}
