using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
   public class ResourceRequisitionViewModel
    { 
        public int RequisitionId { get; set; }
        public string RequisitionNo { get; set; }
        public int NumberOfResources { get; set; }
        public int PositionLevelId { get; set; }
        public string PositionLevelName { get; set; }
        public string PriorityLevel { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int DesignationId { get; set; }
        public string DesignationName { get; set; }

        public int EducationId { get; set; }
        public string EducationName { get; set; }
        public int PositionTypeId { get; set; }
        public string PositionTypeName { get; set; }
        public int ContractPeriod { get; set; }
        public string JobDescription { get; set; }
        public string OtherQualification { get; set; }
        public int MinExp { get; set; }
        public int MaxExp { get; set; }
        public decimal MinSalary { get; set; }
        public decimal MaxSalary { get; set; }
        public string CurrencyCode { get; set; }
        public string Remarks { get; set; }
        public string JustificationNotes { get; set; }
        public string InterviewStartDate { get; set; }
        public string HireByDate { get; set; }
        public int CompanyId { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByUserName { get; set; }
        public string CreatedDate { get; set; }
        public int Modifiedby { get; set; }
        public string ModifiedByUserName { get; set; }
        public string ModifiedDate { get; set; }
        public string RequisitionStatus { get; set; }
         public string ApprovalStatus { get; set; }
        public int ApprovedBy { get; set; }
        public string ApprovedByUserName { get; set; }
        public string ApprovedDate { get; set; }
        public string RejectionStatus { get; set; }
        public int RejectedBy { get; set; }
        public string RejectedByUserName { get; set; }
        public string RejectedDate { get; set; }
        public string RejectedReason { get; set; } 
        public string message { get; set; }
        public string status { get; set; }
        public int CompanyBranchId { get; set; }
        public string CompanyBranchName { get; set; }
    } 
 
    public class ResourceRequisitionSkillViewModel
    {
        public int RequisitionSkillId { get; set; }
        public int RequisitionId { get; set; }
        public int SkillSequenceNo { get; set; }
        public int SkillId { get; set; }
        public string SkillName { get; set; }
        public string SkillCode { get; set; }
        public string message { get; set; }
        public string status { get; set; }

    }

    public class PositionLevelViewModel
    {
        public int PositionLevelId { get; set; }
        public string PositionLevelName { get; set; }
        public string PositionLevelCode { get; set; } 
        public bool PositionLevel_Status { get; set; }
        public string message { get; set; }
        public string status { get; set; }

    }
    public class PositionTypeViewModel
    {
        public int PositionTypeId { get; set; }
        public string PositionTypeName { get; set; }
        public string PositionTypeCode { get; set; }
        public bool PositionType_Status { get; set; }
        public string message { get; set; }
        public string status { get; set; }

    }



    public class SkillViewModel
    {
        public int SkillId { get; set; }
        public string SkillName { get; set; }
        public string SkillCode { get; set; } 
        public bool Skill_Status { get; set; } 
        public string message { get; set; }
        public string status { get; set; }

    }


}
