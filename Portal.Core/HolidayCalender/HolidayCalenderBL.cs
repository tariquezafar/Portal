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
    public class HolidayCalenderBL
    {
        HRMSDBInterface dbInterface;
        public HolidayCalenderBL()
        {
            dbInterface = new HRMSDBInterface();
        }
        public ResponseOut AddEditHolidayCalender(HolidayCalenderViewModel holidaycalenderViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                HR_HolidayCalender holidaycalender = new HR_HolidayCalender
                {
                    HolidayCalenderId = holidaycalenderViewModel.HolidayCalenderId,
                    HolidayDate = string.IsNullOrEmpty(holidaycalenderViewModel.HolidayDate) ? Convert.ToDateTime("01-01-1900") : Convert.ToDateTime(holidaycalenderViewModel.HolidayDate),
                    HolidayDescription = holidaycalenderViewModel.HolidayDescription,
                    CalenderId = holidaycalenderViewModel.CalenderId,
                    HolidayTypeId = holidaycalenderViewModel.HolidayTypeId,
                    Status = holidaycalenderViewModel.HolidayStatus,
                    CompanyBranchId= holidaycalenderViewModel.CompanyBranchId
                }; 
                responseOut = dbInterface.AddEditHolidayCalender(holidaycalender);
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

        public List<HolidayCalenderViewModel> GetHolidayCalenderList(int calenderId, int holidaytypeId, string fromDate, string toDate, string Status,int companyBranchId)
        {
            List<HolidayCalenderViewModel> holidayCalenders = new List<HolidayCalenderViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtholidayCalenders = sqlDbInterface.GetHolidayCalenderList(calenderId, holidaytypeId,Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), Status, companyBranchId);
                if (dtholidayCalenders != null && dtholidayCalenders.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtholidayCalenders.Rows)
                    {
                        holidayCalenders.Add(new HolidayCalenderViewModel
                        {
                            HolidayCalenderId = Convert.ToInt32(dr["HolidayCalenderId"]),
                            HolidayDate = Convert.ToString(dr["HolidayDate"]),
                            HolidayDescription = Convert.ToString(dr["HolidayDescription"]),
                            CalenderId = Convert.ToInt32(dr["CalenderId"]),
                            CalenderName = Convert.ToString(dr["CalenderName"]),
                            HolidayTypeId = Convert.ToInt32(dr["HolidayTypeId"]),
                            HolidayTypeName = Convert.ToString(dr["HolidayTypeName"]),
                            HolidayStatus = Convert.ToBoolean(dr["Status"]),
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
            return holidayCalenders;
        }
        public HolidayCalenderViewModel GetHolidayCalenderDetail(int holidaycalenderId = 0)
        {
            HolidayCalenderViewModel holidaycalender = new HolidayCalenderViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtHolidayCalenders = sqlDbInterface.GetHolidayCalenderDetail(holidaycalenderId);
                if (dtHolidayCalenders != null && dtHolidayCalenders.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtHolidayCalenders.Rows)
                    {
                        holidaycalender = new HolidayCalenderViewModel
                        {
                            HolidayCalenderId = Convert.ToInt32(dr["HolidayCalenderId"]),
                            HolidayDate = Convert.ToString(dr["HolidayDate"]),
                            HolidayDescription = Convert.ToString(dr["HolidayDescription"]),
                            CalenderId = Convert.ToInt32(dr["CalenderId"]),
                            HolidayTypeId = Convert.ToInt32(dr["HolidayTypeId"]),
                            HolidayStatus = Convert.ToBoolean(dr["Status"]),
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
            return holidaycalender;
        }

        public HolidayCalenderViewModel GetUpComingHolidayDetail()
        {
            HolidayCalenderViewModel holidaycalender = new HolidayCalenderViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtComingHoliday = sqlDbInterface.GetUpComingHolidayDetail();
                if (dtComingHoliday != null && dtComingHoliday.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtComingHoliday.Rows)
                    {
                        holidaycalender = new HolidayCalenderViewModel
                        {
                            HolidayCalenderId = Convert.ToInt32(dr["HolidayCalenderId"]),
                            HolidayDate = Convert.ToString(dr["HolidayDate"]),
                            HolidayDescription = Convert.ToString(dr["HolidayDescription"]),
                            CalenderId = Convert.ToInt32(dr["CalenderId"]),
                            CalenderName=Convert.ToString(dr["CalenderName"]),
                            HolidayTypeId = Convert.ToInt32(dr["HolidayTypeId"]),
                            HolidayTypeName= Convert.ToString(dr["HolidayTypeName"]),
                            HolidayStatus = Convert.ToBoolean(dr["Status"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return holidaycalender;
        }


        /* Method For Get Year Holiday Details For Comman API */
        public List<HolidayCalenderViewModel> GetHolidays(int year)
        {
            List<HolidayCalenderViewModel> holidays = new List<HolidayCalenderViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtHolidays = sqlDbInterface.GetHolidays(year);
                if (dtHolidays != null && dtHolidays.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtHolidays.Rows)
                    {
                        holidays.Add(new HolidayCalenderViewModel
                        {
                            HolidayDate = Convert.ToString(dr["HolidayDate"]),
                            HolidayDescription = Convert.ToString(dr["HolidayDescription"]),

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return holidays;
        }


    }
}
