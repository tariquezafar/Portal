using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
    public class ApplicantViewModel
    {
        public long ApplicantId { get; set; }
        public string ApplicantNo { get; set; }
        public string ApplicationDate { get; set; }
        public long JobOpeningId { get; set; }
        public string JobOpeningNo { get; set; }
        public int CompanyId { get; set; }
        public int CompanyBranchId { get; set; }
        public string CompanyBranchName { get; set; }
        public string ProjectNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string FatherSpouseName { get; set; }
        public string DOB { get; set; }
        public string BloodGroup { get; set; }
        public string MaritalStatus { get; set; }
        public string ApplicantAddress { get; set; }
        public string City { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string PinCode { get; set; }
        public string ContactNo { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public int ResumeSourceId { get; set; }
        public string ResumeSourceName { get; set; }
        public int PositionAppliedId { get; set; }
        public string PositionAppliedName { get; set; }
        public decimal TotalExperience { get; set; }
        public decimal ReleventExperience { get; set; }
        public int NoticePeriod { get; set; }
        public int CurrentCTC { get; set; }
        public int ExpectedCTC { get; set; }
        public string PreferredLocation { get; set; }
        public string ResumeText { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByUserName { get; set; }
        public string CreatedDate { get; set; }
        public int Modifiedby { get; set; }
        public string ModifiedByUserName { get; set; }
        public string ModifiedDate { get; set; }
        public string ApplicantStatus { get; set; }
        public string ApplicantStortlistStatus { get; set; }
    }
    public class ApplicantEducationViewModel
    {
        public long ApplicantEducationId { get; set; }
        public long ApplicantId { get; set; }
        public int EducationSequenceNo { get; set; }
        public int EducationId { get; set; }
        public string EducationName { get; set; }
        public string RegularDistant { get; set; }
        public string BoardUniversityName { get; set; }
        public decimal PercentageObtained { get; set; }
    }
  
    public class ApplicantExtraActivityViewModel
    {
        public long ApplicantExtraId { get; set; }
        public long ApplicantId { get; set; }
        public string Strength1 { get; set; }
        public string Strength2 { get; set; }
        public string Strength3 { get; set; }
        public string Weakness1 { get; set; }
        public string Weakness2 { get; set; }
        public string Weakness3 { get; set; }
        public string Hobbies { get; set; }
    }
    public class ApplicantPrevEmployerViewModel
    {
        public long ApplicantPrevEmployerId { get; set; }
        public long ApplicantId { get; set; }
        public int EmployerSequenceNo { get; set; }
        public bool CurrentEmployer { get; set; }
        public string EmployerName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public decimal LastCTC { get; set; }
        public string ReasonOfLeaving { get; set; }
        public int LastDesignationId { get; set; }
        public string LastDesignationName { get; set; }
        public int EmploymentStatusId { get; set; }
        public string EmploymentStatusName { get; set; }
    }
    public class ApplicantProjectViewModel
    {
        public long ApplicantProjectId { get; set; }
        public int ApplicantId { get; set; }
        public int ProjectSequenceNo { get; set; }
        public string ProjectName { get; set; }
        public string ClientName { get; set; }
        public string RoleDesc { get; set; }
        public int TeamSize { get; set; }
        public string ProjectDesc { get; set; }
        public string TechnologiesUsed { get; set; }
    }
    public class ApplicantVerificationViewModel
    {
        public long ApplicantVerificationId { get; set; }
        public long ApplicantId { get; set; }
        public int VerificationAgencyId { get; set; }
        public string VerificationAgencyName { get; set; }
        public string VerificationDate { get; set; }
        public decimal VerificationCharges { get; set; }
        public string VerificationStatus { get; set; }
        public string Remarks { get; set; }
    }
}
