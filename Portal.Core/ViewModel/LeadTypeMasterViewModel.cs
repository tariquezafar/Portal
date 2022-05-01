using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
    public class LeadTypeMasterViewModel
    {

        public int LeadTypeId { get; set; }
        public string LeadTypeName { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int Modifiedby { get; set; }
        public string ModifiedDate { get; set; }
        public int Companyid { get; set; }
        public int CompanyBranchid { get; set; }
        public string CreatedByUserName { get; set; }
        public string ModifiedbyUserName { get; set; }
        public string status { get; set; }
        public string CompanyBranchName { get; set; }



    }
}
