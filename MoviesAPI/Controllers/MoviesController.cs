using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Domain;
using MoviesAPI.Domain.RequestModels;
using MoviesAPI.FiltersAttributes;
using MoviesAPI.Services.Interfaces;

namespace MoviesAPI.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}")]
    public class MoviesController : ControllerBase
    {
        private readonly IMoviesControllerService _moviesSvc;

        public MoviesController(IMoviesControllerService moviesSvc)
        {
            _moviesSvc = moviesSvc;
        }

        /// <summary> 
        /// Searches movies using: text in title, genres or/and year of release.
        /// One criteria is mandatory.
        /// </summary>
        /// <response code="200">Success. Movies with given criteria are found</response>
        /// <response code="404">No Movies found for the given criteria.</response>
        /// <response code="400">Input validation error.</response>
        /// <response code="500">Exception while processing the request.Log a support ticket.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<MovieModel>),200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        [Route("movies/search")]
        public async Task<IActionResult> SearchMoviesAsync([FromQuery]SearchMovieFilters filters)
        {
            //If no search criteria is provided return bad request
            if (string.IsNullOrEmpty(filters.Genres?.Trim())
                && string.IsNullOrEmpty(filters.Title.Trim())
                && filters.Year == 0)
            {
                ModelState.AddModelError("NoCriteria",MessageStrings.NoSearchCriteria);
                return new BadRequestObjectResult(ValidationProblem(ModelState));
            }

            //get data from the service
            var movieList = await _moviesSvc.SearchMoviesAsync(filters);

            //Return 404 is no movies found
            if (movieList.Count == 0)
            {
                return new NotFoundObjectResult(
                    Problem(statusCode:404,
                    title: MessageStrings.NoResultsFound,
                    detail:$"Title:{filters.Title}, Year:{filters.Year}, genres:{filters.Genres}"
                    ));
            }

            return new OkObjectResult(movieList);
        }

        /// <summary> 
        /// Gets 5 top rated movies based on overall average rating.
        /// </summary>
        /// <response code="200">Top 5 rated movies</response>
        /// <response code="404">Movies database is empty</response>
        /// <response code="500">Exception while processing the request.Log a support ticket.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<MovieModel>), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        //EXPLANATION: We can follow the strict REST routing convention and use the /movies endpoint with filters:
        // /api/v1/movies?OrderBy=avgrating&OrderByDesc=true
        //Just added 'top-rated' route as it is more friendly to end user

        [Route("movies/top-rated")]
        public async Task<IActionResult> GetTopRatedMovies()
        {
            //get data from the service
            var movieList = await _moviesSvc.GetTopRatedMovies();

            //Return 404 is no movies found
            if (movieList.Count == 0)
            {
                return new NotFoundObjectResult(
                    Problem(statusCode: 404,
                    title: MessageStrings.NoResultsFound,
                    detail: MessageStrings.MovieDbEmpty
                    ));
            }

            return new OkObjectResult(movieList);
        }

        /// <summary> 
        /// Gets 5 top rated movies based on overall average rating.
        /// </summary>
        /// <response code="200">Top 5 movies rated by the given user</response>
        /// <response code="400">UserId cannot be parsed as a number</response>
        /// <response code="404">The given user didn't rate any movie</response>
        /// <response code="500">Exception while processing the request.Log a support ticket.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<MovieModel>), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        //EXPLANATION: This is a very unique use case.
        //Typically in terms of REST, this should have been done using a user/ratings endpoint
        //where it would have given an ordered list of ratings given to movies by a user
        //and then we could have fetched the top 5 movies and their averageRatings

        //Going with a 'top-rated-by-user' route for now as it is more friendly to end user
        [Route("movies/top-rated-by-user/{userId}")]
        public async Task<IActionResult> GetTopMoviesRatedByUser([Range(1, long.MaxValue)]long userId)
        {
            //get data from the service
            var movieList = await _moviesSvc.GetTopMoviesRatedByUser(userId);

            //Return 404 is no movies found
            if (movieList.Count == 0)
            {
                return new NotFoundObjectResult(
                    Problem(statusCode: 404,
                        title: MessageStrings.NoResultsFound,
                        detail: string.Format(MessageStrings.NoMoviesRatedByUser, userId)
                    ));
            }

            return new OkObjectResult(movieList);
        }



    }
}
