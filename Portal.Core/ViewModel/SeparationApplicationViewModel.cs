using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class SeparationApplicationViewModel
    {
        public long ApplicationId { get; set; }       
        public string ApplicationNo { get; set; }
        public string ApplicationDate { get; set; }
        public int CompanyId { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string SeparationCategoryName { get; set; }      
        public int SeparationCategoryId { get; set; }           
        public string Reason { get; set; }
        public string Remarks { get; set; }
        public string ApplicationStatus { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string CreatedDate { get; set; }
        public int Modifiedby { get; set; }
        public string ModifiedByName { get; set; }
        public string ModifiedDate { get; set; } 
        public string ApprovalStatus { get; set; }
        public int ApprovedBy { get; set; }
        public string ApprovedByName { get; set; }
        public string ApprovedDate { get; set; }
        public string RejectionStatus { get; set; }
        public int RejectBy { get; set; }
        public string RejectedByName { get; set; }
        public string RejectDate { get; set; }
        public string RejectReason { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public int CompanyBranchId { get; set; }
        public string CompanyBranchName { get; set; }

    }

}
