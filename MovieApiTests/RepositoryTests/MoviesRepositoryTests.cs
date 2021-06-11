
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.DataAccess;
using MoviesAPI.DataAccess.DataSeed;
using MoviesAPI.DataAccess.Entities;
using MoviesAPI.Repositories;
using MoviesAPI.Repositories.Interfaces;
using Xunit;

namespace MovieApiTests.RepositoryTests
{
    public class MoviesRepositoryTests:IDisposable
    {
        private readonly IMoviesRepository _moviesRepo;
        private readonly MoviesDbContext _dbContext;

        public MoviesRepositoryTests()
        {  
            _dbContext = TestDbContext.GetInMemoryDbContext();
            _moviesRepo = new MoviesRepository(_dbContext);
        }

        [Fact]
        public async Task GetMovieByIdReturnsNullIfNotFound()
        {
            var entity = await _moviesRepo.GetMovieByIdAsync(500);
            entity.Should().BeNull();
        }

        [Fact]
        public async Task GetMovieByIdReturnsMovieEntity()
        {
            var entity = await _moviesRepo.GetMovieByIdAsync(125);
            entity.Should().NotBeNull();
            entity.Should().BeOfType<MovieEntity>();
            entity.Should().BeEquivalentTo(DataSeeder.Data
                .Movies
                .Single(x=>x.MovieId == 125),
                //AvgRating and Genres are assigned on while seeding.
                //Static data doesn't have these fields
                options => options
                    .Excluding(m => m.AvgRating)
                    .Excluding(m=>m.MovieGenres)
                    .Excluding(m => m.Genres));
        }

        [Fact]
        public async Task SearchMoviesReturnsEmptyList()
        {
            //Minimum year value for data is 1850
            var entity = await _moviesRepo.SearchMoviesAsync(null,1600,null);
            entity.Should().NotBeNull();
            entity.Should().BeEmpty();
        }

        [Fact]
        public async Task SearchMoviesWithOneFilterAtTime()
        {
            //Search with Title works
            //We added a movie The Shawshank Redemption
            var entity = await _moviesRepo.
                SearchMoviesAsync("Shawshank", 0, null);

            entity.Should().NotBeEmpty();
            entity.Should().Contain(m => m.Title.Contains("Shawshank"));

            //Search with Genre works
            //now search for Genre 'Romantic' and verify all returned movies have this genre
            entity = await _moviesRepo.SearchMoviesAsync("",0,new List<string>(){ "romance" });

            entity.Should().NotBeEmpty();
            //verify all entities have this genre
            entity.All(m => m.MovieGenres.Any(mg=>mg.Genre.GenreName.ToLower() == "romance"))
                .Should().BeTrue();

            //Searching with Year works
            entity = await _moviesRepo.SearchMoviesAsync("",2000,null);
            entity.All(m => m.Year.Equals(2000)).Should().BeTrue();

        }

        [Fact]
        public async Task SearchMoviesWithFilterCombination()
        {
            //Search with all three filters present
            //We added a movie The Shawshank Redemption, 1994, Drama
            var entity = await _moviesRepo.
                SearchMoviesAsync("Shawshank", 1994, new List<string>() { "drama" });

            entity.Should().NotBeEmpty();
            entity.Should().HaveCount(1);
            entity.Should().Contain(m => m.Title.Contains("Shawshank"));

            //Search with Two Filters works
            //now search for Genre 'Romance' and year 2000 movies
            entity = await _moviesRepo.SearchMoviesAsync("", 2000, new List<string>() { "romance" });

            entity.Should().NotBeEmpty();
            //verify all entities have this genre and year
            entity.All(m => m.MovieGenres.Any(mg => mg.Genre.GenreName.ToLower() == "romance")
                && m.Year == 2000)
                .Should().BeTrue();

            //Searching with Title and year works
            entity = await _moviesRepo.SearchMoviesAsync("The", 1994, null);
            //verify all entities have year and text in the title
            entity.All(m => m.Year.Equals(1994) && m.Title.ToLower().Contains("the"))
                .Should().BeTrue();

        }

