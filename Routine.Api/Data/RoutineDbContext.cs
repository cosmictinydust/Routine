using Microsoft.EntityFrameworkCore;
using Routine.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.Data
{
    public class RoutineDbContext:DbContext
    {
        public RoutineDbContext(DbContextOptions<RoutineDbContext> options) : base(options) { }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>().Property(x => x.Name).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Company>().Property(x => x.Introduction).HasMaxLength(500);
            modelBuilder.Entity<Employee>().Property(x => x.EmployeeNo).IsRequired().HasMaxLength(10);
            modelBuilder.Entity<Employee>().Property(x => x.FirstName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Employee>().Property(x => x.LastName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Employee>().HasOne(x => x.Company)  //一个人对应一家公司
                .WithMany(x => x.Employees)     //一家公司对应多个Employee
                .HasForeignKey(x => x.CompanyId)    //指定关联的字段
                .OnDelete(DeleteBehavior.Restrict);  //增加约束，如果Company下面有员工的话，就不能删除操作

            //种子数据 
            modelBuilder.Entity<Company>().HasData(
                new Company { 
                    ID=Guid.Parse("1b0bcd14-4c4e-4de6-b88b-af684d657871"),
                    Name="Microsoft",
                    Introduction="Great Company"
                },
                new Company
                {
                    ID = Guid.Parse("03d39713-c42a-41e8-8529-8ab22c843d09"),
                    Name = "Google",
                    Introduction = "Don't be evil"
                },
                new Company
                {
                    ID = Guid.Parse("d36fcf89-99a3-44ef-ad2a-9af87dba3134"),
                    Name = "Alipapa",
                    Introduction = "Fubao Company"
                }

                );
        }
    }
}
