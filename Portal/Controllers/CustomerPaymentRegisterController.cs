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

namespace Portal.Controllers
{

    [CheckSessionBeforeControllerExecuteAttribute(Order = 1)]
    public class CustomerPaymentRegisterController : BaseController
    {
        //
        // GET: /CustomerPaymentRegister/

        [ValidateRequest(true, UserInterfaceHelper.AddCustomer_Payment_Register, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListCustomerPaymentRegister()
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
        public PartialViewResult GetCustomerPaymentRegisterList(int customerId=0, int paymentModeId=0, string sortBy="", string sortOrder="", string fromDate="", string toDate="")
        {
        
            List<CustomerPaymentViewModel> customerPaymentList = new List<CustomerPaymentViewModel>();
            CustomerPaymentRegisterBL customerPaymentRegisterBL = new CustomerPaymentRegisterBL();
            try
            {

                customerPaymentList = customerPaymentRegisterBL.GetCustomerPaymentRegisterList(customerId, paymentModeId, sortBy, sortOrder, fromDate,toDate ,ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(customerPaymentList);
        }
    }
}
