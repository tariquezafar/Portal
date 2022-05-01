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
using Microsoft.ReportingServices;
using System.IO;


namespace Portal.Controllers
{
    [CheckSessionBeforeControllerExecuteAttribute(Order = 1)]
    public class ChasisModelController : BaseController
    {
        //
        // GET: /Company/
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ChasisModel, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditChasisModel(int chasisModelID = 0, int accessMode = 3)
        {

            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                if (chasisModelID != 0)
                {
                    ViewData["chasisModelID"] = chasisModelID;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["chasisModelID"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ChasisModel, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditChasisModel(ChasisModelViewModel chasisModelViewModel)
        {
            ResponseOut responseOut = new ResponseOut();         
            ChasisModelBL chasisModelBL = new ChasisModelBL();
            try
            {
                if (chasisModelViewModel != null)
                {
                    responseOut = chasisModelBL.AddEditChasisModel(chasisModelViewModel);
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


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ChasisModel, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListChasisModel()
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
        public PartialViewResult GetChasisModelList(string chasisModelName = "", string chasisModelCode = "", string motorModelCode="", int productSubGroupId = 0, string ChasisModelStatus = "",int companyBranchId=0)
        {
            List<ChasisModelViewModel> chasisModelViewModels = new List<ChasisModelViewModel>();        
            ChasisModelBL chasisModelBL = new ChasisModelBL();
            try
            {
                chasisModelViewModels = chasisModelBL.GetChasisModelList(chasisModelName, chasisModelCode, motorModelCode, productSubGroupId, ChasisModelStatus, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(chasisModelViewModels);
        }

         
        [HttpGet]
        public JsonResult GetChasisModelDetail(long chasisModelID)
        {            
            ChasisModelBL chasisModelBL = new ChasisModelBL();
            ChasisModelViewModel chasisModelViewModel = new ChasisModelViewModel();
            try
            {
                chasisModelViewModel = chasisModelBL.GetChasisModelDetail(chasisModelID);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(chasisModelViewModel, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetChasisModelSubGroupList()
        {            
            ProductSubGroupBL productSubGroupBL = new ProductSubGroupBL();
            List<ProductSubGroupViewModel> productSubGroupList = new List<ProductSubGroupViewModel>();
            try
            {
                productSubGroupList = productSubGroupBL.GetChasisModelSubGroupList();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(productSubGroupList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChasisModelPrint(string chasisModelName = "", string chasisModelCode = "", string motorModelCode = "", int productSubGroupId = 0, string ChasisModelStatus = "", string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();          
            ChasisModelBL chasisModelBL = new ChasisModelBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "ChasisModelReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("ListChasisModel");
            }
            ReportDataSource rd = new ReportDataSource("ChasisModelDataSet", chasisModelBL.ChasisModelPrint(chasisModelName, chasisModelCode, motorModelCode, productSubGroupId, ChasisModelStatus));
            lr.DataSources.Add(rd);          
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo =

             "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>9.5in</PageWidth>" +
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

    }
}