using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
 public   class TargetTypeViewModel
    {
        public int TargetTypeId { get; set; }
        public string TargetName { get; set; }
        public string TargetDesc { get; set; }
        public bool Status { get; set; }
        public bool TargetType_Status { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int Modifiedby { get; set; }
        public string ModifiedDate { get; set; }
        public string CreatedByUser { get; set; }
        public string ModifyByUser { get; set; }

        public int CompanyBranchId { get; set; }
        public string CompanyBranchName { get; set; }
    }
}
