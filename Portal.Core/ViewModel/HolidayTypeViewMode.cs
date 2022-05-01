using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class HolidayTypeViewModel:IModel
    {
        public int HolidayTypeId { get; set; }
        public string HolidayTypeName { get; set; } 
        public bool HolidayType_Status { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public int CompanyBranchId { get; set; }
        public string CompanyBranchName { get; set; }
    }
    
}
