using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class PayHeadGLMappingViewModel : IModel
    {  
        public int PayHeadMappingId { get; set; }
        public string PayHeadName { get; set; } 
        public int GLId { get; set; } 
        public string GLCode { get; set; }
        public int SLId { get; set; }
        public string SLCode { get; set; } 
        public string TaxGLHead { get; set; }
        public string TaxSLHead { get; set; }
        public int CompanyId { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedName { get; set; }
        public string CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedName { get; set; }
        public string ModifiedDate { get; set; }
        public bool PayHeadGLMapping_Status { get; set; }
        public string message { get; set; }
        public bool Status { get; set; }
        public string CompanyBranchName { get; set; }
        public int CompanyBranch { get; set; }

    }
    
}
