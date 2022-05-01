using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class HR_EmployeeAttendanceViewModel : IModel
    {
        public long EmployeeAttendanceId { get; set; }
        public int CompanyId { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string AttendanceDate { get; set; }
        public string PresentAbsent { get; set; }
        public string TrnDateTime { get; set; }
        public string InTime { get; set; }
        public string OutTime { get; set; }
        public string AttendanceStatus { get; set; }
        public int AttendanceApprovedBy { get; set; }
        public string AttendanceApproveDate { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
        public string InOut { get; set; }
        public string LeaveStatus { get; set; }
        public int companyBranch { get; set; }
        public string companyBranchName { get; set; }
    }
    
}
