using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Portal.Core;
using Portal.Core.ViewModel;
using Portal.Common;
using System.Reflection;
using System.IO;
using Microsoft.Reporting.WebForms;
using Portal.Core.LeadType;

namespace Portal.Controllers
{
    public class LeadMasterController : BaseController
    {
        //
        // GET: /LeadMaster/

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_LeadMaster, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditLeadMaster(int LeadTypeId = 0, int accessMode = 3)
        {

            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                if (LeadTypeId != 0)
                {
                    ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                    ViewData["LeadTypeId"] = LeadTypeId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                    ViewData["LeadTypeId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_LeadMaster, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditLeadMaster(LeadTypeMasterViewModel leadTypeMasterViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            LeadTypeMasterBL leadTypeMasterBL = new LeadTypeMasterBL();
            try
            {
                if (leadTypeMasterViewModel != null)
                {
                    //PayRollTdsViewModel.TdsSlaBid = 0;
                    leadTypeMasterViewModel.Companyid = ContextUser.CompanyId;
                    leadTypeMasterViewModel.CreatedBy = ContextUser.UserId;
                    leadTypeMasterViewModel.Modifiedby = ContextUser.UserId;
                    responseOut = leadTypeMasterBL.AddEditLeadTypeMaster(leadTypeMasterViewModel);

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


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_LeadMaster, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListLeadMaster()
        {
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();

               
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }

        [HttpGet]
        public PartialViewResult GetDetailLeadMaster(string txtLeadType,string companyBranch)
        {
            List<LeadTypeMasterViewModel> leadMaster = new List<LeadTypeMasterViewModel>();
            LeadTypeMasterBL leadTypeMasterBL = new LeadTypeMasterBL();
            try
            {
                leadMaster = leadTypeMasterBL.GetAllLeadList(txtLeadType, companyBranch);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(leadMaster);
        }

        [HttpGet]
        public JsonResult GetLeadMasterDetails(int LeadTypeId)
        {
            LeadTypeMasterBL leadTypeMasterBL = new LeadTypeMasterBL();
            LeadTypeMasterViewModel LeadTypeDetails = new LeadTypeMasterViewModel();
            try
            {
                LeadTypeDetails = leadTypeMasterBL.GetLeadTypeDetails(LeadTypeId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(LeadTypeDetails, JsonRequestBehavior.AllowGet);
        }

    }
}
