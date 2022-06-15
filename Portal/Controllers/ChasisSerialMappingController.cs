using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Portal.Core;
using Portal.Core.ViewModel;
using Portal.Common;
using System.Reflection;
using System.IO;
using System.Data;
using System.Text;

namespace Portal.Controllers
{
    [CheckSessionBeforeControllerExecuteAttribute(Order = 1)]
    public class ChasisSerialMappingController : BaseController
    {
        #region  ChasisSerialMapping
        //
        // GET: /ChasisSerialMapping/

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ChasisSerialMapping, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditChasisSerialMapping(int chasisSerialMappingId = 0, int accessMode = 3)
        {

            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                if (chasisSerialMappingId != 0)
                {
                    ViewData["chasisSerialMappingId"] = chasisSerialMappingId;
                    ViewData["accessMode"] = accessMode;
                    
                }
                else
                {
                    ViewData["chasisSerialMappingId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ChasisSerialMapping, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditChasisSerialMapping(ChasisSerialMappingViewModel chasisSerialMappingViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            ChasisSerialMappingBL chasisSerialMappingBL = new ChasisSerialMappingBL();
            try
            {
                if (chasisSerialMappingViewModel != null)
                {
                    chasisSerialMappingViewModel.CreatedBy = ContextUser.UserId;
                    responseOut = chasisSerialMappingBL.AddEditChasisSerialMapping(chasisSerialMappingViewModel);

                    ChasisSerialSingleton.ResetChasisSerialSingleton();
                    ChasisSerialSingleton pro = ChasisSerialSingleton.Instance();
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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ChasisSerialMapping, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListChasisSerial()
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
        public PartialViewResult GetChasisSerialList(int productId, string ChasisSerialNo, string MotorNo, string ControllerNo,int companyBranchId=0,string serialStatus="")
        {
            List<ChasisSerialMappingViewModel> chasisSerialMappings = new List<ChasisSerialMappingViewModel>();
            
            ChasisSerialMappingBL chasisSerialMappingBL = new ChasisSerialMappingBL();
            try
            {
                chasisSerialMappings = chasisSerialMappingBL.GetChasisSerialList(productId, ChasisSerialNo,MotorNo,ControllerNo, companyBranchId, serialStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(chasisSerialMappings);
        }


        [HttpGet]
        public JsonResult GetChasisSerialAutoCompleteList(string term,int companyBranchId)
        {
           

            ChasisSerialPlanBL chasisSerialPlanBL = new ChasisSerialPlanBL();
            List<ChasisSerialMappingViewModel> chasisSerialList = new List<ChasisSerialMappingViewModel>();
            try
            {
                ChasisSerialSingleton.ResetChasisSerialSingleton();
                ChasisSerialSingleton pro = ChasisSerialSingleton.Instance();
                //chasisSerialList = chasisSerialPlanBL.GetChasisSerialNoAutoCompleteList(term);
                chasisSerialList = ChasisSerialSingleton.GetChasisSerials(term, companyBranchId);
                    
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(chasisSerialList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetChasisSerialMappingDetail(long mappingId)
        {
            ChasisSerialMappingBL chasisSerialMappingBL = new ChasisSerialMappingBL();
            ChasisSerialMappingViewModel chasisSerialMapping = new ChasisSerialMappingViewModel();
            try
            {
                chasisSerialMapping = chasisSerialMappingBL.GetChasisSerialMappingDetail(mappingId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(chasisSerialMapping, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Import Chessis Serial Mapping
        [ValidateRequest(true, UserInterfaceHelper.Product_ImportChessisSerialMapping, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
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

        [ValidateRequest(true, UserInterfaceHelper.Product_ImportChessisSerialMapping, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
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
                                    //bool frontGlassAvailable;
                                    //bool viperAvailable;
                                    //bool rearShockerAvailable;
                                    //bool chargerAvailable;
                                    //bool fmAvailable;
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["Product Name"]).Trim()))
                                    {
                                        strErrMsg.Append("Please Enter Product Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["Chassis Serial No"]).Trim()))
                                    {
                                        strErrMsg.Append("Please Enter Chessis Serial No data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }

                                    if (string.IsNullOrEmpty(Convert.ToString(dr["Motor No"]).Trim()))
                                    {
                                        strErrMsg.Append("Please Enter Motor No data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }

                                    if (string.IsNullOrEmpty(Convert.ToString(dr["Controller No"]).Trim()))
                                    {
                                        dr["Controller No"] = "";
                                    }

                                    //if (string.IsNullOrEmpty(Convert.ToString(dr["Battery Serial No"]).Trim()))
                                    //{
                                    //    dr["Color"] = "";
                                    //}

                                    //if (string.IsNullOrEmpty(Convert.ToString(dr["BatteryPower"]).Trim()))
                                    //{
                                    //    dr["BatteryPower"] = "";
                                    //}

                                    if (!string.IsNullOrEmpty(Convert.ToString(dr["Product Name"])))
                                    {
                                        productId = utilityBL.GetIdByProductNameForChessis(Convert.ToString(dr["Product Name"]));
                                        if (productId == 0)
                                        {
                                            strErrMsg.Append("Invalid Product Name data in Row #" + rowCounter.ToString() + ",Product should be Main Assembly and should be Serialized." + Environment.NewLine);
                                            rowVerifyStatus = false;
                                        }
                                    }
                                    else
                                    {
                                        strErrMsg.Append("Please Enter Product Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }

                                    

                                    if (string.IsNullOrEmpty(Convert.ToString(dr["Battery Serial No"]).Trim()))
                                    {
                                        dr["Battery Serial No"] = "";
                                    }

                                    if (string.IsNullOrEmpty(Convert.ToString(dr["Charger No"]).Trim()))
                                    {
                                        dr["Charger No"] = "";
                                    }

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

                                    //end of code to validate data in excel//

                                    //code to get Id from Name data in excel//

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

                                        chasisSerialMappingViewModel.ProductId = productId;
                                        chasisSerialMappingViewModel.ChasisSerialNo = Convert.ToString(dr["Chassis Serial No"]).Trim();
                                        chasisSerialMappingViewModel.MotorNo = Convert.ToString(dr["Motor No"]).Trim();
                                        chasisSerialMappingViewModel.ControllerNo = Convert.ToString(dr["Controller No"]).Trim();
                                        chasisSerialMappingViewModel.Color = string.Empty;
                                        chasisSerialMappingViewModel.BatteryPower = string.Empty;
                                        chasisSerialMappingViewModel.BatterySerialNo1 = Convert.ToString(dr["Battery Serial No"]);
                                        chasisSerialMappingViewModel.BatterySerialNo2 = Convert.ToString(dr["Charger No"]);
                                        chasisSerialMappingViewModel.BatterySerialNo3 =string.Empty;
                                        chasisSerialMappingViewModel.BatterySerialNo4 = string.Empty;
                                        chasisSerialMappingViewModel.Tier = string.Empty;
                                        chasisSerialMappingViewModel.FrontGlassAvailable = false;
                                        chasisSerialMappingViewModel.ViperAvailable = false;
                                        chasisSerialMappingViewModel.RearShockerAvailable = false;
                                        chasisSerialMappingViewModel.ChargerAvailable =false;
                                        chasisSerialMappingViewModel.FmAvailable =false;
                                        chasisSerialMappingViewModel.ChasisSerialMapping_Status = true;
                                        chasisSerialMappingViewModel.CreatedBy = ContextUser.UserId;
                                        chasisSerialMappingViewModel.CompanyBranchId =Convert.ToInt32( ContextUser.CompanyBranchId);
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
    }
}
