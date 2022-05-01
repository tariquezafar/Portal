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
    public class PayHeadGLMappingController : BaseController
    {
        //
        // GET: /User/
        #region PayHeadGLMapping

        [ValidateRequest(true, UserInterfaceHelper.List_PayHeadGlMapping, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditPayHeadGLMapping(int payHeadMappingId = 0, int accessMode = 3)
        {

            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                if (payHeadMappingId != 0)
                {
                    ViewData["payHeadMappingId"] = payHeadMappingId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["payHeadMappingId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.List_PayHeadGlMapping, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditPayHeadGLMapping(PayHeadGLMappingViewModel payHeadGLMappingViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            PayHeadGLMappingBL payHeadGLMappingBL = new PayHeadGLMappingBL();        
            try
            {
                if (payHeadGLMappingViewModel != null)
                {
                    payHeadGLMappingViewModel.CreatedBy = ContextUser.UserId;
                    payHeadGLMappingViewModel.CompanyId = ContextUser.CompanyId;
                    responseOut = payHeadGLMappingBL.AddEditPayHeadGLMapping(payHeadGLMappingViewModel);
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


        [ValidateRequest(true, UserInterfaceHelper.List_PayHeadGlMapping, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListPayHeadGLMapping()
        {
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }
        [HttpGet]
        public PartialViewResult GetPayHeadGLMappingList(string payHeadName = "", string status = "",string companyBranch="")
        {
            List<PayHeadGLMappingViewModel> payHeadGLMappingViewModels = new List<PayHeadGLMappingViewModel>();            
            PayHeadGLMappingBL payHeadGLMappingBL = new PayHeadGLMappingBL();
          
            try
            {
                payHeadGLMappingViewModels = payHeadGLMappingBL.GetPayHeadGLMappingList(payHeadName, ContextUser.CompanyId, status, companyBranch);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(payHeadGLMappingViewModels);
        }
        public JsonResult GetPayHeadGLMappingDetail(int payHeadMappingId)
        {         
            PayHeadGLMappingBL payHeadGLMappingBL = new PayHeadGLMappingBL();
            PayHeadGLMappingViewModel payHeadGLMappingViewModel = new PayHeadGLMappingViewModel();
            try
            {
                payHeadGLMappingViewModel = payHeadGLMappingBL.GetPayHeadGLMappingDetail(payHeadMappingId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(payHeadGLMappingViewModel, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
