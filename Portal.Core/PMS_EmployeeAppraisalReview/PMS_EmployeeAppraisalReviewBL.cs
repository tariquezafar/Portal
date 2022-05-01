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
using System.Transactions;
namespace Portal.Core
{
    public class PMS_EmployeeAppraisalReviewBL
    {
        HRMSDBInterface dbInterface;
        public PMS_EmployeeAppraisalReviewBL()
        {
            dbInterface = new HRMSDBInterface();
        }
        public ResponseOut AddEditEmployeeAppraisalReview(PMS_EmployeeAppraisalReviewViewModel employeeAppraisalReviewViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                HR_PMS_EmployeeAppraisalReview employeeAppraisalReview = new HR_PMS_EmployeeAppraisalReview
                {
                    PMSReviewId = employeeAppraisalReviewViewModel.PMSReviewId,
                    PerformanceCycleId = employeeAppraisalReviewViewModel.PerformanceCycleId,
                    FinYearId = employeeAppraisalReviewViewModel.FinYearId,
                    CompanyId = employeeAppraisalReviewViewModel.CompanyId,
                    EmployeeId = employeeAppraisalReviewViewModel.EmployeeId,
                    EmpAppraisalTemplateMappingId = employeeAppraisalReviewViewModel.EmpAppraisalTemplateMappingId,
                    PMSFormStatus = employeeAppraisalReviewViewModel.PMSFormStatus,
                    PMSFormSubmitDate =Convert.ToDateTime(string.IsNullOrEmpty(employeeAppraisalReviewViewModel.PMSFormSubmitDate)?"01-01-1900": employeeAppraisalReviewViewModel.PMSFormSubmitDate),
                    PMSReviewRemarks = employeeAppraisalReviewViewModel.PMSReviewRemarks,
                    PMSFinalStatus = employeeAppraisalReviewViewModel.PMSFinalStatus,
                    CreatedBy = employeeAppraisalReviewViewModel.CreatedBy,
                    CompanyBranchId= employeeAppraisalReviewViewModel.CompanyBranchId

                };
               
                responseOut = sqlDbInterface.AddEditEmployeeAppraisalReview(employeeAppraisalReview); 

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
        public PMS_EmployeeAppraisalReviewViewModel GetEmployeeAppraisalReviewDetail(long pmsReviewId = 0)
        {
            PMS_EmployeeAppraisalReviewViewModel empReview = new PMS_EmployeeAppraisalReviewViewModel();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtEmpAppraisalTemplate = sqlDbInterface.GetEmployeeAppraisalReviewDetail(pmsReviewId);
                if (dtEmpAppraisalTemplate != null && dtEmpAppraisalTemplate.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtEmpAppraisalTemplate.Rows)
                    {
                        empReview = new PMS_EmployeeAppraisalReviewViewModel
                        {
                            PMSReviewId = Convert.ToInt32(dr["PMSReviewId"]),
                            PerformanceCycleId = Convert.ToInt32(dr["PerformanceCycleId"]),
                            PerformanceCycleName = Convert.ToString(dr["PerformanceCycleName"]),
                            FinYearId = Convert.ToInt32(dr["FinYearId"]),
                            FinYearDesc = Convert.ToString(dr["FinYearCode"]),

                            EmployeeId = Convert.ToInt32(dr["EmployeeId"]),
                            EmployeeName = Convert.ToString(dr["EmployeeName"]),
                            EmpAppraisalTemplateMappingId = Convert.ToInt32(dr["EmpAppraisalTemplateMappingId"]),
                            PMSFormStatus = Convert.ToString(dr["PMSFormStatus"]),

                            PMSFormSubmitDate = Convert.ToString(dr["PMSFormSubmitDate"]),
                            PMSReviewRemarks= Convert.ToString(dr["PMSReviewRemarks"]),
                            PMSFinalStatus = Convert.ToString(dr["PMSFinalStatus"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            CompanyBranchId= Convert.ToInt32(dr["CompanyBranchId"]),
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return empReview;
        }
        public List<PMS_EmployeeAppraisalReviewViewModel> GetEmployeeAppraisalReviewList(string employeeName, string pmsFinalStatus,int companyBranchId, int companyId)
        {
            List<PMS_EmployeeAppraisalReviewViewModel> empReviews = new List<PMS_EmployeeAppraisalReviewViewModel>();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtQuotations = sqlDbInterface.GetEmployeeAppraisalReviewList(employeeName,pmsFinalStatus, companyBranchId, companyId);
                if (dtQuotations != null && dtQuotations.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtQuotations.Rows)
                    {
                        empReviews.Add(new PMS_EmployeeAppraisalReviewViewModel
                        {
                            PMSReviewId = Convert.ToInt32(dr["PMSReviewId"]),
                            PerformanceCycleId = Convert.ToInt32(dr["PerformanceCycleId"]),
                            PerformanceCycleName = Convert.ToString(dr["PerformanceCycleName"]),
                            FinYearId = Convert.ToInt32(dr["FinYearId"]),
                            FinYearDesc = Convert.ToString(dr["FinYearCode"]),

                            EmployeeId = Convert.ToInt32(dr["EmployeeId"]),
                            EmployeeName = Convert.ToString(dr["EmployeeName"]),
                            EmpAppraisalTemplateMappingId = Convert.ToInt32(dr["EmpAppraisalTemplateMappingId"]),
                            PMSFormStatus = Convert.ToString(dr["PMSFormStatus"]),

                            PMSFormSubmitDate = Convert.ToString(dr["PMSFormSubmitDate"]),
                            PMSFinalStatus = Convert.ToString(dr["PMSFinalStatus"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]), 
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            CompanyBranchName= Convert.ToString(dr["CompanyBranchName"]),

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return empReviews;
        }
        public DataTable GetPMS_EmployeeDetail(long empAppraisalTemplateMappingId = 0)
        {
            DataTable dtEmpAppraisalTemplate = new DataTable();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                 dtEmpAppraisalTemplate = sqlDbInterface.GetPMS_EmployeeDetail(empAppraisalTemplateMappingId);
                
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtEmpAppraisalTemplate;
        }
        public DataTable GetPMS_EmployeeAssessmentDetail(long empAppraisalTemplateMappingId = 0)
        {
            DataTable dtEmpAppraisalTemplate = new DataTable();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                dtEmpAppraisalTemplate = sqlDbInterface.GetPMS_EmployeeAssessmentDetail(empAppraisalTemplateMappingId);

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtEmpAppraisalTemplate;
        }
        public DataTable GetPMS_EmployeeAssessmentFooterDetail(long empAppraisalTemplateMappingId = 0)
        {
            DataTable dtEmpAppraisalTemplate = new DataTable();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                dtEmpAppraisalTemplate = sqlDbInterface.GetPMS_EmployeeAssessmentFooterDetail(empAppraisalTemplateMappingId);

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtEmpAppraisalTemplate;
        }

    }
}
