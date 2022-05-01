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
    public class UOMController : BaseController
    {
        //
        // GET: /User/
        #region UOM
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_UOM_CP, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditUOM(int UOMId = 0, int accessMode = 3)
        {

            try
            {
                if (UOMId != 0)
                { 
                    ViewData["UOMId"] = UOMId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["UOMId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_UOM_CP, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditUOM(UOMViewModel uomViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            UOMBL uomBL = new UOMBL();
            try
            {
                if (uomViewModel != null)
                { 
                    responseOut = uomBL.AddEditUOM(uomViewModel);
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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_UOM_CP, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListUOM()
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
        public PartialViewResult GetUOMList(string uomName = "", string uomDesc = "", string Status = "")
        {
            List<UOMViewModel> uom = new List<UOMViewModel>();
            UOMBL uomBL = new UOMBL();
            try
            {
                uom = uomBL.GetUOMList(uomName, uomDesc, Status);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(uom);
        }


        [HttpGet]
        public JsonResult GetUOMDetail(int uomId)
        {
            UOMBL uomBL = new UOMBL();
            UOMViewModel uom = new UOMViewModel();
            try
            {
                uom = uomBL.GetUOMDetail(uomId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(uom, JsonRequestBehavior.AllowGet);
        }

        #endregion


    }
}