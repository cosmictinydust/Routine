using Routine.Api.Entities;
using Routine.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.Services
{
    public class PropertyMappingService : IPropertyMappingService
    {
        //这里是Employee的排序功能的映射对应关系
        private readonly Dictionary<string, PropertyMappingValue> _employeePropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)  //忽略大小写
            {
                {"Id",new PropertyMappingValue(new List<string>{"Id"}) },
                {"CompanyId",new PropertyMappingValue(new List<string>{"CompanyId"}) },
                {"EmployeeNo",new PropertyMappingValue(new List<string>{"EmployeeNo"}) },
                {"Name",new PropertyMappingValue(new List<string>{"FirstName","LastName"}) },    //Name对FirstName+LastName的映射
                {"Age",new PropertyMappingValue(new List<string>{"DateOfBirth"},true) }         //Age对DateOfBrith的映射
            };


        //还可以有其他Model的排序功能的映射对应关系
        private Dictionary<string, PropertyMappingValue> _companyPropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)  //忽略大小写
            {
                {"Id",new PropertyMappingValue(new List<string>{"Id"}) },
                {"CompanyName",new PropertyMappingValue(new List<string>{"Name"}) },
                {"Country",new PropertyMappingValue(new List<string>{"Country"}) },
                {"Industry",new PropertyMappingValue(new List<string>{ "Industry" }) },   
                {"Product",new PropertyMappingValue(new List<string>{"Product"}) },
                {"Introduction",new PropertyMappingValue(new List<string>{"Introduction"}) }
            };



        //private IList<PropertyMapping<TSource,TDestination>> _propertyMappings;   //注意这个是建立不了的，但如果让PropertyMapping这个类变为的个接口就可以了，可以建立一个空的接口IPropertyMapping，让PropertyMapping继承于IPropertyMapping接口就可以了，如下面那行所写的
        private readonly IList<IPropertyMapping> _propertyMappings = new List<IPropertyMapping>();
        public PropertyMappingService()
        {
            _propertyMappings.Add(new PropertyMapping<EmployeeDto, Employee>(_employeePropertyMapping));
            _propertyMappings.Add(new PropertyMapping<CompanyDto, Company>(_companyPropertyMapping));  //如果已做好上面的那个_companyPropertyMapping也可以把那个对应关系字典放在_propertyMappings这个集合里面
        }

        //可以根据输入数据的类型，提取相应的映射关系的数据字典
        public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>()
        {
            var matchingMapping = _propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();   //偿试找一下对应关系是否存在

            var propertyMappings = matchingMapping.ToList();

            if (propertyMappings.Count == 1)  //如果只找到了一个关系，正常返回
            {
                return propertyMappings.First().MappingDictionary;
            }

            throw new Exception($"无法找到唯一的映射关系：{typeof(TSource)},{typeof(TDestination)}");
        }

        //此方法用于检查用户输入的OrderBy是否存在此映射
        public bool VaildMappingExistsFor<TSource,TDestination>(string fields)
        {
            var propertyMapping = GetPropertyMapping<TSource, TDestination>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                return true;
            }

            var fieldAffterSplit = fields.Split(",");
            foreach (var field in fieldAffterSplit)
            {
                var trmmedField = field.Trim();
                var indexofFirstSpace = trmmedField.IndexOf(" ", StringComparison.Ordinal);
                var propertyName = (indexofFirstSpace == -1) ? trmmedField
                    : trmmedField.Remove(indexofFirstSpace);

                if (!propertyMapping.ContainsKey(propertyName))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
