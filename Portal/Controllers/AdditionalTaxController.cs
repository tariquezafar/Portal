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
    public class AdditionalTaxController : BaseController
    {
        //
        // GET: /User/
        #region Tax
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_AdditionalTax_ACCOUNT, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditAdditionalTax(int taxId = 0, int accessMode = 3)
        {

            try
            {
                if (taxId != 0)
                {
                    ViewData["taxId"] = taxId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["taxId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_AdditionalTax_ACCOUNT, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditAdditionalTax(AdditionalTaxViewModel additionaltaxViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            AdditionalTaxBL additionaltaxBL = new AdditionalTaxBL();
            try
            {
                if (additionaltaxViewModel != null)
                {
                    additionaltaxViewModel.CreatedBy = ContextUser.UserId;
                    additionaltaxViewModel.CompanyId = ContextUser.CompanyId;

                    responseOut = additionaltaxBL.AddEditAdditionalTax(additionaltaxViewModel);
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


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_AdditionalTax_ACCOUNT, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListAdditionalTax()
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


        public PartialViewResult GetAdditionalTaxList(string taxName = "", string status = "")
        {
            List<AdditionalTaxViewModel> taxess = new List<AdditionalTaxViewModel>();
            AdditionalTaxBL additionaltaxBL = new AdditionalTaxBL();
            try
            {
                taxess = additionaltaxBL.GetAdditionalTaxList(taxName,ContextUser.CompanyId, status);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(taxess);
        }


        public JsonResult GetAddtionalTaxDetail(int taxId)
        {
            AdditionalTaxBL additionaltaxBL = new AdditionalTaxBL();
            AdditionalTaxViewModel additionaltax = new AdditionalTaxViewModel();
            try
            {
                additionaltax = additionaltaxBL.GetAddtionalTaxDetail(taxId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(additionaltax, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetGLAutoCompleteListForTax(string term)
        {
            GLBL glBL = new GLBL();

            List<GLViewModel> glList = new List<GLViewModel>();
            try
            {
                glList = glBL.GetGLAutoCompleteListForTax(term, ContextUser.CompanyId);

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(glList, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetSLAutoCompleteListForTax(string term)
        {
            SLBL slBL = new SLBL();

            List<SLViewModel> slList = new List<SLViewModel>();
            try
            {
                slList = slBL.GetSLAutoCompleteListForTax(term, ContextUser.CompanyId);

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(slList, JsonRequestBehavior.AllowGet);
        }





        #endregion
    }
}
