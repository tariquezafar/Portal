using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core
{
  public class EmployeeLeaveDetailViewmodel
    {
        public long EmployeeLeaveId { get; set; }
        public int CompanyId { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int LeaveTypeId { get; set; }
        public string LeaveTypeName { get; set; }
        public decimal LeaveCount { get; set; }
        public string LeaveDate { get; set; }
        public int FinYearId { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string CreatedDate { get; set; }
        public int  ModifiedBy { get; set; }
        public string ModifiedByName { get; set; }
        public string ModifiedDate { get; set; }
        public bool EmployeeLeaveDetailStatus { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}
