using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MoviesAPI.FiltersAttributes;

namespace MoviesAPI.Domain
{
    public class RatingModel
    {
        [Required]
        [Range(1, long.MaxValue)]
        public int MovieId { get; set; }

        [Required]
        [Range(1,long.MaxValue)]
        public long UserId { get; set; }

        [Required]
        [Range(minimum:1,maximum:5,ErrorMessage = "Rating should a be numeric value between 1 and 5")]
        public byte Rating { get; set; }
    }
}
