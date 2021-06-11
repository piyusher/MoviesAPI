using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MoviesAPI.Domain;
using MoviesAPI.Domain.Converters;
using MoviesAPI.Domain.RequestModels;
using MoviesAPI.Repositories.Interfaces;
using MoviesAPI.Services.Interfaces;

namespace MoviesAPI.Services
{
    public class MoviesControllerService : IMoviesControllerService
    {
        private readonly IMoviesRepository _repo;
        private readonly ILogger<IMoviesControllerService> _logger;

        public MoviesControllerService(IMoviesRepository repo, ILogger<IMoviesControllerService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<List<MovieModelWithAvgRating>> SearchMoviesAsync(string title, int year, string genres)
        {
            var listOfGenres = new List<string>();

            _logger.LogDebug("{@Method}: Converting {@GenresString} to a list of genres",
                MethodBase.GetCurrentMethod(),
                genres);

            //It is common to put a space between comma separated items, trim those
            //Avoid case sensitivity in some DBs,
            //Convert to lower-case to do lower-case comparison in DB
            if (!string.IsNullOrEmpty(genres))
            {
                listOfGenres = genres.Split(',')
                    .Select(genre => genre.Trim().ToLower())
                    .ToList();
            }
            _logger.LogDebug("{@Method}: Generated list of genres: {@GenresList}",
                MethodBase.GetCurrentMethod(),
                listOfGenres);

            //Get entities and convert to domain model
            var entities = await _repo.SearchMoviesAsync(title, year, listOfGenres);
            _logger.LogDebug("{@Method}: Repository returned {@MovieEntities}",
                MethodBase.GetCurrentMethod(),
                entities);



            //If null return an empty list of models
            return entities.Select(movie => movie.ToMovieDomainModelWithRating()).ToList();
        }

        public async Task<List<MovieModelWithAvgRating>> GetTopRatedMovies()
        {
            var movies = await _repo.GetTopRatedMovies();
            
            _logger.LogDebug("{@Method}: Repository returned {@MovieEntities}",
                MethodBase.GetCurrentMethod(),
                movies);

            return movies.Select(movie => movie.ToMovieDomainModelWithRating()).ToList();
        }

        public async Task<List<MovieModelWithAvgRating>> GetTopMoviesRatedByUser(long userId)
        {
            var movies = await _repo.GetTopMoviesRatedByUser(userId);

            _logger.LogDebug("{@Method}: Repository returned {@MovieEntities}",
                MethodBase.GetCurrentMethod(),
                movies);

            return movies.Select(movie => movie.ToMovieDomainModelWithRating()).ToList();
        }

    }
}
