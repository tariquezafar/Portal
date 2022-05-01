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
    public class TravelTypeBL
    {
        HRMSDBInterface dbInterface;
        public TravelTypeBL()
        {
            dbInterface = new HRMSDBInterface();
        }
       
        public ResponseOut AddEditTravelType(HR_TravelTypeViewModel traveltypeViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                HR_TravelType traveltype = new HR_TravelType
                {
                    TravelTypeId = traveltypeViewModel.TravelTypeId,
                    TravelTypeName = traveltypeViewModel.TravelTypeName, 
                    Status = traveltypeViewModel.TravelType_Status
                };
                responseOut = dbInterface.AddEditTravelType(traveltype);
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

        public List<HR_TravelTypeViewModel> GetTravelTypeList(string traveltypeName = "", string Status = "")
        {
            List<HR_TravelTypeViewModel> traveltypes = new List<HR_TravelTypeViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtTravelTypes = sqlDbInterface.GetTravelTypeList(traveltypeName, Status);
                if (dtTravelTypes != null && dtTravelTypes.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtTravelTypes.Rows)
                    {
                        traveltypes.Add(new HR_TravelTypeViewModel
                        {
                            TravelTypeId = Convert.ToInt32(dr["TravelTypeId"]),
                            TravelTypeName = Convert.ToString(dr["TravelTypeName"]), 
                            TravelType_Status = Convert.ToBoolean(dr["Status"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return traveltypes;
        }

        public HR_TravelTypeViewModel GetTravelTypeDetail(int traveltypeId = 0)
        {
            HR_TravelTypeViewModel traveltype = new HR_TravelTypeViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtTravelTypes = sqlDbInterface.GetTravelTypeDetail(traveltypeId);
                if (dtTravelTypes != null && dtTravelTypes.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtTravelTypes.Rows)
                    {
                        traveltype = new HR_TravelTypeViewModel
                        {  
                            TravelTypeId = Convert.ToInt32(dr["TravelTypeId"]),
                            TravelTypeName = Convert.ToString(dr["TravelTypeName"]),
                            TravelType_Status = Convert.ToBoolean(dr["Status"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return traveltype;
        }


        public List<HR_TravelTypeViewModel> GetTravelTypeForEmpolyeeTravelAppList()
        {
            List<HR_TravelTypeViewModel> travelapplications = new List<HR_TravelTypeViewModel>();
            try
            {
                List<HR_TravelType> travelList = dbInterface.GetTravelTypeForEmpolyeeTravelAppList();
                if (travelList != null && travelList.Count > 0)
                {
                    foreach (HR_TravelType travel in travelList)
                    {
                        travelapplications.Add(new HR_TravelTypeViewModel { TravelTypeId = travel.TravelTypeId, TravelTypeName = travel.TravelTypeName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return travelapplications;
        }

        public List<HR_EmployeeTravelApplicationViewModel> GetEmployeeTravelApplicationDetails(int companyId, int userId, int reportingUserId, int reportingRoleId)
        {

            List<HR_EmployeeTravelApplicationViewModel> travelApplicationList = new List<HR_EmployeeTravelApplicationViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtList = sqlDbInterface.GetEmployeeTravelApplicationDetails(companyId, userId, reportingUserId, reportingRoleId);
                if (dtList != null && dtList.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtList.Rows)
                    {
                        travelApplicationList.Add(new HR_EmployeeTravelApplicationViewModel
                        {
                            ApplicationId = Convert.ToInt32(dr["ApplicationId"]),
                            ApplicationNo = Convert.ToString(dr["ApplicationNo"]),
                            ApplicationDate = Convert.ToString(dr["ApplicationDate"]),
                            TravelTypeName = Convert.ToString(dr["TravelTypeName"]),
                            TravelReason = Convert.ToString(dr["TravelReason"]),
                            TravelStatus = Convert.ToString(dr["TravelStatus"]),
                            TravelDestination = Convert.ToString(dr["TravelDestination"]),
                            TravelStartDate = Convert.ToString(dr["TravelStartDate"]),
                            TravelEndDate = Convert.ToString(dr["TravelEndDate"]),
                            EmployeeId = Convert.ToInt32(dr["EmployeeId"]),
                            EmployeeName = Convert.ToString(dr["EmployeeName"])
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return travelApplicationList;
        }

    }
}
