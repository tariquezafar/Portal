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

namespace Portal.Core
{
   public class JobCardBL
    {
        DBInterface dbInterface;
        public JobCardBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditJobCard(JobCardViewModel jobCardViewModel,  List<JobCardProductDetailViewModel> jobCardProductDetailViewModel,List<JobCardDetailViewModel> jobCardDetailViewModelList)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                JobCard jobCard = new JobCard
                {
                    JobCardID = jobCardViewModel.JobCardID,
                    JobCardDate = Convert.ToDateTime(jobCardViewModel.JobCardDate),
                    ApprovalStatus= jobCardViewModel.ApprovalStatus,
                    CreatedBy = jobCardViewModel.CreatedBy,
                    CompanyId = jobCardViewModel.CompanyId,
                    TimeIn = jobCardViewModel.TimeIn,                 
                    DeliveryTime = jobCardViewModel.DeliveryTime,
                    CustomerID = jobCardViewModel.CustomerID,
                    ModelName = jobCardViewModel.ModelName,
                    RegNo = jobCardViewModel.RegNo,
                    DateOfSale = Convert.ToDateTime(jobCardViewModel.DateOfSale),
                    FrameNo = jobCardViewModel.FrameNo,
                    EngineNo = jobCardViewModel.EngineNo,
                    KMSCovered = jobCardViewModel.KMSCovered,
                    CouponNo = jobCardViewModel.CouponNo,
                    
                    FuelLevel = jobCardViewModel.FuelLevel,
                    EngineOilLevel = jobCardViewModel.EngineOilLevel,
                    KeyNo = jobCardViewModel.KeyNo,
                    BatteryMakeNo = jobCardViewModel.BatteryMakeNo,


                    Damage = jobCardViewModel.Damage,
                    Accessories = jobCardViewModel.Accessories,
                    TypeOfService = jobCardViewModel.TypeOfService,
                    EstimationDeliveryTime = jobCardViewModel.EstimationDeliveryTime,
                    
                    EstimationCostOfRepair = jobCardViewModel.EstimationCostOfRepair,
                    EstimationCostOfParts = jobCardViewModel.EstimationCostOfParts,
                    PreJobCardNo = jobCardViewModel.PreJobCardNo,
                    PreSeviceDate = string.IsNullOrEmpty(jobCardViewModel.PreSeviceDate) ? Convert.ToDateTime("01-01-1900") : Convert.ToDateTime(jobCardViewModel.PreSeviceDate),
                    //PreSeviceDate = Convert.ToDateTime(jobCardViewModel.PreSeviceDate),
                    PreKey = jobCardViewModel.PreKey,
                    MahenicName = jobCardViewModel.MahenicName,
                    StartTime = jobCardViewModel.StartTime,
                    ClosingTime = jobCardViewModel.ClosingTime, 
                    VehicleNo=jobCardViewModel.VehicleNo,
                    ChassisNo = jobCardViewModel.ChassisNo,
                };

                List<JobCardDetail> jobCardDetailItemList = new List<JobCardDetail>();

                if (jobCardProductDetailViewModel != null && jobCardProductDetailViewModel.Count > 0)
                {
                    foreach (JobCardProductDetailViewModel item in jobCardProductDetailViewModel)
                    {
                        jobCardDetailItemList.Add(new JobCardDetail
                        {
                            
                            ProductID = item.ProductID,
                            ServiceItemID = item.ServiceItemID,
                            ServiceItemName = item.ServiceItemName,
                            CustComplaintObservation = item.CustComplaintObservation,
                            SupervisorAdvice = item.SupervisorAdvice,
                            AmountEstimated = item.AmountEstimated,

                        });
                    }
                }

                List<JobCardProductDetail> jobCardProductDetailList = new List<JobCardProductDetail>();

