using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{

    public partial class ClearanceTemplateViewModel
    {
        public long ClearanceTemplateId { get; set; }
        public string ClearanceTemplateName { get; set; }
        public int CompanyId { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public int DesignationId { get; set; }
        public int SeparationCategoryId { get; set; }
        public string  SeparationCategoryName { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedByUserName { get; set; }
        public string ModifiedByUserName { get; set; } 
        public bool ClearanceTemplate_Status { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
        public string message { get; set; }
        public string status { get; set; }
    }


    public class ClearanceTemplateDetailViewModel
    {
        public long ClearanceTemplateDetailId { get; set; }
        public string ClearanceTemplateName { get; set; }
        public int ClearanceTemplateId { get; set; } 
        public int SeparationClearListId { get; set; }
        public int TaxSequenceNo { get; set; }
        public string  SeparationClearListName { get; set; }
        public int ClearanceByUserId { get; set; }
        public string ClearanceByUserName { get; set; }
        public bool ClearanceTemplateDetail_Status { get; set; }
        public string message { get; set; }
        public string status { get; set; }

    }
}
