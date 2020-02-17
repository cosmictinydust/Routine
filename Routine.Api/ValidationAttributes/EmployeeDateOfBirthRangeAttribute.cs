using Routine.Api.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Routine.Api.ValidationAttributes
{
    public class EmployeeDateOfBirthRangeAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var addDto = (EmployeeAddOrUpdateDto)validationContext.ObjectInstance;
            var age = DateTime.Now.Year - addDto.DateOfBirth.Year;
            if (age<18 || age>=60)
            {
                return new ValidationResult(ErrorMessage,
                    new[] { nameof(EmployeeAddOrUpdateDto) });
            }

            return ValidationResult.Success;
        }
    }
}
