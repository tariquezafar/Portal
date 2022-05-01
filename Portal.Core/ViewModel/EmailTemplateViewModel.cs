using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
    public class EmailTemplateViewModel
    {
        public int EmailTemplateId { get; set; }
        public string EmailTemplateSubject { get; set; }
        public int EmailTemplateTypeId { get; set; }
        public string EmailTemplateTypeName { get; set; }
        public string EmailTemplateDesc { get; set; }
        public int CompanyId { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedByName { get; set; }
        public string ModifiedDate { get; set; }
        public bool EmailTemplateStatus { get; set; }
        public int CompanyBranchId { get; set; }
        public string CompanyBranchName { get; set; }
    }
    public class EmailTemplateTypeViewModel
    {
        public int EmailTemplateTypeId { get; set; }
        public string EmailTemplateName { get; set; }
        public bool Status { get; set; }
        
    }
}
