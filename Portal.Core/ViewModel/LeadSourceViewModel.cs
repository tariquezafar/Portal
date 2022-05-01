using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class LeadSourceViewModel:IModel
    {
        public int LeadSourceId { get; set; }
        public string LeadSourceName { get; set; } 
        public int CompanyId { get; set; }
        public bool LeadSource_Status { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public int CompanyBranchId { get; set; }
        public string CompanyBranchName { get; set; }

    }
    
}
