using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Routine.Api.Entities;
using Routine.Api.Models;
using Routine.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;

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

        [HttpPatch("{employeeId}")]
        public async Task<ActionResult<EmployeeDto>> PartiallyUpdateEmployeeFroCompany(
            Guid companyId,
            Guid employeeId,
            JsonPatchDocument<EmployeeUpdateDto> patchDocument)
        {
            if (!await _companyRepository.CompanyExistsAsync(companyId))
            {
                return NotFound();
            }

            var employeeEntity =await _companyRepository.GetEmployee(companyId, employeeId);
            if (employeeEntity == null)     //如果没有找到符合条件的employee,就进行新增操作
            {
                var employeeDto = new EmployeeUpdateDto();
                patchDocument.ApplyTo(employeeDto, ModelState);
                if (!TryValidateModel(employeeDto))
                {
                    #region     
                    var probleDetails = new ValidationProblemDetails(ModelState)
                    {
                        Type = "http://www.baidu.com",
                        Title = "有错误！！！",
                        Status = StatusCodes.Status422UnprocessableEntity,
                        Detail = "请看详细信息",
                        Instance = HttpContext.Request.Path
                    };

                    probleDetails.Extensions.Add("traceId", HttpContext.TraceIdentifier);

                    return new UnprocessableEntityObjectResult(probleDetails)
                    {
                        ContentTypes = { "application/problem+json" }
                    };
                    #endregion
                }

                var employeeToAdd = _mapper.Map<Employee>(employeeDto);

                employeeToAdd.ID = employeeId;
                _companyRepository.AddEmployee(companyId, employeeToAdd);
                await _companyRepository.SaveAsync();

                var dtoToReturn = _mapper.Map<EmployeeDto>(employeeToAdd);
                return CreatedAtRoute(
                    nameof(GetEmployeeFromCompany),     //此参数为 ：GetEmployeeFromCompnay方法的[HttpGet]属性条目中已标识了Route的名称
                    new
                    {
                        companyId = dtoToReturn.CompanyId,
                        employeeId = dtoToReturn.ID
                    },
                    dtoToReturn);
            }

            var dtoToPatch = _mapper.Map<EmployeeUpdateDto>(employeeEntity);
            
            //侍更新的数据需要进行验证处理
            patchDocument.ApplyTo(dtoToPatch,ModelState);   //这里的第二个参数是在把patchDocument应用到EmployeeUpdateDto时验证Model的合法性，如果错误的话，就引发TryValidateModel为false

            if (!TryValidateModel(dtoToPatch))
            {
                //return ValidationProblem(ModelState);     //使用这种的话，返回的错误代码为400

                //如果要返回422的话，有下面两种方法
                //方法一：直接写格式
                #region     
                var probleDetails = new ValidationProblemDetails(ModelState)
                {
                    Type = "http://www.baidu.com",
                    Title = "有错误！！！",
                    Status = StatusCodes.Status422UnprocessableEntity,
                    Detail = "请看详细信息",
                    Instance = HttpContext.Request.Path
                };

                probleDetails.Extensions.Add("traceId", HttpContext.TraceIdentifier);

                return new UnprocessableEntityObjectResult(probleDetails)
                {
                    ContentTypes = { "application/problem+json" }
                };
                #endregion
                //方法二：还是使用ValidationProblem,但因为他是在ControllerBase下的，错误信息模板不会使用已在Startup.cs定义的那个，而是使用默认的，如果要返回我们自定义的模板，就需要重写一下个这方法
                //return ValidationProblem(ModelState); 重写的代码一直出错，不知什么原因 public override ActionResult ValidationProblem。。。。
            }

            _mapper.Map(dtoToPatch, employeeEntity);
            _companyRepository.UpdateEmployee(employeeEntity);
            await _companyRepository.SaveAsync();
            return NoContent();
        }

        //public override ActionResult ValidationProblem([ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
        //{
        //    var options = HttpContext.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>();

        //    return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
        //}

    }
}


