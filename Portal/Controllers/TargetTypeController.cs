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
    public class TargetTypeController : BaseController
    {
        //
        // GET: /TargetType/


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_TargetType, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditTargetType(int targetTypeId = 0, int accessMode = 3)
        {

            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                if (targetTypeId != 0)
                {
                    ViewData["targetTypeId"] = targetTypeId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["targetTypeId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_TargetType, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditTargetType(TargetTypeViewModel targettypeViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            TargetTypeBL targettypeBL = new TargetTypeBL();
            try
            {
                if (targettypeViewModel != null)
                {
                    
                    targettypeViewModel.CreatedBy= ContextUser.UserId;
                    targettypeViewModel.Modifiedby = ContextUser.UserId;
                    targettypeViewModel.CreatedDate= DateTime.Now.ToString("dd-MMM-yyyy");
                    targettypeViewModel.ModifiedDate = DateTime.Now.ToString("dd-MMM-yyyy");
                    responseOut = targettypeBL.AddEditTargetType(targettypeViewModel);
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

        [HttpGet]
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_TargetType, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListTargetType()
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
        public PartialViewResult GetTargetTypeList(string targettypeName = "", string targettypeDesc = "", int Status =0,int companyBranchId=0)
        {
            List<TargetTypeViewModel> targetType = new List<TargetTypeViewModel>();
            TargetTypeBL targetTypeBL = new TargetTypeBL();
            try
            {
                int UserId = ContextUser.UserId;
                targetType = targetTypeBL.GetTargetTypeList(targettypeName, targettypeDesc, Status, UserId, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(targetType);
        }

      

        [HttpGet]
        public JsonResult GetTargetTypeDetail(int targettypeId)
        {
            TargetTypeBL targetTypeBL = new TargetTypeBL();
            TargetTypeViewModel targetType = new TargetTypeViewModel();
            try
            {
                targetType = targetTypeBL.GetTargetTypeDetail(targettypeId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(targetType, JsonRequestBehavior.AllowGet);
        }




    }
}
