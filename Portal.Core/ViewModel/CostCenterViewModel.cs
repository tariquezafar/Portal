using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
    public class CostCenterViewModel
    {
        public int CostCenterId { get; set; }
        public string CostCenterName { get; set; }
        public  int CompanyId { get; set; }
        public bool CostCenter_Status { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedName { get; set; }
        public string CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedName { get; set; }
        public string ModifiedDate { get; set; }
        public string message { get; set; }
        public bool status { get; set; }

        public long CompanyBranchId { get; set; }
        public string CompanyBranchName { get; set; }
    }
}
