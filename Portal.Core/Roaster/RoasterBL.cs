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
    public class RoasterBL
    {
        HRMSDBInterface dbInterface;
        public RoasterBL()
        {
            dbInterface = new HRMSDBInterface();
        }
       
        public ResponseOut AddEditRoaster(HR_RoasterViewModel roasterViewModel, List<HR_RoasterWeekViewModel> weeks)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            ResponseOut responseOut = new ResponseOut();
            try
            {
                HR_Roaster roaster = new HR_Roaster
                {
                    RoasterId = roasterViewModel.RoasterId,
                    RoasterName = roasterViewModel.RoasterName, 
                    RoasterDesc = roasterViewModel.RoasterDesc, 
                    RoasterStartDate = string.IsNullOrEmpty(roasterViewModel.RoasterStartDate) ? Convert.ToDateTime("01-01-1900") : Convert.ToDateTime(roasterViewModel.RoasterStartDate),
                    RoasterType = roasterViewModel.RoasterType,
                    Remarks = roasterViewModel.Remarks,
                    CompanyId = roasterViewModel.CompanyId,
                    DepartmentId = roasterViewModel.DepartmentId,
                    NoOfWeeks = roasterViewModel.NoOfWeeks,
                    CreatedBy = roasterViewModel.CreatedBy,
                    Status = roasterViewModel.RoasterStatus,
                    CompanyBranchId= roasterViewModel.CompanyBranchId
                };
                List<HR_RoasterWeek> weekList = new List<HR_RoasterWeek>();
                if (weeks != null && weeks.Count > 0)
                {
                    foreach (HR_RoasterWeekViewModel item in weeks)
                    {
                        weekList.Add(new HR_RoasterWeek
                        {
                            WeekNo = item.WeekNo,
                            Mon_ShiftId=item.Mon_ShiftId,
                            Tue_ShiftId = item.Tue_ShiftId,
                            Wed_ShiftId = item.Wed_ShiftId,
                            Thu_ShiftId = item.Thu_ShiftId,
                            Fri_ShiftId = item.Fri_ShiftId,
                            Sat_ShiftId = item.Sat_ShiftId,
                            Sun_ShiftId = item.Sun_ShiftId
                        });
                    }
                }


                responseOut = sqlDbInterface.AddEditRoaster(roaster,weekList);
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

        public List<HR_RoasterViewModel> GetRoasterList(string roasterName = "", string fromDate="", string toDate="", string roasterType = "", int department = 0,int companyId = 0, string Status = "",string companyBranch="")
        {
            List<HR_RoasterViewModel> roasters = new List<HR_RoasterViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtRoasters = sqlDbInterface.GetRoasterList(roasterName, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), roasterType, department, companyId, Status, companyBranch);
                if (dtRoasters != null && dtRoasters.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtRoasters.Rows)
                    {
                        roasters.Add(new HR_RoasterViewModel
                        {
                            RoasterId = Convert.ToInt32(dr["RoasterId"]),
                            RoasterName = Convert.ToString(dr["RoasterName"]),
                            RoasterDesc = Convert.ToString(dr["RoasterDesc"]),
                            RoasterStartDate = Convert.ToString(dr["RoasterStartDate"]),
                            RoasterType = Convert.ToString(dr["RoasterType"]),
                            DepartmentId = Convert.ToInt32(dr["DepartmentId"]),
                            DepartmentName = Convert.ToString(dr["DepartmentName"]), 
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            CreatedByName =Convert.ToString(dr["CreatedByName"]),
                            NoOfWeeks = Convert.ToInt32(dr["NoOfWeeks"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            ModifiedByName = Convert.ToString(dr["ModifiedByName"]),
                            Remarks = Convert.ToString(dr["Remarks"]),
                            RoasterStatus = Convert.ToBoolean(dr["Status"]),
                            CompanyBranchName = Convert.ToString(dr["BranchName"])

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return roasters;
        }

        public HR_RoasterViewModel GetRoasterDetail(int roasterId = 0)
        {
            HR_RoasterViewModel roaster = new HR_RoasterViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtRoasters = sqlDbInterface.GetRoasterDetail(roasterId);
                if (dtRoasters != null && dtRoasters.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtRoasters.Rows)
                    {
                        roaster = new HR_RoasterViewModel
                        {
                            RoasterId = Convert.ToInt32(dr["RoasterId"]),
                            RoasterName = Convert.ToString(dr["RoasterName"]),
                            RoasterDesc = Convert.ToString(dr["RoasterDesc"]),
                            RoasterStartDate = Convert.ToString(dr["RoasterStartDate"]),
                            RoasterEndDate = Convert.ToString(dr["RoasterEndDate"]),
                            DepartmentName = Convert.ToString(dr["DepartmentName"]),
                            RoasterType = Convert.ToString(dr["RoasterType"]),
                            DepartmentId = Convert.ToInt32(dr["DepartmentId"]),
                            NoOfWeeks = Convert.ToInt32(dr["NoOfWeeks"]),
                            Remarks = Convert.ToString(dr["Remarks"]),
                            RoasterStatus = Convert.ToBoolean(dr["Status"]),
                            CompanyBranchId=Convert.ToInt32(dr["CompanyBranchId"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return roaster;
        }
        public List<HR_RoasterWeekViewModel> GetRoasterWeekList(int roasterId)
        {
            List<HR_RoasterWeekViewModel> weeks = new List<HR_RoasterWeekViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtWeeks = sqlDbInterface.GetRoasterWeekList(roasterId);
                if (dtWeeks != null && dtWeeks.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtWeeks.Rows)
                    {
                        weeks.Add(new HR_RoasterWeekViewModel
                        {
                            WeekSequenceNo = Convert.ToInt16(dr["WeekSequenceNo"]),
                            RoasterWeekId = Convert.ToInt32(dr["RoasterWeekId"]),
                            WeekNo = Convert.ToInt16(dr["WeekNo"]),
                            Mon_ShiftId = Convert.ToInt32(dr["Mon_ShiftId"]),
                            Mon_ShiftName = Convert.ToString(dr["Mon_ShiftName"]),
                            Tue_ShiftId = Convert.ToInt32(dr["Tue_ShiftId"]),
                            Tue_ShiftName = Convert.ToString(dr["Tue_ShiftName"]),
                            Wed_ShiftId = Convert.ToInt32(dr["Wed_ShiftId"]),
                            Wed_ShiftName = Convert.ToString(dr["Wed_ShiftName"]),
                            Thu_ShiftId = Convert.ToInt32(dr["Thu_ShiftId"]),
                            Thu_ShiftName = Convert.ToString(dr["Thu_ShiftName"]),
                            Fri_ShiftId = Convert.ToInt32(dr["Fri_ShiftId"]),
                            Fri_ShiftName = Convert.ToString(dr["Fri_ShiftName"]),
                            Sat_ShiftId = Convert.ToInt32(dr["Sat_ShiftId"]),
                            Sat_ShiftName = Convert.ToString(dr["Sat_ShiftName"]),
                            Sun_ShiftId = Convert.ToInt32(dr["Sun_ShiftId"]),
                            Sun_ShiftName = Convert.ToString(dr["Sun_ShiftName"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return weeks;
        }

        public List<HR_RoasterViewModel> GetRosterList(int companyId)
        {
            List<HR_RoasterViewModel> rosters = new List<HR_RoasterViewModel>();
            try
            {
                List<HR_Roaster> rosterList = dbInterface.GetRosterList(companyId);
                if (rosterList != null && rosterList.Count > 0)
                {
                    foreach (HR_Roaster roster in rosterList)
                    {
                        rosters.Add(new HR_RoasterViewModel { RoasterId = roster.RoasterId, RoasterName= roster.RoasterName});
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return rosters;
        }
        public List<EmployeeViewModel> GetRosterLinkedEmployeeDetail(int roasterId = 0)
        {
            List<EmployeeViewModel> employees = new List<EmployeeViewModel>();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtEmployees = sqlDbInterface.GetRoasterLinkedEmployeeDetail(roasterId);
                if (dtEmployees != null && dtEmployees.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtEmployees.Rows)
                    {
                        employees.Add(new EmployeeViewModel
                        {
                            EmployeeId = Convert.ToInt32(dr["EmployeeId"]),
                            EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
                            FirstName = Convert.ToString(dr["FirstName"]),
                            LastName = Convert.ToString(dr["LastName"]),
                            DesignationName = Convert.ToString(dr["DesignationName"]),
                           
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return employees;
        }
        public ResponseOut UpdateEmployeeRosterShift(int rosterId, int shiftId, string startDate, string endDate, int createdBy,int companyBranchId, List<HR_EmployeeRosterViewModel> employeeRosters)
        {
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            ResponseOut responseOut = new ResponseOut();
            try
            {
                List<HR_EmployeeRoster> employeeRosterList = new List<HR_EmployeeRoster>();
                if (employeeRosters != null && employeeRosters.Count > 0)
                {
                    foreach (HR_EmployeeRosterViewModel item in employeeRosters)
                    {
                        employeeRosterList.Add(new HR_EmployeeRoster
                        {
                            EmployeeId=item.EmployeeId
                        });
                    }
                }


                responseOut = sqlDbInterface.UpdateEmployeeRosterShift(rosterId,shiftId,Convert.ToDateTime(startDate),Convert.ToDateTime(endDate),createdBy, companyBranchId,employeeRosterList);
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

        
        public List<HR_RoasterViewModel> GetESSEmployeeRoasterList(int employeeId=0)
        {
            List<HR_RoasterViewModel> roasters = new List<HR_RoasterViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtRoasters = sqlDbInterface.GetESSEmployeeRoasterList(employeeId);
                if (dtRoasters != null && dtRoasters.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtRoasters.Rows)
                    {
                        roasters.Add(new HR_RoasterViewModel
                        {
                            RoasterStartDate = Convert.ToString(dr["RosterDate"]),
                            RoasterDay   = Convert.ToString(dr["RosterDay"]),
                            ShiftName =Convert.ToString(dr["ShiftName"]),
                            ShiftDescription = Convert.ToString(dr["ShiftDescription"]),
                            ShiftStartTime = Convert.ToString(dr["ShiftStartTime"]),
                            ShiftEndTime = Convert.ToString(dr["ShiftEndTime"]),
                            FullDayWorkHour = Convert.ToDecimal(dr["FullDayWorkHour"]),
                            HalfDayWorkHour = Convert.ToDecimal(dr["HalfDayWorkHour"]),
                            ShiftTypeName = Convert.ToString(dr["ShiftTypeName"]),
                            RoasterStatus = Convert.ToBoolean(dr["Status"])

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return roasters;
        }
    }
}
