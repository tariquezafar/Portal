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
    public class SeparationCategoryController : BaseController
    {
        //
        // GET: /Company/
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_SeparationCategory, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditSeparationCategory(int separationcategoryId = 0, int accessMode = 3)
        {

            try
            {
                if (separationcategoryId != 0)
                {
                    ViewData["separationcategoryId"] = separationcategoryId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["separationcategoryId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_SeparationCategory, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditSeparationCategory(SeparationCategoryViewModel separationcategoryViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            SeparationCategoryBL separationcategoryBL = new SeparationCategoryBL();
            try
            {
                if (separationcategoryViewModel != null)
                {
                    responseOut = separationcategoryBL.AddEditSeparationCategory(separationcategoryViewModel);
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


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_SeparationCategory, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListSeparationCategory()
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
        public PartialViewResult GetSeparationCategory(string separationcategoryName = "", string separationcategoryStatus = "")
        {
            List<SeparationCategoryViewModel> separationcategorys = new List<SeparationCategoryViewModel>();
            SeparationCategoryBL separationcategoryBL = new SeparationCategoryBL();
            try
            {
                separationcategorys = separationcategoryBL.GetSeparationCategory(separationcategoryName, separationcategoryStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(separationcategorys);
        }


        [HttpGet]
        public JsonResult GetSeparationCategoryDetail(int separationcategoryId)
        {
            SeparationCategoryBL separationcategoryBL = new SeparationCategoryBL();
            SeparationCategoryViewModel separationcategory = new SeparationCategoryViewModel();
            try
            {
                separationcategory = separationcategoryBL.GetSeparationCategoryDetail(separationcategoryId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(separationcategory, JsonRequestBehavior.AllowGet);
        }

    }
}
