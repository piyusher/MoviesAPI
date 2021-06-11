using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using MoviesAPI.DataAccess.Entities;

namespace MoviesAPI.DataAccess.DataSeed
{
    [ExcludeFromCodeCoverage]
    public class SeedDataModel
    {
        //JSON data has genres as string.
        //Extending the class to add the string property and deserialize JSON
        public class MoviesExtended : MovieEntity
        {
            public List<string> Genres { get; set; }
        }

        public List<GenreEntity> Genres { get; set; }
        public List<MoviesExtended> Movies { get; set; }
        public List<UserEntity> Users { get; set; }
        public List<MovieRatingEntity> Ratings { get; set; }
        
    }
}
