using MoviesAPI.DataAccess.Entities;

namespace MoviesAPI.Domain.Converters
{
    public static class ModelToEntityConverter
    {
        public static MovieRatingEntity ToMovieRatingsEntity(this RatingModel model)
        {
            if (model == null) return null;
            
            return new MovieRatingEntity()
            {
                MovieId = model.MovieId,
                Rating = model.Rating,
                UserId = model.UserId
            };
        }
    }
}