        [Fact]
        public async Task GetTop5RatedMoviesOrdersByRatings()
        {
           var entityToBeChecked = _dbContext.Movies
               .Include(x=>x.MovieGenres)
               .ThenInclude(x=>x.Genre)
               .First(x => x.AvgRating == 0);
            //we update avg rating to 6 since we don't have any > 5 in DB
            //this should be on the top of the list
           entityToBeChecked.AvgRating = 6;
           await _dbContext.SaveChangesAsync(true);

           //The updated entity should on the top of the list
           var topList = await _moviesRepo.GetTopRatedMovies();
           topList.Should().NotBeEmpty();
           topList.First().Title.Should().Be(entityToBeChecked.Title);
        }

        [Fact]
        public async Task TopRatedOrderedByTitleIfSameRating()
        {
            //The updated entity should on the top of the list
            var topList = await _moviesRepo.GetTopRatedMovies();
            topList.Should().NotBeEmpty();
            topList.Should().BeInDescendingOrder(m => m.AvgRating);
            topList.Should().BeInAscendingOrder(m => m.Title);

        }

        [Fact]
        public async Task TopRatedByUserOrderedByUserRating()
        {
            var entityToBeChecked = _dbContext.MovieRatings
                .Include(x => x.User)
                .First(x => x.UserId == 1);

            //we update avg rating to 6 since we don't have any > 5 in DB
            //this should be on the top of the list
            entityToBeChecked.Rating = 6;
            await _dbContext.SaveChangesAsync(true);

            //The updated entity should on the top of the list
            var topList = await _moviesRepo.GetTopMoviesRatedByUser(1);
            topList.Should().NotBeEmpty();
            topList.First().MovieId.Should().Be(entityToBeChecked.MovieId);
        }

        [Fact]
        public async Task TopRatedByUserOrderedByTitleIfSameRating()
        {
            //Build expected list
            var queryable = _dbContext.MovieRatings
                .Include(rating => rating.User)
                .Include(rating => rating.Movie)
                .ThenInclude(movie => movie.MovieGenres)
                .ThenInclude(movieGenre => movieGenre.Genre);

            var expectedList = await queryable
                .Where(rating => rating.UserId == 1)
                .OrderByDescending(rating => rating.Rating)
                .ThenBy(rating => rating.Movie.Title)
                .Select(rating => rating.Movie)
                .Take(5)
                .ToListAsync();

            //The updated entity should on the top of the list
            var topList = await _moviesRepo.GetTopMoviesRatedByUser(1);
            topList.Should().BeEquivalentTo(expectedList);
        }

        [Fact]
        public async Task TopRatedByUserReturnsEmptyList()
        {
            //The updated entity should on the top of the list
            //User 50 isn't there. No ratings by the user
            var topList = await _moviesRepo.GetTopMoviesRatedByUser(50);
            topList.Should().BeEmpty();
        }

        [Fact]
        public async Task RecalculatesAvgRating()
        {
            //per seeded data  "userId": 16,"movieId": 40,"rating": 4

            var movie = await _moviesRepo.GetMovieByIdAsync(40);
            Math.Round(movie.AvgRating,1).Should().Be((decimal)4.7);

            //Update rating
           var entity = await _dbContext.MovieRatings.SingleAsync(rating => rating.UserId == 16 && rating.MovieId == 40);
           entity.Rating = 5;
           await _dbContext.SaveChangesAsync(true);

           //Call repository method to recalculate
           await _moviesRepo.RecalculateAvgRatingAsync(40);

           movie = await _moviesRepo.GetMovieByIdAsync(40);
           movie.AvgRating.Should().Be(5);


        }


        public void Dispose()
        {
            //Destroy database after test executes
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }

}
