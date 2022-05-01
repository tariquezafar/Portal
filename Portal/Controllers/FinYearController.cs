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
    public class FinYearController : BaseController
    {
        //
        // GET: /FinYear/

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_FinYear_CP, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditFinYear(int finYearId = 0, int accessMode = 3)
        {

            try
            {
                if (finYearId != 0)
                {
                    ViewData["finYearId"] = finYearId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["finYearId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_FinYear_CP, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditFinYear(FinYearViewModel finYearViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            FinYearBL finYearBL = new FinYearBL();
            try
            {
                if (finYearViewModel != null)
                {
                    responseOut = finYearBL.AddEditFinYear(finYearViewModel);
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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_FinYear_CP, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListFinYear()
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
        public PartialViewResult GetFinYearList()
        {
            List<FinYearViewModel> finYears = new List<FinYearViewModel>();
            FinYearBL finYearBL = new FinYearBL();
            try
            {
                finYears = finYearBL.GetFinYearList();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(finYears);
        }

        [HttpGet]
        public JsonResult GetEssFinYearList()
        {
            List<FinYearViewModel> finYears = new List<FinYearViewModel>();
            FinYearBL finYearBL = new FinYearBL();
            try
            {
                finYears = finYearBL.GetFinYearList();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(finYears, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetFinYearDetail(int finYearId)
        {
            FinYearBL finYearBL = new FinYearBL();
            FinYearViewModel finYear = new FinYearViewModel();
           
            try
            {
                finYear = finYearBL.GetFinYearDetail(finYearId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(finYear, JsonRequestBehavior.AllowGet);
        }

    }
}
