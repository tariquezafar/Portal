using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Core.ViewModel;
using Portal.DAL;
using Portal.Common;
using System.Reflection;
using System.Data;
using Portal.DAL.Infrastructure;
namespace Portal.Core
{
  public class InterviewBL
    {
        HRMSDBInterface dbInterface;
        public InterviewBL()
        {
            dbInterface = new HRMSDBInterface();
        }
        public ResponseOut AddEditInterview(InterviewViewModel interviewViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            HRSQLDBInterface hRSQLDBInterface = new HRSQLDBInterface();
            try
            {
                    HR_Interview interview = new HR_Interview
                    {
                           InterviewId = interviewViewModel.InterviewId,
                           ApplicantId = interviewViewModel.ApplicantId,
                           AptitudeTestStatus = interviewViewModel.AptitudeTestStatus,
                           AptitudeTestRemarks = interviewViewModel.AptitudeTestRemarks,
                           AptitudeTestTotalMarks= interviewViewModel.AptitudeTestTotalMarks,
                           AptitudeTestMarkObtained= interviewViewModel.AptitudeTestMarkObtained,
                           TechnicalRound1_Status= interviewViewModel.TechnicalRound1_Status,
                           TechnicalRound1_Remarks= interviewViewModel.TechnicalRound1_Remarks,
                           TechnicalRound1_TotalMarks= interviewViewModel.TechnicalRound1_TotalMarks,
                           TechnicalRound1_MarkObtained= interviewViewModel.TechnicalRound1_MarkObtained,
                           TechnicalRound2_Status = interviewViewModel.TechnicalRound2_Status,
                           TechnicalRound2_Remarks = interviewViewModel.TechnicalRound2_Remarks,
                           TechnicalRound2_TotalMarks = interviewViewModel.TechnicalRound2_TotalMarks,
                           TechnicalRound2_MarkObtained = interviewViewModel.TechnicalRound2_MarkObtained,
                           TechnicalRound3_Status = interviewViewModel.TechnicalRound3_Status,
                           TechnicalRound3_Remarks = interviewViewModel.TechnicalRound3_Remarks,
                           TechnicalRound3_TotalMarks = interviewViewModel.TechnicalRound3_TotalMarks,
                           TechnicalRound3_MarkObtained = interviewViewModel.TechnicalRound3_MarkObtained,
                           MachineRound_Status= interviewViewModel.MachineRound_Status,
                           MachineRound_Remarks= interviewViewModel.MachineRound_Remarks,
                           MachineRound_TotalMarks= interviewViewModel.MachineRound_TotalMarks,
                           MachineRound_MarkObtained= interviewViewModel.MachineRound_MarkObtained,
                           HRRound_Status = interviewViewModel.HRRound_Status,
                           HRRound_Remarks = interviewViewModel.HRRound_Remarks,
                           HRRound_TotalMarks= interviewViewModel.HRRound_TotalMarks,
                           HRRound_MarkObtained= interviewViewModel.HRRound_MarkObtained,
                           FinalRemarks = interviewViewModel.FinalRemarks,
                           InterviewFinalStatus = interviewViewModel.InterviewFinalStatus,
                           CompanyId = interviewViewModel.CompanyId,
                           CreatedBy = interviewViewModel.CreatedBy,
                           CompanyBranchId= interviewViewModel.CompanyBranchId



                    };
                responseOut = hRSQLDBInterface.AddEditInterview(interview);
            }

            catch (Exception ex)
            {
                responseOut.status = ActionStatus.Fail;
                responseOut.message = ActionMessage.ApplicationException;
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }

            return responseOut;
        }

