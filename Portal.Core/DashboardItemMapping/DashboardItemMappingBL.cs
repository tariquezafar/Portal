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
    public class DashboardItemMappingBL
    {

        DBInterface dbInterface;
        public DashboardItemMappingBL()
        {
            dbInterface = new DBInterface();
        }

        //public ResponseOut AddEditDashboardContainer(DashboardContainerViewModel dashboardContainerViewModel)
        //{
        //    ResponseOut responseOut = new ResponseOut();
        //    try
        //    {

        //        DashboardContainer dashboardContainer = new DashboardContainer
        //        {
        //            DashboardContainterID = dashboardContainerViewModel.DashboardContainterID,
        //            ContainerName = dashboardContainerViewModel.ContainerName,
        //            ContainerDisplayName = dashboardContainerViewModel.ContainerDisplayName,
        //            ContainterNo = dashboardContainerViewModel.ContainterNo,
        //            TotalItem = dashboardContainerViewModel.TotalItem,
        //            ModuleName = dashboardContainerViewModel.ModuleName


        //        };
        //        responseOut = dbInterface.AddEditDashboardContainer(dashboardContainer);
        //    }
        //    catch (Exception ex)
        //    {
        //        responseOut.status = ActionStatus.Fail;
        //        responseOut.message = ActionMessage.ApplicationException;
        //        Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
        //        throw ex;
        //    }
        //    return responseOut;
        //}



        //public List<DashboardContainerViewModel> GetDashboardContainerList(string dashboardContainerName = "", string dashboardContainerdisplayName = "", int dashboardcontainerNo = 0, int totalItem = 0, string module = "")
        //{
        //    List<DashboardContainerViewModel> dashboardContainers = new List<DashboardContainerViewModel>();
        //    SQLDbInterface sqlDbInterface = new SQLDbInterface();
        //    try
        //    {
        //        DataTable dtDashboardContainer = sqlDbInterface.GetDashboardContainerList(dashboardContainerName,
        //            dashboardContainerdisplayName, dashboardcontainerNo, totalItem, module);

        //        if (dtDashboardContainer != null && dtDashboardContainer.Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in dtDashboardContainer.Rows)
        //            {
        //                dashboardContainers.Add(new DashboardContainerViewModel
        //                {
        //                    DashboardContainterID = Convert.ToInt64(dr["DashboardContainterID"]),
        //                    ContainerName = Convert.ToString(dr["ContainerName"]),
        //                    ContainerDisplayName = Convert.ToString(dr["ContainerDisplayName"]),
        //                    ContainterNo = Convert.ToInt32(dr["ContainterNo"]),
        //                    TotalItem = Convert.ToInt32(dr["TotalItem"]),
        //                    ModuleName = Convert.ToString(dr["ModuleName"])

        //                });
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
        //        throw ex;
        //    }
        //    return dashboardContainers;

        //}


        //public DashboardContainerViewModel GetDashboardContainerDetail(int dashboardContainerId)
        //{

        //    DashboardContainerViewModel dashboardContainer = new DashboardContainerViewModel();
        //    SQLDbInterface sqlDbInterface = new SQLDbInterface();

        //    try
        //    {
        //        DataTable dtDashboardContainer = sqlDbInterface.GetDashboardContainerDetail(dashboardContainerId);

        //        if (dtDashboardContainer != null && dtDashboardContainer.Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in dtDashboardContainer.Rows)
        //            {

        //                dashboardContainer = new DashboardContainerViewModel
        //                {

        //                    DashboardContainterID = Convert.ToInt32(dr["DashboardContainterID"]),
        //                    ContainerName = Convert.ToString(dr["ContainerName"]),
        //                    ContainerDisplayName = Convert.ToString(dr["ContainerDisplayName"]),
        //                    ContainterNo = Convert.ToInt32(dr["ContainterNo"]),
        //                    TotalItem = Convert.ToInt32(dr["TotalItem"]),
        //                    ModuleName = Convert.ToString(dr["ModuleName"])

        //                };
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
        //        throw ex;
        //    }
        //    return dashboardContainer;
        //}




        public List<DashboardContainerViewModel> FillDashboardContainerList(string moduleName)
        {

            List<DashboardContainerViewModel> dashboardContainers = new List<DashboardContainerViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtDashboardContainer = sqlDbInterface.FillDashboardContainerList(moduleName);

                if (dtDashboardContainer != null && dtDashboardContainer.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtDashboardContainer.Rows)
                    {
                        dashboardContainers.Add(new DashboardContainerViewModel
                        {
                            DashboardContainterID = Convert.ToInt64(dr["DashboardContainterID"]),
                            ContainerName = Convert.ToString(dr["ContainerName"]),
                            //ContainerDisplayName = Convert.ToString(dr["ContainerDisplayName"]),                          
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



        public List<DashboardItemMappingViewModel> GetDashboardItemMapping(string moduleName = "", int containerID = 0, int roleId = 0, int companyBranchID = 0)
        {
            List<DashboardItemMappingViewModel> dashboardItemMappings = new List<DashboardItemMappingViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtDashboardItemmapping = sqlDbInterface.GetDashboadItemMappingDetail(moduleName, containerID, roleId, companyBranchID);

                if (dtDashboardItemmapping != null && dtDashboardItemmapping.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtDashboardItemmapping.Rows)
                    {
                        //if (Convert.ToInt32(dr["ParentInterfaceId"]) == 0 && Convert.ToString(dr["InterfaceURL"]) == "")
                        //{
                        //    continue;
                        //}
                        // else {
                        dashboardItemMappings.Add(new DashboardItemMappingViewModel
                        {
                            DashboardItemMappingID = Convert.ToInt64(dr["DashboardItemMappingID"]),
                            DashboardItemId = Convert.ToInt64(dr["DashboardItemId"]),
                            ItemName = Convert.ToString(dr["ItemName"]),
                            ItemDisplayName = Convert.ToString(dr["ItemDisplayName"]),
                            MappingStatus = Convert.ToBoolean(dr["MappingStatus"]),
                            ContainerNo = Convert.ToInt32 (dr["ContainterNo"]),
                            ContainerName = Convert.ToString(dr["ContainerName"]),


                        });
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dashboardItemMappings;
        }








        public ResponseOut AddEditDashDashboardItemMapping(List<DashboardItemMappingViewModel> DashboardItemMappingList, string moduleName, int containerID, int roleId, int companyBranchID)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {

                foreach (var item in DashboardItemMappingList)
                {
                    
                    DashboardItemMapping dashboardItemmapping = new DashboardItemMapping
                    {
                        DashboardItemMappingID = item.DashboardItemMappingID,
                        DashboardItemId = item.DashboardItemId,
                        Status = item.MappingStatus,
                        ModuleName = moduleName,
                        ContainerID = containerID,
                        RoleId = roleId,                       
                        CompanyBranchId = companyBranchID,
                        //ContainerNo = item.ContainerNo,
                        
                   };

                    if (item.MappingStatus == true)
                    {
                        if (item.ContainerNo == 0)
                        {
                            responseOut = dbInterface.AddEditDashDashboardItemMapping(dashboardItemmapping);
                        }
                    }
                    else
                    {
                        responseOut = dbInterface.DeleteDashDashboardItemMapping(dashboardItemmapping);
                    }
                }

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



        public DashboardContainerViewModel GetDashboardContainerDetailforItemMapping(int dashboardContainerId)
        {
            // newly added
            DashboardContainerViewModel dashboardContainer = new DashboardContainerViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();

            try
            {
                DataTable dtDashboardContainer = sqlDbInterface.GetDashboardContainerDetailForItemMapping(dashboardContainerId);

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
