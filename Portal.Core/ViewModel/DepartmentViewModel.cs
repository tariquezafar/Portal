using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class DepartmentViewModel
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentCode { get; set; } 
        public int CompanyId { get; set; } 
        public int CreatedBy { get; set; } 
        public string CreatedDate { get; set; }
        public int? ModifiedBy { get; set; } 
        public string ModifiedDate { get; set; } 
        public bool DepartmentStatus { get; set; }
        public string message { get; set; }
        public string status { get; set; }

        public int CompanyBranchId { get; set; }
        public string CompanyBranchName { get; set; }

    }
   
}
