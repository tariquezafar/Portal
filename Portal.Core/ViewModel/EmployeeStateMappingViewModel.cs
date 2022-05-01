using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
  public  class EmployeeStateMappingViewModel
    {
        public long EmployeeStateMappingId { get; set; }
        public int EmployeeId { get; set; }
        public string  EmployeeName { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public string  StateCode { get; set; }
        public bool  SelectState { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool EmployeeStateStatus { get; set; }
        public string message { get; set; }
        public string status { get; set; }
    }
}
