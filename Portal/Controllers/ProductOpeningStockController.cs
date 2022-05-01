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
    public class ProductOpeningStockController : BaseController
    {
        //
        // GET: /ProductOpeningStock/

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Product_Opening_Stock, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditProductOpening(int openingTrnId = 0, int accessMode = 3)
        {

            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                if (openingTrnId != 0)
                {
                    ViewData["openingTrnId"] = openingTrnId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["openingTrnId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Product_Opening_Stock, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditProductOpening(ProductOpeningViewModel productOpeningViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            ProductOpeningBL productOpeningBL = new ProductOpeningBL();
            try
            {
                if (productOpeningViewModel != null)
                {
                    productOpeningViewModel.CompanyId = ContextUser.CompanyId;
                    productOpeningViewModel.CreatedBy = ContextUser.UserId;
                    responseOut = productOpeningBL.AddEditProductOpening(productOpeningViewModel);
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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Product_Opening_Stock, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListProductOpening()
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
        public PartialViewResult GetProductOpeningList(string productName, int finYearId, int companyBranchId)
        {
            List<ProductOpeningViewModel> productOpenings = new List<ProductOpeningViewModel>();
            ProductOpeningBL productOpeningBL = new ProductOpeningBL();
            try
            {

                productOpenings = productOpeningBL.GetProductOpeningList(productName, ContextUser.CompanyId, finYearId, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(productOpenings);
        }

        [HttpGet]
        public JsonResult GetProductOpeningDetail(long openingTrnId)
        {
            ProductOpeningBL productOpeningBL = new ProductOpeningBL();
            ProductOpeningViewModel productOpening = new ProductOpeningViewModel();
            try
            {
                productOpening = productOpeningBL.GetProductOpeningDetail(openingTrnId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(productOpening, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCompanyBranchList()
        {
            CompanyBL companyBL = new CompanyBL();
          
            List<CompanyBranchViewModel> companyViewModellist = new List<CompanyBranchViewModel>();
            try
            {
                companyViewModellist = companyBL.GetCompanyBranchList(ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(companyViewModellist, JsonRequestBehavior.AllowGet);
        }

    }
}
