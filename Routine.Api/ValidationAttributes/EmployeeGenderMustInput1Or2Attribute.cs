using Routine.Api.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.ValidationAttributes
{
    public class EmployeeGenderMustInput1Or2Attribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var addDto = (EmployeeAddOrUpdateDto)validationContext.ObjectInstance;
            if (addDto.Gender.ToString()!= "男" && addDto.Gender.ToString()!="女")
            {
                return new ValidationResult(ErrorMessage,
                    new[] { nameof(EmployeeAddOrUpdateDto) });
            }

            return ValidationResult.Success;
        }
    }
}
