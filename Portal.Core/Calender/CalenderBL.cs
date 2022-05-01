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
    public class CalenderBL
    {
        HRMSDBInterface dbInterface;
        public CalenderBL()
        {
            dbInterface = new HRMSDBInterface();
        }
       
        public ResponseOut AddEditCalender(CalenderViewModel calenderViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                HR_Calender calender = new HR_Calender
                {
                    CalenderId = calenderViewModel.CalenderId,
                    CalenderName = calenderViewModel.CalenderName,
                    CalenderYear = calenderViewModel.CalenderYear,
                    CreatedBy = calenderViewModel.CreatedBy,
                    Status = calenderViewModel.Calender_Status
                };
                responseOut = dbInterface.AddEditCalender(calender);
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

        public List<CalenderViewModel> GetCalenderList(string calenderName = "", int calenderYear = 0, string Status = "")
        {
            List<CalenderViewModel> calenders = new List<CalenderViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCalenders = sqlDbInterface.GetCalenderList(calenderName, calenderYear, Status);
                if (dtCalenders != null && dtCalenders.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCalenders.Rows)
                    {
                        calenders.Add(new CalenderViewModel
                        {
                            CalenderId = Convert.ToInt32(dr["CalenderId"]),
                            CalenderName = Convert.ToString(dr["CalenderName"]),
                            CalenderYear = Convert.ToInt32(dr["CalenderYear"]),
                            CreatedBy = Convert.ToInt32(dr["CreatedBy"].ToString() == "" ? "0" : dr["CreatedBy"].ToString()),
                            CreatedName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedBy = Convert.ToInt32(dr["ModifiedBy"].ToString() == "" ? "0" : dr["ModifiedBy"].ToString()),
                            ModifiedName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            Calender_Status = Convert.ToBoolean(dr["Status"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return calenders;
        }

        public CalenderViewModel GetCalenderDetail(int calenderId = 0)
        {
            CalenderViewModel calenders = new CalenderViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCalenders = sqlDbInterface.GetCalenderDetail(calenderId);
                if (dtCalenders != null && dtCalenders.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCalenders.Rows)
                    {
                        calenders = new CalenderViewModel
                        {
                            CalenderId = Convert.ToInt32(dr["CalenderId"]),
                            CalenderName = Convert.ToString(dr["CalenderName"]),
                            CalenderYear = Convert.ToInt32(dr["CalenderYear"]),
                            CreatedBy = Convert.ToInt32(dr["CreatedBy"].ToString() == "" ? "0" : dr["CreatedBy"].ToString()), 
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedBy = Convert.ToInt32(dr["ModifiedBy"].ToString() == "" ? "0" : dr["ModifiedBy"].ToString()), 
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            Calender_Status = Convert.ToBoolean(dr["Status"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return calenders;
        }


        public List<CalenderViewModel> GetCalenderList()
        {
            List<CalenderViewModel> calenders = new List<CalenderViewModel>();
            try
            {
                List<HR_Calender> calenderList = dbInterface.GetCalenderList();
                if (calenderList != null && calenderList.Count > 0)
                {
                    foreach (HR_Calender calender in calenderList)
                    {
                        calenders.Add(new CalenderViewModel {CalenderId = calender.CalenderId, CalenderName = calender.CalenderName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return calenders;
        }


    }
}
