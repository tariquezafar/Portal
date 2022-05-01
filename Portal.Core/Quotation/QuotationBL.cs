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
    public class QuotationBL
    {
        DBInterface dbInterface;
        public QuotationBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditQuotation(QuotationViewModel quotationViewModel,List<QuotationProductViewModel> quotationProducts, List<QuotationTaxViewModel> quotationTaxes, List<QuotationTermViewModel> quotationTerms, List<QuotationSupportingDocumentViewModel> quotationDocuments)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                Quotation quotation = new Quotation
                {
                    QuotationId = quotationViewModel.QuotationId,
                    QuotationDate = Convert.ToDateTime(quotationViewModel.QuotationDate),
                    CompanyBranchId = quotationViewModel.CompanyBranchId,
                    CurrencyCode = quotationViewModel.CurrencyCode,
                    CustomerId = quotationViewModel.CustomerId,
                    CustomerName = quotationViewModel.CustomerName,
                    BillingAddress = quotationViewModel.BillingAddress,
                    City = quotationViewModel.City,
                    StateId = quotationViewModel.StateId,
                    CountryId = quotationViewModel.CountryId,
                    PinCode = quotationViewModel.PinCode,
                    CSTNo = quotationViewModel.CSTNo,
                    TINNo = quotationViewModel.TINNo,
                    PANNo = quotationViewModel.PANNo,
                    GSTNo = quotationViewModel.GSTNo,
                    ExciseNo = quotationViewModel.ExciseNo,
                    RefNo = quotationViewModel.RefNo,
                    RefDate = string.IsNullOrEmpty(quotationViewModel.RefDate) ? Convert.ToDateTime("01-01-1900") : Convert.ToDateTime(quotationViewModel.RefDate),
                    BasicValue = quotationViewModel.BasicValue,
                    TotalValue = quotationViewModel.TotalValue,

                    FreightValue = quotationViewModel.FreightValue,
                    FreightCGST_Perc = quotationViewModel.FreightCGST_Perc,
                    FreightCGST_Amt = quotationViewModel.FreightCGST_Amt,
                    FreightSGST_Perc = quotationViewModel.FreightSGST_Perc,
                    FreightSGST_Amt = quotationViewModel.FreightSGST_Amt,
                    FreightIGST_Perc = quotationViewModel.FreightIGST_Perc,
                    FreightIGST_Amt = quotationViewModel.FreightIGST_Amt,

                    LoadingValue = quotationViewModel.LoadingValue,
                    LoadingCGST_Perc = quotationViewModel.LoadingCGST_Perc,
                    LoadingCGST_Amt = quotationViewModel.LoadingCGST_Amt,
                    LoadingSGST_Perc = quotationViewModel.LoadingSGST_Perc,
                    LoadingSGST_Amt = quotationViewModel.LoadingSGST_Amt,
                    LoadingIGST_Perc = quotationViewModel.LoadingIGST_Perc,
                    LoadingIGST_Amt = quotationViewModel.LoadingIGST_Amt,
                    InsuranceValue = quotationViewModel.InsuranceValue,
                    InsuranceCGST_Perc = quotationViewModel.InsuranceCGST_Perc,
                    InsuranceCGST_Amt = quotationViewModel.InsuranceCGST_Amt,
                    InsuranceSGST_Perc = quotationViewModel.InsuranceSGST_Perc,
                    InsuranceSGST_Amt = quotationViewModel.InsuranceSGST_Amt,
                    InsuranceIGST_Perc = quotationViewModel.InsuranceIGST_Perc,
                    InsuranceIGST_Amt = quotationViewModel.InsuranceIGST_Amt,

                    RtoRegsValue = quotationViewModel.RtoRegsValue,
                    RtoRegsCGST_Amt = quotationViewModel.RtoRegsCGST_Amt,
                    RtoRegsSGST_Amt = quotationViewModel.RtoRegsSGST_Amt,
                    RtoRegsIGST_Amt = quotationViewModel.RtoRegsIGST_Amt,
                    RtoRegsCGST_Perc = quotationViewModel.RtoRegsCGST_Perc,
                    RtoRegsSGST_Perc = quotationViewModel.RtoRegsSGST_Perc,
                    RtoRegsIGST_Perc = quotationViewModel.RtoRegsIGST_Perc,
                    VehicleInsuranceValue = quotationViewModel.VehicleInsuranceValue,


                    Remarks1 = quotationViewModel.Remarks1,
                    Remarks2 = quotationViewModel.Remarks2,

                    FinYearId = quotationViewModel.FinYearId,
                    CompanyId = quotationViewModel.CompanyId,
                    CreatedBy = quotationViewModel.CreatedBy,
                    ApprovalStatus= quotationViewModel.ApprovalStatus,
                    ReverseChargeApplicable = quotationViewModel.ReverseChargeApplicable,
                    ReverseChargeAmount = quotationViewModel.ReverseChargeAmount


                };
                List<QuotationProductDetail> quotationProductList = new List<QuotationProductDetail>();
                if(quotationProducts!=null && quotationProducts.Count>0)
                {
                    foreach(QuotationProductViewModel item in quotationProducts )
                    {
                        quotationProductList.Add(new QuotationProductDetail
                        {
                            ProductId=item.ProductId,
                            ProductShortDesc=item.ProductShortDesc,
                            Price=item.Price,
                            Quantity=item.Quantity,
                            DiscountPercentage=item.DiscountPercentage,
                            DiscountAmount=item.DiscountAmount,
                            TaxPercentage=item.TaxPercentage,
                            TaxAmount=item.TaxAmount,
                            TotalPrice =item.TotalPrice,
                            TaxId=item.TaxId,
                            TaxName=item.TaxName,
                            SurchargeName_1 = item.SurchargeName_1,
                            SurchargePercentage_1 = item.SurchargePercentage_1,
                            SurchargeAmount_1 = item.SurchargeAmount_1,
                            SurchargeName_2 = item.SurchargeName_2,
                            SurchargePercentage_2 = item.SurchargePercentage_2,
                            SurchargeAmount_2 = item.SurchargeAmount_2,
                            SurchargeName_3 = item.SurchargeName_3,
                            SurchargePercentage_3 = item.SurchargePercentage_3,
                            SurchargeAmount_3 = item.SurchargeAmount_3,
                            CGST_Perc = item.CGST_Perc,
                            CGST_Amount = item.CGST_Amount,
                            SGST_Perc = item.SGST_Perc,
                            SGST_Amount = item.SGST_Amount,
                            IGST_Perc = item.IGST_Perc,
                            IGST_Amount = item.IGST_Amount,
                            HSN_Code = item.HSN_Code
                        });
                    }
                }

                List<QuotationTaxDetail> quotationTaxList = new List<QuotationTaxDetail>();
                if (quotationTaxes != null && quotationTaxes.Count > 0)
                {
                    foreach (QuotationTaxViewModel item in quotationTaxes)
                    {
                        quotationTaxList.Add(new QuotationTaxDetail
                        {
                            TaxId = item.TaxId,
                            TaxName = item.TaxName,
                            TaxPercentage = item.TaxPercentage,
                            TaxAmount = item.TaxAmount,
                            SurchargeName_1 = item.SurchargeName_1,
                            SurchargePercentage_1 = item.SurchargePercentage_1,
                            SurchargeAmount_1 = item.SurchargeAmount_1,
                            SurchargeName_2 = item.SurchargeName_2,
                            SurchargePercentage_2 = item.SurchargePercentage_2,
                            SurchargeAmount_2 = item.SurchargeAmount_2,
                            SurchargeName_3 = item.SurchargeName_3,
                            SurchargePercentage_3 = item.SurchargePercentage_3,
                            SurchargeAmount_3 = item.SurchargeAmount_3
                        });
                    }
                }
                List<QuotationTermsDetail> quotationTermList = new List<QuotationTermsDetail>();
                if (quotationTerms != null && quotationTerms.Count > 0)
                {
                    foreach (QuotationTermViewModel item in quotationTerms)
                    {
                        quotationTermList.Add(new QuotationTermsDetail
                        {
                            TermDesc = item.TermDesc,
                            TermSequence = item.TermSequence
                        });
                    }
                }


                List<QuotationSupportingDocument> quotationDocumentList = new List<QuotationSupportingDocument>();
                if (quotationDocuments != null && quotationDocuments.Count > 0)
                {
                    foreach (QuotationSupportingDocumentViewModel item in quotationDocuments)
                    {
                        quotationDocumentList.Add(new QuotationSupportingDocument
                        {
                            DocumentTypeId = item.DocumentTypeId,
                            DocumentName = item.DocumentName,
                            DocumentPath = item.DocumentPath
                        });
                    }
                }

                responseOut = sqlDbInterface.AddEditQuotation(quotation, quotationProductList, quotationTaxList, quotationTermList, quotationDocumentList);
             

             
             
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
        public ResponseOut RemoveCustomerBranch(long customerBranchId)
        {
            
            ResponseOut responseOut = new ResponseOut();
            try
                {
                    responseOut = dbInterface.RemoveCustomerBranch(customerBranchId);
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

        public ResponseOut RemoveCustomerProduct(long mappingId)
        {

            ResponseOut responseOut = new ResponseOut();
            try
            {
                responseOut = dbInterface.RemoveCustomerProduct(mappingId);
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

        public List<QuotationViewModel> GetQuotationList(string quotationNo, string customerName, string refNo, string fromDate, string toDate, int companyId, string displayType = "",string approvalStatus="", int companyBranchId = 0,int CustomerId=0)
        {
            List<QuotationViewModel> quotations = new List<QuotationViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtQuotations = sqlDbInterface.GetQuotationList(quotationNo, customerName, refNo, Convert.ToDateTime(fromDate),Convert.ToDateTime(toDate),companyId, displayType, approvalStatus, companyBranchId,CustomerId);
                if (dtQuotations != null && dtQuotations.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtQuotations.Rows)
                    {
                        quotations.Add(new QuotationViewModel
                        {
                            QuotationId = Convert.ToInt32(dr["QuotationId"]),
                            QuotationNo = Convert.ToString(dr["QuotationNo"]),
                            QuotationDate = Convert.ToString(dr["QuotationDate"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            CustomerId = Convert.ToInt32(dr["CustomerID"]),
                            CustomerCode= Convert.ToString(dr["CustomerCode"]),
                            City = Convert.ToString(dr["City"]),
                            StateName = Convert.ToString(dr["StateName"]),
                            RefNo = Convert.ToString(dr["RefNo"]),
                            RefDate = Convert.ToString(dr["RefDate"]),
                            BasicValue = Convert.ToDecimal(dr["BasicValue"]),
                            TotalValue = Convert.ToDecimal(dr["TotalValue"]),
                            QuotationRevisedStatus = Convert.ToBoolean(dr["QuotationRevisedStatus"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ApprovalStatus=Convert.ToString(dr["ApprovalStatus"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            CompanyBranchId=Convert.ToInt32(dr["CompanyBranchId"]),
                            CompanyBranchName= Convert.ToString(dr["CompanyBranchName"])
                           

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return quotations;
        }
        public QuotationViewModel GetQuotationDetail(long quotationId = 0)
        {
            QuotationViewModel quotation = new QuotationViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCompanies = sqlDbInterface.GetQuotationDetail(quotationId);
                if (dtCompanies != null && dtCompanies.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCompanies.Rows)
                    {
                        quotation = new QuotationViewModel
                        {
                            QuotationId = Convert.ToInt32(dr["QuotationId"]),
                            QuotationNo = Convert.ToString(dr["QuotationNo"]),
                            QuotationDate = Convert.ToString(dr["QuotationDate"]),
                            CompanyBranchId=Convert.ToInt32(dr["CompanyBranchId"]),
                            BranchType= Convert.ToString(dr["BranchType"]),
                            CurrencyCode = Convert.ToString(dr["CurrencyCode"]),
                            CustomerId = Convert.ToInt32(dr["CustomerId"]),
                            CustomerCode = Convert.ToString(dr["CustomerCode"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            BillingAddress = Convert.ToString(dr["BillingAddress"]),
                            City = Convert.ToString(dr["City"]),
                            StateId = Convert.ToInt32(dr["StateId"]),
                            CountryId = Convert.ToInt32(dr["CountryId"]),
                            PinCode = Convert.ToString(dr["PinCode"]),
                            CSTNo = Convert.ToString(dr["CSTNo"]),
                            TINNo = Convert.ToString(dr["TINNo"]),
                            PANNo = Convert.ToString(dr["PANNo"]),
                            GSTNo = Convert.ToString(dr["GSTNo"]),
                            RefNo = Convert.ToString(dr["RefNo"]),
                            RefDate = Convert.ToString(dr["RefDate"]),
                            ApprovalStatus = Convert.ToString(dr["ApprovalStatus"]),
                            BasicValue = string.IsNullOrEmpty(dr["BasicValue"].ToString()) ? Convert.ToDecimal("0.0") : Convert.ToDecimal(dr["BasicValue"]),
                            TotalValue = string.IsNullOrEmpty(dr["TotalValue"].ToString()) ? Convert.ToDecimal("0.0") : Convert.ToDecimal(dr["TotalValue"]),                         
                            FreightValue = Convert.ToDecimal(dr["FreightValue"]),
                            FreightCGST_Perc = Convert.ToDecimal(dr["FreightCGST_Perc"]),
                            FreightCGST_Amt = Convert.ToDecimal(dr["FreightCGST_Amt"]),
                            FreightSGST_Perc = Convert.ToDecimal(dr["FreightSGST_Perc"]),
                            FreightSGST_Amt = Convert.ToDecimal(dr["FreightSGST_Amt"]),
                            FreightIGST_Perc = Convert.ToDecimal(dr["FreightIGST_Perc"]),
                            FreightIGST_Amt = Convert.ToDecimal(dr["FreightIGST_Amt"]),
                            LoadingValue = Convert.ToDecimal(dr["LoadingValue"]),
                            LoadingCGST_Perc = Convert.ToDecimal(dr["LoadingCGST_Perc"]),
                            LoadingCGST_Amt = Convert.ToDecimal(dr["LoadingCGST_Amt"]),
                            LoadingSGST_Perc = Convert.ToDecimal(dr["LoadingSGST_Perc"]),
                            LoadingSGST_Amt = Convert.ToDecimal(dr["LoadingSGST_Amt"]),
                            LoadingIGST_Perc = Convert.ToDecimal(dr["LoadingIGST_Perc"]),

                            LoadingIGST_Amt = Convert.ToDecimal(dr["LoadingIGST_Amt"]),
                            InsuranceValue = Convert.ToDecimal(dr["InsuranceValue"]),
                            InsuranceCGST_Perc = Convert.ToDecimal(dr["InsuranceCGST_Perc"]),
                            InsuranceCGST_Amt = Convert.ToDecimal(dr["InsuranceCGST_Amt"]),
                            InsuranceSGST_Perc = Convert.ToDecimal(dr["InsuranceSGST_Perc"]),
                            InsuranceSGST_Amt = Convert.ToDecimal(dr["InsuranceSGST_Amt"]),
                            InsuranceIGST_Perc = Convert.ToDecimal(dr["InsuranceIGST_Perc"]),
                            InsuranceIGST_Amt = Convert.ToDecimal(dr["InsuranceIGST_Amt"]),

                            RtoRegsValue = Convert.ToDecimal(dr["RtoRegsValue"]),
                            RtoRegsCGST_Amt = Convert.ToDecimal(dr["RtoRegsCGST_Amt"]),
                            RtoRegsCGST_Perc = Convert.ToDecimal(dr["RtoRegsCGST_Perc"]),
                            RtoRegsIGST_Amt = Convert.ToDecimal(dr["RtoRegsIGST_Amt"]),
                            RtoRegsIGST_Perc = Convert.ToDecimal(dr["RtoRegsIGST_Perc"]),
                            RtoRegsSGST_Amt = Convert.ToDecimal(dr["RtoRegsSGST_Amt"]),
                            RtoRegsSGST_Perc = Convert.ToDecimal(dr["RtoRegsSGST_Perc"]),
                            VehicleInsuranceValue = Convert.ToDecimal(dr["VehicleInsuranceValue"]),



                            Remarks1 = Convert.ToString(dr["Remarks1"]),
                            Remarks2 = Convert.ToString(dr["Remarks2"]),
                            ReverseChargeApplicable = Convert.ToBoolean(dr["ReverseChargeApplicable"]),
                            ReverseChargeAmount = Convert.ToDecimal(dr["ReverseChargeAmount"]),
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
            return quotation;
        }
        public DataTable GetQuotationDetailDataTable(long quotationId = 0)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtQuotation = new DataTable();
            try
            {
                 dtQuotation = sqlDbInterface.GetQuotationDetail(quotationId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtQuotation;
        }
        public List<CustomerBranchViewModel> GetCustomerBranchList(int customerId)
        {
            List<CustomerBranchViewModel> customerBranchs = new List<CustomerBranchViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCustomers = sqlDbInterface.GetCustomerBranchList(customerId);
                if (dtCustomers != null && dtCustomers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCustomers.Rows)
                    {
                        customerBranchs.Add(new CustomerBranchViewModel
                        {
                            CustomerBranchId = Convert.ToInt32(dr["CustomerBranchId"]),
                            BranchName = Convert.ToString(dr["BranchName"]),
                            PrimaryAddress = Convert.ToString(dr["PrimaryAddress"]),
                            City = Convert.ToString(dr["City"]),
                            StateId = Convert.ToInt32(dr["StateId"]),
                            StateName = Convert.ToString(dr["StateName"]),
                            CountryId = Convert.ToInt32(dr["CountryId"]),
                            PinCode = Convert.ToString(dr["PinCode"]),
                            CSTNo = Convert.ToString(dr["CSTNo"]),
                            TINNo = Convert.ToString(dr["TINNo"]),
                            PANNo = Convert.ToString(dr["PANNo"]),
                            GSTNo = Convert.ToString(dr["GSTNo"]),
                            ContactPersonName = Convert.ToString(dr["ContactPersonName"]),
                            Designation = Convert.ToString(dr["Designation"]),
                            Email = Convert.ToString(dr["Email"]),
                            MobileNo = Convert.ToString(dr["MobileNo"]),
                            ContactNo = Convert.ToString(dr["ContactNo"]),
                            Fax = Convert.ToString(dr["Fax"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return customerBranchs;
        }

        public List<QuotationProductViewModel> GetQuotationProductList(long quotationId)
        {
            List<QuotationProductViewModel> quotationProducts = new List<QuotationProductViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCustomers = sqlDbInterface.GetQuotationProductList(quotationId);
                if (dtCustomers != null && dtCustomers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCustomers.Rows)
                    {
                        quotationProducts.Add(new QuotationProductViewModel
                        {
                            QuotationProductDetailId = Convert.ToInt32(dr["QuotationProductDetailId"]),
                            SequenceNo = Convert.ToInt32(dr["SNo"]),
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ProductCode = Convert.ToString(dr["ProductCode"]),
                            ProductShortDesc = Convert.ToString(dr["ProductShortDesc"]),
                            UOMName = Convert.ToString(dr["UOMName"]),
                            Price = Convert.ToDecimal(dr["Price"]),
                            Quantity = Convert.ToDecimal(dr["Quantity"]),
                            DiscountPercentage = Convert.ToDecimal(dr["DiscountPercentage"]),
                            DiscountAmount = Convert.ToDecimal(dr["DiscountAmount"]),
                            TaxId = Convert.ToInt32(dr["TaxId"]),
                            TaxName = Convert.ToString(dr["TaxName"]),
                            TaxPercentage = Convert.ToDecimal(dr["TaxPercentage"]),
                            TaxAmount = Convert.ToDecimal(dr["TaxAmount"]),
                            TotalPrice = Convert.ToDecimal(dr["TotalPrice"]),
                            SurchargeName_1 = Convert.ToString(dr["SurchargeName_1"]),
                            SurchargePercentage_1 = Convert.ToDecimal(dr["SurchargePercentage_1"]),
                            SurchargeAmount_1 = Convert.ToDecimal(dr["SurchargeAmount_1"]),
                            SurchargeName_2 = Convert.ToString(dr["SurchargeName_2"]),
                            SurchargePercentage_2 = Convert.ToDecimal(dr["SurchargePercentage_2"]),
                            SurchargeAmount_2 = Convert.ToDecimal(dr["SurchargeAmount_2"]),
                            SurchargeName_3 = Convert.ToString(dr["SurchargeName_3"]),
                            SurchargePercentage_3 = Convert.ToDecimal(dr["SurchargePercentage_3"]),
                            SurchargeAmount_3 = Convert.ToDecimal(dr["SurchargeAmount_3"]),
                            CGST_Perc = Convert.ToDecimal(dr["CGST_Perc"]),
                            CGST_Amount = Convert.ToDecimal(dr["CGST_Amount"]),
                            SGST_Perc = Convert.ToDecimal(dr["SGST_Perc"]),
                            SGST_Amount = Convert.ToDecimal(dr["SGST_Amount"]),
                            IGST_Perc = Convert.ToDecimal(dr["IGST_Perc"]),
                            IGST_Amount = Convert.ToDecimal(dr["IGST_Amount"]),
                            HSN_Code = Convert.ToString(dr["HSN_Code"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return quotationProducts;
        }
        public DataTable GetQuotationProductListDataTable(long quotationId)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtProducts = new DataTable();
            try
            {
                 dtProducts = sqlDbInterface.GetQuotationProductList(quotationId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtProducts;
        }
        public DataTable GetQuotationTermListDataTable(long quotationId)
        {
            
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtTerms = new DataTable();
            try
            {
                 dtTerms = sqlDbInterface.GetQuotationTermList(quotationId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtTerms;
        }

        public DataTable GetQuotationTaxListDataTable(long quotationId)
        {
            DataTable dtTaxes = new DataTable();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                dtTaxes = sqlDbInterface.GetQuotationTaxList(quotationId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtTaxes;
        }

        public List<QuotationTaxViewModel> GetQuotationTaxList(long quotationId)
        {
            List<QuotationTaxViewModel> quotationTaxes = new List<QuotationTaxViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCustomers = sqlDbInterface.GetQuotationTaxList(quotationId);
                if (dtCustomers != null && dtCustomers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCustomers.Rows)
                    {
                        quotationTaxes.Add(new QuotationTaxViewModel
                        {
                            QuotationTaxDetailId = Convert.ToInt32(dr["QuotationTaxDetailId"]),
                            TaxSequenceNo= Convert.ToInt32(dr["SNo"]),
                            TaxId = Convert.ToInt32(dr["TaxId"]),
                            TaxName = Convert.ToString(dr["TaxName"]),
                            TaxPercentage = Convert.ToDecimal(dr["TaxPercentage"]),
                            TaxAmount = Convert.ToDecimal(dr["TaxAmount"]),
                            SurchargeName_1 = Convert.ToString(dr["SurchargeName_1"]),
                            SurchargePercentage_1 = Convert.ToDecimal(dr["SurchargePercentage_1"]),
                            SurchargeAmount_1 = Convert.ToDecimal(dr["SurchargeAmount_1"]),
                            SurchargeName_2 = Convert.ToString(dr["SurchargeName_2"]),
                            SurchargePercentage_2 = Convert.ToDecimal(dr["SurchargePercentage_2"]),
                            SurchargeAmount_2 = Convert.ToDecimal(dr["SurchargeAmount_2"]),
                            SurchargeName_3 = Convert.ToString(dr["SurchargeName_3"]),
                            SurchargePercentage_3 = Convert.ToDecimal(dr["SurchargePercentage_3"]),
                            SurchargeAmount_3 = Convert.ToDecimal(dr["SurchargeAmount_3"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return quotationTaxes;
        }
        public List<QuotationTermViewModel> GetQuotationTermList(long quotationId)
        {
            List<QuotationTermViewModel> quotationTerms = new List<QuotationTermViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCustomers = sqlDbInterface.GetQuotationTermList(quotationId);
                if (dtCustomers != null && dtCustomers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCustomers.Rows)
                    {
                        quotationTerms.Add(new QuotationTermViewModel
                        {
                            QuotationTermDetailId = Convert.ToInt32(dr["QuotationTermDetailId"]),
                            TermDesc = Convert.ToString(dr["TermDesc"]),
                            TermSequence = Convert.ToInt16(dr["TermSequence"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return quotationTerms;
        }

        public List<CustomerViewModel> GetCustomerAutoCompleteList(string searchTerm, int companyId)
        {
            List<CustomerViewModel> customers = new List<CustomerViewModel>();
            try
            {
                List<Customer> customerList = dbInterface.GetCustomerAutoCompleteList(searchTerm, companyId);

                if (customerList != null && customerList.Count > 0)
                {
                    foreach (Customer customer in customerList)
                    {
                        customers.Add(new CustomerViewModel { CustomerId = customer.CustomerId, CustomerName = customer.CustomerName, CustomerCode = customer.CustomerCode, PrimaryAddress = customer.PrimaryAddress });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return customers;
        }

        public List<QuotationSupportingDocumentViewModel> GetQuotationSupportingDocumentList(long quotationId)
        {
            List<QuotationSupportingDocumentViewModel> quotaionDocuments = new List<QuotationSupportingDocumentViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtDocument = sqlDbInterface.GetQupotationSupportingDocumentList(quotationId);
                if (dtDocument != null && dtDocument.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtDocument.Rows)
                    {
                        quotaionDocuments.Add(new QuotationSupportingDocumentViewModel
                        {
                            QuotationDocId = Convert.ToInt32(dr["QuotationDocId"]),
                            DocumentSequenceNo = Convert.ToInt32(dr["SNo"]),
                            DocumentTypeId = Convert.ToInt32(dr["DocumentTypeId"]),
                            DocumentTypeDesc = Convert.ToString(dr["DocumentTypeDesc"]),
                            DocumentName = Convert.ToString(dr["DocumentName"]),
                            DocumentPath = Convert.ToString(dr["DocumentPath"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return quotaionDocuments;
        }

        #region Revised Quotation 
        public ResponseOut AddRevisedQuotation(QuotationViewModel quotationViewModel, List<QuotationProductViewModel> quotationProducts, List<QuotationTaxViewModel> quotationTaxes, List<QuotationTermViewModel> quotationTerms, List<QuotationSupportingDocumentViewModel> revisedQuotationDocuments)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                Quotation quotation = new Quotation
                {
                    QuotationId = quotationViewModel.QuotationId,
                    QuotationNo = quotationViewModel.QuotationNo,
                    QuotationDate = Convert.ToDateTime(quotationViewModel.QuotationDate),

                    CompanyBranchId= Convert.ToInt32(quotationViewModel.CompanyBranchId),
                    CurrencyCode = Convert.ToString(quotationViewModel.CurrencyCode),
                    CustomerId = quotationViewModel.CustomerId,
                    CustomerName = quotationViewModel.CustomerName,
                    BillingAddress = quotationViewModel.BillingAddress,
                    City = quotationViewModel.City,
                    StateId = quotationViewModel.StateId,
                    CountryId = quotationViewModel.CountryId,
                    PinCode = quotationViewModel.PinCode,
                    CSTNo = quotationViewModel.CSTNo,
                    TINNo = quotationViewModel.TINNo,
                    PANNo = quotationViewModel.PANNo,
                    GSTNo = quotationViewModel.GSTNo,
                    ExciseNo = quotationViewModel.ExciseNo,
                    RefNo = quotationViewModel.RefNo,
                    RefDate = string.IsNullOrEmpty(quotationViewModel.RefDate) ? Convert.ToDateTime("01-01-1900") : Convert.ToDateTime(quotationViewModel.RefDate),
                    BasicValue = quotationViewModel.BasicValue,
                    TotalValue = quotationViewModel.TotalValue,

                    FreightValue = quotationViewModel.FreightValue,
                    FreightCGST_Perc = quotationViewModel.FreightCGST_Perc,
                    FreightCGST_Amt = quotationViewModel.FreightCGST_Amt,
                    FreightSGST_Perc = quotationViewModel.FreightSGST_Perc,
                    FreightSGST_Amt = quotationViewModel.FreightSGST_Amt,
                    FreightIGST_Perc = quotationViewModel.FreightIGST_Perc,
                    FreightIGST_Amt = quotationViewModel.FreightIGST_Amt,

                    LoadingValue = quotationViewModel.LoadingValue,
                    LoadingCGST_Perc = quotationViewModel.LoadingCGST_Perc,
                    LoadingCGST_Amt = quotationViewModel.LoadingCGST_Amt,
                    LoadingSGST_Perc = quotationViewModel.LoadingSGST_Perc,
                    LoadingSGST_Amt = quotationViewModel.LoadingSGST_Amt,
                    LoadingIGST_Perc = quotationViewModel.LoadingIGST_Perc,
                    LoadingIGST_Amt = quotationViewModel.LoadingIGST_Amt,
                    InsuranceValue = quotationViewModel.InsuranceValue,
                    InsuranceCGST_Perc = quotationViewModel.InsuranceCGST_Perc,
                    InsuranceCGST_Amt = quotationViewModel.InsuranceCGST_Amt,
                    InsuranceSGST_Perc = quotationViewModel.InsuranceSGST_Perc,
                    InsuranceSGST_Amt = quotationViewModel.InsuranceSGST_Amt,
                    InsuranceIGST_Perc = quotationViewModel.InsuranceIGST_Perc,
                    InsuranceIGST_Amt = quotationViewModel.InsuranceIGST_Amt,


                    RtoRegsValue = quotationViewModel.RtoRegsValue,
                    RtoRegsCGST_Amt = quotationViewModel.RtoRegsCGST_Amt,
                    RtoRegsSGST_Amt = quotationViewModel.RtoRegsSGST_Amt,
                    RtoRegsIGST_Amt = quotationViewModel.RtoRegsIGST_Amt,
                    RtoRegsCGST_Perc = quotationViewModel.RtoRegsCGST_Perc,
                    RtoRegsSGST_Perc = quotationViewModel.RtoRegsSGST_Perc,
                    RtoRegsIGST_Perc = quotationViewModel.RtoRegsIGST_Perc,
                    VehicleInsuranceValue = quotationViewModel.VehicleInsuranceValue,

                    Remarks1 = quotationViewModel.Remarks1,
                    Remarks2 = quotationViewModel.Remarks2,

                    FinYearId = quotationViewModel.FinYearId,
                    CompanyId = quotationViewModel.CompanyId,
                    CreatedBy = quotationViewModel.CreatedBy,
                    ApprovalStatus = quotationViewModel.ApprovalStatus,
                    ReverseChargeApplicable = quotationViewModel.ReverseChargeApplicable,
                    ReverseChargeAmount = quotationViewModel.ReverseChargeAmount

                };
                List<QuotationProductDetail> quotationProductList = new List<QuotationProductDetail>();
                if (quotationProducts != null && quotationProducts.Count > 0)
                {
                    foreach (QuotationProductViewModel item in quotationProducts)
                    {
                        quotationProductList.Add(new QuotationProductDetail
                        {
                            ProductId = item.ProductId,
                            ProductShortDesc = item.ProductShortDesc,
                            Price = item.Price,
                            Quantity = item.Quantity,
                            DiscountPercentage = item.DiscountPercentage,
                            DiscountAmount = item.DiscountAmount,
                            TaxPercentage = item.TaxPercentage,
                            TaxAmount = item.TaxAmount,
                            TotalPrice = item.TotalPrice,
                            TaxId=item.TaxId,
                            TaxName=item.TaxName,
                            SurchargeName_1 = item.SurchargeName_1,
                            SurchargePercentage_1 = item.SurchargePercentage_1,
                            SurchargeAmount_1 = item.SurchargeAmount_1,
                            SurchargeName_2 = item.SurchargeName_2,
                            SurchargePercentage_2 = item.SurchargePercentage_2,
                            SurchargeAmount_2 = item.SurchargeAmount_2,
                            SurchargeName_3 = item.SurchargeName_3,
                            SurchargePercentage_3 = item.SurchargePercentage_3,
                            SurchargeAmount_3 = item.SurchargeAmount_3,
                            CGST_Perc = item.CGST_Perc,
                            CGST_Amount = item.CGST_Amount,
                            SGST_Perc = item.SGST_Perc,
                            SGST_Amount = item.SGST_Amount,
                            IGST_Perc = item.IGST_Perc,
                            IGST_Amount = item.IGST_Amount,
                            HSN_Code = item.HSN_Code
                        });
                    }
                }

                List<QuotationTaxDetail> quotationTaxList = new List<QuotationTaxDetail>();
                if (quotationTaxes != null && quotationTaxes.Count > 0)
                {
                    foreach (QuotationTaxViewModel item in quotationTaxes)
                    {
                        quotationTaxList.Add(new QuotationTaxDetail
                        {
                            TaxId = item.TaxId,
                            TaxName = item.TaxName,
                            TaxPercentage = item.TaxPercentage,
                            TaxAmount = item.TaxAmount,
                            SurchargeName_1 = item.SurchargeName_1,
                            SurchargePercentage_1 = item.SurchargePercentage_1,
                            SurchargeAmount_1 = item.SurchargeAmount_1,
                            SurchargeName_2 = item.SurchargeName_2,
                            SurchargePercentage_2 = item.SurchargePercentage_2,
                            SurchargeAmount_2 = item.SurchargeAmount_2,
                            SurchargeName_3 = item.SurchargeName_3,
                            SurchargePercentage_3 = item.SurchargePercentage_3,
                            SurchargeAmount_3 = item.SurchargeAmount_3,
                        });
                    }
                }
                List<QuotationTermsDetail> quotationTermList = new List<QuotationTermsDetail>();
                if (quotationTerms != null && quotationTerms.Count > 0)
                {
                    foreach (QuotationTermViewModel item in quotationTerms)
                    {
                        quotationTermList.Add(new QuotationTermsDetail
                        {
                            TermDesc = item.TermDesc,
                            TermSequence = item.TermSequence
                        });
                    }
                }
                List<QuotationSupportingDocument> quotationDocumentList = new List<QuotationSupportingDocument>();
                if (revisedQuotationDocuments != null && revisedQuotationDocuments.Count > 0)
                {
                    foreach (QuotationSupportingDocumentViewModel item in revisedQuotationDocuments)
                    {
                        quotationDocumentList.Add(new QuotationSupportingDocument
                        {
                            DocumentTypeId = item.DocumentTypeId,
                            DocumentName = item.DocumentName,
                            DocumentPath = item.DocumentPath
                        });
                    }
                }

                responseOut = sqlDbInterface.AddRevisedQuotation(quotation, quotationProductList, quotationTaxList, quotationTermList, quotationDocumentList);
                 

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
        #endregion 
    }
}
