using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
    public class TDSSetionViewModel
    {
        public int SectionId { get; set; }
        public string SectionName { get; set; }
        public string SectionDesc { get; set; }
        public decimal SectionMaxValue { get; set; }
        public bool Status { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int Modifiedby { get; set; }
        public string ModifiedDate { get; set; }
        public bool TDSSetion_Status { get; set; }
        public int CompanyBranch { get; set; }
        public string CompanyBranchName { get; set; }
    }
    public class TDSSectionDocumentDetailViewModel
    {
        public int SectionDetailID { get; set; }
        public int SectionId { get; set; }
        public string DocumentName { get; set; }
    }
}
