using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using MoviesAPI.DataAccess.Entities;
using MoviesAPI.Domain;
using MoviesAPI.Domain.RequestModels;
using MoviesAPI.Repositories.Interfaces;
using MoviesAPI.Services;
using MoviesAPI.Services.Interfaces;
using Xunit;

namespace MovieApiTests.Services
{
    public class MovieControllerServiceTests
    {
        private readonly IMoviesControllerService _svc;
        private readonly Mock<IMoviesRepository> _mockMoviesRepo;

        public MovieControllerServiceTests()
        {
            var mockRepo = new Mock<IMoviesRepository>();
            _mockMoviesRepo = mockRepo;
            _svc = new MoviesControllerService(_mockMoviesRepo.Object, 
                new Mock<ILogger<IMoviesControllerService>>().Object);
        }

        [Fact]
        public async Task SearchMoviesGeneratesGenresList()
        {
            _mockMoviesRepo.Setup(repo => repo.SearchMoviesAsync("abc",
                    2020,
                    new List<string>() {"abc", "xyz"}))
                .ReturnsAsync(new List<MovieEntity>());

            await _svc.SearchMoviesAsync(new SearchMovieFilters("abc", 2020, "abc,xyz"));

            _mockMoviesRepo.VerifyAll();

        }

        [Fact]
        public async Task HandlesWhitespacesWhenGenerateGenresList()
        {
            _mockMoviesRepo.Setup(repo => repo.SearchMoviesAsync("abc",
                    2020,
                    new List<string>() { "abc", "xyz" }))
                .ReturnsAsync(new List<MovieEntity>());

            await _svc.SearchMoviesAsync( new SearchMovieFilters("abc", 2020, "abc    ,      xyz      "));

            _mockMoviesRepo.VerifyAll();

        }

        [Fact]
        public async Task GetTopRatedMoviesCallsRepoMethod()
        {
            _mockMoviesRepo.Setup(repo => repo.GetTopRatedMovies())
                .ReturnsAsync(new List<MovieEntity>());

            await _svc.GetTopRatedMovies();
            _mockMoviesRepo.VerifyAll();

        }

        [Fact]
        public async Task GetTopRatedMoviesByUserCallsRepoMethod()
        {
            _mockMoviesRepo.Setup(repo => repo.GetTopMoviesRatedByUser(1))
                .ReturnsAsync(new List<MovieEntity>());

            await _svc.GetTopMoviesRatedByUser(1);
            _mockMoviesRepo.VerifyAll();

        }
    }
}
