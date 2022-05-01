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
    public class ProductGLMappingController :BaseController
    {
        //
        // GET: /ProductTaxMapping/

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ProductGLMapping, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditProductGLMapping(int mappingId = 0, int accessMode = 3)
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ProductGLMapping, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditProductGLMapping(ProductGLMappingViewModel productGLMapping)
        {
            ResponseOut responseOut = new ResponseOut();
            ProductGLMappingBL productGLMappingBL = new ProductGLMappingBL();
            try
            {
                if (productGLMapping != null)
                {
                   responseOut= productGLMappingBL.AddEditProductGLMapping(productGLMapping);
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

        public ActionResult ListProductGLMapping()
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
        public PartialViewResult GetProductGLMappingList(int productsubgroupid = 0,int glId = 0)
        { 
            List<ProductGLMappingViewModel> productGLMappings = new List<ProductGLMappingViewModel>();
            ProductGLMappingBL productGLMappingBL = new ProductGLMappingBL();
            try
            {
                productGLMappings = productGLMappingBL.GetProductGLMappingList(productsubgroupid, glId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(productGLMappings);
        }


        [HttpGet]
        public JsonResult GetProductGLDetail(int mappingId)
        {
            ProductGLMappingBL productGLMappingBL = new ProductGLMappingBL();
            ProductGLMappingViewModel productGLMappingViewModel = new ProductGLMappingViewModel();
            try
            {

                productGLMappingViewModel = productGLMappingBL.GetProductGLDetail(mappingId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(productGLMappingViewModel, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetGLAutoCompleteListForProductGLMapping(string term)
        {
            GLBL glBL = new GLBL();

            List<GLViewModel> glList = new List<GLViewModel>();
            try
            {
                glList = glBL.GetGLAutoCompleteListForProductGLMapping(term, ContextUser.CompanyId);

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(glList, JsonRequestBehavior.AllowGet);
        }



    }
}
