using FluentAssertions;
using MoviesAPI.DataAccess.Entities;
using MoviesAPI.Domain;
using MoviesAPI.Domain.Converters;
using Xunit;

namespace MovieApiTests.Domain.ConverterTests
{
    public class ModelToEntityTests
    {
        [Fact]
        public void ToRatingModelReturnNullIfNull()
        {
            RatingModel model = null;
            var entity = model.ToMovieRatingsEntity();
            entity.Should().BeNull();
        }

        [Fact]
        public void ToMovieRatingEntityAllFieldsAreMapped()
        {
            var model = new RatingModel()
            {
                MovieId = 1,
                UserId = 1,
                Rating = 1
            };
            var entity = model.ToMovieRatingsEntity();
            entity.MovieId.Should().Be(1);
            entity.UserId.Should().Be(1);
            entity.Rating.Should().Be(1);
        }
    }
}
