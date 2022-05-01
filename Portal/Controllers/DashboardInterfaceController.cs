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
    public class DashboardInterfaceController : BaseController
    {
        //
        // GET: /DashboardInterface/

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_DashboardInterface_ADMIN, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditDashboardInterface(int itemId = 0, int accessMode = 3)
        {

            try
            {
               FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
               ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                ViewData["RoleId"] = ContextUser.RoleId;
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;
                if (itemId != 0)
                {
                    ViewData["itemId"] = itemId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["itemId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_DashboardInterface_ADMIN, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditDashboardInterface(DashboardInterfaceViewModel dashboardInterfaceViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            DashboardInterfaceBL dashboardInterfaceBL = new DashboardInterfaceBL();
            try
            {
                if (dashboardInterfaceViewModel != null)
                {
                    responseOut = dashboardInterfaceBL.AddEditDashboardInterface(dashboardInterfaceViewModel);
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


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_DashboardInterface_ADMIN, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListDashboardInterface()
        {
            try
            {

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }

        public PartialViewResult GetDashboardInterfaceList(string itemName = "", string moduleName = "",string containerName="",string status="",string companyBranchId="")
        {
            List<DashboardInterfaceViewModel> dashboardInterface = new List<DashboardInterfaceViewModel>();
            DashboardInterfaceBL dashboardInterfaceBL = new DashboardInterfaceBL();
            try
            {
                dashboardInterface = dashboardInterfaceBL.GetDashboardInterfaceList(itemName,moduleName,containerName,status,companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(dashboardInterface);
        }


        [HttpGet]
        public JsonResult GetDashboardInterfaceDetail(int itemId)
        {
            DashboardInterfaceBL dashboardInterfaceBL = new DashboardInterfaceBL();
            DashboardInterfaceViewModel dashboardInterface = new DashboardInterfaceViewModel();
            try
            {
                dashboardInterface = dashboardInterfaceBL.GetDashboardInterfaceDetail(itemId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(dashboardInterface, JsonRequestBehavior.AllowGet);
        }

    }
}
