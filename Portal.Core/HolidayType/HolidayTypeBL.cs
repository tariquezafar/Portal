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
    public class HolidayTypeBL
    {
        HRMSDBInterface dbInterface;
        public HolidayTypeBL()
        {
            dbInterface = new HRMSDBInterface();
        }
       
        public ResponseOut AddEditHolidayType(HolidayTypeViewModel holidaytypeViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                HR_HolidayType holidaytype = new HR_HolidayType
                {
                    HolidayTypeId = holidaytypeViewModel.HolidayTypeId,
                    HolidayTypeName = holidaytypeViewModel.HolidayTypeName, 
                    Status = holidaytypeViewModel.HolidayType_Status,
                    CompanyBranchId= holidaytypeViewModel.CompanyBranchId
                };
                responseOut = dbInterface.AddEditHolidayType(holidaytype);
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

        public List<HolidayTypeViewModel> GetHolidayTypeList(string holidaytypeName = "", string Status = "",int companyBranchId=0)
        {
            List<HolidayTypeViewModel> holidaytypes = new List<HolidayTypeViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtHolidayTypes = sqlDbInterface.GetHolidayTypeList(holidaytypeName, Status, companyBranchId);
                if (dtHolidayTypes != null && dtHolidayTypes.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtHolidayTypes.Rows)
                    {
                        holidaytypes.Add(new HolidayTypeViewModel
                        {
                            HolidayTypeId = Convert.ToInt32(dr["HolidayTypeId"]),
                            HolidayTypeName = Convert.ToString(dr["HolidayTypeName"]), 
                            HolidayType_Status = Convert.ToBoolean(dr["Status"]),
                            CompanyBranchName=Convert.ToString(dr["CompanyBranchName"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return holidaytypes;
        }

        public HolidayTypeViewModel GetHolidayTypeDetail(int holidaytypeId = 0)
        {
            HolidayTypeViewModel holidaytype = new HolidayTypeViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtHolidayTypes = sqlDbInterface.GetHolidayTypeDetail(holidaytypeId);
                if (dtHolidayTypes != null && dtHolidayTypes.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtHolidayTypes.Rows)
                    {
                        holidaytype = new HolidayTypeViewModel
                        {
                            HolidayTypeId = Convert.ToInt32(dr["HolidayTypeId"]),
                            HolidayTypeName = Convert.ToString(dr["HolidayTypeName"]), 
                            HolidayType_Status = Convert.ToBoolean(dr["Status"]),
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
            return holidaytype;
        }


        public List<HolidayTypeViewModel> GetHolidayTypeIdList(int companyBranchId)
        {
            List<HolidayTypeViewModel> holidaytypes = new List<HolidayTypeViewModel>();
            try
            {
                List<HR_HolidayType> holidaytypeList = dbInterface.GetHolidayTypeIdList(companyBranchId);
                if (holidaytypeList != null && holidaytypeList.Count > 0)
                {
                    foreach (HR_HolidayType holidaytype in holidaytypeList)
                    {
                        holidaytypes.Add(new HolidayTypeViewModel { HolidayTypeId = holidaytype.HolidayTypeId, HolidayTypeName = holidaytype.HolidayTypeName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return holidaytypes;
        }




    }
}
