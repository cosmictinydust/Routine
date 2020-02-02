using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Routine.Api.Models;
using Routine.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.Controllers
{
    [ApiController]
    [Route("api/companies")]
    //[Route("api/[controller]")] 也可以用这种方法代替上面的那一句
    public class CompaniesController:ControllerBase
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;

        public CompaniesController(ICompanyRepository companyRepository,IMapper mapper)
        {
            _companyRepository = companyRepository??
                throw new ArgumentNullException(nameof(companyRepository));
            _mapper = mapper??throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [HttpHead]  //让该方法支持 Head 请求，可能用于查看Api的此功能是否可以正常使用，执行后不返回具体数据，只返回状态码，但里面的代码是跟get一样执行的，注意这个 Head 请求的写法只能写在Get操作的方法上
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompanies() //使用 ActionResult<T>注明返回类型，有利于自动文档的建立 
        {
            var companies = await _companyRepository.GetCompaniesAsync();
            var companyDtos = _mapper.Map<IEnumerable<CompanyDto>>(companies);
            return Ok(companyDtos);
        }

        [HttpGet("{companyId}")]
        //[HttpGet]
        //[Route("{companyId}")]  //可以用注悉掉的这两句代替上面的
        public async Task<ActionResult<CompanyDto>> GetCompany(Guid companyId)
        {
            var company = await _companyRepository.GetCompanyAsync(companyId);
            if (company == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<CompanyDto>(company));
        }

    }
}
