using System;

namespace MoviesAPI.DataAccess.Entities
{
    public class MovieGenreEntity
    {
        public int MovieId { get; set; }
        public short GenreId { get; set; }

        public virtual MovieEntity Movie { get; set; }
        public virtual GenreEntity Genre { get; set; }
    }
}
