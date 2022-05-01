using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Portal.Core;
using Portal.Core.ViewModel;
using Portal.Common;
using System.Reflection;
using System.Web.Http;
using System.Data;

namespace Portal.Controllers
{
    public class CommonAPIController : ApiController
    {
        [HttpGet]
        public List<string> GetDepartmentList()
        {
            List<string> depList = new List<string>();
            List<DepartmentViewModel> department = new List<DepartmentViewModel>();
            DepartmentBL departmentBL = new DepartmentBL();
            try
            {
                department = departmentBL.GetDepartmentList(1);

                foreach (var item in department)
                {
                    depList.Add(item.DepartmentName);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), "GetDepartmentList", ex);
            }
            return depList;
        }
        [HttpGet]
        public List<SelectedDepartmentViewModel> GetDepartmentWithIdList()
        {
            List<SelectedDepartmentViewModel> depList = new List<SelectedDepartmentViewModel>();
            List<DepartmentViewModel> department = new List<DepartmentViewModel>();
            DepartmentBL departmentBL = new DepartmentBL();
            try
            {
                department = departmentBL.GetDepartmentList(1);

                foreach (var item in department)
                {
                    depList.Add(new SelectedDepartmentViewModel { DepartmentId = item.DepartmentId, DepartmentName = item.DepartmentName });
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), "GetDepartmentList", ex);
            }
            return depList;
        }

        [HttpGet]
        public List<EmployeeAPIViewModel> GetAllEmployeeList()
        {
            List<EmployeeAPIViewModel> employees = new List<EmployeeAPIViewModel>();
            EmployeeBL employeeBL = new EmployeeBL();
            try
            {
                employees = employeeBL.GetAllEmployeeDetail();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return employees;
        }

        [HttpGet]
        public List<HolidaysViewModel> GetHolidays(int year)
        {
            List<HolidayCalenderViewModel> holidayList = new List<HolidayCalenderViewModel>();
            List<HolidaysViewModel> holidays = new List<HolidaysViewModel>();

            HolidayCalenderBL holidayCalenderBL = new HolidayCalenderBL();
            try
            {
                holidayList = holidayCalenderBL.GetHolidays(year);
                if (holidayList != null && holidayList.Count > 0)
                {
                    foreach (var item in holidayList)
                    {
                        holidays.Add(
                            new HolidaysViewModel
                            {
                                HolidayDate = item.HolidayDate,
                                HolidayDescription = item.HolidayDescription,
                                Message = "SUCCESS",
                                Status = true
                            });
                    }
                }
                else
                {
                    holidays.Add(new HolidaysViewModel { Message = "Fail", Status = false });
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), "GetDepartmentList", ex);
            }
            return holidays;
        }

        [HttpGet]
        public List<EmployeeLeaveDetailViewModel> GetEmployeeLeaveDetails(string fromDate, string toDate)
        {
            DataTable dtEmployeeLeaveDetails = new DataTable();
            List<EmployeeLeaveDetailViewModel> employeeLeaveDetails = new List<EmployeeLeaveDetailViewModel>();

            EmployeeBL employeeBL = new EmployeeBL();
            try
            {
                dtEmployeeLeaveDetails = employeeBL.GetEmployeeLeaveDetails(fromDate, toDate);
                if (dtEmployeeLeaveDetails != null && dtEmployeeLeaveDetails.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtEmployeeLeaveDetails.Rows)
                    {
                        employeeLeaveDetails.Add(new EmployeeLeaveDetailViewModel
                        {
                            EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
                            LeaveAppliedDate = Convert.ToString(dr["LeaveDate"]),
                            LeaveType = Convert.ToString(dr["LeaveTypeName"]),
                            ActualLeavedate = Convert.ToString(dr["PresentAbsent"]),
                            Status = true,
                            Message = "Success"
                        });
                    }
                }
                else
                {
                    employeeLeaveDetails.Add(new EmployeeLeaveDetailViewModel
                    {
                        EmployeeCode = "",
                        LeaveAppliedDate = "",
                        LeaveType = "",
                        ActualLeavedate = "",
                        Status = false,
                        Message = "Fail"
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), "GetDepartmentList", ex);
            }
            return employeeLeaveDetails;
        }
    }

    public class SelectedDepartmentViewModel
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
    }
    public class HolidaysViewModel
    {
        public string HolidayDate { get; set; }
        public string HolidayDescription { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
    }

    public class EmployeeLeaveDetailViewModel
    {
        public string EmployeeCode { get; set; }
        public string LeaveAppliedDate { get; set; }
        public string LeaveType { get; set; }
        public string ActualLeavedate { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
    }
}