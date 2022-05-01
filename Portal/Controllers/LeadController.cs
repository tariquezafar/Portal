using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Portal.Core;
using Portal.Core.ViewModel;
using Portal.Common;
using System.Reflection;
using Microsoft.Reporting.WebForms;
using System.IO;
namespace Portal.Controllers
{
    [CheckSessionBeforeControllerExecuteAttribute(Order = 1)]
    public class LeadController : BaseController
    {
        //
        // GET: /User/
        #region Lead
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Lead_CRM, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditLead(int leadId = 0, int accessMode = 3)
        {
          try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                if (leadId != 0)
                {
                    ViewData["leadId"] = leadId;
                    ViewData["accessMode"] = accessMode;
                    ViewData["UserId"] = ContextUser.UserId;
                    ViewData["FollowUpByUserName"] = ContextUser.FullName;
                    ViewData["UserId"] = ContextUser.UserId;
                    
                }
                else
                {
                    ViewData["leadId"] = 0;
                    ViewData["accessMode"] = 0;
                    
                    ViewData["ModifyDate"] = DateTime.Now;
                    ViewData["FollowUpByUserName"] = ContextUser.FullName;
                    ViewData["UserId"] = ContextUser.UserId;
                }
                ViewData["CreatedDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }
        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Lead_CRM, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditLead(LeadViewModel leadViewModel, List<LeadFollowUpViewModel> leadFollowUps)
        {
            ResponseOut responseOut = new ResponseOut();
            LeadBL leadBL = new LeadBL();
            try
            {
                if (leadViewModel != null)
                {
                    leadViewModel.CreatedBy = ContextUser.UserId;
                    leadViewModel.CompanyId = ContextUser.CompanyId;
                    responseOut = leadBL.AddEditLead(leadViewModel, leadFollowUps);
                    
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
        public JsonResult GetLeadSourceList()
        {
            LeadSourceBL leadsourceBL = new LeadSourceBL();
            List<LeadSourceViewModel> leadsourceList = new List<LeadSourceViewModel>();
            try
            {
                leadsourceList = leadsourceBL.GetLeadSourceList();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(leadsourceList, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetAllLeadType()
        {
            LeadBL leadBL = new LeadBL();
            List<LeadTypeMasterViewModel> followupList = new List<LeadTypeMasterViewModel>();
            try
            {
                followupList = leadBL.GetAllLeadType();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(followupList, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetFollowUpActivityTypeList()
        {
            LeadBL leadBL = new LeadBL();
            List<FollowUpActivityTypeViewModel> followupList = new List<FollowUpActivityTypeViewModel>();
            try
            {
                followupList = leadBL.GetFollowUpActivityTypeList();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(followupList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetLeadStatusList()
        {
            LeadStatusBL leadstatusBL = new LeadStatusBL();
            List<LeadStatusViewModel> leadstatusList = new List<LeadStatusViewModel>();
            try
            {
                leadstatusList = leadstatusBL.GetLeadStatusList();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(leadstatusList, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetUserAutoCompleteList(string term,long companyBranchID)
        {
            LeadBL leadBL = new LeadBL();
            List<UserViewModel> userList = new List<UserViewModel>();
            try
            {
                if (term != "")
                {
                    userList = leadBL.GetUserAutoCompleteList(term, companyBranchID);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(userList, JsonRequestBehavior.AllowGet);
        }

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Lead_CRM, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListLead()
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

        public PartialViewResult GetLeadList(string leadCode = "", string companyName = "", string contactPersonName = "", string email = "", string contactNo = "", string companyAddress = "", string companyCity = "", int companyStateId = 0, int leadSourceId = 0, int leadStatusId = 0, int leadSourceType = 0, int userId = 0, string status = "", int LeadTypeId = 0, string companyBranch = "")
        { 
            List<LeadViewModel> leads = new List<LeadViewModel>();
            LeadBL leadBL = new LeadBL();
            try
            {
                if (leadSourceType==2)
                {
                    leads = leadBL.GetLeadList(leadCode, companyName, contactPersonName, email, contactNo, companyAddress, companyCity, companyStateId, leadStatusId, leadSourceId, ContextUser.UserId, status, ContextUser.UserId, ContextUser.RoleId, LeadTypeId, companyBranch);
                }
                else
                {
                    leads = leadBL.GetLeadList(leadCode, companyName, contactPersonName, email, contactNo, companyAddress, companyCity, companyStateId, leadStatusId, leadSourceId, ContextUser.UserId, status,ContextUser.UserId,ContextUser.RoleId, LeadTypeId, companyBranch);
                }
                
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(leads);
        }


        public JsonResult GetLeadDetail(int leadId)
        {
            LeadBL leadBL = new LeadBL();
            LeadViewModel lead = new LeadViewModel();
            try
            {
                lead = leadBL.GetLeadDetail(leadId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(lead, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCountryList()
        {
            CountryBL countryBL = new CountryBL();
            List<CountryViewModel> countryList = new List<CountryViewModel>();
            try
            {
                countryList = countryBL.GetCountryList();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(countryList, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetStateList(int countryId)
        {
            StateBL stateBL = new StateBL();
            List<StateViewModel> stateList = new List<StateViewModel>();
            LeadBL leadBL = new LeadBL();
            try
            {
                stateList = leadBL.GetStateList(ContextUser.UserName,countryId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(stateList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Lead_CRM, (int)AccessMode.EditAccess, (int)RequestMode.Ajax)]
        public JsonResult LeadFollowUpValidation(LeadFollowUpViewModel leadFollowUpViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            LeadBL leadBL = new LeadBL();
            try
            {
                responseOut = leadBL.LeadFollowUpValidation(leadFollowUpViewModel);
            }
            catch (Exception ex)
            {
                responseOut.message = ActionMessage.ApplicationException;
                responseOut.status = ActionStatus.Fail;
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(responseOut, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public PartialViewResult GetLeadFollowUpList(List<LeadFollowUpViewModel> leadFollowUps, int leadid)
        {
            LeadBL leadBL = new LeadBL();
            try
            {
                if (leadFollowUps == null)
                {
                  leadFollowUps = leadBL.GetLeadFollowUpList(leadid);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(leadFollowUps);
        }


        public ActionResult GenerateLeadReport(string leadCode = "", string companyName = "", string contactPersonName = "", string email = "", string contactNo = "", string companyAddress = "", string companyCity = "", int companyStateId = 0, int leadSourceId = 0, int leadStatusId = 0, int leadSourceType = 0, int userId = 0, string status = "",int leadTypeId=0,string companyBranch="", string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            LeadBL leadBL = new LeadBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "LeadReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }

            if (leadSourceType == 2)
            {
                ReportDataSource rd = new ReportDataSource("LeadReportDataSet", leadBL.GenerateLeadReport(leadCode, companyName, contactPersonName, email, contactNo, companyAddress, companyCity, companyStateId, leadStatusId, leadSourceId, userId, status, ContextUser.UserId, ContextUser.RoleId,leadTypeId, companyBranch));
                lr.DataSources.Add(rd);
            }
            else
            {
                ReportDataSource rd = new ReportDataSource("LeadReportDataSet", leadBL.GenerateLeadReport(leadCode, companyName, contactPersonName, email, contactNo, companyAddress, companyCity, companyStateId, leadStatusId, leadSourceId, ContextUser.UserId, status, ContextUser.UserId, ContextUser.RoleId, leadTypeId));
                lr.DataSources.Add(rd);
            }
              
           

            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =

             "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>21.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.50in</MarginTop>" +
            "  <MarginLeft>.1in</MarginLeft>" +
            "  <MarginRight>.1in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
            "</DeviceInfo>";


            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);


            return File(renderedBytes, mimeType);
        }
        #endregion
    }
}
