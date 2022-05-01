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
    public class EmployeeAdvanceAppBL
    {
        HRMSDBInterface dbInterface;
        public EmployeeAdvanceAppBL()
        {
            dbInterface = new HRMSDBInterface();
        }
       
        public ResponseOut AddEditEmployeeAdvanceApp(HR_EmployeeAdvanceApplicationViewModel employeeadvanceapplicationViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                HR_EmployeeAdvanceApplication employeeadvanceapplication = new HR_EmployeeAdvanceApplication
                {
                    ApplicationId = employeeadvanceapplicationViewModel.ApplicationId, 
                    ApplicationDate =Convert.ToDateTime(employeeadvanceapplicationViewModel.ApplicationDate),
                    EmployeeId = employeeadvanceapplicationViewModel.EmployeeId,
                    AdvanceTypeId = employeeadvanceapplicationViewModel.AdvanceTypeId,
                    AdvanceAmount = employeeadvanceapplicationViewModel.AdvanceAmount,
                    AdvanceInstallmentAmount = employeeadvanceapplicationViewModel.AdvanceInstallmentAmount,
                    AdvanceReason = employeeadvanceapplicationViewModel.AdvanceReason, 
                    AdvanceStartDate = Convert.ToDateTime(employeeadvanceapplicationViewModel.AdvanceStartDate),
                    AdvanceEndDate = Convert.ToDateTime(employeeadvanceapplicationViewModel.AdvanceEndDate),
                   
                    AdvanceStatus = employeeadvanceapplicationViewModel.AdvanceStatus,
                    CompanyId = employeeadvanceapplicationViewModel.CompanyId,
                    CompanyBranchId= employeeadvanceapplicationViewModel.companyBranch


                };
                responseOut = sqlDbInterface.AddEditEmployeeAdvanceApp(employeeadvanceapplication);
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


        public List<HR_EmployeeAdvanceApplicationViewModel> GetEmployeeAdvanceAppList(string applicationNo, int employeeId , string advanceTypeId ,  string advanceStatus, string fromDate, string toDate, int companyId,string companyBranch)
        {
            List<HR_EmployeeAdvanceApplicationViewModel> employeeadvanceApplications = new List<HR_EmployeeAdvanceApplicationViewModel>();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtEmployeeAdvanceApplications = sqlDbInterface.GetEmployeeAdvanceAppList(applicationNo, employeeId, advanceTypeId, advanceStatus, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, companyBranch);
                if (dtEmployeeAdvanceApplications != null && dtEmployeeAdvanceApplications.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtEmployeeAdvanceApplications.Rows)
                    {
                        employeeadvanceApplications.Add(new HR_EmployeeAdvanceApplicationViewModel
                        {
                            ApplicationId = Convert.ToInt32(dr["ApplicationId"]),
                            ApplicationNo = Convert.ToString(dr["ApplicationNo"]), 
                            AdvanceTypeId = Convert.ToInt16(dr["AdvanceTypeId"]),
                            AdvanceTypeName = Convert.ToString(dr["AdvanceTypeName"]),
                            EmployeeId = Convert.ToInt32(dr["EmployeeId"]),
                            EmployeeName = Convert.ToString(dr["EmployeeName"]),
                            AdvanceInstallmentAmount = Convert.ToInt32(dr["AdvanceInstallmentAmount"]),
                            AdvanceAmount = Convert.ToInt32(dr["AdvanceAmount"]),
                            AdvanceStartDate = Convert.ToString(dr["AdvanceStartDate"]),
                            AdvanceEndDate = Convert.ToString(dr["AdvanceEndDate"]),
                            ApplicationDate = Convert.ToString(dr["ApplicationDate"]),
                            AdvanceReason = Convert.ToString(dr["AdvanceReason"]),
                            AdvanceStatus = Convert.ToString(dr["AdvanceStatus"]),
                            companyBranchName= Convert.ToString(dr["BranchName"])



                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return employeeadvanceApplications;
        }



        public HR_EmployeeAdvanceApplicationViewModel GetEmployeeAdvanceAppDetail(long applicationId = 0)
        {
            HR_EmployeeAdvanceApplicationViewModel employeeadvanceApplications = new HR_EmployeeAdvanceApplicationViewModel();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtEmployeeAdvanceApplication = sqlDbInterface.GetEmployeeAdvanceAppDetail(applicationId);
                if (dtEmployeeAdvanceApplication != null && dtEmployeeAdvanceApplication.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtEmployeeAdvanceApplication.Rows)
                    {
                        employeeadvanceApplications = new HR_EmployeeAdvanceApplicationViewModel
                        {
                            ApplicationId = Convert.ToInt32(dr["ApplicationId"]),
                            ApplicationNo = Convert.ToString(dr["ApplicationNo"]),
                            AdvanceTypeId = Convert.ToInt16(dr["AdvanceTypeId"]),
                            EmployeeId = Convert.ToInt32(dr["EmployeeId"]),
                            EmployeeName = Convert.ToString(dr["EmployeeName"]),
                            AdvanceInstallmentAmount = Convert.ToInt32(dr["AdvanceInstallmentAmount"]),
                            AdvanceAmount = Convert.ToInt32(dr["AdvanceAmount"]),
                            AdvanceStartDate = Convert.ToString(dr["AdvanceStartDate"]),
                            AdvanceEndDate = Convert.ToString(dr["AdvanceEndDate"]), 
                            ApplicationDate = Convert.ToString(dr["ApplicationDate"]),
                            AdvanceReason = Convert.ToString(dr["AdvanceReason"]),  
                            AdvanceStatus = Convert.ToString(dr["AdvanceStatus"]), 
                            ApproveDate = Convert.ToString(dr["ApproveDate"]), 
                            RejectDate = Convert.ToString(dr["RejectDate"]),
                            RejectReason = Convert.ToString(dr["RejectReason"]),
                            companyBranch= Convert.ToInt32(dr["CompanyBranchId"])
                            

                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return employeeadvanceApplications;
        }


        public List<HR_EmployeeAdvanceApplicationViewModel> GetEmployeeAdvanceAppApprovalList(string applicationNo, int employeeId, string advanceTypeId, string advanceStatus, string fromDate, string toDate, int companyId)
        {
            List<HR_EmployeeAdvanceApplicationViewModel> employeeadvanceApplications = new List<HR_EmployeeAdvanceApplicationViewModel>();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtEmployeeAdvanceApplications = sqlDbInterface.GetEmployeeAdvanceAppApprovalList(applicationNo, employeeId, advanceTypeId, advanceStatus, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId);
                if (dtEmployeeAdvanceApplications != null && dtEmployeeAdvanceApplications.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtEmployeeAdvanceApplications.Rows)
                    {
                        employeeadvanceApplications.Add(new HR_EmployeeAdvanceApplicationViewModel
                        {
                            ApplicationId = Convert.ToInt32(dr["ApplicationId"]),
                            ApplicationNo = Convert.ToString(dr["ApplicationNo"]),
                            AdvanceTypeId = Convert.ToInt16(dr["AdvanceTypeId"]),
                            AdvanceTypeName = Convert.ToString(dr["AdvanceTypeName"]),
                            EmployeeId = Convert.ToInt32(dr["EmployeeId"]),
                            EmployeeName = Convert.ToString(dr["EmployeeName"]),
                            AdvanceInstallmentAmount = Convert.ToInt32(dr["AdvanceInstallmentAmount"]),
                            AdvanceAmount = Convert.ToInt32(dr["AdvanceAmount"]),
                            AdvanceStartDate = Convert.ToString(dr["AdvanceStartDate"]),
                            AdvanceEndDate = Convert.ToString(dr["AdvanceEndDate"]),
                            ApplicationDate = Convert.ToString(dr["ApplicationDate"]),
                            AdvanceReason = Convert.ToString(dr["AdvanceReason"]),
                            AdvanceStatus = Convert.ToString(dr["AdvanceStatus"]),
                            ApprovedByUserName = Convert.ToString(dr["ApprovedByUserName"]),
                            ApproveDate = Convert.ToString(dr["ApproveDate"]),
                            RejectDate = Convert.ToString(dr["RejectDate"])

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return employeeadvanceApplications;
        }
         

        public ResponseOut ApproveRejectEmployeeAdvanceApp(HR_EmployeeAdvanceApplicationViewModel employeeadvanceapplicationViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                HR_EmployeeAdvanceApplication employeeApplication = new HR_EmployeeAdvanceApplication
                {
                    ApplicationId = employeeadvanceapplicationViewModel.ApplicationId,
                    RejectReason = employeeadvanceapplicationViewModel.RejectReason, 
                    ApproveBy = employeeadvanceapplicationViewModel.ApproveBy,
                    AdvanceStatus = employeeadvanceapplicationViewModel.AdvanceStatus

                };

                responseOut = dbInterface.ApproveRejectEmployeeAdvanceApp(employeeApplication);
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



 























    }
}
