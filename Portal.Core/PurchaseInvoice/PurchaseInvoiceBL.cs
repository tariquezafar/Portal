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
 public class PurchaseInvoiceBL
    {
        DBInterface dbInterface;
        public PurchaseInvoiceBL()
        {
            dbInterface = new DBInterface();
        }
        public List<PurchaseInvoiceProductDetailViewModel> GetPIProductList(long InvoiceId)
        {
            List<PurchaseInvoiceProductDetailViewModel> piProducts = new List<PurchaseInvoiceProductDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtProducts = sqlDbInterface.GetPIProductList(InvoiceId);
                if (dtProducts != null && dtProducts.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtProducts.Rows)
                    {
                        piProducts.Add(new PurchaseInvoiceProductDetailViewModel
                        {
                            InvoiceProductDetailId = Convert.ToInt32(dr["InvoiceProductDetailId"]),
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
                            TaxName=Convert.ToString(dr["TaxName"]),
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
            return piProducts;
        }

        public List<PurchaseInvoiceProductDetailViewModel> GetPIMRNProductList(long InvoiceId)
        {
            List<PurchaseInvoiceProductDetailViewModel> piProducts = new List<PurchaseInvoiceProductDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtProducts = sqlDbInterface.GetPIMRNProductList(InvoiceId);
                if (dtProducts != null && dtProducts.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtProducts.Rows)
                    {
                        piProducts.Add(new PurchaseInvoiceProductDetailViewModel
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
                            ReceivedQuantity=Convert.ToDecimal(dr["ReceivedQuantity"]),
                            AcceptQuantity =Convert.ToDecimal(dr["AcceptQuantity"]),
                            RejectQuantity = Convert.ToDecimal(dr["RejectQuantity"]),
                            DiscountPercentage = Convert.ToDecimal(dr["DiscountPercentage"]),
                            DiscountAmount = Convert.ToDecimal(dr["DiscountAmount"]),
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
            return piProducts;
        }
        public List<PurchaseInvoiceTaxDetailViewModel> GetPITaxList(long InvoiceId)
        {
            List<PurchaseInvoiceTaxDetailViewModel> poTaxes = new List<PurchaseInvoiceTaxDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtTaxs = sqlDbInterface.GetPITaxList(InvoiceId);
                if (dtTaxs != null && dtTaxs.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtTaxs.Rows)
                    {
                        poTaxes.Add(new PurchaseInvoiceTaxDetailViewModel
                        {
                            InvoiceTaxDetailId = Convert.ToInt32(dr["InvoiceTaxDetailId"]),
                            TaxSequenceNo = Convert.ToInt32(dr["SNo"]),
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
            return poTaxes;
        }
        public List<PurchaseInvoiceTermsDetailViewModel> GetPITermList(long InvoiceId)
        {
            List<PurchaseInvoiceTermsDetailViewModel> piTerms = new List<PurchaseInvoiceTermsDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtTerms = sqlDbInterface.GetPITermList(InvoiceId);
                if (dtTerms != null && dtTerms.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtTerms.Rows)
                    {
                        piTerms.Add(new PurchaseInvoiceTermsDetailViewModel
                        {
                            InvoiceTermDetailId = Convert.ToInt32(dr["InvoiceTermDetailId"]),
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
            return piTerms;
        }
       
        public ResponseOut AddEditPI(PurchaseInvoiceViewModel piViewModel, List<PurchaseInvoiceProductDetailViewModel> piProducts, List<PurchaseInvoiceTaxDetailViewModel> piTaxes, List<PurchaseInvoiceTermsDetailViewModel> piTerms, List<PISupportingDocumentViewModel> piDocuments,List<PurchaseInvoiceChasisDetailViewModel> piChasisLists)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                PurchaseInvoice pi = new PurchaseInvoice {
                    InvoiceId = piViewModel.InvoiceId,
                    InvoiceDate = Convert.ToDateTime(piViewModel.InvoiceDate),
                    SaleInvoiceInvoiceId= piViewModel.SaleInvoiceInvoiceId,
                    SaleInvoiceInvoiceNo = piViewModel.SaleInvoiceInvoiceNo,
                    POId = piViewModel.POId,
                    PONo = piViewModel.PONo,
                    CurrencyCode = piViewModel.CurrencyCode,
                    VendorId = piViewModel.VendorId,
                    VendorName = piViewModel.VendorName,
                    BillingAddress = piViewModel.BillingAddress,
                    ShippingAddress = piViewModel.ShippingAddress,
                    City = piViewModel.City,
                    StateId = piViewModel.StateId,
                    CountryId = piViewModel.CountryId,
                    PinCode = string.IsNullOrEmpty(piViewModel.PinCode)?"":Convert.ToString(piViewModel.PinCode),
                    CSTNo = string.IsNullOrEmpty(piViewModel.CSTNo) ? "" : Convert.ToString(piViewModel.CSTNo),
                    TINNo = string.IsNullOrEmpty(piViewModel.CSTNo) ? "" : Convert.ToString(piViewModel.TINNo),
                    PANNo = string.IsNullOrEmpty(piViewModel.PANNo) ? "" : Convert.ToString(piViewModel.PANNo),
                    GSTNo = string.IsNullOrEmpty(piViewModel.GSTNo) ? "" : Convert.ToString(piViewModel.GSTNo), 
                    ExciseNo = piViewModel.ExciseNo,
                    ApprovalStatus =piViewModel.ApprovalStatus,
                    RefNo = string.IsNullOrEmpty(piViewModel.RefNo) ? "" : piViewModel.RefNo,
                    RefDate = string.IsNullOrEmpty(piViewModel.RefDate) ? Convert.ToDateTime("01-01-1900") : Convert.ToDateTime(piViewModel.RefDate),
                    BasicValue = piViewModel.BasicValue,
                    TotalValue = piViewModel.TotalValue,

                    ConsigneeId = piViewModel.ConsigneeId,
                    ConsigneeName = piViewModel.ConsigneeName,

                    ShippingCity = piViewModel.ShippingCity,
                    ShippingStateId = piViewModel.ShippingStateId,
                    ShippingCountryId = piViewModel.ShippingCountryId,
                    ShippingPinCode = piViewModel.ShippingPinCode,
                    ConsigneeGSTNo = piViewModel.ConsigneeGSTNo,

                    FreightValue = piViewModel.FreightValue,
                    FreightCGST_Perc = piViewModel.FreightCGST_Perc,
                    FreightCGST_Amt = piViewModel.FreightCGST_Amt,
                    FreightSGST_Perc = piViewModel.FreightSGST_Perc,
                    FreightSGST_Amt = piViewModel.FreightSGST_Amt,
                    FreightIGST_Perc = piViewModel.FreightIGST_Perc,
                    FreightIGST_Amt = piViewModel.FreightIGST_Amt,

                    LoadingValue = piViewModel.LoadingValue,
                    LoadingCGST_Perc = piViewModel.LoadingCGST_Perc,
                    LoadingCGST_Amt = piViewModel.LoadingCGST_Amt,
                    LoadingSGST_Perc = piViewModel.LoadingSGST_Perc,
                    LoadingSGST_Amt = piViewModel.LoadingSGST_Amt,
                    LoadingIGST_Perc = piViewModel.LoadingIGST_Perc,
                    LoadingIGST_Amt = piViewModel.LoadingIGST_Amt,
                    InsuranceValue = piViewModel.InsuranceValue,
                    InsuranceCGST_Perc = piViewModel.InsuranceCGST_Perc,
                    InsuranceCGST_Amt = piViewModel.InsuranceCGST_Amt,
                    InsuranceSGST_Perc = piViewModel.InsuranceSGST_Perc,
                    InsuranceSGST_Amt = piViewModel.InsuranceSGST_Amt,
                    InsuranceIGST_Perc = piViewModel.InsuranceIGST_Perc,
                    InsuranceIGST_Amt = piViewModel.InsuranceIGST_Amt,

                    Remarks = string.IsNullOrEmpty(piViewModel.Remarks)?"":piViewModel.Remarks,
                    FinYearId = piViewModel.FinYearId,
                    CompanyId = piViewModel.CompanyId,
                    CreatedBy = piViewModel.CreatedBy,
                    ReverseChargeApplicable = piViewModel.ReverseChargeApplicable,
                    ReverseChargeAmount = piViewModel.ReverseChargeAmount,
                    RoundOfValue = piViewModel.RoundOfValue,
                    GrossValue = piViewModel.GrossValue,
                    PurchaseType=piViewModel.PurchaseType,
                    CompanyBranchId= piViewModel.CompanyBranchId,
                    SICompanyBranchId = piViewModel.SICompanyBranchId
                };
                
                List<PurchaseInvoiceProductDetail> piProductList = new List<PurchaseInvoiceProductDetail>();
                if (piProducts != null && piProducts.Count > 0)
                {
                    foreach (PurchaseInvoiceProductDetailViewModel item in piProducts)
                    {
                        piProductList.Add(new PurchaseInvoiceProductDetail
                        {
                            ProductId = item.ProductId,
                            ProductShortDesc = item.ProductShortDesc,
                            Price = item.Price,
                            Quantity = item.Quantity,
                            ReceivedQuantity=0,
                            AcceptQuantity=0,
                            RejectQuantity=0,
                            DiscountPercentage = item.DiscountAmount,
                            DiscountAmount = item.DiscountAmount,
                            TaxId=item.TaxId,
                            TaxName=item.TaxName,
                            TaxPercentage = item.TaxPercentage,
                            TaxAmount = item.TaxAmount,
                            TotalPrice = item.TotalPrice,
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
               
                List<PurchaseInvoiceTaxDetail> piTaxList = new List<PurchaseInvoiceTaxDetail>();
                if (piTaxes != null && piTaxes.Count > 0)
                {
                    foreach (PurchaseInvoiceTaxDetailViewModel item in piTaxes)
                    {
                        piTaxList.Add(new PurchaseInvoiceTaxDetail
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
                List<PurchaseInvoiceTermsDetail> piTermList = new List<PurchaseInvoiceTermsDetail>();
                if (piTerms != null && piTerms.Count > 0)
                {
                    foreach (PurchaseInvoiceTermsDetailViewModel item in piTerms)
                    {
                        piTermList.Add(new PurchaseInvoiceTermsDetail
                        {
                            TermDesc = item.TermDesc,
                            TermSequence = item.TermSequence
                        });
                    }
                }

                List<PISupportingDocument> piDocumentList = new List<PISupportingDocument>();
                if (piDocuments != null && piDocuments.Count > 0)
                {
                    foreach (PISupportingDocumentViewModel item in piDocuments)
                    {
                        piDocumentList.Add(new PISupportingDocument
                        {
                            DocumentTypeId = item.DocumentTypeId,
                            DocumentName = item.DocumentName,
                            DocumentPath = item.DocumentPath
                        });
                    }
                }


                List<PurchaseInvoiceChasisDetail> piChasisList = new List<PurchaseInvoiceChasisDetail>();
                if (piChasisLists != null && piChasisLists.Count > 0)
                {
                    foreach (PurchaseInvoiceChasisDetailViewModel item in piChasisLists)
                    {
                        piChasisList.Add(new PurchaseInvoiceChasisDetail
                        {
                            ProductId = item.ProductId,
                            ChasisSerialNo = item.ChasisSerialNo
                          
                        });
                    }
                }

                responseOut = sqlDbInterface.AddEditPI(pi,piProductList,piTaxList, piTermList, piDocumentList, piChasisList);
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

        public List<PurchaseInvoiceViewModel> GetPIList(string piNo, string vendorName, string refNo, string fromDate, string toDate, int companyId, string approvalStatus="", string displayType="",string vendorCode="",string purchaseType= "",string CreatedByUserName="",string poNo="",string companyBranch="", string MRNNo="")
        {
            List<PurchaseInvoiceViewModel> pos = new List<PurchaseInvoiceViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtPIs = sqlDbInterface.GetPIList(piNo, vendorName, refNo, fromDate, toDate, companyId, approvalStatus, displayType, vendorCode, purchaseType, CreatedByUserName, poNo, companyBranch, MRNNo);
                if (dtPIs != null && dtPIs.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPIs.Rows)
                    {
                        pos.Add(new PurchaseInvoiceViewModel
                        {
                            InvoiceId = Convert.ToInt32(dr["InvoiceId"]),
                            InvoiceNo = Convert.ToString(dr["InvoiceNo"]),
                            InvoiceDate = Convert.ToString(dr["InvoiceDate"]),
                            PONo = Convert.ToString(dr["PONo"]),
                            VendorId = Convert.ToInt32(dr["VendorId"]),
                            VendorName = Convert.ToString(dr["VendorName"]),
                            VendorCode = Convert.ToString(dr["VendorCode"]),
                            City = Convert.ToString(dr["City"]),
                            StateName = Convert.ToString(dr["StateName"]),
                            RefNo = Convert.ToString(dr["RefNo"]),
                            PurchaseType = Convert.ToString(dr["PurchaseType"]),
                            RefDate = Convert.ToString(dr["RefDate"]),
                            BasicValue = Convert.ToDecimal(dr["BasicValue"]),
                            TotalValue = Convert.ToDecimal(dr["TotalValue"]),
                            ApprovalStatus=Convert.ToString(dr["ApprovalStatus"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            companyBranch= Convert.ToString(dr["BranchName"]),
                            MRNNO = Convert.ToString(dr["MRNNO"]),
                            MRNDate= Convert.ToString(dr["MRNDate"]),
                            MRNId = Convert.ToInt32(dr["MRNId"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return pos;
        }

        public List<PurchaseInvoiceViewModel> GetPIAndPIImportMergeList(string piNo, string vendorName, string refNo, string fromDate, string toDate, int companyId, string approvalStatus = "", string displayType = "", string vendorCode = "", string purchaseType = "", string CreatedByUserName = "", string poNo = "", string companyBranch = "")
        {
            List<PurchaseInvoiceViewModel> pos = new List<PurchaseInvoiceViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtPIs = sqlDbInterface.GetPIAndPIImportMergeList(piNo, vendorName, refNo, fromDate, toDate, companyId, approvalStatus, displayType, vendorCode, purchaseType, CreatedByUserName, poNo, companyBranch);
                if (dtPIs != null && dtPIs.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPIs.Rows)
                    {
                        pos.Add(new PurchaseInvoiceViewModel
                        {
                            InvoiceId = Convert.ToInt32(dr["InvoiceId"]),
                            InvoiceNo = Convert.ToString(dr["InvoiceNo"]),
                            InvoiceDate = Convert.ToString(dr["InvoiceDate"]),
                            PONo = Convert.ToString(dr["PONo"]),
                            VendorId = Convert.ToInt32(dr["VendorId"]),
                            VendorName = Convert.ToString(dr["VendorName"]),
                            VendorCode = Convert.ToString(dr["VendorCode"]),
                            City = Convert.ToString(dr["City"]),
                            StateName = Convert.ToString(dr["StateName"]),
                            RefNo = Convert.ToString(dr["RefNo"]),
                            PurchaseType = Convert.ToString(dr["PurchaseType"]),
                            RefDate = Convert.ToString(dr["RefDate"]),
                            BasicValue = Convert.ToDecimal(dr["BasicValue"]),
                            TotalValue = Convert.ToDecimal(dr["TotalValue"]),
                            ApprovalStatus = Convert.ToString(dr["ApprovalStatus"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            companyBranch = Convert.ToString(dr["BranchName"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return pos;
        }


        public PurchaseInvoiceViewModel GetPIDetail(long invoiceId = 0)
        {
            PurchaseInvoiceViewModel pi = new PurchaseInvoiceViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtpos = sqlDbInterface.GetPIDetail(invoiceId);
                if (dtpos != null && dtpos.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtpos.Rows)
                    {
                        pi = new PurchaseInvoiceViewModel
                        {
                          InvoiceId = Convert.ToInt32(dr["InvoiceId"]),
                          InvoiceNo =Convert.ToString(dr["InvoiceNo"]),
                          InvoiceDate = Convert.ToString(dr["InvoiceDate"]),
                          POId =Convert.ToInt32(dr["POId"]),
                          PONo = Convert.ToString(dr["PONo"]),


                          SaleInvoiceInvoiceId = Convert.ToInt32(dr["SaleInvoiceInvoiceId"]),
                          SaleInvoiceInvoiceNo = Convert.ToString(dr["SaleInvoiceInvoiceNo"]),

                          PODate = Convert.ToString(dr["PODate"]),
                          CurrencyCode = Convert.ToString(dr["CurrencyCode"]),
                          VendorId = Convert.ToInt32(dr["VendorId"]),
                          VendorCode = Convert.ToString(dr["VendorCode"]),
                          VendorName = Convert.ToString(dr["VendorName"]),

                          ConsigneeName = Convert.ToString(dr["ConsigneeName"]),
                          ConsigneeId = Convert.ToInt32(dr["ConsigneeId"]),
                          ConsigneeCode = Convert.ToString(dr["ConsigneeCode"]),
                          BillingAddress = Convert.ToString(dr["BillingAddress"]),
                          ShippingAddress = Convert.ToString(dr["ShippingAddress"]),
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
                          BasicValue = Convert.ToDecimal(dr["BasicValue"]),
                          TotalValue = Convert.ToDecimal(dr["TotalValue"]),
                          GrossValue = Convert.ToDecimal(dr["GrossValue"]),
                          RoundOfValue= Convert.ToDecimal(dr["RoundOfValue"]),
                          PurchaseType= Convert.ToString(dr["PurchaseType"]),

                            ShippingCity = Convert.ToString(dr["ShippingCity"]),
                            ShippingStateId = Convert.ToInt32(dr["ShippingStateId"]),
                            ShippingCountryId = Convert.ToInt32(dr["ShippingCountryId"]),
                            ShippingPinCode = Convert.ToString(dr["ShippingPinCode"]),
                            ConsigneeGSTNo = Convert.ToString(dr["ConsigneeGSTNo"]),
                            ReverseChargeApplicable = Convert.ToBoolean(dr["ReverseChargeApplicable"]),
                            ReverseChargeAmount = Convert.ToDecimal(dr["ReverseChargeAmount"]),
                            FreightValue = Convert.ToDecimal(dr["FreightValue"]),
                            FreightCGST_Amt = Convert.ToDecimal(dr["FreightCGST_Amt"]),
                            FreightSGST_Amt = Convert.ToDecimal(dr["FreightSGST_Amt"]),
                            FreightIGST_Amt = Convert.ToDecimal(dr["FreightIGST_Amt"]),

                            FreightCGST_Perc = Convert.ToDecimal(dr["FreightCGST_Perc"]),
                            FreightSGST_Perc = Convert.ToDecimal(dr["FreightSGST_Perc"]),
                            FreightIGST_Perc = Convert.ToDecimal(dr["FreightIGST_Perc"]),

                            LoadingValue = Convert.ToDecimal(dr["LoadingValue"]),
                            LoadingCGST_Amt = Convert.ToDecimal(dr["LoadingCGST_Amt"]),
                            LoadingSGST_Amt = Convert.ToDecimal(dr["LoadingSGST_Amt"]),
                            LoadingIGST_Amt = Convert.ToDecimal(dr["LoadingIGST_Amt"]),

                            LoadingCGST_Perc = Convert.ToDecimal(dr["LoadingCGST_Perc"]),
                            LoadingSGST_Perc = Convert.ToDecimal(dr["LoadingSGST_Perc"]),
                            LoadingIGST_Perc = Convert.ToDecimal(dr["LoadingIGST_Perc"]),

                            InsuranceValue = Convert.ToDecimal(dr["InsuranceValue"]),
                            InsuranceCGST_Amt = Convert.ToDecimal(dr["InsuranceCGST_Amt"]),
                            InsuranceSGST_Amt = Convert.ToDecimal(dr["InsuranceSGST_Amt"]),
                            InsuranceIGST_Amt = Convert.ToDecimal(dr["InsuranceIGST_Amt"]),

                            InsuranceCGST_Perc = Convert.ToDecimal(dr["InsuranceCGST_Perc"]),
                            InsuranceSGST_Perc = Convert.ToDecimal(dr["InsuranceSGST_Perc"]),
                            InsuranceIGST_Perc = Convert.ToDecimal(dr["InsuranceIGST_Perc"]),

                          Remarks = Convert.ToString(dr["Remarks"]),
                          ApprovalStatus = Convert.ToString(dr["ApprovalStatus"]),
                          CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                          CreatedDate = Convert.ToString(dr["CreatedDate"]),
                          ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                          ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                          GST_Exempt = Convert.ToBoolean(dr["GST_Exempt"]),
                          CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                          SICompanyBranchId = Convert.ToInt32(dr["SICompanyBranchId"])

                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return pi;
        }

        public DataTable GetPIDetailDataTable(long piId = 0)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtPI = new DataTable();
            try
            {
                dtPI = sqlDbInterface.GetPIDetail(piId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtPI;
        }
        public DataTable GetPIProductListDataTable(long piId)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtProducts = new DataTable();
            try
            {
                dtProducts = sqlDbInterface.GetPIProductList(piId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtProducts;
        }
        public DataTable GetPITermListDataTable(long piId)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtTerms = new DataTable();
            try
            {
                dtTerms = sqlDbInterface.GetPITermList(piId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtTerms;
        }
        public DataTable GetPITaxDataTable(long piId)
        {

            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtTaxs = new DataTable();
            try
            {
                dtTaxs = sqlDbInterface.GetPITaxList(piId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtTaxs;
        }


        public List<PISupportingDocumentViewModel> GetPISupportingDocumentList(long piId)
        {
            List<PISupportingDocumentViewModel> piDocuments = new List<PISupportingDocumentViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtDocument = sqlDbInterface.GetPISupportingDocumentList(piId);
                if (dtDocument != null && dtDocument.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtDocument.Rows)
                    {
                        piDocuments.Add(new PISupportingDocumentViewModel
                        {
                            InvoiceDocId = Convert.ToInt32(dr["InvoiceDocId"]),
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
            return piDocuments;
        }
         


        public ResponseOut CancelPI(PurchaseInvoiceViewModel purchaseinvoiceViewModel)
        {

            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            ResponseOut responseOut = new ResponseOut();
            try
            { 
                PurchaseInvoice pi = new PurchaseInvoice
                {
                    InvoiceId = purchaseinvoiceViewModel.InvoiceId,
                    InvoiceNo=purchaseinvoiceViewModel.InvoiceNo,
                    CancelStatus = "Cancel",
                    ApprovalStatus = "Cancelled",
                    CreatedBy = purchaseinvoiceViewModel.CreatedBy,
                    CompanyId= purchaseinvoiceViewModel.CompanyId,
                    FinYearId= purchaseinvoiceViewModel.FinYearId,
                    CancelReason = purchaseinvoiceViewModel.CancelReason
                };
                responseOut = sqlDbInterface.CancelPI(pi);
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




        public List<PurchaseInvoiceViewModel> GetPIPrpductPurchaseList(long productID, long companyBranchId)
        {
            List<PurchaseInvoiceViewModel> pis = new List<PurchaseInvoiceViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtPOs = sqlDbInterface.GetPIPrpductPurchaseList(productID, companyBranchId);
                if (dtPOs != null && dtPOs.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPOs.Rows)
                    {
                        pis.Add(new PurchaseInvoiceViewModel
                        {

                            InvoiceNo = Convert.ToString(dr["InvoiceNo"]),
                            InvoiceDate = Convert.ToString(dr["InvoiceDate"]),
                            VendorName = Convert.ToString(dr["VendorName"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            Price = Convert.ToDecimal(dr["Price"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return pis;
        }


        public List<SaleInvoiceProductSerialDetailViewModel> GetSIChaisList(long invoiceId = 0,string mode="")
        {
            List<SaleInvoiceProductSerialDetailViewModel> saleInvoiceProductSerialDetailList = new List<SaleInvoiceProductSerialDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtSaleInvoiceProductSerials = sqlDbInterface.GetSaleInvoiceChaisList(invoiceId, mode);

                if (dtSaleInvoiceProductSerials != null && dtSaleInvoiceProductSerials.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtSaleInvoiceProductSerials.Rows)
                    {
                        saleInvoiceProductSerialDetailList.Add(new SaleInvoiceProductSerialDetailViewModel
                        {
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            RefSerial1 = Convert.ToString(dr["RefSerial1"]),
                        });
                    }

                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return saleInvoiceProductSerialDetailList;
        }

    }
}
