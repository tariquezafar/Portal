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
    public class ExitInterviewBL
    {
        HRMSDBInterface dbInterface;
        public ExitInterviewBL()
        {
            dbInterface = new HRMSDBInterface();
        }
       
        public ResponseOut AddEditExitInterview(ExitInterviewViewModel exitinterviewViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                HR_ExitInterview exitInterview = new HR_ExitInterview
                {
                    ExitInterviewId = exitinterviewViewModel.ExitInterviewId, 
                    ExitInterviewDate =Convert.ToDateTime(exitinterviewViewModel.ExitInterviewDate),
                    EmployeeId = exitinterviewViewModel.EmployeeId,
                    ApplicationId = exitinterviewViewModel.ApplicationId,
                    InterviewDescription = exitinterviewViewModel.InterviewDescription,
                    InterviewRemarks = exitinterviewViewModel.InterviewRemarks,
                    InterviewByUserId = exitinterviewViewModel.InterviewByUserId, 
                    CompanyId = exitinterviewViewModel.CompanyId,
                    InterviewStatus = exitinterviewViewModel.InterviewStatus,
                    CreatedBy = exitinterviewViewModel.CreatedBy

                };
                responseOut = sqlDbInterface.AddEditExitInterview(exitInterview);
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
        

        public List<ExitInterviewViewModel> GetExitInterviewList(string exitinterviewNo, int employeeId, int applicationId, string interviewStatus,int interviewbyuserId, string fromDate, string toDate, int companyId)
        {
            List<ExitInterviewViewModel> exitInterviews = new List<ExitInterviewViewModel>();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtExitInterviews = sqlDbInterface.GetExitInterviewList(exitinterviewNo, employeeId, applicationId, interviewStatus, interviewbyuserId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId);
                if (dtExitInterviews != null && dtExitInterviews.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtExitInterviews.Rows)
                    {
                        exitInterviews.Add(new ExitInterviewViewModel
                        {
                            ExitInterviewId = Convert.ToInt32(dr["ExitInterviewId"]),
                            ExitInterviewDate = Convert.ToString(dr["ExitInterviewDate"]),
                            ExitInterviewNo = Convert.ToString(dr["ExitInterviewNo"]),
                            ApplicationId = Convert.ToInt16(dr["ApplicationId"]),
                            ApplicationNo = Convert.ToString(dr["ApplicationNo"]),
                            EmployeeId = Convert.ToInt16(dr["EmployeeId"]), 
                            InterviewByUserId = Convert.ToInt32(dr["InterviewByUserId"]),
                            InterviewByUserName = Convert.ToString(dr["InterviewByUserName"]),
                            EmployeeName = Convert.ToString(dr["EmployeeName"]),
                            InterviewDescription = Convert.ToString(dr["InterviewDescription"]),
                            InterviewRemarks = Convert.ToString(dr["InterviewRemarks"]),
                            InterviewStatus = Convert.ToString(dr["InterviewStatus"]),
                            CreatedByName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return exitInterviews;
        }



        public ExitInterviewViewModel GetExitInterviewDetail(long exitinterviewId = 0)
        {
            ExitInterviewViewModel exitInterviews = new ExitInterviewViewModel();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtExitInterviews = sqlDbInterface.GetExitInterviewDetail(exitinterviewId);
                if (dtExitInterviews != null && dtExitInterviews.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtExitInterviews.Rows)
                    {
                        exitInterviews = new ExitInterviewViewModel
                        {
                            ExitInterviewId = Convert.ToInt32(dr["ExitInterviewId"]),
                            ExitInterviewDate = Convert.ToString(dr["ExitInterviewDate"]), 
                            ExitInterviewNo = Convert.ToString(dr["ExitInterviewNo"]),
                            ApplicationId = Convert.ToInt16(dr["ApplicationId"]),
                            ApplicationNo = Convert.ToString(dr["ApplicationNo"]),
                            EmployeeId = Convert.ToInt32(dr["EmployeeId"]),
                            EmployeeName = Convert.ToString(dr["EmployeeName"]),
                            InterviewByUserId = Convert.ToInt32(dr["InterviewByUserId"]),
                            InterviewByUserName = Convert.ToString(dr["InterviewByUserName"]),
                            InterviewDescription = Convert.ToString(dr["InterviewDescription"]),
                            InterviewRemarks = Convert.ToString(dr["InterviewRemarks"]),
                            InterviewStatus = Convert.ToString(dr["InterviewStatus"]),
                            CreatedByName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                          
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return exitInterviews;
        }

         
        public List<ExitInterviewViewModel> GetExitInterviewForSeparationOrderList()
        {
            List<ExitInterviewViewModel> exitinterviews = new List<ExitInterviewViewModel>();
            try
            {
                List<HR_ExitInterview> exitinterviewList = dbInterface.GetExitInterviewForSeparationOrderList();
                if (exitinterviewList != null && exitinterviewList.Count > 0)
                {
                    foreach (HR_ExitInterview advance in exitinterviewList)
                    {
                        exitinterviews.Add(new ExitInterviewViewModel { ExitInterviewId = advance.ExitInterviewId, ExitInterviewNo = advance.ExitInterviewNo });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return exitinterviews;
        }
        

         

    }
}
