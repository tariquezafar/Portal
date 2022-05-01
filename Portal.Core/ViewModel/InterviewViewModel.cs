using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
 public class InterviewViewModel
    {
        public long InterviewId { get; set; }
        public string InterviewNo { get; set; }
        public long ApplicantId { get; set; }
        
        public string ApplicantNo { get; set; }
        public string ApplicantName { get; set; }

        public string AptitudeTestStatus { get; set; }
        public string AptitudeTestRemarks { get; set; }
        public int AptitudeTestTotalMarks { get; set; }
        public int AptitudeTestMarkObtained { get; set; }
        public string TechnicalRound1_Status { get; set; }
        public string TechnicalRound1_Remarks { get; set; }
        public int TechnicalRound1_TotalMarks { get; set; }
        public int TechnicalRound1_MarkObtained { get; set; }
        public string TechnicalRound2_Status { get; set; }
        public string TechnicalRound2_Remarks { get; set; }
        public int TechnicalRound2_TotalMarks { get; set; }
        public int TechnicalRound2_MarkObtained { get; set; }
        public string TechnicalRound3_Status { get; set; }
        public string TechnicalRound3_Remarks { get; set; }
        public int TechnicalRound3_TotalMarks { get; set; }
        public int TechnicalRound3_MarkObtained { get; set; }
        public string MachineRound_Status { get; set; }
        public string MachineRound_Remarks { get; set; }
        public int MachineRound_TotalMarks { get; set; }
        public int MachineRound_MarkObtained { get; set; }
        public string HRRound_Status { get; set; }
        public string HRRound_Remarks { get; set; }
        public int HRRound_TotalMarks { get; set; }
        public int HRRound_MarkObtained { get; set; }
        public string FinalRemarks { get; set; }
        public string InterviewFinalStatus { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByUserName { get; set; }
        public string CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedByUserName { get; set; }
        public string ModifiedDate { get; set; }
        
        public int CompanyId { get; set; }
        public int  CompanyBranchId { get; set; }
        public string CompanyBranchName { get; set; }
    }
}
