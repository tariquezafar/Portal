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
using System.Transactions;
using System.Web.Mvc;
using Portal.DAL.Infrastructure;
namespace Portal.Core
{
    public class JobOpeningBL
    {
        HRMSDBInterface dbInterface;
        public JobOpeningBL()
        {
            dbInterface = new HRMSDBInterface();
        }

        public ResponseOut AddEditJobOpening(JobOpeningViewModel jobOpeningViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                HR_JobOpening jobOpening = new HR_JobOpening {
                    JobOpeningId=jobOpeningViewModel.JobOpeningId,
                    JobOpeningNo= jobOpeningViewModel.JobOpeningNo,
                    JobOpeningDate = Convert.ToDateTime(jobOpeningViewModel.JobOpeningDate),
                    CompanyId = jobOpeningViewModel.CompanyId,
                    JobTitle = Convert.ToString(jobOpeningViewModel.JobTitle),
                    JobPortalRefNo = Convert.ToString(jobOpeningViewModel.JobPortalRefNo),
                    NoOfOpening = Convert.ToInt32(jobOpeningViewModel.NoOfOpening),
                    MinExp = jobOpeningViewModel.MinExp,
                    MaxExp = jobOpeningViewModel.MaxExp,
                    MinSalary = jobOpeningViewModel.MinSalary,
                    MaxSalary = jobOpeningViewModel.MaxSalary,
                    KeySkills = jobOpeningViewModel.KeySkills,
                    JobDescription = jobOpeningViewModel.JobDescription,
                    JobStartDate=Convert.ToDateTime(jobOpeningViewModel.JobStartDate),
                    JobExpiryDate =Convert.ToDateTime(jobOpeningViewModel.JobExpiryDate),
                    CreatedBy = jobOpeningViewModel.CreatedBy,
                    JobStatus = jobOpeningViewModel.JobStatus,
                    RequisitionId = jobOpeningViewModel.RequisitionId,
                    EducationId = jobOpeningViewModel.EducationId,
                    OtherQualification = jobOpeningViewModel.OtherQualification,
                    CurrencyCode = jobOpeningViewModel.CurrencyCode,
                    Remarks = jobOpeningViewModel.Remarks,
                    CompanyBranchId=jobOpeningViewModel.CompanyBranchId
                    
                };
              
                responseOut = sqlDbInterface.AddEditHRJobOpening(jobOpening);
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

       


        public List<JobOpeningViewModel> GetJobOpeningList(string jobOpeningNo = "", string requisitionNo = "", string jobPortalRefNo = "", string jobTitle="", string fromDate = "", string toDate = "", int companyId=0,string jobStatus="Final",string companyBranch="")
        {
            List<JobOpeningViewModel> jobOpenings = new List<JobOpeningViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtJobOpenings = sqlDbInterface.GetJobOpeningList(jobOpeningNo,requisitionNo, jobPortalRefNo, jobTitle, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId,jobStatus, companyBranch);
                if (dtJobOpenings != null && dtJobOpenings.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtJobOpenings.Rows)
                    {
                        jobOpenings.Add(new JobOpeningViewModel
                        {
                            JobOpeningId = Convert.ToInt32(dr["JobOpeningId"]),
                            JobOpeningNo = Convert.ToString(dr["JobOpeningNo"]),
                            RequisitionNo = Convert.ToString(dr["RequisitionNo"]),
                            JobTitle = Convert.ToString(dr["JobTitle"]),
                            JobOpeningDate = Convert.ToString(dr["JobOpeningDate"]),
                            JobPortalRefNo = Convert.ToString(dr["JobPortalRefNo"]),
                            NoOfOpening= Convert.ToInt32(dr["NoOfOpening"]),
                            MinExp = Convert.ToInt32(dr["MinExp"]),
                            MaxExp=Convert.ToInt32(dr["MaxExp"]),
                            MinSalary = Convert.ToInt32(dr["MinSalary"]),
                            MaxSalary = Convert.ToInt32(dr["MaxSalary"]),
                            JobStartDate = Convert.ToString(dr["JobStartDate"]),
                            JobExpiryDate = Convert.ToString(dr["JobExpiryDate"]),
                            JobStatus = Convert.ToString(dr["JobStatus"]),
                            KeySkills = Convert.ToString(dr["KeySkills"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            CompanyBranchName = Convert.ToString(dr["BranchName"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return jobOpenings;
        }

        public JobOpeningViewModel GetJobOpeningDetail(long jobOpeningId = 0)
        {
            
            JobOpeningViewModel jobOpening = new JobOpeningViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtJobOpening= sqlDbInterface.GetJobOpeningDetail(jobOpeningId);
                if (dtJobOpening != null && dtJobOpening.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtJobOpening.Rows)
                    {
                        jobOpening = new JobOpeningViewModel
                        {
                            RequisitionId = Convert.ToInt32(dr["RequisitionId"]),
                            RequisitionNo = Convert.ToString(dr["RequisitionNo"]),
                            JobOpeningId = Convert.ToInt64(dr["JobOpeningId"]),
                            JobOpeningNo= Convert.ToString(dr["JobOpeningNo"]),
                            JobOpeningDate = Convert.ToString(dr["JobOpeningDate"]),
                            JobTitle = Convert.ToString(dr["JobTitle"]),
                            JobPortalRefNo = string.IsNullOrEmpty(Convert.ToString(dr["JobPortalRefNo"])) ? "" : Convert.ToString(dr["JobPortalRefNo"]),
                            NoOfOpening = Convert.ToInt32(dr["NoOfOpening"]),
                            MinExp = Convert.ToInt32(dr["MinExp"]),
                            MaxExp = Convert.ToInt32(dr["MaxExp"]),
                            MinSalary = Convert.ToDecimal(dr["MinSalary"]),
                            MaxSalary = Convert.ToDecimal(dr["MaxSalary"]),
                            KeySkills = string.IsNullOrEmpty(Convert.ToString(dr["KeySkills"])) ? "" : Convert.ToString(dr["KeySkills"]),
                            JobDescription = string.IsNullOrEmpty(Convert.ToString(dr["JobDescription"])) ? "" : Convert.ToString(dr["JobDescription"]),
                            JobStartDate = string.IsNullOrEmpty(Convert.ToString(dr["JobStartDate"])) ? "" : Convert.ToString(dr["JobStartDate"]),
                            JobExpiryDate = string.IsNullOrEmpty(Convert.ToString(dr["JobExpiryDate"])) ? "" : Convert.ToString(dr["JobExpiryDate"]),
                            EducationId = Convert.ToInt32(dr["EducationId"]),
                            OtherQualification = string.IsNullOrEmpty(Convert.ToString(dr["OtherQualification"])) ? "" : Convert.ToString(dr["OtherQualification"]),
                            CurrencyCode = string.IsNullOrEmpty(Convert.ToString(dr["CurrencyCode"])) ? "" : Convert.ToString(dr["CurrencyCode"]),
                            Remarks = string.IsNullOrEmpty(Convert.ToString(dr["Remarks"])) ? "" : Convert.ToString(dr["Remarks"]),
                            CreatedByUserName = string.IsNullOrEmpty(Convert.ToString(dr["CreatedByName"])) ? "" : Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = string.IsNullOrEmpty(Convert.ToString(dr["CreatedDate"])) ? "" : Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = string.IsNullOrEmpty(Convert.ToString(dr["ModifiedByName"])) ? "" : Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = string.IsNullOrEmpty(Convert.ToString(dr["ModifiedDate"])) ? "" : Convert.ToString(dr["ModifiedDate"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return jobOpening;
        }
    }
}
