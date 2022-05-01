using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
   public class TermTemplateViewModel
    {
         
     
        public int TermTemplateId { get; set; }
        public string TermTempalteName { get; set; }
        public int CompanyId { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string CreatedDate { get; set; }
        public int Modifiedby { get; set; }
        public string ModifiedByUserName { get; set; }
        public string ModifiedDate { get; set; }
        public bool TermTemplate_Status { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public int CompanyBranchId { get; set; }
        public string CompanyBranchName { get; set; }



    }

    public class TermTemplateDetailViewModel
    {
        public long MappingId { get; set; }
        public int SequenceNo { get; set; }
        public long TrnId { get; set; }
        public int TermTemplateId { get; set; }
        public string TermsDesc { get; set; }
        public bool Term_Status { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public int CompanyBranchId { get; set; }
        public string CompanyBranchName { get; set; }

    }
}
