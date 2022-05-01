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
    public class CompanyController : BaseController
    {
        //
        // GET: /Company/
        [ValidateRequest(true,UserInterfaceHelper.Add_Edit_Company_CP,(int)AccessMode.ViewAccess,(int)RequestMode.GetPost)]
        public ActionResult AddEditCompany(int companyId=0,int accessMode=3)
        {

            try
            {
                if (companyId!=0)
                {
                    ViewData["companyId"] = companyId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["companyId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Company_CP, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditCompany(CompanyViewModel companyViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            CompanyBL companyBL = new CompanyBL();
            try
            {
                if (companyViewModel!=null)
                {
                    responseOut = companyBL.AddEditCompany(companyViewModel);
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


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Company_CP, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListCompany()
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
        public PartialViewResult GetCompanyList(string companyName="",string cityName="",string panNo="",string phoneNo="",string tinNo="")
        {
            List<CompanyViewModel> companies = new List<CompanyViewModel>();
            CompanyBL companyBL = new CompanyBL();
            try
            {
                companies = companyBL.GetCompanyList(companyName, cityName, panNo, phoneNo, tinNo);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(companies);
        }

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Company_CP, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult SuperAdminDashboard()
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
        public JsonResult GetCountryList()
        {
            CountryBL countryBL = new CountryBL();
            List<CountryViewModel> countryList = new List<CountryViewModel>();
            try
            {
                countryList = countryBL.GetCountryList();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(countryList, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetStateList(int countryId)
        {
            StateBL stateBL = new StateBL();
            List<StateViewModel> stateList = new List<StateViewModel>();
            try
            {
                stateList = stateBL.GetStateList(countryId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(stateList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCompanyDetail(int companyId)
        {
            CompanyBL countryBL = new CompanyBL();
            CompanyViewModel company = new CompanyViewModel();
            try
            {
                company = countryBL.GetCompanyDetail(companyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(company, JsonRequestBehavior.AllowGet);
        }

    }
}
