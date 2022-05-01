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
    public class SeparationClearListController : BaseController
    {
        //
        // GET: /Company/
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_SeparationClearList, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditSeparationClearList(int separationclearlistId = 0, int accessMode = 3)
        {

            try
            {
                if (separationclearlistId != 0)
                {
                    ViewData["separationclearlistId"] = separationclearlistId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["separationclearlistId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_SeparationClearList, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditSeparationClearList(SeparationClearListViewModel separationclearlistViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            SeparationClearListBL separationclearlistBL = new SeparationClearListBL();
            try
            {
                if (separationclearlistViewModel != null)
                {
                    responseOut = separationclearlistBL.AddEditSeparationClearList(separationclearlistViewModel);
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


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_SeparationClearList, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListSeparationClearList()
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
        public PartialViewResult GetSeparationClearList(string separationclearlistName = "", string separationclearlistStatus = "")
        {
            List<SeparationClearListViewModel> separationclearlists = new List<SeparationClearListViewModel>();
            SeparationClearListBL separationclearlistBL = new SeparationClearListBL();
            try
            {
                separationclearlists = separationclearlistBL.GetSeparationClearList(separationclearlistName, separationclearlistStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(separationclearlists);
        }


        [HttpGet]
        public JsonResult GetSeparationClearListDetail(int separationclearlistId)
        {
            SeparationClearListBL separationclearlistBL = new SeparationClearListBL();
            SeparationClearListViewModel separationclearlist = new SeparationClearListViewModel();
            try
            {
                separationclearlist = separationclearlistBL.GetSeparationClearListDetail(separationclearlistId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(separationclearlist, JsonRequestBehavior.AllowGet);
        }

    }
}
