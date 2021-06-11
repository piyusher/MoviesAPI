using System;

namespace MoviesAPI.DataAccess.Entities
{
    public class MovieRatingEntity
    {
        public int MovieId { get; set; }
        public long UserId { get; set; }
        public byte Rating { get; set; }

        public virtual MovieEntity Movie { get; set; }
        public virtual UserEntity User { get; set; }
    }
}
