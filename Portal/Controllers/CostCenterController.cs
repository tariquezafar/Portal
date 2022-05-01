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
    public class CostCenterController : BaseController
    {
        //
        // GET: /Company/
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_CostCenter_ACCOUNT, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditCostCenter(int costcenterId = 0, int accessMode = 3)
        {

            try
            {

                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                if (costcenterId != 0)
                {
                    ViewData["costcenterId"] = costcenterId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["costcenterId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_CostCenter_ACCOUNT, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditCostCenter(CostCenterViewModel costcenterViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            CostCenterBL costcenterBL = new CostCenterBL();
            try
            {
                if (costcenterViewModel != null)
                {
                    costcenterViewModel.CreatedBy = ContextUser.UserId;
                    costcenterViewModel.CompanyId = ContextUser.CompanyId;
                    responseOut = costcenterBL.AddEditCostCenter(costcenterViewModel);
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


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_CostCenter_ACCOUNT, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListCostCenter()
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
        public PartialViewResult GetCostCenterList(string costcenterName = "", string status = "",int companyBranchId=0)
        {
            List<CostCenterViewModel> costcenter = new List<CostCenterViewModel>();
            CostCenterBL costcenterBL = new CostCenterBL();
            try
            {
                costcenter = costcenterBL.GetCostCenterList(costcenterName,ContextUser.CompanyId, status, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(costcenter);
        }


        [HttpGet]
        public JsonResult GetCostCenterDetail(int costcenterId)
        {
            CostCenterBL costcenterBL = new CostCenterBL();
            CostCenterViewModel costcenter = new CostCenterViewModel();
            try
            {
                costcenter = costcenterBL.GetCostCenterDetail(costcenterId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(costcenter, JsonRequestBehavior.AllowGet);
        }

    }
}
