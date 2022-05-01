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
    public class EmployeeLeaveApplicationBL
    {
        HRMSDBInterface dbInterface;
        public EmployeeLeaveApplicationBL()
        {
            dbInterface = new HRMSDBInterface();
        }
       

        public ResponseOut AddEditEmployeeLeaveApplication(EmployeeLeaveApplicationViewModel employeeleaveapplicationViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                HR_EmployeeLeaveApplication employeeleaveapplication = new HR_EmployeeLeaveApplication
                {
                    ApplicationId = employeeleaveapplicationViewModel.ApplicationId, 
                    ApplicationDate = Convert.ToDateTime(employeeleaveapplicationViewModel.ApplicationDate),
                    EmployeeId = employeeleaveapplicationViewModel.EmployeeId,
                    LeaveTypeId = employeeleaveapplicationViewModel.LeaveTypeId,
                    LeaveReason = employeeleaveapplicationViewModel.LeaveReason,
                    FromDate = Convert.ToDateTime(employeeleaveapplicationViewModel.FromDate),
                    ToDate = Convert.ToDateTime(employeeleaveapplicationViewModel.ToDate),
                    NoofDays = employeeleaveapplicationViewModel.NoofDays,
                    LeaveStatus = employeeleaveapplicationViewModel.LeaveStatus, 
                    CompanyId = employeeleaveapplicationViewModel.CompanyId,
                    CompanyBranchId= employeeleaveapplicationViewModel.CompanyBranchId,

                };
                responseOut = sqlDbInterface.AddEditEmployeeLeaveApplication(employeeleaveapplication);
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


        public List<EmployeeLeaveApplicationViewModel> GetEmployeeLeaveApplicationList(string applicationNo, int employeeId, string leaveTypeId, string leaveStatus, string fromDate, string toDate, int companyId,int companyBranchId)
        {
            List<EmployeeLeaveApplicationViewModel> employeeleaveApplications = new List<EmployeeLeaveApplicationViewModel>();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtEmployeeLeaveApplications = sqlDbInterface.GetEmployeeLeaveApplicationList(applicationNo, employeeId, leaveTypeId, leaveStatus, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, companyBranchId);
                if (dtEmployeeLeaveApplications != null && dtEmployeeLeaveApplications.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtEmployeeLeaveApplications.Rows)
                    {
                        employeeleaveApplications.Add(new EmployeeLeaveApplicationViewModel
                        {
                            ApplicationId = Convert.ToInt32(dr["ApplicationId"]),
                            ApplicationNo = Convert.ToString(dr["ApplicationNo"]),
                            ApplicationDate = Convert.ToString(dr["ApplicationDate"]),
                            LeaveTypeId = Convert.ToInt16(dr["LeaveTypeId"]),
                            LeaveTypeName = Convert.ToString(dr["LeaveTypeName"]),
                            EmployeeId = Convert.ToInt32(dr["EmployeeId"]),
                            EmployeeName = Convert.ToString(dr["EmployeeName"]),
                            FromDate = Convert.ToString(dr["FromDate"]),
                            ToDate = Convert.ToString(dr["ToDate"]),
                            LeaveReason = Convert.ToString(dr["LeaveReason"]),
                            NoofDays = Convert.ToDecimal(dr["NoofDays"]),
                            LeaveStatus = Convert.ToString(dr["LeaveStatus"]), 
                            CompanyBranchName=Convert.ToString(dr["CompanyBranchName"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return employeeleaveApplications;
        }

 
        public EmployeeLeaveApplicationViewModel GetEmployeeLeaveApplicationDetail(long applicationId = 0)
        {
            EmployeeLeaveApplicationViewModel employeeLeaveApplications = new EmployeeLeaveApplicationViewModel();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtEmployeeLeaveApplication = sqlDbInterface.GetEmployeeLeaveApplicationDetails(applicationId);
                if (dtEmployeeLeaveApplication != null && dtEmployeeLeaveApplication.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtEmployeeLeaveApplication.Rows)
                    {
                        employeeLeaveApplications = new EmployeeLeaveApplicationViewModel
                        {
                            ApplicationId = Convert.ToInt32(dr["ApplicationId"]),
                            ApplicationNo = Convert.ToString(dr["ApplicationNo"]),
                            ApplicationDate = Convert.ToString(dr["ApplicationDate"]),
                            LeaveTypeId = Convert.ToInt16(dr["LeaveTypeId"]), 
                            EmployeeId = Convert.ToInt32(dr["EmployeeId"]),
                            EmployeeName = Convert.ToString(dr["EmployeeName"]),
                            FromDate = Convert.ToString(dr["FromDate"]),
                            ToDate = Convert.ToString(dr["ToDate"]),
                            NoofDays = Convert.ToDecimal(dr["NoofDays"]),
                            LeaveReason = Convert.ToString(dr["LeaveReason"]),
                            LeaveStatus = Convert.ToString(dr["LeaveStatus"]),
                            RejectDate = Convert.ToString(dr["RejectDate"]),
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
            return employeeLeaveApplications;
        }


        public List<EmployeeLeaveApplicationViewModel> GetEmployeeLeaveApplicationApprovalList(string applicationNo, int employeeId, string leaveTypeId, string leaveStatus, string fromDate, string toDate, int companyId,int companyBranchId)
        {
            List<EmployeeLeaveApplicationViewModel> employeeLeaveApplications = new List<EmployeeLeaveApplicationViewModel>();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtEmployeeLeaveApplications = sqlDbInterface.GetEmployeeLeaveApplicationApprovalList(applicationNo, employeeId, leaveTypeId, leaveStatus, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, companyBranchId);
                if (dtEmployeeLeaveApplications != null && dtEmployeeLeaveApplications.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtEmployeeLeaveApplications.Rows)
                    {
                        employeeLeaveApplications.Add(new EmployeeLeaveApplicationViewModel
                        {
                            ApplicationId = Convert.ToInt32(dr["ApplicationId"]),
                            ApplicationNo = Convert.ToString(dr["ApplicationNo"]),
                            LeaveTypeId = Convert.ToInt16(dr["LeaveTypeId"]),
                            LeaveTypeName = Convert.ToString(dr["LeaveTypeName"]),
                            EmployeeId = Convert.ToInt32(dr["EmployeeId"]),
                            EmployeeName = Convert.ToString(dr["EmployeeName"]), 
                            FromDate = Convert.ToString(dr["FromDate"]),
                            ToDate = Convert.ToString(dr["ToDate"]),
                            ApplicationDate = Convert.ToString(dr["ApplicationDate"]),
                            LeaveReason = Convert.ToString(dr["LeaveReason"]),
                            LeaveStatus = Convert.ToString(dr["LeaveStatus"]),
                            ApprovedByUserName = Convert.ToString(dr["ApprovedByUserName"]),
                            ApproveDate = Convert.ToString(dr["ApproveDate"]),
                            RejectDate = Convert.ToString(dr["RejectDate"]),
                            CompanyBranchName=Convert.ToString(dr["CompanyBranchName"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return employeeLeaveApplications;
        }


        public ResponseOut ApproveRejectEmployeeLeaveApplication(EmployeeLeaveApplicationViewModel employeeleaveapplicationViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            HRSQLDBInterface hRSQLDBInterface = new HRSQLDBInterface();
            try
            {
                HR_EmployeeLeaveApplication employeeApplication = new HR_EmployeeLeaveApplication
                {
                    ApplicationId = employeeleaveapplicationViewModel.ApplicationId,
                    RejectReason =  employeeleaveapplicationViewModel.RejectReason,
                    ApproveBy = employeeleaveapplicationViewModel.ApproveBy,
                    LeaveStatus = employeeleaveapplicationViewModel.LeaveStatus,
                 };
               
                responseOut= hRSQLDBInterface.ApproveEmployeeLeaveDetail(employeeApplication);
               // responseOut = dbInterface.ApproveRejectEmployeeLeaveApplication(employeeApplication);
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
