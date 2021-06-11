using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MoviesAPI.DataAccess.Entities;
using MoviesAPI.Domain;
using MoviesAPI.Domain.Converters;
using MoviesAPI.Repositories.Interfaces;
using MoviesAPI.Services.Interfaces;

namespace MoviesAPI.Services
{
    public class RatingsControllerService: IRatingsControllerService
    {
        private readonly IRatingsRepository _ratingsRepo;
        private readonly IMoviesRepository _moviesRepo;
        private readonly IUsersRepository _usersRepo;
        private readonly ILogger<IRatingsControllerService> _logger;

        public RatingsControllerService(IRatingsRepository ratingsRepo,
            IMoviesRepository moviesRepo,
            IUsersRepository usersRepo,
            ILogger<IRatingsControllerService> logger)
        {
            _moviesRepo = moviesRepo;
            _ratingsRepo = ratingsRepo;
            _usersRepo = usersRepo;
            _logger = logger;
        }
        public async Task<RatingModel> AddUpdateMovieRatingAsync(RatingModel model)
        {
            MovieRatingEntity updatedEntity;

            _logger.LogDebug("{@Method}: Checking if rating exists for {@RatingModel}",
                MethodBase.GetCurrentMethod(),
                model);

            var ratingEntity = await _ratingsRepo.GetUserRatingForMovieAsync(model.UserId, model.MovieId);

            
            if (ratingEntity == default(MovieRatingEntity))
            {
                _logger.LogDebug("{@Method}: Inserting new rating {@RatingModel}",
                    MethodBase.GetCurrentMethod(),
                    model);
                //Rating doesn't exist, add new
                updatedEntity = await _ratingsRepo.AddNewRatingAsync(model.ToMovieRatingsEntity());
            }
            else
            {
                _logger.LogDebug("{@Method}: Existing rating {@RatingEntity}",
                    MethodBase.GetCurrentMethod(),
                    ratingEntity);

                _logger.LogDebug("{@Method}: Updating existing rating {@RatingModel}",
                    MethodBase.GetCurrentMethod(),
                    model);

                //Rating exists, update
                ratingEntity.Rating = model.Rating;

                updatedEntity = await _ratingsRepo.UpdateRatingAsync(model.ToMovieRatingsEntity());
            }

            _logger.LogDebug("{@Method}: Recalculating averageRating for {@MovieId}",
                MethodBase.GetCurrentMethod(),
                model.MovieId);

            //rating has been added/updated, now is time to recalculate the average rating for movie
            await _moviesRepo.RecalculateAvgRatingAsync(model.MovieId);

            return updatedEntity.ToRatingDomainModel();

        }

        public async Task<(bool, string)> ValidateMovieAndUserId(long movieId, long userId)
        {
            var messageBuilder = new StringBuilder();
            _logger.LogDebug("{@Method}: Checking if {@UserId} and {@MovieId} exists",
                MethodBase.GetCurrentMethod(),
                userId,movieId);

            var movieEntity = await _moviesRepo.GetMovieByIdAsync(movieId);
            var userEntity = await _usersRepo.GetUserByIdAsync(userId);

            if (movieEntity == default(MovieEntity))
            {
                _logger.LogDebug("{@Method}: {@MovieId} doesn't exist",
                    MethodBase.GetCurrentMethod(),
                     movieId);

                messageBuilder.Append(string.Format(MessageStrings.MovieDoesNotExist, movieId));
            }
            if (userEntity == default(UserEntity))
            {
                _logger.LogDebug("{@Method}: {@UserId} doesn't exist",
                    MethodBase.GetCurrentMethod(),
                    movieId);

                messageBuilder.Append(string.Format(MessageStrings.UserDoesNotExist, userId));
            }

            var message = messageBuilder.ToString();

            return string.IsNullOrEmpty(message) ? (true, message) : (false, message);

        }
    }
}
