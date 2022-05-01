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
namespace Portal.Controllers
{
    [CheckSessionBeforeControllerExecuteAttribute(Order = 1)]
    public class SizeController : BaseController
    {
        //
        // GET: /Product/
        //
        // GET: /User/
        
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Size, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditSize(int sizeId = 0, int accessMode = 3)
        {

            try
            {
                if (sizeId != 0)
                {
                    ViewData["sizeId"] = sizeId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["sizeId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Size, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditSize(SizeViewModel sizeViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            SizeBL sizeBL = new SizeBL();
            try
            {
                if (sizeViewModel != null)
                {
                   
                    responseOut = sizeBL.AddEditSize(sizeViewModel);
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

       
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Size, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListSize()
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
        public PartialViewResult GetSizeList(string sizeDesc,string sizeCode, int productSubGroupId, int productMainGroupId, string sizeStatus)
        {
            List<SizeViewModel> sizes = new List<SizeViewModel>();
            SizeBL sizeBL = new SizeBL();
            try
            {
                sizes = sizeBL.GetSizeList(sizeDesc, sizeCode, productSubGroupId, productMainGroupId, sizeStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(sizes);
        }

        [HttpGet]
        public JsonResult GetSizeDetail(long sizeid)
        {
            SizeBL sizeBL = new SizeBL();
            SizeViewModel size = new SizeViewModel();
            try
            {
                size = sizeBL.GetSizeDetail(sizeid);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(size, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetProductAutoCompleteList(string term)
        {
            ProductBL productBL = new ProductBL();
            List<ProductViewModel> productList = new List<ProductViewModel>();
            try
            {
                productList = productBL.GetProductAutoCompleteList(term,ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(productList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetProductTypeList()
        {
            ProductTypeBL productTypeBL = new ProductTypeBL();
            List<ProductTypeViewModel> productTypeList = new List<ProductTypeViewModel>();
            try
            {
                productTypeList = productTypeBL.GetProductTypeList();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(productTypeList, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetProductMainGroupList()
        {
            ProductMainGroupBL productMainGroupBL = new ProductMainGroupBL();
            List<ProductMainGroupViewModel> productMainGroupList = new List<ProductMainGroupViewModel>();
            try
            {
                productMainGroupList = productMainGroupBL.GetProductMainGroupList();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(productMainGroupList, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetProductSubGroupList(int productMainGroupId)
        {
            ProductSubGroupBL productSubGroupBL = new ProductSubGroupBL();
            List<ProductSubGroupViewModel> productSubGroupList = new List<ProductSubGroupViewModel>();
            try
            {
                productSubGroupList = productSubGroupBL.GetProductSubGroupList(productMainGroupId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(productSubGroupList, JsonRequestBehavior.AllowGet);
        }
        

       

    
       
       
    }
}
