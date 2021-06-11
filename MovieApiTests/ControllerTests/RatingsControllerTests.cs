using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;
using MoviesAPI.Controllers;
using MoviesAPI.Domain;
using MoviesAPI.Services.Interfaces;
using Xunit;

namespace MovieApiTests.ControllerTests
{
    public class RatingsControllerTests
    {
        private readonly RatingsController _ratingsController;
        private readonly Mock<IRatingsControllerService> _ratingsService;
        private readonly Mock<ProblemDetailsFactory> _mockProblemFactory;

        public RatingsControllerTests()
        {
            var mockProblemFactory = new Mock<ProblemDetailsFactory>();

            _mockProblemFactory = mockProblemFactory;

            _ratingsService = new Mock<IRatingsControllerService>();
            _ratingsController = new RatingsController(_ratingsService.Object)
            {
                ProblemDetailsFactory = mockProblemFactory.Object
            };
        }

        [Fact]
        public async Task PostValidatesMovieAndUserId()
        {
            var ratingModel = new RatingModel()
            {
                Rating = 3,
                MovieId = 2,
                UserId = 1
            };

            _ratingsService.Setup(r => r.ValidateMovieAndUserId(2, 1))
                .ReturnsAsync((true, ""));

            _ratingsService.Setup(r => r.AddUpdateMovieRatingAsync(ratingModel))
                .ReturnsAsync(ratingModel);

            var result = await _ratingsController.AddUpdateRating(ratingModel);

            result.Should().BeOfType<OkObjectResult>();

           _ratingsService.VerifyAll();
        }

        [Fact]
        public async Task PostReturns404WhenMovieOrUserNotFound()
        {
            var ratingModel = new RatingModel()
            {
                Rating = 3,
                MovieId = 2,
                UserId = 1
            };

            _ratingsService.Setup(r => r.ValidateMovieAndUserId(2, 1))
                .ReturnsAsync((false, "Not found"));

            _mockProblemFactory.SetupMockProblemDetails();


            var result = await _ratingsController.AddUpdateRating(ratingModel);

            result.Should().BeOfType<NotFoundObjectResult>();

            _mockProblemFactory.VerifyAll();
        }
    }
}
