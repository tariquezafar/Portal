using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Portal.Core;
using Portal.Core.ViewModel;
using Portal.Common;
using System.Reflection;
using System.Data;
using Microsoft.Reporting.WebForms;
using System.IO;
namespace Portal.Controllers
{
    [CheckSessionBeforeControllerExecuteAttribute(Order = 1)]
    public class SaleVoucherController : BaseController
    {
        #region SaleVoucher Entry
        //
        // GET: /SaleVoucher/


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_SaleVoucher, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditSaleVoucher(int voucherId = 0, int accessMode = 3, string ReportLevel = "", string FromDate = "", string ToDate = "", string StartInterface = "", int GLMainGroupId = 0, int GLSubGroupId = 0, Int32 GLId = 0, Int32 SLId = 0, string checkBalanceSheet = "")
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                ViewData["reportLevel"] = ReportLevel;
                ViewData["drillFromDate"] = FromDate;
                ViewData["drillToDate"] = ToDate;
                ViewData["startInterface"] = StartInterface;
                ViewData["gLMainGroupId"] = GLMainGroupId;
                ViewData["gLSubGroupId"] = GLSubGroupId;
                ViewData["glId"] = GLId;
                ViewData["slId"] = SLId;
                ViewData["checkBalanceSheet"] = checkBalanceSheet;

                if (voucherId != 0)
                {
                    ViewData["svId"] = voucherId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["svId"] = 0;
                    ViewData["accessMode"] = 0;
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }



        [HttpGet]
        public PartialViewResult GetSaleVoucherEntryList(List<VoucherDetailViewModel> voucherEntryList, long voucherId)
        {
            
            SaleVoucherBL saleVoucherBL = new SaleVoucherBL();

            try
            {
                if (voucherEntryList == null)
                {
                    voucherEntryList = saleVoucherBL.GetSaleVoucherEntryList(voucherId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(voucherEntryList);
        }
        #endregion
    }
}
