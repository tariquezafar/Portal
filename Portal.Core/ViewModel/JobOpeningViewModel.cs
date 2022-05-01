using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
 public class JobOpeningViewModel
    {
        public long JobOpeningId { get; set; }
        public string JobOpeningNo { get; set; }
        public string RequisitionNo { get; set; }
        public int CompanyId { get; set; }
        public string JobOpeningDate { get; set; }
        public string JobTitle { get; set; }
        public string JobPortalRefNo { get; set; }
        public int NoOfOpening { get; set; }
        public int MinExp { get; set; }
        public int MaxExp { get; set; }
        public decimal MinSalary { get; set; }
        public decimal MaxSalary { get; set; }
        public string KeySkills { get; set; }
        public string JobDescription { get; set; }
        public string JobStartDate { get; set; }
        public string JobExpiryDate { get; set; }
        public string CreatedByUserName { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedByUserName { get; set; }
        public string ModifiedDate { get; set; }
        public string JobStatus { get; set; }
        public int RequisitionId { get; set; }
        public string RequisitionName { get; set; }
        public int EducationId { get; set; }

        public string EducationName { get; set; }
        public string OtherQualification { get; set; }
        public string CurrencyCode { get; set; }
        public string Remarks { get; set; }
        public int CompanyBranchId { get; set; }
        public string CompanyBranchName { get; set; }
    }
}
