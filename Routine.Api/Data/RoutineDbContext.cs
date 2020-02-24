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
                .OnDelete(DeleteBehavior.Cascade);  //增加约束，如果Company下面有员工的话，Restrict 设置为就不能删除操作 ; Cascade 设置为删除

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

            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    ID=Guid.Parse("89b477db-3546-4cb0-9c6f-3a5d5fb16443"),
                    CompanyId=Guid.Parse("1b0bcd14-4c4e-4de6-b88b-af684d657871"),
                    DateOfBirth=new DateTime(1975,1,15),
                    EmployeeNo="MSFT222",
                    FirstName="Log",
                    LastName="Hill",
                    Gender=Gender.男
                },
                new Employee
                {
                    ID = Guid.Parse("259e7699-3b1c-41b2-94d6-35f696a06b91"),
                    CompanyId = Guid.Parse("1b0bcd14-4c4e-4de6-b88b-af684d657871"),
                    DateOfBirth = new DateTime(1975, 4, 2),
                    EmployeeNo = "MSFT223",
                    FirstName = "Li",
                    LastName = "Emily",
                    Gender = Gender.女
                },
                new Employee
                {
                    ID = Guid.Parse("b28ce9be-8e2d-4bdd-99c9-22f69be1ef66"),
                    CompanyId = Guid.Parse("1b0bcd14-4c4e-4de6-b88b-af684d657871"),
                    DateOfBirth = new DateTime(2002, 1, 15),
                    EmployeeNo = "MSFT224",
                    FirstName = "Log",
                    LastName = "Simon",
                    Gender = Gender.男
                },
                new Employee
                {
                    ID = Guid.Parse("7a6578a8-f97d-4d58-8568-b4456c529e8f"),
                    CompanyId = Guid.Parse("03d39713-c42a-41e8-8529-8ab22c843d09"),
                    DateOfBirth = new DateTime(399, 1, 15),
                    EmployeeNo = "Go224",
                    FirstName = "李",
                    LastName = "白",
                    Gender = Gender.男
                },
                new Employee
                {
                    ID = Guid.Parse("97123943-68eb-4613-ab9c-7a8469beeab4"),
                    CompanyId = Guid.Parse("03d39713-c42a-41e8-8529-8ab22c843d09"),
                    DateOfBirth = new DateTime(409, 1, 15),
                    EmployeeNo = "Go225",
                    FirstName = "杜",
                    LastName = "甫",
                    Gender = Gender.男
                },
                new Employee
                {
                    ID = Guid.Parse("722b8517-87ae-4f88-b435-3acfa22183c1"),
                    CompanyId = Guid.Parse("d36fcf89-99a3-44ef-ad2a-9af87dba3134"),
                    DateOfBirth = new DateTime(183, 1, 15),
                    EmployeeNo = "Albb225",
                    FirstName = "曹",
                    LastName = "操",
                    Gender = Gender.男
                },
                new Employee
                {
                    ID = Guid.Parse("901f2404-15c4-494e-8167-66df3aaee144"),
                    CompanyId = Guid.Parse("d36fcf89-99a3-44ef-ad2a-9af87dba3134"),
                    DateOfBirth = new DateTime(193, 1, 15),
                    EmployeeNo = "Albb226",
                    FirstName = "孙",
                    LastName = "尚香",
                    Gender = Gender.女
                }
                );
        }
    }
}
