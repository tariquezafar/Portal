using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
 public class ProductGLMappingViewModel
    {
        public long MappingId { get; set; }
        public int ProductMainGroupId { get; set; }
        public string ProductMainGroupName { get; set; }
        public int ProductSubGroupId { get; set; }
        public string ProductSubGroupName { get; set; } 
        public int GLId { get; set; }
        public string GLHead { get; set; }

        public string GLType { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int MappingStatus { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string CreatedDate { get; set; }
    }
}
