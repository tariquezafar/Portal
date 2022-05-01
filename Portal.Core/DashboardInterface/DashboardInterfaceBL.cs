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

    public class DashboardInterfaceBL
    {
        DBInterface dbInterface;

        public DashboardInterfaceBL()
        {
            dbInterface = new DBInterface();
        }

        
        public DashboardInterfaceViewModel GetDashboardInterfaceDetail(int itemId = 0)
        {
            DashboardInterfaceViewModel dashboardInterface = new DashboardInterfaceViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtProductSubGroups = sqlDbInterface.GetDashboardInterfaceDetail(itemId);
                if (dtProductSubGroups != null && dtProductSubGroups.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtProductSubGroups.Rows)
                    {
                        dashboardInterface = new DashboardInterfaceViewModel
                        {
                            ItemId = Convert.ToInt32(dr["ItemId"]),
                            ItemName = Convert.ToString(dr["ItemName"]),
                            ItemDescription = Convert.ToString(dr["ItemDescription"]),
                            ModuleName = Convert.ToString(dr["ModuleName"]),
                            ContainerNo = Convert.ToString(dr["ContainerNo"]),
                            ContainerName = Convert.ToString(dr["ContainerName"]),
                            Status = Convert.ToBoolean(dr["Status"]),
                            CompanyBranchId = Convert.ToInt16(dr["CompanyBranchId"]),
                            SequenceNo = Convert.ToInt16(dr["SequenceNo"])


                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dashboardInterface;
        }

      

        public ResponseOut AddEditDashboardInterface(DashboardInterfaceViewModel dashboardInterfaceViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sQLDbInterface = new SQLDbInterface();
            try
            {
                DashboardInterface dashboardInterface = new DashboardInterface
                {
                    ItemId= dashboardInterfaceViewModel.ItemId,
                    ItemName= dashboardInterfaceViewModel.ItemName,
                    ItemDescription= dashboardInterfaceViewModel.ItemDescription,
                    ModuleName= dashboardInterfaceViewModel.ModuleName,
                    ContainerNo= dashboardInterfaceViewModel.ContainerNo,
                    ContainerName= dashboardInterfaceViewModel.ContainerName,
                    Status= dashboardInterfaceViewModel.Status,
                    CompanyBranchId= dashboardInterfaceViewModel.CompanyBranchId,
                    SequenceNo= dashboardInterfaceViewModel.SequenceNo

                 

                };
                responseOut = sQLDbInterface.AddEditDashboardInterface(dashboardInterface);
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

        public List<DashboardInterfaceViewModel> GetDashboardInterfaceList(string itemName, string moduleName, string containerName, string status, string companyBranchId)
        {
            List<DashboardInterfaceViewModel> dashboardInterfaces = new List<DashboardInterfaceViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtDashboardInterface = sqlDbInterface.GetDashboardInterfaceList(itemName,moduleName,containerName,status, companyBranchId);
                if (dtDashboardInterface != null && dtDashboardInterface.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtDashboardInterface.Rows)
                    {
                        dashboardInterfaces.Add(new DashboardInterfaceViewModel
                        {
                            ItemId = Convert.ToInt32(dr["ItemId"]),
                            ItemName = Convert.ToString(dr["ItemName"]),
                            ItemDescription = Convert.ToString(dr["ItemDescription"]),
                            ModuleName = Convert.ToString(dr["ModuleName"]),
                            ContainerNo = Convert.ToString(dr["ContainerNo"]),
                            ContainerName = Convert.ToString(dr["ContainerName"]),
                            CompanyBranchName = Convert.ToString(dr["BranchName"]),
                            Status = Convert.ToBoolean(dr["Status"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dashboardInterfaces;
        }
    }
}
