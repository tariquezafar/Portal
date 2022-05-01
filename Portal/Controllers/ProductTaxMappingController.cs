using Portal.Common;
using Portal.Core;

using Portal.Core.ViewModel;
using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Portal.Controllers
{
    [CheckSessionBeforeControllerExecuteAttribute(Order = 1)]
    public class ProductTaxMappingController :BaseController
    {
        //
        // GET: /ProductTaxMapping/

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ProductTaxMapping, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditProductTaxMapping(int mappingId = 0, int accessMode = 3)
        {
            try
            {
                if (mappingId != 0)
                {
                    ViewData["mappingId"] = mappingId;
                    ViewData["accessMode"] = accessMode;
                    ViewData["CompanyId"] = ContextUser.CompanyId;
                    ViewData["CreatedBy"] = ContextUser.UserId;
                }
                else
                {
                    ViewData["mappingId"] = 0;
                    ViewData["accessMode"] = 0;
                    ViewData["CompanyId"] = ContextUser.CompanyId;
                    ViewData["CreatedBy"] = ContextUser.UserId;
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }

        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ProductTaxMapping, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditProductTaxMapping(ProductSubCategoryStateTaxMappingViewModel productSubCategoryStateTaxMapping)
        {
            ResponseOut responseOut = new ResponseOut();
            ProductTaxMappingBL productTaxMappingBL = new ProductTaxMappingBL();
            try
            {
                if (productSubCategoryStateTaxMapping != null)
                {
                   responseOut=productTaxMappingBL.AddEditProductTaxMapping(productSubCategoryStateTaxMapping);
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

        public ActionResult ListProductTaxMapping()
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
        public PartialViewResult GetProductTaxMappingList(int productsubgroupid = 0, int stateId = 0, int taxId = 0)
        {
            List<ProductSubCategoryStateTaxMappingViewModel> productTaxMappings = new List<ProductSubCategoryStateTaxMappingViewModel>();
            ProductTaxMappingBL productTaxMappingBL = new ProductTaxMappingBL();
           try
            {
                productTaxMappings = productTaxMappingBL.GetProductTaxMappingList(productsubgroupid, stateId, taxId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(productTaxMappings);
        }


        [HttpGet]
        public JsonResult GetProductStateTaxDetail(int mappingId)
        {
            ProductTaxMappingBL productTaxMappingBL = new ProductTaxMappingBL();
            ProductSubCategoryStateTaxMappingViewModel productSubCategoryStateTaxMappingViewModel = new ProductSubCategoryStateTaxMappingViewModel();
            try
            {

                productSubCategoryStateTaxMappingViewModel = productTaxMappingBL.GetProductStateTaxDetail(mappingId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(productSubCategoryStateTaxMappingViewModel, JsonRequestBehavior.AllowGet);
        }



    }
}
