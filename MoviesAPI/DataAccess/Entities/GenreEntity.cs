using System;
using System.Collections.Generic;

namespace MoviesAPI.DataAccess.Entities
{
    public class GenreEntity
    {
        public short GenreId { get; set; }
        public string GenreName { get; set; }

        public virtual ICollection<MovieGenreEntity> MovieGenres { get; set; }
    }
}
