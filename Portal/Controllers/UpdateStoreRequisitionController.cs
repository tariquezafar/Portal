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
    public class UpdateStoreRequisitionController :BaseController
    {
        //
        // GET: /Update StoreRequisition/

        #region Update Store Requisition


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_StoreRequisition, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult UpdateApprovedStoreRequisition(int storeRequisitionId = 0, int accessMode = 3)
        {

            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();

                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                if (storeRequisitionId != 0)
                {

                    ViewData["storeRequisitionId"] = storeRequisitionId;
                    ViewData["accessMode"] = accessMode;
                    ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                }
                else
                {
                    ViewData["storeRequisitionId"] = 0;
                    ViewData["accessMode"] = 0;
                    ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }

        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_StoreRequisition, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult UpdateApprovedStoreRequisition(StoreRequisitionViewModel storeRequisitionViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            StoreRequisitionBL storeRequisitionBL = new StoreRequisitionBL();

            try
            {
                if (storeRequisitionViewModel != null)
                {
                    storeRequisitionViewModel.CompanyId = ContextUser.CompanyId;
                    storeRequisitionViewModel.ApprovedBy = ContextUser.UserId;
                    storeRequisitionViewModel.RejectedBy = ContextUser.UserId;
                    responseOut = storeRequisitionBL.ApproveRejectStoreRequisition(storeRequisitionViewModel);
                }
                else
                {
                    responseOut.message = ActionMessage.ProbleminData;
                    responseOut.status = ActionStatus.Fail;
                    responseOut.trnId = 0;
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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_StoreRequisition, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListApprovedStoreRequisition()
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

        [HttpPost]
        public PartialViewResult GetStoreRequisitionProductApprovalList(List<StoreRequisitionProductDetailViewModel> storeRequisitionProducts, long requisitionId)
        {
            StoreRequisitionBL storeRequisitionBL = new StoreRequisitionBL();
            try
            {
                if (storeRequisitionProducts == null)
                {
                    storeRequisitionProducts = storeRequisitionBL.GetStoreRequisitionProductList(requisitionId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(storeRequisitionProducts);
        }

        [HttpGet]
        public JsonResult GetStoreRequisitionApprovalDetail(long requisitionId)
        {
            StoreRequisitionBL storeRequisitionBL = new StoreRequisitionBL();
            StoreRequisitionViewModel storeRequisition = new StoreRequisitionViewModel();
            try
            {
                storeRequisition = storeRequisitionBL.GetStoreRequisitionDetail(requisitionId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(storeRequisition, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public PartialViewResult GetApprovedStoreRequisitionList(string requisitionNo = "", string workOrderNo = "", string requisitionType = "", string customerName = "", int companyBranchId = 0, string fromDate = "", string toDate = "", string displayType = "", string approvalStatus = "")
        {
            List<StoreRequisitionViewModel> requisitions = new List<StoreRequisitionViewModel>();
            StoreRequisitionBL storeRequisitionBL = new StoreRequisitionBL();

            try
            {
                requisitions = storeRequisitionBL.GetStoreRequisitionApprovelList(requisitionNo, workOrderNo, requisitionType, customerName, companyBranchId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), ContextUser.CompanyId, displayType, approvalStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(requisitions);
        }
        
        public JsonResult GetBranchLocationList(int companyBranchID)
        {
            LocationBL locationBL = new LocationBL();
            List<LocationViewModel> locationViewModel = new List<LocationViewModel>();
            try
            {
                locationViewModel = locationBL.GetFromLocationList(companyBranchID);

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(locationViewModel, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}
