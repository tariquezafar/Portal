using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class EmployeeMarkAttendanceViewModel
    {
        public long EmployeeAttendanceId { get; set; }   
        public string AttendanceDate { get; set; }
        public int CompanyId { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string PresentAbsent { get; set; }        
        public string TrnDateTime { get; set; }
        public string AttendanceStatus { get; set; }
        public string InOut { get; set; }
        public int AttendanceApprovedBy { get; set; }
        public string AttendanceApproveDate { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public int companyBranchId { get; set; }

    }
   
}
