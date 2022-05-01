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
using System.IO;

namespace Portal.Controllers
{
    [CheckSessionBeforeControllerExecuteAttribute(Order = 1)]
    public class DashboardController : BaseController
    {
        //
        // GET: /ModuleDashboard/

        public ActionResult ModuleDashboard()
        {
            try
            {
                UserBL userBL = new UserBL();
                SOBL SO = new SOBL();
                InventoryDashboardBL inventoryDashboardBL = new InventoryDashboardBL();
                ViewData["TotalUsersCount"] = userBL.GetTotalUsersCount(ContextUser.CompanyId);
                FinYearViewModel currentFinYear = (FinYearViewModel)Session[SessionKey.CurrentFinYear];
                ViewData["totalProductCount"] = inventoryDashboardBL.GetTotalProductCount(ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }
        public ActionResult AdminDashboard()
        {
            ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
            ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;
            try
            {
                UserBL userBL = new UserBL();
                RoleBL roleBL = new RoleBL();
                ViewData["TodayUsersCount"] = userBL.GetTodayUsersCount(ContextUser.CompanyId);
                ViewData["TotalUsersCount"] = userBL.GetTotalUsersCount(ContextUser.CompanyId);
                ViewData["TotalRolesCount"] = roleBL.GetTotalRolesCount(ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }
        public ActionResult CRMDashboard()
        {
            try
            {
                CRMDashboardBL cRMDashboardBL = new CRMDashboardBL();
                FinYearViewModel currentFinYear = (FinYearViewModel)Session[SessionKey.CurrentFinYear];
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["currentFinyearId"] = currentFinYear.FinYearId;
                

                int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                HRDashboardBL hrDashboardBL = new HRDashboardBL();
                List<InventoryDashboardItemsViewModel> inventoryDashboardItems = hrDashboardBL.GetHRDashboardItems(ContextUser.RoleId, ContextUser.CompanyId, Convert.ToInt32(ContextUser.CompanyBranchId), finYearId);
                if (inventoryDashboardItems.Count > 0 && inventoryDashboardItems != null)
                {
                    foreach (InventoryDashboardItemsViewModel item in inventoryDashboardItems)
                    {
                        if (item.BoxNumber == "1")
                        {
                            ViewData["Box1ItemKey"] = item.ContainerItemKey;
                            ViewData["Box1ItemValue"] = item.ContainerItemValue;
                        }
                        if (item.BoxNumber == "2")
                        {
                            ViewData["Box2ItemKey"] = item.ContainerItemKey;
                            ViewData["Box2ItemValue"] = item.ContainerItemValue;
                        }
                        if (item.BoxNumber == "3")
                        {
                            ViewData["Box3ItemKey"] = item.ContainerItemKey;
                            ViewData["Box3ItemValue"] = item.ContainerItemValue;
                        }
                        if (item.BoxNumber == "4")
                        {
                            ViewData["Box4ItemKey"] = item.ContainerItemKey;
                            ViewData["Box4ItemValue"] = item.ContainerItemValue;
                        }
                        if (item.BoxNumber == "6")
                        {
                            ViewData["Box6ItemKey"] = item.ContainerItemKey;
                            ViewData["Box6ItemValue"] = item.ContainerItemValue;
                        }
                        if (item.BoxNumber == "7")
                        {
                            ViewData["Box7ItemKey"] = item.ContainerItemKey;
                            ViewData["Box7ItemValue"] = item.ContainerItemValue;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }
        public ActionResult CRMTeamDashboard()
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
        
        public ActionResult SaleDashboard(int companyBranchId=0)
        {
            try
            {
                SalePendingPaymentCountViewModel salePendingPaymentCount = new SalePendingPaymentCountViewModel();
                SaleDashboardTargetAmountViewModel saleDashboardTargetAmount = new SaleDashboardTargetAmountViewModel();
                SOCountViewModel SOcount = new SOCountViewModel();
                SICountViewModel saledashboard = new SICountViewModel();
                SOBL SO = new SOBL();
                CustomerBL customer = new CustomerBL();
                InventoryDashboardBL inventoryDashboardBL = new InventoryDashboardBL();
                FinYearViewModel currentFinYear = (FinYearViewModel)Session[SessionKey.CurrentFinYear];
                int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                companyBranchId = (companyBranchId == 0) ? Convert.ToInt32(ContextUser.CompanyBranchId) : companyBranchId;
                ViewData["CompanyBranchId"] = companyBranchId;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;
                ViewData["currentFinyearId"] = finYearId;
                CustomerCountViewModel customerCount = customer.GetNewCustomerCount(ContextUser.CompanyId);
                /*----------------------------------------New Code For Sale Dashboad Start -----------------------------------*/
                List<SaleDashboardItemsViewModel> saleDashboardItems = customer.GetSaleDashboardItems(ContextUser.RoleId,ContextUser.CompanyId,Convert.ToInt32(ContextUser.CompanyBranchId),finYearId);
                if(saleDashboardItems.Count>0 && saleDashboardItems!=null)
                {
                    foreach (SaleDashboardItemsViewModel item in saleDashboardItems)
                    {
                        if (item.BoxNumber == "1")
                        {
                            ViewData["Box1ItemKey"] = item.ContainerItemKey;
                            ViewData["Box1ItemValue"] = item.ContainerItemValue;
                        }
                        if(item.BoxNumber == "2")
                        {
                            ViewData["Box2ItemKey"] = item.ContainerItemKey;
                            ViewData["Box2ItemValue"] = item.ContainerItemValue;
                        }
                        if (item.BoxNumber == "3")
                        {
                            ViewData["Box3ItemKey"] = item.ContainerItemKey;
                            ViewData["Box3ItemValue"] = item.ContainerItemValue;
                        }
                        if (item.BoxNumber == "4")
                        {
                            ViewData["Box4ItemKey"] = item.ContainerItemKey;
                            ViewData["Box4ItemValue"] = item.ContainerItemValue;
                        }
                        if (item.BoxNumber == "5")
                        {
                            ViewData["Box5ItemKey"] = item.ContainerItemKey;
                            ViewData["Box5ItemValue"] = item.ContainerItemValue;
                        }
                        if (item.BoxNumber == "6")
                        {
                            ViewData["Box6ItemKey"] = item.ContainerItemKey;
                            ViewData["Box6ItemValue"] = item.ContainerItemValue;
                        }
                        if (item.BoxNumber == "7")
                        {
                            ViewData["Box7ItemKey"] = item.ContainerItemKey;
                            ViewData["Box7ItemValue"] = item.ContainerItemValue;
                        }
                        if (item.BoxNumber == "8")
                        {
                            ViewData["Box8ItemKey"] = item.ContainerItemKey;
                            ViewData["Box8ItemValue"] = item.ContainerItemValue;
                        }
                        if (item.BoxNumber == "9" && item.ContainerItemKey.Trim() == "Today Sale")
                        {
                             ViewData["Box9TodaySale"] = item.ContainerItemKey;
                            ViewData["Box9TodaySaleCnt"] = item.ContainerItemValue;
                        }
                        if (item.BoxNumber == "9" && item.ContainerItemKey.Trim() == "Today Sale Amt")
                        {
                            ViewData["Box9TodaySale"] = item.ContainerItemKey;
                            ViewData["Box9TodaySaleAmt"] = item.ContainerItemValue;
                        }
                        if (item.BoxNumber == "9" && item.ContainerItemKey.Trim() == "MTD Sale")
                        {
                            ViewData["Box9MTDSale"] = item.ContainerItemKey;
                            ViewData["Box9MTDSaleCnt"] = item.ContainerItemValue;
                        }
                        if (item.BoxNumber == "9" && item.ContainerItemKey.Trim() == "MTD Sale Amt")
                        {
                            ViewData["Box9MTDSale"] = item.ContainerItemKey;
                            ViewData["Box9MTDSaleAmt"] = item.ContainerItemValue;
                        }

                        if (item.BoxNumber == "10" && item.ContainerItemKey.Trim() == "Today SQ")
                        {
                            ViewData["Box10TodaySQ"] = item.ContainerItemKey;
                            ViewData["Box10TodaySQCnt"] = item.ContainerItemValue;
                        }
                        if (item.BoxNumber == "10" && item.ContainerItemKey.Trim() == "Today SQ Amt")
                        {
                            ViewData["Box10TodaySQ"] = item.ContainerItemKey;
                            ViewData["Box10TodaySQAmt"] = item.ContainerItemValue;
                        }
                        if (item.BoxNumber == "10" && item.ContainerItemKey.Trim() == "MTD SQ")
                        {
                            ViewData["Box10MTDSQ"] = item.ContainerItemKey;
                            ViewData["Box10MTDSQCnt"] = item.ContainerItemValue;
                        }
                        if (item.BoxNumber == "10" && item.ContainerItemKey.Trim() == "MTD SQ Amt")
                        {
                            ViewData["Box10MTDSQ"] = item.ContainerItemKey;
                            ViewData["Box10MTDSQAmt"] = item.ContainerItemValue;
                        }
                        if (item.BoxNumber == "10" && item.ContainerItemKey.Trim() == "YTD SQ")
                        {
                            ViewData["Box10YTDSQ"] = item.ContainerItemKey;
                            ViewData["Box10YTDSQCnt"] = item.ContainerItemValue;
                        }
                        if (item.BoxNumber == "10" && item.ContainerItemKey.Trim() == "YTD SQ Amt")
                        {
                            ViewData["Box10YTDSQ"] = item.ContainerItemKey;
                            ViewData["Box10YTDSQAmt"] = item.ContainerItemValue;
                        }
                        /*-------------------------------------------------------------*/
                        if (item.BoxNumber == "11" && item.ContainerItemKey.Trim() == "Today SO")
                        {
                            ViewData["Box11TodaySO"] = item.ContainerItemKey;
                            ViewData["Box11TodaySOCnt"] = item.ContainerItemValue;
                        }
                        if (item.BoxNumber == "11" && item.ContainerItemKey.Trim() == "Today SO Amt")
                        {
                            ViewData["Box11TodaySO"] = item.ContainerItemKey;
                            ViewData["Box11TodaySOAmt"] = item.ContainerItemValue;
                        }
                        if (item.BoxNumber == "11" && item.ContainerItemKey.Trim() == "MTD SO")
                        {
                            ViewData["Box11MTDSO"] = item.ContainerItemKey;
                            ViewData["Box11MTDSOCnt"] = item.ContainerItemValue;
                        }
                        if (item.BoxNumber == "11" && item.ContainerItemKey.Trim() == "MTD SO Amt")
                        {
                            ViewData["Box11MTDSO"] = item.ContainerItemKey;
                            ViewData["Box11MTDSOAmt"] = item.ContainerItemValue;
                        }
                        if (item.BoxNumber == "11" && item.ContainerItemKey.Trim() == "YTD SO")
                        {
                            ViewData["Box11YTDSO"] = item.ContainerItemKey;
                            ViewData["Box11YTDSOCnt"] = item.ContainerItemValue;
                        }
                        if (item.BoxNumber == "11" && item.ContainerItemKey.Trim() == "YTD SO Amt")
                        {
                            ViewData["Box11YTDSO"] = item.ContainerItemKey;
                            ViewData["Box11YTDSOAmt"] = item.ContainerItemValue;
                        }
                    }
                }

                

                /*----------------------------------------New Code For Sale Dashboard END -----------------------------------*/
                ViewData["New_Customer_Count"] = customerCount.NewCustomer;
                customerCount = customer.GetTotalCustomerCount(ContextUser.CompanyId);
                ViewData["Total_Customer_Count"] = customerCount.TotalCustomer;
                SOcount = SO.GetSOCount(ContextUser.CompanyId, finYearId, companyBranchId);
                ViewData["SO_Count"] = SOcount.sOTotalCount;

                //Total Packing

                saledashboard = inventoryDashboardBL.GetSaleQutationDashboard(ContextUser.CompanyId, finYearId, ContextUser.UserId, companyBranchId, 12);
                ViewData["totalInvoicePackingCount"] = saledashboard.SITotalCount;

                //Total Sale Return

                saledashboard = inventoryDashboardBL.GetSaleQutationDashboard(ContextUser.CompanyId, finYearId, ContextUser.UserId, companyBranchId, 13);
                ViewData["totalSaleReturn"] = saledashboard.SITotalAmountSum;

                //Total Sale Target

                saledashboard = inventoryDashboardBL.GetSaleQutationDashboard(ContextUser.CompanyId, finYearId, ContextUser.UserId, companyBranchId, 14);
                ViewData["totalSaleTarget"] = saledashboard.SITotalCount;
                
                //For Sale 

                saledashboard = inventoryDashboardBL.GetSaleQutationDashboard(ContextUser.CompanyId, finYearId, ContextUser.UserId, companyBranchId, 4);
                ViewData["todaySaleCount"] = saledashboard.SITotalCount;
                ViewData["todaysaleamount"] = saledashboard.SITotalAmountSum;

                saledashboard = inventoryDashboardBL.GetSaleQutationDashboard(ContextUser.CompanyId, finYearId, ContextUser.UserId, companyBranchId, 5);
                ViewData["mtdsalecount"] = saledashboard.SITotalCount;
                ViewData["mtdsaleamount"] = saledashboard.SITotalAmountSum;

                saledashboard = inventoryDashboardBL.GetSaleQutationDashboard(ContextUser.CompanyId, finYearId, ContextUser.UserId, companyBranchId, 6);
                ViewData["ytdsalecount"] = saledashboard.SITotalCount;
                ViewData["ytdsaleamount"] = saledashboard.SITotalAmountSum;

                //For Sale Order

                saledashboard = inventoryDashboardBL.GetSaleQutationDashboard(ContextUser.CompanyId, finYearId, ContextUser.UserId, companyBranchId, 7);
                ViewData["todaySaleoCount"] = saledashboard.SITotalCount;
                ViewData["todaysaleoamount"] = saledashboard.SITotalAmountSum;

                saledashboard = inventoryDashboardBL.GetSaleQutationDashboard(ContextUser.CompanyId, finYearId, ContextUser.UserId, companyBranchId, 8);
                ViewData["mtdsaleocount"] = saledashboard.SITotalCount;
                ViewData["mtdsaleoamount"] = saledashboard.SITotalAmountSum;

                saledashboard = inventoryDashboardBL.GetSaleQutationDashboard(ContextUser.CompanyId, finYearId, ContextUser.UserId, companyBranchId, 9);
                ViewData["ytdsaleocount"] = saledashboard.SITotalCount;
                ViewData["ytdsaleoamount"] = saledashboard.SITotalAmountSum;
                


                //For Sale Quation 
                saledashboard = inventoryDashboardBL.GetSaleQutationDashboard(ContextUser.CompanyId, finYearId, ContextUser.UserId, companyBranchId, 1);
                ViewData["todayQuotationCount"] = saledashboard.SITotalCount;
                ViewData["todayQuotationamount"] = saledashboard.SITotalAmountSum;
                saledashboard = inventoryDashboardBL.GetSaleQutationDashboard(ContextUser.CompanyId, finYearId, ContextUser.UserId, companyBranchId, 2);
                ViewData["ytdQuotationCount"] = saledashboard.SITotalCount;
                ViewData["ytdQuotationamount"] = saledashboard.SITotalAmountSum;
                saledashboard = inventoryDashboardBL.GetSaleQutationDashboard(ContextUser.CompanyId, finYearId, ContextUser.UserId, companyBranchId, 3);
                ViewData["mtdQuotationCount"] = saledashboard.SITotalCount;
                ViewData["mtdQuotationamount"] = saledashboard.SITotalAmountSum;

                saledashboard = inventoryDashboardBL.GetSaleQutationDashboard(ContextUser.CompanyId, finYearId, ContextUser.UserId, companyBranchId, 10);
                ViewData["total_so_for_wo"] = saledashboard.SITotalCount;
                ViewData["total_so_for_wo_Amount"] = saledashboard.SITotalAmountSum;

                //saledashboard = inventoryDashboardBL.GetSaleQutationDashboard(ContextUser.CompanyId, finYearId, ContextUser.UserId, companyBranchId, 11);
                //ViewData["total_wo_pending"] = saledashboard.SITotalCount;
                salePendingPaymentCount = inventoryDashboardBL.GetSalePendingPaymentCountDashboard(ContextUser.CompanyId,finYearId, ContextUser.UserId, companyBranchId,1);
                ViewData["pendingInvoiceCount"] = salePendingPaymentCount.salePendingInvoiceCount;
                ViewData["salePendingInvoiceAmount"] = salePendingPaymentCount.salePendingInvoiceAmount;

                saleDashboardTargetAmount = inventoryDashboardBL.GetSalePendingTargetCountDashboard(ContextUser.CompanyId, finYearId,companyBranchId);
                ViewData["targetAmount"] = saleDashboardTargetAmount.TargetAmount;
                ViewData["totalInvoiceAmount"] = saleDashboardTargetAmount.TotalInvoiceAmount;

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }
        /* Start Code for Parctial View for Sale Dashboard  Container 9 10 11 */
        [HttpPost]
        public PartialViewResult GetContainer9List()
        {
            List<Container9ViewModel> container9List = new List<Container9ViewModel>();
            SaleDashboardBL saledashboardBL = new SaleDashboardBL();
            try
            {
                int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                container9List = saledashboardBL.GetContainer9List(ContextUser.RoleId, ContextUser.CompanyId,Convert.ToInt32(ContextUser.CompanyBranchId), finYearId, 9);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(container9List);
        }
        [HttpPost]
        public PartialViewResult GetContainer10List()
        {
            List<Container9ViewModel> container10List = new List<Container9ViewModel>();
            SaleDashboardBL saledashboardBL = new SaleDashboardBL();
            try
            {
                int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                container10List = saledashboardBL.GetContainer9List(ContextUser.RoleId, ContextUser.CompanyId, Convert.ToInt32(ContextUser.CompanyBranchId), finYearId, 10);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(container10List);
        }
        [HttpPost]
        public PartialViewResult GetContainer11List()
        {
            List<Container9ViewModel> container11List = new List<Container9ViewModel>();
            SaleDashboardBL saledashboardBL = new SaleDashboardBL();
            try
            {
                int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                container11List = saledashboardBL.GetContainer9List(ContextUser.RoleId, ContextUser.CompanyId, Convert.ToInt32(ContextUser.CompanyBranchId), finYearId, 11);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(container11List);
        }
        /* End Code for Parctial View for Sale Dashboard  Container 9 10 11 */
        public ActionResult SaleTeamDashboard()
        {
            try
            {
                CustomerBL customer = new CustomerBL();
                SOBL SO = new SOBL();
                FinYearViewModel currentFinYear = (FinYearViewModel)Session[SessionKey.CurrentFinYear];
                int companyBranchId = Convert.ToInt32(Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0);
                ViewData["currentFinyearId"] = currentFinYear.FinYearId;
                CustomerCountViewModel customerCount = customer.GetNewCustomerCount(ContextUser.CompanyId);
                ViewData["New_Customer_Count"] = customerCount.NewCustomer;
                customerCount = customer.GetTotalCustomerCount(ContextUser.CompanyId);
                ViewData["Total_Customer_Count"] = customerCount.TotalCustomer;
                SOCountViewModel SOcount = SO.GetSOCount(ContextUser.CompanyId, currentFinYear.FinYearId,companyBranchId);
                ViewData["SO_Count"] = SOcount.sOTotalCount;
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }
        [HttpPost]
        public void SetFinancialYear(int finYearId)
        {
            try
            {
                SessionWrapper session = new SessionWrapper();
                FinYearBL finYearBL = new FinYearBL();
                FinYearViewModel currentFinYear = finYearBL.GetCurrentFinancialYear(finYearId);
                session.SetInSession(SessionKey.CurrentFinYear, currentFinYear);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return;
        }
        public ActionResult PurchaseDashboard()
        {
            try
            {
                PurchaseDashboardBL purchaseDashboardBL = new PurchaseDashboardBL();
                FinYearViewModel currentFinYear = (FinYearViewModel)Session[SessionKey.CurrentFinYear];
                ViewData["currentFinyearId"] = currentFinYear.FinYearId;
                int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["currentFinyearId"] = currentFinYear.FinYearId;

                /*----------------------------------------New Code For Purchase Dashboad Start -----------------------------------*/
                List<PurchaseDashboardItemViewModel> purchaseDashboardItems = purchaseDashboardBL.GetPurchaseDashboardItems(ContextUser.RoleId, ContextUser.CompanyId, Convert.ToInt32(ContextUser.CompanyBranchId), finYearId);
                if (purchaseDashboardItems.Count > 0 && purchaseDashboardItems != null)
                {
                    foreach (PurchaseDashboardItemViewModel item in purchaseDashboardItems)
                    {
                        if (item.BoxNumber == "1")
                        {
                            ViewData["Box1ItemKey"] = item.ContainerItemKey;
                            ViewData["Box1ItemValue"] = item.ContainerItemValue;
                        }
                        if (item.BoxNumber == "2")
                        {
                            ViewData["Box2ItemKey"] = item.ContainerItemKey;
                            ViewData["Box2ItemValue"] = item.ContainerItemValue;
                        }
                        if (item.BoxNumber == "3")
                        {
                            ViewData["Box3ItemKey"] = item.ContainerItemKey;
                            ViewData["Box3ItemValue"] = item.ContainerItemValue;
                        }
                        if (item.BoxNumber == "4")
                        {
                            ViewData["Box4ItemKey"] = item.ContainerItemKey;
                            ViewData["Box4ItemValue"] = item.ContainerItemValue;
                        }
                    }
                }
                /*----------------------------------------End Code For Purchase Dashboad Start -----------------------------------*/
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }
        [HttpPost]
        public PartialViewResult GetPurchaseDashboardPOs()
        {

            List<Container9ViewModel> containerList = new List<Container9ViewModel>();
            PurchaseDashboardBL purchaseDashboardBL = new PurchaseDashboardBL();
            try
            {
                int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                containerList = purchaseDashboardBL.GetPurchaseDashboardPOList(ContextUser.RoleId, ContextUser.CompanyId, Convert.ToInt32(ContextUser.CompanyBranchId), finYearId, 5);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(containerList);
        }

        [HttpPost]
        public PartialViewResult GetPurchaseDashboardPIs()
        {

            List<Container9ViewModel> containerList = new List<Container9ViewModel>();
            PurchaseDashboardBL purchaseDashboardBL = new PurchaseDashboardBL();
            try
            {
                int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                containerList = purchaseDashboardBL.GetPurchaseDashboardPOList(ContextUser.RoleId, ContextUser.CompanyId, Convert.ToInt32(ContextUser.CompanyBranchId), finYearId, 6);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(containerList);
        }


        [HttpPost]
        public PartialViewResult GetPurchaseDashboardPendingPOs()
        {

            List<Container9ViewModel> containerList = new List<Container9ViewModel>();
            PurchaseDashboardBL purchaseDashboardBL = new PurchaseDashboardBL();
            try
            {
                int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                containerList = purchaseDashboardBL.GetPurchaseDashboardPOList(ContextUser.RoleId, ContextUser.CompanyId, Convert.ToInt32(ContextUser.CompanyBranchId), finYearId, 7);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(containerList);
        }

        public ActionResult PurchaseTeamDashboard()
        {
            try
            {
                PurchaseDashboardBL purchaseDashboardBL = new PurchaseDashboardBL();
                FinYearViewModel currentFinYear = (FinYearViewModel)Session[SessionKey.CurrentFinYear];
                ViewData["currentFinyearId"] = currentFinYear.FinYearId;
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }
        public ActionResult InventoryDashboard()
        {
            InventoryDashboardBL inventoryDashboardBL = new InventoryDashboardBL();
            CustomerBL customer = new CustomerBL();
            UserBL userBL = new UserBL();
            RoleBL roleBL = new RoleBL();
            SOBL SO = new SOBL();
            ProductionDashboardBL productionDashboardBL = new ProductionDashboardBL();
            CustomerCountViewModel customerCount = new CustomerCountViewModel();
            SOCountViewModel SOcount = new SOCountViewModel();
            POCountViewModel pocount = new POCountViewModel();
            SICountViewModel saledashboard = new SICountViewModel();
            MRNCountViewModel mRNCount = new MRNCountViewModel();

            List<ProductionSummaryReportViewModel> productionSummaryReportList = new List<ProductionSummaryReportViewModel>();
            try
            {

                //For Production

                FinYearViewModel currentFinYear = (FinYearViewModel)Session[SessionKey.CurrentFinYear];

                int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["currentFinyearId"] = currentFinYear.FinYearId;
                ViewData["totalProductCount"] = inventoryDashboardBL.GetTotalProductCount(ContextUser.CompanyId);
                ViewData["todayProductCount"] = inventoryDashboardBL.GetTodayProduct(ContextUser.CompanyId);
                ViewData["todayPendingWorkOrderCount"] = inventoryDashboardBL.GetTodayPendingWorkOrderCount(ContextUser.CompanyId,Convert.ToInt32(ContextUser.CompanyBranchId));
                ViewData["todayMRNCount"] = inventoryDashboardBL.GetTodayMRNCount(ContextUser.CompanyId, Convert.ToInt32(ContextUser.CompanyBranchId));

                ViewData["stockTransferCount"] = inventoryDashboardBL.GetTotalStockTransferCount(ContextUser.CompanyId);
                ViewData["stockReceiveCount"] = inventoryDashboardBL.GetTotalStockReceiveCount(ContextUser.CompanyId);

                ViewData["jobWorkCount"] = inventoryDashboardBL.GetTotalJobWorkCount(ContextUser.CompanyId, Convert.ToInt32(ContextUser.CompanyBranchId));
                ViewData["gateInCount"] = inventoryDashboardBL.GetTotalGateInCount(ContextUser.CompanyId, Convert.ToInt32(ContextUser.CompanyBranchId));
                ViewData["storeRequisitionCount"] = inventoryDashboardBL.GetTotalStoreRequisitionCount(ContextUser.CompanyId, Convert.ToInt32(ContextUser.CompanyBranchId));
                ViewData["stockIssueCount"] = inventoryDashboardBL.GetTotalStockIssueCount(ContextUser.CompanyId, Convert.ToInt32(ContextUser.CompanyBranchId));

                ViewData["PendingJobWorkMRN"] = inventoryDashboardBL.GetTotalPendingJobWorkMRNCount(ContextUser.CompanyId, Convert.ToInt32(ContextUser.CompanyBranchId));
                ViewData["QCPendingForMRN"] = inventoryDashboardBL.GetTotalQCPendingForMRNCount(ContextUser.CompanyId, Convert.ToInt32(ContextUser.CompanyBranchId));
                ViewData["GateInPendingforQC"] = inventoryDashboardBL.GetTotalGateInPendingforQCCount(ContextUser.CompanyId, Convert.ToInt32(ContextUser.CompanyBranchId));
                ViewData["pendingRequistionforSIN"] = inventoryDashboardBL.GetTotalpendingRequistionforSINCount(ContextUser.CompanyId, Convert.ToInt32(ContextUser.CompanyBranchId));
                //ViewData["mostConsumeProductMTD"] = inventoryDashboardBL.GetTotalmostConsumeProductMTDCount(currentFinYear.FinYearId, ContextUser.CompanyId);
                ViewData["totalPendingProduct"] = inventoryDashboardBL.GetTotalPendingProductCount(currentFinYear.FinYearId, ContextUser.CompanyId);
                ViewData["physicalAsOnDate"] = inventoryDashboardBL.GetPhysicalAsOnDate(ContextUser.CompanyId);

                mRNCount = inventoryDashboardBL.GetMRNInventoryDashboard(ContextUser.CompanyId, currentFinYear.FinYearId, ContextUser.UserId, Convert.ToInt32(ContextUser.CompanyBranchId), 1);
                ViewData["todaymrnCount"] = mRNCount.MRNCount_Head;
                mRNCount = inventoryDashboardBL.GetMRNInventoryDashboard(ContextUser.CompanyId, currentFinYear.FinYearId, ContextUser.UserId, Convert.ToInt32(ContextUser.CompanyBranchId), 2);
                ViewData["mtdmrnCount"] = mRNCount.MRNCount_Head;
                mRNCount = inventoryDashboardBL.GetMRNInventoryDashboard(ContextUser.CompanyId, currentFinYear.FinYearId, ContextUser.UserId, Convert.ToInt32(ContextUser.CompanyBranchId), 3);
                ViewData["ytdmrnCount"] = mRNCount.MRNCount_Head;

                mRNCount = inventoryDashboardBL.GetMRNInventoryDashboard(ContextUser.CompanyId, currentFinYear.FinYearId, ContextUser.UserId, Convert.ToInt32(ContextUser.CompanyBranchId), 4);
                ViewData["todaysinCount"] = mRNCount.MRNCount_Head;
                mRNCount = inventoryDashboardBL.GetMRNInventoryDashboard(ContextUser.CompanyId, currentFinYear.FinYearId, ContextUser.UserId, Convert.ToInt32(ContextUser.CompanyBranchId), 5);
                ViewData["mtdsinCount"] = mRNCount.MRNCount_Head;
                mRNCount = inventoryDashboardBL.GetMRNInventoryDashboard(ContextUser.CompanyId, currentFinYear.FinYearId, ContextUser.UserId, Convert.ToInt32(ContextUser.CompanyBranchId), 6);
                ViewData["ytdsinCount"] = mRNCount.MRNCount_Head;

                mRNCount = inventoryDashboardBL.GetMRNInventoryDashboard(ContextUser.CompanyId, currentFinYear.FinYearId, ContextUser.UserId, Convert.ToInt32(ContextUser.CompanyBranchId), 7);
                ViewData["todaystnCount"] = mRNCount.MRNCount_Head;
                mRNCount = inventoryDashboardBL.GetMRNInventoryDashboard(ContextUser.CompanyId, currentFinYear.FinYearId, ContextUser.UserId, Convert.ToInt32(ContextUser.CompanyBranchId), 8);
                ViewData["mtdstnCount"] = mRNCount.MRNCount_Head;
                mRNCount = inventoryDashboardBL.GetMRNInventoryDashboard(ContextUser.CompanyId, currentFinYear.FinYearId, ContextUser.UserId, Convert.ToInt32(ContextUser.CompanyBranchId), 9);
                ViewData["ytdstnCount"] = mRNCount.MRNCount_Head;

                ViewData["mostConsumeProductTODAY"] = inventoryDashboardBL.GetTotalmostConsumeProductCount(currentFinYear.FinYearId, ContextUser.CompanyId, Convert.ToInt32(ContextUser.CompanyBranchId),1);
                ViewData["mostConsumeProductMTD"] = inventoryDashboardBL.GetTotalmostConsumeProductCount(currentFinYear.FinYearId, ContextUser.CompanyId, Convert.ToInt32(ContextUser.CompanyBranchId), 2);
                ViewData["mostConsumeProductYTD"] = inventoryDashboardBL.GetTotalmostConsumeProductCount(currentFinYear.FinYearId, ContextUser.CompanyId, Convert.ToInt32(ContextUser.CompanyBranchId), 3);

                ViewData["rawMaterialTotalPrice"] = inventoryDashboardBL.GetAllProductTotalPrice(1014, finYearId, ContextUser.CompanyId, Convert.ToInt32(ContextUser.CompanyBranchId));
                ViewData["consumableTotalPrice"] = inventoryDashboardBL.GetAllProductTotalPrice(1019, finYearId, ContextUser.CompanyId, Convert.ToInt32(ContextUser.CompanyBranchId));
                ViewData["finishedGoodTotalPrice"] = inventoryDashboardBL.GetAllProductTotalPrice(1020, finYearId, ContextUser.CompanyId, Convert.ToInt32(ContextUser.CompanyBranchId));

                /*----------------------------------------New Code For Sale Dashboad Start -----------------------------------*/
                List<InventoryDashboardItemsViewModel> inventoryDashboardItems = customer.GetInventoryDashboardItems(ContextUser.RoleId, ContextUser.CompanyId, Convert.ToInt32(ContextUser.CompanyBranchId), finYearId);
                if (inventoryDashboardItems.Count > 0 && inventoryDashboardItems != null)
                {
                    foreach (InventoryDashboardItemsViewModel item in inventoryDashboardItems)
                    {
                        if (item.BoxNumber == "1")
                        {
                            ViewData["Box1ItemKey"] = item.ContainerItemKey;
                            ViewData["Box1ItemValue"] = item.ContainerItemValue;
                        }
                        if (item.BoxNumber == "2")
                        {
                            ViewData["Box2ItemKey"] = item.ContainerItemKey;
                            ViewData["Box2ItemValue"] = item.ContainerItemValue;
                        }
                        if (item.BoxNumber == "3")
                        {
                            ViewData["Box3ItemKey"] = item.ContainerItemKey;
                            ViewData["Box3ItemValue"] = item.ContainerItemValue;
                        }
                        if (item.BoxNumber == "4")
                        {
                            ViewData["Box4ItemKey"] = item.ContainerItemKey;
                            ViewData["Box4ItemValue"] = item.ContainerItemValue;
                        }
                        if (item.BoxNumber == "5" && item.ContainerItemKey.Trim() == "MRNs")
                        {
                            ViewData["Box5MRNsKey"] = item.ContainerItemKey;
                        }
                        if (item.BoxNumber == "5" && item.ContainerItemKey.Trim()== "Today MRN")
                        {
                            ViewData["Box5TodayMRNKey"] = item.ContainerItemKey;
                            ViewData["Box5TodayMRNValue"] = item.ContainerItemValue;
                        }
                        if (item.BoxNumber == "5" && item.ContainerItemKey.Trim() == "MTD MRN")
                        {
                            ViewData["Box5MTDMRNKey"] = item.ContainerItemKey;
                            ViewData["Box5MTDMRNValue"] = item.ContainerItemValue;
                        }
                        if (item.BoxNumber == "5" && item.ContainerItemKey.Trim() == "YTD MRN")
                        {
                            ViewData["Box5YTDMRNKey"] = item.ContainerItemKey;
                            ViewData["Box5YTDMRNValue"] = item.ContainerItemValue;
                        }


                        if (item.BoxNumber == "6" && item.ContainerItemKey.Trim() == "SINs")
                        {
                            ViewData["Box6SINsKey"] = item.ContainerItemKey;
                        }
                        if (item.BoxNumber == "6" && item.ContainerItemKey.Trim() == "Today SIN")
                        {
                            ViewData["Box6TodaySINKey"] = item.ContainerItemKey;
                            ViewData["Box6TodaySINValue"] = item.ContainerItemValue;
                        }
                        if (item.BoxNumber == "6" && item.ContainerItemKey.Trim() == "MTD SIN")
                        {
                            ViewData["Box6MTDSINKey"] = item.ContainerItemKey;
                            ViewData["Box6MTDSINValue"] = item.ContainerItemValue;
                        }
                        if (item.BoxNumber == "6" && item.ContainerItemKey.Trim() == "YTD SIN")
                        {
                            ViewData["Box6YTDSINKey"] = item.ContainerItemKey;
                            ViewData["Box6YTDSINValue"] = item.ContainerItemValue;
                        }

                        if (item.BoxNumber == "7" && item.ContainerItemKey.Trim() == "STNs")
                        {
                            ViewData["Box7STNsKey"] = item.ContainerItemKey;
                        }
                        if (item.BoxNumber == "7" && item.ContainerItemKey.Trim() == "Today STN")
                        {
                            ViewData["Box7TodaySTNKey"] = item.ContainerItemKey;
                            ViewData["Box7TodaySTNValue"] = item.ContainerItemValue;
                        }
                        if (item.BoxNumber == "7" && item.ContainerItemKey.Trim() == "MTD STN")
                        {
                            ViewData["Box7MTDSTNKey"] = item.ContainerItemKey;
                            ViewData["Box7MTDSTNValue"] = item.ContainerItemValue;
                        }
                        if (item.BoxNumber == "7" && item.ContainerItemKey.Trim() == "YTD STN")
                        {
                            ViewData["Box7YTDSTNKey"] = item.ContainerItemKey;
                            ViewData["Box7YTDSTNValue"] = item.ContainerItemValue;
                        }

                        if (item.BoxNumber == "8" && item.ContainerItemKey.Trim() == "Total Available Stock Cost (INR)")
                        {
                            ViewData["Box8TotalAvailableStockCost"] = item.ContainerItemKey;
                        }
                        if (item.BoxNumber == "8" && item.ContainerItemKey.Trim() == "Total Raw Material Goods Cost")
                        {
                            ViewData["Box8TotalRawMaterialGoodsCostKey"] = item.ContainerItemKey;
                            ViewData["Box8TotalRawMaterialGoodsCostValue"] = item.ContainerItemValue;
                        }
                        if (item.BoxNumber == "8" && item.ContainerItemKey.Trim() == "Total Consumable Goods Cost")
                        {
                            ViewData["Box8TotalConsumableGoodsCostKey"] = item.ContainerItemKey;
                            ViewData["Box8TotalConsumableGoodsCostValue"] = item.ContainerItemValue;
                        }
                        if (item.BoxNumber == "8" && item.ContainerItemKey.Trim() == "Total Finished Goods Cost")
                        {
                            ViewData["Box8TotalFinishedGoodsCostKey"] = item.ContainerItemKey;
                            ViewData["Box8TotalFinishedGoodsCostValue"] = item.ContainerItemValue;
                        }

                        if (item.BoxNumber == "9" && item.ContainerItemKey.Trim() == "Consume Products")
                        {
                            ViewData["Box9ConsumeProducts"] = item.ContainerItemKey;
                        }
                        if (item.BoxNumber == "9" && item.ContainerItemKey.Trim() == "Today Consumed")
                        {
                            ViewData["Box9TodayConsumedKey"] = item.ContainerItemKey;
                            ViewData["Box9TodayConsumedValue"] = item.ContainerItemValue;
                        }
                        if (item.BoxNumber == "9" && item.ContainerItemKey.Trim() == "MTD Consumed")
                        {
                            ViewData["Box9MTDConsumedKey"] = item.ContainerItemKey;
                            ViewData["Box9MTDConsumedValue"] = item.ContainerItemValue;
                        }
                        if (item.BoxNumber == "9" && item.ContainerItemKey.Trim() == "YTD Consumed")
                        {
                            ViewData["Box9YTDConsumedKey"] = item.ContainerItemKey;
                            ViewData["Box9YTDConsumedValue"] = item.ContainerItemValue;
                        }
                        ////////////////////////////////////////////////////
                        if (item.BoxNumber == "10" && item.ContainerItemKey.Trim() == "Pending MRN")
                        {
                            ViewData["Box10PendingMRN"] = item.ContainerItemKey;
                        }
                        if (item.BoxNumber == "10" && item.ContainerItemKey.Trim() == "Pending JW For JW MRN")
                        {
                            ViewData["Box10PendingJWForJWMRNKey"] = item.ContainerItemKey;
                            ViewData["Box10PendingJWForJWMRNValue"] = item.ContainerItemValue;
                        }
                        if (item.BoxNumber == "10" && item.ContainerItemKey.Trim() == "Job Work MRN")
                        {
                            ViewData["Box10JobWorkMRNKey"] = item.ContainerItemKey;
                            ViewData["Box10JobWorkMRNValue"] = item.ContainerItemValue;
                        }
                        if (item.BoxNumber == "10" && item.ContainerItemKey.Trim() == "Pending QC For MRN")
                        {
                            ViewData["Box10PendingQCForMRNKey"] = item.ContainerItemKey;
                            ViewData["Box10PendingQCForMRNValue"] = item.ContainerItemValue;
                        }
                        ////
                        if (item.BoxNumber == "11" && item.ContainerItemKey.Trim() == "Others")
                        {
                            ViewData["Box11Others"] = item.ContainerItemKey;
                        }
                        if (item.BoxNumber == "11" && item.ContainerItemKey.Trim() == "Total Gate In")
                        {
                            ViewData["Box11TodayConsumedKey"] = item.ContainerItemKey;
                            ViewData["Box11TodayConsumedValue"] = item.ContainerItemValue;
                        }
                        if (item.BoxNumber == "11" && item.ContainerItemKey.Trim() == "Last Physical Stock Taken On")
                        {
                            ViewData["Box11LastPhysicalStockTakenOnKey"] = item.ContainerItemKey;
                            ViewData["Box11LastPhysicalStockTakenOnValue"] = item.ContainerItemValue;
                        }
                        if (item.BoxNumber == "11" && item.ContainerItemKey.Trim() == "Pending Requistion for SIN")
                        {
                            ViewData["Box11PendingRequistionforSINKey"] = item.ContainerItemKey;
                            ViewData["Box11PendingRequistionforSINValue"] = item.ContainerItemValue;
                        }
                    }
                }
                /*----------------------------------------New Code For Sale Dashboad END -----------------------------------*/
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }
        /* Start Code for Parctial View for Inventory Dashboard  Container 9 10 11 */
        [HttpPost]
        public PartialViewResult GetInventoryDashboardMRNs()
        {

            List<Container9ViewModel> container11List = new List<Container9ViewModel>();
            SaleDashboardBL saledashboardBL = new SaleDashboardBL();
            try
            {
                    int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                    container11List = saledashboardBL.GetInventoryDashboardList(ContextUser.RoleId, ContextUser.CompanyId, Convert.ToInt32(ContextUser.CompanyBranchId), finYearId, 5);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(container11List);
        }

        public PartialViewResult GetInventoryDashboardSINs()
        {

            List<Container9ViewModel> container11List = new List<Container9ViewModel>();
            SaleDashboardBL saledashboardBL = new SaleDashboardBL();
            try
            {
                int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                container11List = saledashboardBL.GetInventoryDashboardList(ContextUser.RoleId, ContextUser.CompanyId, Convert.ToInt32(ContextUser.CompanyBranchId), finYearId, 6);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(container11List);
        }

        public PartialViewResult GetInventoryDashboardSTNs()
        {

            List<Container9ViewModel> container11List = new List<Container9ViewModel>();
            SaleDashboardBL saledashboardBL = new SaleDashboardBL();
            try
            {
                int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                container11List = saledashboardBL.GetInventoryDashboardList(ContextUser.RoleId, ContextUser.CompanyId, Convert.ToInt32(ContextUser.CompanyBranchId), finYearId, 7);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(container11List);
        }
        public PartialViewResult GetInventoryDashboardCosts()
        {

            List<Container9ViewModel> container11List = new List<Container9ViewModel>();
            SaleDashboardBL saledashboardBL = new SaleDashboardBL();
            try
            {
                int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                container11List = saledashboardBL.GetInventoryDashboardList(ContextUser.RoleId, ContextUser.CompanyId, Convert.ToInt32(ContextUser.CompanyBranchId), finYearId, 8);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(container11List);
        }
        public PartialViewResult GetInventoryDashboardConsumes()
        {

            List<Container9ViewModel> container11List = new List<Container9ViewModel>();
            SaleDashboardBL saledashboardBL = new SaleDashboardBL();
            try
            {
                int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                container11List = saledashboardBL.GetInventoryDashboardList(ContextUser.RoleId, ContextUser.CompanyId, Convert.ToInt32(ContextUser.CompanyBranchId), finYearId, 9);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(container11List);
        }
        public PartialViewResult GetInventoryDashboardPendingMRNs()
        {

            List<Container9ViewModel> container11List = new List<Container9ViewModel>();
            SaleDashboardBL saledashboardBL = new SaleDashboardBL();
            try
            {
                int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                container11List = saledashboardBL.GetInventoryDashboardList(ContextUser.RoleId, ContextUser.CompanyId, Convert.ToInt32(ContextUser.CompanyBranchId), finYearId, 10);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(container11List);
        }

        public PartialViewResult GetInventoryDashboardPendingOthers()
        {

            List<Container9ViewModel> container11List = new List<Container9ViewModel>();
            SaleDashboardBL saledashboardBL = new SaleDashboardBL();
            try
            {
                int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                container11List = saledashboardBL.GetInventoryDashboardList(ContextUser.RoleId, ContextUser.CompanyId, Convert.ToInt32(ContextUser.CompanyBranchId), finYearId, 11);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(container11List);
        }
        /* End Code for Parctial View for Inventory Dashboard  Container 9 10 11 */

        public ActionResult AccountDashboard()
        {
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;
                FinYearViewModel currentFinYear = (FinYearViewModel)Session[SessionKey.CurrentFinYear];
                ViewData["currentFinyearId"] = currentFinYear.FinYearId;
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }
        public ActionResult HRDashboard()
        {
            try
            {
                FinYearViewModel currentFinYear = (FinYearViewModel)Session[SessionKey.CurrentFinYear];
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["currentFinyearId"] = currentFinYear.FinYearId;
                

                int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                HRDashboardBL hrDashboardBL = new HRDashboardBL();
                List<InventoryDashboardItemsViewModel> inventoryDashboardItems = hrDashboardBL.GetHRDashboardItems(ContextUser.RoleId, ContextUser.CompanyId, Convert.ToInt32(ContextUser.CompanyBranchId), finYearId);
                if (inventoryDashboardItems.Count > 0 && inventoryDashboardItems != null)
                {
                    foreach (InventoryDashboardItemsViewModel item in inventoryDashboardItems)
                    {
                        if (item.BoxNumber == "1")
                        {
                            ViewData["Box1ItemKey"] = item.ContainerItemKey;
                            ViewData["Box1ItemValue"] = item.ContainerItemValue;
                        }
                        if (item.BoxNumber == "2")
                        {
                            ViewData["Box2ItemKey"] = item.ContainerItemKey;
                            ViewData["Box2ItemValue"] = item.ContainerItemValue;
                        }
                        if (item.BoxNumber == "3")
                        {
                            ViewData["Box3ItemKey"] = item.ContainerItemKey;
                            ViewData["Box3ItemValue"] = item.ContainerItemValue;
                        }
                        if (item.BoxNumber == "4")
                        {
                            ViewData["Box4ItemKey"] = item.ContainerItemKey;
                            ViewData["Box4ItemValue"] = item.ContainerItemValue;
                        }
                        if (item.BoxNumber == "6")
                        {
                            ViewData["Box6ItemKey"] = item.ContainerItemKey;
                            ViewData["Box6ItemValue"] = item.ContainerItemValue;
                        }
                        if (item.BoxNumber == "7")
                        {
                            ViewData["Box7ItemKey"] = item.ContainerItemKey;
                            ViewData["Box7ItemValue"] = item.ContainerItemValue;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }
        public ActionResult PayrollDashboard()
        {
            try
            {
              
                FinYearViewModel currentFinYear = (FinYearViewModel)Session[SessionKey.CurrentFinYear];
                ViewData["currentFinyearId"] = currentFinYear.FinYearId;
               
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }

        [HttpPost]
        public PartialViewResult GetBookBalanceList(int companyBranchId=0)
        {
            List<BookBalanceViewModel> bookBalanceList = new List<BookBalanceViewModel>();
            AccountDashboardBL dashboardBL = new AccountDashboardBL();
            try
            {
                string tempBookList = string.Empty;
                string tempBalanceList = string.Empty;
                int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                bookBalanceList = dashboardBL.GetBookBalanceList(ContextUser.CompanyId, finYearId, companyBranchId, out tempBookList, out tempBalanceList);
                ViewBag.BookList = tempBookList.Trim();
                ViewBag.BalanceList = tempBalanceList.Trim();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(bookBalanceList);
        }

        [HttpPost]
        public PartialViewResult GetQuatationCountList(int userId = 0, string selfOrTeam = "SELF")
        {
            List<QuatationCountViewModel> quatationCountList = new List<QuatationCountViewModel>();
            SaleDashboardBL saledashboardBL = new SaleDashboardBL();
            try
            {
                string tempQuatationList = string.Empty;
                string tempCountList = string.Empty;
                int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;

                if (selfOrTeam == "SELF")
                {
                    quatationCountList = saledashboardBL.GetQuatationCountList(ContextUser.CompanyId, ContextUser.UserId, ContextUser.UserId, ContextUser.RoleId, finYearId, out tempQuatationList, out tempCountList);
                }
                else if (selfOrTeam == "TEAM")
                {
                    quatationCountList = saledashboardBL.GetQuatationCountList(ContextUser.CompanyId, userId, ContextUser.UserId, ContextUser.RoleId, finYearId, out tempQuatationList, out tempCountList);
                }

                ViewBag.QuatationList = tempQuatationList.Trim();
                ViewBag.CountList = tempCountList.Trim();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(quatationCountList);
        }

        [HttpPost]
        public PartialViewResult GetSOCountList(int userId = 0, string selfOrTeam = "SELF",int companyBranchId=0)
        {
            List<SOCountViewModel> sOCountList = new List<SOCountViewModel>();
            SaleDashboardBL saledashboardBL = new SaleDashboardBL();
            try
            {
                string tempSOList = string.Empty;
                string tempSOCountList = string.Empty;
                int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                if (selfOrTeam == "SELF")
                {
                    sOCountList = saledashboardBL.GetSOCountList(ContextUser.CompanyId, ContextUser.UserId, ContextUser.UserId, ContextUser.RoleId, finYearId, companyBranchId, out tempSOList, out tempSOCountList);
                }
                else if (selfOrTeam == "TEAM")
                {
                    sOCountList = saledashboardBL.GetSOCountList(ContextUser.CompanyId, userId, ContextUser.UserId, ContextUser.RoleId, finYearId, companyBranchId, out tempSOList, out tempSOCountList);
                }

                ViewBag.SOList = tempSOList.Trim();
                ViewBag.SOCountList = tempSOCountList.Trim();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(sOCountList);
        }

        [HttpPost]
        public PartialViewResult GetSICountList(int userId = 0, string selfOrTeam = "SELF",int companyBranchId=0)
        {
            List<SICountViewModel> sICountList = new List<SICountViewModel>();
            SaleDashboardBL saledashboardBL = new SaleDashboardBL();
            try
            {
                string tempSIList = string.Empty;
                string tempSICountList = string.Empty;
                int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                if (selfOrTeam == "SELF")
                {
                    sICountList = saledashboardBL.GetSICountList(ContextUser.CompanyId, ContextUser.UserId, ContextUser.UserId, ContextUser.RoleId, finYearId, companyBranchId, out tempSIList, out tempSICountList);
                }
                else if (selfOrTeam == "TEAM")
                {
                    sICountList = saledashboardBL.GetSICountList(ContextUser.CompanyId, userId, ContextUser.UserId, ContextUser.RoleId, finYearId, companyBranchId, out tempSIList, out tempSICountList);
                }

                ViewBag.SIList = tempSIList.Trim();
                ViewBag.SICountList = tempSICountList.Trim();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(sICountList);
        }



        [HttpPost]
        public JsonResult GetSISumByUser(int userId = 0, string selfOrTeam = "SELF")
        {
            SOBL sOBL = new SOBL();
            SICountViewModel sICountViewModel = new SICountViewModel();
            try
            {
                FinYearViewModel currentFinYear = (FinYearViewModel)Session[SessionKey.CurrentFinYear];
                if (selfOrTeam == "SELF")
                {
                    sICountViewModel = sOBL.GetSITotalAmountSumByUser(ContextUser.CompanyId, currentFinYear.FinYearId, ContextUser.UserId, ContextUser.UserId, ContextUser.RoleId);

                }
                else if (selfOrTeam == "TEAM")
                {
                    sICountViewModel = sOBL.GetSITotalAmountSumByUser(ContextUser.CompanyId, currentFinYear.FinYearId, userId, ContextUser.UserId, ContextUser.RoleId);
                }


            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(sICountViewModel, JsonRequestBehavior.AllowGet);
        }

        #region PO
        [HttpPost]
        public JsonResult GetPendingPOCountList(int userId = 0,int companyBranchId=0, string selfOrTeam = "SELF")
        {

            PurchaseDashboardBL pOBL = new PurchaseDashboardBL();
            string PoCountPending = "";
            companyBranchId = (companyBranchId == 0) ? Convert.ToInt32(ContextUser.CompanyBranchId) : companyBranchId;
            //POCountViewModel poCountViewModel = new POCountViewModel();
            try
            {
                FinYearViewModel currentFinYear = (FinYearViewModel)Session[SessionKey.CurrentFinYear];
                //int companyId, int userId, int reportingUserId, int reportingRoleId, int finYearId
                PoCountPending = pOBL.GetPendingPOCountList(ContextUser.CompanyId, companyBranchId,ContextUser.UserId, ContextUser.UserId, ContextUser.RoleId, currentFinYear.FinYearId);

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(PoCountPending, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public PartialViewResult GetPOCountList(int userId = 0,int companyBranchId=0, string selfOrTeam = "SELF")
        {
            List<POCountViewModel> pOCountList = new List<POCountViewModel>();
            PurchaseDashboardBL purchaseDashboardBL = new PurchaseDashboardBL();
            companyBranchId = (companyBranchId == 0) ? Convert.ToInt32(ContextUser.CompanyBranchId) : companyBranchId;
            try
            {
                string tempPOList = string.Empty;
                string tempPOCountList = string.Empty;
                int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                if (selfOrTeam == "SELF")
                {
                    pOCountList = purchaseDashboardBL.GetPOCountList(ContextUser.CompanyId, companyBranchId, ContextUser.UserId, ContextUser.UserId, ContextUser.RoleId, finYearId, out tempPOList, out tempPOCountList);
                }
                else if (selfOrTeam == "TEAM")
                {
                    pOCountList = purchaseDashboardBL.GetPOCountList(ContextUser.CompanyId, companyBranchId, userId, ContextUser.UserId, ContextUser.RoleId, finYearId, out tempPOList, out tempPOCountList);
                }
                ViewBag.POList = tempPOList.Trim();
                ViewBag.POCountList = tempPOCountList.Trim();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(pOCountList);
        }


        [HttpPost]
        public PartialViewResult GetPICountList(int userId = 0, int companyBranchId = 0, string selfOrTeam = "SELF")
        {
            List<PICountViewModel> pICountList = new List<PICountViewModel>();
            PurchaseDashboardBL purchaseDashboardBL = new PurchaseDashboardBL();
            companyBranchId = (companyBranchId == 0) ? Convert.ToInt32(ContextUser.CompanyBranchId) : companyBranchId;
            try
            {
                string tempPIList = string.Empty;
                string tempPICountList = string.Empty;
                int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                if (selfOrTeam == "SELF")
                {
                    pICountList = purchaseDashboardBL.GetPICountList(ContextUser.CompanyId, companyBranchId, ContextUser.UserId, ContextUser.UserId, ContextUser.RoleId, finYearId, out tempPIList, out tempPICountList);
                }
                else if (selfOrTeam == "TEAM")
                {
                    pICountList = purchaseDashboardBL.GetPICountList(ContextUser.CompanyId, companyBranchId, userId, ContextUser.UserId, ContextUser.RoleId, finYearId, out tempPIList, out tempPICountList);
                }
                ViewBag.PIList = tempPIList.Trim();
                ViewBag.PICountList = tempPICountList.Trim();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(pICountList);
        }


        [HttpPost]
        public PartialViewResult GetPendingPOList(int companyBranchId = 0)
        {
            List<POCountViewModel> pOCountList = new List<POCountViewModel>();
            PurchaseDashboardBL purchaseDashboardBL = new PurchaseDashboardBL();
            companyBranchId = (companyBranchId == 0) ? Convert.ToInt32(ContextUser.CompanyBranchId) : companyBranchId;
            try
            {               
                int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;               
                pOCountList = purchaseDashboardBL.GetPendingPOList(ContextUser.CompanyId, companyBranchId, finYearId);                              
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(pOCountList);
        }

        [HttpPost]
        public JsonResult GetPendingPQCountList(int companyBranchId = 0)
        {

            PurchaseDashboardBL pOBL = new PurchaseDashboardBL();
            string todayPQCount = "";
            companyBranchId = (companyBranchId == 0) ? Convert.ToInt32(ContextUser.CompanyBranchId) : companyBranchId;
            //POCountViewModel poCountViewModel = new POCountViewModel();
            try
            {
                FinYearViewModel currentFinYear = (FinYearViewModel)Session[SessionKey.CurrentFinYear];
                //int companyId, int userId, int reportingUserId, int reportingRoleId, int finYearId
                todayPQCount = pOBL.GetPendingPQCountList(ContextUser.CompanyId, companyBranchId, currentFinYear.FinYearId);

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(todayPQCount, JsonRequestBehavior.AllowGet);
        }


        #endregion

        [HttpPost]
        public PartialViewResult GetMonthWiseBankCashTransactionSummary(int companyBranchId=0)
        {
            List<MonthWiseCashBankTrnSummaryViewModel> monthWiseTrnList = new List<MonthWiseCashBankTrnSummaryViewModel>();
            AccountDashboardBL dashboardBL = new AccountDashboardBL();
            try
            {
                string tempMonthList = string.Empty;
                string tempBankPaymentList = string.Empty;
                string tempBankReceiptList = string.Empty;
                string tempCashPaymentList = string.Empty;
                string tempCashReceiptList = string.Empty;

                int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                monthWiseTrnList = dashboardBL.GetMonthWiseBankCashTrnList(ContextUser.CompanyId, finYearId, companyBranchId, out tempMonthList, out tempBankPaymentList, out tempBankReceiptList, out tempCashPaymentList, out tempCashReceiptList);
                ViewBag.MonthList = tempMonthList.Trim();
                ViewBag.BankPaymentList = tempBankPaymentList.Trim();
                ViewBag.BankReceiptList = tempBankReceiptList.Trim();
                ViewBag.CashPaymentList = tempCashPaymentList.Trim();
                ViewBag.CashReceiptList = tempCashReceiptList.Trim();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(monthWiseTrnList);
        }

        [HttpPost]
        public PartialViewResult GetLeadStatusCountList(int userId = 0, string selfOrTeam = "SELF")
        {
            List<LeadStatusCountViewModel> leadStatusCountList = new List<LeadStatusCountViewModel>();
            CRMDashboardBL dashboardBL = new CRMDashboardBL();
            try
            {
                if (selfOrTeam == "SELF")
                {
                    leadStatusCountList = dashboardBL.GetLeadStatusCountList(ContextUser.CompanyId, ContextUser.UserId, ContextUser.UserId, ContextUser.RoleId);
                }
                else if (selfOrTeam == "TEAM")
                {
                    leadStatusCountList = dashboardBL.GetLeadStatusCountList(ContextUser.CompanyId, userId, ContextUser.UserId, ContextUser.RoleId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(leadStatusCountList);
        }
        [HttpPost]
        public PartialViewResult GetLeadSourceCountList(int userId = 0, string selfOrTeam = "SELF")
        {
            List<LeadSourceCountViewModel> leadSourceCountList = new List<LeadSourceCountViewModel>();
            CRMDashboardBL dashboardBL = new CRMDashboardBL();
            try
            {
                if (selfOrTeam == "SELF")
                {
                    leadSourceCountList = dashboardBL.GetLeadSourceCountList(ContextUser.CompanyId, ContextUser.UserId, ContextUser.UserId, ContextUser.RoleId);
                }
                else if (selfOrTeam == "TEAM")
                {
                    leadSourceCountList = dashboardBL.GetLeadSourceCountList(ContextUser.CompanyId, userId, ContextUser.UserId, ContextUser.RoleId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(leadSourceCountList);
        }
        [HttpPost]
        public PartialViewResult GetLeadFollowUpCountList(int userId = 0, string selfOrTeam = "SELF")
        {
            List<LeadFollowUpCountViewModel> leadFollowUpCountList = new List<LeadFollowUpCountViewModel>();
            CRMDashboardBL dashboardBL = new CRMDashboardBL();
            try
            {
                if (selfOrTeam == "SELF")
                {
                    leadFollowUpCountList = dashboardBL.GetLeadFollowUpCountList(ContextUser.CompanyId, ContextUser.UserId, ContextUser.UserId, ContextUser.RoleId);
                }
                else if (selfOrTeam == "TEAM")
                {
                    leadFollowUpCountList = dashboardBL.GetLeadFollowUpCountList(ContextUser.CompanyId, userId, ContextUser.UserId, ContextUser.RoleId);
                }



            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(leadFollowUpCountList);
        }
        [HttpPost]
        public PartialViewResult GetDateWiseLeadConversionCountList(int userId = 0, string selfOrTeam = "SELF")
        {
            List<LeadConversionCountViewModel> leadConversionCountList = new List<LeadConversionCountViewModel>();
            CRMDashboardBL dashboardBL = new CRMDashboardBL();
            try
            {
                string dateList = string.Empty;
                string totalLeadCountList = string.Empty;
                string newOpportunityCountList = string.Empty;
                string quotationCountList = string.Empty;
                if (selfOrTeam == "SELF")
                {
                    leadConversionCountList = dashboardBL.GetDateWiseLeadConversionCountList(ContextUser.CompanyId, ContextUser.UserId, ContextUser.UserId, ContextUser.RoleId, out dateList, out totalLeadCountList, out newOpportunityCountList, out quotationCountList);
                }
                else if (selfOrTeam == "TEAM")
                {
                    leadConversionCountList = dashboardBL.GetDateWiseLeadConversionCountList(ContextUser.CompanyId, userId, ContextUser.UserId, ContextUser.RoleId, out dateList, out totalLeadCountList, out newOpportunityCountList, out quotationCountList);
                }

                //                leadConversionCountList = dashboardBL.GetDateWiseLeadConversionCountList(ContextUser.CompanyId, 0, out dateList, out totalLeadCountList, out newOpportunityCountList, out quotationCountList);
                ViewBag.DateList = dateList.Trim();
                ViewBag.TotalLeadCountList = totalLeadCountList.Trim();
                ViewBag.NewOpportunityCountList = newOpportunityCountList.Trim();
                ViewBag.QuotationCountList = quotationCountList.Trim();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(leadConversionCountList);
        }
        [HttpPost]
        public PartialViewResult GetLeadFollowUpReminderDueDateCountList(int userId = 0, string selfOrTeam = "SELF")
        {
            List<LeadFollowUpReminderDueDateCountViewModel> leadFollowUpCountList = new List<LeadFollowUpReminderDueDateCountViewModel>();
            CRMDashboardBL dashboardBL = new CRMDashboardBL();
            try
            {
                if (selfOrTeam == "SELF")
                {
                    leadFollowUpCountList = dashboardBL.GetLeadFollowUpReminderDueDateCountList(ContextUser.CompanyId, ContextUser.UserId, ContextUser.UserId, ContextUser.RoleId);
                }
                else if (selfOrTeam == "TEAM")
                {
                    leadFollowUpCountList = dashboardBL.GetLeadFollowUpReminderDueDateCountList(ContextUser.CompanyId, userId, ContextUser.UserId, ContextUser.RoleId);
                }



            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(leadFollowUpCountList);
        }

        [HttpPost]
        public PartialViewResult GetLeadFollowUpReminderDueDateList(int userId = 0, string selfOrTeam = "SELF", int FollowUpActivityTypeId = 0)
        {
            List<LeadFollowUpReminderDueDateListViewModel> leadFollowUpList = new List<LeadFollowUpReminderDueDateListViewModel>();
            CRMDashboardBL dashboardBL = new CRMDashboardBL();
            try
            {
                if (selfOrTeam == "SELF")
                {
                    leadFollowUpList = dashboardBL.GetLeadFollowUpReminderDueDateList(ContextUser.CompanyId, ContextUser.UserId, ContextUser.UserId, ContextUser.RoleId, FollowUpActivityTypeId);
                }
                else if (selfOrTeam == "TEAM")
                {
                    leadFollowUpList = dashboardBL.GetLeadFollowUpReminderDueDateList(ContextUser.CompanyId, userId, ContextUser.UserId, ContextUser.RoleId, FollowUpActivityTypeId);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(leadFollowUpList);
        }

        [HttpPost]
        public PartialViewResult GetReorderPointProductCountList()
        {
            List<ReorderPointProductCountViewModel> reorderProductCountList = new List<ReorderPointProductCountViewModel>();
            InventoryDashboardBL inventoryDashboardBL = new InventoryDashboardBL();
            try
            {
                int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                reorderProductCountList = inventoryDashboardBL.GetReorderPointProductCountList(ContextUser.CompanyId, finYearId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(reorderProductCountList);
        }


        [HttpPost]
        public PartialViewResult GetInOutProductQuantityCountList(int companyBrachId=0)
        {
            List<ProductQuantityCountViewModel> productQuantityCountList = new List<ProductQuantityCountViewModel>();
            InventoryDashboardBL inventoryDashboardBL = new InventoryDashboardBL();
            try
            {
                companyBrachId = (companyBrachId == 0) ? Convert.ToInt32(ContextUser.CompanyBranchId) : companyBrachId;
                int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                productQuantityCountList = inventoryDashboardBL.GetInOutProductQuantityCountList(ContextUser.CompanyId,companyBrachId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(productQuantityCountList);
        }

        [HttpPost]
        public PartialViewResult GetSINProductQuantityCountList(int companyBranchId=0)
        {
            List<SINProductQuantityCountViewModel> sINproductQuantityCountList = new List<SINProductQuantityCountViewModel>();
            InventoryDashboardBL inventoryDashboardBL = new InventoryDashboardBL();
            try
            {
                companyBranchId = (companyBranchId == 0) ? Convert.ToInt32(ContextUser.CompanyBranchId) : companyBranchId;
                int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                sINproductQuantityCountList = inventoryDashboardBL.GetSINProductQuantityCountList(ContextUser.CompanyId, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(sINproductQuantityCountList);
        }

        public ActionResult StoreDashboard()
        {
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;
                FinYearViewModel currentFinYear = (FinYearViewModel)Session[SessionKey.CurrentFinYear];
                InventoryDashboardBL inventoryDashboardBL = new InventoryDashboardBL();
                ProductionCommanCountViewModel productionCommanCount = new ProductionCommanCountViewModel();
                List<ProductionSummaryReportViewModel> productionSummaryReportList = new List<ProductionSummaryReportViewModel>();
                ProductionDashboardBL productionDashboardBL = new ProductionDashboardBL();
                /*-------------------------------------------------Start Production New Dashboard -14-06-2016----------------------------------*/
                int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                List<ProductionDashboardItemsViewModel> productionDashboardItems = productionDashboardBL.GetProductionDashboardItems(ContextUser.RoleId, ContextUser.CompanyId, Convert.ToInt32(ContextUser.CompanyBranchId), finYearId);
                if (productionDashboardItems.Count > 0 && productionDashboardItems != null)
                {
                    foreach (ProductionDashboardItemsViewModel item in productionDashboardItems)
                    {
                        if (item.BoxNumber == "1")
                        {
                            ViewData["Box1ItemKey"] = item.ContainerItemKey;
                            ViewData["Box1ItemValue"] = item.ContainerItemValue;
                        }
                        if (item.BoxNumber == "2")
                        {
                            ViewData["Box2ItemKey"] = item.ContainerItemKey;
                            ViewData["Box2ItemValue"] = item.ContainerItemValue;
                        }
                        if (item.BoxNumber == "3")
                        {
                            ViewData["Box3ItemKey"] = item.ContainerItemKey;
                            ViewData["Box3ItemValue"] = item.ContainerItemValue;
                        }
                        if (item.BoxNumber == "4")
                        {
                            ViewData["Box4ItemKey"] = item.ContainerItemKey;
                            ViewData["Box4ItemValue"] = item.ContainerItemValue;
                        }
                        if (item.BoxNumber == "7")
                        {
                            ViewData["Box7ItemKey"] = item.ContainerItemKey;
                            ViewData["Box7ItemValue"] = item.ContainerItemValue;
                        }
                        if (item.BoxNumber == "8")
                        {
                            ViewData["Box8ItemKey"] = item.ContainerItemKey;
                            ViewData["Box8ItemValue"] = item.ContainerItemValue;
                        }
                    }
                }
                        /*-------------------------------------------------End Production New Dashboard -14-06-2016----------------------------------*/



                        ViewData["currentFinyearId"] = currentFinYear.FinYearId;
              
                int CompanyBranchId =Convert.ToInt32( Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0);


                
                productionSummaryReportList = productionDashboardBL.GetProdctionSummaryReportList(ContextUser.CompanyId, finYearId, CompanyBranchId);
                foreach (var pair in productionSummaryReportList)
                {
                    string TotalValue = pair.TotalValue.ToString();
                    string Nature = pair.Nature;
                    if (Nature == "Total No of sales order received from Sales Department.")
                    {
                        ViewData["total_no_of_so_rec_sdept"] = TotalValue;
                    }
                    else if (Nature == "Total No of work order generate against sales order.")
                    {
                        ViewData["total_no_of_work_order_gen_so"] = TotalValue;
                    }
                    else if (Nature == "Total No of Work Order generate without sales order.")
                    {
                        ViewData["total_no_of_work_order_gen_with_so"] = TotalValue;
                    }
                    else if (Nature == "No. of incomplete vehicle on line.")
                    {
                        ViewData["total_no_of_incom_vej_online"] = TotalValue;
                    }
                    else if (Nature == "Total No of SO pending For Work order.")
                    {
                        ViewData["total_no_of_Work_Order_Pending_ForSo"] = TotalValue;
                    }
                    else if (Nature == "Total No of Work order pending")
                    {
                        ViewData["total_no_of_Work_Order_Pending"] = TotalValue;
                    }
                    else  //For Total No of vehicle in finished goods.
                    {
                        ViewData["total_no_of_veh_in_fin_good"] = TotalValue;
                    }

                }

                productionCommanCount = inventoryDashboardBL.GetTodayPendingWorkOrderCount(ContextUser.CompanyId, CompanyBranchId);
                ViewData["todayPendingWorkOrderCount"] = productionCommanCount.TodayCount;
                ViewData["totalPendingWorkOrderCount"] = productionCommanCount.TotalCount;

                productionCommanCount = inventoryDashboardBL.GetDashboardProductBOMCount(ContextUser.CompanyId, CompanyBranchId);
                ViewData["todayProductBOMCount"] = productionCommanCount.TodayCount;
                ViewData["totalProductBOMCount"] = productionCommanCount.TotalCount;

                productionCommanCount = inventoryDashboardBL.GetDashboardFinishedGoodCount(ContextUser.CompanyId, CompanyBranchId);
                ViewData["todayFinishedGoodCount"] = productionCommanCount.TodayCount;
                ViewData["totalFinishedGoodCount"] = productionCommanCount.TotalCount;

                productionCommanCount = inventoryDashboardBL.GetDashboardFabricationCount(ContextUser.CompanyId, CompanyBranchId);
                ViewData["todayFabricationCount"] = productionCommanCount.TodayCount;
                ViewData["totalFabricationCount"] = productionCommanCount.TotalCount;

                ViewData["totalProductCount"] = inventoryDashboardBL.GetTotalProductCount(ContextUser.CompanyId);
                //ViewData["todayPendingWorkOrderCount"] = inventoryDashboardBL.GetTodayPendingWorkOrderCount(ContextUser.CompanyId, CompanyBranchId);
                ViewData["todayMRNCount"] = inventoryDashboardBL.GetTodayMRNCount(ContextUser.CompanyId, CompanyBranchId);
                //need to Discuss
                ViewData["stockTransferCount"] = inventoryDashboardBL.GetTotalStockTransferCount(ContextUser.CompanyId);
                ViewData["stockReceiveCount"] = inventoryDashboardBL.GetTotalStockReceiveCount(ContextUser.CompanyId,Convert.ToInt32(ContextUser.CompanyBranchId));

                ViewData["jobWorkCount"] = inventoryDashboardBL.GetTotalJobWorkCount(ContextUser.CompanyId, CompanyBranchId);
                ViewData["gateInCount"] = inventoryDashboardBL.GetTotalGateInCount(ContextUser.CompanyId, CompanyBranchId);
                ViewData["storeRequisitionCount"] = inventoryDashboardBL.GetTotalStoreRequisitionCount(ContextUser.CompanyId, CompanyBranchId);
                ViewData["stockIssueCount"] = inventoryDashboardBL.GetTotalStockIssueCount(ContextUser.CompanyId, CompanyBranchId);

                ViewData["PendingJobWorkMRN"] = inventoryDashboardBL.GetTotalPendingJobWorkMRNCount(ContextUser.CompanyId, CompanyBranchId);
                ViewData["QCPendingForMRN"] = inventoryDashboardBL.GetTotalQCPendingForMRNCount(ContextUser.CompanyId, CompanyBranchId);
                ViewData["GateInPendingforQC"] = inventoryDashboardBL.GetTotalGateInPendingforQCCount(ContextUser.CompanyId, CompanyBranchId);
                ViewData["pendingRequistionforSIN"] = inventoryDashboardBL.GetTotalpendingRequistionforSINCount(ContextUser.CompanyId,CompanyBranchId);
                ViewData["mostConsumeProductMTD"] = inventoryDashboardBL.GetTotalmostConsumeProductMTDCount(currentFinYear.FinYearId, ContextUser.CompanyId);
                ViewData["totalPendingProduct"] = inventoryDashboardBL.GetTotalPendingProductCount(currentFinYear.FinYearId, ContextUser.CompanyId);
                ViewData["physicalAsOnDate"] = inventoryDashboardBL.GetPhysicalAsOnDate(ContextUser.CompanyId);




            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();

        }
        /* Start Code for Parctial View for Production Dashboard  Container 5 6 7 */
        [HttpPost]
        public PartialViewResult GetProductionList()
        {
            List<Container9ViewModel> container11List = new List<Container9ViewModel>();
            ProductionDashboardBL productionDashboardBL = new ProductionDashboardBL();
            try
            {
                int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                container11List = productionDashboardBL.GetProductionDashboardList(ContextUser.RoleId, ContextUser.CompanyId, Convert.ToInt32(ContextUser.CompanyBranchId), finYearId, 5);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(container11List);
        }
        [HttpPost]
        public PartialViewResult GetProductionWOList()
        {
            List<Container9ViewModel> container11List = new List<Container9ViewModel>();
            ProductionDashboardBL productionDashboardBL = new ProductionDashboardBL();
            try
            {
                int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                container11List = productionDashboardBL.GetProductionDashboardList(ContextUser.RoleId, ContextUser.CompanyId, Convert.ToInt32(ContextUser.CompanyBranchId), finYearId, 6);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(container11List);
        }
        /* Start Code for Parctial View for Production Dashboard  Container 9 10 11 */
        [HttpGet]
        public ActionResult EssDashboard()
        {
            ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            ViewData["essEmployeeId"] = Session[SessionKey.EmployeeId];
            return View();
        }
        [HttpGet]
        public JsonResult GetESSEmployeeDetail()
        {
            EmployeeBL employeeBL = new EmployeeBL();
            EmployeeViewModel employee = new EmployeeViewModel();
            try
            {
                employee = employeeBL.GetESSEmployeeDetail(ContextUser.UserId, ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(employee, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetUpComingHolidayDetail()
        {
            HolidayCalenderBL holidayCalenderBL = new HolidayCalenderBL();
            HolidayCalenderViewModel holiday = new HolidayCalenderViewModel();
            try
            {
                holiday = holidayCalenderBL.GetUpComingHolidayDetail();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(holiday, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetUpComingActivityDetail()
        {
            ActivityCalenderBL activityCalenderBL = new ActivityCalenderBL();
            ActivityCalenderViewModel activity = new ActivityCalenderViewModel();
            try
            {
                activity = activityCalenderBL.GetUpcomingActivityDetail();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(activity, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public PartialViewResult GetESSEmployeeRoasterList(int employeeId = 0)
        {
            List<HR_RoasterViewModel> roasters = new List<HR_RoasterViewModel>();
            RoasterBL roasterBL = new RoasterBL();
            try
            {
                roasters = roasterBL.GetESSEmployeeRoasterList(employeeId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(roasters);
        }

        [HttpGet]
        public PartialViewResult GetDashboardThoughtList()
        {
            List<ThoughtViewModel> thoughts = new List<ThoughtViewModel>();
            ThoughtBL thoughtBL = new ThoughtBL();
            try
            {
                thoughts = thoughtBL.GetDashboardThoughtDetail();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(thoughts);
        }

        [HttpGet]
        public JsonResult GetStickyNotesDetails()
        {
            StickyNotesBL stickyNotesBL = new StickyNotesBL();
            StickyNotesViewModel stickyNotes = new StickyNotesViewModel();
            try
            {
                int UserId = ContextUser.UserId;
                stickyNotes = stickyNotesBL.GetStickyNotesDetail(UserId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(stickyNotes, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Thought, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditStickyNotes(StickyNotesViewModel stickyNotesViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            StickyNotesBL stickyNotesBL = new StickyNotesBL();
            try
            {
                if (stickyNotesViewModel != null)
                {
                    stickyNotesViewModel.UserId = ContextUser.UserId;
                    responseOut = stickyNotesBL.AddEditStickyNotes(stickyNotesViewModel);
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

        [HttpGet]
        public JsonResult GetHolidayandActivityDetails()
        {
            ActivityCalenderBL activityCalenderBL = new ActivityCalenderBL();
            List<HolidayActivityCalenderViewModel> holidayActivityCalenders = new List<HolidayActivityCalenderViewModel>();
            try
            {
                int UserId = ContextUser.UserId;
                holidayActivityCalenders = activityCalenderBL.GetHolidayandActivityDetails();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(holidayActivityCalenders, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetTeamDetailList()
        {
            LeadBL leadBL = new LeadBL();
            List<UserViewModel> users = new List<UserViewModel>();
            try
            {
                users = leadBL.GetTeamDetailList(ContextUser.UserId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(users, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public PartialViewResult GetTodayNewCustomerList()
        {
            List<CustomerViewModel> customers = new List<CustomerViewModel>();
            CustomerBL customerBL = new CustomerBL();
            try
            {
                customers = customerBL.GetTodayNewCustomerList(ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(customers);
        }

        [HttpPost]
        public JsonResult GetTodayPOSumAmount(int userId = 0,int companyBranchId=0, string selfOrTeam = "SELF")
        {
            POCountViewModel pOCountList = new POCountViewModel();
            PurchaseDashboardBL purchaseDashboardBL = new PurchaseDashboardBL();
            try
            {
                companyBranchId = (companyBranchId == 0) ? Convert.ToInt32(ContextUser.CompanyBranchId) : companyBranchId;
                if (selfOrTeam == "SELF")
                {
                    pOCountList = purchaseDashboardBL.GetDashboard_TodayPOSumAmount(ContextUser.CompanyId, companyBranchId, ContextUser.UserId, ContextUser.UserId, ContextUser.RoleId);
                }
                else if (selfOrTeam == "TEAM")
                {
                    pOCountList = purchaseDashboardBL.GetDashboard_TodayPOSumAmount(ContextUser.CompanyId, companyBranchId, userId, ContextUser.UserId, ContextUser.RoleId);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(pOCountList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetTodayPISumAmount(int userId = 0,int companyBranchId=0, string selfOrTeam = "SELF")
        {
            PICountViewModel pOCountList = new PICountViewModel();
            PurchaseDashboardBL purchaseDashboardBL = new PurchaseDashboardBL();
            try
            {
                companyBranchId = (companyBranchId == 0) ? Convert.ToInt32(ContextUser.CompanyBranchId) : companyBranchId;
                if (selfOrTeam == "SELF")
                {
                    pOCountList = purchaseDashboardBL.GetDashboard_TodayPISumAmount(ContextUser.CompanyId, companyBranchId, ContextUser.UserId, ContextUser.UserId, ContextUser.RoleId);
                }
                else if (selfOrTeam == "TEAM")
                {
                    pOCountList = purchaseDashboardBL.GetDashboard_TodayPISumAmount(ContextUser.CompanyId, companyBranchId, userId, ContextUser.UserId, ContextUser.RoleId);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(pOCountList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public PartialViewResult GetESSEmployeeAdvanceApplicationList(int userId = 0, string selfOrTeam = "SELF")
        {
            List<EmployeeAdvanceApplicationViewModel> advanceApplicationList = new List<EmployeeAdvanceApplicationViewModel>();
            AdvanceTypeBL advanceTypeBL = new AdvanceTypeBL();
            try
            {
                if (selfOrTeam == "SELF")
                {
                    advanceApplicationList = advanceTypeBL.GetEmployeeAdvanceApplicationDetails(ContextUser.CompanyId, ContextUser.UserId, ContextUser.UserId, ContextUser.RoleId);
                }
                else if (selfOrTeam == "TEAM")
                {
                    advanceApplicationList = advanceTypeBL.GetEmployeeAdvanceApplicationDetails(ContextUser.CompanyId, userId, ContextUser.UserId, ContextUser.RoleId);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(advanceApplicationList);
        }

        [HttpPost]
        public PartialViewResult GetESSEmployeeLeaveApplicationList(int userId = 0, string selfOrTeam = "SELF")
        {
            List<EmployeeLeaveApplicationViewModel> leaveApplicationList = new List<EmployeeLeaveApplicationViewModel>();
            LeaveTypeBL leaveTypeBL = new LeaveTypeBL();
            try
            {
                if (selfOrTeam == "SELF")
                {
                    leaveApplicationList = leaveTypeBL.GetEmployeeLeaveApplicationDetails(ContextUser.CompanyId, ContextUser.UserId, ContextUser.UserId, ContextUser.RoleId);
                }
                else if (selfOrTeam == "TEAM")
                {
                    leaveApplicationList = leaveTypeBL.GetEmployeeLeaveApplicationDetails(ContextUser.CompanyId, userId, ContextUser.UserId, ContextUser.RoleId);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(leaveApplicationList);
        }

        [HttpPost]
        public PartialViewResult GetESSEmployeeAssetApplicationList(int userId = 0, string selfOrTeam = "SELF")
        {
            List<EmployeeAssetApplicationViewModel> assetApplicationList = new List<EmployeeAssetApplicationViewModel>();
            AssetTypeBL assetTypeBL = new AssetTypeBL();
            try
            {
                if (selfOrTeam == "SELF")
                {
                    assetApplicationList = assetTypeBL.GetEmployeeAssetApplicationDetails(ContextUser.CompanyId, ContextUser.UserId, ContextUser.UserId, ContextUser.RoleId);
                }
                else if (selfOrTeam == "TEAM")
                {
                    assetApplicationList = assetTypeBL.GetEmployeeAssetApplicationDetails(ContextUser.CompanyId, userId, ContextUser.UserId, ContextUser.RoleId);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(assetApplicationList);
        }

        [HttpPost]
        public PartialViewResult GetESSEmployeeClaimApplicationList(int userId = 0, string selfOrTeam = "SELF")
        {
            List<EmployeeClaimApplicationViewModel> claimApplicationList = new List<EmployeeClaimApplicationViewModel>();
            ClaimTypeBL claimTypeBL = new ClaimTypeBL();
            try
            {
                if (selfOrTeam == "SELF")
                {
                    claimApplicationList = claimTypeBL.GetEmployeeClaimApplicationDetails(ContextUser.CompanyId, ContextUser.UserId, ContextUser.UserId, ContextUser.RoleId);
                }
                else if (selfOrTeam == "TEAM")
                {
                    claimApplicationList = claimTypeBL.GetEmployeeClaimApplicationDetails(ContextUser.CompanyId, userId, ContextUser.UserId, ContextUser.RoleId);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(claimApplicationList);
        }

        [HttpPost]
        public PartialViewResult GetESSEmployeeLoanApplicationList(int userId = 0, string selfOrTeam = "SELF")
        {
            List<EmployeeLoanApplicationViewModel> loanApplicationList = new List<EmployeeLoanApplicationViewModel>();
            LoanTypeBL loanTypeBL = new LoanTypeBL();
            try
            {
                if (selfOrTeam == "SELF")
                {
                    loanApplicationList = loanTypeBL.GetEmployeeLoanApplicationDetails(ContextUser.CompanyId, ContextUser.UserId, ContextUser.UserId, ContextUser.RoleId);
                }
                else if (selfOrTeam == "TEAM")
                {
                    loanApplicationList = loanTypeBL.GetEmployeeLoanApplicationDetails(ContextUser.CompanyId, userId, ContextUser.UserId, ContextUser.RoleId);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(loanApplicationList);
        }
        [HttpPost]
        public PartialViewResult GetESSEmployeeTravelApplicationList(int userId = 0, string selfOrTeam = "SELF")
        {
            List<HR_EmployeeTravelApplicationViewModel> travelApplicationList = new List<HR_EmployeeTravelApplicationViewModel>();
            TravelTypeBL travelTypeBL = new TravelTypeBL();
            try
            {
                if (selfOrTeam == "SELF")
                {
                    travelApplicationList = travelTypeBL.GetEmployeeTravelApplicationDetails(ContextUser.CompanyId, ContextUser.UserId, ContextUser.UserId, ContextUser.RoleId);
                }
                else if (selfOrTeam == "TEAM")
                {
                    travelApplicationList = travelTypeBL.GetEmployeeTravelApplicationDetails(ContextUser.CompanyId, userId, ContextUser.UserId, ContextUser.RoleId);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(travelApplicationList);
        }

        [HttpPost]
        public PartialViewResult GetAdminDashboardRolesList(int companyBranchId=0)
        {
            List<RoleViewModel> roleList = new List<RoleViewModel>();
            RoleBL roleBL= new RoleBL();
            try
            {
                roleList = roleBL.GetDashboardRolesDetails(ContextUser.CompanyId, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(roleList);
        }

        [HttpPost]
        public PartialViewResult GetAdminDashboardUsersList(int companyBranchId =0)
        {
            List<UserViewModel> usersList = new List<UserViewModel>();
            UserBL userBL = new UserBL();
            try
            {
                usersList = userBL.GetDashboardUsersDetails(ContextUser.CompanyId, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(usersList);
        }

        public ActionResult GenerateESSSalarySlipReport(long payrollProcessingPeriodId, int companyBranchId, int departmentId, int designationId, string employeeType, string employeeCodes, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            PayrollProcessPeriodBL payrollProcessBL = new PayrollProcessPeriodBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "SalarySlipReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("SalarySummaryReport");
            }
            ReportDataSource rd = new ReportDataSource("SalarySummaryReportDataSet", payrollProcessBL.GetEssSalarySummaryReport(payrollProcessingPeriodId, companyBranchId, departmentId, designationId, employeeType, employeeCodes));
            lr.DataSources.Add(rd);


            string mimeType;
            string encoding;
            string fileNameExtension;



            string deviceInfo =

             "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.50in</MarginTop>" +
            "  <MarginLeft>.1in</MarginLeft>" +
            "  <MarginRight>.1in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
            "</DeviceInfo>";


            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);


            return File(renderedBytes, mimeType);
        }

        [HttpGet]
        public JsonResult GetPayrollProcessPeriodDetail(int monthId, int yearId)
        {
            PayrollProcessPeriodBL payrollProcessBL = new PayrollProcessPeriodBL();
            PayrollProcessPeriodViewModel payrollPeriodViewModel = new PayrollProcessPeriodViewModel();
            try
            {
                payrollPeriodViewModel = payrollProcessBL.GetEssPayrollProcessPeriodDetail(monthId, yearId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(payrollPeriodViewModel, JsonRequestBehavior.AllowGet);
        }


        ///////////////

        public PartialViewResult GetSOPendingCountList()
        {
            List<SOPendingViewModel> sOPendingList = new List<SOPendingViewModel>();
            ProductionDashboardBL productionDashboardBL = new ProductionDashboardBL();
            try
            {
                int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                sOPendingList = productionDashboardBL.GetSOPendingList(ContextUser.CompanyId, finYearId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(sOPendingList);
        }
        [HttpPost]
        public PartialViewResult GetProdctionSummaryReportList()
        {
            List<ProductionSummaryReportViewModel> productionSummaryReportList = new List<ProductionSummaryReportViewModel>();
            ProductionDashboardBL productionDashboardBL = new ProductionDashboardBL();
            int CompanyBranchId = Convert.ToInt32(Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0);
            try
            {
                int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                productionSummaryReportList = productionDashboardBL.GetProdctionSummaryReportList(ContextUser.CompanyId, finYearId, CompanyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(productionSummaryReportList);
        }

        [HttpPost]
        public PartialViewResult GetWOPendingCountList()
        {
            List<WorkOrderViewModel> wOPendingList = new List<WorkOrderViewModel>();
            ProductionDashboardBL productionDashboardBL = new ProductionDashboardBL();
            try
            {

                wOPendingList = productionDashboardBL.GetWOPendingList(ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(wOPendingList);
        }


        [HttpPost]
        public PartialViewResult GetSRPendingCountList()
        {
            List<StoreRequisitionViewModel> srPendingList = new List<StoreRequisitionViewModel>();
            InventoryDashboardBL inventoryDashboardBL = new InventoryDashboardBL();
            try
            {

                srPendingList = inventoryDashboardBL.GetSRPending(ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(srPendingList);
        }

        [HttpPost]
        public PartialViewResult GetMRNPendingCountList()
        {
            List<QualityCheckViewModel> qcPendingList = new List<QualityCheckViewModel>();
            InventoryDashboardBL inventoryDashboardBL = new InventoryDashboardBL();
            try
            {

                qcPendingList = inventoryDashboardBL.GetMRNPending(ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(qcPendingList);
        }


        [HttpPost]
        public PartialViewResult GetJobMRNPendingCountList()
        {
            List<JobOrderViewModel> joPendingList = new List<JobOrderViewModel>();
            InventoryDashboardBL inventoryDashboardBL = new InventoryDashboardBL();
            try
            {
                joPendingList = inventoryDashboardBL.GetJobMRNPending(ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(joPendingList);
        }

        [HttpPost]
        public PartialViewResult GetPendingIndentList()
        {
            List<PurchaseIndentViewModel> inPendingList = new List<PurchaseIndentViewModel>();
            PurchaseDashboardBL purchaseDashboardBL = new PurchaseDashboardBL();
            try
            {
                int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                inPendingList = purchaseDashboardBL.GetPendingIndentList(ContextUser.CompanyId, finYearId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(inPendingList);
        }

        [HttpGet]
        public JsonResult GetCompanyBranchList()
        {
            InventoryDashboardBL inventoryDashboardBL = new InventoryDashboardBL();
            List<CompanyBranchViewModel> companyBranchList = new List<CompanyBranchViewModel>();
            try
            {
                companyBranchList = inventoryDashboardBL.GetCompanyBranchList(ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(companyBranchList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetMRNInventoryDashboardCount(int companyBranchId)
        {
            InventoryDashboardBL inventoryDashboardBL = new InventoryDashboardBL();
            MRNCountViewModel mRNCount = new MRNCountViewModel();
            MRNCountViewModel mRN = new MRNCountViewModel();
            try
            {
           
                FinYearViewModel currentFinYear = (FinYearViewModel)Session[SessionKey.CurrentFinYear];
                int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                mRNCount = inventoryDashboardBL.GetMRNInventoryDashboard(ContextUser.CompanyId, currentFinYear.FinYearId, ContextUser.UserId, companyBranchId, 1);
                string todayCount = mRNCount.MRNCount_Head;
                mRNCount = inventoryDashboardBL.GetMRNInventoryDashboard(ContextUser.CompanyId, currentFinYear.FinYearId, ContextUser.UserId, companyBranchId, 2);
                string mdtCount = mRNCount.MRNCount_Head;
                mRNCount = inventoryDashboardBL.GetMRNInventoryDashboard(ContextUser.CompanyId, currentFinYear.FinYearId, ContextUser.UserId, companyBranchId, 3);
                string ydtCount = mRNCount.MRNCount_Head;
                mRN = new MRNCountViewModel() {
                   MRNTodayCount=todayCount,
                   MRNMtdCount= mdtCount,
                   MRNYtdCount=ydtCount
                };
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(mRN, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSINInventoryDashboardCount(int companyBranchId)
        {
            InventoryDashboardBL inventoryDashboardBL = new InventoryDashboardBL();

            SINCountViewModel sINCount = new SINCountViewModel();
            SINCountViewModel sIN = new SINCountViewModel();
            try
            {

                FinYearViewModel currentFinYear = (FinYearViewModel)Session[SessionKey.CurrentFinYear];
                int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                sINCount = inventoryDashboardBL.GetSINInventoryDashboard(ContextUser.CompanyId, currentFinYear.FinYearId, ContextUser.UserId, companyBranchId, 4);
                string todayCount = sINCount.SINCount_Head;
                sINCount = inventoryDashboardBL.GetSINInventoryDashboard(ContextUser.CompanyId, currentFinYear.FinYearId, ContextUser.UserId, companyBranchId, 5);
                string mdtCount = sINCount.SINCount_Head;
                sINCount = inventoryDashboardBL.GetSINInventoryDashboard(ContextUser.CompanyId, currentFinYear.FinYearId, ContextUser.UserId, companyBranchId, 6);
                string ydtCount = sINCount.SINCount_Head;
                sIN = new SINCountViewModel()
                {
                    SINTodayCount = todayCount,
                    SINMtdCount = mdtCount,
                    SINYtdCount = ydtCount
                };
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(sIN, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSTNInventoryDashboardCount(int companyBranchId)
        {
            InventoryDashboardBL inventoryDashboardBL = new InventoryDashboardBL();

            STNCountViewModel sTNCount = new STNCountViewModel();
            STNCountViewModel sTN = new STNCountViewModel();
            try
            {

                FinYearViewModel currentFinYear = (FinYearViewModel)Session[SessionKey.CurrentFinYear];
                int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                sTNCount = inventoryDashboardBL.GetSTNInventoryDashboard(ContextUser.CompanyId, currentFinYear.FinYearId, ContextUser.UserId, companyBranchId, 7);
                string todayCount = sTNCount.STNCount_Head;
                sTNCount = inventoryDashboardBL.GetSTNInventoryDashboard(ContextUser.CompanyId, currentFinYear.FinYearId, ContextUser.UserId, companyBranchId, 8);
                string mdtCount = sTNCount.STNCount_Head;
                sTNCount = inventoryDashboardBL.GetSTNInventoryDashboard(ContextUser.CompanyId, currentFinYear.FinYearId, ContextUser.UserId, companyBranchId, 9);
                string ydtCount = sTNCount.STNCount_Head;
                sTN = new STNCountViewModel()
                {
                    STNTodayCount = todayCount,
                    STNMtdCount = mdtCount,
                    STNYtdCount = ydtCount
                };
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(sTN, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult GetConsumeProductInventoryDashboardCount(int companyBranchId)
        {
            InventoryDashboardBL inventoryDashboardBL = new InventoryDashboardBL();
            ProductConsumeCountViewModel productConsumeCount = new ProductConsumeCountViewModel(); 
            try
            {
                FinYearViewModel currentFinYear = (FinYearViewModel)Session[SessionKey.CurrentFinYear];
                int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                int todayCount = inventoryDashboardBL.GetTotalmostConsumeProductCount(currentFinYear.FinYearId, ContextUser.CompanyId,companyBranchId, 1);
                int mtdCount = inventoryDashboardBL.GetTotalmostConsumeProductCount(currentFinYear.FinYearId, ContextUser.CompanyId, companyBranchId, 2);
                int ytdCount = inventoryDashboardBL.GetTotalmostConsumeProductCount(currentFinYear.FinYearId, ContextUser.CompanyId, companyBranchId, 3);
                productConsumeCount = new ProductConsumeCountViewModel()
                {
                    ProductConsumeTodayCount=todayCount,
                    ProductConsumeMtdCount = mtdCount,
                    ProductConsumeYtdCount= ytdCount
                };
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(productConsumeCount, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPendingMRNInventoryDashboardCount(int companyBranchId)
        {
            InventoryDashboardBL inventoryDashboardBL = new InventoryDashboardBL();
            PendingMRNCountViewModel pendingMRNCount = new PendingMRNCountViewModel();
            try
            {
                FinYearViewModel currentFinYear = (FinYearViewModel)Session[SessionKey.CurrentFinYear];
                int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                int jobWorkCount = inventoryDashboardBL.GetTotalJobWorkCount(ContextUser.CompanyId, companyBranchId);
                int pendingJobWorkMRN = inventoryDashboardBL.GetTotalPendingJobWorkMRNCount(ContextUser.CompanyId, companyBranchId);
                int qCPendingForMRN = inventoryDashboardBL.GetTotalQCPendingForMRNCount(ContextUser.CompanyId, companyBranchId);
                pendingMRNCount = new PendingMRNCountViewModel()
                {
                    JobWorkCount=jobWorkCount,
                    PendingJobWorkMRN=pendingJobWorkMRN,
                    QCPendingForMRN=qCPendingForMRN
                };
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(pendingMRNCount, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOthersInventoryDashboardCount(int companyBranchId)
        {
            InventoryDashboardBL inventoryDashboardBL = new InventoryDashboardBL();
            InventoryDashboardOthersCountViewModel inventoryDashboardOthersCount = new InventoryDashboardOthersCountViewModel();
            try
            {
                FinYearViewModel currentFinYear = (FinYearViewModel)Session[SessionKey.CurrentFinYear];
                int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                int gateInCount = inventoryDashboardBL.GetTotalGateInCount(ContextUser.CompanyId, companyBranchId);
                int pendingRequistionforSIN = inventoryDashboardBL.GetTotalpendingRequistionforSINCount(ContextUser.CompanyId, companyBranchId);
                int physicalAsOnDate = 0;
                inventoryDashboardOthersCount = new InventoryDashboardOthersCountViewModel()
                {
                    GateInCount = gateInCount,
                    PendingRequistionforSIN = pendingRequistionforSIN,
                    PhysicalAsOnDate = physicalAsOnDate
                };
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(inventoryDashboardOthersCount, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetInventoryDashboardProductPrice(int companyBranchId)
        {
            InventoryDashboardBL inventoryDashboardBL = new InventoryDashboardBL();
            InventoryDashboardProductPriceViewModel inventoryDashboardProductPrice = new InventoryDashboardProductPriceViewModel();
            try
            {
                FinYearViewModel currentFinYear = (FinYearViewModel)Session[SessionKey.CurrentFinYear];
                int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                decimal rawMaterialTotalPrice = inventoryDashboardBL.GetAllProductTotalPrice(1014, finYearId, ContextUser.CompanyId, companyBranchId);
                decimal consumableTotalPrice = inventoryDashboardBL.GetAllProductTotalPrice(1019, finYearId, ContextUser.CompanyId, companyBranchId);
                decimal finishedGoodTotalPrice = inventoryDashboardBL.GetAllProductTotalPrice(1020, finYearId, ContextUser.CompanyId, companyBranchId);
                inventoryDashboardProductPrice = new InventoryDashboardProductPriceViewModel()
                {
                    RawMaterialTotalPrice = rawMaterialTotalPrice,
                    ConsumableTotalPrice = consumableTotalPrice,
                    FinishedGoodTotalPrice = finishedGoodTotalPrice
                };
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(inventoryDashboardProductPrice, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTotalStockReceiveCount(int companyBranchId)
        {
            InventoryDashboardBL inventoryDashboardBL = new InventoryDashboardBL();
            int totalStockReceive = 0;
            try
            {
                FinYearViewModel currentFinYear = (FinYearViewModel)Session[SessionKey.CurrentFinYear];
                int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                totalStockReceive = inventoryDashboardBL.GetTotalStockReceiveCount(ContextUser.CompanyId, (companyBranchId==0)?Convert.ToInt32(ContextUser.CompanyBranchId):companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(totalStockReceive, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetTodayPendingPOApproval(int userId = 0,int companyBranchId=0, string selfOrTeam = "SELF")
        {
            POCountViewModel pOCountList = new POCountViewModel();
            PurchaseDashboardBL purchaseDashboardBL = new PurchaseDashboardBL();
            companyBranchId = (companyBranchId == 0) ? Convert.ToInt32(ContextUser.CompanyBranchId) : companyBranchId;
            try
            {

                if (selfOrTeam == "SELF")
                {
                    pOCountList = purchaseDashboardBL.GetDashboard_TodayPOSumAmount(ContextUser.CompanyId, companyBranchId, ContextUser.UserId, ContextUser.UserId, ContextUser.RoleId);
                }
                else if (selfOrTeam == "TEAM")
                {
                    pOCountList = purchaseDashboardBL.GetDashboard_TodayPOSumAmount(ContextUser.CompanyId, companyBranchId, userId, ContextUser.UserId, ContextUser.RoleId);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(pOCountList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSaleDashboardSaleCount(int companyBranchId)
        {
            InventoryDashboardBL inventoryDashboardBL = new InventoryDashboardBL();
            SaleInvoiceCountViewModel saleInvoiceCount = new SaleInvoiceCountViewModel();
            FinYearViewModel currentFinYear = (FinYearViewModel)Session[SessionKey.CurrentFinYear];
            int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
            SICountViewModel saledashboard = new SICountViewModel();
            companyBranchId = (companyBranchId == 0) ? Convert.ToInt32(ContextUser.CompanyBranchId) : companyBranchId;
            try
            {
                saledashboard = inventoryDashboardBL.GetSaleQutationDashboard(ContextUser.CompanyId, finYearId, ContextUser.UserId, companyBranchId, 4);
                string todaySaleCount = saledashboard.SITotalCount;
                string todaysaleamount = saledashboard.SITotalAmountSum;

                saledashboard = inventoryDashboardBL.GetSaleQutationDashboard(ContextUser.CompanyId, finYearId, ContextUser.UserId, companyBranchId, 5);
                string sIMTDTotalCount = saledashboard.SITotalCount;
                string sIMTDTotalAmountSum = saledashboard.SITotalAmountSum;

                saledashboard = inventoryDashboardBL.GetSaleQutationDashboard(ContextUser.CompanyId, finYearId, ContextUser.UserId, companyBranchId, 6);
                string sIYTDTotalCount = saledashboard.SITotalCount;
                string sITotalAmountSum = saledashboard.SITotalAmountSum;
                saleInvoiceCount = new SaleInvoiceCountViewModel() {
                    TodaySaleCount =todaySaleCount,
                    TodaySaleAmount=todaysaleamount,
                    MTDSaleCount= sIMTDTotalCount,
                    MTASaleAmount= sIMTDTotalAmountSum,
                    YTDSaleCount=sIYTDTotalCount,
                    YTDSaleAmount=sITotalAmountSum
                };

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(saleInvoiceCount, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSaleDashboardQutationCount(int companyBranchId)
        {
            InventoryDashboardBL inventoryDashboardBL = new InventoryDashboardBL();
            SaleQutationCountViewModel saleQutationCount = new SaleQutationCountViewModel();
            FinYearViewModel currentFinYear = (FinYearViewModel)Session[SessionKey.CurrentFinYear];
            int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
            SICountViewModel saledashboard = new SICountViewModel();
            companyBranchId = (companyBranchId == 0) ? Convert.ToInt32(ContextUser.CompanyBranchId) : companyBranchId;
            try
            {
                saledashboard = inventoryDashboardBL.GetSaleQutationDashboard(ContextUser.CompanyId, finYearId, ContextUser.UserId, companyBranchId, 1);
                string todayQutationCount = saledashboard.SITotalCount;
                string todayQutationAmount = saledashboard.SITotalAmountSum;

                saledashboard = inventoryDashboardBL.GetSaleQutationDashboard(ContextUser.CompanyId, finYearId, ContextUser.UserId, companyBranchId, 2);
                string sQutationMTSTotalCount = saledashboard.SITotalCount;
                string sQutationMTDTotalAmountSum = saledashboard.SITotalAmountSum;

                saledashboard = inventoryDashboardBL.GetSaleQutationDashboard(ContextUser.CompanyId, finYearId, ContextUser.UserId, companyBranchId, 3);
                string sQutationYTDTotalCount = saledashboard.SITotalCount;
                string sQutationYTDTotalAmountSum = saledashboard.SITotalAmountSum;
                saleQutationCount = new SaleQutationCountViewModel()
                {
                    TodaySaleQutationCount = todayQutationCount,
                    TodaySaleQutationAmount = todayQutationAmount,
                    MTDSaleQutationCount = sQutationMTSTotalCount,
                    MTASaleQutationAmount = sQutationMTDTotalAmountSum,
                    YTDSaleQutationCount = sQutationYTDTotalCount,
                    YTDSaleQutationAmount = sQutationYTDTotalAmountSum
                };

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(saleQutationCount, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSaleDashboardSOCount(int companyBranchId)
        {
            InventoryDashboardBL inventoryDashboardBL = new InventoryDashboardBL();
            SaleOrderCountViewModel soCount = new SaleOrderCountViewModel();
            FinYearViewModel currentFinYear = (FinYearViewModel)Session[SessionKey.CurrentFinYear];
            int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
            SICountViewModel saledashboard = new SICountViewModel();
            companyBranchId = (companyBranchId == 0) ? Convert.ToInt32(ContextUser.CompanyBranchId) : companyBranchId;
            try
            {
                saledashboard = inventoryDashboardBL.GetSaleQutationDashboard(ContextUser.CompanyId, finYearId, ContextUser.UserId, companyBranchId, 1);
                string todaySOCount = saledashboard.SITotalCount;
                string todaySOAmount = saledashboard.SITotalAmountSum;

                saledashboard = inventoryDashboardBL.GetSaleQutationDashboard(ContextUser.CompanyId, finYearId, ContextUser.UserId, companyBranchId, 2);
                string sOMTSTotalCount = saledashboard.SITotalCount;
                string sOMTDTotalAmountSum = saledashboard.SITotalAmountSum;

                saledashboard = inventoryDashboardBL.GetSaleQutationDashboard(ContextUser.CompanyId, finYearId, ContextUser.UserId, companyBranchId, 3);
                string sOYTDTotalCount = saledashboard.SITotalCount;
                string sOYTDTotalAmountSum = saledashboard.SITotalAmountSum;
                soCount = new SaleOrderCountViewModel()
                {
                    TodaySaleOrderCount = todaySOCount,
                    TodaySaleOrderAmount = todaySOAmount,
                    MTDSaleOrderCount = sOMTSTotalCount,
                    MTDSaleOrderAmount = sOMTDTotalAmountSum,
                    YTDSaleOrderCount = sOYTDTotalCount,
                    YTDSaleOrderAmount = sOYTDTotalAmountSum
                };

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(soCount, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSalePendingPaymentCount(int companyBranchId)
        {
            InventoryDashboardBL inventoryDashboardBL = new InventoryDashboardBL();
            FinYearViewModel currentFinYear = (FinYearViewModel)Session[SessionKey.CurrentFinYear];
            int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
            SalePendingPaymentCountViewModel saledashboard = new SalePendingPaymentCountViewModel();
            SalePendingPaymentCountViewModel salePendingPaymentCount = new SalePendingPaymentCountViewModel();
            
            companyBranchId = (companyBranchId == 0) ? Convert.ToInt32(ContextUser.CompanyBranchId) : companyBranchId;
            try
            {
                salePendingPaymentCount = inventoryDashboardBL.GetSalePendingPaymentCountDashboard(ContextUser.CompanyId, finYearId, ContextUser.UserId, companyBranchId, 1);
                string pendingInvoiceCount = salePendingPaymentCount.salePendingInvoiceCount;
                string salePendingInvoiceAmount = salePendingPaymentCount.salePendingInvoiceAmount;
                /*saledashboard = inventoryDashboardBL.GetSaleQutationDashboard(ContextUser.CompanyId, finYearId, ContextUser.UserId, companyBranchId, 12);
                string totalInvoicePackingCount = saledashboard.SITotalCount;
                saledashboard = inventoryDashboardBL.GetSaleQutationDashboard(ContextUser.CompanyId, finYearId, ContextUser.UserId, companyBranchId, 13);
                string totalSaleReturn = saledashboard.SITotalAmountSum;
                saledashboard = inventoryDashboardBL.GetSaleQutationDashboard(ContextUser.CompanyId, finYearId, ContextUser.UserId, companyBranchId, 14);
                string totalSaleTarget = saledashboard.SITotalCount;
                saledashboard = inventoryDashboardBL.GetSaleQutationDashboard(ContextUser.CompanyId, finYearId, ContextUser.UserId, companyBranchId, 4);
                string todaysaleamount = saledashboard.SITotalAmountSum;*/
                saledashboard = new SalePendingPaymentCountViewModel()
                {
                    salePendingInvoiceCount= pendingInvoiceCount,
                    salePendingInvoiceAmount= salePendingInvoiceAmount
                };

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(saledashboard, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSalePendingTargetCountDashboard(int companyBranchId)
        {
            InventoryDashboardBL inventoryDashboardBL = new InventoryDashboardBL();
            FinYearViewModel currentFinYear = (FinYearViewModel)Session[SessionKey.CurrentFinYear];
            int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
            SICountViewModel saledashboard = new SICountViewModel();
            SaleDashboardTargetAmountViewModel salePendingPayment = new SaleDashboardTargetAmountViewModel();
            companyBranchId = (companyBranchId == 0) ? Convert.ToInt32(ContextUser.CompanyBranchId) : companyBranchId;
            try
            {
                salePendingPayment = inventoryDashboardBL.GetSalePendingTargetCountDashboard(ContextUser.CompanyId,finYearId,companyBranchId);
                string totalPendingPayment = salePendingPayment.TargetAmount;
                string amountPending = salePendingPayment.TotalInvoiceAmount;

                salePendingPayment = new SaleDashboardTargetAmountViewModel()
                {
                    TargetAmount =totalPendingPayment,
                    TotalInvoiceAmount= amountPending
                };

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(salePendingPayment, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProductionCountDashboard(int companyBranchId)
        {
            
            InventoryDashboardBL inventoryDashboardBL = new InventoryDashboardBL();
            FinYearViewModel currentFinYear = (FinYearViewModel)Session[SessionKey.CurrentFinYear];
            int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
            ProductionCommanCountViewModel productionCommanCount = new ProductionCommanCountViewModel();
            ProductionFullCommanCountViewModel productionFullCommanCount = new ProductionFullCommanCountViewModel();
            companyBranchId = (companyBranchId == 0) ? Convert.ToInt32(ContextUser.CompanyBranchId) : companyBranchId;
            try
            {
                productionCommanCount = inventoryDashboardBL.GetTodayPendingWorkOrderCount(ContextUser.CompanyId, companyBranchId);
                string todayPendingWorkOrderCount = productionCommanCount.TodayCount;
                string totalPendingWorkOrderCount = productionCommanCount.TotalCount;
                productionCommanCount = inventoryDashboardBL.GetDashboardProductBOMCount(ContextUser.CompanyId, companyBranchId);
                string todayProductBOMCount = productionCommanCount.TodayCount;
                string totalProductBOMCount = productionCommanCount.TotalCount;
                productionCommanCount = inventoryDashboardBL.GetDashboardFinishedGoodCount(ContextUser.CompanyId, companyBranchId);
                string todayFinishedGoodCount = productionCommanCount.TodayCount;
                string totalFinishedGoodCount = productionCommanCount.TotalCount;
                productionCommanCount = inventoryDashboardBL.GetDashboardFabricationCount(ContextUser.CompanyId, companyBranchId);
                string todayFabricationCount = productionCommanCount.TodayCount;
                string totalFabricationCount = productionCommanCount.TotalCount;
                ProductionDashboardBL productionDashboardBL = new ProductionDashboardBL();
                List<ProductionSummaryReportViewModel> productionSummaryReportList = new List<ProductionSummaryReportViewModel>();
                productionSummaryReportList = productionDashboardBL.GetProdctionSummaryReportList(ContextUser.CompanyId, finYearId, companyBranchId);
                string total_no_of_so_rec_sdept = "";
                string total_no_of_work_order_gen_so = "";
                string total_no_of_work_order_gen_with_so = "";
                string total_no_of_incom_vej_online = "";
                string total_no_of_Work_Order_Pending_ForSo = "";
                string total_no_of_Work_Order_Pending = "";
                string total_no_of_veh_in_fin_good = "";

                foreach (var pair in productionSummaryReportList)
                {
                    string TotalValue = pair.TotalValue.ToString();
                    string Nature = pair.Nature;
                    if (Nature == "Total No of sales order received from Sales Department.")
                    {
                        total_no_of_so_rec_sdept = TotalValue;
                    }
                    else if (Nature == "Total No of work order generate against sales order.")
                    {
                        total_no_of_work_order_gen_so = TotalValue;
                    }
                    else if (Nature == "Total No of Work Order generate without sales order.")
                    {
                        total_no_of_work_order_gen_with_so = TotalValue;
                    }
                    else if (Nature == "No. of incomplete vehicle on line.")
                    {
                        total_no_of_incom_vej_online = TotalValue;
                    }
                    else if (Nature == "Total No of SO pending For Work order.")
                    {
                        total_no_of_Work_Order_Pending_ForSo = TotalValue;
                    }
                    else if (Nature == "Total No of Work order pending")
                    {
                        total_no_of_Work_Order_Pending = TotalValue;
                    }
                    else  //For Total No of vehicle in finished goods.
                    {
                        total_no_of_veh_in_fin_good = TotalValue;
                    }

                    productionFullCommanCount = new ProductionFullCommanCountViewModel()
                    {
                        TodayProductBOMCount = todayProductBOMCount,
                        TotalProductBOMCount = totalProductBOMCount,
                        TodayPendingWorkOrderCount = todayPendingWorkOrderCount,
                        TotalPendingWorkOrderCount = totalPendingWorkOrderCount,
                        TodayFinishedGoodCount = todayFinishedGoodCount,
                        TotalFinishedGoodCount = totalFinishedGoodCount,
                        TodayFabricationCount = todayFabricationCount,
                        TotalFabricationCount = totalFabricationCount,
                        Total_no_of_so_rec_sdept = total_no_of_so_rec_sdept,
                        Total_no_of_work_order_gen_so = total_no_of_work_order_gen_so,
                        Total_no_of_work_order_gen_with_so = total_no_of_work_order_gen_with_so,
                        Total_no_of_incom_vej_online = total_no_of_incom_vej_online,
                        Total_no_of_Work_Order_Pending_ForSo = total_no_of_Work_Order_Pending_ForSo,
                        Total_no_of_Work_Order_Pending = total_no_of_Work_Order_Pending,
                        Total_no_of_veh_in_fin_good = total_no_of_veh_in_fin_good
                    };

                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(productionFullCommanCount, JsonRequestBehavior.AllowGet);
        }
    }
}
