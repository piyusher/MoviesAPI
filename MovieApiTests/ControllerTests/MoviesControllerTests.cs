using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using MoviesAPI.Controllers;
using MoviesAPI.Domain;
using MoviesAPI.Domain.RequestModels;
using MoviesAPI.Services.Interfaces;
using Xunit;

namespace MovieApiTests.ControllerTests
{
    public class MoviesControllerTests
    {
        private readonly MoviesController _moviesController;
        private readonly Mock<IMoviesControllerService> _movieService;
        private readonly Mock<ProblemDetailsFactory> _mockProblemFactory;

        public MoviesControllerTests()
        {
          
            var mockProblemFactory = new Mock<ProblemDetailsFactory>();
            
            _mockProblemFactory = mockProblemFactory;

            _movieService = new Mock<IMoviesControllerService>();
            _moviesController = new MoviesController(_movieService.Object)
            {
                ProblemDetailsFactory = mockProblemFactory.Object
            };
        }

        [Fact]
        public async Task SearchReturns404WhenNotFound()
        {
            var filters = new SearchMovieFilters("abc", 2020, "abc,xyz");
            //This should be called
            _movieService.Setup(s => s.SearchMoviesAsync(filters))
                .ReturnsAsync(new List<MovieModelWithAvgRating>());

            //This should be called too
            _mockProblemFactory.SetupMockProblemDetails();

            var result = await _moviesController.SearchMoviesAsync(filters);

           //should return Not found with problem details
           result.Should().BeOfType<NotFoundObjectResult>();

           _movieService.VerifyAll();
           _mockProblemFactory.VerifyAll();

        }

        [Fact]
        public async Task SearchReturns400WhenNoCriteriaGiven()
        {
            var modelState = new Mock<ModelStateDictionary>();

            //Bad Request problem details should be returned
            _mockProblemFactory.SetupMockValidationProblemDetails();

            var result = await _moviesController.SearchMoviesAsync(new SearchMovieFilters("", 0, null));


            //should return BadRequest with problem details
            result.Should().BeOfType<BadRequestObjectResult>();

            _mockProblemFactory.VerifyAll();

        }

        [Fact]
        public async Task SearchReturns200WhenDataFound()
        {
            var filters = new SearchMovieFilters("abc", 2020, "abc,xyz");
            //This should be called
            _movieService.Setup(s => s.SearchMoviesAsync(filters))
                .ReturnsAsync(new List<MovieModelWithAvgRating>()
                {
                    new MovieModelWithAvgRating()
                });


            var result = await _moviesController.SearchMoviesAsync(filters);


            //should return BadRequest with problem details
            result.Should().BeOfType<OkObjectResult>();

        }


        [Fact]
        public async Task GetTopRatedReturns404WhenNoMovies()
        {
            //This should be called
            _movieService.Setup(s => s.GetTopRatedMovies())
                .ReturnsAsync(new List<MovieModelWithAvgRating>());

            //This should be called too
            _mockProblemFactory.SetupMockProblemDetails();

            var result = await _moviesController.GetTopRatedMovies();

            //should return BadRequest with problem details
            result.Should().BeOfType<NotFoundObjectResult>();

            _movieService.VerifyAll();
            _mockProblemFactory.VerifyAll();

        }

        [Fact]
        public async Task GetTopRatedReturns200WhenFound()
        {
            //This should be called
            _movieService.Setup(s => s.GetTopRatedMovies())
                .ReturnsAsync(new List<MovieModelWithAvgRating>()
                {
                    new MovieModelWithAvgRating()
                });

            var result = await _moviesController.GetTopRatedMovies();

            //should return BadRequest with problem details
            result.Should().BeOfType<OkObjectResult>();

            _movieService.VerifyAll();

        }

        [Fact]
        public async Task GetTopRatedByUserReturns404WhenNoMovies()
        {
            //This should be called
            _movieService.Setup(s => s.GetTopMoviesRatedByUser(1))
                .ReturnsAsync(new List<MovieModelWithAvgRating>());

            //this should be called too
            _mockProblemFactory.SetupMockProblemDetails();

            var result = await _moviesController.GetTopMoviesRatedByUser(1);

            //should return BadRequest with problem details
            result.Should().BeOfType<NotFoundObjectResult>();

            _movieService.VerifyAll();
            _mockProblemFactory.VerifyAll();

        }

        [Fact]
        public async Task GetTopRatedByUserReturns200WhenFound()
        {
            //This should be called
            _movieService.Setup(s => s.GetTopMoviesRatedByUser(1))
                .ReturnsAsync(new List<MovieModelWithAvgRating>()
                {
                    new MovieModelWithAvgRating()
                });

            var result = await _moviesController.GetTopMoviesRatedByUser(1);

            //should return BadRequest with problem details
            result.Should().BeOfType<OkObjectResult>();

            _movieService.VerifyAll();

        }

        
    }
}
