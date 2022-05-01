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
    public class VendorFormController : BaseController
    {
        //
        // GET: /VendorForm/
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_VendorForm, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditVendorForm(int vendorFormTrnId = 0, int accessMode = 3)
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();

                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");


                if (vendorFormTrnId != 0)
                {
                    ViewData["VendorFormTrnId"] = vendorFormTrnId;
                    ViewData["accessMode"] = accessMode;
                    ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                }
                else
                {
                    ViewData["VendorFormTrnId"] = 0;
                    ViewData["accessMode"] = 3;
                    ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }

        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_VendorForm, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditVendorForm(VendorFormViewModel vendorFormViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            VendorFormBL vendorFormBL = new VendorFormBL();
            try
            {
                if (vendorFormViewModel != null)
                {
                    vendorFormViewModel.CreatedBy = ContextUser.UserId;
                    vendorFormViewModel.CompanyId = ContextUser.CompanyId;
                    responseOut = vendorFormBL.AddEditVendorForm(vendorFormViewModel);
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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_VendorForm, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListVendorForm()
        {
            try
            {
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
        public PartialViewResult GetVendorFormList(string FormStatus = "0", string vendorName = "", string invoiceNo = "", string refNo = "", string fromDate = "", string toDate = "")
        {

            List<VendorFormViewModel> vendorFormViewModel = new List<VendorFormViewModel>();
            VendorFormBL vendorFormBL = new VendorFormBL();
            try
            {
                vendorFormViewModel = vendorFormBL.GetVendorFormList(FormStatus, vendorName, invoiceNo, refNo, fromDate, toDate, ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(vendorFormViewModel);
        }

        [HttpGet]
        public JsonResult GetVendorFormDetail(long vendorFormTrnId)
        {
            VendorFormBL vendorFormBL = new VendorFormBL();
            VendorFormViewModel vendorFormViewModel = new VendorFormViewModel();
            try
            {
                vendorFormViewModel = vendorFormBL.GetVendorFormDetail(vendorFormTrnId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(vendorFormViewModel, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public PartialViewResult GetPIList(string piNo = "", string vendorName = "", string refNo = "", string fromDate = "", string toDate = "",string approvalStatus="0" , string displayType = "")
        {
            List<PurchaseInvoiceViewModel> pis = new List<PurchaseInvoiceViewModel>();
            PurchaseInvoiceBL piBL = new PurchaseInvoiceBL();
            try
            {
                pis = piBL.GetPIList(piNo, vendorName, refNo, fromDate, toDate, ContextUser.CompanyId, approvalStatus, displayType);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(pis);
        }


      
    }
}
