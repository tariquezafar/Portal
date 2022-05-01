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
    public class DocumentTypeController : BaseController
    {
        //
        // GET: /Company/
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_DocumentType_Admin, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditDocumentType(int documenttypeId = 0, int accessMode = 3)
        {

            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                if (documenttypeId != 0)
                {
                    ViewData["documenttypeId"] = documenttypeId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["documenttypeId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_DocumentType_Admin, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditDocumentType(DocumentTypeViewModel documenttypeViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
           DocumentTypeBL documenttypeBL = new DocumentTypeBL();
            try
            {
                if (documenttypeViewModel != null)
                {
                    documenttypeViewModel.CompanyId = ContextUser.CompanyId;
                    responseOut = documenttypeBL.AddEditDocumentType(documenttypeViewModel);
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


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_DocumentType_Admin, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListDocumentType()
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
        public PartialViewResult GetDocumentTypeList(string documenttypeDesc = "", string status = "",int companyBranchId=0)
        {
            List<DocumentTypeViewModel> documenttypes = new List<DocumentTypeViewModel>();
            DocumentTypeBL documenttypeBL = new DocumentTypeBL();
            try
            {
               documenttypes = documenttypeBL.GetDocumentTypeList(documenttypeDesc, ContextUser.CompanyId, status, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(documenttypes);
        }

        //[ValidateRequest(true, UserInterfaceHelper.Add_Edit_Company_CP, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        //public ActionResult SuperAdminDashboard()
        //{
        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
        //    }
        //    return View();
        //}


        [HttpGet]
        public JsonResult GetDocumentTypeDetail(int documenttypeId)
        {
            DocumentTypeBL documenttypeBL = new DocumentTypeBL();
            DocumentTypeViewModel documenttype = new DocumentTypeViewModel();
            try
            {
                documenttype = documenttypeBL.GetDocumentTypeDetail(documenttypeId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(documenttype, JsonRequestBehavior.AllowGet);
        }

    }
}
