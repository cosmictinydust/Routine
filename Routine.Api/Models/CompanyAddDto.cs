using System.Collections.Generic;

namespace Routine.Api.Models
{
    public class CompanyAddDto
    {
        public string Name { get; set; }
        public string Introduction { get; set; }

        public ICollection<EmployeeAddDto> Employees { get; set; } = new List<EmployeeAddDto>();  //这里的 new List...的写法是防止使用时出现空引用类型的 据说是C#6.0后的新功能
        //员工集合的名称最好与Company中对应的员工集合的名字一致（这例子中就是Employees）因为CompanyAddDto与Company及EmployeeAddDto与Employee都是已经映射好了的，如果这里的员工集合的名称也致的话，就不需要的Automap的Profile中映射时写一一对应的关系，直接默认即可。
    }
}
