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
    public class EmployeeAssetApplicationBL
    {
        HRMSDBInterface dbInterface;
        public EmployeeAssetApplicationBL()
        {
            dbInterface = new HRMSDBInterface();
        }

        #region Employee Asset Application
        public ResponseOut AddEditEmployeeAssetApplication(EmployeeAssetApplicationViewModel employeeAssetApplicationViewModel)
        {

            ResponseOut responseOut = new ResponseOut();
            HRSQLDBInterface hRSQLDBInterface = new HRSQLDBInterface();              
            try
            {
                HR_EmployeeAssetApplication employeeAssetApplication = new HR_EmployeeAssetApplication
                {
                    ApplicationId =Convert.ToInt32( employeeAssetApplicationViewModel.ApplicationId),
                    ApplicationNo = employeeAssetApplicationViewModel.ApplicationNo,
                    ApplicationDate =Convert.ToDateTime(employeeAssetApplicationViewModel.ApplicationDate),
                    CompanyId = employeeAssetApplicationViewModel.CompanyId,
                    EmployeeId = employeeAssetApplicationViewModel.EmployeeId,
                    AssetTypeId = employeeAssetApplicationViewModel.AssetTypeId,
                    AssetReason = employeeAssetApplicationViewModel.AssetReason,
                    AssetStatus =Convert.ToBoolean(employeeAssetApplicationViewModel.AssetStatus),
                    ApplicationStatus = employeeAssetApplicationViewModel.ApplicationStatus,
                    CompanyBranchId= employeeAssetApplicationViewModel.CompanyBranchId,
                };
                responseOut = hRSQLDBInterface.AddEditEmployeeAssetApplication(employeeAssetApplication);

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
        public List<EmployeeAssetApplicationViewModel> GetEmployeeAssetApplicationList(string applicationNo , int employeeId , string assetTypeName, string assetStatus , string fromDate , string toDate , int companyId,int companyBranchId)
        {
            List<EmployeeAssetApplicationViewModel> employeeAssetApplicationViewModel = new List<EmployeeAssetApplicationViewModel>();          
            HRSQLDBInterface hRSQLDBInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtAssetTypeApplication = hRSQLDBInterface.GetEmployeeAssetApplicationList(applicationNo, employeeId, assetTypeName, assetStatus, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate),companyId, companyBranchId);
                if (dtAssetTypeApplication != null && dtAssetTypeApplication.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtAssetTypeApplication.Rows)
                    {
                        employeeAssetApplicationViewModel.Add(new EmployeeAssetApplicationViewModel
                        {
                            ApplicationId = Convert.ToInt32(dr["ApplicationId"]),
                            ApplicationNo = Convert.ToString(dr["ApplicationNo"]),
                            ApplicationDate = Convert.ToString(dr["ApplicationDate"]),
                            EmployeeId = Convert.ToInt32(dr["EmployeeId"]),
                            EmployeeName = Convert.ToString(dr["EmployeeName"]),
                            AssetTypeId = Convert.ToInt32(dr["AssetTypeId"]),
                            AssetTypeName = Convert.ToString(dr["AssetTypeName"]),
                            AssetReason = Convert.ToString(dr["AssetReason"]),
                            ApplicationStatus = Convert.ToString(dr["ApplicationStatus"]),
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
            return employeeAssetApplicationViewModel;
        }
        public EmployeeAssetApplicationViewModel GetEmployeeAssetApplicationDetail(long applicationId = 0)
        {
            EmployeeAssetApplicationViewModel employeeAssetApplicationViewModel = new EmployeeAssetApplicationViewModel();
            HRSQLDBInterface hRSQLDBInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtassetTypeApplication = hRSQLDBInterface.GetEmployeeAssetApplicationDetail(applicationId);
                if (dtassetTypeApplication != null && dtassetTypeApplication.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtassetTypeApplication.Rows)
                    {
                        employeeAssetApplicationViewModel = new EmployeeAssetApplicationViewModel
                        {
                            ApplicationId = Convert.ToInt32(dr["ApplicationId"]),
                            ApplicationNo = Convert.ToString(dr["ApplicationNo"]),
                            ApplicationDate = Convert.ToString(dr["ApplicationDate"]),
                            EmployeeId = Convert.ToInt32(dr["EmployeeId"]),
                            EmployeeName = Convert.ToString(dr["EmployeeName"]),
                            AssetTypeId = Convert.ToInt32(dr["AssetTypeId"]),
                            AssetTypeName = Convert.ToString(dr["AssetTypeName"]),
                            AssetReason = Convert.ToString(dr["AssetReason"]),
                            ApplicationStatus = Convert.ToString(dr["ApplicationStatus"]),
                            RejectReason=Convert.ToString(dr["RejectReason"]),
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
            return employeeAssetApplicationViewModel;
        }
        #endregion

        #region Approval Employee Asset Application
        public ResponseOut ApprovalRejectedEmployeeAssetApplication(EmployeeAssetApplicationViewModel employeeAssetApplicationViewModel)
        {

            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                HR_EmployeeAssetApplication employeeAssetApplication = new HR_EmployeeAssetApplication
                {
                    ApplicationId = employeeAssetApplicationViewModel.ApplicationId,
                    RejectReason = employeeAssetApplicationViewModel.RejectReason,
                    ApproveBy = employeeAssetApplicationViewModel.ApproveBy,
                    ApplicationStatus= employeeAssetApplicationViewModel.ApplicationStatus,
                    CompanyBranchId= employeeAssetApplicationViewModel.CompanyBranchId
                    // LoanStatus = employeeAssetApplicationViewModel.LoanStatus

                };

                responseOut = dbInterface.ApproveRejectEmployeeAssetApplication(employeeAssetApplication);

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
        public List<EmployeeAssetApplicationViewModel> GetEmployeeAssetApplicationApprovalList(string applicationNo, int employeeId, string assetTypeName, string assetStatus, string fromDate, string toDate, int companyId,int companyBranchId)
        {
            List<EmployeeAssetApplicationViewModel> employeeAssetApplicationViewModel = new List<EmployeeAssetApplicationViewModel>();
            HRSQLDBInterface hRSQLDBInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtAssetTypeApplication = hRSQLDBInterface.GetEmployeeAssetApplicationApprovalList(applicationNo, employeeId, assetTypeName, assetStatus, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, companyBranchId);
                if (dtAssetTypeApplication != null && dtAssetTypeApplication.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtAssetTypeApplication.Rows)
                    {
                        employeeAssetApplicationViewModel.Add(new EmployeeAssetApplicationViewModel
                        {
                            ApplicationId = Convert.ToInt32(dr["ApplicationId"]),
                            ApplicationNo = Convert.ToString(dr["ApplicationNo"]),
                            ApplicationDate = Convert.ToString(dr["ApplicationDate"]),
                            EmployeeId = Convert.ToInt32(dr["EmployeeId"]),
                            EmployeeName = Convert.ToString(dr["EmployeeName"]),
                            AssetTypeId = Convert.ToInt32(dr["AssetTypeId"]),
                            AssetTypeName = Convert.ToString(dr["AssetTypeName"]),
                            AssetReason = Convert.ToString(dr["AssetReason"]),
                            ApplicationStatus = Convert.ToString(dr["ApplicationStatus"]),
                            ApprovedByUserName = Convert.ToString(dr["ApprovedByUserName"]),
                            ApproveDate = Convert.ToString(dr["ApproveDate"]),
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
            return employeeAssetApplicationViewModel;
        }       
        #endregion

    }
}
