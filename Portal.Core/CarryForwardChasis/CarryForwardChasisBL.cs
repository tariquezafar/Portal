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
    public class CarryForwardChasisBL
    {
        DBInterface dbInterface;
        public CarryForwardChasisBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditCarryForwardChasis(CarryForwardChasissViewModel carryForwardChasissViewModel, List<CarryForwardChasisDetailViewModel> carryForwardChasisDetailViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                CarryForwardChasi carryForwardChasis = new CarryForwardChasi
                {
                    CarryForwardID = carryForwardChasissViewModel.CarryForwardID,
                    CarryForwardDate = Convert.ToDateTime(carryForwardChasissViewModel.CarryForwardDate),
                    CompanyId=carryForwardChasissViewModel.CompanyId,
                    PrevChasisMonth = carryForwardChasissViewModel.PrevChasisMonth,
                    PrevChasisYear = carryForwardChasissViewModel.PrevChasisYear,
                    CarryForwardMonth = carryForwardChasissViewModel.CarryForwardMonth,
                    CarryForwardYear = carryForwardChasissViewModel.CarryForwardYear,                               
                    CreatedBy = carryForwardChasissViewModel.CreatedBy,              
                    CompanyBranchId= carryForwardChasissViewModel.CompanyBranchId ,
                    ApprovalStatus= carryForwardChasissViewModel.ApprovalStatus
                                       
                };
                List<CarryForwardChasisDetail> CarryForwardChasisDetailList = new List<CarryForwardChasisDetail>();
                if (carryForwardChasisDetailViewModel != null && carryForwardChasisDetailViewModel.Count > 0)
                {
                    foreach (CarryForwardChasisDetailViewModel item in carryForwardChasisDetailViewModel)
                    {
                        CarryForwardChasisDetailList.Add(new CarryForwardChasisDetail
                        {     
                             ChasisModelID=item.ChasisModelID,                                                                                                                       
                             ChasisSerialNo=item.ChasisSerialNo,
                             ChasisSerialDetailID=item.ChasisSerialDetailID,
                             MotorNo=item.MotorNo ,                                                  
                        });
                    }
                }                
                responseOut = sqlDbInterface.AddEditCarryForwardChasis(carryForwardChasis, CarryForwardChasisDetailList);
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
      
        public CarryForwardChasissViewModel GetCarryForwardChasisDetail(long carryForwardID = 0)
        {
            CarryForwardChasissViewModel carryForwardChasissViewModel = new CarryForwardChasissViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCompanies = sqlDbInterface.GetCarryForwardChasisDetail(carryForwardID);
                if (dtCompanies != null && dtCompanies.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCompanies.Rows)
                    {
                        carryForwardChasissViewModel = new CarryForwardChasissViewModel
                        {
                            CarryForwardID = Convert.ToInt32(dr["CarryForwardID"]),
                            CarryForwardNo = Convert.ToString(dr["CarryForwardNo"]),
                            CarryForwardDate = Convert.ToString(dr["CarryForwardDate"]),
                            PrevChasisYear = Convert.ToInt32(dr["PrevChasisYear"]),
                            CarryForwardYear = Convert.ToInt32(dr["CarryForwardYear"]),
                            ChasisMonth = Convert.ToString(dr["PrevChasisMonth"]),
                            CarryMonth = Convert.ToString(dr["CarryForwardMonth"]),
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
            return carryForwardChasissViewModel;
        }
        public List<CarryForwardChasissViewModel> GetCarryForwardChasisList(string carryForwardNo, int companyBranchId, int prevChasisMonth, int prevChasisYear, int carryForwardMonth, int carryForwardYear,  DateTime fromDate, DateTime toDate, int companyId, string approvalStatus = "")
        {
            List<CarryForwardChasissViewModel> printChasisViewModel = new List<CarryForwardChasissViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtQuotations = sqlDbInterface.GetCarryForwardChasisList(carryForwardNo, companyBranchId, prevChasisMonth, prevChasisYear, carryForwardMonth, carryForwardYear, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, approvalStatus);
                if (dtQuotations != null && dtQuotations.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtQuotations.Rows)
                    {
                        printChasisViewModel.Add(new CarryForwardChasissViewModel
                        {
                            CarryForwardID = Convert.ToInt32(dr["CarryForwardID"]),
                            CarryForwardNo = Convert.ToString(dr["CarryForwardNo"]),
                            CarryForwardDate = Convert.ToString(dr["CarryForwardDate"]),
                            PrevChasisYear= Convert.ToInt32(dr["PrevChasisYear"]),
                            CarryForwardYear = Convert.ToInt32(dr["CarryForwardYear"]),
                            ChasisMonth = Convert.ToString(dr["PrevChasisMonth"]),
                            CarryMonth = Convert.ToString(dr["CarryForwardMonth"]),
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
     
        public List<CarryForwardChasisDetailViewModel> GetCarryForwardChasisProductsList(int prevYear=0,int prevMonth=0)
        {
            List<CarryForwardChasisDetailViewModel> carryForwardDetailsproduct = new List<CarryForwardChasisDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCustomers = sqlDbInterface.GetCarryForwardChasisProductsList(prevYear, prevMonth);
                if (dtCustomers != null && dtCustomers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCustomers.Rows)
                    {
                        carryForwardDetailsproduct.Add(new CarryForwardChasisDetailViewModel
                        {
                            SequenceNo = Convert.ToInt32(dr["SNo"]),
                            ChasisSerialDetailID = Convert.ToInt32(dr["ChasisSerialDetailID"]),
                            ChasisModelID = Convert.ToInt32(dr["ChasisModelID"]),
                            ChasisSerialNo = Convert.ToString(dr["ChasisSerialNo"]),
                            MotorNo = Convert.ToString(dr["MotorNo"]) ,
                            CarryForwardMonth=Convert.ToInt32(dr["CarryForwardMonth"]),
                            CarryForwardYear = Convert.ToInt32(dr["CarryForwardYear"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return carryForwardDetailsproduct;
        }

        public List<CarryForwardChasisDetailViewModel> GetCarryForwardChasisProducts(long carryForwardID)
        {
            List<CarryForwardChasisDetailViewModel> CarryForwardChasisDetails = new List<CarryForwardChasisDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCustomers = sqlDbInterface.GetCarryForwardChasisProducts(carryForwardID);
                if (dtCustomers != null && dtCustomers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCustomers.Rows)
                    {
                        CarryForwardChasisDetails.Add(new CarryForwardChasisDetailViewModel
                        {
                            SequenceNo = Convert.ToInt32(dr["SNo"]),
                            ChasisSerialDetailID = Convert.ToInt32(dr["ChasisSerialDetailID"]),
                            ChasisModelID = Convert.ToInt32(dr["ChasisModelID"]),
                            ChasisSerialNo = Convert.ToString(dr["ChasisSerialNo"]),
                            MotorNo = Convert.ToString(dr["MotorNo"]),
                            CarryMonth= Convert.ToString(dr["CarryMonth"]),
                            CarryForwardMonth=Convert.ToInt32(dr["CarryForwardMonth"]),
                            CarryForwardYear = Convert.ToInt32(dr["CarryForwardYear"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return CarryForwardChasisDetails;
        }

        public DataTable GetPrintCarryForwardChasisPrint(long carryForwardID = 0)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtPrintChasis = new DataTable();
            try
            {
                dtPrintChasis = sqlDbInterface.GetCarryForwardChasisDetail(carryForwardID);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtPrintChasis;
        }
        public DataTable GetPrintCarryForwardProductsPrint(long carryForwardID = 0)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtPrintChasisProducts = new DataTable();
            try
            {
                dtPrintChasisProducts = sqlDbInterface.GetCarryForwardChasisProducts(carryForwardID);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtPrintChasisProducts;
        }
    }
}
