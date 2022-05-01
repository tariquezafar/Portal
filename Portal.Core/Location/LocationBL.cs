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
    public class LocationBL
    {
        DBInterface dbInterface;
        public LocationBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditLocation(LocationViewModel locationViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            
            try
            {
                Location location = new Location
                {
                    LocationId = locationViewModel.LocationId,
                    LocationName = locationViewModel.LocationName,
                    LocationCode = locationViewModel.LocationCode,
                    IsStoreLocation = locationViewModel.IsStoreLocation,
                    CompanyBranchId = locationViewModel.CompanyBranchId, 
                    CompanyId = locationViewModel.CompanyId,  
                    Status= locationViewModel.LocationStatus,
                    CreatedBy = locationViewModel.CreatedBy

                };
                responseOut = dbInterface.AddEditLocation(location);
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

        public List<LocationViewModel> GetLocationList(string locationName, string locationCode, int companybranchId, int companyId,string locationStatus)
        {
            List<LocationViewModel> locations = new List<LocationViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
               
                DataTable dtSeparationOrders = sqlDbInterface.GetLocationList(locationName, locationCode, companybranchId, companyId, locationStatus);
                if (dtSeparationOrders != null && dtSeparationOrders.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtSeparationOrders.Rows)
                    {
                        locations.Add(new LocationViewModel
                        {
                            LocationId = Convert.ToInt32(dr["LocationId"]),
                            LocationName = Convert.ToString(dr["LocationName"]),
                            LocationCode = Convert.ToString(dr["LocationCode"]),
                            IsStoreLocation = Convert.ToBoolean(dr["IsStoreLocation"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                            CompanyBranchName = Convert.ToString(dr["BranchName"]),
                            LocationStatus = Convert.ToBoolean(dr["Status"]),
                            CreatedByName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return locations;
        }

        public LocationViewModel GetLocationDetail(int locationId = 0)
        {
            LocationViewModel locations = new LocationViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtSeparationOrders = sqlDbInterface.GetLocationDetail(locationId);
                if (dtSeparationOrders != null && dtSeparationOrders.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtSeparationOrders.Rows)
                    {
                        locations = new LocationViewModel
                        {
                            LocationId = Convert.ToInt32(dr["LocationId"]),
                            LocationName = Convert.ToString(dr["LocationName"]),
                            LocationCode = Convert.ToString(dr["LocationCode"]),
                            IsStoreLocation = Convert.ToBoolean(dr["IsStoreLocation"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                            CompanyBranchName = Convert.ToString(dr["BranchName"]),
                            LocationStatus = Convert.ToBoolean(dr["Status"]),
                            CreatedByName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"])

                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return locations;
        }

        public List<LocationViewModel> GetFromLocationList(int companyBranchId)
        {
            List<LocationViewModel> locationViewModel = new List<LocationViewModel>();
            try
            {
                List<Location> locationList = dbInterface.GetFromLocationList(companyBranchId );
                if (locationList != null && locationList.Count > 0)
                {
                    foreach (Location location in locationList)
                    {
                        locationViewModel.Add(new LocationViewModel { LocationId = location.LocationId, LocationName = location.LocationName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return locationViewModel;
        }
    }
}