                if (jobCardDetailViewModelList != null && jobCardDetailViewModelList.Count > 0)
                {
                    foreach (JobCardDetailViewModel item in jobCardDetailViewModelList)
                    {
                        jobCardProductDetailList.Add(new JobCardProductDetail
                        {

                            ProductId = item.ProductId,                           
                            Price = item.Price,
                            Quantity = item.Quantity,
                            DiscountPercentage = item.DiscountPercentage,
                            DiscountAmount = item.DiscountAmount,                            
                            CGST_Perc = item.CGST_Perc,
                            CGST_Amount = item.CGST_Amount,
                            SGST_Perc = item.SGST_Perc,
                            SGST_Amount = item.SGST_Amount,
                            TotalAmount = item.TotalPrice,

                        });
                    }
                }
                responseOut = sqlDbInterface.AddEditJobCard(jobCard, jobCardDetailItemList, jobCardProductDetailList);
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

       
        public JobCardViewModel GetJobCardDetail(long jobCardID)
        {
            JobCardViewModel  jobCardViewModel = new JobCardViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtServices = sqlDbInterface.GetJobCardDetail(jobCardID);
                if (dtServices != null && dtServices.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtServices.Rows)
                    {
                        jobCardViewModel = new JobCardViewModel
                        {                         
                            JobCardID = Convert.ToInt32(dr["JobCardID"]),
                            JobCardNo = Convert.ToString(dr["JobCardNo"]),
                            JobCardDate = Convert.ToString(dr["JobCardDate"]),
                            ApprovalStatus = Convert.ToString(dr["ApprovalStatus"]),
                           
                            TimeIn = Convert.ToString(dr["TimeIn"]),
                            TimeInMinute = Convert.ToString(dr["TimeInMinute"]),
                            DeliveryTime = Convert.ToString(dr["DeliveryTime"]),
                            DeliveryTimeMinute = Convert.ToString(dr["DeliveryTimeMinute"]),
                            CustomerID = Convert.ToInt32(dr["CustomerID"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            CustomerCode = Convert.ToString(dr["CustomerCode"]),
                            ModelName = Convert.ToString(dr["ModelName"]),
                            RegNo = Convert.ToString(dr["RegNo"]),
                            DateOfSale = Convert.ToString(dr["DateOfSale"]),
                            FrameNo = Convert.ToString(dr["FrameNo"]),
                            EngineNo = Convert.ToString(dr["EngineNo"]),
                            KMSCovered = Convert.ToString(dr["KMSCovered"]),
                            CouponNo = Convert.ToString(dr["CouponNo"]),
                            FuelLevel = Convert.ToString(dr["FuelLevel"]),
                            EngineOilLevel = Convert.ToString(dr["EngineOilLevel"]),
                            KeyNo = Convert.ToString(dr["KeyNo"]),
                            BatteryMakeNo = Convert.ToString(dr["BatteryMakeNo"]),
                            Damage = Convert.ToString(dr["Damage"]),
                            Accessories = Convert.ToString(dr["Accessories"]),
                            TypeOfService = Convert.ToString(dr["TypeOfService"]),
                            EstimationDeliveryTime = Convert.ToString(dr["EstimationDeliveryTime"]),
                            EstimationDeliveryTimeMinute = Convert.ToString(dr["EstimationDeliveryTimeMinute"]),

                            EstimationCostOfRepair = Convert.ToDecimal(dr["EstimationCostOfRepair"]),
                            EstimationCostOfParts = Convert.ToDecimal(dr["EstimationCostOfParts"]),
                            PreJobCardNo = Convert.ToString(dr["PreJobCardNo"]),

                            PreSeviceDate = Convert.ToString(dr["PreSeviceDate"]),
                            PreKey = Convert.ToString(dr["PreKey"]),
                            MahenicName = Convert.ToString(dr["MahenicName"]),
                            StartTime = Convert.ToString(dr["StartTime"]),
                            StartTimeMinute = Convert.ToString(dr["StartTimeMinute"]),
                            ClosingTime = Convert.ToString(dr["ClosingTime"]),
                            ClosingTimeMinute = Convert.ToString(dr["ClosingTimeMinute"]),
                            VehicleNo = Convert.ToString(dr["VehicleNo"]),
                            ChassisNo = Convert.ToString(dr["ChassisNo"]),
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return jobCardViewModel;
        }
        public List<JobCardProductDetailViewModel> GetJobCardProductList(long serviceId)
        {
            List<JobCardProductDetailViewModel> jobCardProductDetailViewModelList = new List<JobCardProductDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCustomers = sqlDbInterface.GetJobCardProductList(serviceId);
                if (dtCustomers != null && dtCustomers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCustomers.Rows)
                    {
                        jobCardProductDetailViewModelList.Add(new JobCardProductDetailViewModel
                        {
                            JobCardDetailID = Convert.ToInt32(dr["JobCardDetailID"]),
                            SequenceNo = Convert.ToInt32(dr["SequenceNo"]),
                            ProductID = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ServiceItemID = Convert.ToInt32(dr["ServiceItemID"]),
                            ServiceItemName = Convert.ToString(dr["ServiceItemName"]),
                            CustComplaintObservation = Convert.ToString(dr["CustComplaintObservation"]),
                            SupervisorAdvice = Convert.ToString(dr["SupervisorAdvice"]),
                            AmountEstimated = Convert.ToDecimal(dr["AmountEstimated"]),

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return jobCardProductDetailViewModelList;
        }

        public List<JobCardViewModel> GetJobCardList(string jobCardNo , string customerName , string approvalStatus , string modelName , string engineNo , string regNo , string keyNo , string fromDate , string toDate ,int companyId, int CustomerId=0)
        {
            List<JobCardViewModel> jobCardViewModelList = new List<JobCardViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtJobCard = sqlDbInterface.GetJobCardList(jobCardNo,  customerName,  approvalStatus,  modelName,  engineNo,  regNo,  keyNo, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId,CustomerId);
                if (dtJobCard != null && dtJobCard.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtJobCard.Rows)
                    {
                        jobCardViewModelList.Add(new JobCardViewModel
                        {
                            JobCardID = Convert.ToInt32(dr["JobCardID"]),
                            JobCardNo = Convert.ToString(dr["JobCardNo"]),
                            JobCardDate = Convert.ToString(dr["JobCardDate"]),
                            ApprovalStatus = Convert.ToString(dr["ApprovalStatus"]),
                            CustomerID = Convert.ToInt32(dr["CustomerID"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            CustomerCode = Convert.ToString(dr["CustomerCode"]),
                            ModelName = Convert.ToString(dr["ModelName"]),
                            RegNo = Convert.ToString(dr["RegNo"]),
                            EngineNo = Convert.ToString(dr["EngineNo"]),
                            KeyNo = Convert.ToString(dr["KeyNo"]),
                            TimeIn = Convert.ToString(dr["TimeIn"]),
                            DeliveryTime = Convert.ToString(dr["DeliveryTime"]),
                            FuelLevel = Convert.ToString(dr["FuelLevel"]),
                            MahenicName = Convert.ToString(dr["MahenicName"]),
                            StartTime = Convert.ToString(dr["StartTime"]),
                            ClosingTime = Convert.ToString(dr["ClosingTime"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return jobCardViewModelList;
        }


        public DataTable GetJobCardDetailDataTable(long jobCardID)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtJobCard = new DataTable();
            try
            {
                dtJobCard = sqlDbInterface.GetJobCardDetail(jobCardID);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtJobCard;
        }
        public DataTable GetJobCardProductDataTable(long jobCardID)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtProducts = new DataTable();
            try
            {
                dtProducts = sqlDbInterface.GetJobCardProductList(jobCardID);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtProducts;
        }

        public DataTable GetJobCardProductDataTable1(long jobCardID)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtProducts = new DataTable();
            try
            {
                dtProducts = sqlDbInterface.GetJobCardProduct(jobCardID);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtProducts;
        }

        public List<JobCardDetailViewModel> GetJobCardProduct(long jobCardID)
        {
            List<JobCardDetailViewModel> jobCardProductDetailViewModelList = new List<JobCardDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCustomers = sqlDbInterface.GetJobCardProduct(jobCardID);
                if (dtCustomers != null && dtCustomers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCustomers.Rows)
                    {
                        jobCardProductDetailViewModelList.Add(new JobCardDetailViewModel
                        {
                            SequenceNo = Convert.ToInt32(dr["SNo"]),
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ProductCode = Convert.ToString(dr["ProductCode"]),                          
                            Price = Convert.ToDecimal(dr["Price"]),
                            Quantity = Convert.ToDecimal(dr["Quantity"]),
                            DiscountPercentage = Convert.ToDecimal(dr["DiscountPercentage"]),
                            DiscountAmount = Convert.ToDecimal(dr["DiscountAmount"]),
                            CGST_Perc = Convert.ToDecimal(dr["CGST_Perc"]),
                            CGST_Amount = Convert.ToDecimal(dr["CGST_Amount"]),
                            SGST_Perc = Convert.ToDecimal(dr["SGST_Perc"]),
                            SGST_Amount = Convert.ToDecimal(dr["SGST_Amount"]),
                            TotalPrice = Convert.ToDecimal(dr["TotalPrice"]),

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return jobCardProductDetailViewModelList;
        }
    }
}
