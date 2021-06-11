using System.Text;
using System.Threading.Tasks;
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

        public RatingsControllerService(IRatingsRepository ratingsRepo,
            IMoviesRepository moviesRepo,
            IUsersRepository usersRepo)
        {
            _moviesRepo = moviesRepo;
            _ratingsRepo = ratingsRepo;
            _usersRepo = usersRepo;
        }
        public async Task<RatingModel> AddUpdateMovieRatingAsync(RatingModel model)
        {
            MovieRatingEntity updatedEntity;

            var ratingEntity = await _ratingsRepo.GetUserRatingForMovieAsync(model.UserId, model.MovieId);
            
            if (ratingEntity == default(MovieRatingEntity))
            {
                //Rating doesn't exist, add new
                updatedEntity = await _ratingsRepo.AddNewRatingAsync(model.ToMovieRatingsEntity());
            }
            else
            {
                //Rating exists, update
                ratingEntity.Rating = model.Rating;
                updatedEntity = await _ratingsRepo.UpdateRatingAsync(model.ToMovieRatingsEntity());
            }

            //rating has been added/updated, now is time to recalculate the average rating for movie
            await _moviesRepo.RecalculateAvgRatingAsync(model.MovieId);

            return updatedEntity.ToRatingDomainModel();

        }

        public async Task<(bool, string)> ValidateMovieAndUserId(long movieId, long userId)
        {
            var messageBuilder = new StringBuilder();

            var movieEntity = await _moviesRepo.GetMovieByIdAsync(movieId);
            var userEntity = await _usersRepo.GetUserByIdAsync(userId);

            if (movieEntity == default(MovieEntity))
            {
                messageBuilder.Append(string.Format(MessageStrings.MovieDoesNotExist, movieId));
            }
            if (userEntity == default(UserEntity))
            {
                messageBuilder.Append(string.Format(MessageStrings.UserDoesNotExist, userId));
            }

            var message = messageBuilder.ToString();

            return string.IsNullOrEmpty(message) ? (true, message) : (false, message);

        }
    }
}
