using System.Collections.Generic;
using FluentAssertions;
using MoviesAPI.DataAccess.Entities;
using MoviesAPI.Domain.Converters;
using Xunit;

namespace MovieApiTests.Domain.ConverterTests
{
    public class EntityToDomainModelTests
    {
        [Fact]
        public void ToMovieModelReturnNullIfNull()
        {
            MovieEntity entity = null;
            var model = entity.ToMovieDomainModelWithRating();
            model.Should().BeNull();
        }

        [Fact]
        public void ToMovieModelAllFieldsAreMapped()
        {
            var entity = new MovieEntity()
            {
                Title = "abc",
                AvgRating = decimal.Parse("1.1"),
                MovieId = 1,
                Runtime = 120,
                Year = 1999
            };
            var model = entity.ToMovieDomainModelWithRating();
            model.Title.Should().Be("abc");
            model.AverageRating.Should().Be("1.0");
            model.Id.Should().Be(1);
            model.RunningTime.Should().Be(120);
            model.YearOfRelease.Should().Be(1999);
        }

        [Fact]
        public void GenresConvertsToCommaSeparatedString()
        {
            var entity = new MovieEntity()
            {
                MovieGenres = new List<MovieGenreEntity>()
                {
                    new MovieGenreEntity()
                    {
                         Genre = new GenreEntity()
                         {
                             GenreName = "ABC"
                         }
                    },
                    new MovieGenreEntity()
                    {
                        Genre = new GenreEntity()
                        {
                            GenreName = "XYZ"
                        }
                    }
                }
            };
            var model = entity.ToMovieDomainModelWithRating();
            model.Genres.Should().Be("ABC,XYZ");
        }

        [Fact]
        public void TestAverageRatingRoundingLogic()
        {
            var entity = new MovieEntity()
            {
                AvgRating = decimal.Parse("2.91")
            };

            //2.91 should be rounded to 3.0
            var model = entity.ToMovieDomainModelWithRating();
            model.AverageRating.Should().Be("3.0");

            //3.249 should be rounded to 3.0
            entity.AvgRating = decimal.Parse("3.249");
            model = entity.ToMovieDomainModelWithRating();
            model.AverageRating.Should().Be("3.0");

            //3.25 should be rounded to 3.5
            entity.AvgRating = decimal.Parse("3.25");
            model = entity.ToMovieDomainModelWithRating();
            model.AverageRating.Should().Be("3.5");

            //3.6 should be rounded to 3.5
            entity.AvgRating = decimal.Parse("3.6");
            model = entity.ToMovieDomainModelWithRating();
            model.AverageRating.Should().Be("3.5");

            //3.75 should be rounded to 4.0
            entity.AvgRating = decimal.Parse("3.75");
            model = entity.ToMovieDomainModelWithRating();
            model.AverageRating.Should().Be("4.0");
        }

        [Fact]
        public void ToRatingModelReturnNullIfNull()
        {
            MovieRatingEntity entity = null;
            var model = entity.ToRatingDomainModel();
            model.Should().BeNull();
        }

        [Fact]
        public void ToRatingModelAllFieldsAreMapped()
        {
            var entity = new MovieRatingEntity()
            {
                MovieId = 1,
                UserId = 1,
                Rating = 1
            };
            var model = entity.ToRatingDomainModel();
            model.MovieId.Should().Be(1);
            model.UserId.Should().Be(1);
            model.Rating.Should().Be(1);
        }

    }
}
