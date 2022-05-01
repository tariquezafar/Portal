using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{

    public partial class EmployeeClearanceProcessViewModel
    {
        public int EmployeeClearanceId { get; set; }
        public string EmployeeClaaranceNo { get; set; }
        public int CompanyId { get; set; }
        
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public long ClearanceTemplateId { get; set; }
        public string ClearanceTemplateName { get; set; }
        public string ClearanceFinalStatus { get; set; } 
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedByUserName { get; set; }
        public string ModifiedByUserName { get; set; } 
        public int ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
        public string message { get; set; }
        public string status { get; set; }
    }


    public class EmployeeClearanceProcessDetailViewModel
    {
        public int TaxSequenceNo { get; set; }
        public long EmployeeClearanceDetailId { get; set; }
        public long EmployeeClearanceId { get; set; } 
        public int SeparationClearListId { get; set; }
        public string SeparationClearListName { get; set; }
        public int ClearanceByUserId { get; set; } 
        public string ClearanceByUserName { get; set; }

        public string ClearanceRemarks { get; set; }
        public string ClearanceStatus { get; set; }
        public string message { get; set; }
        public string status { get; set; }

    }
}
