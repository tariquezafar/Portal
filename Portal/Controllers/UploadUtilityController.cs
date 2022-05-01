using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Portal.Core;
using Portal.Core.ViewModel;
using Portal.Common;
using Portal.DAL;
using System.Reflection;
using System.IO;
using System.Data;
using System.Text;

namespace Portal.Controllers
{
    [CheckSessionBeforeControllerExecuteAttribute(Order = 1)]
    public class UploadUtilityController : BaseController
    {
        #region Import Lead
        [ValidateRequest(true, UserInterfaceHelper.ImportLead, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        [HttpGet]
        public ActionResult ImportLead(int accessMode = 3)
        {
            try
            {
                ViewData["RoleId"] = ContextUser.RoleId;
                if (accessMode != 0)
                {
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["accessMode"] = 0;
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }

        [ValidateRequest(true, UserInterfaceHelper.ImportLead, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        [ActionName("ImportLead")]
        [HttpPost]
        public ActionResult ImportLeadData()
        {
            UploadUtilityBL utilityBL = new UploadUtilityBL();
            LeadBL leadBL = new LeadBL();
            try
            {
                if (Request.Files["FileUpload1"].ContentLength > 0)
                {
                    string extension = System.IO.Path.GetExtension(Request.Files["FileUpload1"].FileName).ToLower();
                    string query = null;
                    string connString = "";
                    string[] validFileTypes = { ".xls", ".xlsx", ".csv" };

                    string path1 = string.Format("{0}/{1}", Server.MapPath("~/Content/Uploads"), Request.Files["FileUpload1"].FileName);
                    if (!Directory.Exists(path1))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Content/Uploads"));
                    }
                    if (validFileTypes.Contains(extension))
                    {
                        DataTable dt = new DataTable();
                        if (System.IO.File.Exists(path1))
                        { System.IO.File.Delete(path1); }
                        Request.Files["FileUpload1"].SaveAs(path1);
                        if (extension == ".csv")
                        {
                            dt = CommonHelper.ConvertCSVtoDataTable(path1);
                            ViewBag.Data = dt;
                        }
                        //Connection String to Excel Workbook  
                        else if (extension.Trim() == ".xls")
                        {
                            connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path1 + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
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
                            Int32 leadId = 0;
                            int branchStateId = 0;
                            int branchCountryId = 0;
                            int companyStateId = 0;
                            int companyCountryId = 0;
                            int leadSourceId = 0;
                            int leadStatusId = 0;
                            int followUpActivityTypeId = 0;
                            int priorityId = 0;
                            int rowCounter = 1;
                            LeadViewModel leadViewModel;
                            LeadFollowUpViewModel leadFollowUpViewModel;
                            dt.Columns.Add("UploadStatus", typeof(bool));
                            bool rowVerifyStatus = true;

                            foreach (DataRow dr in dt.Rows)
                            {
                                try
                                {
                                    leadViewModel = new LeadViewModel();
                                    leadFollowUpViewModel = new LeadFollowUpViewModel();

                                    if (string.IsNullOrEmpty(Convert.ToString(dr["LeadCode"]).Trim()))
                                    {
                                        strErrMsg.Append("Please Enter Lead Code Column data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }

                                    int priority = 0;

                                    //code to validate data in excel//
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["CompanyName"]).Trim()))
                                    {
                                        strErrMsg.Append("Please Enter Company Name Column data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["ContactPersonName"]).Trim()))
                                    {
                                        strErrMsg.Append("Please Enter Contact Person Name Column data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["Designation"]).Trim()))
                                    {
                                        strErrMsg.Append("Please Enter Designation Column data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["Email"]).Trim()))
                                    {
                                        dr["Email"] = "";
                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["ContactNo"]).Trim()))
                                    {
                                        strErrMsg.Append("Please Enter Contact No. Column data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["CompanyAddress"]).Trim()))
                                    {
                                        strErrMsg.Append("Please Enter Company Address Column data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["CompanyCity"]).Trim()))
                                    {
                                        strErrMsg.Append("Please Enter Company City Column data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }

                                    if (!string.IsNullOrEmpty(Convert.ToString(dr["CompanyStateName"])))
                                    {
                                        companyStateId = utilityBL.GetIdByStateName(Convert.ToString(dr["CompanyStateName"]));
                                        if (companyStateId == 0)
                                        {
                                            strErrMsg.Append("Invalid Company State Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                            rowVerifyStatus = false;
                                        }
                                    }

                                    else
                                    {
                                        strErrMsg.Append("Please Enter Company State Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["FollowUpRemarks"]).Trim()))
                                    {
                                        dr["FollowUpRemarks"] = "";
                                    }

                                    if (string.IsNullOrEmpty(Convert.ToString(dr["LeadStatusReason"]).Trim()))
                                    {
                                        dr["LeadStatusReason"] = "";
                                    }

                                    //end of code to validate data in excel//

                                    //code to get Id from Name data in excel//
                                    if (!string.IsNullOrEmpty(Convert.ToString(dr["StateName"])))
                                    {
                                        branchStateId = utilityBL.GetIdByStateName(Convert.ToString(dr["StateName"]));
                                        if (branchStateId == 0)
                                        {
                                            strErrMsg.Append("Invalid State Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                            rowVerifyStatus = false;
                                        }
                                    }
                                    else
                                    {
                                        dr["StateName"] = 0;
                                    }

                                    if (!string.IsNullOrEmpty(Convert.ToString(dr["CountryName"])))
                                    {
                                        branchCountryId = utilityBL.GetIdByCountryName(Convert.ToString(dr["CountryName"]));
                                        if (branchCountryId == 0)
                                        {
                                            strErrMsg.Append("Invalid Country Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                            rowVerifyStatus = false;
                                        }
                                    }
                                    else
                                    {
                                        dr["CountryName"] = 0;
                                    }

                                    if (!string.IsNullOrEmpty(Convert.ToString(dr["CompanyCountryName"])))
                                    {
                                        companyCountryId = utilityBL.GetIdByCountryName(Convert.ToString(dr["CompanyCountryName"]));
                                        if (companyCountryId == 0)
                                        {
                                            strErrMsg.Append("Invalid Company Country Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                            rowVerifyStatus = false;
                                        }
                                    }
                                    else
                                    {
                                        strErrMsg.Append("Please Enter Company Country Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }
                                    if (!string.IsNullOrEmpty(Convert.ToString(dr["LeadSource"])))
                                    {
                                        leadSourceId = utilityBL.GetIdByLeadSourceName(Convert.ToString(dr["LeadSource"]));
                                        if (leadSourceId == 0)
                                        {
                                            strErrMsg.Append("Invalid Lead Source data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                            rowVerifyStatus = false;
                                        }
                                    }
                                    else
                                    {
                                        strErrMsg.Append("Please Enter Lead Source data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }

                                    if (!string.IsNullOrEmpty(Convert.ToString(dr["FollowUpActivityType"])))
                                    {
                                        followUpActivityTypeId = utilityBL.GetIdByFollowUpActivityName(Convert.ToString(dr["FollowUpActivityType"]));
                                        if (followUpActivityTypeId == 0)
                                        {
                                            strErrMsg.Append("Invalid Follow Up Activity Type data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                            rowVerifyStatus = false;
                                        }
                                    }
                                    else
                                    {
                                        strErrMsg.Append("Please Enter Follow Up Activity Type data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }

                                    if (!string.IsNullOrEmpty(Convert.ToString(dr["Priority"])))
                                    {
                                        priorityId = utilityBL.GetIdByPriorityName(Convert.ToString(dr["Priority"]));
                                        if (priorityId == 0)
                                        {
                                            strErrMsg.Append("Invalid Priority data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                            rowVerifyStatus = false;
                                        }
                                    }
                                    else
                                    {
                                        strErrMsg.Append("Please Enter Priority data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }

                                    if (!string.IsNullOrEmpty(Convert.ToString(dr["LeadStatus"])))
                                    {
                                        leadStatusId = utilityBL.GetIdByLeadStatusName(Convert.ToString(dr["LeadStatus"]));
                                        if (leadStatusId == 0)
                                        {
                                            strErrMsg.Append("Invalid Lead Status data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                            rowVerifyStatus = false;
                                        }
                                    }
                                    else
                                    {
                                        strErrMsg.Append("Please Enter Lead Status data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }


                                    if (rowVerifyStatus == true)
                                    {

                                        //End of code to get Id from Name data in excel//
                                        leadViewModel.LeadCode = Convert.ToString(dr["LeadCode"]);
                                        leadViewModel.CompanyId = ContextUser.CompanyId;
                                        leadViewModel.CompanyName = Convert.ToString(dr["CompanyName"]);
                                        leadViewModel.ContactPersonName = Convert.ToString(dr["ContactPersonName"]);
                                        leadViewModel.Designation = Convert.ToString(dr["Designation"]);
                                        leadViewModel.Email = Convert.ToString(dr["Email"]);
                                        leadViewModel.AlternateEmail = Convert.ToString(dr["AlternateEmail"]);
                                        leadViewModel.ContactNo = Convert.ToString(dr["ContactNo"]);
                                        leadViewModel.AlternateContactNo = Convert.ToString(dr["AlternateContactNo"]);
                                        leadViewModel.Fax = Convert.ToString(dr["Fax"]);
                                        leadViewModel.BranchAddress = Convert.ToString(dr["BranchAddress"]);
                                        leadViewModel.City = Convert.ToString(dr["City"]);
                                        leadViewModel.StateId = branchStateId;
                                        leadViewModel.CountryId = branchCountryId;
                                        leadViewModel.PinCode = Convert.ToString(dr["PinCode"]);
                                        leadViewModel.CompanyAddress = Convert.ToString(dr["CompanyAddress"]);
                                        leadViewModel.CompanyCity = Convert.ToString(dr["CompanyCity"]);
                                        leadViewModel.CompanyStateId = companyStateId;
                                        leadViewModel.CompanyCountryId = companyCountryId;
                                        leadViewModel.CompanyPinCode = Convert.ToString(dr["CompanyPinCode"]);
                                        leadViewModel.LeadSourceId = leadSourceId;
                                        leadViewModel.OtherLeadSourceDescription = Convert.ToString(dr["OtherLeadSourceDescription"]);
                                        leadViewModel.LeadStatusId = leadStatusId;
                                        leadViewModel.CreatedBy = ContextUser.UserId;
                                        leadViewModel.Lead_Status = true;
                                        leadFollowUpViewModel.FollowUpActivityTypeId = followUpActivityTypeId;
                                        leadFollowUpViewModel.FollowUpRemarks = Convert.ToString(dr["FollowUpRemarks"]);
                                        leadFollowUpViewModel.Priority = priorityId;
                                        leadFollowUpViewModel.LeadStatusId = leadStatusId;
                                        leadFollowUpViewModel.LeadStatusReason = Convert.ToString(dr["LeadStatusReason"]);
                                        leadFollowUpViewModel.FollowUpByUserId = ContextUser.UserId;
                                        leadFollowUpViewModel.CreatedBy = ContextUser.UserId;
                                        ResponseOut responseOut = leadBL.ImportLead(leadViewModel, leadFollowUpViewModel);
                                        if (responseOut.status == ActionStatus.Fail)
                                        {
                                            strErrMsg.Append(responseOut.message + ": Error in Inserting in Row #" + rowCounter.ToString() + Environment.NewLine);
                                            dr["UploadStatus"] = false;
                                        }
                                        else
                                        {
                                            dr["UploadStatus"] = true;
                                        }
                                    }
                                    else
                                    {
                                        dr["UploadStatus"] = false;
                                    }
                                }
                                catch (Exception colError)
                                {
                                    dr["UploadStatus"] = false;
                                    strErrMsg.Append(colError.InnerException == null ? colError.Message : Convert.ToString(colError.InnerException) + Environment.NewLine);

                                }
                                rowCounter += 1;

                            }
                            dt.AcceptChanges();
                        }
                        else
                        {
                            strErrMsg.Append("Import not found");
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
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }

        #endregion

        #region Import GLMainGroup
        [ValidateRequest(true, UserInterfaceHelper.ImportGLMainGroup, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        [HttpGet]
        public ActionResult ImportGLMainGroup()
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
        [ValidateRequest(true, UserInterfaceHelper.ImportGLMainGroup, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        [ActionName("ImportGLMainGroup")]
        [HttpPost]
        public ActionResult ImportGLMainGroupData()
        {
            UploadUtilityBL utilityBL = new UploadUtilityBL();
            GLMainGroupBL gLMainGroupBL = new GLMainGroupBL();
            try
            {
                if (Request.Files["FileUpload1"].ContentLength > 0)
                {
                    string extension = System.IO.Path.GetExtension(Request.Files["FileUpload1"].FileName).ToLower();
                    string query = null;
                    string connString = "";
                    string[] validFileTypes = { ".xls", ".xlsx", ".csv" };

                    string path1 = string.Format("{0}/{1}", Server.MapPath("~/Content/Uploads"), Request.Files["FileUpload1"].FileName);
                    if (!Directory.Exists(path1))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Content/Uploads"));
                    }
                    if (validFileTypes.Contains(extension))
                    {
                        DataTable dt = new DataTable();
                        if (System.IO.File.Exists(path1))
                        { System.IO.File.Delete(path1); }
                        Request.Files["FileUpload1"].SaveAs(path1);
                        if (extension == ".csv")
                        {
                            dt = CommonHelper.ConvertCSVtoDataTable(path1);
                            ViewBag.Data = dt;
                        }
                        //Connection String to Excel Workbook  
                        else if (extension.Trim() == ".xls")
                        {
                            connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path1 + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
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
                            GLMainGroupViewModel gLMainGroupViewModel;
                            dt.Columns.Add("UploadStatus", typeof(bool));
                            bool rowVerifyStatus = true;
                            int sequenceNo = 0;
                            foreach (DataRow dr in dt.Rows)
                            {
                                gLMainGroupViewModel = new GLMainGroupViewModel();

                                //code to validate data in excel//
                                if (string.IsNullOrEmpty(Convert.ToString(dr["GLMainGroupName"])))
                                {
                                    strErrMsg.Append("GLMainGroup Name Column has not proper data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                    rowVerifyStatus = false;
                                }


                                else if (string.IsNullOrEmpty(Convert.ToString(dr["GLType"])))
                                {
                                    strErrMsg.Append("GLType Name Column has not proper data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                    rowVerifyStatus = false;
                                }
                                else if (string.IsNullOrEmpty(Convert.ToString(dr["SequenceNo"])) || int.TryParse(Convert.ToString(dr["SequenceNo"]), out sequenceNo) == false)
                                {
                                    strErrMsg.Append("SequenceNo Column has not proper data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                    rowVerifyStatus = false;
                                }

                                //end of code to validate data in excel//

                                //code to get Id from Name data in excel//


                                if (rowVerifyStatus == true)
                                {
                                    //End of code to get Id from Name data in excel//
                                    gLMainGroupViewModel.GLMainGroupName = Convert.ToString(dr["GLMainGroupName"]);
                                    gLMainGroupViewModel.CompanyId = ContextUser.CompanyId;
                                    gLMainGroupViewModel.GLType = Convert.ToString(dr["GLType"]);
                                    gLMainGroupViewModel.CreatedBy = ContextUser.UserId;
                                    gLMainGroupViewModel.SequenceNo = sequenceNo;
                                    gLMainGroupViewModel.GLMainGroup_Status = true;
                                    ResponseOut responseOut = gLMainGroupBL.ImportGlMainGroup(gLMainGroupViewModel);
                                    if (responseOut.status == ActionStatus.Fail)
                                    {
                                        strErrMsg.Append(responseOut.message + ": Error in Inserting in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        dr["UploadStatus"] = false;
                                    }
                                    else
                                    {
                                        dr["UploadStatus"] = true;
                                    }
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
                            strErrMsg.Append("Import not found");
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
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }

        #endregion

        #region Import GLSubGroup
        [ValidateRequest(true, UserInterfaceHelper.ImportGLSubGroup, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        [HttpGet]
        public ActionResult ImportGLSubGroup()
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
        [ValidateRequest(true, UserInterfaceHelper.ImportGLSubGroup, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        [ActionName("ImportGLSubGroup")]
        [HttpPost]
        public ActionResult ImportGLSubGroupData()
        {
            UploadUtilityBL utilityBL = new UploadUtilityBL();
            GLSubGroupBL gLSubGroupBL = new GLSubGroupBL();
            try
            {
                if (Request.Files["FileUpload1"].ContentLength > 0)
                {
                    string extension = System.IO.Path.GetExtension(Request.Files["FileUpload1"].FileName).ToLower();
                    string query = null;
                    string connString = "";
                    string[] validFileTypes = { ".xls", ".xlsx", ".csv" };

                    string path1 = string.Format("{0}/{1}", Server.MapPath("~/Content/Uploads"), Request.Files["FileUpload1"].FileName);
                    if (!Directory.Exists(path1))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Content/Uploads"));
                    }
                    if (validFileTypes.Contains(extension))
                    {
                        DataTable dt = new DataTable();
                        if (System.IO.File.Exists(path1))
                        { System.IO.File.Delete(path1); }
                        Request.Files["FileUpload1"].SaveAs(path1);
                        if (extension == ".csv")
                        {
                            dt = CommonHelper.ConvertCSVtoDataTable(path1);
                            ViewBag.Data = dt;
                        }
                        //Connection String to Excel Workbook  
                        else if (extension.Trim() == ".xls")
                        {
                            connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path1 + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
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
                            int gLMainGroupID = 0;
                            int scheduleID = 0;
                            GLSubGroupViewModel gLSubGroupViewModel;
                            dt.Columns.Add("UploadStatus", typeof(bool));
                            bool rowVerifyStatus = true;
                            int sequenceNo = 0;
                            foreach (DataRow dr in dt.Rows)
                            {
                                gLSubGroupViewModel = new GLSubGroupViewModel();
                                //code to validate data in excel//
                                if (string.IsNullOrEmpty(Convert.ToString(dr["GLMainGroupName"])))
                                {
                                    strErrMsg.Append("GLMainGroup Name Column has not proper data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                    rowVerifyStatus = false;
                                }
                                else if (string.IsNullOrEmpty(Convert.ToString(dr["SequenceNo"])) || int.TryParse(Convert.ToString(dr["SequenceNo"]), out sequenceNo) == false)
                                {
                                    strErrMsg.Append("SequenceNo Column has not proper data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                    rowVerifyStatus = false;
                                }
                                //end of code to validate data in excel//
                                //code to get Id from Name data in excel//
                                if (!string.IsNullOrEmpty(Convert.ToString(dr["GLMainGroupName"])))
                                {
                                    gLMainGroupID = utilityBL.GetIdByGlMainGroupName(Convert.ToString(dr["GLMainGroupName"]));
                                    if (gLMainGroupID == 0)
                                    {
                                        strErrMsg.Append("Invalid GLMainGroup Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }
                                }
                                if (!string.IsNullOrEmpty(Convert.ToString(dr["ScheduleName"])))
                                {
                                    scheduleID = utilityBL.GetIdByScheduleName(Convert.ToString(dr["ScheduleName"]));
                                    if (scheduleID == 0)
                                    {
                                        strErrMsg.Append("Invalid Schedule Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }
                                }

                                if (rowVerifyStatus == true)
                                {
                                    //End of code to get Id from Name data in excel//
                                    gLSubGroupViewModel.GLSubGroupName = Convert.ToString(dr["GLSubGroupName"]);
                                    gLSubGroupViewModel.CompanyId = ContextUser.CompanyId;
                                    gLSubGroupViewModel.CreatedBy = ContextUser.UserId;
                                    gLSubGroupViewModel.SequenceNo = sequenceNo;
                                    gLSubGroupViewModel.GLMainGroupId = gLMainGroupID;
                                    gLSubGroupViewModel.ScheduleId = scheduleID;
                                    gLSubGroupViewModel.GLSubGroup_Status = true;
                                    ResponseOut responseOut = gLSubGroupBL.ImportGLSubGroup(gLSubGroupViewModel);
                                    if (responseOut.status == ActionStatus.Fail)
                                    {
                                        strErrMsg.Append(responseOut.message + ": Error in Inserting in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        dr["UploadStatus"] = false;
                                    }
                                    else
                                    {
                                        dr["UploadStatus"] = true;
                                    }
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
                            strErrMsg.Append("Import not found");
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
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }

        #endregion

        #region Import SL
        [ValidateRequest(true, UserInterfaceHelper.ImportSL, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        [HttpGet]
        public ActionResult ImportSL()
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
        [ValidateRequest(true, UserInterfaceHelper.ImportSL, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        [ActionName("ImportSL")]
        [HttpPost]
        public ActionResult ImportSLData()
        {
            UploadUtilityBL utilityBL = new UploadUtilityBL();
            //  GLSubGroupBL gLSubGroupBL = new GLSubGroupBL();
            SLBL sLBL = new SLBL();
            try
            {
                if (Request.Files["FileUpload1"].ContentLength > 0)
                {
                    string extension = System.IO.Path.GetExtension(Request.Files["FileUpload1"].FileName).ToLower();
                    string query = null;
                    string connString = "";
                    string[] validFileTypes = { ".xls", ".xlsx", ".csv" };

                    string path1 = string.Format("{0}/{1}", Server.MapPath("~/Content/Uploads"), Request.Files["FileUpload1"].FileName);
                    if (!Directory.Exists(path1))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Content/Uploads"));
                    }
                    if (validFileTypes.Contains(extension))
                    {
                        DataTable dt = new DataTable();
                        if (System.IO.File.Exists(path1))
                        { System.IO.File.Delete(path1); }
                        Request.Files["FileUpload1"].SaveAs(path1);
                        if (extension == ".csv")
                        {
                            dt = CommonHelper.ConvertCSVtoDataTable(path1);
                            ViewBag.Data = dt;
                        }
                        //Connection String to Excel Workbook  
                        else if (extension.Trim() == ".xls")
                        {
                            connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path1 + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
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
                            int costCenterId = 0;
                            int subCostCenterId = 0;
                            int sLTypeId = 0;
                            int postingGLId = 0;
                            // GLSubGroupViewModel gLSubGroupViewModel;
                            SLViewModel sLViewModel;
                            dt.Columns.Add("UploadStatus", typeof(bool));
                            bool rowVerifyStatus = true;

                            foreach (DataRow dr in dt.Rows)
                            {
                                sLViewModel = new SLViewModel();
                                //code to validate data in excel//
                                if (string.IsNullOrEmpty(Convert.ToString(dr["SLCode"])))
                                {
                                    strErrMsg.Append("SLCode Column has not proper data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                    rowVerifyStatus = false;
                                }
                                else if (string.IsNullOrEmpty(Convert.ToString(dr["SLHead"])))
                                {
                                    strErrMsg.Append("SLHead Column has not proper data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                    rowVerifyStatus = false;
                                }
                                else if (string.IsNullOrEmpty(Convert.ToString(dr["RefCode"])))
                                {
                                    strErrMsg.Append("RefCode Column has not proper data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                    rowVerifyStatus = false;
                                }
                                //end of code to validate data in excel//
                                //code to get Id from Name data in excel//
                                if (!string.IsNullOrEmpty(Convert.ToString(dr["SLTypeName"])))
                                {
                                    sLTypeId = utilityBL.GetIdBySLTypeName(Convert.ToString(dr["SLTypeName"]));
                                    if (sLTypeId == 0)
                                    {
                                        strErrMsg.Append("Invalid SLTypeName data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }
                                }
                                if (!string.IsNullOrEmpty(Convert.ToString(dr["CostCenterName"])))
                                {
                                    costCenterId = utilityBL.GetIdByCostCenterName(Convert.ToString(dr["CostCenterName"]));
                                    if (costCenterId == 0)
                                    {
                                        strErrMsg.Append("Invalid Cost Center Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }
                                }

                                if (!string.IsNullOrEmpty(Convert.ToString(dr["SubCostCenterName"])))
                                {
                                    subCostCenterId = utilityBL.GetIdBySubCostCenterName(Convert.ToString(dr["SubCostCenterName"]));
                                    if (subCostCenterId == 0)
                                    {
                                        strErrMsg.Append("Invalid SubCostCenterName data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }
                                }
                                if (!string.IsNullOrEmpty(Convert.ToString(dr["GLHead"])))
                                {
                                    postingGLId = utilityBL.GetIdByGLHead(Convert.ToString(dr["GLHead"]));
                                    if (postingGLId == 0)
                                    {
                                        strErrMsg.Append("Invalid GLHead data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }
                                }

                                if (rowVerifyStatus == true)
                                {
                                    //End of code to get Id from Name data in excel//
                                    sLViewModel.SLCode = Convert.ToString(dr["SLCode"]);
                                    sLViewModel.SLHead = Convert.ToString(dr["SLHead"]);
                                    sLViewModel.RefCode = Convert.ToString(dr["RefCode"]);
                                    sLViewModel.CompanyId = ContextUser.CompanyId;
                                    sLViewModel.CreatedBy = ContextUser.UserId;
                                    sLViewModel.SLTypeId = sLTypeId;
                                    sLViewModel.CostCenterId = costCenterId;
                                    sLViewModel.SubCostCenterId = subCostCenterId;

                                    sLViewModel.PostingGLId = postingGLId;
                                    sLViewModel.SL_Status = true;
                                    ResponseOut responseOut = sLBL.ImportSL(sLViewModel);
                                    if (responseOut.status == ActionStatus.Fail)
                                    {
                                        strErrMsg.Append(responseOut.message + ": Error in Inserting in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        dr["UploadStatus"] = false;
                                    }
                                    else
                                    {
                                        dr["UploadStatus"] = true;
                                    }
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
                            strErrMsg.Append("Import not found");
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
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }

        #endregion

        #region Import Customer
        [ValidateRequest(true, UserInterfaceHelper.ImportCustomer, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        [HttpGet]
        public ActionResult ImportCustomer()
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
        [ValidateRequest(true, UserInterfaceHelper.ImportCustomer, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        [ActionName("ImportCustomer")]
        [HttpPost]
        public ActionResult ImportCustomerData()
        {
            UploadUtilityBL utilityBL = new UploadUtilityBL();
            CustomerBL customerBL = new CustomerBL();
            try
            {
                if (Request.Files["FileUpload1"].ContentLength > 0)
                {
                    string extension = System.IO.Path.GetExtension(Request.Files["FileUpload1"].FileName).ToLower();
                    string connString = "";
                    string[] validFileTypes = { ".xls", ".xlsx", ".csv" };

                    string path1 = string.Format("{0}/{1}", Server.MapPath("~/Content/Uploads"), Request.Files["FileUpload1"].FileName);
                    if (!Directory.Exists(path1))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Content/Uploads"));
                    }
                    if (validFileTypes.Contains(extension))
                    {
                        DataTable dt = new DataTable();
                        if (System.IO.File.Exists(path1))
                        { System.IO.File.Delete(path1); }
                        Request.Files["FileUpload1"].SaveAs(path1);
                        if (extension == ".csv")
                        {
                            dt = CommonHelper.ConvertCSVtoDataTable(path1);
                            ViewBag.Data = dt;
                        }
                        //Connection String to Excel Workbook  
                        else if (extension.Trim() == ".xls")
                        {
                            connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path1 + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
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
                            int stateId = 0;
                            int countryId = 0;
                            int employeeID = 0;
                            int customerTypeId = 0;
                            int creditDays = 0;
                            int creditLimit = 0;
                            int rowCounter = 1;
                            int leadID = 0;
                            CustomerViewModel customerViewModel;
                            dt.Columns.Add("UploadStatus", typeof(bool));
                            bool rowVerifyStatus = true;
                            Random rnd = new Random(50000);
                            foreach (DataRow dr in dt.Rows)
                            {
                                try
                                {
                                    customerViewModel = new CustomerViewModel();
                                    //code to validate data in excel//
                                    bool gST_Exempt;


                                    if (string.IsNullOrEmpty(Convert.ToString(dr["CustomerName"]).Trim()))
                                    {
                                        strErrMsg.Append("Please Enter Customer Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["CustomerCode"]).Trim()))
                                    {
                                        strErrMsg.Append("Please Enter Customer Code data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }

                                    if (string.IsNullOrEmpty(Convert.ToString(dr["ContactPersonName"]).Trim()))
                                    {
                                        strErrMsg.Append("Please Enter Contact Person Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }



                                    if (string.IsNullOrEmpty(Convert.ToString(dr["Designation"]).Trim()))
                                    {
                                        dr["Designation"] = "";
                                    }

                                    if (string.IsNullOrEmpty(Convert.ToString(dr["Email"]).Trim()))
                                    {
                                        dr["Email"] = "";
                                    }

                                    if (string.IsNullOrEmpty(Convert.ToString(dr["MobileNo"]).Trim()))
                                    {
                                        strErrMsg.Append("Please Enter Mobile No. Column data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }


                                    if (string.IsNullOrEmpty(Convert.ToString(dr["ContactNo"]).Trim()))
                                    {
                                        dr["ContactNo"] = "";
                                    }


                                    if (string.IsNullOrEmpty(Convert.ToString(dr["Fax"]).Trim()))
                                    {
                                        dr["Fax"] = "";
                                    }


                                    if (string.IsNullOrEmpty(Convert.ToString(dr["PrimaryAddress"]).Trim()))
                                    {
                                        strErrMsg.Append("Please Enter Primary Address Column data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }


                                    if (string.IsNullOrEmpty(Convert.ToString(dr["City"]).Trim()))
                                    {
                                        strErrMsg.Append("Please Enter City Name Column data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;

                                    }
                                    if (!string.IsNullOrEmpty(Convert.ToString(dr["StateName"])))
                                    {
                                        stateId = utilityBL.GetIdByStateName(Convert.ToString(dr["StateName"]));
                                        if (stateId == 0)
                                        {
                                            strErrMsg.Append("Invalid State Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                            rowVerifyStatus = false;
                                        }
                                    }
                                    else
                                    {
                                        strErrMsg.Append("Please Enter State Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }

                                    if (!string.IsNullOrEmpty(Convert.ToString(dr["CountryName"])))
                                    {
                                        countryId = utilityBL.GetIdByCountryName(Convert.ToString(dr["CountryName"]));
                                        if (countryId == 0)
                                        {
                                            strErrMsg.Append("Invalid Country Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                            rowVerifyStatus = false;
                                        }
                                    }
                                    else
                                    {
                                        strErrMsg.Append("Please Enter Country Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }


                                    if (string.IsNullOrEmpty(Convert.ToString(dr["PinCode"]).Trim()))
                                    {
                                        dr["PinCode"] = "";
                                    }

                                    if (string.IsNullOrEmpty(Convert.ToString(dr["CSTNo"]).Trim()))
                                    {
                                        dr["CSTNo"] = "";
                                    }

                                    if (string.IsNullOrEmpty(Convert.ToString(dr["TINNo"]).Trim()))
                                    {
                                        dr["TINNo"] = "";
                                    }


                                    if (string.IsNullOrEmpty(Convert.ToString(dr["PANNo"]).Trim()))
                                    {
                                        dr["PANNo"] = "";
                                    }

                                    if (string.IsNullOrEmpty(Convert.ToString(dr["GSTNo"]).Trim()))
                                    {
                                        dr["GSTNo"] = "";
                                    }

                                    if (string.IsNullOrEmpty(Convert.ToString(dr["ExciseNo"]).Trim()))
                                    {
                                        dr["ExciseNo"] = "";
                                    }



                                    if (!string.IsNullOrEmpty(Convert.ToString(dr["EmployeeName"])))
                                    {
                                        employeeID = utilityBL.GetIdByEmployeeName(Convert.ToString(dr["EmployeeName"]));
                                        if (employeeID == 0)
                                        {
                                            strErrMsg.Append("Invalid Employee Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                            rowVerifyStatus = false;
                                        }
                                    }
                                    else
                                    {
                                        dr["EmployeeName"] = 0;

                                    }



                                    if (string.IsNullOrEmpty(Convert.ToString(dr["CreditLimit"])))
                                    {
                                        dr["CreditLimit"] = 0;
                                    }
                                    else if (int.TryParse(Convert.ToString(dr["CreditLimit"]), out creditLimit) == false)
                                    {
                                        strErrMsg.Append("Credit Limit Column has not proper data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }

                                    if (string.IsNullOrEmpty(Convert.ToString(dr["CreditDays"])))
                                    {
                                        dr["CreditDays"] = 0;
                                    }
                                    else if (int.TryParse(Convert.ToString(dr["CreditDays"]), out creditDays) == false)
                                    {
                                        strErrMsg.Append("Credit Days Column has not proper data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;

                                    }

                                    //end of code to validate data in excel//

                                    //code to get Id from Name data in excel//


                                    if (!string.IsNullOrEmpty(Convert.ToString(dr["CustomerTypeDesc"])))
                                    {
                                        customerTypeId = utilityBL.GetIdByCustomerTypeDesc(Convert.ToString(dr["CustomerTypeDesc"]));
                                        if (customerTypeId == 0)
                                        {
                                            strErrMsg.Append("Invalid Customer Type Desc data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                            rowVerifyStatus = false;
                                        }
                                    }
                                    else
                                    {
                                        strErrMsg.Append("Please Enter Customer Type Desc data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;

                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["GST_Exempt"])))
                                    {
                                        dr["GST_Exempt"] = false;
                                    }
                                    else if (bool.TryParse(Convert.ToString(dr["GST_Exempt"]), out gST_Exempt) == false)
                                    {
                                        strErrMsg.Append("GST_Exempt Column has not proper data in Rows #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }




                                    if (rowVerifyStatus == true)
                                    {
                                        //End of code to get Id from Name data in excel// 

                                        customerViewModel.LeadID = leadID;
                                        customerViewModel.CustomerCode = Convert.ToString(dr["CustomerCode"]);
                                        customerViewModel.CustomerName = Convert.ToString(dr["CustomerName"]);
                                        customerViewModel.ContactPersonName = Convert.ToString(dr["ContactPersonName"]);
                                        customerViewModel.Designation = Convert.ToString(dr["Designation"]);
                                        customerViewModel.Email = Convert.ToString(dr["Email"]);
                                        customerViewModel.MobileNo = Convert.ToString(dr["MobileNo"]);
                                        customerViewModel.ContactNo = Convert.ToString(dr["ContactNo"]);
                                        customerViewModel.Fax = Convert.ToString(dr["Fax"]);
                                        customerViewModel.PrimaryAddress = Convert.ToString(dr["PrimaryAddress"]);
                                        customerViewModel.City = Convert.ToString(dr["City"]);
                                        customerViewModel.StateId = stateId;
                                        customerViewModel.CountryId = countryId;
                                        customerViewModel.PinCode = Convert.ToString(dr["PinCode"]);
                                        customerViewModel.TINNo = Convert.ToString(dr["TINNo"]);
                                        customerViewModel.CSTNo = Convert.ToString(dr["CSTNo"]);
                                        customerViewModel.PANNo = Convert.ToString(dr["PANNo"]);
                                        customerViewModel.GSTNo = Convert.ToString(dr["GSTNo"]);
                                        customerViewModel.ExciseNo = Convert.ToString(dr["ExciseNo"]);
                                        customerViewModel.EmployeeId = employeeID;
                                        customerViewModel.CustomerTypeId = customerTypeId;
                                        customerViewModel.CreditLimit = creditLimit;
                                        customerViewModel.CreditDays = creditDays;
                                        customerViewModel.CompanyId = ContextUser.CompanyId;
                                        customerViewModel.CreatedBy = ContextUser.UserId;
                                        customerViewModel.GST_Exempt = Convert.ToBoolean(dr["GST_Exempt"]);
                                        customerViewModel.Customer_Status = true;

                                        ResponseOut responseOut = customerBL.ImportCustomer(customerViewModel);
                                        if (responseOut.status == ActionStatus.Fail)
                                        {
                                            strErrMsg.Append(responseOut.message + ": Error in Inserting in Row #" + rowCounter.ToString() + Environment.NewLine);
                                            dr["UploadStatus"] = false;
                                        }
                                        else
                                        {
                                            dr["UploadStatus"] = true;
                                        }
                                    }
                                    else
                                    {
                                        dr["UploadStatus"] = false;
                                    }

                                }
                                catch (Exception colError)
                                {
                                    dr["UploadStatus"] = false;
                                    strErrMsg.Append(colError.InnerException == null ? colError.Message : Convert.ToString(colError.InnerException) + Environment.NewLine);

                                }
                                rowCounter += 1;

                            }
                            dt.AcceptChanges();
                        }
                        else
                        {
                            strErrMsg.Append("Import not found");
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
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }

        #endregion

        #region Import Employee
        [ValidateRequest(true, UserInterfaceHelper.ImportEmployee, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        [HttpGet]
        public ActionResult ImportEmployee()
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
        [ValidateRequest(true, UserInterfaceHelper.ImportEmployee, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        [ActionName("ImportEmployee")]
        [HttpPost]
        public ActionResult ImportEmployeeData()
        {
            UploadUtilityBL utilityBL = new UploadUtilityBL();
            EmployeeBL employeeBL = new EmployeeBL();
            try
            {
                if (Request.Files["FileUpload1"].ContentLength > 0)
                {
                    string extension = System.IO.Path.GetExtension(Request.Files["FileUpload1"].FileName).ToLower();
                    string connString = "";
                    string[] validFileTypes = { ".xls", ".xlsx", ".csv" };

                    string path1 = string.Format("{0}/{1}", Server.MapPath("~/Content/Uploads"), Request.Files["FileUpload1"].FileName);
                    if (!Directory.Exists(path1))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Content/Uploads"));
                    }
                    if (validFileTypes.Contains(extension))
                    {
                        DataTable dt = new DataTable();
                        if (System.IO.File.Exists(path1))
                        { System.IO.File.Delete(path1); }
                        Request.Files["FileUpload1"].SaveAs(path1);
                        if (extension == ".csv")
                        {
                            dt = CommonHelper.ConvertCSVtoDataTable(path1);
                            ViewBag.Data = dt;
                        }
                        //Connection String to Excel Workbook  
                        else if (extension.Trim() == ".xls")
                        {
                            connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path1 + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
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
                            int cstateId = 0;
                            int ccountryId = 0;
                            int pstateId = 0;
                            int pcountryId = 0;
                            int designationId = 0;
                            int customerTypeId = 0;
                            int creditDays = 0;
                            int creditLimit = 0;
                            int rowCounter = 1;
                            int departmentId = 0;
                            long companyBranchId = 0;


                            EmployeeViewModel employeeViewModel;
                            dt.Columns.Add("UploadStatus", typeof(bool));
                            bool rowVerifyStatus = true;
                            Random rnd = new Random(50000);
                            foreach (DataRow dr in dt.Rows)
                            {
                                try
                                {
                                    employeeViewModel = new EmployeeViewModel();
                                    //code to validate data in excel//
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["EmployeeCode"]).Trim()))
                                    {
                                        strErrMsg.Append(" Please Enter Employee Code Column data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["FirstName"]).Trim()))
                                    {
                                        strErrMsg.Append("Please Enter First Name Column data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }

                                    if (string.IsNullOrEmpty(Convert.ToString(dr["LastName"]).Trim()))
                                    {
                                        dr["LastName"] = "";
                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["FatherSpouseName"]).Trim()))
                                    {
                                        strErrMsg.Append("Please Enter Father/Spouse Name Column data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["Gender"]).Trim()))
                                    {
                                        dr["Gender"] = "";
                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["DateOfBirth"])))
                                    {
                                        strErrMsg.Append("Please Enter Date Of Birth Column data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }

                                    if (string.IsNullOrEmpty(Convert.ToString(dr["MaritalStatus"]).Trim()))
                                    {
                                        strErrMsg.Append("Please Enter Marital Status Column data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["BloodGroup"]).Trim()))
                                    {
                                        dr["BloodGroup"] = "";
                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["Email"]).Trim()))
                                    {
                                        strErrMsg.Append("Please Enter Email Column has not proper data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["AlternateEmail"]).Trim()))
                                    {
                                        dr["AlternateEmail"] = "";
                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["ContactNo"]).Trim()))
                                    {
                                        dr["ContactNo"] = "";
                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["AlternateContactno"]).Trim()))
                                    {
                                        dr["AlternateContactno"] = "";
                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["MobileNo"]).Trim()))
                                    {
                                        strErrMsg.Append("Please Enter Mobile No. Column has not proper data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["CAddress"]).Trim()))
                                    {
                                        strErrMsg.Append("Please Enter Current Address Column data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["CCity"]).Trim()))
                                    {
                                        strErrMsg.Append("Please Enter Current City Column data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }

                                    if (!string.IsNullOrEmpty(Convert.ToString(dr["CStateName"])))
                                    {
                                        cstateId = utilityBL.GetIdByStateName(Convert.ToString(dr["CStateName"]));
                                        if (cstateId == 0)
                                        {
                                            strErrMsg.Append("Invalid State Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                            rowVerifyStatus = false;
                                        }
                                    }
                                    else
                                    {
                                        strErrMsg.Append("Please Enter State Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;

                                    }

                                    if (!string.IsNullOrEmpty(Convert.ToString(dr["CCountryName"])))
                                    {
                                        ccountryId = utilityBL.GetIdByCountryName(Convert.ToString(dr["CCountryName"]));
                                        if (ccountryId == 0)
                                        {
                                            strErrMsg.Append("Invalid Country Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                            rowVerifyStatus = false;
                                        }
                                    }
                                    else
                                    {
                                        strErrMsg.Append("Please Enter Country Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;

                                    }


                                    if (string.IsNullOrEmpty(Convert.ToString(dr["CPinCode"]).Trim()))
                                    {
                                        dr["CPinCode"] = "";
                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["PAddress"]).Trim()))
                                    {
                                        strErrMsg.Append("Please Enter PAddress Column data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["PCity"]).Trim()))
                                    {
                                        strErrMsg.Append("Please Enter PCity Column data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }
                                    if (!string.IsNullOrEmpty(Convert.ToString(dr["PStateName"])))
                                    {
                                        pstateId = utilityBL.GetIdByStateName(Convert.ToString(dr["PStateName"]));
                                        if (pstateId == 0)
                                        {
                                            strErrMsg.Append("Invalid PState Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                            rowVerifyStatus = false;
                                        }
                                    }
                                    else
                                    {
                                        strErrMsg.Append("Please Enter PState Name Column data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;

                                    }
                                    if (!string.IsNullOrEmpty(Convert.ToString(dr["PCountryName"])))
                                    {
                                        pcountryId = utilityBL.GetIdByCountryName(Convert.ToString(dr["PCountryName"]));
                                        if (pcountryId == 0)
                                        {
                                            strErrMsg.Append("Invalid PCountry Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                            rowVerifyStatus = false;
                                        }
                                    }
                                    else
                                    {
                                        strErrMsg.Append("Please Enter PCountry Name Column data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;

                                    }


                                    if (string.IsNullOrEmpty(Convert.ToString(dr["PPinCode"]).Trim()))
                                    {
                                        dr["PPinCode"] = "";
                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["DateOfJoin"])))
                                    {
                                        strErrMsg.Append("Please Enter Date Of Join Column data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }


                                    if (string.IsNullOrEmpty(Convert.ToString(dr["DateOfLeave"])))
                                    {
                                        dr["DateOfLeave"] = "";

                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["PANNo"]).Trim()))
                                    {
                                        dr["PANNo"] = "";
                                    }
                                    if (!string.IsNullOrEmpty(Convert.ToString(dr["BranchName"])))
                                    {

                                        companyBranchId = utilityBL.GetIdByCompanyBranchName(Convert.ToString(dr["BranchName"]));
                                        if (companyBranchId == 0)
                                        {
                                            strErrMsg.Append("Invalid Company Branch Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                            rowVerifyStatus = false;
                                        }

                                    }
                                    else
                                    {
                                        strErrMsg.Append("Please Enter Company Branch Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;

                                    }

                                    if (string.IsNullOrEmpty(Convert.ToString(dr["AadharNo"]).Trim()))
                                    {
                                        dr["AadharNo"] = "";
                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["BankDetail"]).Trim()))
                                    {
                                        dr["BankDetail"] = "";
                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["BankAccountNo"]).Trim()))
                                    {
                                        dr["BankAccountNo"] = "";
                                    }

                                    if (string.IsNullOrEmpty(Convert.ToString(dr["PFApplicable"])))
                                    {
                                        dr["PFApplicable"] = false;
                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["PFNo"]).Trim()))
                                    {
                                        dr["PFNo"] = "";

                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["ESIApplicable"])))
                                    {
                                        dr["ESIApplicable"] = false;
                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["ESINo"]).Trim()))
                                    {
                                        dr["ESINo"] = "";

                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["EmployeePicPath"]).Trim()))
                                    {
                                        dr["EmployeePicPath"] = "";
                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["EmployeePicName"]).Trim()))
                                    {
                                        dr["EmployeePicPath"] = "";
                                    }

                                    if (string.IsNullOrEmpty(Convert.ToString(dr["Division"]).Trim()))
                                    {
                                        strErrMsg.Append("Please Enter Division No data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }
                                    if (!string.IsNullOrEmpty(Convert.ToString(dr["DepartmentName"])))
                                    {
                                        departmentId = utilityBL.GetIdByDepartmentName(Convert.ToString(dr["DepartmentName"]));
                                        if (departmentId == 0)
                                        {
                                            strErrMsg.Append("Invalid Department Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                            rowVerifyStatus = false;
                                        }
                                    }
                                    else
                                    {
                                        strErrMsg.Append("Please Enter Department Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;

                                    }
                                    if (!string.IsNullOrEmpty(Convert.ToString(dr["DesignationName"])))
                                    {
                                        designationId = utilityBL.GetIdByDesignationName(Convert.ToString(dr["DesignationName"]));
                                        if (designationId == 0)
                                        {
                                            strErrMsg.Append("Invalid Designation Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                            rowVerifyStatus = false;
                                        }
                                    }
                                    else
                                    {
                                        strErrMsg.Append("Please Enter Designation Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;

                                    }

                                    if (string.IsNullOrEmpty(Convert.ToString(dr["EmploymentType"]).Trim()))
                                    {
                                        strErrMsg.Append("Please Enter Employment Type data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }

                                    if (string.IsNullOrEmpty(Convert.ToString(dr["EmployeeCurrentStatus"]).Trim()))
                                    {
                                        dr["EmployeeCurrentStatus"] = "";
                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["EmployeeStatusPeriod"])))
                                    {
                                        dr["EmployeeStatusPeriod"] = 0;
                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["EmployeeStatusStartDate"]).Trim()))
                                    {
                                        dr["EmployeeStatusStartDate"] = "";
                                    }

                                    //else if (string.IsNullOrEmpty(Convert.ToString(dr["CreditLimit"])) || int.TryParse(Convert.ToString(dr["CreditLimit"]), out creditLimit) == false)
                                    //{
                                    //    strErrMsg.Append("Credit Limit Column has not proper data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                    //    rowVerifyStatus = false;
                                    //}
                                    //else if (string.IsNullOrEmpty(Convert.ToString(dr["CreditDays"])) || int.TryParse(Convert.ToString(dr["CreditDays"]), out creditDays) == false)
                                    //{
                                    //    strErrMsg.Append("Credit Days Column has not proper data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                    //    rowVerifyStatus = false;
                                    //}

                                    //end of code to validate data in excel//

                                    //code to get Id from Name data in excel//






                                    if (rowVerifyStatus == true)
                                    {
                                        //End of code to get Id from Name data in excel//

                                        employeeViewModel.EmployeeCode = Convert.ToString(dr["EmployeeCode"]);
                                        employeeViewModel.FirstName = Convert.ToString(dr["FirstName"]);
                                        employeeViewModel.LastName = Convert.ToString(dr["LastName"]);
                                        employeeViewModel.FatherSpouseName = Convert.ToString(dr["FatherSpouseName"]);
                                        employeeViewModel.Gender = Convert.ToString(dr["Gender"]);
                                        employeeViewModel.DateOfBirth = Convert.ToString(dr["DateOfBirth"]);
                                        employeeViewModel.MaritalStatus = Convert.ToString(dr["MaritalStatus"]);
                                        employeeViewModel.BloodGroup = Convert.ToString(dr["BloodGroup"]);
                                        employeeViewModel.Email = Convert.ToString(dr["Email"]);
                                        employeeViewModel.AlternateEmail = Convert.ToString(dr["AlternateEmail"]);
                                        employeeViewModel.ContactNo = Convert.ToString(dr["ContactNo"]);
                                        employeeViewModel.AlternateContactno = Convert.ToString(dr["AlternateContactno"]);
                                        employeeViewModel.MobileNo = Convert.ToString(dr["MobileNo"]);
                                        employeeViewModel.CAddress = Convert.ToString(dr["CAddress"]);
                                        employeeViewModel.CCity = Convert.ToString(dr["CCity"]);
                                        employeeViewModel.CStateId = cstateId;
                                        employeeViewModel.CCountryId = ccountryId;
                                        employeeViewModel.DepartmentId = departmentId;
                                        employeeViewModel.CompanyBranchId = Convert.ToInt32(companyBranchId);
                                        employeeViewModel.DesignationId = designationId;
                                        employeeViewModel.CPinCode = Convert.ToString(dr["CPinCode"]);
                                        employeeViewModel.Division = Convert.ToString(dr["Division"]);
                                        employeeViewModel.EmploymentType = Convert.ToString(dr["EmploymentType"]);
                                        employeeViewModel.PAddress = Convert.ToString(dr["PAddress"]);
                                        employeeViewModel.PCity = Convert.ToString(dr["PCity"]);
                                        employeeViewModel.PStateId = pstateId;
                                        employeeViewModel.PCountryId = pcountryId;
                                        employeeViewModel.PPinCode = Convert.ToString(dr["PPinCode"]);
                                        employeeViewModel.DateOfJoin = Convert.ToString(dr["DateOfJoin"]);
                                        employeeViewModel.DateOfLeave = Convert.ToString(dr["DateOfLeave"]);
                                        employeeViewModel.PANNo = Convert.ToString(dr["PANNo"]);
                                        employeeViewModel.AadharNo = Convert.ToString(dr["AadharNo"]);
                                        employeeViewModel.BankDetail = Convert.ToString(dr["BankDetail"]);
                                        employeeViewModel.BankAccountNo = Convert.ToString(dr["BankAccountNo"]);
                                        employeeViewModel.PANNo = Convert.ToString(dr["PANNo"]);

                                        employeeViewModel.EmployeeStatusPeriod = Convert.ToInt32(dr["EmployeeStatusPeriod"]);
                                        employeeViewModel.EmployeeCurrentStatus = Convert.ToString(dr["EmployeeCurrentStatus"]);

                                        employeeViewModel.PFApplicable = Convert.ToBoolean(dr["PFApplicable"]);
                                        employeeViewModel.PFNo = Convert.ToString(dr["PFNo"]);
                                        employeeViewModel.ESIApplicable = Convert.ToBoolean(dr["ESIApplicable"]);
                                        employeeViewModel.ESINo = Convert.ToString(dr["ESINo"]);
                                        employeeViewModel.EmployeePicPath = Convert.ToString(dr["EmployeePicPath"]);
                                        employeeViewModel.EmployeePicName = Convert.ToString(dr["EmployeePicName"]);
                                        employeeViewModel.CompanyId = ContextUser.CompanyId;
                                        employeeViewModel.CreatedBy = ContextUser.UserId;
                                        employeeViewModel.Emp_Status = true;

                                        employeeViewModel.CreatedBy = ContextUser.UserId;
                                        ResponseOut responseOut = employeeBL.ImportEmployee(employeeViewModel);
                                        if (responseOut.status == ActionStatus.Fail)
                                        {
                                            strErrMsg.Append(responseOut.message + ": Error in Inserting in Row #" + rowCounter.ToString() + Environment.NewLine);
                                            dr["UploadStatus"] = false;
                                        }
                                        else
                                        {
                                            dr["UploadStatus"] = true;
                                        }
                                    }
                                    else
                                    {
                                        dr["UploadStatus"] = false;
                                    }
                                }
                                catch (Exception colError)
                                {
                                    dr["UploadStatus"] = false;
                                    strErrMsg.Append(colError.InnerException == null ? colError.Message : Convert.ToString(colError.InnerException) + Environment.NewLine);

                                }
                                rowCounter += 1;

                            }
                            dt.AcceptChanges();
                        }
                        else
                        {
                            strErrMsg.Append("Import not found");
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
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }

        #endregion

        #region Import Department
        [ValidateRequest(true, UserInterfaceHelper.ImportDepartment, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        [HttpGet]
        public ActionResult ImportDepartment()
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
        [ValidateRequest(true, UserInterfaceHelper.ImportDepartment, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        [ActionName("ImportDepartment")]
        [HttpPost]
        public ActionResult ImportDepartmentData()
        {
            UploadUtilityBL utilityBL = new UploadUtilityBL();
            DepartmentBL departmentBL = new DepartmentBL();
            try
            {
                if (Request.Files["FileUpload1"].ContentLength > 0)
                {
                    string extension = System.IO.Path.GetExtension(Request.Files["FileUpload1"].FileName).ToLower();
                    string query = null;
                    string connString = "";
                    string[] validFileTypes = { ".xls", ".xlsx", ".csv" };

                    string path1 = string.Format("{0}/{1}", Server.MapPath("~/Content/Uploads"), Request.Files["FileUpload1"].FileName);
                    if (!Directory.Exists(path1))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Content/Uploads"));
                    }
                    if (validFileTypes.Contains(extension))
                    {
                        DataTable dt = new DataTable();
                        if (System.IO.File.Exists(path1))
                        { System.IO.File.Delete(path1); }
                        Request.Files["FileUpload1"].SaveAs(path1);
                        if (extension == ".csv")
                        {
                            dt = CommonHelper.ConvertCSVtoDataTable(path1);
                            ViewBag.Data = dt;
                        }
                        //Connection String to Excel Workbook  
                        else if (extension.Trim() == ".xls")
                        {
                            connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path1 + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
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
                            DepartmentViewModel departmentViewModel;
                            dt.Columns.Add("UploadStatus", typeof(bool));
                            bool rowVerifyStatus = true;
                            int sequenceNo = 0;
                            foreach (DataRow dr in dt.Rows)
                            {
                                try
                                {
                                    departmentViewModel = new DepartmentViewModel();

                                    //code to validate data in excel//
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["DepartmentName"]).Trim()))
                                    {
                                        strErrMsg.Append("Please Enter Department Name Column data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }


                                    if (string.IsNullOrEmpty(Convert.ToString(dr["DepartmentCode"]).Trim()))
                                    {
                                        strErrMsg.Append("Please Enter Department Code Column data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }

                                    //end of code to validate data in excel//

                                    //code to get Id from Name data in excel//


                                    if (rowVerifyStatus == true)
                                    {
                                        //End of code to get Id from Name data in excel//
                                        departmentViewModel.DepartmentName = Convert.ToString(dr["DepartmentName"]);
                                        departmentViewModel.CompanyId = ContextUser.CompanyId;
                                        departmentViewModel.DepartmentCode = Convert.ToString(dr["DepartmentCode"]);
                                        departmentViewModel.CreatedBy = ContextUser.UserId;
                                        departmentViewModel.DepartmentStatus = true;
                                        ResponseOut responseOut = departmentBL.ImportDepartment(departmentViewModel);
                                        if (responseOut.status == ActionStatus.Fail)
                                        {
                                            strErrMsg.Append(responseOut.message + ": Error in Inserting in Row #" + rowCounter.ToString() + Environment.NewLine);
                                            dr["UploadStatus"] = false;
                                        }
                                        else
                                        {
                                            dr["UploadStatus"] = true;
                                        }
                                    }
                                    else
                                    {
                                        dr["UploadStatus"] = false;
                                    }
                                }
                                catch (Exception colError)
                                {
                                    dr["UploadStatus"] = false;
                                    strErrMsg.Append(colError.InnerException == null ? colError.Message : Convert.ToString(colError.InnerException) + Environment.NewLine);

                                }
                                rowCounter += 1;

                            }
                            dt.AcceptChanges();
                        }
                        else
                        {
                            strErrMsg.Append("Import not found");
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
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }

        #endregion

        #region Import Designation
        [ValidateRequest(true, UserInterfaceHelper.ImportDesignation, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        [HttpGet]
        public ActionResult ImportDesignation()
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
        [ValidateRequest(true, UserInterfaceHelper.ImportDesignation, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        [ActionName("ImportDesignation")]
        [HttpPost]
        public ActionResult ImportDesignationData()
        {
            UploadUtilityBL utilityBL = new UploadUtilityBL();
            DesignationBL designationBL = new DesignationBL();
            try
            {
                if (Request.Files["FileUpload1"].ContentLength > 0)
                {
                    string extension = System.IO.Path.GetExtension(Request.Files["FileUpload1"].FileName).ToLower();
                    string query = null;
                    string connString = "";
                    string[] validFileTypes = { ".xls", ".xlsx", ".csv" };

                    string path1 = string.Format("{0}/{1}", Server.MapPath("~/Content/Uploads"), Request.Files["FileUpload1"].FileName);
                    if (!Directory.Exists(path1))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Content/Uploads"));
                    }
                    if (validFileTypes.Contains(extension))
                    {
                        DataTable dt = new DataTable();
                        if (System.IO.File.Exists(path1))
                        { System.IO.File.Delete(path1); }
                        Request.Files["FileUpload1"].SaveAs(path1);
                        if (extension == ".csv")
                        {
                            dt = CommonHelper.ConvertCSVtoDataTable(path1);
                            ViewBag.Data = dt;
                        }
                        //Connection String to Excel Workbook  
                        else if (extension.Trim() == ".xls")
                        {
                            connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path1 + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
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
                            int departmentId = 0;
                            DesignationViewModel designationViewModel;
                            dt.Columns.Add("UploadStatus", typeof(bool));
                            bool rowVerifyStatus = true;

                            foreach (DataRow dr in dt.Rows)
                            {
                                try
                                {
                                    designationViewModel = new DesignationViewModel();

                                    //code to validate data in excel//
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["DesignationName"]).Trim()))
                                    {
                                        strErrMsg.Append("Please Enter Designation Name Column data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }

                                    if (string.IsNullOrEmpty(Convert.ToString(dr["DesignationCode"]).Trim()))
                                    {
                                        strErrMsg.Append("Please Enter Designation Code Column data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }

                                    //end of code to validate data in excel//

                                    //code to get Id from Name data in excel//
                                    if (!string.IsNullOrEmpty(Convert.ToString(dr["DepartmentName"]).Trim()))
                                    {
                                        departmentId = utilityBL.GetIdByDepartmentName(Convert.ToString(dr["DepartmentName"]));
                                        if (departmentId == 0)
                                        {
                                            strErrMsg.Append("Invalid Department Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                            rowVerifyStatus = false;
                                        }
                                    }
                                    else
                                    {
                                        strErrMsg.Append("Please Enter Department Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }



                                    if (rowVerifyStatus == true)
                                    {
                                        //End of code to get Id from Name data in excel//
                                        designationViewModel.DesignationName = Convert.ToString(dr["DesignationName"]);
                                        designationViewModel.DesignationCode = Convert.ToString(dr["DesignationCode"]);
                                        designationViewModel.CreatedBy = ContextUser.UserId;
                                        designationViewModel.DepartmentId = departmentId;
                                        designationViewModel.DesignationStatus = true;
                                        ResponseOut responseOut = designationBL.ImportDesignation(designationViewModel);
                                        if (responseOut.status == ActionStatus.Fail)
                                        {
                                            strErrMsg.Append(responseOut.message + ": Error in Inserting in Row #" + rowCounter.ToString() + Environment.NewLine);
                                            dr["UploadStatus"] = false;
                                        }
                                        else
                                        {
                                            dr["UploadStatus"] = true;
                                        }
                                    }
                                    else
                                    {
                                        dr["UploadStatus"] = false;
                                    }
                                }
                                catch (Exception colError)
                                {
                                    dr["UploadStatus"] = false;
                                    strErrMsg.Append(colError.InnerException == null ? colError.Message : Convert.ToString(colError.InnerException) + Environment.NewLine);

                                }

                                rowCounter += 1;

                            }
                            dt.AcceptChanges();
                        }
                        else
                        {
                            strErrMsg.Append("Import not found");
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
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }

        #endregion

        #region Import Product MainGroup
        [ValidateRequest(true, UserInterfaceHelper.ImportProductMainGroup, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        [HttpGet]
        public ActionResult ImportProductMainGroup()
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
        [ValidateRequest(true, UserInterfaceHelper.ImportProductMainGroup, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        [ActionName("ImportProductMainGroup")]
        [HttpPost]
        public ActionResult ImportProductMainGroupData()
        {
            UploadUtilityBL utilityBL = new UploadUtilityBL();
            ProductMainGroupBL productMainGroupBL = new ProductMainGroupBL();
            try
            {
                if (Request.Files["FileUpload1"].ContentLength > 0)
                {
                    string extension = System.IO.Path.GetExtension(Request.Files["FileUpload1"].FileName).ToLower();
                    string query = null;
                    string connString = "";
                    string[] validFileTypes = { ".xls", ".xlsx", ".csv" };

                    string path1 = string.Format("{0}/{1}", Server.MapPath("~/Content/Uploads"), Request.Files["FileUpload1"].FileName);
                    if (!Directory.Exists(path1))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Content/Uploads"));
                    }
                    if (validFileTypes.Contains(extension))
                    {
                        DataTable dt = new DataTable();
                        if (System.IO.File.Exists(path1))
                        { System.IO.File.Delete(path1); }
                        Request.Files["FileUpload1"].SaveAs(path1);
                        if (extension == ".csv")
                        {
                            dt = CommonHelper.ConvertCSVtoDataTable(path1);
                            ViewBag.Data = dt;
                        }
                        //Connection String to Excel Workbook  
                        else if (extension.Trim() == ".xls")
                        {
                            connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path1 + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
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
                            ProductMainGroupViewModel productMainGroupViewModel;
                            dt.Columns.Add("UploadStatus", typeof(bool));
                            bool rowVerifyStatus = true;

                            foreach (DataRow dr in dt.Rows)
                            {
                                try
                                {

                                    productMainGroupViewModel = new ProductMainGroupViewModel();
                                    //code to validate data in excel//
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["ProductMainGroupName"]).Trim()))
                                    {
                                        strErrMsg.Append("Please Enter Product Main Group Name Column data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }


                                    if (string.IsNullOrEmpty(Convert.ToString(dr["ProductMainGroupCode"]).Trim()))
                                    {
                                        strErrMsg.Append("Please Enter Product Main Group Code Column data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }

                                    //end of code to validate data in excel//

                                    //code to get Id from Name data in excel//


                                    if (rowVerifyStatus == true)
                                    {
                                        //End of code to get Id from Name data in excel//
                                        productMainGroupViewModel.ProductMainGroupName = Convert.ToString(dr["ProductMainGroupName"]);
                                        productMainGroupViewModel.ProductMainGroupCode = Convert.ToString(dr["ProductMainGroupCode"]);
                                        productMainGroupViewModel.ProductMainGroup_Status = true;
                                        ResponseOut responseOut = productMainGroupBL.ImportProductMainGroup(productMainGroupViewModel);
                                        if (responseOut.status == ActionStatus.Fail)
                                        {
                                            strErrMsg.Append(responseOut.message + ": Error in Inserting in Row #" + rowCounter.ToString() + Environment.NewLine);
                                            dr["UploadStatus"] = false;
                                        }
                                        else
                                        {
                                            dr["UploadStatus"] = true;
                                        }
                                    }
                                    else
                                    {
                                        dr["UploadStatus"] = false;
                                    }
                                }
                                catch (Exception colError)
                                {
                                    dr["UploadStatus"] = false;
                                    strErrMsg.Append(colError.InnerException == null ? colError.Message : Convert.ToString(colError.InnerException) + Environment.NewLine);

                                }
                                rowCounter += 1;

                            }
                            dt.AcceptChanges();
                        }
                        else
                        {
                            strErrMsg.Append("Import not found");
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
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }

        #endregion

        #region Import Product SubGroup
        [ValidateRequest(true, UserInterfaceHelper.ImportProductSubGroup, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        [HttpGet]
        public ActionResult ImportProductSubGroup()
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
        [ValidateRequest(true, UserInterfaceHelper.ImportProductSubGroup, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        [ActionName("ImportProductSubGroup")]
        [HttpPost]
        public ActionResult ImportProductSubGroupData()
        {
            UploadUtilityBL utilityBL = new UploadUtilityBL();
            ProductSubGroupBL productSubGroupBL = new ProductSubGroupBL();
            try
            {
                if (Request.Files["FileUpload1"].ContentLength > 0)
                {
                    string extension = System.IO.Path.GetExtension(Request.Files["FileUpload1"].FileName).ToLower();
                    string query = null;
                    string connString = "";
                    string[] validFileTypes = { ".xls", ".xlsx", ".csv" };

                    string path1 = string.Format("{0}/{1}", Server.MapPath("~/Content/Uploads"), Request.Files["FileUpload1"].FileName);
                    if (!Directory.Exists(path1))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Content/Uploads"));
                    }
                    if (validFileTypes.Contains(extension))
                    {
                        DataTable dt = new DataTable();
                        if (System.IO.File.Exists(path1))
                        { System.IO.File.Delete(path1); }
                        Request.Files["FileUpload1"].SaveAs(path1);
                        if (extension == ".csv")
                        {
                            dt = CommonHelper.ConvertCSVtoDataTable(path1);
                            ViewBag.Data = dt;
                        }
                        //Connection String to Excel Workbook  
                        else if (extension.Trim() == ".xls")
                        {
                            connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path1 + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
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
                            int productMainGroupID = 0;
                            ProductSubGroupViewModel productSubGroupViewModel;
                            dt.Columns.Add("UploadStatus", typeof(bool));
                            bool rowVerifyStatus = true;
                            foreach (DataRow dr in dt.Rows)
                            {

                                try
                                {
                                    productSubGroupViewModel = new ProductSubGroupViewModel();
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["ProductSubGroupName"]).Trim()))
                                    {
                                        strErrMsg.Append("Please Enter Product Sub Group Name Column has not proper data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }

                                    if (string.IsNullOrEmpty(Convert.ToString(dr["ProductSubGroupCode"]).Trim()))
                                    {
                                        strErrMsg.Append("Please Enter Product Sub Group Code Column data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }

                                    //end of code to validate data in excel//

                                    //code to get Id from Name data in excel//
                                    if (!string.IsNullOrEmpty(Convert.ToString(dr["ProductMainGroupName"]).Trim()))
                                    {
                                        productMainGroupID = utilityBL.GetIdByProductMainGroupName(Convert.ToString(dr["ProductMainGroupName"]));
                                        if (productMainGroupID == 0)
                                        {
                                            strErrMsg.Append("Invalid Product Main Group Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                            rowVerifyStatus = false;
                                        }
                                    }
                                    else
                                    {
                                        strErrMsg.Append("Please Enter Product Main Group Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;

                                    }

                                    if (rowVerifyStatus == true)
                                    {
                                        //End of code to get Id from Name data in excel//
                                        productSubGroupViewModel.ProductSubGroupName = Convert.ToString(dr["ProductSubGroupName"]);
                                        productSubGroupViewModel.ProductSubGroupCode = Convert.ToString(dr["ProductSubGroupCode"]);
                                        productSubGroupViewModel.ProductMainGroupId = productMainGroupID;
                                        productSubGroupViewModel.ProductSubGroup_Status = true;
                                        ResponseOut responseOut = productSubGroupBL.ImportProductSubGroup(productSubGroupViewModel);
                                        if (responseOut.status == ActionStatus.Fail)
                                        {
                                            strErrMsg.Append(responseOut.message + ": Error in Inserting in Row #" + rowCounter.ToString() + Environment.NewLine);
                                            dr["UploadStatus"] = false;
                                        }
                                        else
                                        {
                                            dr["UploadStatus"] = true;
                                        }
                                    }
                                    else
                                    {
                                        dr["UploadStatus"] = false;
                                    }
                                }
                                catch (Exception colError)
                                {
                                    dr["UploadStatus"] = false;
                                    strErrMsg.Append(colError.InnerException == null ? colError.Message : Convert.ToString(colError.InnerException) + Environment.NewLine);

                                }
                                rowCounter += 1;

                            }
                            dt.AcceptChanges();
                        }
                        else
                        {
                            strErrMsg.Append("Import not found");
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
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }

        #endregion

        #region Import Product
        [ValidateRequest(true, UserInterfaceHelper.ImportProduct, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        [HttpGet]
        public ActionResult ImportProduct()
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
        [ValidateRequest(true, UserInterfaceHelper.ImportProduct, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        [ActionName("ImportProduct")]
        [HttpPost]
        public ActionResult ImportProductData()
        {
            UploadUtilityBL utilityBL = new UploadUtilityBL();
            ProductBL productBL = new ProductBL();
            try
            {
                if (Request.Files["FileUpload1"].ContentLength > 0)
                {
                    string extension = System.IO.Path.GetExtension(Request.Files["FileUpload1"].FileName).ToLower();
                    string connString = "";
                    string[] validFileTypes = { ".xls", ".xlsx", ".csv" };

                    string path1 = string.Format("{0}/{1}", Server.MapPath("~/Content/Uploads"), Request.Files["FileUpload1"].FileName);
                    if (!Directory.Exists(path1))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Content/Uploads"));
                    }
                    if (validFileTypes.Contains(extension))
                    {
                        DataTable dt = new DataTable();
                        if (System.IO.File.Exists(path1))
                        { System.IO.File.Delete(path1); }
                        Request.Files["FileUpload1"].SaveAs(path1);
                        if (extension == ".csv")
                        {
                            dt = CommonHelper.ConvertCSVtoDataTable(path1);
                            ViewBag.Data = dt;
                        }
                        //Connection String to Excel Workbook  
                        else if (extension.Trim() == ".xls")
                        {
                            connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path1 + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
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

                            int UOMId = 0;
                            int productTypeId = 0;
                            int rowCounter = 1;
                            int productMainGroupID = 0;
                            int productSubGroupID = 0;
                            int sizeID = 0;
                            int manufacturerID = 0;

                            ProductViewModel productViewModel;
                            dt.Columns.Add("UploadStatus", typeof(bool));
                            bool rowVerifyStatus = true;
                            Random rnd = new Random(50000);

                            foreach (DataRow dr in dt.Rows)
                            {
                                try
                                {
                                    sizeID = 0;
                                    manufacturerID = 0;

                                    productViewModel = new ProductViewModel();
                                    //code to validate data in excel//

                                    if (string.IsNullOrEmpty(Convert.ToString(dr["ProductName"]).Trim()))
                                    {
                                        strErrMsg.Append("Please Enter Product Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }


                                    if (string.IsNullOrEmpty(Convert.ToString(dr["ProductCode"]).Trim()))
                                    {
                                        strErrMsg.Append("Please Enter Product Code Column data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }


                                    decimal reOrderQty = 0;
                                    decimal cGST_Perc = 0;
                                    decimal sGST_Perc = 0;
                                    decimal iGST_Perc = 0;


                                    if (!string.IsNullOrEmpty(Convert.ToString(dr["ProductTypeName"])))
                                    {
                                        productTypeId = utilityBL.GetIdByProductTypeName(Convert.ToString(dr["ProductTypeName"]));
                                        if (productTypeId == 0)
                                        {
                                            strErrMsg.Append("Invalid Product Type Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                            rowVerifyStatus = false;

                                        }
                                    }
                                    else
                                    {
                                        strErrMsg.Append("Please Enter Product Type Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }

                                    if (!string.IsNullOrEmpty(Convert.ToString(dr["ProductMainGroupName"])))
                                    {
                                        productMainGroupID = utilityBL.GetIdByProductMainGroupName(Convert.ToString(dr["ProductMainGroupName"]));
                                        if (productMainGroupID == 0)
                                        {
                                            strErrMsg.Append("Invalid Product MainGroup Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                            rowVerifyStatus = false;
                                        }
                                    }
                                    else
                                    {
                                        strErrMsg.Append("Please Enter MainGroup Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }

                                    if (!string.IsNullOrEmpty(Convert.ToString(dr["ProductSubGroupName"])))
                                    {
                                        productSubGroupID = utilityBL.GetIdByProductSubGroupName(Convert.ToString(dr["ProductSubGroupName"]));
                                        if (productSubGroupID == 0)
                                        {
                                            strErrMsg.Append("Invalid Product SubGroup Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                            rowVerifyStatus = false;
                                        }
                                    }
                                    else
                                    {
                                        strErrMsg.Append("Please Enter SubGroup Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }

                                    if (!string.IsNullOrEmpty(Convert.ToString(dr["UOMName"])))
                                    {
                                        UOMId = utilityBL.GetIdByUOMName(Convert.ToString(dr["UOMName"]));
                                        if (UOMId == 0)
                                        {
                                            strErrMsg.Append("Invalid UOM Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                            rowVerifyStatus = false;
                                        }
                                    }
                                    else
                                    {
                                        strErrMsg.Append("Please Enter UOM Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;

                                    }

                                    if (!string.IsNullOrEmpty(Convert.ToString(dr["AssemblyType"])))
                                    {
                                        var AssemblyType = Convert.ToString(dr["AssemblyType"]);
                                        if (AssemblyType != "MA" && AssemblyType != "SA" && AssemblyType != "RC")
                                        {
                                            strErrMsg.Append("Assembly Type should be MA= Main Assembly, SA=Sub Assembly, RC=Raw Component in Row #" + rowCounter.ToString() + Environment.NewLine);
                                            rowVerifyStatus = false;
                                        }
                                    }
                                    else
                                    {
                                        strErrMsg.Append("Please Enter Assembly Type  MA= Main Assembly, SA=Sub Assembly, RC=Raw Component in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;

                                    }


                                    if (string.IsNullOrEmpty(Convert.ToString(dr["ReOrderQty"])))
                                    {
                                        strErrMsg.Append("Please Enter ReOrderQty data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }
                                    else if (decimal.TryParse(Convert.ToString(dr["ReOrderQty"]), out reOrderQty) == false)
                                    {
                                        strErrMsg.Append("ReOrderQty Column has not proper data in Rows #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }

                                    //  pressureID = 0;

                                    if (!string.IsNullOrEmpty(Convert.ToString(dr["Size"])))
                                    {
                                        sizeID = utilityBL.GetIdBySizeDesc(Convert.ToString(dr["Size"]));
                                        if (sizeID == 0)
                                        {
                                            strErrMsg.Append("Invalid Size Descriptoin data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                            rowVerifyStatus = false;
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(Convert.ToString(dr["Manufacturer"])))
                                    {
                                        manufacturerID = utilityBL.GetIdByManufacturerName(Convert.ToString(dr["Manufacturer"]));
                                        if (manufacturerID == 0)
                                        {
                                            strErrMsg.Append("Invalid Manufacturer name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                            rowVerifyStatus = false;
                                        }
                                    }

                                    if (string.IsNullOrEmpty(Convert.ToString(dr["CGST_Perc"])))
                                    {
                                        dr["CGST_Perc"] = 0;
                                    }
                                    else if (decimal.TryParse(Convert.ToString(dr["CGST_Perc"]), out cGST_Perc) == false)
                                    {
                                        strErrMsg.Append("CGST_Perc Column has not proper data in Rows #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }


                                    if (string.IsNullOrEmpty(Convert.ToString(dr["SGST_Perc"])))
                                    {
                                        dr["SGST_Perc"] = 0;
                                    }
                                    else if (decimal.TryParse(Convert.ToString(dr["SGST_Perc"]), out sGST_Perc) == false)
                                    {
                                        strErrMsg.Append("SGST_Perc Column has not proper data in Rows #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }


                                    if (string.IsNullOrEmpty(Convert.ToString(dr["IGST_Perc"])))
                                    {
                                        dr["IGST_Perc"] = 0;
                                    }
                                    else if (decimal.TryParse(Convert.ToString(dr["IGST_Perc"]), out iGST_Perc) == false)
                                    {
                                        strErrMsg.Append("IGST_Perc Column has not proper data in Rows #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }


                                    if (rowVerifyStatus == true)
                                    {
                                        //End of code to get Id from Name data in excel//

                                        productViewModel.ProductName = Convert.ToString(dr["ProductName"]);
                                        productViewModel.ProductCode = Convert.ToString(dr["ProductCode"]);
                                        productViewModel.ProductShortDesc = Convert.ToString(dr["ProductShortDesc"]);
                                        productViewModel.ProductFullDesc = Convert.ToString(dr["ProductFullDesc"]);
                                        productViewModel.CompanyId = ContextUser.CompanyId;
                                        productViewModel.ProductTypeId = productTypeId;
                                        productViewModel.ProductMainGroupId = productMainGroupID;
                                        productViewModel.ProductSubGroupId = productSubGroupID;
                                        productViewModel.AssemblyType = Convert.ToString(dr["AssemblyType"]);
                                        productViewModel.UOMId = UOMId;
                                        productViewModel.SizeId = sizeID;
                                        productViewModel.Length = Convert.ToString(dr["Length"]);
                                        productViewModel.ManufacturerId = manufacturerID;

                                        productViewModel.PurchasePrice = 0;
                                        productViewModel.SalePrice = 0;
                                        productViewModel.LocalTaxRate = 0;
                                        productViewModel.CentralTaxRate = 0;
                                        productViewModel.OtherTaxRate = 0;
                                        productViewModel.IsSerializedProduct = true;
                                        productViewModel.BrandName = Convert.ToString(dr["Manufacturer"]);
                                        productViewModel.ReOrderQty = Convert.ToDecimal(dr["ReOrderQty"]);
                                        productViewModel.MinOrderQty = 0;
                                        //productViewModel.ProductPicPath = Convert.ToString(dr["UserPicPath"]);
                                        productViewModel.CGST_Perc = Convert.ToDecimal(dr["CGST_Perc"]);
                                        productViewModel.SGST_Perc = Convert.ToDecimal(dr["SGST_Perc"]);
                                        productViewModel.IGST_Perc = Convert.ToDecimal(dr["IGST_Perc"]);
                                        productViewModel.HSN_Code = Convert.ToString(dr["HSN_Code"]);
                                        productViewModel.CreatedBy = ContextUser.UserId;
                                        productViewModel.GST_Exempt = false;
                                        productViewModel.Product_Status = true;

                                        ResponseOut responseOut = productBL.ImportProduct(productViewModel);
                                        if (responseOut.status == ActionStatus.Fail)
                                        {
                                            strErrMsg.Append(responseOut.message + ": Error in Inserting in Row #" + rowCounter.ToString() + Environment.NewLine);
                                            dr["UploadStatus"] = false;
                                        }
                                        else
                                        {
                                            dr["UploadStatus"] = true;
                                        }
                                    }
                                    else
                                    {
                                        dr["UploadStatus"] = false;
                                    }


                                }
                                catch (Exception colError)
                                {
                                    dr["UploadStatus"] = false;
                                    strErrMsg.Append(colError.InnerException == null ? colError.Message : Convert.ToString(colError.InnerException) + Environment.NewLine);

                                }
                                rowCounter += 1;
                            }


                            dt.AcceptChanges();
                        }
                        else
                        {
                            strErrMsg.Append("Import not found");
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
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }

        #endregion

        #region Import ProductBOM
        [ValidateRequest(true, UserInterfaceHelper.ImportProductBOM, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        [HttpGet]
        public ActionResult ImportProductBOM()
        {
            try
            { }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }
        [ValidateRequest(true, UserInterfaceHelper.ImportProductBOM, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        [ActionName("ImportProductBOM")]
        [HttpPost]
        public ActionResult ImportProductBOMData()
        {
            UploadUtilityBL utilityBL = new UploadUtilityBL();

            ProductBOMBL productBOMBL = new ProductBOMBL();

            try
            {
                if (Request.Files["FileUpload1"].ContentLength > 0)
                {
                    string extension = System.IO.Path.GetExtension(Request.Files["FileUpload1"].FileName).ToLower();
                    string connString = "";
                    string[] validFileTypes = { ".xls", ".xlsx", ".csv" };

                    string path1 = string.Format("{0}/{1}", Server.MapPath("~/Content/Uploads"), Request.Files["FileUpload1"].FileName);
                    if (!Directory.Exists(path1))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Content/Uploads"));
                    }
                    if (validFileTypes.Contains(extension))
                    {
                        DataTable dt = new DataTable();
                        if (System.IO.File.Exists(path1))
                        { System.IO.File.Delete(path1); }
                        Request.Files["FileUpload1"].SaveAs(path1);
                        if (extension == ".csv")
                        {
                            dt = CommonHelper.ConvertCSVtoDataTable(path1);
                            ViewBag.Data = dt;
                        }
                        //Connection String to Excel Workbook  
                        else if (extension.Trim() == ".xls")
                        {
                            connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path1 + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
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


                            long assemblyID = 0;
                            int rowCounter = 1;
                            long productID = 0;
                            string productMainGroupName = "";
                            ProductViewModel prodViewModel;
                            ProductBOMViewModel productBOMViewModel;
                            dt.Columns.Add("UploadStatus", typeof(bool));
                            bool rowVerifyStatus = true;
                            Random rnd = new Random(50000);
                            foreach (DataRow dr in dt.Rows)
                            {
                                try
                                {
                                    decimal bOMQty = 0;
                                    productBOMViewModel = new ProductBOMViewModel();
                                    prodViewModel = new ProductViewModel();
                                    //code to validate data in excel//
                                    //if (string.IsNullOrEmpty(Convert.ToString(dr["ProductName"])))
                                    //{
                                    //    strErrMsg.Append("Product Name Column has not proper data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                    //    rowVerifyStatus = false;
                                    //}
                                    //else if (string.IsNullOrEmpty(Convert.ToString(dr["AssemblyName"])))
                                    //{
                                    //    strErrMsg.Append("Assembly Name Column has not proper data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                    //    rowVerifyStatus = false;
                                    //}


                                    //end of code to validate data in excel//

                                    //code to get Id from Name data in excel//            

                                    if (!string.IsNullOrEmpty(Convert.ToString(dr["AssemblyName"])))
                                    {

                                        assemblyID = utilityBL.GetIdByAssemblyName(Convert.ToString(dr["AssemblyName"]));
                                        if (assemblyID == 0)
                                        {
                                            strErrMsg.Append("Invalid Assembly Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                            rowVerifyStatus = false;
                                        }

                                    }
                                    else if (string.IsNullOrEmpty(Convert.ToString(dr["AssemblyName"])))
                                    {
                                        strErrMsg.Append("Please Enter Assembly Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }
                                    if (!string.IsNullOrEmpty(Convert.ToString(dr["ProductName"])))
                                    {
                                        productID = utilityBL.GetIdByProductName(Convert.ToString(dr["ProductName"]));
                                        if (productID == 0)
                                        {
                                            strErrMsg.Append("Invalid Product Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                            rowVerifyStatus = false;
                                        }
                                        else
                                        {
                                            prodViewModel = productBOMBL.GetProductMainGroupNameByProductID(productID);
                                        }
                                    }
                                    else if (string.IsNullOrEmpty(Convert.ToString(dr["ProductName"])))
                                    {
                                        strErrMsg.Append("Please Enter Product Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }

                                    if (string.IsNullOrEmpty(Convert.ToString(dr["BOMQty"])))
                                    {
                                        strErrMsg.Append("Please Enter BOM Qty data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }
                                    else if (decimal.TryParse(Convert.ToString(dr["BOMQty"]), out bOMQty) == false)
                                    {
                                        strErrMsg.Append("BOM Qty Column has not proper data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }




                                    if (rowVerifyStatus == true)
                                    {
                                        //End of code to get Id from Name data in excel//
                                        productBOMViewModel.AssemblyId = assemblyID;
                                        productBOMViewModel.ProductId = productID;
                                        productBOMViewModel.ProcessType =prodViewModel.ProductMainGroupName;
                                        productBOMViewModel.BOMQty = Convert.ToDecimal(dr["BOMQty"]);
                                        productBOMViewModel.CompanyId = ContextUser.CompanyId;
                                        productBOMViewModel.CreatedBy = ContextUser.UserId;
                                        productBOMViewModel.BOM_Status = true;
                                        ResponseOut responseOut = productBOMBL.ImportProductBOM(productBOMViewModel);
                                        if (responseOut.status == ActionStatus.Fail)
                                        {
                                            strErrMsg.Append(responseOut.message + ": Error in Inserting in Row #" + rowCounter.ToString() + Environment.NewLine);
                                            dr["UploadStatus"] = false;
                                        }
                                        else
                                        {
                                            dr["UploadStatus"] = true;
                                        }
                                    }
                                    else
                                    {
                                        dr["UploadStatus"] = false;
                                    }
                                }
                                catch (Exception colError)
                                {
                                    dr["UploadStatus"] = false;
                                    strErrMsg.Append(colError.InnerException == null ? colError.Message : Convert.ToString(colError.InnerException) + Environment.NewLine);

                                }
                                rowCounter += 1;

                            }
                            dt.AcceptChanges();
                        }
                        else
                        {
                            strErrMsg.Append("Import not found");
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
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }

        #endregion


        #region Import Product Opening Stock
        [ValidateRequest(true, UserInterfaceHelper.ImportProductOpeningStock, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        [HttpGet]
        public ActionResult ImportProductOpeningStock()
        {
            try
            { }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }
        [ValidateRequest(true, UserInterfaceHelper.ImportProductOpeningStock, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        [ActionName("ImportProductOpeningStock")]
        [HttpPost]
        public ActionResult ImportProductOpeningStockData()
        {
            UploadUtilityBL utilityBL = new UploadUtilityBL();
            ProductOpeningBL productOpeningBL = new ProductOpeningBL();
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                if (Request.Files["FileUpload1"].ContentLength > 0)
                {
                    string extension = System.IO.Path.GetExtension(Request.Files["FileUpload1"].FileName).ToLower();
                    string connString = "";
                    string[] validFileTypes = { ".xls", ".xlsx", ".csv" };

                    string path1 = string.Format("{0}/{1}", Server.MapPath("~/Content/Uploads"), Request.Files["FileUpload1"].FileName);
                    if (!Directory.Exists(path1))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Content/Uploads"));
                    }
                    if (validFileTypes.Contains(extension))
                    {
                        DataTable dt = new DataTable();
                        if (System.IO.File.Exists(path1))
                        { System.IO.File.Delete(path1); }
                        Request.Files["FileUpload1"].SaveAs(path1);
                        if (extension == ".csv")
                        {
                            dt = CommonHelper.ConvertCSVtoDataTable(path1);
                            ViewBag.Data = dt;
                        }
                        //Connection String to Excel Workbook  
                        else if (extension.Trim() == ".xls")
                        {
                            connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path1 + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
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


                            long companyBranchId = 0;
                            int rowCounter = 1;
                            long productID = 0;

                            ProductOpeningViewModel productOpeningViewModel;
                            dt.Columns.Add("UploadStatus", typeof(bool));
                            bool rowVerifyStatus = true;
                            Random rnd = new Random(50000);
                            foreach (DataRow dr in dt.Rows)
                            {
                                try
                                {
                                    productOpeningViewModel = new ProductOpeningViewModel();
                                    //code to validate data in excel//

                                    decimal openingQty = 0;
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["OpeningQty"])))
                                    {
                                        strErrMsg.Append("Please Enter Opening Qty data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }
                                    else if (decimal.TryParse(Convert.ToString(dr["OpeningQty"]), out openingQty) == false)
                                    {
                                        strErrMsg.Append("Opening Qty Column has not proper data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }


                                    //end of code to validate data in excel//

                                    //code to get Id from Name data in excel//            

                                    if (!string.IsNullOrEmpty(Convert.ToString(dr["CompanyBranchName"])))
                                    {

                                        companyBranchId = utilityBL.GetIdByCompanyBranchName(Convert.ToString(dr["CompanyBranchName"]));
                                        if (companyBranchId == 0)
                                        {
                                            strErrMsg.Append("Invalid Company Branch Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                            rowVerifyStatus = false;
                                        }

                                    }
                                    else
                                    {
                                        strErrMsg.Append("Please Enter Company Branch Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;

                                    }
                                    if (!string.IsNullOrEmpty(Convert.ToString(dr["ProductName"])))
                                    {
                                        productID = utilityBL.GetIdByProductNameForOpening(Convert.ToString(dr["ProductName"]));
                                        if (productID == 0)
                                        {
                                            strErrMsg.Append("Invalid Product Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                            rowVerifyStatus = false;
                                        }
                                    }
                                    else
                                    {
                                        strErrMsg.Append("Please Enter Product Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }


                                    if (rowVerifyStatus == true)
                                    {

                                        //End of code to get Id from Name data in excel//
                                        productOpeningViewModel.CompanyBranchId = Convert.ToInt32(companyBranchId);
                                        productOpeningViewModel.ProductId = productID;
                                        productOpeningViewModel.CompanyId = ContextUser.CompanyId;
                                        productOpeningViewModel.FinYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year; ;
                                        productOpeningViewModel.OpeningQty = Convert.ToDecimal(dr["OpeningQty"]);
                                        productOpeningViewModel.CreatedBy = ContextUser.UserId;
                                        ResponseOut responseOut = productOpeningBL.ImportProductOpening(productOpeningViewModel);
                                        if (responseOut.status == ActionStatus.Fail)
                                        {
                                            strErrMsg.Append(responseOut.message + ": Error in Inserting in Row #" + rowCounter.ToString() + Environment.NewLine);
                                            dr["UploadStatus"] = false;
                                        }
                                        else
                                        {
                                            dr["UploadStatus"] = true;
                                        }
                                    }
                                    else
                                    {
                                        dr["UploadStatus"] = false;
                                    }
                                }
                                catch (Exception colError)
                                {
                                    dr["UploadStatus"] = false;
                                    strErrMsg.Append(colError.InnerException == null ? colError.Message : Convert.ToString(colError.InnerException) + Environment.NewLine);

                                }
                                rowCounter += 1;

                            }
                            dt.AcceptChanges();
                        }
                        else
                        {
                            strErrMsg.Append("Import not found");
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
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }

        #endregion


        #region Import Vendor   
        [ValidateRequest(true, UserInterfaceHelper.ImportVendor, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        [HttpGet]
        public ActionResult ImportVendor()
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
        [ValidateRequest(true, UserInterfaceHelper.ImportVendor, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        [ActionName("ImportVendor")]
        [HttpPost]
        public ActionResult ImportVendorData()
        {
            UploadUtilityBL utilityBL = new UploadUtilityBL();
            VendorBL vendorBL = new VendorBL();
            try
            {
                if (Request.Files["FileUpload1"].ContentLength > 0)
                {
                    string extension = System.IO.Path.GetExtension(Request.Files["FileUpload1"].FileName).ToLower();
                    string connString = "";
                    string[] validFileTypes = { ".xls", ".xlsx", ".csv" };

                    string path1 = string.Format("{0}/{1}", Server.MapPath("~/Content/Uploads"), Request.Files["FileUpload1"].FileName);
                    if (!Directory.Exists(path1))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Content/Uploads"));
                    }
                    if (validFileTypes.Contains(extension))
                    {
                        DataTable dt = new DataTable();
                        if (System.IO.File.Exists(path1))
                        { System.IO.File.Delete(path1); }
                        Request.Files["FileUpload1"].SaveAs(path1);
                        if (extension == ".csv")
                        {
                            dt = CommonHelper.ConvertCSVtoDataTable(path1);
                            ViewBag.Data = dt;
                        }
                        //Connection String to Excel Workbook  
                        else if (extension.Trim() == ".xls")
                        {
                            connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path1 + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
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
                            int stateId = 0;
                            int countryId = 0;
                            int creditDays = 0;
                            int creditLimit = 0;
                            int rowCounter = 1;

                            VendorViewModel vendorViewModel;
                            dt.Columns.Add("UploadStatus", typeof(bool));
                            bool rowVerifyStatus = true;
                            Random rnd = new Random(50000);
                            foreach (DataRow dr in dt.Rows)
                            {
                                try
                                {
                                    vendorViewModel = new VendorViewModel();
                                    //code to validate data in excel//
                                    bool gST_Exempt;
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["VendorName"]).Trim()))
                                    {
                                        strErrMsg.Append("Please Enter Vendor Name Column data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["VendorCode"]).Trim()))
                                    {
                                        strErrMsg.Append("Please Enter Vendor Code Column data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }

                                    if (string.IsNullOrEmpty(Convert.ToString(dr["ContactPersonName"]).Trim()))
                                    {
                                        strErrMsg.Append("Please Enter Contact Person Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }
                                    //else if (string.IsNullOrEmpty(Convert.ToString(dr["Designation"])))
                                    //{
                                    //    strErrMsg.Append("Designation Column has not proper data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                    //    rowVerifyStatus = false;
                                    //}
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["Email"]).Trim()))
                                    {
                                        dr["Email"] = "";
                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["MobileNo"]).Trim()))
                                    {
                                        strErrMsg.Append("Please Enter Mobile No. Column data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["ContactNo"]).Trim()))
                                    {
                                        dr["ContactNo"] = "";
                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["Fax"]).Trim()))
                                    {
                                        dr["Fax"] = "";
                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["Address"]).Trim()))
                                    {
                                        strErrMsg.Append("Please Enter Primary Address Column data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["City"]).Trim()))
                                    {
                                        strErrMsg.Append("Please Enter City Name Column data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;

                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["PinCode"]).Trim()))
                                    {
                                        dr["PinCode"] = "";
                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["CSTNo"]).Trim()))
                                    {
                                        dr["CSTNo"] = "";
                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["TINNo"]).Trim()))
                                    {
                                        dr["TINNo"] = "";
                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["PANNo"]).Trim()))
                                    {
                                        dr["PANNo"] = "";
                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["GSTNo"]).Trim()))
                                    {
                                        dr["GSTNo"] = "";
                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["ExciseNo"]).Trim()))
                                    {
                                        dr["ExciseNo"] = "";
                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["CreditLimit"])))
                                    {
                                        dr["CreditLimit"] = 0;
                                    }
                                    else if (int.TryParse(Convert.ToString(dr["CreditLimit"]), out creditLimit) == false)
                                    {
                                        strErrMsg.Append("Credit Limit Column has not proper data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;

                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["CreditDays"])))
                                    {
                                        dr["CreditDays"] = 0;
                                    }
                                    else if (int.TryParse(Convert.ToString(dr["CreditDays"]), out creditDays) == false)
                                    {
                                        strErrMsg.Append("Credit Days Column has not proper data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;

                                    }
                                    //end of code to validate data in excel//

                                    //code to get Id from Name data in excel//

                                    if (!string.IsNullOrEmpty(Convert.ToString(dr["StateName"])))
                                    {
                                        stateId = utilityBL.GetIdByStateName(Convert.ToString(dr["StateName"]));
                                        if (stateId == 0)
                                        {
                                            strErrMsg.Append("Invalid State Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                            rowVerifyStatus = false;
                                        }
                                    }
                                    else
                                    {
                                        strErrMsg.Append("Please Enter State Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;

                                    }

                                    if (!string.IsNullOrEmpty(Convert.ToString(dr["CountryName"])))
                                    {
                                        countryId = utilityBL.GetIdByCountryName(Convert.ToString(dr["CountryName"]));
                                        if (countryId == 0)
                                        {
                                            strErrMsg.Append("Invalid Country Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                            rowVerifyStatus = false;
                                        }
                                    }
                                    else
                                    {
                                        strErrMsg.Append("Please Enter Country Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;

                                    }


                                    if (string.IsNullOrEmpty(Convert.ToString(dr["GST_Exempt"])))
                                    {
                                        dr["GST_Exempt"] = false;
                                    }
                                    else if (bool.TryParse(Convert.ToString(dr["GST_Exempt"]), out gST_Exempt) == false)
                                    {
                                        strErrMsg.Append("GST_Exempt Column has not proper data in Rows #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }




                                    if (rowVerifyStatus == true)
                                    {
                                        //End of code to get Id from Name data in excel//

                                        vendorViewModel.VendorCode = Convert.ToString(dr["VendorCode"]);
                                        vendorViewModel.VendorName = Convert.ToString(dr["VendorName"]);
                                        vendorViewModel.ContactPersonName = Convert.ToString(dr["ContactPersonName"]);

                                        vendorViewModel.Email = Convert.ToString(dr["Email"]);
                                        vendorViewModel.MobileNo = Convert.ToString(dr["MobileNo"]);
                                        vendorViewModel.ContactNo = Convert.ToString(dr["ContactNo"]);
                                        vendorViewModel.Fax = Convert.ToString(dr["Fax"]);
                                        vendorViewModel.Address = Convert.ToString(dr["Address"]);
                                        vendorViewModel.City = Convert.ToString(dr["City"]);
                                        vendorViewModel.StateId = stateId;
                                        vendorViewModel.CountryId = countryId;
                                        vendorViewModel.PinCode = Convert.ToString(dr["PinCode"]);
                                        vendorViewModel.CSTNo = Convert.ToString(dr["CSTNo"]);
                                        vendorViewModel.GSTNo = Convert.ToString(dr["GSTNo"]);
                                        vendorViewModel.TINNo = Convert.ToString(dr["TINNo"]);
                                        vendorViewModel.PANNo = Convert.ToString(dr["PANNo"]);
                                        vendorViewModel.ExciseNo = Convert.ToString(dr["ExciseNo"]);
                                        vendorViewModel.CreditLimit = creditLimit;
                                        vendorViewModel.CreditDays = creditDays;
                                        vendorViewModel.CompanyId = ContextUser.CompanyId;
                                        vendorViewModel.CreatedBy = ContextUser.UserId;
                                        vendorViewModel.GST_Exempt = Convert.ToBoolean(dr["GST_Exempt"]);
                                        vendorViewModel.Vendor_Status = true;

                                        vendorViewModel.CreatedBy = ContextUser.UserId;
                                        ResponseOut responseOut = vendorBL.ImportVendor(vendorViewModel);
                                        if (responseOut.status == ActionStatus.Fail)
                                        {
                                            strErrMsg.Append(responseOut.message + ": Error in Inserting in Row #" + rowCounter.ToString() + Environment.NewLine);
                                            dr["UploadStatus"] = false;
                                        }
                                        else
                                        {
                                            dr["UploadStatus"] = true;
                                        }
                                    }
                                    else
                                    {
                                        dr["UploadStatus"] = false;
                                    }
                                }
                                catch (Exception colError)
                                {
                                    dr["UploadStatus"] = false;
                                    strErrMsg.Append(colError.InnerException == null ? colError.Message : Convert.ToString(colError.InnerException) + Environment.NewLine);

                                }
                                rowCounter += 1;

                            }
                            dt.AcceptChanges();
                        }
                        else
                        {
                            strErrMsg.Append("Import not found");
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
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }

        #endregion

        #region Import GL
        [ValidateRequest(true, UserInterfaceHelper.ImportGL, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        [HttpGet]
        public ActionResult ImportGL()
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
        [ValidateRequest(true, UserInterfaceHelper.ImportGL, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        [ActionName("ImportGL")]
        [HttpPost]
        public ActionResult ImportGLData()
        {
            UploadUtilityBL utilityBL = new UploadUtilityBL();
            GLBL glBL = new GLBL();
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                if (Request.Files["FileUpload1"].ContentLength > 0)
                {
                    string extension = System.IO.Path.GetExtension(Request.Files["FileUpload1"].FileName).ToLower();
                    string query = null;
                    string connString = "";
                    string[] validFileTypes = { ".xls", ".xlsx", ".csv" };

                    string path1 = string.Format("{0}/{1}", Server.MapPath("~/Content/Uploads"), Request.Files["FileUpload1"].FileName);
                    if (!Directory.Exists(path1))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Content/Uploads"));
                    }
                    if (validFileTypes.Contains(extension))
                    {
                        DataTable dt = new DataTable();
                        if (System.IO.File.Exists(path1))
                        { System.IO.File.Delete(path1); }
                        Request.Files["FileUpload1"].SaveAs(path1);
                        if (extension == ".csv")
                        {
                            dt = CommonHelper.ConvertCSVtoDataTable(path1);
                            ViewBag.Data = dt;
                        }
                        //Connection String to Excel Workbook  
                        else if (extension.Trim() == ".xls")
                        {
                            connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path1 + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
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
                            Int32 GLId = 0;
                            int glSubGroupId = 0;
                            int sLTypeId = 0;
                            GLViewModel glViewModel;
                            GLDetailViewModel gLDetailViewModel = new GLDetailViewModel();
                            dt.Columns.Add("UploadStatus", typeof(bool));
                            bool rowVerifyStatus = true;
                            foreach (DataRow dr in dt.Rows)
                            {
                                glViewModel = new GLViewModel();

                                //code to validate data in excel//
                                if (string.IsNullOrEmpty(Convert.ToString(dr["GLCode"])))
                                {
                                    strErrMsg.Append("GLCode Name Column has not proper data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                    rowVerifyStatus = false;
                                }
                                else if (string.IsNullOrEmpty(Convert.ToString(dr["GLHead"])))
                                {
                                    strErrMsg.Append("GLHead Name Column has not proper data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                    rowVerifyStatus = false;
                                }
                                else if (string.IsNullOrEmpty(Convert.ToString(dr["GLType"])))
                                {
                                    strErrMsg.Append("GLType Name Column has not proper data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                    rowVerifyStatus = false;
                                }
                                else if (!string.IsNullOrEmpty(Convert.ToString(dr["GLSubGroupName"])))
                                {
                                    glSubGroupId = utilityBL.GetIdByGLSubGroupName(Convert.ToString(dr["GLSubGroupName"]));
                                    if (glSubGroupId == 0)
                                    {
                                        strErrMsg.Append("Invalid GLSubGroupName data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }
                                }
                                else if (!string.IsNullOrEmpty(Convert.ToString(dr["SLTypeName"])))
                                {
                                    sLTypeId = utilityBL.GetIdBySLTypeName(Convert.ToString(dr["SLTypeName"]));
                                    if (sLTypeId == 0)
                                    {
                                        strErrMsg.Append("Invalid SLTypeName data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }
                                }


                                else if (string.IsNullOrEmpty(Convert.ToString(dr["GLHead"])))
                                {
                                    strErrMsg.Append("GLHead Column has not proper data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                    rowVerifyStatus = false;
                                }
                                else if (string.IsNullOrEmpty(Convert.ToString(dr["OpeningBalanceDebit"])))
                                {
                                    strErrMsg.Append("OpeningBalanceDebit Column has not proper data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                    rowVerifyStatus = false;
                                }
                                else if (string.IsNullOrEmpty(Convert.ToString(dr["OpeningBalanceCredit"])))
                                {
                                    strErrMsg.Append("OpeningBalanceCredit Column has not proper data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                    rowVerifyStatus = false;
                                }
                                else if (string.IsNullOrEmpty(Convert.ToString(dr["OpeningBalance"])))
                                {
                                    strErrMsg.Append("OpeningBalance Column has not proper data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                    rowVerifyStatus = false;
                                }

                                //end of code to validate data in excel//

                                //code to get Id from Name data in excel//


                                if (rowVerifyStatus == true)
                                {
                                    //End of code to get Id from Name data in excel//
                                    glViewModel.GLCode = Convert.ToString(dr["GLCode"]);
                                    glViewModel.GLHead = Convert.ToString(dr["GLHead"]);
                                    glViewModel.GLType = Convert.ToString(dr["GLType"]);
                                    glViewModel.GLSubGroupId = glSubGroupId;
                                    glViewModel.SLTypeId = sLTypeId;
                                    glViewModel.CompanyId = ContextUser.CompanyId;
                                    glViewModel.CreatedBy = ContextUser.UserId;
                                    glViewModel.IsBookGL = Convert.ToBoolean(dr["IsBookGL"]);
                                    glViewModel.IsBranchGL = Convert.ToBoolean(dr["IsBranchGL"]);
                                    glViewModel.IsDebtorGL = Convert.ToBoolean(dr["IsDebtorGL"]);
                                    glViewModel.IsCreditorGL = Convert.ToBoolean(dr["IsCreditorGL"]);
                                    glViewModel.IsTaxGL = Convert.ToBoolean(dr["IsTaxGL"]);
                                    glViewModel.IsPostGL = Convert.ToBoolean(dr["IsPostGL"]);
                                    glViewModel.GLStatus = true;

                                    gLDetailViewModel.GLId = GLId;
                                    gLDetailViewModel.OpeningBalanceDebit = Convert.ToDecimal(dr["OpeningBalanceDebit"]);
                                    gLDetailViewModel.OpeningBalanceCredit = Convert.ToDecimal(dr["OpeningBalanceCredit"]);
                                    gLDetailViewModel.OpeningBalanceCredit = Convert.ToDecimal(dr["OpeningBalance"]);
                                    gLDetailViewModel.CompanyId = ContextUser.CompanyId;
                                    gLDetailViewModel.FinYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                                    gLDetailViewModel.CreatedBy = ContextUser.UserId;

                                    ResponseOut responseOut = glBL.ImportGL(glViewModel, gLDetailViewModel);
                                    if (responseOut.status == ActionStatus.Fail)
                                    {
                                        strErrMsg.Append(responseOut.message + ": Error in Inserting in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        dr["UploadStatus"] = false;
                                    }
                                    else
                                    {
                                        dr["UploadStatus"] = true;
                                    }
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
                            strErrMsg.Append("Import not found");
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
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }

        #endregion 

        #region Import GLDetail
        [ValidateRequest(true, UserInterfaceHelper.ImportGLDetail, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        [HttpGet]
        public ActionResult ImportGLDetail()
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
        [ValidateRequest(true, UserInterfaceHelper.ImportGLDetail, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        [ActionName("ImportGLDetail")]
        [HttpPost]
        public ActionResult ImportGLDetailData()
        {
            UploadUtilityBL utilityBL = new UploadUtilityBL();
            GLBL glBL = new GLBL();
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                if (Request.Files["FileUpload1"].ContentLength > 0)
                {
                    string extension = System.IO.Path.GetExtension(Request.Files["FileUpload1"].FileName).ToLower();
                    string query = null;
                    string connString = "";
                    string[] validFileTypes = { ".xls", ".xlsx", ".csv" };

                    string path1 = string.Format("{0}/{1}", Server.MapPath("~/Content/Uploads"), Request.Files["FileUpload1"].FileName);
                    if (!Directory.Exists(path1))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Content/Uploads"));
                    }
                    if (validFileTypes.Contains(extension))
                    {
                        DataTable dt = new DataTable();
                        if (System.IO.File.Exists(path1))
                        { System.IO.File.Delete(path1); }
                        Request.Files["FileUpload1"].SaveAs(path1);
                        if (extension == ".csv")
                        {
                            dt = CommonHelper.ConvertCSVtoDataTable(path1);
                            ViewBag.Data = dt;
                        }
                        //Connection String to Excel Workbook  
                        else if (extension.Trim() == ".xls")
                        {
                            connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path1 + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
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
                            int GLId = 0;

                            GLDetailViewModel gLDetailViewModel;
                            dt.Columns.Add("UploadStatus", typeof(bool));
                            bool rowVerifyStatus = true;

                            foreach (DataRow dr in dt.Rows)
                            {
                                gLDetailViewModel = new GLDetailViewModel();

                                //code to validate data in excel//

                                if (!string.IsNullOrEmpty(Convert.ToString(dr["GLName"])))
                                {
                                    GLId = utilityBL.GetIdByGLName(Convert.ToString(dr["GLName"]));
                                    if (GLId == 0)
                                    {
                                        strErrMsg.Append("Invalid GL Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }
                                }

                                if (string.IsNullOrEmpty(Convert.ToString(dr["OpeningBalanceDebit"])))
                                {
                                    strErrMsg.Append("OpeningBalanceDebit Column has not proper data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                    rowVerifyStatus = false;
                                }
                                //else if (string.IsNullOrEmpty(Convert.ToString(dr["OpeningBalanceCredit"])))
                                //{
                                //    strErrMsg.Append("OpeningBalanceCredit Column has not proper data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                //    rowVerifyStatus = false;
                                //}
                                else if (string.IsNullOrEmpty(Convert.ToString(dr["OpeningBalanceCredit"])))
                                {
                                    strErrMsg.Append("OpeningBalanceCredit Column has not proper data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                    rowVerifyStatus = false;
                                }

                                //end of code to validate data in excel//

                                //code to get Id from Name data in excel//
                                //if (!string.IsNullOrEmpty(Convert.ToString(dr["CountryName"])))
                                //{
                                //    GLId = utilityBL.GetIdByGLName(Convert.ToString(dr["CountryName"]));
                                //    if (GLId == 0)
                                //    {
                                //        strErrMsg.Append("Invalid Country Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                //        rowVerifyStatus = false;
                                //    }
                                //}

                                if (rowVerifyStatus == true)
                                {
                                    //End of code to get Id from Name data in excel//
                                    gLDetailViewModel.GLId = GLId;
                                    gLDetailViewModel.OpeningBalanceDebit = Convert.ToDecimal(dr["OpeningBalanceDebit"]);
                                    gLDetailViewModel.OpeningBalanceCredit = Convert.ToDecimal(dr["OpeningBalanceCredit"]);
                                    gLDetailViewModel.OpeningBalance = ((Convert.ToDecimal(dr["OpeningBalanceDebit"])) - (Convert.ToDecimal(dr["OpeningBalanceCredit"])));

                                    gLDetailViewModel.CompanyId = ContextUser.CompanyId;
                                    gLDetailViewModel.FinYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                                    gLDetailViewModel.CreatedBy = ContextUser.UserId;
                                    gLDetailViewModel.GLDetailStatus = true;

                                    ResponseOut responseOut = glBL.ImportGLDetail(gLDetailViewModel);
                                    if (responseOut.status == ActionStatus.Fail)
                                    {
                                        strErrMsg.Append(responseOut.message + ": Error in Inserting in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        dr["UploadStatus"] = false;
                                    }
                                    else
                                    {
                                        dr["UploadStatus"] = true;
                                    }
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
                            strErrMsg.Append("Import not found");
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
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }

        #endregion

        #region Import SLDetail
        [ValidateRequest(true, UserInterfaceHelper.ImportSLDetail, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        [HttpGet]
        public ActionResult ImportSLDetail()
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
        [ValidateRequest(true, UserInterfaceHelper.ImportSLDetail, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        [ActionName("ImportSLDetail")]
        [HttpPost]
        public ActionResult ImportSLDetailData()
        {
            UploadUtilityBL utilityBL = new UploadUtilityBL();
            SLDetailBL sLDetailBL = new SLDetailBL();
            
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                if (Request.Files["FileUpload1"].ContentLength > 0)
                {
                    string extension = System.IO.Path.GetExtension(Request.Files["FileUpload1"].FileName).ToLower();
                    string connString = "";
                    string[] validFileTypes = { ".xls", ".xlsx", ".csv" };

                    string path1 = string.Format("{0}/{1}", Server.MapPath("~/Content/Uploads"), Request.Files["FileUpload1"].FileName);
                    if (!Directory.Exists(path1))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Content/Uploads"));
                    }
                    if (validFileTypes.Contains(extension))
                    {
                        DataTable dt = new DataTable();
                        if (System.IO.File.Exists(path1))
                        { System.IO.File.Delete(path1); }
                        Request.Files["FileUpload1"].SaveAs(path1);
                        if (extension == ".csv")
                        {
                            dt = CommonHelper.ConvertCSVtoDataTable(path1);
                            ViewBag.Data = dt;
                        }
                        //Connection String to Excel Workbook  
                        else if (extension.Trim() == ".xls")
                        {
                            connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path1 + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
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
                            int GLId = 0;
                            long sLId = 0;
                            long CompanyBranchId = 0;
                            int CompanyId = 0;
                            int FinyearId = 0;
                            int CreatedBy = 0;
                           SLDetail sLDetailViewModel;
                            dt.Columns.Add("UploadStatus", typeof(bool));
                            bool rowVerifyStatus = true;
                            foreach (DataRow dr in dt.Rows)
                            {
                                sLDetailViewModel = new SLDetail();

                                //code to validate data in excel//

                                if (string.IsNullOrEmpty(Convert.ToString(dr["OpeningBalanceDebit"])))
                                {
                                    dr["OpeningBalanceDebit"] = 0;
                                    rowVerifyStatus = true;
                                }
                                else if (string.IsNullOrEmpty(Convert.ToString(dr["OpeningBalanceCredit"])))
                                {
                                    dr["OpeningBalanceCredit"] = 0;
                                    rowVerifyStatus = true;
                                }

                                //end of code to validate data in excel//

                                //code to get Id from Name data in excel//
                                if (!string.IsNullOrEmpty(Convert.ToString(dr["GLHead"])))
                                {
                                    GLId = utilityBL.GetIdByGLName(Convert.ToString(dr["GLHead"]));
                                    if (GLId == 0)
                                    {
                                        strErrMsg.Append("Invalid Country Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }
                                }
                                if (!string.IsNullOrEmpty(Convert.ToString(dr["SLHead"])))
                                {
                                    sLId = utilityBL.GetIdBySLHead(Convert.ToString(dr["SLHead"]));
                                    if (sLId == 0)
                                    {
                                        strErrMsg.Append("Invalid SLHead data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }
                                }


                                if (!string.IsNullOrEmpty(Convert.ToString(dr["CompanyBranchName"])))
                                {
                                    CompanyBranchId = utilityBL.GetIdByCompanyBranchName(Convert.ToString(dr["CompanyBranchName"]));
                                    if (CompanyBranchId == 0)
                                    {
                                        strErrMsg.Append("Invalid Company Branch Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }
                                }

                                if (rowVerifyStatus == true)
                                {
                                    //End of code to get Id from Name data in excel//
                                    List<SLDetailViewModel> sl = new List<SLDetailViewModel>();
                                    sl.Add(new SLDetailViewModel
                                    {
                                        GLId = GLId,
                                        SLId = sLId,
                                        CompanyBranchId = CompanyBranchId,
                                        OpeningBalanceDebit = Convert.ToDecimal(dr["OpeningBalanceDebit"]),
                                        OpeningBalanceCredit = Convert.ToDecimal(dr["OpeningBalanceCredit"]),
                                        SLDetailStatus = true,
                                       
                                    });
                                    CompanyId = ContextUser.CompanyId;
                                    FinyearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                                    CreatedBy = ContextUser.UserId;
                                    

                                    ResponseOut responseOut = sLDetailBL.AddEditSLDetail(sl, CompanyId, FinyearId, CreatedBy);
                                    if (responseOut.status == ActionStatus.Fail)
                                    {
                                        strErrMsg.Append(responseOut.message + ": Error in Inserting in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        dr["UploadStatus"] = false;
                                    }
                                    else
                                    {
                                        dr["UploadStatus"] = true;
                                    }
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
                            strErrMsg.Append("Import not found");
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
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }

        #endregion

        #region Helper Method

        #endregion


        #region Import Size
        [ValidateRequest(true, UserInterfaceHelper.ImportSize, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        [HttpGet]
        public ActionResult ImportSize()
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
        [ValidateRequest(true, UserInterfaceHelper.ImportSize, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        [ActionName("ImportSize")]
        [HttpPost]
        public ActionResult ImportSizeData()
        {
            UploadUtilityBL utilityBL = new UploadUtilityBL();
            SizeBL sizeBL = new SizeBL();
            try
            {
                if (Request.Files["FileUpload1"].ContentLength > 0)
                {
                    string extension = System.IO.Path.GetExtension(Request.Files["FileUpload1"].FileName).ToLower();
                    string connString = "";
                    string[] validFileTypes = { ".xls", ".xlsx", ".csv" };

                    string path1 = string.Format("{0}/{1}", Server.MapPath("~/Content/Uploads"), Request.Files["FileUpload1"].FileName);
                    if (!Directory.Exists(path1))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Content/Uploads"));
                    }
                    if (validFileTypes.Contains(extension))
                    {
                        DataTable dt = new DataTable();
                        if (System.IO.File.Exists(path1))
                        { System.IO.File.Delete(path1); }
                        Request.Files["FileUpload1"].SaveAs(path1);
                        if (extension == ".csv")
                        {
                            dt = CommonHelper.ConvertCSVtoDataTable(path1);
                            ViewBag.Data = dt;
                        }
                        //Connection String to Excel Workbook  
                        else if (extension.Trim() == ".xls")
                        {
                            connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path1 + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
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
                            int productMainGroupID = 0;
                            int productSubGroupID = 0;

                            SizeViewModel sizeViewModel;
                            dt.Columns.Add("UploadStatus", typeof(bool));
                            bool rowVerifyStatus = true;
                            Random rnd = new Random(50000);

                            foreach (DataRow dr in dt.Rows)
                            {
                                try
                                {
                                    sizeViewModel = new SizeViewModel();
                                    //code to validate data in excel// 

                                    if (string.IsNullOrEmpty(Convert.ToString(dr["SizeDesc"]).Trim()))
                                    {
                                        strErrMsg.Append("Please Enter Size Desc Column data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }

                                    if (string.IsNullOrEmpty(Convert.ToString(dr["SizeCode"]).Trim()))
                                    {
                                        strErrMsg.Append("Please Enter Size Code data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }

                                    if (!string.IsNullOrEmpty(Convert.ToString(dr["ProductMainGroupName"])))
                                    {
                                        productMainGroupID = utilityBL.GetIdByProductMainGroupName(Convert.ToString(dr["ProductMainGroupName"]));
                                        if (productMainGroupID == 0)
                                        {
                                            strErrMsg.Append("Invalid Product MainGroup Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                            rowVerifyStatus = false;
                                        }
                                    }
                                    else
                                    {
                                        strErrMsg.Append("Please Enter MainGroup Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }

                                    if (!string.IsNullOrEmpty(Convert.ToString(dr["ProductSubGroupName"])))
                                    {
                                        productSubGroupID = utilityBL.GetIdByProductSubGroupName(Convert.ToString(dr["ProductSubGroupName"]));
                                        if (productSubGroupID == 0)
                                        {
                                            strErrMsg.Append("Invalid Product SubGroup Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                            rowVerifyStatus = false;
                                        }
                                    }
                                    else
                                    {
                                        strErrMsg.Append("Please Enter SubGroup Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }


                                    if (rowVerifyStatus == true)
                                    {
                                        //End of code to get Id from Name data in excel//

                                        sizeViewModel.SizeDesc = Convert.ToString(dr["SizeDesc"]);
                                        sizeViewModel.SizeCode = Convert.ToString(dr["SizeCode"]);
                                        sizeViewModel.ProductMainGroupId = productMainGroupID;
                                        sizeViewModel.ProductSubGroupId = productSubGroupID;
                                        sizeViewModel.Size_Status = true;

                                        ResponseOut responseOut = sizeBL.ImportSize(sizeViewModel);
                                        if (responseOut.status == ActionStatus.Fail)
                                        {
                                            strErrMsg.Append(responseOut.message + ": Error in Inserting in Row #" + rowCounter.ToString() + Environment.NewLine);
                                            dr["UploadStatus"] = false;
                                        }
                                        else
                                        {
                                            dr["UploadStatus"] = true;
                                        }
                                    }
                                    else
                                    {
                                        dr["UploadStatus"] = false;
                                    }


                                }
                                catch (Exception colError)
                                {
                                    dr["UploadStatus"] = false;
                                    strErrMsg.Append(colError.InnerException == null ? colError.Message : Convert.ToString(colError.InnerException) + Environment.NewLine);

                                }
                                rowCounter += 1;
                            }


                            dt.AcceptChanges();
                        }
                        else
                        {
                            strErrMsg.Append("Import not found");
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
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }

        #endregion

        #region Import Manufacturer   
        [ValidateRequest(true, UserInterfaceHelper.ImportManufacturer, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        [HttpGet]
        public ActionResult ImportManufacturer()
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
        [ValidateRequest(true, UserInterfaceHelper.ImportManufacturer, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        [ActionName("ImportManufacturer")]
        [HttpPost]
        public ActionResult ImportManufacturerData()
        {
            UploadUtilityBL utilityBL = new UploadUtilityBL();
            ManufacturerBL manufacturerBL = new ManufacturerBL();
            try
            {
                if (Request.Files["FileUpload1"].ContentLength > 0)
                {
                    string extension = System.IO.Path.GetExtension(Request.Files["FileUpload1"].FileName).ToLower();
                    string connString = "";
                    string[] validFileTypes = { ".xls", ".xlsx", ".csv" };

                    string path1 = string.Format("{0}/{1}", Server.MapPath("~/Content/Uploads"), Request.Files["FileUpload1"].FileName);
                    if (!Directory.Exists(path1))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Content/Uploads"));
                    }
                    if (validFileTypes.Contains(extension))
                    {
                        DataTable dt = new DataTable();
                        if (System.IO.File.Exists(path1))
                        { System.IO.File.Delete(path1); }
                        Request.Files["FileUpload1"].SaveAs(path1);
                        if (extension == ".csv")
                        {
                            dt = CommonHelper.ConvertCSVtoDataTable(path1);
                            ViewBag.Data = dt;
                        }
                        //Connection String to Excel Workbook  
                        else if (extension.Trim() == ".xls")
                        {
                            connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path1 + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
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

                            ManufacturerViewModel manufacturerViewModel;
                            dt.Columns.Add("UploadStatus", typeof(bool));
                            bool rowVerifyStatus = true;
                            Random rnd = new Random(50000);
                            foreach (DataRow dr in dt.Rows)
                            {
                                try
                                {
                                    manufacturerViewModel = new ManufacturerViewModel();
                                    //code to validate data in excel//

                                    if (string.IsNullOrEmpty(Convert.ToString(dr["ManufacturerName"]).Trim()))
                                    {
                                        strErrMsg.Append("Please Enter Manufacturer Name Column data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["ManufacturerCode"]).Trim()))
                                    {
                                        strErrMsg.Append("Please Enter Manufacturer Code Column data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }

                                    if (rowVerifyStatus == true)
                                    {
                                        //End of code to get Id from Name data in excel//

                                        manufacturerViewModel.ManufacturerCode = Convert.ToString(dr["ManufacturerCode"]);
                                        manufacturerViewModel.ManufacturerName = Convert.ToString(dr["ManufacturerName"]);
                                        manufacturerViewModel.CreatedBy = ContextUser.UserId;
                                        manufacturerViewModel.Manufacturer_Status = true;
                                        ResponseOut responseOut = manufacturerBL.ImportManufacturer(manufacturerViewModel);
                                        if (responseOut.status == ActionStatus.Fail)
                                        {
                                            strErrMsg.Append(responseOut.message + ": Error in Inserting in Row #" + rowCounter.ToString() + Environment.NewLine);
                                            dr["UploadStatus"] = false;
                                        }
                                        else
                                        {
                                            dr["UploadStatus"] = true;
                                        }
                                    }
                                    else
                                    {
                                        dr["UploadStatus"] = false;
                                    }
                                }
                                catch (Exception colError)
                                {
                                    dr["UploadStatus"] = false;
                                    strErrMsg.Append(colError.InnerException == null ? colError.Message : Convert.ToString(colError.InnerException) + Environment.NewLine);

                                }
                                rowCounter += 1;

                            }
                            dt.AcceptChanges();
                        }
                        else
                        {
                            strErrMsg.Append("Import not found");
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
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }

        #endregion

        #region Import Chessis Serial Mapping
        [ValidateRequest(true, UserInterfaceHelper.ImportChessisSerialMapping, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        [HttpGet]
        public ActionResult ImportChessisSerialMapping()
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
        [ValidateRequest(true, UserInterfaceHelper.ImportChessisSerialMapping, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        [ActionName("ImportChessisSerialMapping")]
        [HttpPost]
        public ActionResult ImportChessisSerialMappingData()
        {
            UploadUtilityBL utilityBL = new UploadUtilityBL();
            ChasisSerialMappingBL chasisSerialMappingBLBL = new ChasisSerialMappingBL();
            try
            {
                if (Request.Files["FileUpload1"].ContentLength > 0)
                {
                    string extension = System.IO.Path.GetExtension(Request.Files["FileUpload1"].FileName).ToLower();
                    string connString = "";
                    string[] validFileTypes = { ".xls", ".xlsx", ".csv" };

                    string path1 = string.Format("{0}/{1}", Server.MapPath("~/Content/Uploads"), Request.Files["FileUpload1"].FileName);
                    if (!Directory.Exists(path1))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Content/Uploads"));
                    }
                    if (validFileTypes.Contains(extension))
                    {
                        DataTable dt = new DataTable();
                        if (System.IO.File.Exists(path1))
                        { System.IO.File.Delete(path1); }
                        Request.Files["FileUpload1"].SaveAs(path1);
                        if (extension == ".csv")
                        {
                            dt = CommonHelper.ConvertCSVtoDataTable(path1);
                            ViewBag.Data = dt;
                        }
                        //Connection String to Excel Workbook  
                        else if (extension.Trim() == ".xls")
                        {
                            connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path1 + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
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
                            long productId = 0;
                            int rowCounter = 1;
                            ChasisSerialMappingViewModel chasisSerialMappingViewModel;
                            dt.Columns.Add("UploadStatus", typeof(bool));
                            bool rowVerifyStatus = true;
                            Random rnd = new Random(50000);
                            foreach (DataRow dr in dt.Rows)
                            {
                                try
                                {
                                    chasisSerialMappingViewModel = new ChasisSerialMappingViewModel();
                                    //code to validate data in excel//
                                    bool frontGlassAvailable;
                                    bool viperAvailable;
                                    bool rearShockerAvailable;
                                    bool chargerAvailable;
                                    bool fmAvailable;
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["ProductName"]).Trim()))
                                    {
                                        strErrMsg.Append("Please Enter Product Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["ChessisSerialNo"]).Trim()))
                                    {
                                        strErrMsg.Append("Please Enter Chessis Serial No data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }

                                    if (string.IsNullOrEmpty(Convert.ToString(dr["MotorNo"]).Trim()))
                                    {
                                        strErrMsg.Append("Please Enter Motor No data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }

                                    if (string.IsNullOrEmpty(Convert.ToString(dr["ControllerNo"]).Trim()))
                                    {
                                        dr["ControllerNo"] = "";
                                    }

                                    if (string.IsNullOrEmpty(Convert.ToString(dr["Color"]).Trim()))
                                    {
                                        dr["Color"] = "";
                                    }

                                    if (string.IsNullOrEmpty(Convert.ToString(dr["BatteryPower"]).Trim()))
                                    {
                                        dr["BatteryPower"] = "";
                                    }
                                   
                                    if (!string.IsNullOrEmpty(Convert.ToString(dr["ProductName"])))
                                    {
                                        productId = utilityBL.GetIdByProductNameForChessis(Convert.ToString(dr["ProductName"]));
                                        if (productId == 0)
                                        {
                                            strErrMsg.Append("Invalid Product Name data in Row #" + rowCounter.ToString() + ",Product should be Main Assembly" + Environment.NewLine);
                                            rowVerifyStatus = false;
                                        }
                                    }
                                    else
                                    {
                                        strErrMsg.Append("Please Enter Product Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }

                                    if (string.IsNullOrEmpty(Convert.ToString(dr["BatterySerialNo1"]).Trim()))
                                    {
                                        dr["BatterySerialNo1"] = "";
                                    }

                                    if (string.IsNullOrEmpty(Convert.ToString(dr["BatterySerialNo2"]).Trim()))
                                    {
                                        dr["BatterySerialNo2"] = "";
                                    }

                                    if (string.IsNullOrEmpty(Convert.ToString(dr["BatterySerialNo3"]).Trim()))
                                    {
                                        dr["BatterySerialNo3"] = "";
                                    }

                                    if (string.IsNullOrEmpty(Convert.ToString(dr["BatterySerialNo4"]).Trim()))
                                    {
                                        dr["BatterySerialNo4"] = "";
                                    }

                                    if (string.IsNullOrEmpty(Convert.ToString(dr["Tyre"]).Trim()))
                                    {
                                        dr["Tyre"] = "";
                                    }
                                                
                                    //end of code to validate data in excel//

                                    //code to get Id from Name data in excel//
                                    
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["FrontGlassAvailable"])))
                                    {
                                        dr["FrontGlassAvailable"] = false;
                                    }
                                    else if (bool.TryParse(Convert.ToString(dr["FrontGlassAvailable"]), out frontGlassAvailable) == false)
                                    {
                                        strErrMsg.Append("FrontGlassAvailable Column has not proper data in Rows #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }

                                    if (string.IsNullOrEmpty(Convert.ToString(dr["ViperAvailable"])))
                                    {
                                        dr["ViperAvailable"] = false;
                                    }
                                    else if (bool.TryParse(Convert.ToString(dr["ViperAvailable"]), out viperAvailable) == false)
                                    {
                                        strErrMsg.Append("ViperAvailable Column has not proper data in Rows #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }

                                    if (string.IsNullOrEmpty(Convert.ToString(dr["RearShockerAvailable"])))
                                    {
                                        dr["RearShockerAvailable"] = false;
                                    }
                                    else if (bool.TryParse(Convert.ToString(dr["RearShockerAvailable"]), out rearShockerAvailable) == false)
                                    {
                                        strErrMsg.Append("RearShockerAvailable Column has not proper data in Rows #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }

                                    if (string.IsNullOrEmpty(Convert.ToString(dr["ChargerAvailable"])))
                                    {
                                        dr["ChargerAvailable"] = false;
                                    }
                                    else if (bool.TryParse(Convert.ToString(dr["ChargerAvailable"]), out chargerAvailable) == false)
                                    {
                                        strErrMsg.Append("ChargerAvailable Column has not proper data in Rows #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }

                                    if (string.IsNullOrEmpty(Convert.ToString(dr["FMAvailable"])))
                                    {
                                        dr["FMAvailable"] = false;
                                    }
                                    else if (bool.TryParse(Convert.ToString(dr["FMAvailable"]), out fmAvailable) == false)
                                    {
                                        strErrMsg.Append("FMAvailable Column has not proper data in Rows #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }

                                    if (rowVerifyStatus == true)
                                    {
                                        //End of code to get Id from Name data in excel// 

                                        chasisSerialMappingViewModel.ProductId = productId;
                                        chasisSerialMappingViewModel.ChasisSerialNo = Convert.ToString(dr["ChessisSerialNo"]).Trim();
                                        chasisSerialMappingViewModel.MotorNo = Convert.ToString(dr["MotorNo"]).Trim();
                                        chasisSerialMappingViewModel.ControllerNo = Convert.ToString(dr["ControllerNo"]).Trim();
                                        chasisSerialMappingViewModel.Color = Convert.ToString(dr["Color"]);
                                        chasisSerialMappingViewModel.BatteryPower = Convert.ToString(dr["BatteryPower"]);
                                        chasisSerialMappingViewModel.BatterySerialNo1 = Convert.ToString(dr["BatterySerialNo1"]);
                                        chasisSerialMappingViewModel.BatterySerialNo2 = Convert.ToString(dr["BatterySerialNo2"]);
                                        chasisSerialMappingViewModel.BatterySerialNo3 = Convert.ToString(dr["BatterySerialNo3"]);
                                        chasisSerialMappingViewModel.BatterySerialNo4 = Convert.ToString(dr["BatterySerialNo4"]);
                                        chasisSerialMappingViewModel.Tier = Convert.ToString(dr["Tyre"]);
                                        chasisSerialMappingViewModel.FrontGlassAvailable = Convert.ToBoolean(dr["FrontGlassAvailable"]);
                                        chasisSerialMappingViewModel.ViperAvailable = Convert.ToBoolean(dr["ViperAvailable"]);
                                        chasisSerialMappingViewModel.RearShockerAvailable = Convert.ToBoolean(dr["RearShockerAvailable"]);
                                        chasisSerialMappingViewModel.ChargerAvailable = Convert.ToBoolean(dr["ChargerAvailable"]);
                                        chasisSerialMappingViewModel.FmAvailable = Convert.ToBoolean(dr["FMAvailable"]);
                                        chasisSerialMappingViewModel.ChasisSerialMapping_Status = true;
                                        chasisSerialMappingViewModel.CreatedBy = ContextUser.UserId;
                                        ResponseOut responseOut = chasisSerialMappingBLBL.ImportChessisSerialMappingForUploadUtility(chasisSerialMappingViewModel);
                                        if (responseOut.status == ActionStatus.Fail)
                                        {
                                            strErrMsg.Append(responseOut.message + ": Error in Inserting in Row #" + rowCounter.ToString() + Environment.NewLine);
                                            dr["UploadStatus"] = false;
                                        }
                                        else
                                        {
                                            dr["UploadStatus"] = true;
                                        }
                                    }
                                    else
                                    {
                                        dr["UploadStatus"] = false;
                                    }

                                }
                                catch (Exception colError)
                                {
                                    dr["UploadStatus"] = false;
                                    strErrMsg.Append(colError.InnerException == null ? colError.Message : Convert.ToString(colError.InnerException) + Environment.NewLine);

                                }
                                rowCounter += 1;

                            }
                            dt.AcceptChanges();
                        }
                        else
                        {
                            strErrMsg.Append("Import not found");
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
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }
        #endregion

        

        #region Employee Pay Info
        [ValidateRequest(true, UserInterfaceHelper.ImportEmployeePayInfo, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        [HttpGet]
        public ActionResult ImportEmployeePayInfo()
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
        [ValidateRequest(true, UserInterfaceHelper.ImportEmployeePayInfo, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        [ActionName("ImportEmployeePayInfo")]
        [HttpPost]
        public ActionResult ImportEmployeePayInfoData()
        {
            UploadUtilityBL utilityBL = new UploadUtilityBL();
            EmployeeBL employeeBL = new EmployeeBL();
            
            try
            {
                if (Request.Files["FileUpload1"].ContentLength > 0)
                {
                    string extension = System.IO.Path.GetExtension(Request.Files["FileUpload1"].FileName).ToLower();
                    string connString = "";
                    string[] validFileTypes = { ".xls", ".xlsx", ".csv" };

                    string path1 = string.Format("{0}/{1}", Server.MapPath("~/Content/Uploads"), Request.Files["FileUpload1"].FileName);
                    if (!Directory.Exists(path1))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Content/Uploads"));
                    }
                    if (validFileTypes.Contains(extension))
                    {
                        DataTable dt = new DataTable();
                        if (System.IO.File.Exists(path1))
                        { System.IO.File.Delete(path1); }
                        Request.Files["FileUpload1"].SaveAs(path1);
                        if (extension == ".csv")
                        {
                            dt = CommonHelper.ConvertCSVtoDataTable(path1);
                            ViewBag.Data = dt;
                        }
                        //Connection String to Excel Workbook  
                        else if (extension.Trim() == ".xls")
                        {
                            connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path1 + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
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
                           int EmployeeId = 0;
                           int rowCounter = 1;
                            EmployeePayInfoViewModel employeePayInfoViewModel;
                            dt.Columns.Add("UploadStatus", typeof(bool));
                            bool rowVerifyStatus = true;
                            Random rnd = new Random(50000);
                            foreach (DataRow dr in dt.Rows)
                            {
                                try
                                {
                                    employeePayInfoViewModel = new EmployeePayInfoViewModel();
                                    //code to validate data in excel//
                                   

                                    //if (string.IsNullOrEmpty(Convert.ToString(dr["Basic Pay"]).Trim()))
                                    //{
                                    //    strErrMsg.Append("Please Enter Basic Pay data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                    //    rowVerifyStatus = false;
                                    //}
                                    //if (string.IsNullOrEmpty(Convert.ToString(dr["ChessisSerialNo"]).Trim()))
                                    //{
                                    //    strErrMsg.Append("Please Enter Chessis Serial No data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                    //    rowVerifyStatus = false;
                                    //}

                                    //if (string.IsNullOrEmpty(Convert.ToString(dr["MotorNo"]).Trim()))
                                    //{
                                    //    strErrMsg.Append("Please Enter Motor No data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                    //    rowVerifyStatus = false;
                                    //}

                                    //if (string.IsNullOrEmpty(Convert.ToString(dr["ControllerNo"]).Trim()))
                                    //{
                                    //    dr["ControllerNo"] = "";
                                    //}

                                    //if (string.IsNullOrEmpty(Convert.ToString(dr["Color"]).Trim()))
                                    //{
                                    //    dr["Color"] = "";
                                    //}

                                    //if (string.IsNullOrEmpty(Convert.ToString(dr["BatteryPower"]).Trim()))
                                    //{
                                    //    dr["BatteryPower"] = "";
                                    //}

                                   

                                    //if (string.IsNullOrEmpty(Convert.ToString(dr["BatterySerialNo1"]).Trim()))
                                    //{
                                    //    dr["BatterySerialNo1"] = "";
                                    //}

                                    //if (string.IsNullOrEmpty(Convert.ToString(dr["BatterySerialNo2"]).Trim()))
                                    //{
                                    //    dr["BatterySerialNo2"] = "";
                                    //}

                                    //if (string.IsNullOrEmpty(Convert.ToString(dr["BatterySerialNo3"]).Trim()))
                                    //{
                                    //    dr["BatterySerialNo3"] = "";
                                    //}

                                    //if (string.IsNullOrEmpty(Convert.ToString(dr["BatterySerialNo4"]).Trim()))
                                    //{
                                    //    dr["BatterySerialNo4"] = "";
                                    //}

                                    //if (string.IsNullOrEmpty(Convert.ToString(dr["Tyre"]).Trim()))
                                    //{
                                    //    dr["Tyre"] = "";
                                    //}

                                    ////end of code to validate data in excel//

                                    ////code to get Id from Name data in excel//

                                    //if (string.IsNullOrEmpty(Convert.ToString(dr["FrontGlassAvailable"])))
                                    //{
                                    //    dr["FrontGlassAvailable"] = false;
                                    //}
                                    //else if (bool.TryParse(Convert.ToString(dr["FrontGlassAvailable"]), out frontGlassAvailable) == false)
                                    //{
                                    //    strErrMsg.Append("FrontGlassAvailable Column has not proper data in Rows #" + rowCounter.ToString() + Environment.NewLine);
                                    //    rowVerifyStatus = false;
                                    //}

                                    //if (string.IsNullOrEmpty(Convert.ToString(dr["ViperAvailable"])))
                                    //{
                                    //    dr["ViperAvailable"] = false;
                                    //}
                                    //else if (bool.TryParse(Convert.ToString(dr["ViperAvailable"]), out viperAvailable) == false)
                                    //{
                                    //    strErrMsg.Append("ViperAvailable Column has not proper data in Rows #" + rowCounter.ToString() + Environment.NewLine);
                                    //    rowVerifyStatus = false;
                                    //}

                                    //if (string.IsNullOrEmpty(Convert.ToString(dr["RearShockerAvailable"])))
                                    //{
                                    //    dr["RearShockerAvailable"] = false;
                                    //}
                                    //else if (bool.TryParse(Convert.ToString(dr["RearShockerAvailable"]), out rearShockerAvailable) == false)
                                    //{
                                    //    strErrMsg.Append("RearShockerAvailable Column has not proper data in Rows #" + rowCounter.ToString() + Environment.NewLine);
                                    //    rowVerifyStatus = false;
                                    //}

                                    //if (string.IsNullOrEmpty(Convert.ToString(dr["ChargerAvailable"])))
                                    //{
                                    //    dr["ChargerAvailable"] = false;
                                    //}
                                    //else if (bool.TryParse(Convert.ToString(dr["ChargerAvailable"]), out chargerAvailable) == false)
                                    //{
                                    //    strErrMsg.Append("ChargerAvailable Column has not proper data in Rows #" + rowCounter.ToString() + Environment.NewLine);
                                    //    rowVerifyStatus = false;
                                    //}

                                    //if (string.IsNullOrEmpty(Convert.ToString(dr["FMAvailable"])))
                                    //{
                                    //    dr["FMAvailable"] = false;
                                    //}
                                    //else if (bool.TryParse(Convert.ToString(dr["FMAvailable"]), out fmAvailable) == false)
                                    //{
                                    //    strErrMsg.Append("FMAvailable Column has not proper data in Rows #" + rowCounter.ToString() + Environment.NewLine);
                                    //    rowVerifyStatus = false;
                                    //}

                                    if (rowVerifyStatus == true)
                                    {
                                        //End of code to get Id from Name data in excel// 

                                        employeePayInfoViewModel.EmployeeId = EmployeeId;
                                        employeePayInfoViewModel.OTRate = Convert.ToDecimal(dr["OTRate"]);
                                        employeePayInfoViewModel.BasicPay = Convert.ToDecimal(dr["BasicPay"]);
                                        employeePayInfoViewModel.HRA = Convert.ToDecimal(dr["HRA"]);
                                        employeePayInfoViewModel.ConveyanceAllow = Convert.ToDecimal(dr["ConveyanceAllow"]);
                                        employeePayInfoViewModel.SpecialAllow = Convert.ToDecimal(dr["SpecialAllow"]);
                                        employeePayInfoViewModel.OtherAllow = Convert.ToDecimal(dr["OtherAllow"]);
                                        employeePayInfoViewModel.OtherDeduction = Convert.ToDecimal(dr["OtherDeduction"]);
                                        employeePayInfoViewModel.MedicalAllow = Convert.ToDecimal(dr["MedicalAllow"]);
                                        employeePayInfoViewModel.ChildEduAllow = Convert.ToDecimal(dr["ChildEduAllow"]);
                                        employeePayInfoViewModel.EmployeePF = Convert.ToDecimal(dr["EmployeePF"]);
                                        employeePayInfoViewModel.EmployeeESI = Convert.ToDecimal(dr["EmployeeESI"]);
                                        employeePayInfoViewModel.EmployerPF = Convert.ToDecimal(dr["EmployerPF"]);
                                        employeePayInfoViewModel.EmployerESI = Convert.ToDecimal(dr["EmployerESI"]);
                                        employeePayInfoViewModel.LTA = Convert.ToDecimal(dr["LTA"]);
                                        employeePayInfoViewModel.ProfessinalTax = Convert.ToDecimal(dr["ProfessinalTax"]);


                                        ResponseOut responseOut = employeeBL.AddEditEmployeePayInfoForUploadUtility(employeePayInfoViewModel);
                                        if (responseOut.status == ActionStatus.Fail)
                                        {
                                            strErrMsg.Append(responseOut.message + ": Error in Inserting in Row #" + rowCounter.ToString() + Environment.NewLine);
                                            dr["UploadStatus"] = false;
                                        }
                                        else
                                        {
                                            dr["UploadStatus"] = true;
                                        }
                                    }
                                    else
                                    {
                                        dr["UploadStatus"] = false;
                                    }

                                }
                                catch (Exception colError)
                                {
                                    dr["UploadStatus"] = false;
                                    strErrMsg.Append(colError.InnerException == null ? colError.Message : Convert.ToString(colError.InnerException) + Environment.NewLine);

                                }
                                rowCounter += 1;

                            }
                            dt.AcceptChanges();
                        }
                        else
                        {
                            strErrMsg.Append("Import not found");
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
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }
        #endregion

    }
}
