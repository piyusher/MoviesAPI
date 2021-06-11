using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoviesAPI.Domain;

namespace MoviesAPI.Services.Interfaces
{
    public interface IRatingsControllerService
    {
        Task<RatingModel> AddUpdateMovieRatingAsync(RatingModel model);
        Task<(bool,string)> ValidateMovieAndUserId(long movieId, long userId);
    }
}
