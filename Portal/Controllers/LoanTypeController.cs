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
    public class LoanTypeController : BaseController
    {
        //
        // GET: /Company/
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_LoanType, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditLoanType(int loantypeId = 0, int accessMode = 3)
        {

            try
            {
                if (loantypeId != 0)
                {
                    ViewData["loantypeId"] = loantypeId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["loantypeId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_LoanType, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditLoanType(HR_LoanTypeViewModel loantypeViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            LoanTypeBL loantypeBL = new LoanTypeBL();
            try
            {
                if (loantypeViewModel != null)
                {
                    responseOut = loantypeBL.AddEditLoanType(loantypeViewModel);
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


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_LoanType, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListLoanType()
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
        public PartialViewResult GetLoanTypeList(string loantypeName = "",  string loaninterestcalcOn="",  string loantypeStatus = "")
        {
            List<HR_LoanTypeViewModel> loantypes = new List<HR_LoanTypeViewModel>();
            LoanTypeBL loantypeBL = new LoanTypeBL();
            try
            {
                loantypes = loantypeBL.GetLoanTypeList(loantypeName, loaninterestcalcOn, loantypeStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(loantypes);
        }


        [HttpGet]
        public JsonResult GetLoanTypeDetail(int loantypeId)
        {
            LoanTypeBL loantypeBL = new LoanTypeBL();
            HR_LoanTypeViewModel loantype = new HR_LoanTypeViewModel();
            try
            {
                loantype = loantypeBL.GetLoanTypeDetail(loantypeId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(loantype, JsonRequestBehavior.AllowGet);
        }

    }
}
