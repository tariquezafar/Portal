using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Core;
using Portal.DAL;
using Portal.Common;
using System.Reflection;
using System.Data;
using Portal.DAL.Infrastructure;
namespace Portal.Core
{
 public class EmployeeLeaveDetailBL
    {
        HRMSDBInterface dbInterface;
        public EmployeeLeaveDetailBL()
        {
            dbInterface = new HRMSDBInterface();
        }

        public ResponseOut AddEditEmployeeLeaveDetail(EmployeeLeaveDetailViewmodel employeeLeaveDetailViewmodel)
        {

            ResponseOut responseOut = new ResponseOut();
            HRSQLDBInterface hRSQLDBInterface = new HRSQLDBInterface();
            try
            {
                HR_EmployeeLeaveDetail employeeLeaveDetail = new HR_EmployeeLeaveDetail {
                    EmployeeLeaveId= employeeLeaveDetailViewmodel.EmployeeLeaveId,
                    EmployeeId = employeeLeaveDetailViewmodel.EmployeeId,
                    CompanyId = employeeLeaveDetailViewmodel.CompanyId,
                    FinYearId = employeeLeaveDetailViewmodel.FinYearId,
                    LeaveTypeId = employeeLeaveDetailViewmodel.LeaveTypeId,
                    LeaveCount = employeeLeaveDetailViewmodel.LeaveCount,
                    LeaveDate =Convert.ToDateTime(employeeLeaveDetailViewmodel.LeaveDate),
                    CreatedBy= employeeLeaveDetailViewmodel.CreatedBy,
                    Status= employeeLeaveDetailViewmodel.EmployeeLeaveDetailStatus
                };
                responseOut = hRSQLDBInterface.AddEditEmployeeLeaveDetail(employeeLeaveDetail);

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

        public EmployeeLeaveDetailViewmodel GetEmployeeLeaveDetail(int employeeLeaveId = 0)
        {
            EmployeeLeaveDetailViewmodel employeeLeaveDetail = new EmployeeLeaveDetailViewmodel();
           
            try
            {
                HRSQLDBInterface hRSQLDBInterface = new HRSQLDBInterface();
                DataTable employeeLeaveDetails = hRSQLDBInterface.GetEmployeeLeaveDetail(employeeLeaveId);
                if (employeeLeaveDetails != null && employeeLeaveDetails.Rows.Count > 0)
                {
                    foreach (DataRow dr in employeeLeaveDetails.Rows)
                    {
                        employeeLeaveDetail = new EmployeeLeaveDetailViewmodel {

                            EmployeeLeaveId = Convert.ToInt32(dr["EmployeeLeaveId"]),
                            EmployeeId = Convert.ToInt32(dr["EmployeeId"]),
                            EmployeeName = Convert.ToString(dr["EmployeeName"]),
                            LeaveTypeId = Convert.ToInt32(dr["LeaveTypeId"]),
                            LeaveCount = Convert.ToInt32(dr["LeaveCount"]),
                            LeaveDate = Convert.ToString(dr["LeaveDate"]),
                            EmployeeLeaveDetailStatus = Convert.ToBoolean(dr["Status"]),
                            CreatedByName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate =Convert.ToString(dr["CreatedDate"]),
                            ModifiedDate = string.IsNullOrEmpty(Convert.ToString(dr["ModifiedDate"])) ? "" : Convert.ToString(dr["ModifiedDate"]),
                            ModifiedByName = string.IsNullOrEmpty(Convert.ToString(dr["ModifiedByName"])) ? "" : Convert.ToString(dr["ModifiedByName"])
                        };

                        
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return employeeLeaveDetail;
        }
        public List<EmployeeLeaveDetailViewmodel> GetEmployeeLeaveDetailList(int employeeId, int leaveTypeId, string fromDate, string toDate, int companyId, string status)
        {
            List<EmployeeLeaveDetailViewmodel> employeeLeaveDetail = new List<EmployeeLeaveDetailViewmodel>();
            HRSQLDBInterface hRSQLDBInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtactivityCalenders = hRSQLDBInterface.GetEmployeeLeaveDetailList(employeeId, leaveTypeId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, status);
                if (dtactivityCalenders != null && dtactivityCalenders.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtactivityCalenders.Rows)
                    {
                        employeeLeaveDetail.Add(new EmployeeLeaveDetailViewmodel
                        {
                            EmployeeLeaveId = Convert.ToInt32(dr["EmployeeLeaveId"]),
                            EmployeeId = Convert.ToInt32(dr["EmployeeId"]),
                            EmployeeName = Convert.ToString(dr["EmployeeName"]),
                            LeaveTypeId = Convert.ToInt32(dr["LeaveTypeId"]),
                            LeaveTypeName = Convert.ToString(dr["LeaveTypeName"]),
                            LeaveCount = Convert.ToInt32(dr["LeaveCount"]),
                            LeaveDate = Convert.ToString(dr["LeaveDate"]),
                            EmployeeLeaveDetailStatus = Convert.ToBoolean(dr["Status"]),
                            CreatedByName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedDate = string.IsNullOrEmpty(Convert.ToString(dr["ModifiedDate"])) ? "" : Convert.ToString(dr["ModifiedDate"]),
                            ModifiedByName = string.IsNullOrEmpty(Convert.ToString(dr["ModifiedByName"])) ? "" : Convert.ToString(dr["ModifiedByName"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return employeeLeaveDetail;
        }


        public List<EmployeeLeaveDetailViewmodel> GetEmployeeLeaveBalanceList(int employeeId)
        {
            List<EmployeeLeaveDetailViewmodel> employeeLeaveDetail = new List<EmployeeLeaveDetailViewmodel>();
            HRSQLDBInterface hRSQLDBInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtLeaveDetails = hRSQLDBInterface.GetEmployeeLeaveBalanceDetailsList(employeeId);
                if (dtLeaveDetails != null && dtLeaveDetails.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtLeaveDetails.Rows)
                    {
                        employeeLeaveDetail.Add(new EmployeeLeaveDetailViewmodel
                        {
                            LeaveTypeName = Convert.ToString(dr["LeaveType"]),
                            LeaveCount = Convert.ToInt32(dr["LeaveBalance"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return employeeLeaveDetail;
        }
    }
}
