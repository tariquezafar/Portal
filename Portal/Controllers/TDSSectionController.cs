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
    public class TDSSectionController : BaseController
    {
        //
        // GET: /TDSSection/

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_TDSSection, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditTDSSection(int tdssectionId = 0, int accessMode = 3)
        {

            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                if (tdssectionId != 0)
                {
                    ViewData["tdssectionId"] = tdssectionId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["tdssectionId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_TDSSection, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditTDSSection(TDSSetionViewModel tdsSectionViewModel, List<TDSSectionDocumentDetailViewModel> tdsSectionDocumentDetails)
        {
            ResponseOut responseOut = new ResponseOut();
            TDSSectionBL tdsSectionBL = new TDSSectionBL();
            try
            {
                if (tdsSectionViewModel != null)
                {
                    responseOut = tdsSectionBL.AddEditTDSSection(tdsSectionViewModel, tdsSectionDocumentDetails);
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_TDSSection, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListTDSSection()
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
        public PartialViewResult GetTDSSectionList(string sectionName = "", string sectionDesc = "", decimal sectionMAXValue=0,string Status = "",string companyBranch="")
        {
            List<TDSSetionViewModel> tdsSection = new List<TDSSetionViewModel>();
            TDSSectionBL tdsSectionBL = new TDSSectionBL();
            try
            {
                tdsSection = tdsSectionBL.GetTDSSectionList(sectionName, sectionDesc, sectionMAXValue, Status, companyBranch);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(tdsSection);
        }
        [HttpGet]
        public JsonResult GetTDSSectionDetail(int tdsSectionId)
        {
            TDSSectionBL tdsSectionBL = new TDSSectionBL();
            TDSSetionViewModel tdsSection = new TDSSetionViewModel();
            try
            {
                tdsSection = tdsSectionBL.GetTDSSectionDetail(tdsSectionId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(tdsSection, JsonRequestBehavior.AllowGet);
        }

    }
}
