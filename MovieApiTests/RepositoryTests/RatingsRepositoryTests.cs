using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.DataAccess;
using MoviesAPI.DataAccess.Entities;
using MoviesAPI.Repositories;
using MoviesAPI.Repositories.Interfaces;
using Xunit;

namespace MovieApiTests.RepositoryTests
{
    public class RatingsRepositoryTests:IDisposable
    {
        private readonly IRatingsRepository _ratingsRepo;
        private readonly MoviesDbContext _dbContext;

        public RatingsRepositoryTests()
        {
            _dbContext = TestDbContext.GetInMemoryDbContext();
            _ratingsRepo = new RatingsRepository(_dbContext);
        }

        [Fact]
        public async Task InsertsNewRating()
        {
            //Count increases after insert
           var countBeforeInsert = await _dbContext.MovieRatings.CountAsync();
           await  _ratingsRepo.AddNewRatingAsync(new MovieRatingEntity()
            {
                MovieId = 25,
                UserId = 29,
                Rating = 4
            });
           var countAfterInsert = await _dbContext.MovieRatings.CountAsync();

           countAfterInsert.Should().Be(countBeforeInsert + 1);
        }

        [Fact]
        public async Task UpdatesExistingRating()
        {
            //Picked up a record from seeded data
            var entity = await _dbContext
                .MovieRatings
                .SingleOrDefaultAsync(r=>r.MovieId == 130 && r.UserId == 2);

            entity.Rating = 2;

            await _ratingsRepo.UpdateRatingAsync(entity);

            entity = await _dbContext
                .MovieRatings
                .SingleOrDefaultAsync(r => r.MovieId == 130 && r.UserId == 2);


            entity.Rating.Should().Be(2);
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}
