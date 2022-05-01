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
using System.Data;
using System.Text;
using System.Globalization;

namespace Portal.Controllers
{
    public class BankStatementController : BaseController
    {
        #region Bank Statement

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_BankStatement, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditBankStatement(int bankStatementID = 0, int accessMode = 3)
        {
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                int daysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                ViewData["monthStartDate"] = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("dd-MMM-yyyy");
                ViewData["monthEndDate"] = new DateTime(DateTime.Now.Year, DateTime.Now.Month, daysInMonth).ToString("dd-MMM-yyyy");
                if (bankStatementID != 0)
                {
                    ViewData["bankStatementID"] = bankStatementID;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["bankStatementID"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_BankStatement, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditBankStatement(BankStatementViewModel bankStatementViewModel, List<BankStatementDetailViewModel> bankStatementDetails)
        {
            ResponseOut responseOut = new ResponseOut();
            BankStatementBL bankStatementBL = new BankStatementBL();
            
            try
            {
                if (bankStatementViewModel != null)
                {
                    bankStatementViewModel.CreatedBy = ContextUser.UserId;
                    bankStatementViewModel.CompanyId = ContextUser.CompanyId;
                    responseOut = bankStatementBL.AddEditBankStatement(bankStatementViewModel, bankStatementDetails);
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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_BankStatement, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListBankStatement()
        {
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

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
        public PartialViewResult SaveBankStatementDetail()
        {
           
            UploadUtilityBL utilityBL = new UploadUtilityBL();
            List<BankStatementDetailViewModel> bankStatementDetailList = new List<BankStatementDetailViewModel>();
            ResponseOut responseOut = new ResponseOut();
            HttpFileCollectionBase files = Request.Files;
            //int packingListTypeId = Convert.ToInt32(Request.Form["PackingTypeListId"].ToString());
            Random rnd = new Random();
            try
            {
               

                //  Get all files from Request object  
                if (files != null && files.Count > 0 && Request.Files[0] != null && Request.Files[0].ContentLength > 0)
                {
                    HttpPostedFileBase file = files[0];
                    string fname;
                    // Checking for Internet Explorer  
                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = file.FileName;

                    }

                    if (file != null && file.ContentLength > 0)
                    {
                        string extension = System.IO.Path.GetExtension(fname).ToLower();
                        string query = null;
                        string connString = "";
                        string[] validFileTypes = { ".xls", ".xlsx", ".csv" };

                        string path1 = string.Format("{0}/{1}", Server.MapPath("~/Content/BankStatementUpload"), fname);
                        if (!Directory.Exists(path1))
                        {
                            Directory.CreateDirectory(Server.MapPath("~/Content/BankStatementUpload"));
                        }
                        if (validFileTypes.Contains(extension))
                        {
                            DataTable dt = new DataTable();
                            if (System.IO.File.Exists(path1))
                            { System.IO.File.Delete(path1); }
                            file.SaveAs(path1);
                            if (extension == ".csv")
                            {
                                dt = CommonHelper.ConvertCSVtoDataTable(path1);
                                ViewBag.Data = dt;
                            }
                            //Connection String to Excel Workbook  
                            else if (extension.Trim() == ".xls")
                            {
                                connString = "Provider=Microsoft.ACE.OLEDB.8.0;Data Source=" + path1 + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                                dt = CommonHelper.ConvertXSLXtoDataTable(path1, connString);
                                ViewBag.Data = dt;
                            }
                            else if (extension.Trim() == ".xlsx")
                            {
                                connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path1 + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                                dt = CommonHelper.ConvertXSLXtoDataTable(path1, connString);
                                ViewBag.Data = dt;
                            }

                            // Code to Update ID based on Name of field
                            StringBuilder strErrMsg = new StringBuilder(" ");

                            if (dt.Rows.Count > 0)
                            {
                                int rowCounter = 1;
                                BankStatementViewModel bankStatementViewModel;
                                BankStatementDetailViewModel bankStatementDetailViewModel;
                                //dt.Columns.Add("TransactionDate", typeof(string));
                                //dt.Columns.Add("ChequeNumber", typeof(string));
                                //dt.Columns.Add("Withdrawal", typeof(decimal));
                                //dt.Columns.Add("Deposit", typeof(decimal));
                                //dt.Columns.Add("Balance", typeof(decimal));
                                dt.Columns.Add("UploadStatus", typeof(bool));
                                bool rowVerifyStatus = true;
                                foreach (DataRow dr in dt.Rows)
                                {
                                    bankStatementViewModel = new BankStatementViewModel();
                                    bankStatementDetailViewModel = new BankStatementDetailViewModel();

                                    //Start validate data in excel//
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["TransactionDate"]).Trim()))
                                    {
                                        strErrMsg.Append("Transaction Date not proper format. " + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }

                                    //if (string.IsNullOrEmpty(Convert.ToString(dr["ChequeNumber"]).Trim()))
                                    //{
                                    //    strErrMsg.Append("Cheque Number not proper format. " + rowCounter.ToString() + Environment.NewLine);
                                    //    rowVerifyStatus = false;
                                    //}
                                    //if (string.IsNullOrEmpty(Convert.ToString(dr["Withdrawal"]).Trim()))
                                    //{
                                    //    strErrMsg.Append("Withdrawal not proper format. " + rowCounter.ToString() + Environment.NewLine);
                                    //    rowVerifyStatus = false;
                                    //}
                                    //if (string.IsNullOrEmpty(Convert.ToString(dr["Deposit"]).Trim()))
                                    //{
                                    //    strErrMsg.Append("Deposit not proper format. " + rowCounter.ToString() + Environment.NewLine);
                                    //    rowVerifyStatus = false;
                                    //}
                                    //if (string.IsNullOrEmpty(Convert.ToString(dr["Balance"]).Trim()))
                                    //{
                                    //    strErrMsg.Append("Balance not proper format. " + rowCounter.ToString() + Environment.NewLine);
                                    //    rowVerifyStatus = false;
                                    //}
                                    //if (string.IsNullOrEmpty(Convert.ToString(dr["Narration"]).Trim()))
                                    //{
                                    //    strErrMsg.Append("Narration not proper format. " + rowCounter.ToString() + Environment.NewLine);
                                    //    rowVerifyStatus = false;
                                    //}


                                    //End validate data in excel//

                                    
                                    if (rowVerifyStatus == true)
                                    {

                                        DateTime dateTime= DateTime.ParseExact(Convert.ToString(dr["TransactionDate"]).Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                        //DateTime.TryParse(Convert.ToString(dr["TransactionDate"]).Trim(),out dateTime);

                                        bankStatementDetailList.Add(new BankStatementDetailViewModel
                                        {
                                            TransactionDate= string.IsNullOrEmpty(Convert.ToString(dr["TransactionDate"]))?"":Convert.ToString(dateTime).Substring(0,10) ,
                                            ChequeNumber = string.IsNullOrEmpty(Convert.ToString(dr["ChequeNumber"]).Trim())?"": Convert.ToString(dr["ChequeNumber"]).Trim(),
                                            Withdrawal = string.IsNullOrEmpty(Convert.ToString(dr["Withdrawal"]))?0:Convert.ToDecimal(dr["Withdrawal"].ToString().Replace(",","")),
                                            Deposit = string.IsNullOrEmpty(Convert.ToString(dr["Deposit"]))?0:Convert.ToDecimal(dr["Deposit"].ToString().Replace(",", "")),
                                            Balance = string.IsNullOrEmpty(Convert.ToString(dr["Balance"]))?0:Convert.ToDecimal(dr["Balance"].ToString().Replace(",", "").Replace("Cr.","")),
                                            Narration = string.IsNullOrEmpty(Convert.ToString(dr["Narration"]))?"":Convert.ToString(dr["Narration"]).Trim(),
                                            
                                        });
                                        dr["UploadStatus"] = true;

                                    }
                                    else
                                    {
                                        dr["UploadStatus"] = false;


                                    }
                                    rowCounter += 1;

                                }
                                dt.AcceptChanges();
                            }
                            else
                            {
                                strErrMsg.Append("Please Upload Files in .xls, .xlsx or .csv format");
                                // productSerialDetail = null;
                            }

                            ViewBag.Error = strErrMsg.ToString();
                            // End of Code to Update ID based on Name of field

                        }
                        else
                        {
                            ViewBag.Error = "Please Upload Files in .xls, .xlsx or .csv format";

                        }

                    }
                }
                else
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = "";
                }
            }
            catch (Exception ex)
            {
                responseOut.message = "";
                responseOut.status = ActionStatus.Fail;
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView("GetBankStatementDetailList", bankStatementDetailList);
        }

        [HttpPost]
        public PartialViewResult GetBankStatementDetailList(List<BankStatementDetailViewModel> bankStatementDetails, int bankStatementId = 0)
        {
           
            BankStatementBL bankStatementBL = new BankStatementBL();
            try
            {
                if (bankStatementDetails == null)
                {
                    bankStatementDetails = bankStatementBL.GetBankStatementDetailList(bankStatementId);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(bankStatementDetails);
        }

        public JsonResult GetBankStatementDetail(int bankStatementId)
        {
            BankStatementBL bankStatementBL = new BankStatementBL();
            BankStatementViewModel bankStatement = new BankStatementViewModel();
            try
            {
                bankStatement = bankStatementBL.GetBankStatementDetail(bankStatementId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(bankStatement, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult GetBankStatementList(string bankStatementNo = "", int bankBookId = 0, int companyBranchId = 0, string fromDate = "", string toDate = "", string bankStatementStatus = "")
        {
            List<BankStatementViewModel> bankStatements = new List<BankStatementViewModel>();

            BankStatementBL bankStatementBL = new BankStatementBL();
            try
            {
                bankStatements = bankStatementBL.GetBankStatementList(bankStatementNo, bankBookId, companyBranchId, fromDate, toDate, ContextUser.CompanyId, bankStatementStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(bankStatements);
        }

        #endregion
    }
}
