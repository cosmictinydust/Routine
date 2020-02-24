using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Routine.Api.DtoParameters;
using Routine.Api.Entities;
using Routine.Api.Helpers;
using Routine.Api.Models;
using Routine.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
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

        [HttpGet(Name =nameof(GetCompanies))]
        [HttpHead]  //让该方法支持 Head 请求，可能用于查看Api的此功能是否可以正常使用，执行后不返回具体数据，只返回状态码，但里面的代码是跟get一样执行的，注意这个 Head 请求的写法只能写在Get操作的方法上
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompanies(      //使用 ActionResult<T>注明返回类型，有利于自动文档的建立 
            [FromQuery] CompanyDtoParameters parameters)        //现在的parameters参数属于复杂参数，netcore默认这个是[FromBody]的，但如果现在用http://localhost:5000/api/companies/?CompanyName=Microsoft&SearchTerm=i这样的Uril去访问这个资源的话，将会出现415的错误码，因为这个Uril用的是[FromQuery]方式接收参数，与请求中的参数来源不一致导致的，所以这里的参数前要加上[FromQuery]就说明参数的来源.
        {
            var companies = await _companyRepository.GetCompaniesAsync(parameters);

            var previousPageLink = companies.HasPrevious ? CreateCompanyesResourceUri(parameters, ResourceUriType.PreviousPage) : null;
            var nextPageLink = companies.HasNext ? CreateCompanyesResourceUri(parameters, ResourceUriType.NextPage) : null;
            var paginationMatadata = new
            {
                totalCount=companies.TotalCount,
                pageSize=companies.PageSize,
                currentPage=companies.CurrentPage,
                totalPages=companies.TotalPages,
                previousPageLink,
                nextPageLink
            };
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMatadata,
                //加入这个参数，把生成的Link中把不安全的字符比如“\”自动转化安全的功能去掉，也就是生成象"http://abc.com/abc"这种
                new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                }));

            var companyDtos = _mapper.Map<IEnumerable<CompanyDto>>(companies);
            
            return Ok(companyDtos);
        }

        [HttpGet("{companyId}",Name =nameof(GetCompany))]
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

        [HttpPost]
        public async Task<ActionResult<CompanyDto>> CreateCompany([FromBody]CompanyAddDto company) //company参数属于复杂类型参数，就算不写上[FromBody]也会默认为此类型
        {
            //情景一 在2.x版本或之前版本，或没有使用[ApiController]这个属性的话，应该先检查一下输入的参数是否为空
            //if (company == null)
            //{
            //    return BadRequest();
            //}

            //情景二 在没有使用[ApiController]这个属性标识时，当Model验证失败时不会自动返回422错误代码，须要手动添加如下代码
            //if (!ModelState.IsValid)
            //{
            //    return UnprocessableEntity(ModelState);     
            //}

            var entity = _mapper.Map<Company>(company);
            _companyRepository.AddCompany(entity);
            await _companyRepository.SaveAsync();

            var returnDto = _mapper.Map<CompanyDto>(entity);
            return CreatedAtRoute(nameof(GetCompany), new { companyId = returnDto.ID }, returnDto);
        }

        [HttpDelete("{companyId}")]
        public async Task<IActionResult> DeleteCompany(Guid companyID)
        {
            var companyEntity =await _companyRepository.GetCompanyAsync(companyID);
            if (companyEntity == null)
            {
                return NotFound();
            }
            await _companyRepository.GetEmployeesAsync(companyID, new EmployeeDtoParameters());  //不加上这句会出现 FOREIGN KEY constraint failed 外键约束错误的提示，原因未明

            _companyRepository.DeleteCompany(companyEntity);
            await _companyRepository.SaveAsync();
            return NoContent();
        }

        private string CreateCompanyesResourceUri(CompanyDtoParameters parameters,ResourceUriType type)
        {
            switch (type)
            {
                //返回的参数一定要把CompanyDtoParameters里的属性都返回了，要不然查询的结果就会与之前的不一样了
                case ResourceUriType.PreviousPage:
                    return Url.Link(nameof(GetCompanies), new       //返回给客户端的属性，要小写字母开头
                    {
                        pageNumber = parameters.PageNumber-1,
                        pageSize = parameters.PageSize,
                        companyName = parameters.CompanyName,
                        searchTerm = parameters.SearchTerm
                    });
                case ResourceUriType.NextPage:
                    return Url.Link(nameof(GetCompanies), new
                    {
                        pageNumber = parameters.PageNumber+1,
                        pageSize = parameters.PageSize,
                        companyName = parameters.CompanyName,
                        searchTerm = parameters.SearchTerm
                    });
                default:
                    return Url.Link(nameof(GetCompanies), new
                    {
                        pageNumber = parameters.PageNumber,
                        pageSize = parameters.PageSize,
                        companyName = parameters.CompanyName,
                        searchTerm = parameters.SearchTerm
                    });
            }
        }

    }
}
