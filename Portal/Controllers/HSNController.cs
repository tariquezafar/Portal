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
    public class HSNController : BaseController
    {
        //
        // GET: /Company/
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_HSN, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditHSN(int hSNID = 0, int accessMode = 3)
        {

            try
            {
                if (hSNID != 0)
                {
                    ViewData["hSNID"] = hSNID;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["hSNID"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_HSN, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditHSN(HSNViewModel hSNViewModel)
        {
            ResponseOut responseOut = new ResponseOut();                     
            HSNBL hSNBL = new HSNBL();
            try
            {
                if (hSNViewModel != null)
                {

                   
                    responseOut = hSNBL.AddEditHSN(hSNViewModel);
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


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_HSN, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListHSN()
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
        public PartialViewResult GetHSNList( string hSNCode = "", string hsnStatus = "")
        {
            List<HSNViewModel> hSNViewModel = new List<HSNViewModel>();

            HSNBL hSNBL = new HSNBL();
            try
            {
                hSNViewModel = hSNBL.GetHSNList(hSNCode, hsnStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(hSNViewModel);
        }     
        [HttpGet]
        public JsonResult GetHSNDetail(int hSNID)
        {                       
            HSNBL hSNBL = new HSNBL();
            HSNViewModel hSNViewModel = new HSNViewModel();
            try
            {
                hSNViewModel = hSNBL.GetHSNDetail(hSNID);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(hSNViewModel, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetHSNAutoCompleteList(string term)
        {
            HSNBL hSNBL = new HSNBL();
          
            List<HSNViewModel> hSNList = new List<HSNViewModel>();
            try
            {
                hSNList = hSNBL.GetHSNAutoCompleteList(term);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(hSNList, JsonRequestBehavior.AllowGet);
        }

    }
}
