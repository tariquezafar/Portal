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
    public class ProductMainGroupController : BaseController
    {
        //
        // GET: /Company/
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ProductMainGroup_INVENTORY, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditProductMainGroup(int productmaingroupId = 0, int accessMode = 3)
        {

            try
            {
                if (productmaingroupId != 0)
                {
                    ViewData["productmaingroupId"] = productmaingroupId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["productmaingroupId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ProductMainGroup_INVENTORY, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditProductMainGroup(ProductMainGroupViewModel productmaingroupViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            ProductMainGroupBL productmaingroupBL = new ProductMainGroupBL();
            try
            {
                if (productmaingroupViewModel != null)
                {
                    responseOut = productmaingroupBL.AddEditProductMainGroup(productmaingroupViewModel);
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


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ProductMainGroup_INVENTORY, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListProductMainGroup()
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
        public PartialViewResult GetProductMainGroupList(string productmaingroupName = "", string productmaingroupCode = "", string Status = "")
        {
            List<ProductMainGroupViewModel> productmaingroup = new List<ProductMainGroupViewModel>();
            ProductMainGroupBL productmaingroupBL = new ProductMainGroupBL();
            try
            {
                productmaingroup = productmaingroupBL.GetProductMainGroupList(productmaingroupName, productmaingroupCode, Status);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(productmaingroup);
        }


        [HttpGet]
        public JsonResult GetProductMainGroupDetail(int productmaingroupId)
        {
            ProductMainGroupBL productmaingroupBL = new ProductMainGroupBL();
            ProductMainGroupViewModel productmaingroup = new ProductMainGroupViewModel();
            try
            {
                productmaingroup = productmaingroupBL.GetProductMainGroupDetail(productmaingroupId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(productmaingroup, JsonRequestBehavior.AllowGet);
        }

    }
}
