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
    public class ClaimTypeController : BaseController
    {
        //
        // GET: /Company/
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Claim_HR, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditClaimType(int claimTypeId = 0, int accessMode = 3)
        {

            try
            {
                if (claimTypeId != 0)
                {
                    ViewData["claimTypeId"] = claimTypeId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["claimTypeId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Claim_HR, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditClaimType(HR_ClaimTypeViewModel claimtypeViewModel)
        {
            ResponseOut responseOut = new ResponseOut();          
            ClaimTypeBL claimTypeBL = new ClaimTypeBL();
            try
            {
                if (claimtypeViewModel != null)
                {
                    responseOut = claimTypeBL.AddEditClaimType(claimtypeViewModel);
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


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Claim_HR, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListClaimType()
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
        public PartialViewResult GetClaimTypeList(string claimtypeName = "", string claimNature = "", string claimtypeStatus = "")
        {
            List<HR_ClaimTypeViewModel> claimtypes = new List<HR_ClaimTypeViewModel>();
          
            ClaimTypeBL claimTypeBL = new ClaimTypeBL();
            try
            {
                claimtypes = claimTypeBL.GetClaimTypeList(claimtypeName, claimNature, claimtypeStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(claimtypes);
        }     
        [HttpGet]
        public JsonResult GetClaimTypeDetail(int claimtypeId)
        {
            
            ClaimTypeBL claimTypeBL = new ClaimTypeBL();
            HR_ClaimTypeViewModel claimtype = new HR_ClaimTypeViewModel();
            try
            {
                claimtype = claimTypeBL.GetClaimTypeDetail(claimtypeId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(claimtype, JsonRequestBehavior.AllowGet);
        }

    }
}
