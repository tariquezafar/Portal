using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class ExitInterviewViewModel
    {
        public long ExitInterviewId { get; set; }       
        public string ExitInterviewNo { get; set; }
        public string ExitInterviewDate { get; set; }
        public int CompanyId { get; set; }
        public long ApplicationId { get; set; }
        public string ApplicationNo { get; set; }
        public long EmployeeId { get; set; } 
        public string EmployeeName { get; set; }
        public string InterviewDescription { get; set; }      
        public string InterviewStatus { get; set; }
        public int InterviewByUserId { get; set; }
        public string InterviewByUserName { get; set; }
        
        public string InterviewRemarks { get; set; } 
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
