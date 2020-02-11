﻿using AutoMapper;
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
    }
}
