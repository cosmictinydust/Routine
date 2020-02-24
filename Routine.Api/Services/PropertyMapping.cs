using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.Services
{
    public class PropertyMapping<TSource, TDestination>: IPropertyMapping
    {
        public Dictionary<string, PropertyMappingValue> MappingDictionary { get;private set; }

        public PropertyMapping(Dictionary<string, PropertyMappingValue> mappingDictionary)
        {
            if (mappingDictionary is null)
            {
                throw new ArgumentNullException(nameof(mappingDictionary));
            }

            MappingDictionary = mappingDictionary;
        }
    }

}
