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
    public class LeaveTypeBL
    {
        HRMSDBInterface dbInterface;
        public LeaveTypeBL()
        {
            dbInterface = new HRMSDBInterface();
        }
       
        public ResponseOut AddEditLeaveType(HR_LeaveTypeViewModel leaveTypeViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                HR_LeaveType hR_LeaveType = new HR_LeaveType
                {
                    LeaveTypeId = leaveTypeViewModel.LeaveTypeId,
                    LeaveTypeName = leaveTypeViewModel.LeaveTypeName,
                    LeaveTypeCode = leaveTypeViewModel.LeaveTypeCode,
                    LeavePeriod = leaveTypeViewModel.LeavePeriod,
                    PayPeriod = leaveTypeViewModel.PayPeriod,
                    WorkPeriod = leaveTypeViewModel.WorkPeriod,                 
                    Status = leaveTypeViewModel.LeaveType_Status
                };
                responseOut = dbInterface.AddEditLeaveType(hR_LeaveType);
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

        public List<HR_LeaveTypeViewModel> GetLeaveTypeList(string leaveTypeName = "", string leaveTypeCode = "", string Status = "")
        {
            List<HR_LeaveTypeViewModel> leaveTypeViewModel = new List<HR_LeaveTypeViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCLaimTypes = sqlDbInterface.GetLeaveTypeList(leaveTypeName, leaveTypeCode, Status);
                if (dtCLaimTypes != null && dtCLaimTypes.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCLaimTypes.Rows)
                    {
                        leaveTypeViewModel.Add(new HR_LeaveTypeViewModel
                        {
                            LeaveTypeId = Convert.ToInt32(dr["LeaveTypeId"]),
                            LeaveTypeName = Convert.ToString(dr["LeaveTypeName"]),
                            LeaveTypeCode= Convert.ToString(dr["LeaveTypeCode"]),
                            LeavePeriod = Convert.ToDecimal(dr["LeavePeriod"]),
                            PayPeriod = Convert.ToDecimal(dr["PayPeriod"]),
                            WorkPeriod = Convert.ToDecimal(dr["WorkPeriod"]),                         
                            LeaveType_Status = Convert.ToBoolean(dr["Status"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return leaveTypeViewModel;
        }

        public HR_LeaveTypeViewModel GetLeaveTypeDetail(int leaveTypeId = 0)
        {
            HR_LeaveTypeViewModel leaveTypeViewModel = new HR_LeaveTypeViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtClaimTypes = sqlDbInterface.GetLeaveTypeDetail(leaveTypeId);
                if (dtClaimTypes != null && dtClaimTypes.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtClaimTypes.Rows)
                    {
                        leaveTypeViewModel = new HR_LeaveTypeViewModel
                        {
                            LeaveTypeId = Convert.ToInt32(dr["LeaveTypeId"]),
                            LeaveTypeName = Convert.ToString(dr["LeaveTypeName"]),
                            LeaveTypeCode = Convert.ToString(dr["LeaveTypeCode"]),
                            LeavePeriod = Convert.ToDecimal(dr["LeavePeriod"]),
                            PayPeriod = Convert.ToDecimal(dr["PayPeriod"]),
                            WorkPeriod = Convert.ToDecimal(dr["WorkPeriod"]),
                            LeaveType_Status = Convert.ToBoolean(dr["Status"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return leaveTypeViewModel;
        }

        public List<HR_LeaveTypeViewModel> GetLeaveTypeForEmpolyeeLeaveAppList()
        {
            List<HR_LeaveTypeViewModel> leaveapplications = new List<HR_LeaveTypeViewModel>();
            try
            {
                List<HR_LeaveType> leaveList = dbInterface.GetLeaveTypeForEmpolyeeLeaveAppList();
                if (leaveList != null && leaveList.Count > 0)
                {
                    foreach (HR_LeaveType leave in leaveList)
                    {
                        leaveapplications.Add(new HR_LeaveTypeViewModel { LeaveTypeId = leave.LeaveTypeId, LeaveTypeName = leave.LeaveTypeName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return leaveapplications;
        }

        public List<EmployeeLeaveApplicationViewModel> GetEmployeeLeaveApplicationDetails(int companyId, int userId, int reportingUserId, int reportingRoleId)
        {

            List<EmployeeLeaveApplicationViewModel> leaveApplicationList = new List<EmployeeLeaveApplicationViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtList = sqlDbInterface.GetEmployeeLeaveApplicationDetails(companyId, userId, reportingUserId, reportingRoleId);
                if (dtList != null && dtList.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtList.Rows)
                    {
                        leaveApplicationList.Add(new EmployeeLeaveApplicationViewModel
                        {
                            ApplicationId = Convert.ToInt32(dr["ApplicationId"]),
                            ApplicationNo = Convert.ToString(dr["ApplicationNo"]),
                            ApplicationDate = Convert.ToString(dr["ApplicationDate"]),
                            FromDate = Convert.ToString(dr["FromDate"]),
                            ToDate = Convert.ToString(dr["ToDate"]),
                            NoofDays = Convert.ToDecimal(dr["NoofDays"]),
                            LeaveTypeName = Convert.ToString(dr["LeaveTypeName"]),
                            LeaveReason = Convert.ToString(dr["LeaveReason"]),
                            LeaveStatus = Convert.ToString(dr["LeaveStatus"]),
                            EmployeeId = Convert.ToInt32(dr["EmployeeId"]),
                            EmployeeName = Convert.ToString(dr["EmployeeName"])
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return leaveApplicationList;
        }


    }
}
