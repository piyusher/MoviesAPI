using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.Domain.RequestModels
{
    public class SearchMovieRequest
    {
        public string Title;
        public int Year;
        public string Genres;
    }
}
