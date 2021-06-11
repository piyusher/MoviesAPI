using System;
using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.FiltersAttributes
{
    public class MovieYearRangeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success; 
            }
            if (!int.TryParse(value.ToString(), out var val))
            {
                return new ValidationResult("Cannot parse year value as integer");
            }
            if (val == 0 || (val >= 1850 && val <= DateTime.Now.Year))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult($"The year value should be between 1850 and {DateTime.Now.Year}");
        }
    }
}
