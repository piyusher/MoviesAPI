using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MoviesAPI.FiltersAttributes;

namespace MoviesAPI.Domain.RequestModels
{
    public class SearchMovieFilters
    {
        public SearchMovieFilters()
        {
            
        }
        public SearchMovieFilters(string title, short year, string genres)
        {
            Title = title;
            Year = year;
            Genres = genres;
        }
        /// <summary>Full or partial title of the movie.A title is matched if it contains the input text.</summary>
        [BindProperty]
        [MaxLength(500)]
        public string Title { get; set; }

        /// <summary>The year of the release of the movie. Minimum value 1850, Maximum current year</summary>
        [BindProperty]
        [MovieYearRange]
        public short Year { get; set; }

        /// <summary>A comma separated list of genres. Refer to the '/genres' endpoint for the list</summary>
        [BindProperty]
        public string Genres { get; set; }
    }
}
