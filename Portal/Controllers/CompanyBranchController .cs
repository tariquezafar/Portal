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
    public class CompanyBranchController : BaseController
    {
       
        [ValidateRequest(true,UserInterfaceHelper.Add_Edit_CompanyBranch_ADMIN, (int)AccessMode.AddAccess,(int)RequestMode.GetPost)]
        public ActionResult AddEditCompanyBranch(int CompanyBranchId = 0,int accessMode=3)
        {

            try
            {
                if (CompanyBranchId != 0)
                {
                    ViewData["CompanyBranchId"] = CompanyBranchId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["CompanyBranchId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_CompanyBranch_ADMIN, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditCompanyBranch(CompanyBranchViewModel companyBranchViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            CompanyBL companyBL = new CompanyBL();
            try
            {
                if (companyBranchViewModel != null)
                {


                    companyBranchViewModel.CompanyId = ContextUser.CompanyId;
                    responseOut = companyBL.AddEditCompanyBranch(companyBranchViewModel);
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


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_CompanyBranch_ADMIN, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListCompanyBranch()
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
        public PartialViewResult GetCompanyBranchList(string BranchName= "",string cityName="",string panNo="",string phoneNo="",string tinNo="",string companyBranchCode="",string manufactorLocationCode="")
        {
           
            List<CompanyBranchViewModel> companyBranchViewModel = new List<CompanyBranchViewModel>();
            CompanyBL companyBL = new CompanyBL();
            try
            {
                companyBranchViewModel = companyBL.GetCompanyBranchList(BranchName, cityName, panNo, phoneNo, tinNo, companyBranchCode, manufactorLocationCode);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(companyBranchViewModel);
        }    
        [HttpGet]
        public JsonResult GetCompanyBranchDetail(int companyBranchId)
        {
            CompanyBL countryBL = new CompanyBL();            
            CompanyBranchViewModel companyBranchViewModel = new CompanyBranchViewModel();
            try
            {
                companyBranchViewModel = countryBL.GetCompanyBracnhDetail(companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(companyBranchViewModel, JsonRequestBehavior.AllowGet);
        }

    }
}
