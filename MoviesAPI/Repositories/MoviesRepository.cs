using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.DataAccess;
using MoviesAPI.DataAccess.Entities;
using MoviesAPI.Extensions;
using MoviesAPI.Repositories.Interfaces;

namespace MoviesAPI.Repositories
{
    public class MoviesRepository: IMoviesRepository
    {
        private readonly MoviesDbContext _dbContext;

        public MoviesRepository(MoviesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //This method is used to check if movie exists
        //Also, this method is returning a tracked entity which is used for update 
        public async Task<MovieEntity> GetMovieByIdAsync(long movieId)
        {
           return await _dbContext
               .Movies
               .SingleOrDefaultAsync(movie => movie.MovieId == movieId);
        }

        public async Task<List<MovieEntity>> SearchMoviesAsync(string title, int year, List<string> genres)
        {
            var moviesQueryable = _dbContext.Movies
                .Include(movie => movie.MovieGenres)
                .ThenInclude(movieGenre => movieGenre.Genre)
                //NoTracking, we don't want these entities to be tracked
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrEmpty(title))
            {
                //Do a same-case comparison. Some DBs are case-sensitive
                moviesQueryable = moviesQueryable.Where(movie => movie.Title.ToLower()
                    .Contains(title.ToLower()));
            }

            if (year != 0)
            {
                //Match the year
                moviesQueryable = moviesQueryable.Where(movie => movie.Year == year);
            }

            if (genres != null && genres.Count > 0)
            {
                //If genres are given in the input, match them
                moviesQueryable = moviesQueryable.Where(movie =>
                    movie.MovieGenres.Any(mg => 
                        genres.Contains(mg.Genre.GenreName.ToLower())));
            }
            //order by title
            moviesQueryable = moviesQueryable.OrderBy(movie => movie.Title);

            return await moviesQueryable.ToListAsync();
        }

        public async Task<List<MovieEntity>> GetTopRatedMovies()
        {
            var moviesQueryable = _dbContext.Movies
                .Include(movie => movie.MovieGenres)
                .ThenInclude(movieGenre => movieGenre.Genre)
                //NoTracking, we don't want these entities to be tracked
                .AsNoTracking()
                .AsQueryable();

            return await moviesQueryable
                .OrderByDescending(movie => movie.AvgRating)
                .ThenBy(movie=>movie.Title)
                .Take(5)
                .ToListAsync();
        }

        public async Task<List<MovieEntity>> GetTopMoviesRatedByUser(long userId)
        {
            var queryable = _dbContext.MovieRatings
                .Include(rating => rating.User)
                .Include(rating => rating.Movie)
                .ThenInclude(movie => movie.MovieGenres)
                .ThenInclude(movieGenre => movieGenre.Genre);

            return await queryable
                .Where(rating => rating.UserId == userId)
                .OrderByDescending(rating => rating.Rating)
                .ThenBy(rating => rating.Movie.Title)
                .Select(rating => rating.Movie)
                .Take(5)
                .ToListAsync();
        }

        public async Task RecalculateAvgRatingAsync(long movieId)
        {
            //get the tracked entity
            var movieEntity = await GetMovieByIdAsync(movieId);

            //calculate current average rating
            var currentAvgRating = await _dbContext
                .MovieRatings
                .Where(x => x.MovieId == movieId)
                .AverageAsync(x => x.Rating);

            //set the new rating
            movieEntity.AvgRating = (decimal)currentAvgRating;

            //update entity
            _dbContext.Movies.Update(movieEntity);

            await _dbContext.SaveChangesAsync();


        }
    }
}
