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
    public class JobWorkMRNBL
    {
        DBInterface dbInterface;
        public JobWorkMRNBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditJobWorkMRN(JobWorkMRNViewModel jobWorkMRNViewModel, List<JobWorkMRNProductViewModel> jobWorkMRNProducts)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                JobWorkMRN jobWorkMRN = new JobWorkMRN
                {
                    JobWorkMRNId = jobWorkMRNViewModel.JobWorkMRNId,
                    JobWorkMRNDate = Convert.ToDateTime(jobWorkMRNViewModel.JobWorkMRNDate),
                    JobWorkMRNTime = Convert.ToDateTime(jobWorkMRNViewModel.JobWorkMRNTime),
                    JobWorkId = jobWorkMRNViewModel.JobWorkId,
                    JobWorkNo = jobWorkMRNViewModel.JobWorkNo,
                    CompanyId = jobWorkMRNViewModel.CompanyId,
                    CompanyBranchId = jobWorkMRNViewModel.CompanyBranchId,
                    Remarks1 = jobWorkMRNViewModel.Remarks1,
                    Remarks2 = jobWorkMRNViewModel.Remarks2,
                    Remarks3 = jobWorkMRNViewModel.Remarks3,
                    Remarks4 = jobWorkMRNViewModel.Remarks4,
                    Remarks5 = jobWorkMRNViewModel.Remarks5,
                    Remarks6 = jobWorkMRNViewModel.Remarks6,
                    Remarks7 = jobWorkMRNViewModel.Remarks7,                  
                    CreatedBy = jobWorkMRNViewModel.CreatedBy,
                    JobWorkMRNStatus = jobWorkMRNViewModel.JobWorkMRNStatus,
                    FinYearId= jobWorkMRNViewModel.FinYearId,
                };
                List<JobWorkMRNProductDetail> jobWorkMRNProductList = new List<JobWorkMRNProductDetail>();
                if (jobWorkMRNProducts != null && jobWorkMRNProducts.Count > 0)
                {
                    foreach (JobWorkMRNProductViewModel item in jobWorkMRNProducts)
                    {
                        jobWorkMRNProductList.Add(new JobWorkMRNProductDetail
                        {
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            Weight=item.Weight,
                            UomId=item.UomId
                        });
                    }
                }
               

                responseOut = sqlDbInterface.AddEditJobWorkMRN(jobWorkMRN, jobWorkMRNProductList);
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


        public List<JobWorkMRNViewModel> GetJobWorkMRNList(string jobWorkMRNNo, string jobOrderNo, int companyBranchId, string fromDate, string toDate, int companyId, string approvalStatus = "")
        {
            List<JobWorkMRNViewModel> jobWorkMRNViewModel = new List<JobWorkMRNViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtQuotations = sqlDbInterface.GetJobWorkMRNList(jobWorkMRNNo, jobOrderNo, companyBranchId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, approvalStatus);
                if (dtQuotations != null && dtQuotations.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtQuotations.Rows)
                    {
                        jobWorkMRNViewModel.Add(new JobWorkMRNViewModel
                        {
                            JobWorkMRNId = Convert.ToInt32(dr["JobWorkMRNId"]),
                            JobWorkMRNNo = Convert.ToString(dr["JobWorkMRNNo"]),
                            JobWorkId = Convert.ToInt32(dr["JobWorkId"]),
                            JobWorkNo = Convert.ToString(dr["JobWorkNo"]),
                            JobWorkMRNDate = Convert.ToString(dr["JobWorkMRNDate"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                            CompanyBranchName = Convert.ToString(dr["CompanyBranchName"]),
                            JobWorkMRNStatus = Convert.ToString(dr["JobWorkMRNStatus"]),
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
            return jobWorkMRNViewModel;
        }
        public JobWorkMRNViewModel GetJobWorkMRNDetail(long jobWorkMRNId = 0)
        {
            JobWorkMRNViewModel jobWorkMRNViewModel = new JobWorkMRNViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCompanies = sqlDbInterface.GetJobWorkMRNDetail(jobWorkMRNId);
                if (dtCompanies != null && dtCompanies.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCompanies.Rows)
                    {
                        jobWorkMRNViewModel = new JobWorkMRNViewModel
                        {
                            JobWorkMRNId = Convert.ToInt32(dr["JobWorkMRNId"]),
                            JobWorkMRNNo = Convert.ToString(dr["JobWorkMRNNo"]),
                            JobWorkMRNDate = Convert.ToString(dr["JobWorkMRNDate"]),
                            JobWorkMRNTime = Convert.ToString(dr["JobWorkMRNTime"]),
                            JobWorkId = Convert.ToInt32(dr["JobWorkId"]),
                            JobWorkNo = Convert.ToString(dr["JobWorkNo"]),                        
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                            JobWorkMRNStatus = Convert.ToString(dr["JobWorkMRNStatus"]),
                            Remarks1 = Convert.ToString(dr["Remarks1"]),
                            Remarks2 = Convert.ToString(dr["Remarks2"]),
                            Remarks3 = Convert.ToString(dr["Remarks3"]),
                            Remarks4 = Convert.ToString(dr["Remarks4"]),
                            Remarks5 = Convert.ToString(dr["Remarks5"]),
                            Remarks6 = Convert.ToString(dr["Remarks6"]),
                            Remarks7 = Convert.ToString(dr["Remarks7"]),                          
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
            return jobWorkMRNViewModel;
        }


        public List<JobOrderViewModel> GetJobWorkMRNJobOrderList(string workOrderNo, int companyBranchId, string fromDate, string toDate, int companyId)
        {
            List<JobOrderViewModel> jobOrders = new List<JobOrderViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtQuotations = sqlDbInterface.GetJobWorkMRNJobOrderList(workOrderNo, companyBranchId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId);
                if (dtQuotations != null && dtQuotations.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtQuotations.Rows)
                    {
                        jobOrders.Add(new JobOrderViewModel
                        {
                            JobWorkId = Convert.ToInt32(dr["JobWorkId"]),
                            JobWorkNo = Convert.ToString(dr["JobWorkNo"]),
                            JobWorkDate = Convert.ToString(dr["JobWorkDate"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                            CompanyBranchName = Convert.ToString(dr["CompanyBranchName"]),
                            VendorName = Convert.ToString(dr["VendorName"]),
                            JobWorkStatus = Convert.ToString(dr["JobWorkStatus"])                            
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return jobOrders;
        }

        public List<JobWorkProductDetailViewModel> GetJobWorkMRNJobOrderProductList(long jobWorkId)
        {
            List<JobWorkProductDetailViewModel> jobWorkProducts = new List<JobWorkProductDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtJobs = sqlDbInterface.GetJobWorkMRNJobOrderProductList(jobWorkId);
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
                            TotalValue = Convert.ToDecimal(dr["TotalValue"]),
                            ScrapPerc = Convert.ToDecimal(dr["ScrapPerc"]),
                            NatureOfProcessing = Convert.ToString(dr["NatureOfProcessing"]),
                            IdentificationMarks = Convert.ToString(dr["IdentificationMarks"]),
                            ProductHSNCode = Convert.ToString(dr["HSN_Code"])

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

        public List<JobWorkMRNProductViewModel> GetJobWorkMRNProductList(long jobWorkMRNId)
        {
            List<JobWorkMRNProductViewModel> jobWorkProducts = new List<JobWorkMRNProductViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtJobs = sqlDbInterface.GetJobWorkMRNProductList(jobWorkMRNId);
                if (dtJobs != null && dtJobs.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtJobs.Rows)
                    {
                        jobWorkProducts.Add(new JobWorkMRNProductViewModel
                        {
                            JobWorkMRNProductDetailId = Convert.ToInt32(dr["JobWorkMRNProductDetailId"]),
                            SequenceNo = Convert.ToInt32(dr["SNo"]),
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ProductCode = Convert.ToString(dr["ProductCode"]),
                            ProductShortDesc = Convert.ToString(dr["ProductShortDesc"]),
                            UOMName = Convert.ToString(dr["UOMName"]),
                            Quantity = Convert.ToDecimal(dr["Quantity"]),
                            UomId = Convert.ToInt32(dr["UomId"]),                          
                            Weight = Convert.ToDecimal(dr["Weight"]),
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

        public DataTable GetJobWorkMRNDetailPrint(long jobWorkMRNId = 0)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtProducts = new DataTable();
            try
            {
                dtProducts = sqlDbInterface.GetJobWorkMRNDetailPrint(jobWorkMRNId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtProducts;
        }
        public DataTable GetJobWorkMRNProductPrint(long jobWorkMRNId = 0)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtProducts = new DataTable();
            try
            {
                dtProducts = sqlDbInterface.GetJobWorkMRNProductListPrint(jobWorkMRNId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtProducts;
        }

    }
}
