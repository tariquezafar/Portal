using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
 public class AppointmentViewModel
    {
        public long AppointLetterId { get; set; }
        public string AppointLetterNo { get; set; }
        public string ApplicantNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string AppointDate { get; set; }
        public int CompanyId { get; set; }
        public long InterviewId { get; set; }
        public string InterviewNo { get; set; }
        public string JoiningDate { get; set; }
        public string AppointmentLetterDesc { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByUserName { get; set; }
        public string CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedByUserName { get; set; }
        public string ModifiedDate { get; set; }
        public string AppointStatus { get; set; }

        public int companyBranch { get; set; }

        public string companyBranchName { get; set; }
    }

    public class AppointmentCTCViewModel
    {
        public long AppointCTCId { get; set; }
        public long AppointLetterId { get; set; }
        public decimal Basic { get; set; }
        public decimal HRAAmount { get; set; }
        public decimal Conveyance { get; set; }
        public decimal Medical { get; set; }
        public decimal ChildEduAllow { get; set; }
        public decimal LTA { get; set; }
        public decimal SpecialAllow { get; set; }
        public decimal OtherAllow { get; set; }
        public decimal GrossSalary { get; set; }
        public decimal EmployeePF { get; set; }
        public decimal EmployeeESI { get; set; }
        public decimal ProfessionalTax { get; set; }
        public decimal NetSalary { get; set; }
        public decimal EmployerPF { get; set; }
        public decimal EmployerESI { get; set; }
        public decimal MonthlyCTC { get; set; }
        public decimal YearlyCTC { get; set; }
    }
}
