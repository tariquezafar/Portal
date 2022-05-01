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
    public class CTCTypeController : BaseController
    {
        //
        // GET: /Company/
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_CTC_HR, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditCTC(int ctcId = 0, int accessMode = 3)
        {

            try
            {
                if (ctcId != 0)
                {
                    ViewData["ctcId"] = ctcId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["ctcId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_CTC_HR, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditCTC(HR_CTCViewModel cTCViewModel)
        {
            ResponseOut responseOut = new ResponseOut();                   
           
            CTCBL cTCBL = new CTCBL();
            try
            {
                if (cTCViewModel != null)
                {
                    cTCViewModel.CreatedBy = ContextUser.UserId;
                    cTCViewModel.CompanyId = ContextUser.CompanyId;  
                    responseOut = cTCBL.AddEditCTC(cTCViewModel);
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


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_CTC_HR, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListCTC()
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
        public PartialViewResult GetCTCList(string designationId = "" ,string ctcStatus = "")
        {
            List<HR_CTCViewModel> hR_CTCViewModel = new List<HR_CTCViewModel>();

            CTCBL cTCBL = new CTCBL();
            try
            {
                hR_CTCViewModel = cTCBL.GetCTCList(designationId, ContextUser.CompanyId, ctcStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(hR_CTCViewModel);
        }     
        [HttpGet]
        public JsonResult GetCTCDetail(int ctcId)
        {

            CTCBL cTCBL = new CTCBL();
            HR_CTCViewModel hR_CTCViewModel = new HR_CTCViewModel();
            try
            {
                hR_CTCViewModel = cTCBL.GetCTCDetail(ctcId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(hR_CTCViewModel, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetDesignationList()
        {
            DesignationBL designationBL = new DesignationBL();
            List<DesignationViewModel> designations = new List<DesignationViewModel>();
            try
            {

                designations = designationBL.GetDesignationList();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(designations, JsonRequestBehavior.AllowGet);
        }
         

    }
}
