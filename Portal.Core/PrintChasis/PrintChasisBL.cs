using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Core.ViewModel;
using Portal.DAL;
using Portal.Common;
using System.Reflection;
using System.Data;
using System.Transactions;
namespace Portal.Core
{
    public class PrintChasisBL
    {
        DBInterface dbInterface;
        public PrintChasisBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditPrintChasis(PrintChasisViewModel printChasisViewModel, List<PrintChasisDetailViewModel> printChasisDetailViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                PrintChasi printChasis = new PrintChasi
                {
                    PrintID = printChasisViewModel.PrintID,
                    PrintDate = Convert.ToDateTime(printChasisViewModel.PrintDate),                  
                    CompanyId = printChasisViewModel.CompanyId,                               
                    CreatedBy = printChasisViewModel.CreatedBy,              
                    CompanyBranchId= printChasisViewModel.CompanyBranchId ,
                    ApprovalStatus=printChasisViewModel.ApprovalStatus
                                       
                };
                List<PrintChasisDetail> printChasisDetailList = new List<PrintChasisDetail>();
                if (printChasisDetailViewModel != null && printChasisDetailViewModel.Count > 0)
                {
                    foreach (PrintChasisDetailViewModel item in printChasisDetailViewModel)
                    {
                        printChasisDetailList.Add(new PrintChasisDetail
                        {                                                                                                                            
                             ChasisSerialNo=item.ChasisSerialNo,
                             MotorNo=item.MotorNo 
                             //FabricatedFlag = Convert.ToBoolean(item.PrintChasisFlag)                         
                        });
                    }
                }                
                responseOut = sqlDbInterface.AddEditPrintChasis(printChasis, printChasisDetailList);
            }

            catch (Exception ex)
            {
                responseOut.status = ActionStatus.Fail;
                responseOut.message = ActionMessage.ApplicationException;
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }

            return responseOut;
        }
      
