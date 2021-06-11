using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.DataAccess;
using MoviesAPI.DataAccess.Entities;
using MoviesAPI.Repositories.Interfaces;

namespace MoviesAPI.Repositories
{
    public class RatingsRepository:IRatingsRepository
    {
        private readonly MoviesDbContext _dbContext;

        public RatingsRepository(MoviesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<MovieRatingEntity> AddNewRatingAsync(MovieRatingEntity entity)
        {
            _dbContext.MovieRatings.Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<MovieRatingEntity> UpdateRatingAsync(MovieRatingEntity entity)
        {
            _dbContext.MovieRatings.Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<MovieRatingEntity> GetUserRatingForMovieAsync(long userId, long movieId)
        {
            //we want this entity to be tracked for update
            return await _dbContext
                .MovieRatings
                .SingleOrDefaultAsync(rating=> rating.MovieId == movieId && rating.UserId == userId);
        }
    }
}