        public InterviewViewModel GetInterviewDetail(long interviewId = 0)
        {
            InterviewViewModel interview = new InterviewViewModel();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
      
            try
            {
                DataTable dtInterview = sqlDbInterface.GetInterviewDetail(interviewId);
                if (dtInterview != null && dtInterview.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtInterview.Rows)
                    {
                        interview = new InterviewViewModel
                        {
                            InterviewId = Convert.ToInt32(dr["InterviewId"]),
                            ApplicantId = Convert.ToInt32(dr["ApplicantId"]),
                            ApplicantNo = string.IsNullOrEmpty(Convert.ToString(dr["ApplicantNo"]))?"": Convert.ToString(dr["ApplicantNo"]),
                            AptitudeTestStatus = Convert.ToString(dr["AptitudeTestStatus"]),
                            AptitudeTestRemarks = Convert.ToString(dr["AptitudeTestRemarks"]),
                            AptitudeTestTotalMarks = Convert.ToInt32(dr["AptitudeTestTotalMarks"]),
                            AptitudeTestMarkObtained= Convert.ToInt32(dr["AptitudeTestMarkObtained"]),


                            TechnicalRound1_Status = Convert.ToString(dr["TechnicalRound1_Status"]),
                            TechnicalRound1_Remarks = Convert.ToString(dr["TechnicalRound1_Remarks"]),
                            TechnicalRound1_TotalMarks = Convert.ToInt32(dr["TechnicalRound1_TotalMarks"]),
                            TechnicalRound1_MarkObtained = Convert.ToInt32(dr["TechnicalRound1_MarkObtained"]),

                            TechnicalRound2_Status = Convert.ToString(dr["TechnicalRound2_Status"]),
                            TechnicalRound2_Remarks = Convert.ToString(dr["TechnicalRound2_Remarks"]),
                            TechnicalRound2_TotalMarks = Convert.ToInt32(dr["TechnicalRound2_TotalMarks"]),
                            TechnicalRound2_MarkObtained = Convert.ToInt32(dr["TechnicalRound2_MarkObtained"]),

                            TechnicalRound3_Status = Convert.ToString(dr["TechnicalRound3_Status"]),
                            TechnicalRound3_Remarks = Convert.ToString(dr["TechnicalRound3_Remarks"]),
                            TechnicalRound3_TotalMarks = Convert.ToInt32(dr["TechnicalRound3_TotalMarks"]),
                            TechnicalRound3_MarkObtained = Convert.ToInt32(dr["TechnicalRound3_MarkObtained"]),

                            MachineRound_Status = Convert.ToString(dr["MachineRound_Status"]),
                            MachineRound_Remarks = Convert.ToString(dr["MachineRound_Remarks"]),
                            MachineRound_TotalMarks = Convert.ToInt32(dr["MachineRound_TotalMarks"]),
                            MachineRound_MarkObtained = Convert.ToInt32(dr["MachineRound_MarkObtained"]),

                            HRRound_Status = Convert.ToString(dr["HRRound_Status"]),
                            HRRound_Remarks = Convert.ToString(dr["HRRound_Remarks"]),
                            HRRound_TotalMarks = Convert.ToInt32(dr["HRRound_TotalMarks"]),
                            HRRound_MarkObtained = Convert.ToInt32(dr["HRRound_MarkObtained"]),
                            CompanyBranchId= Convert.ToInt32(dr["CompanyBranchId"]),


                            FinalRemarks = Convert.ToString(dr["FinalRemarks"]),
                            InterviewFinalStatus= Convert.ToString(dr["InterviewFinalStatus"]),
                            
                            CreatedByUserName = string.IsNullOrEmpty( Convert.ToString(dr["CreatedByName"]))?"" : Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = string.IsNullOrEmpty(Convert.ToString(dr["CreatedDate"]))?"": Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName =string.IsNullOrEmpty(Convert.ToString(dr["ModifiedByName"]))?"": Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = string.IsNullOrEmpty(Convert.ToString(dr["ModifiedDate"]))?"": Convert.ToString(dr["ModifiedDate"]),
                          
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return interview;
        }

        public List<InterviewViewModel> GetInterviewList(string interviewNo, string applicantNo, string interviewFinalStatus, int companyId, string fromDate, string toDate,string companyBranch)
        {
            List<InterviewViewModel> interviews = new List<InterviewViewModel>();
            HRSQLDBInterface hrsqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtInterviews = hrsqlDbInterface.GetInterviewList(interviewNo, applicantNo, interviewFinalStatus, companyId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyBranch);
                if (dtInterviews != null && dtInterviews.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtInterviews.Rows)
                    {
                        interviews.Add(new InterviewViewModel
                        {
                            InterviewId = Convert.ToInt32(dr["InterviewId"]),
                            InterviewNo = Convert.ToString(dr["InterviewNo"]),
                            ApplicantNo = Convert.ToString(dr["ApplicantNo"]),
                            AptitudeTestStatus = Convert.ToString(dr["AptitudeTestStatus"]),
                            TechnicalRound1_Status = Convert.ToString(dr["TechnicalRound1_Status"]),
                            TechnicalRound2_Status = Convert.ToString(dr["TechnicalRound2_Status"]),
                            TechnicalRound3_Status = Convert.ToString(dr["TechnicalRound3_Status"]),
                            MachineRound_Status = Convert.ToString(dr["MachineRound_Status"]),
                            HRRound_Status = Convert.ToString(dr["HRRound_Status"]),
                            InterviewFinalStatus = Convert.ToString(dr["InterviewFinalStatus"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            CompanyBranchName= Convert.ToString(dr["BranchName"])

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return interviews;
        }
    }
}
