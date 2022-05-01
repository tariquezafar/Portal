//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Portal.DAL.Infrastructure
{
    using System;
    using System.Collections.Generic;
    
    public partial class HR_Applicant
    {
        public long ApplicantId { get; set; }
        public string ApplicantNo { get; set; }
        public Nullable<System.DateTime> ApplicationDate { get; set; }
        public Nullable<long> JobOpeningId { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> CompanyBranchId { get; set; }
        public string ProjectNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string FatherSpouseName { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string BloodGroup { get; set; }
        public string MaritalStatus { get; set; }
        public string ApplicantAddress { get; set; }
        public string City { get; set; }
        public Nullable<int> StateId { get; set; }
        public Nullable<int> CountryId { get; set; }
        public string PinCode { get; set; }
        public string ContactNo { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public Nullable<int> ResumeSourceId { get; set; }
        public Nullable<int> PositionAppliedId { get; set; }
        public Nullable<decimal> TotalExperience { get; set; }
        public Nullable<decimal> ReleventExperience { get; set; }
        public Nullable<int> NoticePeriod { get; set; }
        public Nullable<int> CurrentCTC { get; set; }
        public Nullable<int> ExpectedCTC { get; set; }
        public string PreferredLocation { get; set; }
        public string ResumeText { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ApplicantStatus { get; set; }
        public string ApplicantStortlistStatus { get; set; }
    }
}