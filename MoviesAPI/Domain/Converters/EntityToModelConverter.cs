using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MoviesAPI.DataAccess.Entities;

namespace MoviesAPI.Domain.Converters
{
    public static class EntityToModelConverter
    {
        public static MovieModelWithAvgRating ToMovieDomainModelWithRating(this MovieEntity entity)
        {
            if (entity == null) return null;

            var genres = entity.MovieGenres?.Select(x => x.Genre.GenreName).ToList();

            return new MovieModelWithAvgRating()
            {
                Id = entity.MovieId,
                RunningTime = entity.Runtime,
                Title = entity.Title,
                YearOfRelease = entity.Year,
                Genres =  string.Join(',', genres ?? new List<string>()),
                AverageRating = (Math.Round(entity.AvgRating * 2, MidpointRounding.AwayFromZero) / 2).ToString("F1")
        };
        }

        public static RatingModel ToRatingDomainModel(this MovieRatingEntity entity)
        {
            if (entity == null) return null;

            return new RatingModel()
            {
                Rating = entity.Rating,
                MovieId = entity.MovieId,
                UserId = entity.UserId
            };
        }

    }
}
