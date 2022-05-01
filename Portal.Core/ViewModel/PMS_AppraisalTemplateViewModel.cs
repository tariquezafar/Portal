using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{

    public partial class PMS_AppraisalTemplateViewModel
    {
        public long TemplateId { get; set; }
        public string TemplateName { get; set; }
        public int CompanyId { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public int DesignationId { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedByUserName { get; set; }
        public string ModifiedByUserName { get; set; }
        
        public bool AppraisalTemplate_Status { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public int CompanyBranchId { get; set; }
        public string CompanyBranchName { get; set; }
    }


    public class PMS_AppraisalTemplateGoalViewModel
    {
        public long TemplateGoalId { get; set; }
        public int TemplateId { get; set; }
        public int TaxSequenceNo { get; set; }
        public int GoalId { get; set; }
        public string GoalName { get; set; }
        public bool Goal_Status { get; set; }
        public string message { get; set; }
        public string status { get; set; }

    }
}
