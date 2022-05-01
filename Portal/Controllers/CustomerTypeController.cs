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
    public class CustomerTypeController : BaseController
    {
        //
        // GET: /Company/
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_CustomerType_CP, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditCustomerType(int customertypeId = 0, int accessMode = 3)
        {

            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                if (customertypeId != 0)
                {
                    ViewData["customertypeId"] = customertypeId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["customertypeId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_CustomerType_CP, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditCustomerType(CustomerTypeViewModel customertypeViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            CustomerTypeBL customertypeBL = new CustomerTypeBL();
            try
            {
                if (customertypeViewModel != null)
                {
                    responseOut = customertypeBL.AddEditCustomerType(customertypeViewModel);
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


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_CustomerType_CP, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListCustomerType()
        {
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }
        [HttpGet]
        public PartialViewResult GetCustomerTypeList(string customertypeDesc = "", string status = "",int companyBranchId=0)
        {
            List<CustomerTypeViewModel> customertypes = new List<CustomerTypeViewModel>();
            CustomerTypeBL customertypeBL = new CustomerTypeBL();
            try
            {
                customertypes = customertypeBL.GetCustomerTypeList(customertypeDesc, status, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(customertypes);
        }

        //[ValidateRequest(true, UserInterfaceHelper.Add_Edit_Company_CP, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        //public ActionResult SuperAdminDashboard()
        //{
        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
        //    }
        //    return View();
        //}


        [HttpGet]
        public JsonResult GetCustomerTypeDetail(int customertypeId)
        {
            CustomerTypeBL customertypeBL = new CustomerTypeBL();
            CustomerTypeViewModel customertype = new CustomerTypeViewModel();
            try
            {
                customertype = customertypeBL.GetCustomerTypeDetail(customertypeId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(customertype, JsonRequestBehavior.AllowGet);
        }

    }
}
