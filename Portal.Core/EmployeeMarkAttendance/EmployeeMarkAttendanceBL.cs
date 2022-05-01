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
    public class EmployeeMarkAttendanceBL
    {
        HRMSDBInterface dbInterface;
        public EmployeeMarkAttendanceBL()
        {
            dbInterface = new HRMSDBInterface();
        }
       
        public ResponseOut AddEditEmployeeMarkAttendance(EmployeeMarkAttendanceViewModel employeeMarkAttendanceViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                HR_EmployeeAttendance employeeAttendance = new HR_EmployeeAttendance
                {
                    EmployeeAttendanceId = employeeMarkAttendanceViewModel.EmployeeAttendanceId, 
                    AttendanceDate =Convert.ToDateTime(employeeMarkAttendanceViewModel.AttendanceDate),
                    EmployeeId = employeeMarkAttendanceViewModel.EmployeeId,
                    PresentAbsent = employeeMarkAttendanceViewModel.PresentAbsent,
                    InOut = employeeMarkAttendanceViewModel.InOut,
                    CompanyId = employeeMarkAttendanceViewModel.CompanyId,
                    CreatedBy = employeeMarkAttendanceViewModel.CreatedBy,
                    AttendanceStatus = employeeMarkAttendanceViewModel.AttendanceStatus
                };
                responseOut = sqlDbInterface.AddEditEmployeeMarkAttendance(employeeAttendance);
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

        public List<EmployeeMarkAttendanceViewModel> GetEmployeeInOutDetails(string attendanceDate, int employeeId)
        {
            List<EmployeeMarkAttendanceViewModel> employeeMarkAttendance = new List<EmployeeMarkAttendanceViewModel>();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtEmployeeMarkAttendance = sqlDbInterface.GetEmployeeInOutDetails(attendanceDate, employeeId);
                if (dtEmployeeMarkAttendance != null && dtEmployeeMarkAttendance.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtEmployeeMarkAttendance.Rows)
                    {
                        employeeMarkAttendance.Add(new EmployeeMarkAttendanceViewModel
                        {
                            InOut = Convert.ToString(dr["InOut"]),
                            TrnDateTime = Convert.ToString(dr["TrnDateTime"]),
                            companyBranchId= Convert.ToInt32(dr["CompanyBranchId"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return employeeMarkAttendance;
        }

        public List<EmployeeMarkAttendanceViewModel> GetEmployeeMarkAttendanceList(int employeeId, string fromDate, string toDate, int companyId)
        {
            List<EmployeeMarkAttendanceViewModel> employeeMarkAttendance = new List<EmployeeMarkAttendanceViewModel>();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtEmployeeMarkAttendance = sqlDbInterface.GetEmployeeMarkAttendanceList(employeeId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId);
                if (dtEmployeeMarkAttendance != null && dtEmployeeMarkAttendance.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtEmployeeMarkAttendance.Rows)
                    {
                        employeeMarkAttendance.Add(new EmployeeMarkAttendanceViewModel
                        {
                            EmployeeAttendanceId = Convert.ToInt32(dr["EmployeeAttendanceId"]),
                            AttendanceDate = Convert.ToString(dr["AttendanceDate"]),
                            PresentAbsent = Convert.ToString(dr["PresentAbsent"]),
                            TrnDateTime = Convert.ToString(dr["TrnDateTime"]),
                            EmployeeId = Convert.ToInt32(dr["EmployeeId"]),
                            EmployeeName = Convert.ToString(dr["EmployeeName"]),
                            InOut = Convert.ToString(dr["InOut"]),
                            AttendanceStatus = Convert.ToString(dr["AttendanceStatus"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return employeeMarkAttendance;
        }


























    }
}
