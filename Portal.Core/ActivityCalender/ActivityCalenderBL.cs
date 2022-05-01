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
    public class ActivityCalenderBL
    {
        HRMSDBInterface dbInterface;
        public ActivityCalenderBL()
        {
            dbInterface = new HRMSDBInterface();
        }
        public ResponseOut AddEditActivityCalender(ActivityCalenderViewModel activitycalenderViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
               HR_ActivityCalender activitycalender = new HR_ActivityCalender
               {
                    ActivityCalenderId = activitycalenderViewModel.ActivityCalenderId,
                    ActivityDate = string.IsNullOrEmpty(activitycalenderViewModel.ActivityDate) ? Convert.ToDateTime("01-01-1900") : Convert.ToDateTime(activitycalenderViewModel.ActivityDate), 
                    ActivityDescription = activitycalenderViewModel.ActivityDescription,
                    CalenderId = activitycalenderViewModel.CalenderId, 
                    Status= activitycalenderViewModel.ActivityStatus,
                    CompanyBranchId=activitycalenderViewModel.CompanyBranchId
                };
                responseOut = dbInterface.AddEditActivityCalender(activitycalender);
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

        public List<ActivityCalenderViewModel> GetActivityCalenderList( int calenderId, string fromDate, string toDate, string Status,int companyBranchId)
        {
            List<ActivityCalenderViewModel> activityCalenders = new List<ActivityCalenderViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtactivityCalenders = sqlDbInterface.GetActivityCalenderList( calenderId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), Status, companyBranchId);
                if (dtactivityCalenders != null && dtactivityCalenders.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtactivityCalenders.Rows)
                    {
                        activityCalenders.Add(new ActivityCalenderViewModel
                        {
                            ActivityCalenderId = Convert.ToInt32(dr["ActivityCalenderId"]), 
                            ActivityDate = Convert.ToString(dr["ActivityDate"]),
                            ActivityDescription = Convert.ToString(dr["ActivityDescription"]),
                            CalenderId = Convert.ToInt32(dr["CalenderId"]),
                            CalenderName = Convert.ToString(dr["CalenderName"]), 
                            ActivityStatus = Convert.ToBoolean(dr["Status"]),
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
            return activityCalenders;
        }
        public ActivityCalenderViewModel GetActivityCalenderDetail(int acitvitycalenderId = 0)
        {
            ActivityCalenderViewModel activitycalender = new ActivityCalenderViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtActivityCalenders = sqlDbInterface.GetActivityCalenderDetail(acitvitycalenderId);
                if (dtActivityCalenders != null && dtActivityCalenders.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtActivityCalenders.Rows)
                    {
                        activitycalender = new ActivityCalenderViewModel
                        {
                            ActivityCalenderId = Convert.ToInt32(dr["ActivityCalenderId"]),
                            ActivityDate = Convert.ToString(dr["ActivityDate"]),
                            ActivityDescription = Convert.ToString(dr["ActivityDescription"]),
                            CalenderId = Convert.ToInt32(dr["CalenderId"]), 
                            ActivityStatus = Convert.ToBoolean(dr["Status"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return activitycalender;
        }


        public ActivityCalenderViewModel GetUpcomingActivityDetail(int acitvitycalenderId = 0)
        {
            ActivityCalenderViewModel activitycalender = new ActivityCalenderViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtActivityCalenders = sqlDbInterface.GetUpcomingActivityDetail();
                if (dtActivityCalenders != null && dtActivityCalenders.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtActivityCalenders.Rows)
                    {
                        activitycalender = new ActivityCalenderViewModel
                        {
                            ActivityCalenderId = Convert.ToInt32(dr["ActivityCalenderId"]),
                            ActivityDate = Convert.ToString(dr["ActivityDate"]),
                            ActivityDescription = Convert.ToString(dr["ActivityDescription"]),
                            CalenderId = Convert.ToInt32(dr["CalenderId"]),
                            CalenderName=Convert.ToString(dr["CalenderName"]),
                            ActivityStatus = Convert.ToBoolean(dr["Status"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return activitycalender;
        }


        public List<HolidayActivityCalenderViewModel> GetHolidayandActivityDetails()
        {
            List<HolidayActivityCalenderViewModel> HolidayandActivity = new List<HolidayActivityCalenderViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtHolidayandActivity = sqlDbInterface.GetCalenderDetail();
                if (dtHolidayandActivity != null && dtHolidayandActivity.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtHolidayandActivity.Rows)
                    {
                        HolidayandActivity.Add(new HolidayActivityCalenderViewModel
                        {
                            start = Convert.ToDateTime(dr["start"]).ToString("yyyy-MM-dd"), // + "T00:00:00ToString("dd-MMM-yyyy"),
                            end = Convert.ToDateTime(dr["start"]).ToString("yyyy-MM-dd"), // + "T00:00:00"
                            title = Convert.ToString(dr["title"]),
                            id = Convert.ToInt32(dr["id"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return HolidayandActivity;
        }
    }
}
