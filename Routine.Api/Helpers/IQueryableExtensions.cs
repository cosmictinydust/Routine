using Routine.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Routine.Api.Helpers
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> ApplySort<T>(
            this IQueryable<T> source,
            string orderBy,
            Dictionary<string,PropertyMappingValue> mappingDictionary)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (mappingDictionary is null)
            {
                throw new ArgumentNullException(nameof(mappingDictionary));
            }

            if (string.IsNullOrWhiteSpace(orderBy))
            {
                return source;
            }

            var orderByAfterSplit = orderBy.Split(",");  //把,分隔的字段变为string[]

            foreach (var orderByClause in orderByAfterSplit.Reverse())  //用Reverse()把string[]的顺序反一下
            {
                var trimmedOrderByClause = orderByClause.Trim();        //把orderByClause中的空格去掉

                var orderDescending = trimmedOrderByClause.EndsWith(" desc");       //查找有没有以" desc"结尾

                var indexOfFirstSpace = trimmedOrderByClause.IndexOf(" ",StringComparison.CurrentCultureIgnoreCase);      //找到第一个空格的位置

                var propertyName = indexOfFirstSpace == -1      
                    ? trimmedOrderByClause      //如果没有空格 返回trimmedOrderByClause
                    : trimmedOrderByClause.Remove(indexOfFirstSpace);   //如果有空格,从第一个空格的位置起删除后面的字符串

                if (!mappingDictionary.ContainsKey(propertyName))
                {
                    throw new ArgumentNullException($"没有找到key为{propertyName}的映射");
                }

                var propertyMappingValue = mappingDictionary[propertyName]; 
                if (propertyMappingValue == null)
                {
                    throw new ArgumentNullException(nameof(propertyMappingValue));
                }

                foreach (var destinationProperty in propertyMappingValue.DestinationProperties.Reverse())
                {
                    if (propertyMappingValue.Revert)
                    {
                        orderDescending = !orderDescending;     //实现正序反转功能....

                    }
                    source = source.OrderBy(destinationProperty + (orderDescending ? " descending" : " ascending"));
                }
            }

            return source;
        }
    }
}