        public PrintChasisViewModel GetPrintChasisDetail(long printId = 0)
        {
            PrintChasisViewModel printChasisViewModel = new PrintChasisViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCompanies = sqlDbInterface.GetPrintChasisDetail(printId);
                if (dtCompanies != null && dtCompanies.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCompanies.Rows)
                    {
                        printChasisViewModel = new PrintChasisViewModel
                        {
                            PrintID = Convert.ToInt32(dr["PrintID"]),
                            PrintNo = Convert.ToString(dr["PrintNo"]),
                            PrintDate = Convert.ToString(dr["PrintDate"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),                                                    
                            ApprovalStatus = Convert.ToString(dr["ApprovalStatus"]),                                                
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return printChasisViewModel;
        }
        public List<PrintChasisViewModel> GetPrintChasisList(string printNo, int companyBranchId,  string fromDate, string toDate, int companyId, string approvalStatus = "")
        {
            List<PrintChasisViewModel> printChasisViewModel = new List<PrintChasisViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtQuotations = sqlDbInterface.GetPrintChasisList(printNo, companyBranchId,  Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, approvalStatus);
                if (dtQuotations != null && dtQuotations.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtQuotations.Rows)
                    {
                        printChasisViewModel.Add(new PrintChasisViewModel
                        {
                            PrintID = Convert.ToInt32(dr["PrintID"]),
                            PrintNo = Convert.ToString(dr["PrintNo"]),
                            PrintDate = Convert.ToString(dr["PrintDate"]),
                            CompanyBranchName = Convert.ToString(dr["BranchName"]),                        
                            ApprovalStatus = Convert.ToString(dr["ApprovalStatus"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return printChasisViewModel;
        }
        public ChasisSerialPlanViewModel GetLastIncrement()
        {
            ChasisSerialPlanViewModel chasisSerialPlanViewModel = new ChasisSerialPlanViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCompanies = sqlDbInterface.GetLastIncrementNo();
                if (dtCompanies != null && dtCompanies.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCompanies.Rows)
                    {
                        chasisSerialPlanViewModel = new ChasisSerialPlanViewModel
                        {                           
                            Month = Convert.ToString(dr["ChasisMonth"]),
                            ChasisYear = Convert.ToInt32(dr["ChasisYear"]),                       
                            LastIncreamentNo = Convert.ToInt32(dr["LastIncreamentNo"]),                          
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return chasisSerialPlanViewModel;
        }
        public List<PrintChasisDetailViewModel> GetPrintChasisProductsList(long companyBranchId,int chasisMonth)
        {
            List<PrintChasisDetailViewModel> printChasisPlanDetails = new List<PrintChasisDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCustomers = sqlDbInterface.GetPrintChasisProductsList(companyBranchId, chasisMonth);
                if (dtCustomers != null && dtCustomers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCustomers.Rows)
                    {
                        printChasisPlanDetails.Add(new PrintChasisDetailViewModel
                        {
                            SequenceNo = Convert.ToInt32(dr["SNo"]),
                            ChasisSerialDetailID = Convert.ToInt32(dr["ChasisSerialDetailID"]),
                            ChasisSerialNo = Convert.ToString(dr["ChasisSerialNo"]),
                            MotorNo = Convert.ToString(dr["MotorNo"]),
                            ChasisModelCode= Convert.ToString(dr["ChasisModelCode"]),
                            MotorModelCode = Convert.ToString(dr["MotorModelCode"]),
                            ProductSubGroupName = Convert.ToString(dr["ProductSubGroupName"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return printChasisPlanDetails;
        }

        public List<PrintChasisDetailViewModel> GetPrintChasisProducts(long printID)
        {
            List<PrintChasisDetailViewModel> printChasisPlanDetails = new List<PrintChasisDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCustomers = sqlDbInterface.GetPrintChasisProducts(printID);
                if (dtCustomers != null && dtCustomers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCustomers.Rows)
                    {
                        printChasisPlanDetails.Add(new PrintChasisDetailViewModel
                        {
                            SequenceNo = Convert.ToInt32(dr["SNo"]),                         
                            ChasisSerialNo = Convert.ToString(dr["ChasisSerialNo"]),
                            MotorNo = Convert.ToString(dr["MotorNo"]),
                            ChasisModelCode = Convert.ToString(dr["ChasisModelCode"]),
                            MotorModelCode = Convert.ToString(dr["MotorModelCode"]),
                            ProductSubGroupName = Convert.ToString(dr["ProductSubGroupName"]),
                            PrintStatus=Convert.ToString(dr["PrintStatus"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return printChasisPlanDetails;
        }

        public DataTable GetPrintChasisDetailPrint(long printId=0)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtPrintChasis = new DataTable();
            try
            {
                dtPrintChasis = sqlDbInterface.GetPrintChasisDetail(printId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtPrintChasis;
        }
        public DataTable GetPrintChasisProductsPrint(long printId = 0)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtPrintChasisProducts = new DataTable();
            try
            {
                dtPrintChasisProducts = sqlDbInterface.GetPrintChasisProductPrint(printId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtPrintChasisProducts;
        }
        public List<PrintChasisDetailViewModel> GetPrintChasisProductsForFabrication()
        {
            List<PrintChasisDetailViewModel> printChasisPlanDetails = new List<PrintChasisDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtChasis = sqlDbInterface.GetPrintChasisProductsForFabrication();
                if (dtChasis != null && dtChasis.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtChasis.Rows)
                    {
                        printChasisPlanDetails.Add(new PrintChasisDetailViewModel
                        {
                            SequenceNo = Convert.ToInt32(dr["SNo"]),
                            PrintDetailID = Convert.ToInt32(dr["PrintDetailID"]),
                            ChasisSerialNo = Convert.ToString(dr["ChasisSerialNo"]),
                            MotorNo = Convert.ToString(dr["MotorNo"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return printChasisPlanDetails;
        }
    }
}
