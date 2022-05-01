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
    public class JobWorkBL
    {
        DBInterface dbInterface;
        public JobWorkBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditJobWork(JobOrderViewModel jobOrderViewModel, List<JobWorkProductDetailViewModel> jobWorkProduct, List<JobWorkINProductDetailViewModel> jobWorkMRNProduct)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                JobWork jobWork = new JobWork
                {
                    JobWorkId = jobOrderViewModel.JobWorkId,
                    JobWorkDate = Convert.ToDateTime(jobOrderViewModel.JobWorkDate),
                    JobWorkTime = Convert.ToDateTime(jobOrderViewModel.JobWorkTime),
                    VendorId = Convert.ToInt32(jobOrderViewModel.VendorId),
                    Destination = Convert.ToString(jobOrderViewModel.Destination),
                    MotorVehicleNo= Convert.ToString(jobOrderViewModel.MotorVehicleNo),
                    TransportName= Convert.ToString(jobOrderViewModel.TransportName),
                    CompanyId = jobOrderViewModel.CompanyId,
                    CompanyBranchId = jobOrderViewModel.CompanyBranchId,
                    Remarks1 = jobOrderViewModel.Remarks1,
                    Remarks2 = jobOrderViewModel.Remarks2,
                    CreatedBy = jobOrderViewModel.CreatedBy,
                    FinYearId=jobOrderViewModel.FinYearId,
                    JobWorkStatus = jobOrderViewModel.JobWorkStatus
                };
                List<JobWorkProductDetail> jobWorkProductList = new List<JobWorkProductDetail>();
                if(jobWorkProduct != null && jobWorkProduct.Count>0)
                {
                    foreach(JobWorkProductDetailViewModel item in jobWorkProduct)
                    {
                        jobWorkProductList.Add(new JobWorkProductDetail
                        {
                            JobWorkProductDetailId=item.JobWorkProductDetailId,
                            JobWorkId=item.JobWorkId,
                            ProductId=item.ProductId,
                            NatureOfProcessing=item.NatureOfProcessing,
                            IdentificationMarks = item.IdentificationMarks,
                            Quantity=item.Quantity,
                            TotalValue= item.TotalValue,
                            ScrapPerc = item.ScrapPerc
                        });
                    }
                }

                List<JobWorkINProductDetail> jobWorkINProductList = new List<JobWorkINProductDetail>();
                if (jobWorkMRNProduct != null && jobWorkMRNProduct.Count > 0)
                {
                    foreach (JobWorkINProductDetailViewModel item in jobWorkMRNProduct)
                    {
                        jobWorkINProductList.Add(new JobWorkINProductDetail
                        {
                            JobWorkProductInDetailId = item.JobWorkProductInDetailId,
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            Weight = item.Weight,
                            UomId = item.UomId
                        });
                    }
                }
                responseOut = sqlDbInterface.AddEditJobWork(jobWork, jobWorkProductList, jobWorkINProductList);
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
    
       
        public List<WorkOrderViewModel> GetWorkOrderList(string workOrderNo, int companyBranchId,string fromDate, string toDate, int companyId, string displayType = "",string approvalStatus="")
        {
            List<WorkOrderViewModel> workOrders = new List<WorkOrderViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtQuotations = sqlDbInterface.GetWorkOrderList(workOrderNo, companyBranchId,  Convert.ToDateTime(fromDate),Convert.ToDateTime(toDate),companyId, displayType, approvalStatus);
                if (dtQuotations != null && dtQuotations.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtQuotations.Rows)
                    {
                        workOrders.Add(new WorkOrderViewModel
                        {
                            WorkOrderId = Convert.ToInt32(dr["WorkOrderId"]),
                            WorkOrderNo = Convert.ToString(dr["WorkOrderNo"]),
                            WorkOrderDate = Convert.ToString(dr["WorkOrderDate"]),
                            TargetFromDate = Convert.ToString(dr["TargetFromDate"]),
                            TargetToDate = Convert.ToString(dr["TargetToDate"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                            CompanyBranchName = Convert.ToString(dr["CompanyBranchName"]),
                            AssemblyType= Convert.ToString(dr["AssemblyType"]),
                            WorkOrderStatus = Convert.ToString(dr["WorkOrderStatus"]),
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
            return workOrders;
        }
        public JobOrderViewModel GetJobWorkDetail(long jobWorkId = 0)
        {
            JobOrderViewModel jobWork = new JobOrderViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtJobWorks = sqlDbInterface.GetJobWorkDetail(jobWorkId);
                if (dtJobWorks != null && dtJobWorks.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtJobWorks.Rows)
                    {
                        jobWork = new JobOrderViewModel
                        {
                            JobWorkId = Convert.ToInt32(dr["JobWorkId"]),
                            JobWorkNo = Convert.ToString(dr["JobWorkNo"]),
                            JobWorkDate = Convert.ToString(dr["JobWorkDate"]),
                            JobWorkTime = Convert.ToString(dr["JobWorkTime"]),
                            VendorId = Convert.ToInt32(dr["VendorId"]),
                            VendorName = Convert.ToString(dr["VendorName"]),
                            VendorCode = Convert.ToString(dr["VendorCode"]),
                            Destination = Convert.ToString(dr["Destination"]),
                            MotorVehicleNo = Convert.ToString(dr["MotorVehicleNo"]),
                            TransportName = Convert.ToString(dr["TransportName"]),
                            CompanyBranchId =Convert.ToInt32(dr["CompanyBranchId"]),
                            JobWorkStatus = Convert.ToString(dr["JobWorkStatus"]),
                            Remarks1 = Convert.ToString(dr["Remarks1"]),
                            Remarks2 = Convert.ToString(dr["Remarks2"]),
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
            return jobWork;
        }
    
 
        public List<JobWorkProductDetailViewModel> GetJobWorkProductList(long jobWorkId)
        {
            List<JobWorkProductDetailViewModel> jobWorkProducts = new List<JobWorkProductDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtJobs = sqlDbInterface.GetJobWorkProductList(jobWorkId);
                if (dtJobs != null && dtJobs.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtJobs.Rows)
                    {
                        jobWorkProducts.Add(new JobWorkProductDetailViewModel
                        {
                            JobWorkProductDetailId = Convert.ToInt32(dr["JobWorkProductDetailId"]),
                            SequenceNo = Convert.ToInt32(dr["SNo"]),
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ProductCode = Convert.ToString(dr["ProductCode"]),
                            ProductShortDesc = Convert.ToString(dr["ProductShortDesc"]),
                            UOMName = Convert.ToString(dr["UOMName"]),
                            Quantity = Convert.ToDecimal(dr["Quantity"]),
                            TotalValue= Convert.ToDecimal(dr["TotalValue"]),
                            ScrapPerc= Convert.ToDecimal(dr["ScrapPerc"]),
                            NatureOfProcessing=Convert.ToString(dr["NatureOfProcessing"]),
                            IdentificationMarks= Convert.ToString(dr["IdentificationMarks"]),
                            ProductHSNCode=Convert.ToString(dr["HSN_Code"])

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return jobWorkProducts;
        }

        public List<JobWorkINProductDetailViewModel> GetJobWorkProductInList(long jobWorkId)
        {
            List<JobWorkINProductDetailViewModel> jobWorkProducts = new List<JobWorkINProductDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtJobInProducts = sqlDbInterface.GetJobWorkProductInList(jobWorkId);
                if (dtJobInProducts != null && dtJobInProducts.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtJobInProducts.Rows)
                    {
                        jobWorkProducts.Add(new JobWorkINProductDetailViewModel
                        {
                            JobWorkProductInDetailId = Convert.ToInt32(dr["JobWorkProductInDetailId"]),
                            SequenceNo = Convert.ToInt32(dr["SNo"]),
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ProductCode = Convert.ToString(dr["ProductCode"]),
                            ProductShortDesc = Convert.ToString(dr["ProductShortDesc"]),
                            UomId = Convert.ToInt32(dr["UomId"]),
                            UOMName = Convert.ToString(dr["UOMName"]),
                            Quantity = Convert.ToDecimal(dr["Quantity"]),
                            Weight= Convert.ToDecimal(dr["Weight"]),
                            ProductHSNCode = Convert.ToString(dr["HSN_Code"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return jobWorkProducts;
        }

        public List<JobOrderViewModel> GetJobWorkList(string jobWorkNo, int companyBranchId, string fromDate, string toDate, int companyId, string displayType = "", string jobWorkStatus = "")
        {
            List<JobOrderViewModel> jobWorks = new List<JobOrderViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtJobWorks = sqlDbInterface.GetJobWorkList(jobWorkNo, companyBranchId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, displayType, jobWorkStatus);
                if (dtJobWorks != null && dtJobWorks.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtJobWorks.Rows)
                    {
                        jobWorks.Add(new JobOrderViewModel
                        {
                            JobWorkId = Convert.ToInt32(dr["JobWorkId"]),
                            JobWorkNo = Convert.ToString(dr["JobWorkNo"]),
                            JobWorkDate = Convert.ToString(dr["JobWorkDate"]),
                            JobWorkTime = Convert.ToString(dr["JobWorkTime"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                            CompanyBranchName=Convert.ToString(dr["CompanyBranchName"]),
                            VendorId = Convert.ToInt32(dr["VendorId"]),
                            VendorName = Convert.ToString(dr["VendorName"]),
                            VendorCode = Convert.ToString(dr["VendorCode"]),
                            Destination= Convert.ToString(dr["Destination"]),
                            MotorVehicleNo = Convert.ToString(dr["MotorVehicleNo"]),
                            TransportName= Convert.ToString(dr["TransportName"]),
                            JobWorkStatus = Convert.ToString(dr["JobWorkStatus"]),
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
            return jobWorks;
        }

        public DataTable GetJobWorkProductListDataTable(long jobWorkId=0)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtProducts = new DataTable();
            try
            {
                dtProducts = sqlDbInterface.GetJobWorkProductList(jobWorkId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtProducts;
        }

        public DataTable GetJobWorkDataTable(long jobworkId = 0)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtjobWork = new DataTable();
            try
            {
                dtjobWork = sqlDbInterface.GetJobWorkDetail(jobworkId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtjobWork;
        }

        public DataTable GetJobWorkTotalValue(long jobWorkId = 0)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtTotalValue = new DataTable();
            try
            {
                dtTotalValue = sqlDbInterface.GetJobWorkTotalValue(jobWorkId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtTotalValue;
        }
    }
}
