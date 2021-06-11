using System;
using System.Collections.Generic;


namespace MoviesAPI.DataAccess.Entities
{
    public class MovieEntity
    {
        public int MovieId { get; set; }
        public string Title { get; set; }
        public short Year { get; set; }
        public short Runtime { get; set; }
        public decimal AvgRating { get; set; }

        public virtual ICollection<MovieRatingEntity> Ratings { get; set; }
        public virtual ICollection<MovieGenreEntity> MovieGenres { get; set; }
    }
}
