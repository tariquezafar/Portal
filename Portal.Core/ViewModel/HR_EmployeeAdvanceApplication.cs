using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class HR_EmployeeAdvanceApplicationViewModel : IModel
    {
        public int ApplicationId { get; set; }
        public string ApplicationNo { get; set; }
        public string ApplicationDate { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } 
        public int AdvanceTypeId { get; set; }
        public string AdvanceTypeName { get; set; } 
        public int AdvanceAmount { get; set; }
        public string AdvanceReason { get; set; } 
        public int AdvanceInstallmentAmount { get; set; }
        public string AdvanceStatus { get; set; } 
        public string AdvanceStartDate { get; set; }
        public string AdvanceEndDate { get; set; }
        public int CompanyId { get; set; }
        public int CreatedBy { get; set; }  
        public int ApproveBy { get; set; }
        public string ApprovedByUserName { get; set; }
        public string ApproveDate { get; set; } 
        public int RejectBy { get; set; }
        public string RejectByUserName { get; set; }
        public string RejectDate { get; set; }
        public string RejectReason { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public int companyBranch { get; set; }
        public string  companyBranchName { get; set; }

    }
    
}
