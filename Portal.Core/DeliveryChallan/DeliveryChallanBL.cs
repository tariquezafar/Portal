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
    public class DeliveryChallanBL
    {
        DBInterface dbInterface;
        public DeliveryChallanBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditDeliveryChallan(DeliveryChallanViewModel challanViewModel, List<ChallanProductViewModel> challanProducts, List<ChallanTaxViewModel> challanTaxes, List<ChallanTermsViewModel> challanTerms, List<DeliveryChallanSupportingDocumentViewModel> deliveryChallanDocuments)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DeliveryChallan deliverychallan = new DeliveryChallan
                {
                    ChallanId = challanViewModel.ChallanId,
                    ChallanDate = Convert.ToDateTime(challanViewModel.ChallanDate),
                    InvoiceNo = challanViewModel.InvoiceNo,
                    InvoiceId = challanViewModel.InvoiceId,
                    CustomerId = challanViewModel.CustomerId,
                    CustomerName = challanViewModel.CustomerName,
                    ConsigneeId = challanViewModel.ConsigneeId,
                    ConsigeeName = challanViewModel.ConsigneeName,
                    ContactPerson = challanViewModel.ContactPerson,  
                    ShippingContactPerson = challanViewModel.ShippingContactPerson,
                    ShippingBillingAddress = challanViewModel.ShippingBillingAddress,
                    ShippingCity = challanViewModel.ShippingCity,
                    ShippingStateId = challanViewModel.ShippingStateId,
                    ShippingCountryId = challanViewModel.ShippingCountryId,
                    ShippingPinCode = challanViewModel.ShippingPinCode,
                    ShippingTINNo = challanViewModel.ShippingTINNo,
                    ShippingEmail = challanViewModel.ShippingEmail,
                    ShippingMobileNo = challanViewModel.ShippingMobileNo,
                    ShippingContactNo = challanViewModel.ShippingContactNo,
                    ShippingFax = challanViewModel.ShippingFax,
                    CompanyBranchId=challanViewModel.CompanyBranchId,
                    DispatchRefNo = challanViewModel.DispatchRefNo,
                    LRNo = challanViewModel.LRNo, 
                    LRDate = string.IsNullOrEmpty(challanViewModel.LRDate) ? Convert.ToDateTime("01-01-1900") : Convert.ToDateTime(challanViewModel.LRDate),
                    TransportVia = challanViewModel.TransportVia,
                    NoOfPackets = challanViewModel.NoOfPackets, 
                    DispatchRefDate = string.IsNullOrEmpty(challanViewModel.DispatchRefDate) ? Convert.ToDateTime("01-01-1900") : Convert.ToDateTime(challanViewModel.DispatchRefDate),
                    BasicValue = challanViewModel.BasicValue,
                    TotalValue = challanViewModel.TotalValue,
                    FreightValue = challanViewModel.FreightValue,
                    FreightCGST_Perc = challanViewModel.FreightCGST_Perc,
                    FreightCGST_Amt = challanViewModel.FreightCGST_Amt,
                    FreightSGST_Perc = challanViewModel.FreightSGST_Perc,
                    FreightSGST_Amt = challanViewModel.FreightSGST_Amt,
                    FreightIGST_Perc = challanViewModel.FreightIGST_Perc,
                    FreightIGST_Amt = challanViewModel.FreightIGST_Amt,

                    LoadingValue = challanViewModel.LoadingValue,
                    LoadingCGST_Perc = challanViewModel.LoadingCGST_Perc,
                    LoadingCGST_Amt = challanViewModel.LoadingCGST_Amt,
                    LoadingSGST_Perc = challanViewModel.LoadingSGST_Perc,
                    LoadingSGST_Amt = challanViewModel.LoadingSGST_Amt,
                    LoadingIGST_Perc = challanViewModel.LoadingIGST_Perc,
                    LoadingIGST_Amt = challanViewModel.LoadingIGST_Amt,
                    InsuranceValue = challanViewModel.InsuranceValue,
                    InsuranceCGST_Perc = challanViewModel.InsuranceCGST_Perc,
                    InsuranceCGST_Amt = challanViewModel.InsuranceCGST_Amt,
                    InsuranceSGST_Perc = challanViewModel.InsuranceSGST_Perc,
                    InsuranceSGST_Amt = challanViewModel.InsuranceSGST_Amt,
                    InsuranceIGST_Perc = challanViewModel.InsuranceIGST_Perc,
                    InsuranceIGST_Amt = challanViewModel.InsuranceIGST_Amt,
                    Remarks1 = challanViewModel.Remarks1,
                    Remarks2 = challanViewModel.Remarks2,
                    ApprovalStatus= challanViewModel.ApprovalStatus,
                    FinYearId = challanViewModel.FinYearId,
                    CompanyId = challanViewModel.CompanyId,
                    CreatedBy = challanViewModel.CreatedBy,
                    ReverseChargeApplicable = challanViewModel.ReverseChargeApplicable,
                    ReverseChargeAmount = challanViewModel.ReverseChargeAmount

                };
                List<ChallanProductDetail> challanProductList = new List<ChallanProductDetail>();
                if (challanProducts != null && challanProducts.Count > 0)
                {
                    foreach (ChallanProductViewModel item in challanProducts)
                    {
                        challanProductList.Add(new ChallanProductDetail
                        {
                            ProductId = item.ProductId,
                            ProductShortDesc = item.ProductShortDesc,
                            Price = item.Price,
                            Quantity = item.Quantity,
                            DiscountPercentage = item.DiscountPercentage,
                            DiscountAmount = item.DiscountAmount,
                            TaxId = item.TaxId,
                            TaxName = item.TaxName,
                            TaxPercentage = item.TaxPercentage,
                            TaxAmount = item.TaxAmount,
                            TotalPrice = item.TotalPrice,
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

                List<ChallanTaxDetail> challanTaxList = new List<ChallanTaxDetail>();
                if (challanTaxes != null && challanTaxes.Count > 0)
                {
                    foreach (ChallanTaxViewModel item in challanTaxes)
                    {
                        challanTaxList.Add(new ChallanTaxDetail
                        {
                            TaxId = item.TaxId,
                            TaxName = item.TaxName,
                            TaxPercentage = item.TaxPercentage,
                            TaxAmount = item.TaxAmount
                        });
                    }
                }
                List<ChallanTermsDetail> challanTermList = new List<ChallanTermsDetail>();
                if (challanTerms != null && challanTerms.Count > 0)
                {
                    foreach (ChallanTermsViewModel item in challanTerms)
                    {
                        challanTermList.Add(new ChallanTermsDetail
                        {
                            TermDesc = item.TermDesc,
                            TermSequence = item.TermSequence
                        });
                    }
                }


                List<DeliveryChallanSupportingDocument> deliveryChallanDocumentList = new List<DeliveryChallanSupportingDocument>();
                if (deliveryChallanDocuments != null && deliveryChallanDocuments.Count > 0)
                {
                    foreach (DeliveryChallanSupportingDocumentViewModel item in deliveryChallanDocuments)
                    {
                        deliveryChallanDocumentList.Add(new DeliveryChallanSupportingDocument
                        {
                            DocumentTypeId = item.DocumentTypeId,
                            DocumentName = item.DocumentName,
                            DocumentPath = item.DocumentPath
                        });
                    }
                }

                responseOut = sqlDbInterface.AddEditDeliveryChallan(deliverychallan, challanProductList, challanTaxList, challanTermList, deliveryChallanDocumentList); 

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
        
        public List<DeliveryChallanViewModel> GetChallanList(string deliverychallanNo, string customerName, string dispatchrefNo, string fromDate, string toDate, string approvalStatus, int companyId, string invoiceNo,string createdByUserName,int companyBranchId)
        {
            List<DeliveryChallanViewModel> deliverychallans = new List<DeliveryChallanViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtDeliveryChallans = sqlDbInterface.GetChallanList(deliverychallanNo, customerName, dispatchrefNo, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), approvalStatus, companyId, invoiceNo, createdByUserName, companyBranchId);
                if (dtDeliveryChallans != null && dtDeliveryChallans.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtDeliveryChallans.Rows)
                    {
                        deliverychallans.Add(new DeliveryChallanViewModel
                        {
                            ChallanId = Convert.ToInt32(dr["ChallanId"]),
                            ChallanNo = Convert.ToString(dr["ChallanNo"]),
                            ChallanDate = Convert.ToString(dr["ChallanDate"]),
                            InvoiceNo = Convert.ToString(dr["InvoiceNo"]),
                            CustomerId = Convert.ToInt32(dr["CustomerId"]),
                            CustomerCode = Convert.ToString(dr["CustomerCode"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            ShippingCity = Convert.ToString(dr["ShippingCity"]),
                            ShippingStateName = Convert.ToString(dr["StateName"]),
                            DispatchRefNo = Convert.ToString(dr["DispatchRefNo"]),
                            DispatchRefDate = Convert.ToString(dr["DispatchRefDate"]),
                            BasicValue = Convert.ToDecimal(dr["BasicValue"]),
                            TotalValue = Convert.ToDecimal(dr["TotalValue"]),
                            ApprovalStatus=Convert.ToString(dr["ApprovalStatus"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            CompanyBranchName=Convert.ToString(dr["CompanyBranchName"])

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return deliverychallans;
        }
        public DeliveryChallanViewModel GetChallanDetail(long challanId = 0)
        {
            DeliveryChallanViewModel deliverychallan = new DeliveryChallanViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCompanies = sqlDbInterface.GetChallanDetail(challanId);
                if (dtCompanies != null && dtCompanies.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCompanies.Rows)
                    {
                        deliverychallan = new DeliveryChallanViewModel
                        {
                            ChallanId = Convert.ToInt32(dr["ChallanId"]),
                            ChallanNo = Convert.ToString(dr["ChallanNo"]),
                            ChallanDate = Convert.ToString(dr["ChallanDate"]),
                            InvoiceId = Convert.ToInt32(dr["InvoiceId"]),
                            InvoiceNo = Convert.ToString(dr["InvoiceNo"]),
                            InvoiceDate = Convert.ToString(dr["InvoiceDate"]),

                            CustomerId = Convert.ToInt32(dr["CustomerId"]),
                            CustomerCode = Convert.ToString(dr["CustomerCode"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            ConsigneeId = Convert.ToInt32(dr["ConsigneeId"]),
                            ConsigneeCode = Convert.ToString(dr["ConsigneeCode"]),
                            ConsigneeName = Convert.ToString(dr["ConsigneeName"]),

                            ShippingContactPerson = Convert.ToString(dr["ShippingContactPerson"]),
                            ShippingBillingAddress = Convert.ToString(dr["ShippingBillingAddress"]),
                            ShippingCity = Convert.ToString(dr["ShippingCity"]),
                            ShippingStateId = Convert.ToInt32(dr["ShippingStateId"]),
                            ShippingCountryId = Convert.ToInt32(dr["ShippingCountryId"]),
                            ShippingPinCode = Convert.ToString(dr["ShippingPinCode"]),
                            ShippingTINNo = Convert.ToString(dr["ShippingTINNo"]),

                            ShippingEmail = Convert.ToString(dr["ShippingEmail"]),
                            ShippingMobileNo = Convert.ToString(dr["ShippingMobileNo"]),
                            ShippingContactNo = Convert.ToString(dr["ShippingContactNo"]),
                            ShippingFax = Convert.ToString(dr["ShippingFax"]),

                            CompanyBranchId = Convert.ToInt32(string.IsNullOrEmpty(dr["CompanyBranchId"].ToString())? "0": dr["CompanyBranchId"]),
                            CompanyBranchName = Convert.ToString(dr["CompanyBranchName"]),
                            CompanyBranchAddress = Convert.ToString(dr["CompanyBranchAddress"]),
                            CompanyBranchCity = Convert.ToString(dr["CompanyBranchCity"]),
                            CompanyBranchStateName = Convert.ToString(dr["CompanyBranchStateName"]),
                            CompanyBranchPinCode = Convert.ToString(dr["CompanyBranchPinCode"]),
                            CompanyBranchCSTNo = Convert.ToString(dr["CompanyBranchCSTNo"]),
                            CompanyBranchTINNo = Convert.ToString(dr["CompanyBranchTINNo"]),
                            DispatchRefNo = Convert.ToString(dr["DispatchRefNo"]),
                            DispatchRefDate = Convert.ToString(dr["DispatchRefDate"]),
                             
                            LRNo = Convert.ToString(dr["LRNo"]),
                            LRDate = Convert.ToString(dr["LRDate"]), 

                            TransportVia = Convert.ToString(dr["TransportVia"]),
                            NoOfPackets = Convert.ToInt32(dr["NoOfPackets"]), 

                            BasicValue = Convert.ToDecimal(dr["BasicValue"]),
                            TotalValue = Convert.ToDecimal(dr["TotalValue"]),

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

                            ReverseChargeApplicable = Convert.ToBoolean(dr["ReverseChargeApplicable"]),
                            ReverseChargeAmount = Convert.ToDecimal(dr["ReverseChargeAmount"]),
                            Remarks1 = Convert.ToString(dr["Remarks1"]),
                            Remarks2 = Convert.ToString(dr["Remarks2"]),

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
            return deliverychallan;
        }
    
       

  
        public List<ChallanProductViewModel> GetChallanProductList(long challanId)
        {
            List<ChallanProductViewModel> challanProducts = new List<ChallanProductViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCustomers = sqlDbInterface.GetChallanProductList(challanId);
                if (dtCustomers != null && dtCustomers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCustomers.Rows)
                    {
                        challanProducts.Add(new ChallanProductViewModel
                        {
                            ChallanProductDetailId = Convert.ToInt32(dr["ChallanProductDetailId"]),
                            SequenceNo=Convert.ToInt32(dr["SNo"]),
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
            return challanProducts;
        }

        public List<ChallanTaxViewModel> GetChallanTaxList(long challanId)
        {
            List<ChallanTaxViewModel> challanTaxes = new List<ChallanTaxViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCustomers = sqlDbInterface.GetChallanTaxList(challanId);
                if (dtCustomers != null && dtCustomers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCustomers.Rows)
                    {
                        challanTaxes.Add(new ChallanTaxViewModel
                        {
                            ChallanTaxDetailId = Convert.ToInt32(dr["ChallanTaxDetailId"]),
                            TaxSequenceNo = Convert.ToInt32(dr["SNo"]),
                            TaxId = Convert.ToInt32(dr["TaxId"]),
                            TaxName = Convert.ToString(dr["TaxName"]),
                            TaxPercentage = Convert.ToDecimal(dr["TaxPercentage"]),
                            TaxAmount = Convert.ToDecimal(dr["TaxAmount"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return challanTaxes;
        }
        public List<ChallanTermsViewModel> GetChallanTermList(long challanId)
        {
            List<ChallanTermsViewModel> challanTerms = new List<ChallanTermsViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCustomers = sqlDbInterface.GetChallanTermList(challanId);
                if (dtCustomers != null && dtCustomers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCustomers.Rows)
                    {
                        challanTerms.Add(new ChallanTermsViewModel
                        {
                            ChallanTermDetailId = Convert.ToInt32(dr["ChallanTermDetailId"]),
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
            return challanTerms;
        }

        
        public DataTable GetChallanDetailTable(long challanId=0)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtChallan = new DataTable();
            try
            {
                dtChallan = sqlDbInterface.GetChallanDetail(challanId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtChallan;
        }
        public DataTable GetChallanProductListDataTable(long challanId)
        {

            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtProducts = new DataTable();
            try
            {
                dtProducts = sqlDbInterface.GetChallanProductList(challanId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtProducts;
        }
        public DataTable GetChallanTaxListDataTable(long challanId)
        {

            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtTerms = new DataTable();
            try
            {
                dtTerms = sqlDbInterface.GetChallanTaxList(challanId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtTerms;
        }
        public DataTable GetChallanTermListDataTable(long challanId)
        {

            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtTerms = new DataTable();
            try
            {
                dtTerms = sqlDbInterface.GetChallanTermList(challanId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtTerms;
        }

        public List<DeliveryChallanSupportingDocumentViewModel> GetDeliveryChallanSupportingDocumentList(long challanId)
        {
            List<DeliveryChallanSupportingDocumentViewModel> deliveryChallanDocuments = new List<DeliveryChallanSupportingDocumentViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtDocument = sqlDbInterface.GetDeliveryChallanSupportingDocumentList(challanId);
                if (dtDocument != null && dtDocument.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtDocument.Rows)
                    {
                        deliveryChallanDocuments.Add(new DeliveryChallanSupportingDocumentViewModel
                        {
                            DeliveryChallanDocId = Convert.ToInt32(dr["DeliveryChallanDocId"]),
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
            return deliveryChallanDocuments;
        }


        public DataTable GetChallanProductSerialList(long challanId)
        {

            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtProducts = new DataTable();
            try
            {
                dtProducts = sqlDbInterface.GetChallanProductSerialList(challanId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtProducts;
        }

        public List<SaleInvoiceProductViewModel> GetChallanSaleInvoiceProductList(long saleinvoiceId)
        {
            List<SaleInvoiceProductViewModel> saleinvoiceProducts = new List<SaleInvoiceProductViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCustomers = sqlDbInterface.GetChallanSaleInvoiceProductList(saleinvoiceId);
                if (dtCustomers != null && dtCustomers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCustomers.Rows)
                    {
                        saleinvoiceProducts.Add(new SaleInvoiceProductViewModel
                        {
                            InvoiceProductDetailId = Convert.ToInt32(dr["InvoiceProductDetailId"]),
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
                            HSN_Code = Convert.ToString(dr["HSN_Code"]),
                            IsSerializedProduct = Convert.ToString(dr["IsSerializedProduct"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return saleinvoiceProducts;
        }
    }
}
