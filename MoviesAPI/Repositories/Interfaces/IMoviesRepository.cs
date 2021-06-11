using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoviesAPI.DataAccess.Entities;

namespace MoviesAPI.Repositories.Interfaces
{
    public interface IMoviesRepository
    {
        Task<MovieEntity> GetMovieByIdAsync(long movieId);
        Task<List<MovieEntity>> SearchMoviesAsync(string title, int year, List<string> genres);
        Task<List<MovieEntity>> GetTopRatedMovies();
        Task<List<MovieEntity>> GetTopMoviesRatedByUser(long userId);
        Task RecalculateAvgRatingAsync(long movieId);
    }
}
