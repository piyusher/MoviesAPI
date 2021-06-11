using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using MoviesAPI.DataAccess.Entities;
using MoviesAPI.Domain;
using MoviesAPI.Repositories.Interfaces;
using MoviesAPI.Services;
using MoviesAPI.Services.Interfaces;
using Xunit;

namespace MovieApiTests.Services
{
    public class RatingsControllerServiceTests
    {
        private readonly IRatingsControllerService _svc;
        private readonly Mock<IRatingsRepository> _mockRatingsRepo;
        private readonly Mock<IMoviesRepository> _mockMoviesRepo;
        private readonly Mock<IUsersRepository> _mockUserRepo;

        public RatingsControllerServiceTests()
        {
            _mockUserRepo = new Mock<IUsersRepository>();
            _mockMoviesRepo = new Mock<IMoviesRepository>();
            _mockRatingsRepo = new Mock<IRatingsRepository>();


            _svc = new RatingsControllerService(_mockRatingsRepo.Object,
                _mockMoviesRepo.Object,
                _mockUserRepo.Object);
        }

        [Fact]
        public async Task CallsInsertWhenNoRatingExists()
        {
            //This should be called
            _mockRatingsRepo.Setup(r => r.GetUserRatingForMovieAsync(1, 2))
                .ReturnsAsync(default(MovieRatingEntity));
            
            //This should be called too
            _mockRatingsRepo.Setup(r => r.AddNewRatingAsync(
                It.Is<MovieRatingEntity>(ent=>ent.MovieId == 2 && ent.UserId == 1 && ent.Rating == 3)))
                .ReturnsAsync(new MovieRatingEntity());

            //call service method
            await _svc.AddUpdateMovieRatingAsync(new RatingModel()
            {
                MovieId = 2,
                UserId = 1,
                Rating = 3
            });

            _mockRatingsRepo.VerifyAll();
        }

        [Fact]
        public async Task CallsUpdateWhenRatingExists()
        {
            //This should be called
            _mockRatingsRepo.Setup(r => r.GetUserRatingForMovieAsync(1, 2))
                .ReturnsAsync(new MovieRatingEntity());

            //This should be called too
            _mockRatingsRepo.Setup(r => r.UpdateRatingAsync(
                    It.Is<MovieRatingEntity>(ent => ent.MovieId == 2 && ent.UserId == 1 && ent.Rating == 3)))
                .ReturnsAsync(new MovieRatingEntity());

            //call service method
            await _svc.AddUpdateMovieRatingAsync(new RatingModel()
            {
                MovieId = 2,
                UserId = 1,
                Rating = 3
            });

            _mockRatingsRepo.VerifyAll();
        }

        [Fact]
        public async Task RecalculatesAverageRatingOnInsert()
        {
            _mockMoviesRepo.Setup(m => m.RecalculateAvgRatingAsync(2));
            await CallsInsertWhenNoRatingExists();
            _mockMoviesRepo.VerifyAll();
        }

        [Fact]
        public async Task RecalculatesAverageRatingOnUpdate()
        {
            _mockMoviesRepo.Setup(m => m.RecalculateAvgRatingAsync(2));
            await CallsUpdateWhenRatingExists();
            _mockMoviesRepo.VerifyAll();
        }

        [Fact]
        public async Task ValidationFailsWhenUserIdNotFound()
        {
            _mockMoviesRepo.Setup(m => m.GetMovieByIdAsync(2))
                .ReturnsAsync(new MovieEntity());

            _mockUserRepo.Setup(m => m.GetUserByIdAsync(1))
                .ReturnsAsync(default(UserEntity));

            var (isValid, message) = await _svc.ValidateMovieAndUserId(2, 1);

            _mockMoviesRepo.VerifyAll();
            _mockUserRepo.VerifyAll();

            isValid.Should().BeFalse();
            message.Should().Be(string.Format(MessageStrings.UserDoesNotExist, 1));
        }

        [Fact]
        public async Task ValidationFailsWhenMovieIdNotFound()
        {
            _mockMoviesRepo.Setup(m => m.GetMovieByIdAsync(2))
                .ReturnsAsync(default(MovieEntity));

            _mockUserRepo.Setup(m => m.GetUserByIdAsync(1))
                .ReturnsAsync(new UserEntity());

            var (isValid, message) = await _svc.ValidateMovieAndUserId(2, 1);

            _mockMoviesRepo.VerifyAll();
            _mockUserRepo.VerifyAll();

            isValid.Should().BeFalse();
            message.Should().Be(string.Format(MessageStrings.MovieDoesNotExist, 2));
        }

        [Fact]
        public async Task ValidationFailsWhenBothNotFound()
        {
            _mockMoviesRepo.Setup(m => m.GetMovieByIdAsync(2))
                .ReturnsAsync(default(MovieEntity));

            _mockUserRepo.Setup(m => m.GetUserByIdAsync(1))
                .ReturnsAsync(default(UserEntity));

            var (isValid, message) = await _svc.ValidateMovieAndUserId(2, 1);

            _mockMoviesRepo.VerifyAll();
            _mockUserRepo.VerifyAll();

            isValid.Should().BeFalse();

            var messageShouldBe = $"{string.Format(MessageStrings.MovieDoesNotExist, 2)}{string.Format(MessageStrings.UserDoesNotExist, 1)}";

            message.Should().Be(messageShouldBe);
        }

        [Fact]
        public async Task ValidationPassesOnlyWhenBothFound()
        {
            _mockMoviesRepo.Setup(m => m.GetMovieByIdAsync(2))
                .ReturnsAsync(new MovieEntity());

            _mockUserRepo.Setup(m => m.GetUserByIdAsync(1))
                .ReturnsAsync(new UserEntity());

            var (isValid, message) = await _svc.ValidateMovieAndUserId(2, 1);

            _mockMoviesRepo.VerifyAll();
            _mockUserRepo.VerifyAll();

            isValid.Should().BeTrue();
            message.Should().BeEmpty();
        }
    }
}
