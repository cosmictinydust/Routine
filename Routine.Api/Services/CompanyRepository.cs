using Microsoft.EntityFrameworkCore;
using Routine.Api.Data;
using Routine.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.Services
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly RoutineDbContext _context;

        public CompanyRepository(RoutineDbContext context) 
        {
            //如果上下文为空则抛出错误
            _context = context??throw new ArgumentNullException(nameof(context));
        }
        
        public void AddCompany(Company company)
        {
            if (company == null)
            {
                throw new ArgumentNullException(nameof(company));
            }
            company.ID = Guid.NewGuid();
            foreach(var employee in company.Employees)
            {
                employee.ID = Guid.NewGuid();
            }
            _context.Companies.Add(company);
        }

        public void AddEmployee(Guid companyId, Employee employee)
        {
            if (companyId == null)
            {
                throw new ArgumentNullException(nameof(companyId));
            }
            if (employee==null)
            {
                throw new ArgumentNullException(nameof(employee));
            }
            employee.CompanyId = companyId;
            _context.Employees.Add(employee);
        }

        public async Task<bool> CompanyExistsAsync(Guid companyId)
        {
            if (companyId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(companyId));
            }
            return await _context.Companies.AnyAsync(x => x.ID == companyId);
        }

        public void DeleteCompany(Company company)
        {
            if (company==null)
            {
                throw new ArgumentNullException(nameof(company));
            }
            _context.Companies.Remove(company);
        }

        public void DeleteEmployee(Employee employee)
        {
            _context.Employees.Remove(employee);
        }

        public async Task<IEnumerable<Company>> GetCompaniesAsync()
        {
            return await _context.Companies.ToListAsync();
        }

        public async Task<IEnumerable<Company>> GetCompaniesAsync(IEnumerable<Guid> companyIds)
        {
            if (companyIds == null)
            {
                throw new ArgumentNullException(nameof(companyIds));
            }
            return await _context.Companies
                .Where(x => companyIds.Contains(x.ID))
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<Company> GetCompanyAsync(Guid companyId)
        {
            if (companyId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(companyId));
            }
            return await _context.Companies.FirstOrDefaultAsync(x => x.ID == companyId);
        }

        public async Task<Employee> GetEmployee(Guid companyId, Guid employeeId)
        {
            if (companyId==null)
            {
                throw new ArgumentNullException(nameof(companyId));
            }
            if (employeeId==null)
            {
                throw new ArgumentNullException(nameof(employeeId));
            }
            return await _context.Employees
                .Where(x => x.CompanyId == companyId && x.ID == employeeId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId)
        {
            if (companyId == null)
            {
                throw new ArgumentNullException(nameof(companyId));
            }
            return await _context.Employees
                .Where(x => x.CompanyId == companyId)
                .OrderBy(x => x.EmployeeNo)
                .ToListAsync();
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }

        public void UpdateCompany(Company company)
        {
            //_context.Entry(company).State = EntityState.Modified;
            //因为ef对实体的改变是自动跟踪的，所以应该没有上面这个语句，改变实体的实例后也会更新至数据库
        }

        public void UpdateEmployee(Employee employee)
        {
            //因为ef对实体的改变是自动跟踪的，所以同上
        }
    }
}
