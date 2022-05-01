﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class HRMSEntities : DbContext
    {
        public HRMSEntities()
            : base("name=HRMSEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<HR_ActivityCalender> HR_ActivityCalender { get; set; }
        public virtual DbSet<HR_AdvanceType> HR_AdvanceType { get; set; }
        public virtual DbSet<HR_Applicant> HR_Applicant { get; set; }
        public virtual DbSet<HR_ApplicantEducation> HR_ApplicantEducation { get; set; }
        public virtual DbSet<HR_ApplicantExtraActivity> HR_ApplicantExtraActivity { get; set; }
        public virtual DbSet<HR_ApplicantPrevEmployer> HR_ApplicantPrevEmployer { get; set; }
        public virtual DbSet<HR_ApplicantProject> HR_ApplicantProject { get; set; }
        public virtual DbSet<HR_ApplicantVerification> HR_ApplicantVerification { get; set; }
        public virtual DbSet<HR_Appointment> HR_Appointment { get; set; }
        public virtual DbSet<HR_AppointmentCTC> HR_AppointmentCTC { get; set; }
        public virtual DbSet<HR_AssetType> HR_AssetType { get; set; }
        public virtual DbSet<HR_Calender> HR_Calender { get; set; }
        public virtual DbSet<HR_ClaimType> HR_ClaimType { get; set; }
        public virtual DbSet<HR_ClearanceTemplate> HR_ClearanceTemplate { get; set; }
        public virtual DbSet<HR_ClearanceTemplateDetail> HR_ClearanceTemplateDetail { get; set; }
        public virtual DbSet<HR_CTC> HR_CTC { get; set; }
        public virtual DbSet<HR_Education> HR_Education { get; set; }
        public virtual DbSet<HR_EmployeeAdvanceApplication> HR_EmployeeAdvanceApplication { get; set; }
        public virtual DbSet<HR_EmployeeAssetApplication> HR_EmployeeAssetApplication { get; set; }
        public virtual DbSet<HR_EmployeeAttendance> HR_EmployeeAttendance { get; set; }
        public virtual DbSet<HR_EmployeeClearanceProcess> HR_EmployeeClearanceProcess { get; set; }
        public virtual DbSet<HR_EmployeeClearanceProcessDetail> HR_EmployeeClearanceProcessDetail { get; set; }
        public virtual DbSet<HR_EmployeeLeaveApplication> HR_EmployeeLeaveApplication { get; set; }
        public virtual DbSet<HR_EmployeeLeaveDetail> HR_EmployeeLeaveDetail { get; set; }
        public virtual DbSet<HR_EmployeeLoanApplication> HR_EmployeeLoanApplication { get; set; }
        public virtual DbSet<HR_EmployeeTravelApplication> HR_EmployeeTravelApplication { get; set; }
        public virtual DbSet<HR_ExitInterview> HR_ExitInterview { get; set; }
        public virtual DbSet<HR_HolidayCalender> HR_HolidayCalender { get; set; }
        public virtual DbSet<HR_HolidayType> HR_HolidayType { get; set; }
        public virtual DbSet<HR_Interview> HR_Interview { get; set; }
        public virtual DbSet<HR_InterviewType> HR_InterviewType { get; set; }
        public virtual DbSet<HR_JobOpening> HR_JobOpening { get; set; }
        public virtual DbSet<HR_EmployeeClaimApplication> HR_EmployeeClaimApplication { get; set; }
        public virtual DbSet<HR_Language> HR_Language { get; set; }
        public virtual DbSet<HR_LeaveType> HR_LeaveType { get; set; }
        public virtual DbSet<HR_LoanType> HR_LoanType { get; set; }
        public virtual DbSet<HR_PMS_AppraisalTemplate> HR_PMS_AppraisalTemplate { get; set; }
        public virtual DbSet<HR_PMS_AppraisalTemplateGoal> HR_PMS_AppraisalTemplateGoal { get; set; }
        public virtual DbSet<HR_PMS_EmployeeAppraisalReview> HR_PMS_EmployeeAppraisalReview { get; set; }
        public virtual DbSet<HR_PMS_EmployeeAppraisalTemplateMapping> HR_PMS_EmployeeAppraisalTemplateMapping { get; set; }
        public virtual DbSet<HR_PMS_Goal> HR_PMS_Goal { get; set; }
        public virtual DbSet<HR_PMS_GoalCategory> HR_PMS_GoalCategory { get; set; }
        public virtual DbSet<HR_PMS_PerformanceCycle> HR_PMS_PerformanceCycle { get; set; }
        public virtual DbSet<HR_PMS_Section> HR_PMS_Section { get; set; }
        public virtual DbSet<HR_PositionLevel> HR_PositionLevel { get; set; }
        public virtual DbSet<HR_PositionType> HR_PositionType { get; set; }
        public virtual DbSet<HR_Religion> HR_Religion { get; set; }
        public virtual DbSet<HR_ResourceRequisition> HR_ResourceRequisition { get; set; }
        public virtual DbSet<HR_ResourceRequisitionInterviewStage> HR_ResourceRequisitionInterviewStage { get; set; }
        public virtual DbSet<HR_ResourceRequisitionSkill> HR_ResourceRequisitionSkill { get; set; }
        public virtual DbSet<HR_ResumeSource> HR_ResumeSource { get; set; }
        public virtual DbSet<HR_Roaster> HR_Roaster { get; set; }
        public virtual DbSet<HR_RoasterWeek> HR_RoasterWeek { get; set; }
        public virtual DbSet<HR_SeparationApplication> HR_SeparationApplication { get; set; }
        public virtual DbSet<HR_SeparationCategory> HR_SeparationCategory { get; set; }
        public virtual DbSet<HR_SeparationClearList> HR_SeparationClearList { get; set; }
        public virtual DbSet<HR_SeparationOrder> HR_SeparationOrder { get; set; }
        public virtual DbSet<HR_Shift> HR_Shift { get; set; }
        public virtual DbSet<HR_ShiftType> HR_ShiftType { get; set; }
        public virtual DbSet<HR_Skills> HR_Skills { get; set; }
        public virtual DbSet<HR_TravelType> HR_TravelType { get; set; }
        public virtual DbSet<HR_VerificationAgency> HR_VerificationAgency { get; set; }
        public virtual DbSet<HR_EmployeeRoster> HR_EmployeeRoster { get; set; }
        public virtual DbSet<PR_PayrollMonth> PR_PayrollMonth { get; set; }
        public virtual DbSet<PR_PayrollProcessPeriod> PR_PayrollProcessPeriod { get; set; }
        public virtual DbSet<PR_PayrollTransaction> PR_PayrollTransaction { get; set; }
        public virtual DbSet<HR_PMS_EmployeeGoals> HR_PMS_EmployeeGoals { get; set; }
        public virtual DbSet<PR_PayrollMonthlyAdjustment> PR_PayrollMonthlyAdjustment { get; set; }
        public virtual DbSet<PR_PayrollOtherEarningDeduction> PR_PayrollOtherEarningDeduction { get; set; }
        public virtual DbSet<PR_PayHeadGLMapping> PR_PayHeadGLMapping { get; set; }
        public virtual DbSet<PR_PayrollTdsSlab> PR_PayrollTdsSlab { get; set; }
        public virtual DbSet<PR_PayrollTransactionFullYear> PR_PayrollTransactionFullYear { get; set; }
    }
}