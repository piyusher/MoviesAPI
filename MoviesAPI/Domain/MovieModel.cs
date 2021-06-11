using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.Domain
{
    //Used for put/post
    public class MovieModel
    {
        public int Id { get; set; }
        public string Genres { get; set; }
        public short RunningTime { get; set; }
        public string Title { get; set; }
        public short YearOfRelease { get; set; }
        
    }

    //used to GETs
    public class MovieModelWithAvgRating : MovieModel
    {  
        public string AverageRating { get; set; }
    }
}
