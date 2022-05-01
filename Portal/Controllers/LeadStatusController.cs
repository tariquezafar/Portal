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
    public class LeadStatusController : BaseController
    {
        //
        // GET: /Company/
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_LeadStatus_CP, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditLeadStatus(int leadStatusId = 0, int accessMode = 3)
        {

            try
            {
                if (leadStatusId != 0)
                {
                    ViewData["leadStatusId"] = leadStatusId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["leadStatusId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_LeadStatus_CP, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditLeadStatus(LeadStatusViewModel leadstatusViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            LeadStatusBL leadstatusBL = new LeadStatusBL();
            try
            {
                if (leadstatusViewModel != null)
                {
                    leadstatusViewModel.CompanyId = ContextUser.CompanyId;
                    responseOut = leadstatusBL.AddEditLeadStatus(leadstatusViewModel);

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


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_LeadStatus_CP, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListLeadStatus()
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
        public PartialViewResult GetLeadStatusList(string leadstatusName = "", string Status = "")
        {
            List<LeadStatusViewModel> leadstatuses = new List<LeadStatusViewModel>();
            LeadStatusBL leadstatusBL = new LeadStatusBL();
            try
            {
                leadstatuses = leadstatusBL.GetLeadStatusList(leadstatusName, Status, ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(leadstatuses);
        }


        [HttpGet]
        public JsonResult GetLeadStatusDetail(int leadstatusId)
        {
            LeadStatusBL leadstatusBL = new LeadStatusBL();
            LeadStatusViewModel leadstatus = new LeadStatusViewModel();
            try
            {
                leadstatus = leadstatusBL.GetLeadStatusDetail(leadstatusId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(leadstatus, JsonRequestBehavior.AllowGet);
        }

    }
}