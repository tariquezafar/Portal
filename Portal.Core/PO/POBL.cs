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
 public class POBL
    {
        DBInterface dbInterface;
        public POBL()
        {
            dbInterface = new DBInterface();
        }

        #region PO
        public List<POProductViewModel> GetPOProductList(long poId)
        {
            List<POProductViewModel> quotationProducts = new List<POProductViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtProducts = sqlDbInterface.GetPOProductList(poId);
                if (dtProducts != null && dtProducts.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtProducts.Rows)
                    {
                        quotationProducts.Add(new POProductViewModel
                        {
                            POProductDetailId = Convert.ToInt32(dr["POProductDetailId"]),
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
                            ExpectedDeliveryDate = Convert.ToString(dr["ExpectedDeliveryDate"])
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

        public List<POScheduleViewModel> GetPOScheduleList(long poId)
        {
            List<POScheduleViewModel> pOSchedule = new List<POScheduleViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtPOSchedule = sqlDbInterface.GetPOScheduleList(poId);
                if (dtPOSchedule != null && dtPOSchedule.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPOSchedule.Rows)
                    {
                        pOSchedule.Add(new POScheduleViewModel
                        {
                            POScheduleId = Convert.ToInt32(dr["PoProductScheduleId"]),
                            SequenceNo = Convert.ToInt32(dr["SequenceNo"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                            Location = Convert.ToString(dr["LocationName"]),
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ProductCode = Convert.ToString(dr["ProductCode"]),
                            Unit = Convert.ToString(dr["UOMName"]),
                            OrderQuantity = Convert.ToDecimal(dr["Quantity"]),
                            DeliveryDate = Convert.ToString(dr["DeliveryDate"]),
                            SchQuantity = Convert.ToDecimal(dr["SchQuantity"]),
                            ConDeliveryDate = Convert.ToString(dr["ConDeliveryDate"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return pOSchedule;
        }

        public List<POTaxViewModel> GetPOTaxList(long poId)
        {
            List<POTaxViewModel> poTaxes = new List<POTaxViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtTaxs = sqlDbInterface.GetPOTaxList(poId);
                if (dtTaxs != null && dtTaxs.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtTaxs.Rows)
                    {
                        poTaxes.Add(new POTaxViewModel
                        {
                            POTaxDetailId = Convert.ToInt32(dr["POTaxDetailId"]),
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
        public List<POSupportingDocumentViewModel> GetPOSupportingDocumentList(long poId)
        {
            List<POSupportingDocumentViewModel> poDocuments= new List<POSupportingDocumentViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtDocument = sqlDbInterface.GetPOSupportingDocumentList(poId);
                if (dtDocument != null && dtDocument.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtDocument.Rows)
                    {
                        poDocuments.Add(new POSupportingDocumentViewModel
                        {
                            PODocId = Convert.ToInt32(dr["PODocId"]),
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
            return poDocuments;
        }
        public List<POTermViewModel> GetPoTermList(long poId)
        {
            List<POTermViewModel> poTerms = new List<POTermViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtTerms = sqlDbInterface.GetPOTermList(poId);
                if (dtTerms != null && dtTerms.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtTerms.Rows)
                    {
                        poTerms.Add(new POTermViewModel
                        {
                            POTermDetailId = Convert.ToInt32(dr["POTermDetailId"]),
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
            return poTerms;
        }

        public List<POTermViewModel> GetPOTermsList(long poId)
        {
            List<POTermViewModel> poTerms = new List<POTermViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtTerms = sqlDbInterface.GetPOTermList(poId);
                if (dtTerms != null && dtTerms.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtTerms.Rows)
                    {
                        poTerms.Add(new POTermViewModel
                        {
                            POTermDetailId = Convert.ToInt32(dr["POTermDetailId"]),
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
            return poTerms;
        }

        public ResponseOut AddEditPO(POViewModel poViewModel, List<POProductViewModel> poProducts, List<POTaxViewModel> poTaxes, List<POTermViewModel> poTerms, List<POSupportingDocumentViewModel> poDocuments, List<POScheduleViewModel> poSchedules)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                PO po = new PO
                {
                    POId = poViewModel.POId,
                    PODate = Convert.ToDateTime(poViewModel.PODate),
                    IndentId = poViewModel.IndentId,
                    IndentNo = poViewModel.IndentNo,
                    QuotationId = poViewModel.QuotationId,
                    QuotationNo= poViewModel.QuotationNo,
                    CurrencyCode = poViewModel.CurrencyCode,
                    VendorId = poViewModel.VendorId,
                    VendorName = poViewModel.VendorName,
                    BillingAddress = poViewModel.BillingAddress,
                    ShippingAddress= poViewModel.ShippingAddress,
                    City = poViewModel.City,
                    StateId = poViewModel.StateId,
                    CountryId = poViewModel.CountryId,
                    PinCode = poViewModel.PinCode,
                    CSTNo = poViewModel.CSTNo,
                    TINNo = poViewModel.TINNo,
                    PANNo = poViewModel.PANNo,
                    GSTNo = poViewModel.GSTNo,
                    ExciseNo = poViewModel.ExciseNo,
                    //ApprovalStatus=poViewModel.ApprovalStatus,
                    POStatus = poViewModel.POStatus,

                    RefNo = string.IsNullOrEmpty(poViewModel.RefNo) ? "" : poViewModel.RefNo,
                    RefDate = string.IsNullOrEmpty(poViewModel.RefDate) ? Convert.ToDateTime("01-01-1900") : Convert.ToDateTime(poViewModel.RefDate),
                    BasicValue = poViewModel.BasicValue,
                    TotalValue = poViewModel.TotalValue,

                    ConsigneeId = poViewModel.ConsigneeId,
                    ConsigneeName = poViewModel.ConsigneeName,

                    ShippingCity = poViewModel.ShippingCity,
                    ShippingStateId = poViewModel.ShippingStateId,
                    ShippingCountryId = poViewModel.ShippingCountryId,
                    ShippingPinCode = poViewModel.ShippingPinCode,
                    ConsigneeGSTNo = poViewModel.ConsigneeGSTNo,               

                    FreightValue = poViewModel.FreightValue,
                    FreightCGST_Perc = poViewModel.FreightCGST_Perc,
                    FreightCGST_Amt = poViewModel.FreightCGST_Amt,
                    FreightSGST_Perc = poViewModel.FreightSGST_Perc,
                    FreightSGST_Amt = poViewModel.FreightSGST_Amt,
                    FreightIGST_Perc = poViewModel.FreightIGST_Perc,
                    FreightIGST_Amt = poViewModel.FreightIGST_Amt,

                    LoadingValue = poViewModel.LoadingValue,
                    LoadingCGST_Perc = poViewModel.LoadingCGST_Perc,
                    LoadingCGST_Amt = poViewModel.LoadingCGST_Amt,
                    LoadingSGST_Perc = poViewModel.LoadingSGST_Perc,
                    LoadingSGST_Amt = poViewModel.LoadingSGST_Amt,
                    LoadingIGST_Perc = poViewModel.LoadingIGST_Perc,
                    LoadingIGST_Amt = poViewModel.LoadingIGST_Amt,
                    InsuranceValue = poViewModel.InsuranceValue,
                    InsuranceCGST_Perc = poViewModel.InsuranceCGST_Perc,
                    InsuranceCGST_Amt = poViewModel.InsuranceCGST_Amt,
                    InsuranceSGST_Perc = poViewModel.InsuranceSGST_Perc,
                    InsuranceSGST_Amt = poViewModel.InsuranceSGST_Amt,
                    InsuranceIGST_Perc = poViewModel.InsuranceIGST_Perc,
                    InsuranceIGST_Amt = poViewModel.InsuranceIGST_Amt,
                    ExpectedDeliveryDate = string.IsNullOrEmpty(poViewModel.ExpectedDeliveryDate) ? Convert.ToDateTime("01-01-1900") : Convert.ToDateTime(poViewModel.ExpectedDeliveryDate),
                    Remarks1 = string.IsNullOrEmpty(poViewModel.Remarks1)?null:poViewModel.Remarks1,
                    Remarks2 = string.IsNullOrEmpty(poViewModel.Remarks2)?null:poViewModel.Remarks2,
                    FinYearId = poViewModel.FinYearId,
                    CompanyId = poViewModel.CompanyId,
                    CreatedBy = poViewModel.CreatedBy,
                    ReverseChargeApplicable = poViewModel.ReverseChargeApplicable,
                    ReverseChargeAmount = poViewModel.ReverseChargeAmount,
                    CompanyBranchId=poViewModel.CompanyBranchId,
                    POType=poViewModel.POType,
                    CurrencyConversionRate=poViewModel.CurrencyConversionRate,

                };
                List<POProductDetail> poProductList = new List<POProductDetail>();
                if (poProducts != null && poProducts.Count > 0)
                {
                    foreach (POProductViewModel item in poProducts)
                    {
                        poProductList.Add(new POProductDetail
                        {
                            ProductId = item.ProductId,
                            ProductShortDesc = item.ProductShortDesc,
                            Price = item.Price,
                            Quantity = item.Quantity,
                            DiscountPercentage = item.DiscountPercentage,
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
                            HSN_Code = item.HSN_Code,
                            ExpectedDeliveryDate = Convert.ToDateTime(item.ExpectedDeliveryDate)

                        });
                    }
                }

                List<POProductSchedule> poSchedulesList = new List<POProductSchedule>();
                if (poSchedules != null && poSchedules.Count > 0)
                {
                    foreach (POScheduleViewModel item in poSchedules)
                    {
                        poSchedulesList.Add(new POProductSchedule
                        {
                            PoProductScheduleId = item.POScheduleId,
                            POId = poViewModel.POId,
                            ProductId = item.ProductId,
                            ProductName = item.ProductName,
                            ProductCode = item.ProductCode,
                            CompanyBranchId = poViewModel.CompanyBranchId,
                            LocationName = item.Location,
                            Quantity = item.OrderQuantity,
                            UOMName = item.Unit,
                            SchQuantity = item.SchQuantity,
                            DeliveryDate = Convert.ToDateTime(item.DeliveryDate),
                            ConDeliveryDate = Convert.ToDateTime(item.ConDeliveryDate)
                        });
                    }
                }

                List <POTaxDetail> poTaxList = new List<POTaxDetail>();
                if (poTaxes != null && poTaxes.Count > 0)
                {
                    foreach (POTaxViewModel item in poTaxes)
                    {
                        poTaxList.Add(new POTaxDetail
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
                List<POTermsDetail> poTermList = new List<POTermsDetail>();
                if (poTerms != null && poTerms.Count > 0)
                {
                    foreach (POTermViewModel item in poTerms)
                    {
                        poTermList.Add(new POTermsDetail
                        {
                            TermDesc = item.TermDesc,
                            TermSequence = item.TermSequence
                        });
                    }
                }
                List<POSupportingDocument> poDocumentList = new List<POSupportingDocument>();
                if (poDocuments != null && poDocuments.Count > 0)
                {
                    foreach (POSupportingDocumentViewModel item in poDocuments)
                    {
                        poDocumentList.Add(new POSupportingDocument
                        {
                            DocumentTypeId = item.DocumentTypeId,
                            DocumentName = item.DocumentName,
                            DocumentPath = item.DocumentPath
                        });
                    }
                }

                responseOut = sqlDbInterface.AddEditPO(po, poProductList,poTaxList,poTermList,poDocumentList, poSchedulesList);
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

        public List<POViewModel> GetPOList(string poNo, string vendorName, string refNo, string fromDate, string toDate,string approvalStatus, int companyId, string displayType = "",string CreatedByUserName="",int companyBranch=0,string poType="")
        {
            List<POViewModel> pos = new List<POViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtPOs = sqlDbInterface.GetPOList(poNo, vendorName, refNo, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), approvalStatus, companyId, displayType, CreatedByUserName, companyBranch, poType);
                if (dtPOs != null && dtPOs.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPOs.Rows)
                    {
                        pos.Add(new POViewModel
                        {
                            POId = Convert.ToInt32(dr["POId"]),
                            PONo = Convert.ToString(dr["PONo"]),
                            PODate = Convert.ToString(dr["PODate"]),
                            VendorName = Convert.ToString(dr["VendorName"]),
                            VendorId = Convert.ToInt32(dr["VendorId"]),
                            VendorCode = Convert.ToString(dr["VendorCode"]),
                            ConsigneeName = Convert.ToString(dr["ConsigneeName"]),
                            ConsigneeId = Convert.ToInt32(dr["ConsigneeId"]),
                            ConsigneeCode = Convert.ToString(dr["ConsigneeCode"]),
                            City = Convert.ToString(dr["City"]),
                            StateName = Convert.ToString(dr["StateName"]),
                            RefNo = Convert.ToString(dr["RefNo"]),
                            RefDate = Convert.ToString(dr["RefDate"]),
                            ApprovalStatus = Convert.ToString(dr["ApprovalStatus"]),
                            POStatus = Convert.ToString(dr["POStatus"]),
                            BasicValue = Convert.ToDecimal(dr["BasicValue"]),
                            TotalValue = Convert.ToDecimal(dr["TotalValue"]),
                            PORevisedStatus = Convert.ToBoolean(dr["PORevisedStatus"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            CompanyBranch = Convert.ToString(dr["BranchName"]),
                            POType = Convert.ToString(dr["POType"]),
                            ApprovedByName = Convert.ToString(dr["ApprovedByName"]),
                            ApprovedDate= Convert.ToString(dr["ApprovedDate"])
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
        public POViewModel GetPODetail(long poId = 0)
        {
            
            POViewModel po = new POViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtpos = sqlDbInterface.GetPODetail(poId);
                if (dtpos != null && dtpos.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtpos.Rows)
                    {
                        po = new POViewModel
                        {
                            POId = Convert.ToInt32(dr["POId"]),
                            PONo = Convert.ToString(dr["PONo"]),
                            PODate = Convert.ToString(dr["PODate"]),
                            IndentId= Convert.ToInt32(dr["IndentId"]),
                            IndentNo = Convert.ToString(dr["IndentNo"]),
                            IndentDate = Convert.ToString(dr["IndentDate"]),
                            QuotationId = Convert.ToInt32(dr["QuotationId"]),
                            QuotationNo = Convert.ToString(dr["QuotationNo"]),
                            QuotationDate = Convert.ToString(dr["QuotationDate"]),


                            CurrencyCode = Convert.ToString(dr["CurrencyCode"]),
                            VendorId = Convert.ToInt32(dr["VendorId"]),
                            VendorCode = Convert.ToString(dr["VendorCode"]),
                            VendorName = Convert.ToString(dr["VendorName"]),
                            ConsigneeName = Convert.ToString(dr["ConsigneeName"]),
                            ConsigneeId = Convert.ToInt32(dr["ConsigneeId"]),
                            ConsigneeCode = Convert.ToString(dr["ConsigneeCode"]),
                            BillingAddress = Convert.ToString(dr["BillingAddress"]),
                            ShippingAddress=Convert.ToString(dr["ShippingAddress"]),
                            City = Convert.ToString(dr["City"]),
                            StateId = Convert.ToInt32(dr["StateId"]),
                            CountryId = Convert.ToInt32(dr["CountryId"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                            PinCode = Convert.ToString(dr["PinCode"]),
                            CSTNo = Convert.ToString(dr["CSTNo"]),
                            TINNo = Convert.ToString(dr["TINNo"]),
                            PANNo = Convert.ToString(dr["PANNo"]),
                            GSTNo = Convert.ToString(dr["GSTNo"]),
                            RefNo = Convert.ToString(dr["RefNo"]),
                            RefDate = Convert.ToString(dr["RefDate"]),
                            POStatus =Convert.ToString(dr["POStatus"]),
                            
                            BasicValue = Convert.ToDecimal(dr["BasicValue"]),
                            TotalValue = Convert.ToDecimal(dr["TotalValue"]),
                            ShippingCity = Convert.ToString(dr["ShippingCity"]),
                            ShippingStateId = Convert.ToInt32(dr["ShippingStateId"]),
                            ShippingCountryId = Convert.ToInt32(dr["ShippingCountryId"]),
                            ShippingPinCode = Convert.ToString(dr["ShippingPinCode"]),
                            ConsigneeGSTNo= Convert.ToString(dr["ConsigneeGSTNo"]),
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
                            Remarks1 = Convert.ToString(dr["Remarks1"]),
                            Remarks2 = Convert.ToString(dr["Remarks2"]),
                            ExpectedDeliveryDate = Convert.ToString(dr["ExpectedDeliveryDate"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            CurrencyConversionRate = Convert.ToDecimal(dr["CurrencyConversionRate"]),
                            POType = Convert.ToString(dr["POType"]),
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return po;
        }
        public DataTable GetPODetailDataTable(long poId = 0)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtPO = new DataTable();
            try
            {
                dtPO = sqlDbInterface.GetPODetail(poId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtPO;
        }


        public DataTable GetPOProductListDataTable(long poId)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtProducts = new DataTable();
            try
            {
                dtProducts = sqlDbInterface.GetPOProductList(poId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtProducts;
        }
        public DataTable GetPOTermListDataTable(long poId)
        {

            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtTerms = new DataTable();
            try
            {
                dtTerms = sqlDbInterface.GetPOTermList(poId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtTerms;
        }

        public DataTable GetPOTaxDataTable(long poId)
        {

            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtTaxs = new DataTable();
            try
            {
                dtTaxs = sqlDbInterface.GetPOTaxList(poId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtTaxs;
        }

        public ResponseOut AddRevisedPO(POViewModel poViewModel, List<POProductViewModel> poProducts, List<POTaxViewModel> poTaxes, List<POTermViewModel> poTerms,List<POSupportingDocumentViewModel> revisedPODocuments)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                PO po = new PO
                {
                    POId = poViewModel.POId,
                    PONo = poViewModel.PONo,
                    PODate = Convert.ToDateTime(poViewModel.PODate),
                    CurrencyCode = poViewModel.CurrencyCode,
                    IndentId=poViewModel.IndentId,
                    IndentNo = poViewModel.IndentNo,
                    QuotationId = poViewModel.QuotationId,
                    QuotationNo = poViewModel.QuotationNo,
                    CompanyBranchId= poViewModel.CompanyBranchId,
                    ExpectedDeliveryDate=Convert.ToDateTime(poViewModel.ExpectedDeliveryDate),
                    VendorId = poViewModel.VendorId,
                    VendorName = poViewModel.VendorName,
                    BillingAddress = poViewModel.BillingAddress,
                    ShippingAddress = poViewModel.ShippingAddress,
                    City = poViewModel.City,
                    StateId = poViewModel.StateId,
                    CountryId = poViewModel.CountryId,
                    PinCode = poViewModel.PinCode,
                    CSTNo = poViewModel.CSTNo,
                    TINNo = poViewModel.TINNo,
                    PANNo = poViewModel.PANNo,
                    GSTNo = poViewModel.GSTNo,
                    ExciseNo = poViewModel.ExciseNo,
                    ApprovalStatus=poViewModel.ApprovalStatus,
                    RefNo = string.IsNullOrEmpty(poViewModel.RefNo) ? "" : poViewModel.RefNo,
                    RefDate = string.IsNullOrEmpty(poViewModel.RefDate) ? Convert.ToDateTime("01-01-1900") : Convert.ToDateTime(poViewModel.RefDate),
                    BasicValue = poViewModel.BasicValue,
                    TotalValue = poViewModel.TotalValue,
                    ConsigneeId = poViewModel.ConsigneeId,
                    ConsigneeName = poViewModel.ConsigneeName,

                    ShippingCity = poViewModel.ShippingCity,
                    ShippingStateId = poViewModel.ShippingStateId,
                    ShippingCountryId = poViewModel.ShippingCountryId,
                    ShippingPinCode = poViewModel.ShippingPinCode,
                    ConsigneeGSTNo = poViewModel.ConsigneeGSTNo,

                    FreightValue = poViewModel.FreightValue,
                    FreightCGST_Perc = poViewModel.FreightCGST_Perc,
                    FreightCGST_Amt = poViewModel.FreightCGST_Amt,
                    FreightSGST_Perc = poViewModel.FreightSGST_Perc,
                    FreightSGST_Amt = poViewModel.FreightSGST_Amt,
                    FreightIGST_Perc = poViewModel.FreightIGST_Perc,
                    FreightIGST_Amt = poViewModel.FreightIGST_Amt,

                    LoadingValue = poViewModel.LoadingValue,
                    LoadingCGST_Perc = poViewModel.LoadingCGST_Perc,
                    LoadingCGST_Amt = poViewModel.LoadingCGST_Amt,
                    LoadingSGST_Perc = poViewModel.LoadingSGST_Perc,
                    LoadingSGST_Amt = poViewModel.LoadingSGST_Amt,
                    LoadingIGST_Perc = poViewModel.LoadingIGST_Perc,
                    LoadingIGST_Amt = poViewModel.LoadingIGST_Amt,
                    InsuranceValue = poViewModel.InsuranceValue,
                    InsuranceCGST_Perc = poViewModel.InsuranceCGST_Perc,
                    InsuranceCGST_Amt = poViewModel.InsuranceCGST_Amt,
                    InsuranceSGST_Perc = poViewModel.InsuranceSGST_Perc,
                    InsuranceSGST_Amt = poViewModel.InsuranceSGST_Amt,
                    InsuranceIGST_Perc = poViewModel.InsuranceIGST_Perc,
                    InsuranceIGST_Amt = poViewModel.InsuranceIGST_Amt,
                    Remarks1 = string.IsNullOrEmpty(poViewModel.Remarks1) ? null : poViewModel.Remarks1,
                    Remarks2 = string.IsNullOrEmpty(poViewModel.Remarks2) ? null : poViewModel.Remarks2,
                    FinYearId = poViewModel.FinYearId,
                    CompanyId = poViewModel.CompanyId,
                    CreatedBy = poViewModel.CreatedBy,
                    ReverseChargeApplicable = poViewModel.ReverseChargeApplicable,
                    ReverseChargeAmount = poViewModel.ReverseChargeAmount

                };
                List<POProductDetail> poProductList = new List<POProductDetail>();
                if (poProducts != null && poProducts.Count > 0)
                {
                    foreach (POProductViewModel item in poProducts)
                    {
                        poProductList.Add(new POProductDetail
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

                List<POTaxDetail> poTaxList = new List<POTaxDetail>();
                if (poTaxes != null && poTaxes.Count > 0)
                {
                    foreach (POTaxViewModel item in poTaxes)
                    {
                        poTaxList.Add(new POTaxDetail
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
                List<POTermsDetail> poTermList = new List<POTermsDetail>();
                if (poTerms != null && poTerms.Count > 0)
                {
                    foreach (POTermViewModel item in poTerms)
                    {
                        poTermList.Add(new POTermsDetail
                        {
                            TermDesc = item.TermDesc,
                            TermSequence = item.TermSequence
                        });
                    }
                }
                List<POSupportingDocument> revisedPODocumentList = new List<POSupportingDocument>();
                if (revisedPODocuments != null && revisedPODocuments.Count > 0)
                {
                    foreach (POSupportingDocumentViewModel item in revisedPODocuments)
                    {
                        revisedPODocumentList.Add(new POSupportingDocument
                        {
                            DocumentTypeId = item.DocumentTypeId,
                            DocumentName = item.DocumentName,
                            DocumentPath = item.DocumentPath
                        });
                    }
                }
                responseOut = sqlDbInterface.AddRevisedPO(po, poProductList, poTaxList, poTermList, revisedPODocumentList);
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

        public List<PurchaseQuotationViewModel> GetPurchaseQuotationList(string quotationNo, string vendorName, string fromDate, string toDate, int companyId, string displayType = "", string approvalStatus = "",string companyBranch="")
        {
            List<PurchaseQuotationViewModel> quotations = new List<PurchaseQuotationViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtQuotations = sqlDbInterface.GetPOPurchaseQuotationList(quotationNo, vendorName, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, displayType, approvalStatus, companyBranch);
                if (dtQuotations != null && dtQuotations.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtQuotations.Rows)
                    {
                        quotations.Add(new PurchaseQuotationViewModel
                        {
                            QuotationId = Convert.ToInt32(dr["QuotationId"]),
                            QuotationNo = Convert.ToString(dr["QuotationNo"]),
                            QuotationDate = Convert.ToString(dr["QuotationDate"]),
                            RequisitionId = Convert.ToInt32(dr["RequisitionId"]),
                            RequisitionNo = Convert.ToString(dr["RequisitionNo"]),
                            RequisitionDate= Convert.ToString(dr["RequisitionDate"]),
                            VendorName = Convert.ToString(dr["VendorName"]),
                            VendorId = Convert.ToInt32(dr["VendorId"]),
                            VendorCode = Convert.ToString(dr["VendorCode"]),
                            DeliveryDays = Convert.ToInt32(dr["DeliveryDays"]),
                            DeliveryAt = Convert.ToString(dr["DeliveryAt"]),
                            RefNo = Convert.ToString(dr["RefNo"]),
                            RefDate = Convert.ToString(dr["RefDate"]),
                            BasicValue = Convert.ToDecimal(dr["BasicValue"]),
                            TotalValue = Convert.ToDecimal(dr["TotalValue"]),
                            CreatedByUser = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ApprovalStatus = Convert.ToString(dr["ApprovalStatus"]),
                            ModifiedByUser = Convert.ToString(dr["ModifiedByName"]),
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
            return quotations;
        }

        public List<PurchaseQuotationProductViewModel> GetPurchaseQuotationProductList(long quotationId)
        {
            List<PurchaseQuotationProductViewModel> quotationProducts = new List<PurchaseQuotationProductViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCustomers = sqlDbInterface.GetPOPurchaseQuotationProductList(quotationId);
                if (dtCustomers != null && dtCustomers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCustomers.Rows)
                    {
                        quotationProducts.Add(new PurchaseQuotationProductViewModel
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
            return quotationProducts;
        }

        public ResponseOut CancelPO(POViewModel pOViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                PO pO = new PO
                {
                    POId = pOViewModel.POId,
                    CancelStatus = "Cancel",
                    POStatus = "Cancelled",
                    CreatedBy = pOViewModel.CreatedBy,
                    CancelReason = pOViewModel.CancelReason
                };
                responseOut = dbInterface.CancelPO(pO);
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

        public List<POViewModel> GetPrpductPurchaseList(long productID,long companyBranchId)
        {
            List<POViewModel> pos = new List<POViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtPOs = sqlDbInterface.GetPrpductPurchaseList(productID,companyBranchId);
                if (dtPOs != null && dtPOs.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPOs.Rows)
                    {
                        pos.Add(new POViewModel
                        {
                           
                            PONo = Convert.ToString(dr["PONo"]),
                            PODate = Convert.ToString(dr["PODate"]),
                            VendorName = Convert.ToString(dr["VendorName"]),
                            ProductName=Convert.ToString(dr["ProductName"]),
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
            return pos;
        }

        public List<POProductViewModel> GetPIPOProductList(long poId)
        {
            List<POProductViewModel> quotationProducts = new List<POProductViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtProducts = sqlDbInterface.GetPIPOProductList(poId);
                if (dtProducts != null && dtProducts.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtProducts.Rows)
                    {
                        quotationProducts.Add(new POProductViewModel
                        {
                            POProductDetailId = Convert.ToInt32(dr["POProductDetailId"]),
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
        #endregion


        #region PO Approval
        public ResponseOut ApproveRejectPO(POViewModel poViewModel)
        {
            ResponseOut responseOut = new ResponseOut();

            try
            {

                PO po = new PO
                {
                    POId = poViewModel.POId,
                    RejectedReason = poViewModel.RejectedReason,
                    ApprovedBy = poViewModel.ApprovedBy,
                    ApprovalStatus = poViewModel.ApprovalStatus

                };

                responseOut = dbInterface.ApproveRejectPO(po);
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

        public List<POViewModel> GetApprovePOList(string poNo, string vendorName, string refNo, string fromDate, string toDate, string approvalStatus, int companyId, string displayType = "", string companyBranch ="")
        {
            List<POViewModel> pos = new List<POViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtPOs = sqlDbInterface.GetApprovelPOList(poNo, vendorName, refNo, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), approvalStatus, companyId, displayType, companyBranch);
                if (dtPOs != null && dtPOs.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPOs.Rows)
                    {
                        pos.Add(new POViewModel
                        {
                            POId = Convert.ToInt32(dr["POId"]),
                            PONo = Convert.ToString(dr["PONo"]),
                            PODate = Convert.ToString(dr["PODate"]),
                            VendorName = Convert.ToString(dr["VendorName"]),
                            VendorId = Convert.ToInt32(dr["VendorId"]),
                            VendorCode = Convert.ToString(dr["VendorCode"]),
                            ConsigneeName = Convert.ToString(dr["ConsigneeName"]),
                            ConsigneeId = Convert.ToInt32(dr["ConsigneeId"]),
                            ConsigneeCode = Convert.ToString(dr["ConsigneeCode"]),
                            City = Convert.ToString(dr["City"]),
                            StateName = Convert.ToString(dr["StateName"]),
                            RefNo = Convert.ToString(dr["RefNo"]),
                            RefDate = Convert.ToString(dr["RefDate"]),
                            ApprovalStatus = Convert.ToString(dr["ApprovalStatus"]),
                            BasicValue = Convert.ToDecimal(dr["BasicValue"]),
                            TotalValue = Convert.ToDecimal(dr["TotalValue"]),
                            PORevisedStatus = Convert.ToBoolean(dr["PORevisedStatus"]),
                            ApprovedByUserName=Convert.ToString(dr["ApprovedByName"]),
                            ApprovedDate = Convert.ToString(dr["ApprovedDate"]),
                            RejectedByUserName = Convert.ToString(dr["RejectedByName"]),
                            RejectedDate = Convert.ToString(dr["RejectedDate"]),
                            CancelByUserName = Convert.ToString(dr["CanceledByName"]),
                            CancelDate = Convert.ToString(dr["CancelDate"]),
                            CancelReason = Convert.ToString(dr["CancelReason"]),
                            CompanyBranch=Convert.ToString(dr["BranchName"])

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

        public POViewModel GetPOApprovalDetail(long poId = 0)
        {

            POViewModel po = new POViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtpos = sqlDbInterface.GetApprovelPODetail(poId);
                if (dtpos != null && dtpos.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtpos.Rows)
                    {
                        po = new POViewModel
                        {
                            POId = Convert.ToInt32(dr["POId"]),
                            PONo = Convert.ToString(dr["PONo"]),
                            PODate = Convert.ToString(dr["PODate"]),
                            IndentId = Convert.ToInt32(dr["IndentId"]),
                            IndentNo = Convert.ToString(dr["IndentNo"]),
                            IndentDate = Convert.ToString(dr["IndentDate"]),
                            QuotationId = Convert.ToInt32(dr["QuotationId"]),
                            QuotationNo = Convert.ToString(dr["QuotationNo"]),
                            QuotationDate = Convert.ToString(dr["QuotationDate"]),


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
                            ApprovalStatus = Convert.ToString(dr["ApprovalStatus"]),

                            BasicValue = Convert.ToDecimal(dr["BasicValue"]),
                            TotalValue = Convert.ToDecimal(dr["TotalValue"]),
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
                            Remarks1 = Convert.ToString(dr["Remarks1"]),
                            Remarks2 = Convert.ToString(dr["Remarks2"]),
                            ExpectedDeliveryDate = Convert.ToString(dr["ExpectedDeliveryDate"]),
                            ApprovedByUserName = Convert.ToString(dr["ApprovedByName"]),
                            ApprovedDate = Convert.ToString(dr["ApprovedDate"]),
                            RejectedByUserName = Convert.ToString(dr["RejectedByName"]),
                            RejectedDate = Convert.ToString(dr["RejectedDate"]),
                            RejectedReason = Convert.ToString(dr["RejectedReason"]),
                            CancelByUserName= Convert.ToString(dr["CanceledByName"]),
                            CancelDate=Convert.ToString(dr["CancelDate"]),
                            CancelReason = Convert.ToString(dr["CancelReason"])


                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return po;
        }
        #endregion

        #region Purchase Indent
        public List<PurchaseIndentViewModel> GetPurchaseOrderIndentList(string indentNo, string indentType, string customerName, int companyBranchId, DateTime fromDate, DateTime toDate, int companyId, string displayType = "", string approvalStatus = "0")
        {
            List<PurchaseIndentViewModel> indents = new List<PurchaseIndentViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtIndents = sqlDbInterface.GetPurchaseOrderIndentList(indentNo, indentType, customerName, companyBranchId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, displayType, approvalStatus);
                if (dtIndents != null && dtIndents.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtIndents.Rows)
                    {
                        indents.Add(new PurchaseIndentViewModel
                        {
                            IndentId = Convert.ToInt32(dr["IndentId"]),
                            IndentNo = Convert.ToString(dr["IndentNo"]),
                            IndentDate = Convert.ToString(dr["IndentDate"]),
                            IndentType = Convert.ToString(dr["IndentType"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            CustomerId = Convert.ToInt32(dr["CustomerID"]),
                            CustomerCode = Convert.ToString(dr["CustomerCode"]),
                            IndentStatus = string.IsNullOrEmpty(Convert.ToString(dr["IndentStatus"])) ? "" : Convert.ToString(dr["IndentStatus"]),

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return indents;
        }

        public List<PurchaseIndentProductDetailViewModel> GetPurchaseOrderIndentProductList(long indentId)
        {
            List<PurchaseIndentProductDetailViewModel> indentProducts = new List<PurchaseIndentProductDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtIndentProducts = sqlDbInterface.GetPurchaseOrderIndentProductList(indentId);
                if (dtIndentProducts != null && dtIndentProducts.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtIndentProducts.Rows)
                    {
                        indentProducts.Add(new PurchaseIndentProductDetailViewModel
                        {
                            IndentProductDetailId = Convert.ToInt32(dr["IndentProductDetailId"]),
                            SequenceNo = Convert.ToInt32(dr["SNo"]),
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ProductCode = Convert.ToString(dr["ProductCode"]),
                            ProductShortDesc = Convert.ToString(dr["ProductShortDesc"]),
                            UOMName = Convert.ToString(dr["UOMName"]),
                            Quantity = Convert.ToDecimal(dr["Quantity"]),
                            Price = Convert.ToDecimal(dr["PurchasePrice"]),
                            TotalPrice = Convert.ToDecimal(dr["TotalPrice"])

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return indentProducts;
        }
        #endregion
    }
}
