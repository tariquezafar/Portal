using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class EmployeeLeaveApplicationViewModel : IModel
    {
        public int ApplicationId { get; set; }
        public string ApplicationNo { get; set; }
        public string ApplicationDate { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } 
        public int LeaveTypeId { get; set; }
        public string LeaveTypeName { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public decimal NoofDays { get; set; }
        public string LeaveReason { get; set; }  
        public string LeaveStatus { get; set; }  
        public int CompanyId { get; set; } 
        public int ApproveBy { get; set; }
        public string ApprovedByUserName { get; set; }
        public string ApproveDate { get; set; } 
        public int RejectBy { get; set; }
        public string RejectByUserName { get; set; }
        public string RejectDate { get; set; }
        public string RejectReason { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public int CompanyBranchId { get; set; }
        public string CompanyBranchName { get; set; }
    }
    
}
