using System;
using System.Collections.Generic;

namespace Routine.Api.Entities
{
    public class Company
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Introduction { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}
