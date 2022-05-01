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
    public class DashBoardContainerController : BaseController
    {

        #region DashBoardContainer

        // GET: /DashBoardContainer/

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_DashboardContainer_ADMIN, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditDashBoardContainer(int dashboardcontainterID = 0, int accessMode = 3)
        {
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                if (dashboardcontainterID != 0)
                {
                    ViewData["dashboardcontainterID"] = dashboardcontainterID;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["bookId"] = 0;
                    ViewData["accessMode"] = 0;
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }



        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_DashboardContainer_ADMIN, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditDashboardContainer(DashboardContainerViewModel dashboardcontainerViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            DashBoardContainerBL dashBoardContainerBL = new DashBoardContainerBL();
            try
            {
                if (dashboardcontainerViewModel != null)
                {
                    responseOut = dashBoardContainerBL.AddEditDashboardContainer (dashboardcontainerViewModel);
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



        [HttpGet]
        public PartialViewResult GetDashboardContainerList(string dashboardContainerName = "",string dashboardContainerdisplayName = "", int  dashboardcontainerNo = 0, int totalItem = 0,string module = "0")
        {
            // this method is getting data and showing in grid
            List<DashboardContainerViewModel> dashboardContainers = new List<DashboardContainerViewModel>();

            DashBoardContainerBL dashBoardContainerBL = new DashBoardContainerBL();
            try
            {
                dashboardContainers = dashBoardContainerBL.GetDashboardContainerList(dashboardContainerName,
                    dashboardContainerdisplayName, dashboardcontainerNo, totalItem, module);
                                                             
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(dashboardContainers);
        }


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Book_ACCOUNT, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListDashboardContainer()
        {

            // this code is just opening search form for user.
            try
            {
              //  ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }



        public JsonResult GetDashboardContainerDetail(int dashboardContainerId)
        {
            DashBoardContainerBL dashboardContainerBL = new DashBoardContainerBL();
            DashboardContainerViewModel dashboardContainer = new DashboardContainerViewModel();
            
            try
            {
                dashboardContainer = dashboardContainerBL.GetDashboardContainerDetail(dashboardContainerId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }

            return Json(dashboardContainer, JsonRequestBehavior.AllowGet);
        }

        #endregion DashBoardContainer
    }
}
