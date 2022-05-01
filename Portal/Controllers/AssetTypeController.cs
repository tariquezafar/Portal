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
    public class AssetTypeController : BaseController
    {
        //
        // GET: /Company/
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_AssetType, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditAssetType(int assettypeId = 0, int accessMode = 3)
        {

            try
            {
                if (assettypeId != 0)
                {
                    ViewData["assettypeId"] = assettypeId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["assettypeId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_AssetType, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditAssetType(HR_AssetTypeViewModel assettypeViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            AssetTypeBL assettypeBL = new AssetTypeBL();
            try
            {
                if (assettypeViewModel != null)
                {
                    responseOut = assettypeBL.AddEditAssetType(assettypeViewModel);
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


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_AssetType, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListAssetType()
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
        public PartialViewResult GetAssetTypeList(string assettypeName = "", string assettypeStatus = "")
        {
            List<HR_AssetTypeViewModel> assettypes = new List<HR_AssetTypeViewModel>();
            AssetTypeBL assettypeBL = new AssetTypeBL();
            try
            {
                assettypes = assettypeBL.GetAssetTypeList(assettypeName, assettypeStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(assettypes);
        }


        [HttpGet]
        public JsonResult GetAssetTypeDetail(int assettypeId)
        {
            AssetTypeBL assettypeBL = new AssetTypeBL();
            HR_AssetTypeViewModel assettype = new HR_AssetTypeViewModel();
            try
            {
                assettype = assettypeBL.GetAssetTypeDetail(assettypeId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(assettype, JsonRequestBehavior.AllowGet);
        }

    }
}
