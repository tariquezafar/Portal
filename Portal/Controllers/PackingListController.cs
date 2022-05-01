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
    public class PackingListController : BaseController
    {
        //
        // GET: /PackingListType/
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PackingList, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditPackingList(int packingListId = 0, int accessMode = 3)
        {

            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                if (packingListId != 0)
                {
                    ViewData["packingListId"] = packingListId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["packingListId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PackingList, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditPackingList(PackingListViewModel packingListViewModel, List<PackingListDetailViewModel> packingListProducts)
        {
            ResponseOut responseOut = new ResponseOut();
            PackingListBL packingListBL = new PackingListBL();
            try
            {
                if (packingListViewModel != null)
                {
                    packingListViewModel.CreatedBy = ContextUser.UserId;
                    packingListViewModel.CompanyId = ContextUser.CompanyId;
                    packingListViewModel.ModifiedBy = ContextUser.UserId;
                    responseOut = packingListBL.AddEditPackingList(packingListViewModel, packingListProducts);

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

        [HttpPost]
        public PartialViewResult GetPackingListProductList(List<PackingListDetailViewModel> packingListProducts, long packingListId)
        {
            PackingListBL packingListBL = new PackingListBL();
            try
            {
                if (packingListProducts == null)
                {
                    packingListProducts = packingListBL.GetPackingListProductList(packingListId);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(packingListProducts);
        }

        [HttpGet]
        public JsonResult GetProductSubGroup()
        {
            PackingListBL packingListBL = new PackingListBL();
            List<PackingListViewModel> productSubGroupList = new List<PackingListViewModel>();
            try
            {
                productSubGroupList = packingListBL.GetProductSubGroup(); 
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(productSubGroupList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllPackingListType()
        {
            PackingListBL packingListBL = new PackingListBL();
            List<PackingListViewModel> allPackingListType = new List<PackingListViewModel>();
            try
            {
                allPackingListType = packingListBL.GetAllPackingListType();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(allPackingListType, JsonRequestBehavior.AllowGet);
        }

        // For Geting BOM ProductList from ProductSubGroupType 
        [HttpPost]
        public PartialViewResult GetPackingListBOMProductList( int productSubGroupId)
        {
            PackingListBL packingListBL = new PackingListBL();
            List<PackingListDetailViewModel> packingListProducts = new List<PackingListDetailViewModel>();
            try
            {
                
                    packingListProducts = packingListBL.GetPackingListBOMProductList(productSubGroupId);
                

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(packingListProducts);
        }

        // Get PackingListDetail i.e. PackingMaster Detail
        [HttpGet]
        public JsonResult GetPackingListDetail(long packingListId)
        {
            PackingListBL packingListBL = new PackingListBL();
            PackingListViewModel packingListViewModel = new PackingListViewModel();
            try
            {
                packingListViewModel = packingListBL.GetPackingListDetail(packingListId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(packingListViewModel, JsonRequestBehavior.AllowGet);
        }


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PackingList, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListPackingList()
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;
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
        public PartialViewResult GetPackingList(string packingListName = "", string packingListStatus = "",int companyBranchId=0)
        {
            List<PackingListViewModel> PackingList = new List<PackingListViewModel>();
            PackingListBL packingListBL = new PackingListBL();
            try
            {
                PackingList = packingListBL.GetPackingList(packingListName, packingListStatus, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(PackingList);
        }

    }
}
