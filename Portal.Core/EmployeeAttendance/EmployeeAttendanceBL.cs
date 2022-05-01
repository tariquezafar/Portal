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
    public class EmployeeAttendanceBL
    {
        HRMSDBInterface dbInterface;
        public EmployeeAttendanceBL()
        {
            dbInterface = new HRMSDBInterface();
        }
       
        public ResponseOut AddEditEmployeeAttendance(int companyId, int employeeId, string attendanceDate, string presentAbsent, string inTime, string outTime, string attendanceStatus, int userId,int companyBranch)
        {
            ResponseOut responseOut = new ResponseOut();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                responseOut = sqlDbInterface.AddEditEmployeeAttendance(companyId, employeeId, attendanceDate, presentAbsent, inTime, outTime, attendanceStatus, userId, companyBranch);
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

        public List<HR_EmployeeAttendanceViewModel> GetEmployeeMarkAttendanceList(int employeeId, string attendanceDate, int departmentId, int designationId,string companyBranch)
        {
            List<HR_EmployeeAttendanceViewModel> employeeAttendance = new List<HR_EmployeeAttendanceViewModel>();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtEmployeeAttendance = sqlDbInterface.GetEmployeeAttendanceList(employeeId, attendanceDate, departmentId, designationId, companyBranch);
                if (dtEmployeeAttendance != null && dtEmployeeAttendance.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtEmployeeAttendance.Rows)
                    {
                        employeeAttendance.Add(new HR_EmployeeAttendanceViewModel
                        {
                            EmployeeId = Convert.ToInt32(dr["EmployeeId"]),
                            EmployeeName = Convert.ToString(dr["EmployeeName"]),
                            AttendanceDate = Convert.ToString(dr["AttendanceDate"]),
                            InTime = Convert.ToString(dr["IN_TIME"]),
                            OutTime = Convert.ToString(dr["OUT_TIME"]),
                            AttendanceStatus = Convert.ToString(dr["AttendanceStatus"]),
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
            return employeeAttendance;
        }

        public ResponseOut UpdateEmployeeAttendanceByEmployer(List<HR_EmployeeAttendanceViewModel> employeeAttendanceList, int companyId, int userId)
        {
            ResponseOut responseOut = new ResponseOut();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                foreach (var item in employeeAttendanceList)
                {
                    HR_EmployeeAttendance employeeAttendance = new HR_EmployeeAttendance
                    {
                        EmployeeId = item.EmployeeId,
                        AttendanceDate = Convert.ToDateTime(item.AttendanceDate),
                        AttendanceStatus = item.AttendanceStatus,
                        ModifiedBy = userId,
                        CompanyId = companyId
                    };
                    responseOut = sqlDbInterface.UpdateEmployeeAttendanceByEmployer(employeeAttendance);
                }
                
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


        public List<HR_EmployeeAttendanceViewModel> GetEmployeeAttendanceFormList(int employeeId, string attendanceDate, string employeeType, int departmentId, int designationId)
        {
            List<HR_EmployeeAttendanceViewModel> employeeAttendance = new List<HR_EmployeeAttendanceViewModel>();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtEmployeeAttendance = sqlDbInterface.GetEmployeeAttendanceFormList(employeeId, attendanceDate, employeeType, departmentId, designationId);
                if (dtEmployeeAttendance != null && dtEmployeeAttendance.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtEmployeeAttendance.Rows)
                    {
                        employeeAttendance.Add(new HR_EmployeeAttendanceViewModel
                        {
                            EmployeeId = Convert.ToInt32(dr["EmployeeId"]),
                            EmployeeName = Convert.ToString(dr["EmployeeName"]),
                            AttendanceDate = Convert.ToString(dr["AttendanceDate"]),
                            InTime = Convert.ToString(dr["InTime"]),
                            OutTime = Convert.ToString(dr["OutTime"]),
                            AttendanceStatus = Convert.ToString(dr["AttendanceStatus"]),
                            PresentAbsent = Convert.ToString(dr["PresentAbsent"]),
                            LeaveStatus = Convert.ToString(dr["LeaveStatus"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return employeeAttendance;
        }



        public ResponseOut AddEditEmployeeAttendanceFormByEmployer(List<HR_EmployeeAttendanceViewModel> employeeAttendanceList, int companyId, int userId)
        {
            ResponseOut responseOut = new ResponseOut();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                foreach (var item in employeeAttendanceList)
                {
                    HR_EmployeeAttendance employeeAttendance = new HR_EmployeeAttendance
                    {
                        EmployeeId = item.EmployeeId,
                        AttendanceDate = Convert.ToDateTime(item.AttendanceDate),
                        AttendanceStatus = item.AttendanceStatus,
                        PresentAbsent = item.PresentAbsent,
                        ModifiedBy = userId,
                        CompanyId = companyId,
                        CompanyBranchId= item.companyBranch


                    };
                    responseOut = sqlDbInterface.AddEditEmployeeAttendanceFormByEmployer(employeeAttendance, item.InTime, item.OutTime);
                }

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

        public DataTable GetEmployeeAttendanceReport(int month, int year)
        {
            HRSQLDBInterface hRSQLDBInterface = new HRSQLDBInterface();
            DataTable dtEmployeeAttendance = new DataTable();
            try
            {
                dtEmployeeAttendance = hRSQLDBInterface.GetEmployeeAttendanceReport(month, year);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtEmployeeAttendance;
        }
        public List<HR_EmployeeAttendanceViewModel> GetTempEmployeeAttendanceFormList(int employeeId, string attendanceDate, string employeeType, int departmentId, int designationId)
        {
            List<HR_EmployeeAttendanceViewModel> employeeAttendance = new List<HR_EmployeeAttendanceViewModel>();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtEmployeeAttendance = sqlDbInterface.GetTempEmployeeAttendanceFormList(employeeId, attendanceDate, employeeType, departmentId, designationId);
                if (dtEmployeeAttendance != null && dtEmployeeAttendance.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtEmployeeAttendance.Rows)
                    {
                        employeeAttendance.Add(new HR_EmployeeAttendanceViewModel
                        {
                            EmployeeId = Convert.ToInt32(dr["EmployeeId"]),
                            EmployeeName = Convert.ToString(dr["EmployeeName"]),
                            AttendanceDate = Convert.ToString(dr["AttendanceDate"]),
                            InTime = Convert.ToString(dr["InTime"]),
                            OutTime = Convert.ToString(dr["OutTime"]),
                            AttendanceStatus = Convert.ToString(dr["AttendanceStatus"]),
                            PresentAbsent = Convert.ToString(dr["PresentAbsent"]),
                            LeaveStatus = Convert.ToString(dr["LeaveStatus"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return employeeAttendance;
        }


















    }
}
