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
    public class TaxController : BaseController
    {
        //
        // GET: /User/
        #region Tax
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Tax_ACCOUNT, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditTax(int taxId = 0, int accessMode = 3)
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Tax_ACCOUNT, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditTax(TaxViewModel taxViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            TaxBL taxBL = new TaxBL();
            try
            {
                if (taxViewModel != null)
                {
                    taxViewModel.CreatedBy = ContextUser.UserId;
                    taxViewModel.CompanyId = ContextUser.CompanyId;

                    responseOut = taxBL.AddEditTax(taxViewModel);
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


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Tax_ACCOUNT, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListTax()
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


        public PartialViewResult GetTaxList(string taxName = "",  string taxType = "", string taxSubType = "", string status = "")
        {
            List<TaxViewModel> taxes = new List<TaxViewModel>();
            TaxBL taxBL = new TaxBL();
            try
            {
                taxes = taxBL.GetTaxList(taxName, taxType, taxSubType, ContextUser.CompanyId, status);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(taxes);
        }


        public JsonResult GetTaxDetail(int taxId)
         {
            TaxBL taxBL = new TaxBL();
            TaxViewModel tax = new TaxViewModel();
            try
            {
                tax = taxBL.GetTaxDetail(taxId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(tax, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetGLAutoCompleteListForTax(string term)
        {
            GLBL glBL = new GLBL();

            List<GLViewModel> glList = new List<GLViewModel>();
            try
            {
                glList = glBL.GetGLAutoCompleteListForTax(term,ContextUser.CompanyId);

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
