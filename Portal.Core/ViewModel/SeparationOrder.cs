using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class SeparationOrderViewModel
    {
        public long SeparationOrderId { get; set; }       
        public string SeparationOrderNo { get; set; }
        public string SeparationOrderDate { get; set; }
        public int CompanyId { get; set; }
        public long ExitInterviewId { get; set; }
        public string ExitInterviewNo { get; set; }
        public long EmployeeId { get; set; } 
        public string EmployeeName { get; set; }
        public long EmployeeClearanceId { get; set; }
        public string EmployeeClearanceNo { get; set; }
        public string SeparationStatus { get; set; } 
        public string ExperienceLetter { get; set; }
        public string SepartionRemarks { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string CreatedDate { get; set; }
        public int Modifiedby { get; set; }
        public string ModifiedByName { get; set; }
        public string ModifiedDate { get; set; }  
        public string message { get; set; }
        public string status { get; set; } 
    }
   
}
