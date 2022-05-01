using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Portal.Core;
using Portal.Core.ViewModel;
using Portal.Common;
using System.Reflection;


namespace Portal.Controllers
{
    [CheckSessionBeforeControllerExecuteAttribute(Order = 1)]
    public class DashboardItemMappingController : BaseController
    {
      
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_DashboardContainer_ADMIN, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditDashDashboardItemMapping(int dashboardItemMappingID = 0, int accessMode = 3)
        {
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                if (dashboardItemMappingID != 0)
                {
                    ViewData["dashboardItemMappingID"] = dashboardItemMappingID;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["dashboardItemMappingID"] = 0;
                    ViewData["accessMode"] = 0;
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }
        public JsonResult BindDashboardContainerList(string moduleName)
        {
            DashboardItemMappingBL dashboardContainerBL = new DashboardItemMappingBL();
            List<DashboardContainerViewModel> dashboardContainerList = new List<DashboardContainerViewModel>();
            try
            {
                dashboardContainerList = dashboardContainerBL.FillDashboardContainerList(moduleName);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(dashboardContainerList, JsonRequestBehavior.AllowGet);
        }
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_DashboardContainer_ADMIN, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public PartialViewResult GetDashboardItemMappingDetail(string moduleName = "", int containerID = 0, int roleId = 0, int companyBranchID = 0)
        {
            List<DashboardItemMappingViewModel> dashboardItemMappingViewModels = new List<DashboardItemMappingViewModel>();
            DashboardItemMappingBL dashboardItemMappingBL = new DashboardItemMappingBL();
            try
            {
                dashboardItemMappingViewModels = dashboardItemMappingBL.GetDashboardItemMapping(moduleName, containerID, roleId, companyBranchID);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(dashboardItemMappingViewModels);
        }
        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.AddEditDashDashboardItemMapping_Admin, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditDashDashboardItemMapping(List<DashboardItemMappingViewModel> dashboardItemMappingList, string moduleName, int containerID, int roleId, int companyBranchID)
        {
            ResponseOut responseOut = new ResponseOut();
            DashboardItemMappingBL dashboardItemMappingBL = new DashboardItemMappingBL();
            try
            {
                if (dashboardItemMappingList != null && dashboardItemMappingList.Count > 0)
                {
                    responseOut = dashboardItemMappingBL.AddEditDashDashboardItemMapping(dashboardItemMappingList, moduleName, containerID, roleId, companyBranchID);
                }
                else
                {
                    responseOut.message = ActionMessage.ProbleminData;
                    responseOut.status = ActionStatus.Fail;
                }

            }
            catch (Exception ex)
            {
                responseOut.message = ActionMessage.ApplicationException;
                responseOut.status = ActionStatus.Fail;
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(responseOut, JsonRequestBehavior.AllowGet);
        }


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_DashboardContainer_ADMIN, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]

        public ActionResult GetDashboardContainerDetail(int dashboardContainerId)
        {

            ResponseOut responseOut = new ResponseOut();
            DashboardItemMappingBL dashboardItemMappingBL = new DashboardItemMappingBL();
            DashboardContainerViewModel dashboardContainers = new DashboardContainerViewModel();

            try
            {
                dashboardContainers = dashboardItemMappingBL.GetDashboardContainerDetailforItemMapping(dashboardContainerId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }

            //return PartialView(dashboardContainers);

            return Json(dashboardContainers, JsonRequestBehavior.AllowGet);
        }

    }
}
