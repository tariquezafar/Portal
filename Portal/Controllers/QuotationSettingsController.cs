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
    public class QuotationSettingsController : BaseController
    {
        //
        // GET: /User/
        #region QuotationSetting
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_QuotationSetting_SALE, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditQuotationSetting(int quotationsettingId = 0, int accessMode = 3)
        {

            try
            {
                if (quotationsettingId != 0)
                {
                    ViewData["quotationsettingId"] = quotationsettingId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["quotationsettingId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_QuotationSetting_SALE, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditQuotationSetting(QuotationSettingViewModel quotationsettingViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            QuotationSettingBL quotationsettingBL = new QuotationSettingBL();
            try
            {
                if (quotationsettingViewModel != null)
                {
                    quotationsettingViewModel.CreatedBy = ContextUser.UserId;
                    quotationsettingViewModel.CompanyId = ContextUser.CompanyId;

                    responseOut = quotationsettingBL.AddEditQuotationSetting(quotationsettingViewModel);
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


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_QuotationSetting_SALE, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListQuotationSetting()
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


        public PartialViewResult GetQuotationSettingList(string status = "")
        {
            List<QuotationSettingViewModel> quotationsettings = new List<QuotationSettingViewModel>();
            QuotationSettingBL quotationsettingBL = new QuotationSettingBL();
            try
            {
                quotationsettings = quotationsettingBL.GetQuotationSettingList(ContextUser.CompanyId, status);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(quotationsettings);
        }


        public JsonResult GetQuotationSettingDetail(int quotationsettingId)
        {
            QuotationSettingBL quotationsettingBL = new QuotationSettingBL();
            QuotationSettingViewModel quotationsetting = new QuotationSettingViewModel();
            try
            {
                quotationsetting = quotationsettingBL.GetQuotationSettingDetail(quotationsettingId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(quotationsetting, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetUserAutoCompleteList(string term)
        {
            QuotationSettingBL quotationsettingBL = new QuotationSettingBL();

            List<UserViewModel> userList = new List<UserViewModel>();
            try
            {
                userList = quotationsettingBL.GetUserAutoCompleteList(term);

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(userList, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
