using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class EmployeeClaimApplicationViewModel
    {
        public long ApplicationId { get; set; }       
        public string ApplicationNo { get; set; }
        public string ApplicationDate { get; set; }
        public int CompanyId { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string ClaimTypeName{ get; set; }      
        public int ClaimTypeId { get; set; }   
        
        public decimal ClaimAmount { get; set; }
        public string ClaimReason { get; set; }

        public string ApprovedByUserName { get; set; }
        public string ClaimStatus { get; set; }       
        public int ApproveBy { get; set; }
        public string ApproveDate { get; set; }
        public int RejectBy { get; set; }
        public string RejectDate { get; set; }
        public string RejectReason { get; set; }
        public string message { get; set; }
        public string status { get; set; }

        public int CompanyBranchId { get; set; }
        public string CompanyBranchName { get; set; }

    }
   
}
