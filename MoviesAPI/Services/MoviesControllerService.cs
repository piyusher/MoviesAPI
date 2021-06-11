using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public MoviesControllerService(IMoviesRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<MovieModelWithAvgRating>> SearchMoviesAsync(string title, int year, string genres)
        {
            var listOfGenres = new List<string>();

            //It is common to put a space between comma separated items, trim those
            //Avoid case sensitivity in some DBs,
            //Convert to lower-case to do lower-case comparison in DB
            if (!string.IsNullOrEmpty(genres))
            {
                listOfGenres = genres.Split(',')
                    .Select(genre => genre.Trim().ToLower())
                    .ToList();
            }

            //Get entities and convert to domain model
            var entities = await _repo.SearchMoviesAsync(title, year, listOfGenres);

            //If null return an empty list of models
            return entities.Select(movie => movie.ToMovieDomainModelWithRating()).ToList();
        }

        public async Task<List<MovieModelWithAvgRating>> GetTopRatedMovies()
        {
            var movies = await _repo.GetTopRatedMovies();
            return movies.Select(movie => movie.ToMovieDomainModelWithRating()).ToList();
        }

        public async Task<List<MovieModelWithAvgRating>> GetTopMoviesRatedByUser(long userId)
        {
            var movies = await _repo.GetTopMoviesRatedByUser(userId);
            return movies.Select(movie => movie.ToMovieDomainModelWithRating()).ToList();
        }

    }
}
