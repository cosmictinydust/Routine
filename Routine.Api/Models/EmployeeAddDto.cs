using Routine.Api.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Routine.Api.Models
{
    public class EmployeeAddDto:IValidatableObject
    {
        [Display(Name = "员工代码")]
        [Required(ErrorMessage = "{0}是必填的")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "{0}的长度是{1}")]
        public string EmployeeNo { get; set; }

        [Display(Name = "姓")]
        [Required(ErrorMessage = "{0}是必填的")]
        [MaxLength(50,ErrorMessage ="{0}的长度不能超过{1}")]
        public string FirstName { get; set; }

        [Display(Name ="名"),Required(ErrorMessage = "{0}是必填的"),MaxLength(50,ErrorMessage ="{0}的长度不能超过50")]
        public string LastName { get; set; }

        [Display(Name ="性别")]
        public Gender Gender { get; set; }

        [Display(Name ="出生日期")]
        public DateTime DateOfBirth { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FirstName == LastName)
            {
                //yield此关键字可以返回IEnumerable类型返回值
                yield return new ValidationResult("姓和名不能一样",
                    new[] { nameof(FirstName), nameof(LastName) });
            }
        }
    }
}
