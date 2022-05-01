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

namespace Portal.Core
{
    public class ScheduleBL
    {
        DBInterface dbInterface;
        public ScheduleBL()
        {
            dbInterface = new DBInterface();
        }


        public ResponseOut AddEditSchedule(ScheduleViewModel scheduleViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {

              
              Schedule schedule = new Schedule
              {
                    ScheduleId = scheduleViewModel.ScheduleId,
                    ScheduleName = scheduleViewModel.ScheduleName,
                    ScheduleNo = scheduleViewModel.ScheduleNo,
                    CompanyId = scheduleViewModel.CompanyId,                  
                    CreatedBy = scheduleViewModel.CreatedBy,
                    Status = scheduleViewModel.Schedule_Status                                                  
                };



                responseOut = dbInterface.AddEditSchedule(schedule);
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



        public List<ScheduleViewModel> GetScheduleList(string scheduleName = "", int scheduleNo = 0, int companyId=0, string status = "")
        {
            List<ScheduleViewModel> scheduleViewModel = new List<ScheduleViewModel>();      
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            { 
                DataTable dtSchedules = sqlDbInterface.GetScheduleList(scheduleName, scheduleNo, companyId, status);
                if (dtSchedules != null && dtSchedules.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtSchedules.Rows)
                    {
                        scheduleViewModel.Add(new ScheduleViewModel
                        {
                            ScheduleId = Convert.ToInt32(dr["ScheduleId"]),
                            ScheduleName = Convert.ToString(dr["ScheduleName"]),
                            ScheduleNo = Convert.ToInt32(dr["ScheduleNo"]),
                            Schedule_Status = Convert.ToBoolean(dr["Status"]),
                            CreatedBy = Convert.ToInt32(dr["CreatedBy"].ToString() == "" ? "0" : dr["CreatedBy"].ToString()),
                            CreatedName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedBy = Convert.ToInt32(dr["ModifiedBy"].ToString() == "" ? "0" : dr["ModifiedBy"].ToString()),
                            ModifiedName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
             return scheduleViewModel;
        }

        public ScheduleViewModel GetScheduleDetail(int scheduleId = 0)
        {

            ScheduleViewModel scheduleViewModel = new ScheduleViewModel();
          
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtSchedules = sqlDbInterface.GetScheduleDetail(scheduleId);
                if (dtSchedules != null && dtSchedules.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtSchedules.Rows)
                    {
                        scheduleViewModel = new ScheduleViewModel
                        {
                            ScheduleId = Convert.ToInt32(dr["ScheduleId"]),
                            ScheduleName = Convert.ToString(dr["ScheduleName"]),
                            ScheduleNo = Convert.ToInt32(dr["ScheduleNo"]),
                            Schedule_Status = Convert.ToBoolean(dr["Status"]),
                            CreatedBy = Convert.ToInt32(dr["CreatedBy"].ToString() == "" ? "0" : dr["CreatedBy"].ToString()),
                            CreatedName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedBy = Convert.ToInt32(dr["ModifiedBy"].ToString() == "" ? "0" : dr["ModifiedBy"].ToString()),
                            ModifiedName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return scheduleViewModel;
        }

        public List<ScheduleViewModel> GetGLScheduleList()
        {
            List<ScheduleViewModel> schedules = new List<ScheduleViewModel>();
            try
            {
                List<Schedule> scheduleList = dbInterface.GetGLScheduleList();
                if (scheduleList != null && scheduleList.Count > 0)
                {
                    foreach (Schedule schedule in scheduleList)
                    {
                        schedules.Add(new ScheduleViewModel {ScheduleId = schedule.ScheduleId, ScheduleName = schedule.ScheduleName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return schedules;
        }




    }
}
