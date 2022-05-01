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
   public class ServiceBL
    {
        DBInterface dbInterface;
        public ServiceBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditService(ServicViewModel servicViewModel,  List<ServiceViewModel> servicesViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                Service1 service1 = new Service1
                {
                    ServiceId = servicViewModel.ServiceId,
                    ServiceDate = Convert.ToDateTime(servicViewModel.ServiceDate),
                    ApprovalStatus= servicViewModel.ApprovalStatus,
                    CreatedBy = servicViewModel.CreatedBy,
                    CompanyId = servicViewModel.CompanyId,
                };

                List<ServiceItem> serviceItemList = new List<ServiceItem>();

                if (servicesViewModel != null && servicesViewModel.Count > 0)
                {
                    foreach (ServiceViewModel item in servicesViewModel)
                    {
                        serviceItemList.Add(new ServiceItem
                        {
                            ServiceId =0,
                            ProductID = item.ProductId,
                            ProductTypeID = item.ProductTypeID,
                            ServiceItemName = item.ServiceItemName,
                            Notes = item.Notes,
                           

                        });
                    }
                }
                responseOut = sqlDbInterface.AddEditService(service1,serviceItemList);
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

        public List<ServicViewModel> GetServiceList(string serviceNo, string approvalStatus, string fromDate, string toDate, int companyId)
        {
            List<ServicViewModel> servicViewModelList = new List<ServicViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtservices = sqlDbInterface.GetServiceList(serviceNo,approvalStatus,Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate),companyId);
                if (dtservices != null && dtservices.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtservices.Rows)
                    {
                        servicViewModelList.Add(new ServicViewModel
                        {
                            ServiceId = Convert.ToInt32(dr["ServiceId"]),
                            ServiceNo = Convert.ToString(dr["ServiceNo"]),
                            ServiceDate = Convert.ToString(dr["ServiceDate"]),
                            ApprovalStatus = Convert.ToString(dr["ApprovalStatus"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return servicViewModelList;
        }

        public ServicViewModel GetServiceDetail(long serviceId = 0)
        {
            ServicViewModel servicViewModel = new ServicViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtServices = sqlDbInterface.GetServiceDetail(serviceId);
                if (dtServices != null && dtServices.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtServices.Rows)
                    {
                        servicViewModel = new ServicViewModel
                        {
                            ServiceId = Convert.ToInt32(dr["ServiceId"]),
                            ServiceNo = Convert.ToString(dr["ServiceNo"]),
                            ServiceDate = Convert.ToString(dr["ServiceDate"]),
                            ApprovalStatus = Convert.ToString(dr["ApprovalStatus"]),
                            CreatedByUserName = Convert.ToString(dr["UserName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return servicViewModel;
        }
        public List<ServiceViewModel> GetServiceProductList(long serviceId)
        {
            List<ServiceViewModel> serviceViewModelList = new List<ServiceViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCustomers = sqlDbInterface.GetServiceProductList(serviceId);
                if (dtCustomers != null && dtCustomers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCustomers.Rows)
                    {
                        serviceViewModelList.Add(new ServiceViewModel
                        {
                            ServiceItemId = Convert.ToInt32(dr["ServiceItemId"]),
                            SequenceNo = Convert.ToInt32(dr["SequenceNo"]),
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ServiceItemName = Convert.ToString(dr["ServiceItemName"]),
                            Notes = Convert.ToString(dr["Notes"]),
                            ProductTypeID = Convert.ToInt32(dr["ProductTypeID"]),

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return serviceViewModelList;
        }
    }
}
