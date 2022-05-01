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
    public class PMS_PerformanceCycleController : BaseController
    {
        //
        // GET: /Company/
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PMS_PerformanceCycle, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditPerformanceCycle(int performancecycleId = 0, int accessMode = 3)
        {

            try
            {
                if (performancecycleId != 0)
                {
                    ViewData["performancecycleId"] = performancecycleId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["performancecycleId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PMS_PerformanceCycle, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditPerformanceCycle(PMS_PerformanceCycleViewModel pmsperformancecycleViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            PMS_PerformanceCycleBL pmssectionBL = new PMS_PerformanceCycleBL();
            try
            {
                if (pmsperformancecycleViewModel != null)
                {
                    responseOut = pmssectionBL.AddEditPerformanceCycle(pmsperformancecycleViewModel);
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


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PMS_PerformanceCycle, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListPerformanceCycle()
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
        [HttpGet]
        public PartialViewResult GetPerformanceCycleList(string performancecycleName = "", string performancecycleStatus = "")
        {
            List<PMS_PerformanceCycleViewModel> pmsperformancecycles = new List<PMS_PerformanceCycleViewModel>();
            PMS_PerformanceCycleBL performancecycleBL = new PMS_PerformanceCycleBL();
            try
            {
                pmsperformancecycles = performancecycleBL.GetPerformanceCycleList(performancecycleName, performancecycleStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(pmsperformancecycles);
        }


        [HttpGet]
        public JsonResult GetPerformanceCycleDetail(int performancecycleId)
        {
            PMS_PerformanceCycleBL pmsperformancecycleBL = new PMS_PerformanceCycleBL();
            PMS_PerformanceCycleViewModel pmsperformancecycle = new PMS_PerformanceCycleViewModel();
            try
            {
                pmsperformancecycle = pmsperformancecycleBL.GetPerformanceCycleDetail(performancecycleId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(pmsperformancecycle, JsonRequestBehavior.AllowGet);
        }

    }
}
