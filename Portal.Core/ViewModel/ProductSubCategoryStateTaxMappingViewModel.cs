using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
 public class ProductSubCategoryStateTaxMappingViewModel
    {
        public long MappingId { get; set; }
        public int ProductMainGroupId { get; set; }
        public string ProductMainGroupName { get; set; }
        public int ProductSubGroupId { get; set; }
        public string ProductSubGroupName { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public int TaxId { get; set; }
        public string TaxName { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int MappingStatus { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string CreatedDate { get; set; }
    }
}
