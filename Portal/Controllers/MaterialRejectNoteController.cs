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
using System.Data;
using System.Text;
using Portal.DAL;

namespace Portal.Controllers
{
    public class MaterialRejectNoteController :BaseController
    {
        //
        // GET: /AddEditMaterialRejectNote/
        #region Material Reject Note

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_MaterialRejectNote, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditMaterialRejectNote(int MaterialReceiveNoteId = 0, int accessMode = 3)
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                if (MaterialReceiveNoteId != 0)
                {
                    ViewData["materialReceiveNoteId"] = MaterialReceiveNoteId;
                    ViewData["accessMode"] = accessMode;

                }
                else
                {
                    ViewData["materialReceiveNoteId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_MaterialRejectNote, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditMaterialRejectNote(MaterialRejectNoteViewModel materialRejectNoteViewModel, List<MaterialRejectNoteProductDetailViewModel> mrnProductList)
        {
            ResponseOut responseOut = new ResponseOut();
            MaterialRejectNoteBL materialRejectNoteBL = new MaterialRejectNoteBL();
            try
            {
                if (materialRejectNoteViewModel != null)
                {
                    materialRejectNoteViewModel.CreatedBy = ContextUser.UserId;
                    materialRejectNoteViewModel.CompanyId = ContextUser.CompanyId;
                    materialRejectNoteViewModel.FinYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                    responseOut = materialRejectNoteBL.AddEditMaterialRejectNote(materialRejectNoteViewModel, mrnProductList);
                }
                else
                {
                    responseOut.message = ActionMessage.ProbleminData;
                    responseOut.status = ActionStatus.Fail;
                    responseOut.trnId = 0;
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


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_MaterialRejectNote, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListMaterialRejectNote()
        {
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
               

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }


       

        [HttpGet]
        public PartialViewResult GetMaterialRejectNoteList(string materialReceiveNoteNo = "",string qualityCheckNo = "", string gateInNo = "", string poNo = "", string fromDate = "", string toDate = "", string approvalStatus = "", string companyBranch = "")
        {
            
            List<MaterialRejectNoteViewModel> materialRejectNoteViewModel = new List<MaterialRejectNoteViewModel>();
            MaterialRejectNoteBL manufacturerBL = new MaterialRejectNoteBL();
            
            try
            {
                materialRejectNoteViewModel = manufacturerBL.GetMaterialRejectNoteList(materialReceiveNoteNo,qualityCheckNo, gateInNo, poNo, fromDate, toDate, ContextUser.CompanyId, approvalStatus, companyBranch);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(materialRejectNoteViewModel);
        }

        [HttpGet]
        public PartialViewResult GetQualityCheckRejectList(string qualityCheckNo="", string gateInNO="", string poNo = "", string fromDate = "", string toDate = "", string companyBranch = "")
        {
            List<QualityCheckViewModel> qcs = new List<QualityCheckViewModel>();
            QualityCheckBL qualityCheckBL = new QualityCheckBL();
            try
            {
                qcs = qualityCheckBL.GetQualityCheckRejectList(qualityCheckNo,gateInNO, poNo, fromDate, toDate, ContextUser.CompanyId, companyBranch);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(qcs);
        }


        [HttpPost]
        public PartialViewResult GetQualityCheckProductList(long qualityCheckId)
        {
            QualityCheckBL qualityCheckBL = new QualityCheckBL();
            List<QualityCheckProductDetailViewModel> qualityCheckProducts = new List<QualityCheckProductDetailViewModel>();
            try
            {
                if (qualityCheckId!=0)
                {
                    qualityCheckProducts = qualityCheckBL.GetQualityCheckRejectProductList(qualityCheckId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(qualityCheckProducts);
        }

        [HttpGet]
        public JsonResult GetMaterialRejectNoteDetail(long materialReceiveNoteId)
        {
            MaterialRejectNoteBL materialRejectNoteBL = new MaterialRejectNoteBL();
            MaterialRejectNoteViewModel materialRejectNoteViewModel = new MaterialRejectNoteViewModel();
            try
            {
                materialRejectNoteViewModel = materialRejectNoteBL.GetMaterialRejectNoteDetail(materialReceiveNoteId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(materialRejectNoteViewModel, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Report(long materialReceiveNoteId, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            MaterialRejectNoteBL materialRejectNoteBL = new MaterialRejectNoteBL();

            string path = Path.Combine(Server.MapPath("~/RDLC"), "MaterialRejectNotePrint.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }




            ReportDataSource rd = new ReportDataSource("DataSet1", materialRejectNoteBL.GetMaterialRejectNoteDetailReport(materialReceiveNoteId));
            ReportDataSource rdProduct = new ReportDataSource("DataSet2", materialRejectNoteBL.GetMaterialRejectNoteProductDetailReport(materialReceiveNoteId));
            lr.DataSources.Add(rd);
            lr.DataSources.Add(rdProduct);
            string mimeType;
            string encoding;
            string fileNameExtension;



            string deviceInfo = "<DeviceInfo>" +
                        "  <OutputFormat>" + reportType + "</OutputFormat>" +
                        "  <PageWidth>8.5in</PageWidth>" +
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

        #endregion Material Reject Note

    }
}
