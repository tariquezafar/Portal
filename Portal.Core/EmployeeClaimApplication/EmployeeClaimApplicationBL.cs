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
    public class EmployeeClaimApplicationBL
    {
        HRMSDBInterface dbInterface;
        public EmployeeClaimApplicationBL()
        {
            dbInterface = new HRMSDBInterface();
        }

        #region Employee Claim Application
        public ResponseOut AddEditEmployeeClaimApplication(EmployeeClaimApplicationViewModel employeeClaimApplicationViewModel)
        {

            ResponseOut responseOut = new ResponseOut();
            HRSQLDBInterface hRSQLDBInterface = new HRSQLDBInterface();              
            try
            {
                HR_EmployeeClaimApplication employeeClaimApplication = new HR_EmployeeClaimApplication
                {
                    ApplicationId =Convert.ToInt32(employeeClaimApplicationViewModel.ApplicationId),
                    ApplicationNo = employeeClaimApplicationViewModel.ApplicationNo,
                    ApplicationDate =Convert.ToDateTime(employeeClaimApplicationViewModel.ApplicationDate),
                    CompanyId = employeeClaimApplicationViewModel.CompanyId,
                    EmployeeId = employeeClaimApplicationViewModel.EmployeeId,
                    ClaimTypeId = employeeClaimApplicationViewModel.ClaimTypeId,
                    ClaimReason = employeeClaimApplicationViewModel.ClaimReason,
                    ClaimAmount = employeeClaimApplicationViewModel.ClaimAmount,
                    ClaimStatus = employeeClaimApplicationViewModel.ClaimStatus,
                    CompanyBranchId= employeeClaimApplicationViewModel.CompanyBranchId,
                };
                responseOut = hRSQLDBInterface.AddEditEmployeeClaimApplication(employeeClaimApplication);

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
        public List<EmployeeClaimApplicationViewModel> GetEmployeeClaimApplicationList(string applicationNo , int employeeId , int ClaimTypeId, string claimStatus , string fromDate , string toDate , int companyId,int companyBranchId)
        {
            List<EmployeeClaimApplicationViewModel> employeeClaimApplicationViewModel = new List<EmployeeClaimApplicationViewModel>();          
            HRSQLDBInterface hRSQLDBInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtClaimTypeApplication = hRSQLDBInterface.GetEmployeeClaimApplicationList(applicationNo, employeeId, ClaimTypeId, claimStatus, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate),companyId, companyBranchId);
                if (dtClaimTypeApplication != null && dtClaimTypeApplication.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtClaimTypeApplication.Rows)
                    {
                        employeeClaimApplicationViewModel.Add(new EmployeeClaimApplicationViewModel
                        {
                            ApplicationId = Convert.ToInt32(dr["ApplicationId"]),
                            ApplicationNo = Convert.ToString(dr["ApplicationNo"]),
                            ApplicationDate = Convert.ToString(dr["ApplicationDate"]),
                            EmployeeId = Convert.ToInt32(dr["EmployeeId"]),
                            EmployeeName = Convert.ToString(dr["EmployeeName"]),
                            ClaimTypeId = Convert.ToInt32(dr["ClaimTypeId"]),
                            ClaimTypeName = Convert.ToString(dr["ClaimTypeName"]),
                            ClaimReason = Convert.ToString(dr["ClaimReason"]),
                            ClaimStatus = Convert.ToString(dr["ClaimStatus"]),
                            ClaimAmount = Convert.ToDecimal(dr["ClaimAmount"]),
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
            return employeeClaimApplicationViewModel;
        }


        public EmployeeClaimApplicationViewModel GetEmployeeClaimApplicationDetail(long applicationId = 0)
        {
            EmployeeClaimApplicationViewModel employeeClaimApplicationViewModel = new EmployeeClaimApplicationViewModel();
            HRSQLDBInterface hRSQLDBInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtClaimTypeApplication = hRSQLDBInterface.GetEmployeeClaimApplicationDetail(applicationId);
                if (dtClaimTypeApplication != null && dtClaimTypeApplication.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtClaimTypeApplication.Rows)
                    {
                        employeeClaimApplicationViewModel = new EmployeeClaimApplicationViewModel
                        {
                            ApplicationId = Convert.ToInt32(dr["ApplicationId"]),
                            ApplicationNo = Convert.ToString(dr["ApplicationNo"]),
                            ApplicationDate = Convert.ToString(dr["ApplicationDate"]),
                            EmployeeId = Convert.ToInt32(dr["EmployeeId"]),
                            EmployeeName = Convert.ToString(dr["EmployeeName"]),
                            ClaimTypeId = Convert.ToInt32(dr["ClaimTypeId"]),
                            ClaimTypeName = Convert.ToString(dr["ClaimTypeName"]),
                            ClaimReason = Convert.ToString(dr["ClaimReason"]),
                            ClaimStatus = Convert.ToString(dr["ClaimStatus"]),
                            ClaimAmount = Convert.ToDecimal(dr["ClaimAmount"]),
                            RejectReason =Convert.ToString(dr["RejectReason"]),
                            RejectDate=Convert.ToString(dr["RejectDate"]),
                            ApproveDate = Convert.ToString(dr["ApproveDate"]),
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
            return employeeClaimApplicationViewModel;
        }
        #endregion

        #region Approval Employee Claim Application
        public ResponseOut ApprovalRejectedEmployeeClaimApplication(EmployeeClaimApplicationViewModel employeeClaimApplicationViewModel)
        {

            ResponseOut responseOut = new ResponseOut();
            HRSQLDBInterface hRSQLDBInterface = new HRSQLDBInterface();
            try
            {
                HR_EmployeeClaimApplication employeeClaimApplication = new HR_EmployeeClaimApplication
                {
                    ApplicationId = employeeClaimApplicationViewModel.ApplicationId,
                    RejectReason =string.IsNullOrEmpty(employeeClaimApplicationViewModel.RejectReason)?"":employeeClaimApplicationViewModel.RejectReason,
                    ApproveBy = employeeClaimApplicationViewModel.ApproveBy,
                    ClaimStatus= employeeClaimApplicationViewModel.ClaimStatus                   
                };

                responseOut = hRSQLDBInterface.ApproveRejectEmployeeClaimApplication(employeeClaimApplication);

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
        public List<EmployeeClaimApplicationViewModel> GetEmployeeClaimApplicationApprovalList(string applicationNo, int employeeId, int claimTypeId, string claimStatus, string fromDate, string toDate, int companyId,int companyBranchId)
        {
            List<EmployeeClaimApplicationViewModel> employeeClaimApplicationViewModel = new List<EmployeeClaimApplicationViewModel>();
            HRSQLDBInterface hRSQLDBInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtClaimTypeApplication = hRSQLDBInterface.GetEmployeeClaimApplicationApprovalList(applicationNo, employeeId, claimTypeId, claimStatus, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, companyBranchId);
                if (dtClaimTypeApplication != null && dtClaimTypeApplication.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtClaimTypeApplication.Rows)
                    {
                        employeeClaimApplicationViewModel.Add(new EmployeeClaimApplicationViewModel
                        {
                            ApplicationId = Convert.ToInt32(dr["ApplicationId"]),
                            ApplicationNo = Convert.ToString(dr["ApplicationNo"]),
                            ApplicationDate = Convert.ToString(dr["ApplicationDate"]),
                            EmployeeId = Convert.ToInt32(dr["EmployeeId"]),
                            EmployeeName = Convert.ToString(dr["EmployeeName"]),
                            ClaimTypeId = Convert.ToInt32(dr["ClaimTypeId"]),
                            ClaimTypeName = Convert.ToString(dr["ClaimTypeName"]),
                            ClaimReason = Convert.ToString(dr["ClaimReason"]),
                            ClaimAmount = Convert.ToDecimal(dr["ClaimAmount"]),
                            ClaimStatus = Convert.ToString(dr["ClaimStatus"]),
                            ApprovedByUserName = Convert.ToString(dr["ApprovedByUserName"]),
                            ApproveDate = Convert.ToString(dr["ApproveDate"]),
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
            return employeeClaimApplicationViewModel;
        }       
        #endregion

    }
}
