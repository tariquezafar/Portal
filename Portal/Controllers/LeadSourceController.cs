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
    public class LeadSourceController : BaseController
    {
        //
        // GET: /Company/
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_LeadSource_CP, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditLeadSource(int leadSourceId = 0, int accessMode = 3)
        {

            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                if (leadSourceId != 0)
                {

                    ViewData["leadSourceId"] = leadSourceId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["leadSourceId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_LeadSource_CP, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditLeadSource(LeadSourceViewModel leadsourceViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            LeadSourceBL leadsourceBL = new LeadSourceBL();
            try
            {
                if (leadsourceViewModel != null)
                { 
                    leadsourceViewModel.CompanyId = ContextUser.CompanyId;
                    responseOut = leadsourceBL.AddEditLeadSource(leadsourceViewModel);

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


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_LeadSource_CP, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListLeadSource()
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
        public PartialViewResult GetLeadSourceList(string leadsourceName = "", string Status = "",string CompanyBranch="")
        {
            List<LeadSourceViewModel> leadsources = new List<LeadSourceViewModel>();
            LeadSourceBL leadsourceBL = new LeadSourceBL();
            try
            {
                leadsources = leadsourceBL.GetLeadSourceList(leadsourceName, Status, ContextUser.CompanyId, CompanyBranch);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(leadsources);
        }


        [HttpGet]
        public JsonResult GetLeadSourceDetail(int leadsourceId)
        {
            LeadSourceBL leadsourceBL = new LeadSourceBL();
            LeadSourceViewModel leadsource = new LeadSourceViewModel();
            try
            {
                leadsource = leadsourceBL.GetLeadSourceDetail(leadsourceId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(leadsource, JsonRequestBehavior.AllowGet);
        }

    }
}