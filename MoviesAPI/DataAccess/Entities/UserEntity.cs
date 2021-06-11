using System;
using System.Collections.Generic;

namespace MoviesAPI.DataAccess.Entities
{
    public class UserEntity
    {
        public long UserId { get; set; }
        public string UserName { get; set; }

        public virtual ICollection<MovieRatingEntity> MovieRatings { get; set; }
    }
}
