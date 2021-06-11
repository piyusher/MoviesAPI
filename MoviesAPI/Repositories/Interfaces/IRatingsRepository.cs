using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoviesAPI.DataAccess.Entities;

namespace MoviesAPI.Repositories.Interfaces
{
    public interface IRatingsRepository
    {
        Task<MovieRatingEntity> AddNewRatingAsync(MovieRatingEntity entity);
        Task<MovieRatingEntity> UpdateRatingAsync(MovieRatingEntity entity);
        Task<MovieRatingEntity> GetUserRatingForMovieAsync(long userId, long movieId);

    }
}
