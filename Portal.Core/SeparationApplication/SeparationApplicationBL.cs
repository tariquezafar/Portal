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
    public class SeparationApplicationBL
    {
        HRMSDBInterface dbInterface;
        public SeparationApplicationBL()
        {
            dbInterface = new HRMSDBInterface();
        }
       
        public ResponseOut AddEditSeparationApplication(SeparationApplicationViewModel separationapplicationViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                HR_SeparationApplication separationapplication = new HR_SeparationApplication
                {
                    ApplicationId = separationapplicationViewModel.ApplicationId, 
                    ApplicationDate =Convert.ToDateTime(separationapplicationViewModel.ApplicationDate),
                    EmployeeId = separationapplicationViewModel.EmployeeId,
                    SeparationCategoryId = separationapplicationViewModel.SeparationCategoryId,
                    Reason = separationapplicationViewModel.Reason,
                    Remarks = separationapplicationViewModel.Remarks, 
                    CompanyId = separationapplicationViewModel.CompanyId,
                    ApplicationStatus = separationapplicationViewModel.ApplicationStatus,
                    CreatedBy = separationapplicationViewModel.CreatedBy,
                    CompanyBranchId=separationapplicationViewModel.CompanyBranchId

                };
                responseOut = sqlDbInterface.AddEditSeparationApplication(separationapplication);
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


        public List<SeparationApplicationViewModel> GetSeparationApplicationList(string applicationNo, int employeeId , string separationcategoryId,  string applicationStatus, string fromDate, string toDate, int companyId,int companyBranchId)
        {
            List<SeparationApplicationViewModel> separationApplications = new List<SeparationApplicationViewModel>();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtSeparationApplications = sqlDbInterface.GetSeparationApplicationList(applicationNo, employeeId, separationcategoryId, applicationStatus, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, companyBranchId);
                if (dtSeparationApplications != null && dtSeparationApplications.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtSeparationApplications.Rows)
                    {
                        separationApplications.Add(new SeparationApplicationViewModel
                        {
                            ApplicationId = Convert.ToInt32(dr["ApplicationId"]),
                            ApplicationDate = Convert.ToString(dr["ApplicationDate"]),
                            ApplicationNo = Convert.ToString(dr["ApplicationNo"]), 
                            SeparationCategoryId = Convert.ToInt16(dr["SeparationCategoryId"]),
                            SeparationCategoryName = Convert.ToString(dr["SeparationCategoryName"]),
                            EmployeeId = Convert.ToInt32(dr["EmployeeId"]),
                            EmployeeName = Convert.ToString(dr["EmployeeName"]),   
                            Remarks = Convert.ToString(dr["Remarks"]),
                            Reason = Convert.ToString(dr["Reason"]),
                            ApplicationStatus = Convert.ToString(dr["ApplicationStatus"]),
                            CreatedByName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            CompanyBranchName = Convert.ToString(dr["CompanyBranchName"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return separationApplications;
        }



        public SeparationApplicationViewModel GetSeparationApplicationDetail(long applicationId = 0)
        {
            SeparationApplicationViewModel separationApplications = new SeparationApplicationViewModel();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtEmployeeAdvanceApplication = sqlDbInterface.GetSeparationApplicationDetail(applicationId);
                if (dtEmployeeAdvanceApplication != null && dtEmployeeAdvanceApplication.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtEmployeeAdvanceApplication.Rows)
                    {
                        separationApplications = new SeparationApplicationViewModel
                        {
                            ApplicationId = Convert.ToInt32(dr["ApplicationId"]),
                            ApplicationNo = Convert.ToString(dr["ApplicationNo"]),
                            SeparationCategoryId = Convert.ToInt16(dr["SeparationCategoryId"]),
                            EmployeeId = Convert.ToInt32(dr["EmployeeId"]),
                            EmployeeName = Convert.ToString(dr["EmployeeName"]),  
                            ApplicationDate = Convert.ToString(dr["ApplicationDate"]),
                            Reason = Convert.ToString(dr["Reason"]),
                            Remarks = Convert.ToString(dr["Remarks"]),
                            ApplicationStatus = Convert.ToString(dr["ApplicationStatus"]),
                            CreatedByName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]), 
                            RejectReason = Convert.ToString(dr["RejectReason"]),
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
            return separationApplications;
        }



        public List<SeparationApplicationViewModel> GetSeparationApplicationApprovalList(string applicationNo, int employeeId, string separationcategoryId, string applicationStatus, string fromDate, string toDate, int companyId,int companyBranchId)
        {
            List<SeparationApplicationViewModel> separationApplications = new List<SeparationApplicationViewModel>();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtSeparationApplications = sqlDbInterface.GetSeparationApplicationApprovalList(applicationNo, employeeId, separationcategoryId, applicationStatus, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, companyBranchId);
                if (dtSeparationApplications != null && dtSeparationApplications.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtSeparationApplications.Rows)
                    {
                        separationApplications.Add(new SeparationApplicationViewModel
                        {
                            ApplicationId = Convert.ToInt32(dr["ApplicationId"]),
                            ApplicationDate = Convert.ToString(dr["ApplicationDate"]),
                            ApplicationNo = Convert.ToString(dr["ApplicationNo"]),
                            SeparationCategoryId = Convert.ToInt16(dr["SeparationCategoryId"]),
                            SeparationCategoryName = Convert.ToString(dr["SeparationCategoryName"]),
                            EmployeeId = Convert.ToInt32(dr["EmployeeId"]),
                            EmployeeName = Convert.ToString(dr["EmployeeName"]),
                            Remarks = Convert.ToString(dr["Remarks"]),
                            Reason = Convert.ToString(dr["Reason"]),
                            ApplicationStatus = Convert.ToString(dr["ApplicationStatus"]),
                            ApprovedByName = Convert.ToString(dr["ApprovedByName"]),
                            ApprovedDate = Convert.ToString(dr["ApproveDate"]),
                            RejectDate = Convert.ToString(dr["RejectDate"]),
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
            return separationApplications;
        }

        public ResponseOut ApproveRejectSeparationApplication(SeparationApplicationViewModel separationapplicationViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            HRMSDBInterface dbInterface = new HRMSDBInterface();
            try
            {
                HR_SeparationApplication separationApplication = new HR_SeparationApplication
                {
                    ApplicationId = separationapplicationViewModel.ApplicationId,
                    CreatedBy = separationapplicationViewModel.CreatedBy,
                    RejectReason = separationapplicationViewModel.RejectReason,
                    ApproveBy = separationapplicationViewModel.ApprovedBy,
                    ApplicationStatus = separationapplicationViewModel.ApplicationStatus  
                }; 
                responseOut = dbInterface.ApproveRejectSeparationApplication(separationApplication);
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


        public List<SeparationApplicationViewModel> GetSeparationApplicationForExitInterviewList()
        {
            List<SeparationApplicationViewModel> separationapplications = new List<SeparationApplicationViewModel>();
            try
            {
                List<HR_SeparationApplication> separationapplicationList = dbInterface.GetSeparationApplicationForExitInterviewList();
                if (separationapplicationList != null && separationapplicationList.Count > 0)
                {
                    foreach (HR_SeparationApplication advance in separationapplicationList)
                    {
                        separationapplications.Add(new SeparationApplicationViewModel { ApplicationId = advance.ApplicationId, ApplicationNo = advance.ApplicationNo });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return separationapplications;
        }






    }
}
