using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Validators
{
    public class AllowedYearRangeAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // get the user entered value
            var userEnetredYear = ((DateTime)value).Year;
            var calculateUserAge = (DateTime.Now.Year - ((DateTime)value).Year);

            if (userEnetredYear < 1900)
            {
                return new ValidationResult("Year should be no less than 1900");
            }
            else if (calculateUserAge < 15) {
                return new ValidationResult("User must be at least 15 years old");
            }
            return ValidationResult.Success;
        }
    }
}
