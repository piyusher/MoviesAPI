using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Domain
{
    public class RatingModel
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int MovieId { get; set; }

        [Required]
        [Range(1,long.MaxValue)]
        public long UserId { get; set; }

        /// <summary>
        /// Rating should be numeric value between 1 and 5
        /// </summary>
        [Required]
        [Range(minimum:1,maximum:5,ErrorMessage = "Rating should a be numeric value between 1 and 5")]
        public byte Rating { get; set; }
    }
}
