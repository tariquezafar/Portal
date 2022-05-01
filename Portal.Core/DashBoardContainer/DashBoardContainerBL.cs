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
    public class DashBoardContainerBL
    {

        DBInterface dbInterface;
        public DashBoardContainerBL()
        {
            dbInterface = new DBInterface();
        }

        public ResponseOut AddEditDashboardContainer(DashboardContainerViewModel  dashboardContainerViewModel )
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {

                DashboardContainer dashboardContainer = new DashboardContainer
                {
                    DashboardContainterID = dashboardContainerViewModel.DashboardContainterID,
                    ContainerName = dashboardContainerViewModel.ContainerName,
                    ContainerDisplayName = dashboardContainerViewModel.ContainerDisplayName ,
                    ContainterNo = dashboardContainerViewModel.ContainterNo ,
                    TotalItem = dashboardContainerViewModel.TotalItem,                   
                    ModuleName = dashboardContainerViewModel.ModuleName                  
                   
                    
                };
                responseOut = dbInterface.AddEditDashboardContainer(dashboardContainer);
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

        

        public List<DashboardContainerViewModel> GetDashboardContainerList(string dashboardContainerName = "",string dashboardContainerdisplayName = "",int dashboardcontainerNo = 0,int totalItem = 0,string module = "")            
        {
            List<DashboardContainerViewModel> dashboardContainers = new List<DashboardContainerViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtDashboardContainer = sqlDbInterface.GetDashboardContainerList(dashboardContainerName,
                    dashboardContainerdisplayName, dashboardcontainerNo, totalItem, module);

                    if (dtDashboardContainer != null && dtDashboardContainer.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtDashboardContainer.Rows)
                    {
                        dashboardContainers.Add (new DashboardContainerViewModel
                        {                            
                            DashboardContainterID = Convert.ToInt64(dr["DashboardContainterID"]),
                            ContainerName = Convert.ToString(dr["ContainerName"]),
                            ContainerDisplayName = Convert.ToString(dr["ContainerDisplayName"]),
                            ContainterNo = Convert.ToInt32(dr["ContainterNo"]),
                            TotalItem = Convert.ToInt32(dr["TotalItem"]),
                            ModuleName = Convert.ToString(dr["ModuleName"])
                            
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dashboardContainers;
                
            }


        public DashboardContainerViewModel GetDashboardContainerDetail(int dashboardContainerId)
        {

            DashboardContainerViewModel dashboardContainer = new DashboardContainerViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();

            try
            {
                DataTable dtDashboardContainer = sqlDbInterface.GetDashboardContainerDetail(dashboardContainerId);
                
                if (dtDashboardContainer != null && dtDashboardContainer.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtDashboardContainer.Rows)
                    {
                       
                        dashboardContainer = new DashboardContainerViewModel
                        {
                                                      
                            DashboardContainterID = Convert.ToInt32(dr["DashboardContainterID"]),
                            ContainerName = Convert.ToString(dr["ContainerName"]),
                            ContainerDisplayName = Convert.ToString(dr["ContainerDisplayName"]),
                            ContainterNo = Convert.ToInt32(dr["ContainterNo"]),
                            TotalItem = Convert.ToInt32(dr["TotalItem"]),
                            ModuleName = Convert.ToString(dr["ModuleName"])

                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dashboardContainer;
        }

    }
}
