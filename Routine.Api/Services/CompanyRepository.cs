using Microsoft.EntityFrameworkCore;
using Routine.Api.Data;
using Routine.Api.DtoParameters;
using Routine.Api.Entities;
using Routine.Api.Helpers;
using Routine.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.Services
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly RoutineDbContext _context;

        public IPropertyMappingService _propertyMappingService { get; }

        public CompanyRepository(RoutineDbContext context,IPropertyMappingService propertyMappingService) 
        {
            //如果上下文为空则抛出错误
            _context = context??throw new ArgumentNullException(nameof(context));
            _propertyMappingService = propertyMappingService ?? throw new ArgumentNullException(nameof(propertyMappingService));
        }
        
        public void AddCompany(Company company)
        {
            if (company == null)
            {
                throw new ArgumentNullException(nameof(company));
            }
            company.ID = Guid.NewGuid();

            if (company.Employees != null)
            {
                foreach (var employee in company.Employees)
                {
                    employee.ID = Guid.NewGuid();
                }
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

        public async Task<PageList<Company>> GetCompaniesAsync(CompanyDtoParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            //if (string.IsNullOrWhiteSpace(parameters.CompanyName) && string.IsNullOrWhiteSpace(parameters.SearchTerm))
            //{
            //    return await _context.Companies.ToListAsync();
            //}

            var queryExpression = _context.Companies as IQueryable<Company>;
            
            if (!string.IsNullOrWhiteSpace(parameters.CompanyName))
            {
                parameters.CompanyName = parameters.CompanyName.Trim();
                queryExpression = queryExpression.Where(x => x.Name == parameters.CompanyName);
            }

            if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            {
                parameters.SearchTerm = parameters.SearchTerm.Trim();
                queryExpression = queryExpression.Where(x => x.Name.Contains(parameters.SearchTerm)
                  || x.Introduction.Contains(parameters.SearchTerm)
                );
            }

            //使用了PageList<T>，就不用下面的这一行代码了
            //queryExpression = queryExpression.Skip(parameters.PageSize * (parameters.PageNumber - 1))
            //    .Take(parameters.PageSize);

            var mappingDictionary = _propertyMappingService.GetPropertyMapping<CompanyDto, Company>();
            queryExpression = queryExpression.ApplySort(parameters.OrderBy, mappingDictionary);

            //至此才开始实际从数据库中读取资源
            return await PageList<Company>.CreateAsync(queryExpression,parameters.PageNumber,parameters.PageSize);
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

        public async Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId,EmployeeDtoParameters parameters)
        {
            if (companyId == null)
            {
                throw new ArgumentNullException(nameof(companyId));
            }

            var items = _context.Employees.Where(x => x.CompanyId == companyId);        //先预设一个所有数据的集合IQueryable（此时并不会立即查询数据库，而是相当于拼SQL语句的过程）

            if (!string.IsNullOrWhiteSpace(parameters.Gender))                          //如果筛选条件 genderDisplay不为空，返回过滤后的List
            {
                parameters.Gender = parameters.Gender.Trim();
                var gender = Enum.Parse<Gender>(parameters.Gender);     //转换为枚举类型

                items = items.Where(x => x.Gender == gender);
            }

            if (!string.IsNullOrWhiteSpace(parameters.Q))                                      //如果搜索条件 q 不为空,返回全文搜索后的List
            {
                parameters.Q = parameters.Q.Trim();
                items = items.Where(x => x.EmployeeNo.Contains(parameters.Q)
                        || x.FirstName.Contains(parameters.Q)
                        || x.LastName.Contains(parameters.Q)
                );
            }

            if (!string.IsNullOrWhiteSpace(parameters.OrderBy))
            {
                if (parameters.OrderBy.ToLowerInvariant() == "name")
                {
                    items = items.OrderBy(x => x.FirstName).ThenBy(x => x.LastName);
                }
            }

            var mappingDictionary = _propertyMappingService.GetPropertyMapping<EmployeeDto, Employee>();
            items = items.ApplySort(parameters.OrderBy, mappingDictionary);

            return await items.ToListAsync();
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
