using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Routine.Api.Entities;
using Routine.Api.Models;
using Routine.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.Controllers
{
    [ApiController]
    [Route("api/companies/{companyId}/employees")]
    public class EmployeesController:ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICompanyRepository _companyRepository;

        public EmployeesController(IMapper mapper,ICompanyRepository companyRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployeesFromCompany
            (
                Guid companyId,
                [FromQuery(Name = "genderDisplay")]string genderDisplay,
                string q
            )
        {
            if (! await _companyRepository.CompanyExistsAsync(companyId))
            {
                return NotFound();
            }

            var employees = await _companyRepository.GetEmployeesAsync(companyId,genderDisplay,q);
            if (employees == null)
            {
                return NotFound();
            }

            var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

            return Ok(employeeDtos);
        }

        [HttpGet("{employeeId}",Name =nameof(GetEmployeeFromCompany))]
        public async Task<ActionResult<EmployeeDto>> GetEmployeeFromCompany(Guid companyId,Guid employeeId)
        {
            if (!await _companyRepository.CompanyExistsAsync(companyId))
            {
                return NotFound();
            }

            var employee = await _companyRepository.GetEmployee(companyId, employeeId);
            if (employee == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<EmployeeDto>(employee));
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeDto>> 
            CreateEmployeeForCompany(Guid companyId, EmployeeAddDto employee)
        {
            if (!await _companyRepository.CompanyExistsAsync(companyId))
            {
                return NotFound();
            }
            var entity = _mapper.Map<Employee>(employee);
            _companyRepository.AddEmployee(companyId, entity);
            await _companyRepository.SaveAsync();

            var dtoToReturn = _mapper.Map<EmployeeDto>(entity);

            //使用CreatedAtRoute成功后返回一下201状态码，并在响应头(Response的headers)中带有已添加employee的查看地址
            return CreatedAtRoute(
                nameof(GetEmployeeFromCompany),     //此参数为 ：GetEmployeeFromCompnay方法的[HttpGet]属性条目中已标识了Route的名称
                new
                {
                    companyId = dtoToReturn.CompanyId,
                    employeeId = dtoToReturn.ID
                },
                dtoToReturn);
        }

        [HttpPut("{employeeId}")]
        public async Task<ActionResult<EmployeeDto>> UpdateEmployeeForCompany(Guid companyId, Guid employeeId,EmployeeUpdateDto employee) 
        {
            if (! await _companyRepository.CompanyExistsAsync(companyId))
            {
                return NotFound();
            }
            
            var employeeEntity = await _companyRepository.GetEmployee(companyId, employeeId);
            if (employeeEntity==null)
            {
                var employeeToAddEntity = _mapper.Map<Employee>(employee);
                employeeToAddEntity.ID = employeeId;
                _companyRepository.AddEmployee(companyId, employeeToAddEntity);
                await _companyRepository.SaveAsync();

                var dtoToReturn = _mapper.Map<EmployeeDto>(employeeToAddEntity);
                return CreatedAtRoute(
                    nameof(GetEmployeeFromCompany),     //此参数为 ：GetEmployeeFromCompnay方法的[HttpGet]属性条目中已标识了Route的名称
                    new
                    {
                        companyId = dtoToReturn.CompanyId,
                        employeeId = dtoToReturn.ID
                    },
                    dtoToReturn);
            }

            //1、entity转化为UpdateDto
            //2、把传进来的Employee的值更新至updateDto
            //3、把updateDto映射回entity
            //下面的语句把上面的三个步骤都完成了
            _mapper.Map(employee, employeeEntity);

            _companyRepository.UpdateEmployee(employeeEntity);
            await _companyRepository.SaveAsync();

            return NoContent();
        }
    }
}
