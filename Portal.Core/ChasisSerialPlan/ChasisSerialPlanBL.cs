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
    public class ChasisSerialPlanBL
    {
        DBInterface dbInterface;
        public ChasisSerialPlanBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditChasisSerialPlan(ChasisSerialPlanViewModel chasisSerialPlanViewModel, List<ChasisSerialPlanDetailViewModel> chasisSerialPlanDetails,List<ChasisSerialModelDetailViewModel> chasisSerialModelDetails)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                ChasisSerialPlan chasisSerialPlan = new ChasisSerialPlan
                {
                    ChasisSerialPlanID = chasisSerialPlanViewModel.ChasisSerialPlanID,
                    ChasisSerialPlanDate = Convert.ToDateTime(chasisSerialPlanViewModel.ChasisSerialPlanDate),
                    ChasisMonth =chasisSerialPlanViewModel.ChasisMonth,
                    ChasisYear = chasisSerialPlanViewModel.ChasisYear,
                    CompanyId = chasisSerialPlanViewModel.CompanyId,
                    LastIncreamentNo = chasisSerialPlanViewModel.LastIncreamentNo,                 
                    CreatedBy = chasisSerialPlanViewModel.CreatedBy,
                    Consumed = chasisSerialPlanViewModel.Consumed,
                    CompanyBranchId= chasisSerialPlanViewModel.CompanyBranchId,
                    ApprovalStatus = chasisSerialPlanViewModel.ApprovalStatus
                };
                List<ChasisSerialPlanDetail> chasisSerialPlanDetailList = new List<ChasisSerialPlanDetail>();
                if (chasisSerialPlanDetails != null && chasisSerialPlanDetails.Count > 0)
                {
                    foreach (ChasisSerialPlanDetailViewModel item in chasisSerialPlanDetails)
                    {
                        chasisSerialPlanDetailList.Add(new ChasisSerialPlanDetail
                        {      
                             ChasisModelID=item.ChasisModelID,                                                                 
                             ChasisSerialNo=item.ChasisSerialNo,
                             MotorNo=item.MotorNo ,
                             IncrementalNo=Convert.ToInt64(item.IncrementalNo)                         
                        });
                    }
                }

                List<ChasisSerialModelDetail> chasisSerialModelDetailList = new List<ChasisSerialModelDetail>();
                if (chasisSerialModelDetails != null && chasisSerialModelDetails.Count > 0)
                {
                    foreach (ChasisSerialModelDetailViewModel item in chasisSerialModelDetails)
                    {
                       // if (item.QtyProduced != 0)
                        //{
                            chasisSerialModelDetailList.Add(new ChasisSerialModelDetail
                            {
                                ChasisModelID = item.ChasisModelID,
                                QtyProduced = item.QtyProduced,
                                LastIncreamentNo=item.LastIncreamentNo

                            });
                      //  }
                    }
                }

                responseOut = sqlDbInterface.AddEditChasisSerialPlan(chasisSerialPlan, chasisSerialPlanDetailList,chasisSerialModelDetailList);




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
        public List<ChasisSerialPlanProductViewModel> GetChasisModelProductList(long chasisSerialPlanID,string status,int year,int month)
        {
            List<ChasisSerialPlanProductViewModel> chasisSerialPlanProductViewModel = new List<ChasisSerialPlanProductViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtQuotations = sqlDbInterface.GetChasisModelProductList(chasisSerialPlanID, status, year, month);
                if (dtQuotations != null && dtQuotations.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtQuotations.Rows)
                    {
                        chasisSerialPlanProductViewModel.Add(new ChasisSerialPlanProductViewModel
                        {                            
                            ChasisModelID = Convert.ToInt32(dr["ChasisModelID"]),
                            ChasisModelName = Convert.ToString(dr["ChasisModelName"]),
                            ProductSubGroupName=Convert.ToString(dr["ProductSubGroupName"]),
                            ChasisModelCode = Convert.ToString(dr["ChasisModelCode"]),
                            MotorModelCode = Convert.ToString(dr["MotorModelCode"]),
                            LastIncreamentNo=Convert.ToInt32(dr["LastIncreamentNo"]),
                            QtyProduced=Convert.ToInt32(dr["QtyProduced"]),
                            CarryForwardQTY=Convert.ToInt32(dr["CarryForwardQTY"]),
                            CarryForwardTrueORNot= Convert.ToString(dr["CarryForwardTrueORNot"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return chasisSerialPlanProductViewModel;
        }

        public ChasisSerialPlanViewModel GetChasisSerialPlanDetail(long chasisSerialPlanID = 0)
        {         
            ChasisSerialPlanViewModel chasisSerialPlanViewModel = new ChasisSerialPlanViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCompanies = sqlDbInterface.GetChasisSerialPlanDetail(chasisSerialPlanID);
                if (dtCompanies != null && dtCompanies.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCompanies.Rows)
                    {
                        chasisSerialPlanViewModel = new ChasisSerialPlanViewModel
                        {
                            ChasisSerialPlanID = Convert.ToInt32(dr["ChasisSerialPlanID"]),
                            ChasisSerialPlanNo = Convert.ToString(dr["ChasisSerialPlanNo"]),                          
                            ChasisSerialPlanDate = Convert.ToString(dr["ChasisSerialPlanDate"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                            Month=Convert.ToString(dr["ChasisMonth"]),
                            ChasisYear = Convert.ToInt32(dr["ChasisYear"]),
                            ApprovalStatus = Convert.ToString(dr["ApprovalStatus"]),                       
                            LastIncreamentNo = Convert.ToInt32(dr["LastIncreamentNo"]),
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
            return chasisSerialPlanViewModel;
        }

        public List<ChasisSerialPlanDetailViewModel> GetChasisSerialPlanProductList(long chasisSerialPlanID)
        {
            List<ChasisSerialPlanDetailViewModel> ChasisSerialPlanDetails = new List<ChasisSerialPlanDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCustomers = sqlDbInterface.GetChasisSerialPlanProductList(chasisSerialPlanID);
                if (dtCustomers != null && dtCustomers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCustomers.Rows)
                    {
                        ChasisSerialPlanDetails.Add(new ChasisSerialPlanDetailViewModel
                        {
                            ChasisSerialDetailID = Convert.ToInt32(dr["ChasisSerialDetailID"]),
                            ChasisModelID = Convert.ToInt32(dr["ChasisModelID"]),                         
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
            return ChasisSerialPlanDetails;
        }


        public DataTable GetChasisSerialNoTrackingReport(string chasisSerialPlanNo)
        {
           
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtChasisSerialNos = new DataTable();
            try
            {
              dtChasisSerialNos = sqlDbInterface.GetChasisSerialNoTrackingReport(chasisSerialPlanNo);
               
              
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtChasisSerialNos;
        }





        public List<ChasisSerialPlanViewModel> GetChasisSerialPlanList(string chasisSerialPlanNo, int chasisYear, int chasisMonth, string fromDate, string toDate, int companyId, string approvalStatus = "",int companyBranchId=0)
        {
            List<ChasisSerialPlanViewModel> chasisSerialPlanViewModel = new List<ChasisSerialPlanViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtQuotations = sqlDbInterface.GetChasisSerialPlanList(chasisSerialPlanNo, chasisYear, chasisMonth, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, approvalStatus, companyBranchId);
                if (dtQuotations != null && dtQuotations.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtQuotations.Rows)
                    {
                        chasisSerialPlanViewModel.Add(new ChasisSerialPlanViewModel
                        {
                            ChasisSerialPlanID = Convert.ToInt32(dr["ChasisSerialPlanID"]),
                            ChasisSerialPlanNo = Convert.ToString(dr["ChasisSerialPlanNo"]),                        
                            ChasisSerialPlanDate = Convert.ToString(dr["ChasisSerialPlanDate"]),
                            ChasisYear = Convert.ToInt32(dr["ChasisYear"]),
                            Month = Convert.ToString(dr["ChasisMonth"]),
                            ApprovalStatus = Convert.ToString(dr["ApprovalStatus"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            CompanyBranchId= Convert.ToInt32(dr["CompanyBranchId"]),
                            CompanyBranchName = Convert.ToString(dr["CompanyBranchName"])
                        });
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

        public DataTable GetChasisNoDetailedList(string saleInvoiceNo, string chasisNo, string partyName, string fromDate, string toDate, int companyId, int companyBranchId)
        {
            DataTable dtChasisNoDetailed;
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                dtChasisNoDetailed = sqlDbInterface.GetChasisNoDetailedList(saleInvoiceNo,chasisNo,partyName, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate),companyId, companyBranchId);
               
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtChasisNoDetailed;
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
        public DataTable GetChasisSerialPlanDetailPrint(long chasisSerialPlanID)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtChasisNo = new DataTable();
            try
            {
                dtChasisNo = sqlDbInterface.GetChasisSerialPlanDetailPrint(chasisSerialPlanID);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtChasisNo;
        }

        public DataTable GetChasisSerialPlanDetailProducts(long chasisSerialPlanID)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtChasisNo = new DataTable();
            try
            {
                dtChasisNo = sqlDbInterface.GetChasisSerialPlanDetailProducts(chasisSerialPlanID);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtChasisNo;
        }

        public DataTable GetChasisSerialPlanMOdelDetailProducts(long chasisSerialPlanID)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtChasisNo = new DataTable();
            try
            {
                dtChasisNo = sqlDbInterface.GetChasisSerialPlanMOdelDetailProducts(chasisSerialPlanID);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtChasisNo;
        }

        public string GetManufactorLocationCode(long companyBranchId)
        {
            string manufactorLocationCode = "";
            SQLDbInterface sqldbinterface = new SQLDbInterface();
            try
            {
                manufactorLocationCode = sqldbinterface.GetManufactorLocationCode(companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return manufactorLocationCode;
        }

        public List<ChasisSerialMappingViewModel> GetChasisSerialNoAutoCompleteList(string searchTerm)
        {
            List<ChasisSerialMappingViewModel> chasis = new List<ChasisSerialMappingViewModel>();
            try
            {
                List<ChasisSerialMapping> chasisSerialMappingList = dbInterface.GetChasisSerialNoAutoCompleteList(searchTerm);
                if (chasisSerialMappingList != null && chasisSerialMappingList.Count > 0)
                {
                    foreach (ChasisSerialMapping chasisSerialMapping in chasisSerialMappingList)
                    {
                        chasis.Add(new ChasisSerialMappingViewModel
                        {
                            ProductId=Convert.ToInt64(chasisSerialMapping.ProductId),
                            ChasisSerialNo=chasisSerialMapping.ChasisSerialNo,
                            MotorNo= chasisSerialMapping.MotorNo,
                            ControllerNo= chasisSerialMapping.ControllerNo
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return chasis;
        }

        public List<ChasisSerialMappingViewModel> GetChasisSerialNoList()
        {
            List<ChasisSerialMappingViewModel> chasis = new List<ChasisSerialMappingViewModel>();
            try
            {
                List<ChasisSerialMapping> chasisSerialMappingList = dbInterface.GetChasisSerialNoList();
                if (chasisSerialMappingList != null && chasisSerialMappingList.Count > 0)
                {
                    foreach (ChasisSerialMapping chasisSerialMapping in chasisSerialMappingList)
                    {
                        chasis.Add(new ChasisSerialMappingViewModel
                        {
                            ProductId = Convert.ToInt64(chasisSerialMapping.ProductId),
                            ChasisSerialNo = chasisSerialMapping.ChasisSerialNo,
                            MotorNo = chasisSerialMapping.MotorNo,
                            ControllerNo = chasisSerialMapping.ControllerNo,
                            CompanyBranchId = Convert.ToInt32(chasisSerialMapping.CompanyBranchId)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return chasis;
        }
    }
}
