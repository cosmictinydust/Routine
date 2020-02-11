using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Routine.Api.Entities;
using Routine.Api.Helpers;
using Routine.Api.Models;
using Routine.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.Controllers
{
    [ApiController]
    [Route("api/companycollections")]
    public class CompanycollectionsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICompanyRepository _companyRepository;

        public CompanycollectionsController(IMapper mapper, ICompanyRepository companyRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
        }

        [HttpGet("({ids})", Name = nameof(GetcompanyCollection))]
        public async Task<IActionResult> GetcompanyCollection(
            [FromRoute]
            [ModelBinder(BinderType =typeof(ArrayModelBinder))]     //使用继承于IModelBinder接口的一个Helper实现参数绑定类似 "1,2,3" 的字符串
            IEnumerable<Guid> ids
            )
        {
            if (ids == null)
            {
                return BadRequest();
            }

            var entities = await _companyRepository.GetCompaniesAsync(ids);
            if (ids.Count() != entities.Count())
            {
                return NotFound();
            }
            var dtosToReturn = _mapper.Map<IEnumerable<CompanyDto>>(entities);
            return Ok(dtosToReturn);
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> CreateCompanyCollection(
            IEnumerable<CompanyAddDto> companyCollection)
        {
            var companyEntities = _mapper.Map<IEnumerable<Company>>(companyCollection);
            foreach (var company in companyEntities)
            {
                _companyRepository.AddCompany(company);
            }
            await _companyRepository.SaveAsync();

            var dtoToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);

            var idsString = string.Join(",", dtoToReturn.Select(x => x.ID));  //生成中间用","分隔的Guid字符串

            return CreatedAtRoute(nameof(GetcompanyCollection), new { ids = idsString }, dtoToReturn);

        }

    }
}
