using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{

    public class PMS_EmployeeAppraisalTemplateMappingViewModel
    {
        public long EmpAppraisalTemplateMappingId { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public long DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public long DesignationId { get; set; }
        public string DesignationName { get; set; }
        public long TemplateId { get; set; }
        public string TemplateName { get; set; }
        public int PerformanceCycleId { get; set; }
        public string PerformanceCycleName { get; set; }
        public int FinYearId { get; set; }
        public string FinYearDesc { get; set; }
        public int CompanyId { get; set; }

        public bool EmpAppraisalTemplateMapping_Status { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedByUserName { get; set; }
        public string ModifiedByUserName { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public string CompanyBranchName { get; set; }
        public int CompanyBranchId { get; set; }
    }
    public class PMS_EmployeeGoalsViewModel
    {
        public int EmployeeGoal_SequenceNo { get; set; }
        public long EmployeeGoalId { get; set; }
        public long EmpAppraisalTemplateMappingId { get; set; }
        public Int32 GoalId { get; set; }
        public string GoalName { get; set; }
        public string GoalDescription { get; set; }
        public int SectionId { get; set; }
        public string SectionName { get; set; }
        public int GoalCategoryId { get; set; }
        public string GoalCategoryName { get; set; }
        public string EvalutionCriteria { get; set; }
        public string StartDate { get; set; }
        public string DueDate { get; set; }
        public decimal Weight { get; set; }
        public decimal SelfScore { get; set; }
        public string SelfRemarks { get; set; }
        public decimal AppraiserScore { get; set; }
        public string AppraiserRemarks { get; set; }
        public decimal ReviewScore { get; set; }
        public string ReviewRemarks { get; set; }
        public bool EmployeeGoal_Status { get; set; }
        public string FixedDyanmic { get; set; } //F=Fixed=Created by HR, D=Dyamic=Created by Employee self
    }
    public class PMS_EmployeeAppraisalReviewViewModel
    {
        public long PMSReviewId { get; set; }
        public int PerformanceCycleId { get; set; }
        public string PerformanceCycleName { get; set; }

        public int FinYearId { get; set; }
        public string FinYearDesc { get; set; }
        public int CompanyId { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; }

        public long EmpAppraisalTemplateMappingId { get; set; }



        public string PMSFormStatus { get; set; }
        public string PMSFormSubmitDate { get; set; }
        public string PMSReviewStatus { get; set; }
        public string PMSReviewDate { get; set; }
        public string PMSReviewRemarks { get; set; }
        public string PMSFinalStatus { get; set; }
        public int PMSReviewBy { get; set; }
        

        
        
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedByUserName { get; set; }
        public string ModifiedByUserName { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
        public string message { get; set; }
        public string status { get; set; }

        public int CompanyBranchId { get; set; }
        public string CompanyBranchName { get; set; }
    }


}
