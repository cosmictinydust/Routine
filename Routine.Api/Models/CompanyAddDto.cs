using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Routine.Api.Models
{
    public class CompanyAddDto
    {
        [Display(Name="公司名")]  //指定此属性在显示时按什么名字显示
        [Required(ErrorMessage ="{0}这个字段是必填的")]
        [MaxLength(100,ErrorMessage ="{0}的最大长度不可以超过{1}")]  //这里的{0}代表[Display(Name="公司名")]这个东西，{1}代表MaxLength中的第一个参数
        public string Name { get; set; }
        
        [Display(Name="简介")]
        [StringLength(500,MinimumLength =10,ErrorMessage ="{0}的长度范围从{2}到{1}")]  //这里的{2}是指MinimumLength的值，{1}是指500这个值
        public string Introduction { get; set; }

        public ICollection<EmployeeAddDto> Employees { get; set; } = new List<EmployeeAddDto>();  //这里的 new List...的写法是防止使用时出现空引用类型的 据说是C#6.0后的新功能
        //员工集合的名称最好与Company中对应的员工集合的名字一致（这例子中就是Employees）因为CompanyAddDto与Company及EmployeeAddDto与Employee都是已经映射好了的，如果这里的员工集合的名称也致的话，就不需要的Automap的Profile中映射时写一一对应的关系，直接默认即可。
    }
}
