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
   public class ServicesBL
    {
        DBInterface dbInterface;
        public ServicesBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditServices(ServicesViewModel servicesViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                Service serviceId = new Service
                {
                    ServicesId = servicesViewModel.ServicesId,
                    ServicesName = servicesViewModel.ServicesName,
                    Status = servicesViewModel.Services_Status,
                };
                responseOut = dbInterface.AddEditServices(serviceId);
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

        public List<ServicesViewModel> GetServicesList(string servicesName = "", string Status = "")
        {
            List<ServicesViewModel> servicesList = new List<ServicesViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtservices = sqlDbInterface.GetServicesList(servicesName, Status);
                if (dtservices != null && dtservices.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtservices.Rows)
                    {
                        servicesList.Add(new ServicesViewModel
                        {
                            ServicesId = Convert.ToInt32(dr["ServicesId"]),
                            ServicesName = Convert.ToString(dr["ServicesName"]),
                            Services_Status = Convert.ToBoolean(dr["Status"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return servicesList;
        }

        public ServicesViewModel GetServicesDetail(int servicesId = 0)
        {
            ServicesViewModel services = new ServicesViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtServices = sqlDbInterface.GetServicesDetail(servicesId);
                if (dtServices != null && dtServices.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtServices.Rows)
                    {
                        services = new ServicesViewModel
                        {
                            ServicesId = Convert.ToInt32(dr["ServicesId"]),
                            ServicesName = Convert.ToString(dr["ServicesName"]),
                            Services_Status = Convert.ToBoolean(dr["Status"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return services;
        }

    }
}
