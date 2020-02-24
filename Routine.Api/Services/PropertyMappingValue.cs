using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.Services
{
    public class PropertyMappingValue
    {
        public IEnumerable<string> DestinationProperties { get; set; }  //用于处理FirstName + LastName = Name 这种情况

        public bool Revert { get; set; }    //处理出生日期与年龄的排序时逻辑的反转问题，即按出生日期升序排列的话，就是年龄的降序排列

        public PropertyMappingValue(IEnumerable<string> destinationProperties,bool revert=false) //设置反转排序功能默认值为 false 
        {
            DestinationProperties = destinationProperties??throw new ArgumentNullException(nameof(destinationProperties));
            Revert = revert;
        }
    }
}
