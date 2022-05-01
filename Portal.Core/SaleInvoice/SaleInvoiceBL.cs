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
    public class SaleInvoiceBL
    {
        DBInterface dbInterface;
        public SaleInvoiceBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditSaleInvoice(SaleInvoiceViewModel saleinvoiceViewModel, List<SaleInvoiceProductViewModel> saleinvoiceProducts, List<SaleInvoiceTaxViewModel> saleinvoiceTaxes, List<SaleInvoiceTermViewModel> saleinvoiceTerms, List<SaleInvoiceProductSerialDetailViewModel> saleInvoiceProductSerialDetail, List<SISupportingDocumentViewModel> siDocuments)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                SaleInvoice saleinvoice = new SaleInvoice
                {
                    InvoiceId = saleinvoiceViewModel.InvoiceId,
                    InvoiceDate = Convert.ToDateTime(saleinvoiceViewModel.InvoiceDate),
                    InvoiceType = saleinvoiceViewModel.InvoiceType,
                    SONo = saleinvoiceViewModel.SONo,
                    SOId = saleinvoiceViewModel.SOId,
                    CompanyBranchId = saleinvoiceViewModel.CompanyBranchId,
                    CurrencyCode = saleinvoiceViewModel.CurrencyCode,
                    CustomerId = saleinvoiceViewModel.CustomerId,
                    CustomerName = saleinvoiceViewModel.CustomerName,
                    ConsigneeId = saleinvoiceViewModel.ConsigneeId,
                    ConsigneeName = saleinvoiceViewModel.ConsigneeName,
                    ContactPerson = saleinvoiceViewModel.ContactPerson,
                    BillingAddress = saleinvoiceViewModel.BillingAddress,
                    City = saleinvoiceViewModel.City,
                    StateId = saleinvoiceViewModel.StateId,
                    CountryId = saleinvoiceViewModel.CountryId,
                    PinCode = saleinvoiceViewModel.PinCode,
                    CSTNo = saleinvoiceViewModel.CSTNo,
                    TINNo = saleinvoiceViewModel.TINNo,
                    PANNo = saleinvoiceViewModel.PANNo,
                    GSTNo = saleinvoiceViewModel.GSTNo,
                    ExciseNo = saleinvoiceViewModel.ExciseNo,
                    Email = saleinvoiceViewModel.Email,
                    MobileNo = saleinvoiceViewModel.MobileNo,
                    ContactNo = saleinvoiceViewModel.ContactNo,
                    Fax = saleinvoiceViewModel.Fax,
                    ApprovalStatus = saleinvoiceViewModel.ApprovalStatus,

                    ShippingContactPerson = saleinvoiceViewModel.ShippingContactPerson,
                    ShippingBillingAddress = saleinvoiceViewModel.ShippingBillingAddress,
                    ShippingCity = saleinvoiceViewModel.ShippingCity,
                    ShippingStateId = saleinvoiceViewModel.ShippingStateId,
                    ShippingCountryId = saleinvoiceViewModel.ShippingCountryId,
                    ShippingPinCode = saleinvoiceViewModel.ShippingPinCode,
                    ShippingTINNo = saleinvoiceViewModel.ShippingTINNo,
                    ShippingEmail = saleinvoiceViewModel.ShippingEmail,
                    ShippingMobileNo = saleinvoiceViewModel.ShippingMobileNo,
                    ShippingContactNo = saleinvoiceViewModel.ShippingContactNo,
                    ShippingFax = saleinvoiceViewModel.ShippingFax,
                    RefNo = saleinvoiceViewModel.RefNo,
                    RefDate = string.IsNullOrEmpty(saleinvoiceViewModel.RefDate) ? Convert.ToDateTime("01-01-1900") : Convert.ToDateTime(saleinvoiceViewModel.RefDate),
                    BasicValue = saleinvoiceViewModel.BasicValue,
                    TotalValue = saleinvoiceViewModel.TotalValue,
                    FreightValue = saleinvoiceViewModel.FreightValue,
                    FreightCGST_Perc = saleinvoiceViewModel.FreightCGST_Perc,
                    FreightCGST_Amt = saleinvoiceViewModel.FreightCGST_Amt,
                    FreightSGST_Perc = saleinvoiceViewModel.FreightSGST_Perc,
                    FreightSGST_Amt = saleinvoiceViewModel.FreightSGST_Amt,
                    FreightIGST_Perc = saleinvoiceViewModel.FreightIGST_Perc,
                    FreightIGST_Amt = saleinvoiceViewModel.FreightIGST_Amt,

                    LoadingValue = saleinvoiceViewModel.LoadingValue,
                    LoadingCGST_Perc = saleinvoiceViewModel.LoadingCGST_Perc,
                    LoadingCGST_Amt = saleinvoiceViewModel.LoadingCGST_Amt,
                    LoadingSGST_Perc = saleinvoiceViewModel.LoadingSGST_Perc,
                    LoadingSGST_Amt = saleinvoiceViewModel.LoadingSGST_Amt,
                    LoadingIGST_Perc = saleinvoiceViewModel.LoadingIGST_Perc,
                    LoadingIGST_Amt = saleinvoiceViewModel.LoadingIGST_Amt,
                    InsuranceValue = saleinvoiceViewModel.InsuranceValue,
                    InsuranceCGST_Perc = saleinvoiceViewModel.InsuranceCGST_Perc,
                    InsuranceCGST_Amt = saleinvoiceViewModel.InsuranceCGST_Amt,
                    InsuranceSGST_Perc = saleinvoiceViewModel.InsuranceSGST_Perc,
                    InsuranceSGST_Amt = saleinvoiceViewModel.InsuranceSGST_Amt,
                    InsuranceIGST_Perc = saleinvoiceViewModel.InsuranceIGST_Perc,
                    InsuranceIGST_Amt = saleinvoiceViewModel.InsuranceIGST_Amt,
                    PayToBookId = saleinvoiceViewModel.PayToBookId,
                    Remarks = saleinvoiceViewModel.Remarks,
                    FinYearId = saleinvoiceViewModel.FinYearId,
                    CompanyId = saleinvoiceViewModel.CompanyId,
                    CreatedBy = saleinvoiceViewModel.CreatedBy,
                    ReverseChargeApplicable = saleinvoiceViewModel.ReverseChargeApplicable,
                    ReverseChargeAmount = saleinvoiceViewModel.ReverseChargeAmount,
                    RoundOfValue=saleinvoiceViewModel.RoundOfValue,
                    GrossValue= saleinvoiceViewModel.GrossValue,
                    SaleType=saleinvoiceViewModel.SaleType,
                    TransportName=saleinvoiceViewModel.TransportName,
                    VehicleNo= saleinvoiceViewModel.VehicleNo,
                    BiltyNo=saleinvoiceViewModel.BiltyNo,
                    BiltyDate = string.IsNullOrEmpty(saleinvoiceViewModel.BiltyDate) ? Convert.ToDateTime("01-01-1900") : Convert.ToDateTime(saleinvoiceViewModel.BiltyDate),
                    AdharcardNo = saleinvoiceViewModel.AdharcardNo,
                    Pancard = saleinvoiceViewModel.Pancard,
                    IdtypeName = saleinvoiceViewModel.IdtypeName,
                    IdtypeValue = saleinvoiceViewModel.IdtypeValue,
                    RtoRegsValue = saleinvoiceViewModel.RtoRegsValue,
                    RtoRegsCGST_Amt = saleinvoiceViewModel.RtoRegsCGST_Amt,
                    RtoRegsSGST_Amt = saleinvoiceViewModel.RtoRegsSGST_Amt,
                    RtoRegsIGST_Amt = saleinvoiceViewModel.RtoRegsIGST_Amt,
                    RtoRegsCGST_Perc = saleinvoiceViewModel.RtoRegsCGST_Perc,
                    RtoRegsSGST_Perc = saleinvoiceViewModel.RtoRegsSGST_Perc,
                    RtoRegsIGST_Perc = saleinvoiceViewModel.RtoRegsIGST_Perc,
                    VehicleInsuranceValue = saleinvoiceViewModel.VehicleInsuranceValue,
                    HypothecationBy= saleinvoiceViewModel.HypothecationBy,
                    EwayBillNo= saleinvoiceViewModel.EwayBillNo,
                    SaleEmpId= saleinvoiceViewModel.SaleEmpId,
                    SaleInvoiceType = saleinvoiceViewModel.SaleInvoiceType,



                };
                List<SaleInvoiceProductDetail> saleinvoiceProductList = new List<SaleInvoiceProductDetail>();
                if (saleinvoiceProducts != null && saleinvoiceProducts.Count > 0)
                {
                    foreach (SaleInvoiceProductViewModel item in saleinvoiceProducts)
                    {
                        saleinvoiceProductList.Add(new SaleInvoiceProductDetail
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

                List<SaleInvoiceTaxDetail> saleinvoiceTaxList = new List<SaleInvoiceTaxDetail>();
                if (saleinvoiceTaxes != null && saleinvoiceTaxes.Count > 0)
                {
                    foreach (SaleInvoiceTaxViewModel item in saleinvoiceTaxes)
                    {
                        saleinvoiceTaxList.Add(new SaleInvoiceTaxDetail
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
                List<SaleInvoiceTermsDetail> saleinvoiceTermList = new List<SaleInvoiceTermsDetail>();
                if (saleinvoiceTerms != null && saleinvoiceTerms.Count > 0)
                {
                    foreach (SaleInvoiceTermViewModel item in saleinvoiceTerms)
                    {
                        saleinvoiceTermList.Add(new SaleInvoiceTermsDetail
                        {
                            TermDesc = item.TermDesc,
                            TermSequence = item.TermSequence
                        });
                    }
                }

        List<SaleInvoiceSupportingDocument> siDocumentList = new List<SaleInvoiceSupportingDocument>();
                if (siDocuments != null && siDocuments.Count > 0)
                {
                    foreach (SISupportingDocumentViewModel item in siDocuments)
                    {
                        siDocumentList.Add(new SaleInvoiceSupportingDocument
                        {
                            DocumentTypeId = item.DocumentTypeId,
                            DocumentName = item.DocumentName,
                            DocumentPath = item.DocumentPath,
                            LabourCode= item.LabourCode,
                            Description = item.Description,
                            LabourRate = item.LabourRate,
                            DiscountPerc = item.DiscountPerc,
                            DiscountAmount = item.DiscountAmount,
                            CGST_Perc = item.CGST_Perc,
                            CGST_Amount = item.CGST_Amount,
                            SGST_Perc = item.SGST_Perc,
                            SGST_Amount = item.SGST_Amount,
                            IGST_Perc = item.IGST_Perc,
                            IGST_Amount = item.IGST_Amount,
                            TotalAmount = item.TotalAmount,
                            ProductId=item.ProductId,
                        });
                    }
                }
                
                List<SaleInvoiceProductSerialDetail> saleInvoiceProductSerialDetailList = new List<SaleInvoiceProductSerialDetail>();
                if (saleInvoiceProductSerialDetail != null && saleInvoiceProductSerialDetail.Count > 0)
                {
                    foreach (SaleInvoiceProductSerialDetailViewModel item in saleInvoiceProductSerialDetail)
                    {
                        saleInvoiceProductSerialDetailList.Add(new SaleInvoiceProductSerialDetail
                        {
                            InvoiceId = 0,
                            ProductId = item.ProductId,
                            RefSerial1 = item.RefSerial1,
                            RefSerial2 = item.RefSerial2,
                            RefSerial3 = item.RefSerial3,
                            RefSerial4 = item.RefSerial4,  
                            PackingListTypeID=item.PackingListTypeID,                         
                           
                        });
                    }
                }

                int prodQty = 0;
               
                bool flag = true;
                if (saleinvoiceProducts != null && saleinvoiceProducts.Count > 0)
                {
                    foreach (SaleInvoiceProductViewModel item in saleinvoiceProducts)
                    {
                        int rowCnt = 0;
                        if (saleInvoiceProductSerialDetail != null && saleInvoiceProductSerialDetail.Count > 0)
                        {
                            foreach (SaleInvoiceProductSerialDetailViewModel itm in saleInvoiceProductSerialDetail)
                            {

                                if (item.ProductId == itm.ProductId)
                                {
                                       rowCnt++;
                                }
                            }
                                if (item.Quantity != rowCnt)
                                {
                                    flag = false;
                                }
                        }
                        
                    }
                }
                responseOut = sqlDbInterface.AddEditSaleInvoice(saleinvoice, saleinvoiceProductList, saleinvoiceTaxList, saleinvoiceTermList, saleInvoiceProductSerialDetailList, siDocumentList);
                //if (flag == true)
                //{
                //    responseOut = sqlDbInterface.AddEditSaleInvoice(saleinvoice, saleinvoiceProductList, saleinvoiceTaxList, saleinvoiceTermList, saleInvoiceProductSerialDetailList,siDocumentList);
                //}
                //else
                //{
                //    responseOut.message = ActionMessage.SaleInvoiceProductQuantity;
                //    responseOut.status = ActionStatus.Fail;
                //}

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
        public List<SaleInvoiceViewModel> GetSaleInvoiceList(string saleinvoiceNo, string customerName, string refNo, string fromDate, string toDate, int companyId,string invoiceType="",string displayType="" ,string approvalStatus="",string customerCode="", int companyBranchId = 0,string saleType="",string CreatedByUserName="",int CustomerId=0,string SaleInvoiceType="")
        {
            List<SaleInvoiceViewModel> saleinvoices = new List<SaleInvoiceViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtSaleInvoices = sqlDbInterface.GetSaleInvoiceList(saleinvoiceNo, customerName, refNo, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId,invoiceType,displayType, approvalStatus,customerCode, companyBranchId,saleType, CreatedByUserName,CustomerId, SaleInvoiceType);
                if (dtSaleInvoices != null && dtSaleInvoices.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtSaleInvoices.Rows)
                    {
                        saleinvoices.Add(new SaleInvoiceViewModel
                        {
                            InvoiceId = Convert.ToInt32(dr["InvoiceId"]),
                            InvoiceNo = Convert.ToString(dr["InvoiceNo"]),
                            InvoiceDate = Convert.ToString(dr["InvoiceDate"]),
                            SONo = Convert.ToString(dr["SONo"]),
                            InvoiceType = Convert.ToString(dr["InvoiceType"]),
                            CustomerId = Convert.ToInt32(dr["CustomerId"]),
                            CustomerCode = Convert.ToString(dr["CustomerCode"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            ConsigneeId = Convert.ToInt32(dr["ConsigneeId"]),
                            ConsigneeName = Convert.ToString(dr["ConsigneeName"]),
                            ConsigneeCode = Convert.ToString(dr["ConsigneeCode"]),
                            CompanyBranchName = Convert.ToString(dr["BranchName"]),
                            City = Convert.ToString(dr["City"]),
                            StateName = Convert.ToString(dr["StateName"]),
                            SaleType = Convert.ToString(dr["SaleType"]),
                            RefNo = Convert.ToString(dr["RefNo"]),
                            RefDate = Convert.ToString(dr["RefDate"]),
                            BasicValue = Convert.ToDecimal(dr["BasicValue"]),
                            BasicAmt = Convert.ToDecimal(dr["BasicAmt"]),
                            TotalValue = Convert.ToDecimal(dr["TotalValue"]),
                            GrossValue = Convert.ToDecimal(dr["GrossValue"]),
                            RoundOfValue = Convert.ToDecimal(dr["RoundOfValue"]),
                            ApprovalStatus =Convert.ToString(dr["ApprovalStatus"]),
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
            return saleinvoices;
        }

        public List<SaleInvoiceProductSerialDetailViewModel> GetSaleInvoiceProductSerialDetailList(int invoiceId=0)
        {
            List<SaleInvoiceProductSerialDetailViewModel> saleInvoiceProductSerialDetailList = new List<SaleInvoiceProductSerialDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtSaleInvoiceProductSerials = sqlDbInterface.GetSaleInvoiceProductSerialDetailList(invoiceId);
                DataTable dtProductVahaanSerialList = sqlDbInterface.GetSaleInvoiceProductVahaanSerialList(invoiceId);
                if (dtSaleInvoiceProductSerials != null && dtSaleInvoiceProductSerials.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtSaleInvoiceProductSerials.Rows)
                    {
                        saleInvoiceProductSerialDetailList.Add(new SaleInvoiceProductSerialDetailViewModel
                        {
                            InvoiceId = Convert.ToInt32(dr["InvoiceId"]),
                            ProductId  = Convert.ToInt32(dr["ProductId"]),
                            ProductName=Convert.ToString(dr["ProductName"]),
                            RefSerial1=Convert.ToString(dr["RefSerial1"]),
                            RefSerial2=Convert.ToString(dr["RefSerial2"]),
                            RefSerial3 = Convert.ToString(dr["RefSerial3"]),
                            RefSerial4 = Convert.ToString(dr["RefSerial4"]),
                            PackingListTypeID = Convert.ToInt32(dr["PackingListTypeID"]),
                            PackingListTypeName= Convert.ToString(dr["PackingListTypeName"])
                        });
                    }

                }
                
                if(dtProductVahaanSerialList!=null && dtProductVahaanSerialList.Rows.Count>0)
                {
                    foreach (DataRow dr in dtProductVahaanSerialList.Rows)
                    {
                        string vahan="";
                        string model = Convert.ToString(dr["ChasisSerialNo"]);
                        string str = model.Substring(9, 1);
                        if(str=="R")
                        {
                             vahan = Convert.ToString(dr["ChasisSerialNo"]).Substring(0, 3) + "|";
                        }
                        else
                        {
                            vahan = Convert.ToString(dr["ChasisSerialNo"]).Substring(0, 3)+ str + "|";
                        }
                          
                                        
                        vahan += Convert.ToString(dr["ChasisSerialNo"])+"|";
                        vahan += Convert.ToString(dr["MotorNo"]) + "|";
                        vahan += Convert.ToString(dr["CreatedDate"]) + "|";
                        vahan += Convert.ToString(dr["ColourCode"]) + "|";
                        vahan += Convert.ToString(dr["CustomerCode"]) + "|";
                        vahan += "NA";
                        Logger.SaveVahaanLog(vahan, Convert.ToString(dr["ChasisSerialNo"]));
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

        public List<SaleInvoiceViewModel> GetJVSaleInvoiceList(string saleinvoiceNo, string refNo, string fromDate, string toDate, int companyId, string invoiceType = "", string displayType = "", string approvalStatus = "")
        {
            List<SaleInvoiceViewModel> saleinvoices = new List<SaleInvoiceViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtSaleInvoices = sqlDbInterface.GetJVSaleInvoiceList(saleinvoiceNo, refNo, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, invoiceType, displayType, approvalStatus);
                if (dtSaleInvoices != null && dtSaleInvoices.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtSaleInvoices.Rows)
                    {
                        saleinvoices.Add(new SaleInvoiceViewModel
                        {
                            InvoiceId = Convert.ToInt32(dr["InvoiceId"]),
                            InvoiceNo = Convert.ToString(dr["InvoiceNo"]),
                            InvoiceDate = Convert.ToString(dr["InvoiceDate"]),
                            SONo = Convert.ToString(dr["SONo"]),
                            InvoiceType = Convert.ToString(dr["InvoiceType"]),
                            CustomerId = Convert.ToInt32(dr["CustomerId"]),
                            CustomerCode = Convert.ToString(dr["CustomerCode"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            City = Convert.ToString(dr["City"]),
                            StateName = Convert.ToString(dr["StateName"]),
                            RefNo = Convert.ToString(dr["RefNo"]),
                            RefDate = Convert.ToString(dr["RefDate"]),                           
                            BasicValue = Convert.ToDecimal(dr["BasicValue"]),
                            TotalValue = Convert.ToDecimal(dr["TotalValue"]),
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
            return saleinvoices;
        }

        public SaleInvoiceViewModel GetSaleInvoiceDetail(long saleinvoiceId = 0)
        {
            SaleInvoiceViewModel saleinvoice = new SaleInvoiceViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCompanies = sqlDbInterface.GetSaleInvoiceDetail(saleinvoiceId);
                if (dtCompanies != null && dtCompanies.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCompanies.Rows)
                    {
                        saleinvoice = new SaleInvoiceViewModel
                        {
                            InvoiceId = Convert.ToInt32(dr["InvoiceId"]),
                            InvoiceNo = Convert.ToString(dr["InvoiceNo"]),
                            InvoiceDate = Convert.ToString(dr["InvoiceDate"]),
                            SOId = Convert.ToInt32(dr["SOId"]),
                            SONo = Convert.ToString(dr["SONo"]),
                            SODate = Convert.ToString(dr["SODate"]),
                            InvoiceType = Convert.ToString(dr["InvoiceType"]),
                            CurrencyCode = Convert.ToString(dr["CurrencyCode"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                            CustomerId = Convert.ToInt32(dr["CustomerId"]),
                            CustomerCode = Convert.ToString(dr["CustomerCode"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            ConsigneeId = Convert.ToInt32(dr["ConsigneeId"]),
                            ConsigneeCode = Convert.ToString(dr["ConsigneeCode"]),
                            ConsigneeName = Convert.ToString(dr["ConsigneeName"]),
                            ContactPerson = Convert.ToString(dr["ContactPerson"]),
                            BillingAddress = Convert.ToString(dr["BillingAddress"]),
                            City = Convert.ToString(dr["City"]),
                            StateId = Convert.ToInt32(dr["StateId"]),
                            CountryId = Convert.ToInt32(dr["CountryId"]),
                            PinCode = Convert.ToString(dr["PinCode"]),
                            TINNo = Convert.ToString(dr["TINNo"]),
                            GSTNo = Convert.ToString(dr["GSTNo"]),
                            Email = Convert.ToString(dr["Email"]),
                            MobileNo = Convert.ToString(dr["MobileNo"]),
                            ContactNo = Convert.ToString(dr["ContactNo"]),
                            Fax = Convert.ToString(dr["Fax"]),
                            ApprovalStatus = Convert.ToString(dr["ApprovalStatus"]),
                            SaleType = Convert.ToString(dr["SaleType"]),

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


                            RefNo = Convert.ToString(dr["RefNo"]),
                            RefDate = Convert.ToString(dr["RefDate"]),

                            BasicValue = Convert.ToDecimal(dr["BasicValue"]),
                            TotalValue = Convert.ToDecimal(dr["TotalValue"]),
                            RoundOfValue = Convert.ToDecimal(dr["RoundOfValue"]),
                            GrossValue = Convert.ToDecimal(dr["GrossValue"]),

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

                            PayToBookId = Convert.ToInt32(dr["PayToBookId"]),
                            PayToBookName = Convert.ToString(dr["PayToBookName"]),
                            PayToBookBranch = Convert.ToString(dr["PayToBookBranch"]),
                            ReverseChargeApplicable = Convert.ToBoolean(dr["ReverseChargeApplicable"]),
                            ReverseChargeAmount = Convert.ToDecimal(dr["ReverseChargeAmount"]),

                            Remarks = Convert.ToString(dr["Remarks"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            TransportName = Convert.ToString(dr["TransportName"]),
                            VehicleNo = Convert.ToString(dr["VehicleNo"]),
                            BiltyNo = Convert.ToString(dr["BiltyNo"]),
                            BiltyDate = Convert.ToString(dr["BiltyDate"]),

                            AdharcardNo = Convert.ToString(dr["AdharcardNo"]),
                            Pancard = Convert.ToString(dr["Pancard"]),
                            IdtypeName = Convert.ToString(dr["IdtypeName"]),
                            IdtypeValue = Convert.ToString(dr["IdtypeValue"]),
                            HypothecationBy = Convert.ToString(dr["HypothecationBy"]),
                            EwayBillNo = Convert.ToString(dr["EwayBillNo"]),
                            SaleEmpId = Convert.ToInt32(dr["SaleEmpId"]),
                            SaleEmployeeName= Convert.ToString(dr["SaleEmpName"]),


                            RtoRegsValue = Convert.ToDecimal(dr["RtoRegsValue"]),
                            RtoRegsCGST_Amt = Convert.ToDecimal(dr["RtoRegsCGST_Amt"]),
                            RtoRegsSGST_Amt = Convert.ToDecimal(dr["RtoRegsSGST_Amt"]),
                            RtoRegsIGST_Amt = Convert.ToDecimal(dr["RtoRegsIGST_Amt"]),
                            RtoRegsCGST_Perc = Convert.ToDecimal(dr["RtoRegsCGST_Perc"]),
                            RtoRegsIGST_Perc = Convert.ToDecimal(dr["RtoRegsSGST_Perc"]),
                            RtoRegsSGST_Perc = Convert.ToDecimal(dr["RtoRegsIGST_Perc"]),
                            VehicleInsuranceValue = Convert.ToDecimal(dr["VehicleInsuranceValue"]),
                            BranchType = Convert.ToString(dr["BranchType"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return saleinvoice;
        }


        public List<SaleInvoiceChasisProductSerialDetailViewModel> GetSaleInvoiceChasisProductList(long saleinvoiceId,int mode)
        {
            List<SaleInvoiceChasisProductSerialDetailViewModel> saleinvoiceProducts = new List<SaleInvoiceChasisProductSerialDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtSaleInvoiceChasisProducts = sqlDbInterface.GetSaleInvoiceChasisProductList(saleinvoiceId, mode);
                if (dtSaleInvoiceChasisProducts != null && dtSaleInvoiceChasisProducts.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtSaleInvoiceChasisProducts.Rows)
                    {
                        saleinvoiceProducts.Add(new SaleInvoiceChasisProductSerialDetailViewModel
                        {
                            SequenceNo= Convert.ToInt32(dr["SequenceNo"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            RefSerial1 = Convert.ToString(dr["RefSerial1"]),
                            InvoiceSerialId= Convert.ToInt32(dr["InvoiceSerialId"]),
                            ProductId= Convert.ToInt32(dr["ProductId"]),
                            Status= Convert.ToInt32(dr["status"])
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

        public List<SaleInvoiceProductViewModel> GetSaleInvoiceProductList(long saleinvoiceId)
        {
            List<SaleInvoiceProductViewModel> saleinvoiceProducts = new List<SaleInvoiceProductViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCustomers = sqlDbInterface.GetSaleInvoiceProductList(saleinvoiceId);
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
                            IsSerializedProduct=Convert.ToString(dr["IsSerializedProduct"]),
                            IsThirdPartyProduct = Convert.ToString(dr["IsThirdPartyProduct"]),
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

        public List<SaleInvoiceTaxViewModel> GetSaleInvoiceTaxList(long saleinvoiceId)
        {
            List<SaleInvoiceTaxViewModel> saleinvoiceTaxes = new List<SaleInvoiceTaxViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCustomers = sqlDbInterface.GetSaleInvoiceTaxList(saleinvoiceId);
                if (dtCustomers != null && dtCustomers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCustomers.Rows)
                    {
                        saleinvoiceTaxes.Add(new SaleInvoiceTaxViewModel
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
                            SurchargeAmount_3 = Convert.ToDecimal(dr["SurchargeAmount_3"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return saleinvoiceTaxes;
        }
        public List<SaleInvoiceTermViewModel> GetSaleInvoiceTermList(long saleinvoiceId)
        {
            List<SaleInvoiceTermViewModel> saleinvoiceTerms = new List<SaleInvoiceTermViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCustomers = sqlDbInterface.GetSaleInvoiceTermList(saleinvoiceId);
                if (dtCustomers != null && dtCustomers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCustomers.Rows)
                    {
                        saleinvoiceTerms.Add(new SaleInvoiceTermViewModel
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
            return saleinvoiceTerms;
        }

     
        
        public DataTable GetSaleInvoiceDetailDataTable(long siId = 0)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtQuotation = new DataTable();
            try
            {
                dtQuotation = sqlDbInterface.GetSaleInvoiceDetail(siId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtQuotation;
        }
        public DataTable GetSaleInvoiceProductListDataTable(long siId)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtProducts = new DataTable();
            try
            {
                dtProducts = sqlDbInterface.GetSaleInvoiceProductList(siId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtProducts;
        }
        public DataTable GetSaleInvoiceTermListDataTable(long siId)
        {

            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtTerms = new DataTable();
            try
            {
                dtTerms = sqlDbInterface.GetSaleInvoiceTermList(siId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtTerms;
        }
        public DataTable GetSaleInvoiceTaxListDataTable(long siId)
        {

            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtTerms = new DataTable();
            try
            {
                dtTerms = sqlDbInterface.GetSaleInvoiceTaxList(siId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtTerms;
        }

        public DataTable GetSaleInvoiceProductSerialDetailDataTable(long siId)
        {
            DataTable dtSaleInvoiceProductSerials;
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                dtSaleInvoiceProductSerials = sqlDbInterface.GetSaleInvoiceProductSerialDetailList(siId);
                
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtSaleInvoiceProductSerials;
        }

        public DataTable GetSaleInvoiceProductSerialFormList(long siId)
        {
            DataTable dtSaleInvoiceProductSerialForms;
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                dtSaleInvoiceProductSerialForms = sqlDbInterface.GetSaleInvoiceProductSerialFormList(siId);

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtSaleInvoiceProductSerialForms;

        }

        public ResponseOut CancelSaleInvoice(SaleInvoiceViewModel saleinvoiceViewModel)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            ResponseOut responseOut = new ResponseOut();
            try
            {
                SaleInvoice saleinvoice = new SaleInvoice
                {
                    InvoiceId = saleinvoiceViewModel.InvoiceId,
                    InvoiceNo=saleinvoiceViewModel.InvoiceNo,
                    CancelStatus = "Cancel",
                    ApprovalStatus = "Cancelled",
                    CreatedBy = saleinvoiceViewModel.CreatedBy, 
                    CancelReason=saleinvoiceViewModel.CancelReason,
                    CompanyId = saleinvoiceViewModel.CompanyId,
                    FinYearId = saleinvoiceViewModel.FinYearId
                };
                responseOut = sqlDbInterface.CancelSI(saleinvoice);
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
        public List<SISupportingDocumentViewModel> GetSISupportingDocumentList(long siId)
        {
            List<SISupportingDocumentViewModel> siDocuments = new List<SISupportingDocumentViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtDocument = sqlDbInterface.GetSISupportingDocumentList(siId);
                if (dtDocument != null && dtDocument.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtDocument.Rows)
                    {
                        siDocuments.Add(new SISupportingDocumentViewModel
                        {
                            SaleInvoiceDocId = Convert.ToInt32(dr["SaleInvoiceDocId"]),
                            DocumentSequenceNo = Convert.ToInt32(dr["SNo"]),
                            DocumentTypeId = Convert.ToInt32(dr["DocumentTypeId"]),
                            DocumentTypeDesc = Convert.ToString(dr["DocumentTypeDesc"]),
                            DocumentName = Convert.ToString(dr["DocumentName"]),
                            DocumentPath = Convert.ToString(dr["DocumentPath"]),
                            LabourCode = Convert.ToString(dr["LabourCode"]),
                            Description = Convert.ToString(dr["Description"]),
                            LabourRate = Convert.ToDecimal(dr["LabourRate"]),
                            DiscountPerc = Convert.ToDecimal(dr["DiscountPerc"]),
                            DiscountAmount = Convert.ToDecimal(dr["DiscountAmount"]),
                            CGST_Perc = Convert.ToDecimal(dr["CGST_Perc"]),
                            CGST_Amount = Convert.ToDecimal(dr["CGST_Amount"]),
                            SGST_Perc = Convert.ToDecimal(dr["SGST_Perc"]),
                            SGST_Amount = Convert.ToDecimal(dr["SGST_Amount"]),
                            IGST_Perc = Convert.ToDecimal(dr["IGST_Perc"]),
                            IGST_Amount = Convert.ToDecimal(dr["IGST_Amount"]),
                            TotalAmount = Convert.ToDecimal(dr["TotalAmount"]),
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return siDocuments;
        }
        public List<GSTR1ViewModel> GetGSTR1B2BList(string fromDate, string toDate, int companyId)
        {
            List<GSTR1ViewModel> gSTR1s = new List<GSTR1ViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtGSTR1 = sqlDbInterface.GetGSTR1B2B(Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId);
                if (dtGSTR1 != null && dtGSTR1.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtGSTR1.Rows)
                    {
                        gSTR1s.Add(new GSTR1ViewModel
                        {
                            RecipientCount = Convert.ToInt32(dr["RecipientCount"]),
                            InvoiceCount = Convert.ToInt32(dr["InvoiceCount"]),
                            TotalInvoiceValue = Convert.ToDecimal(dr["TotalInvoiceValue"]),
                            TotalTaxableValue = Convert.ToDecimal(dr["TotalTaxableValue"]),
                            ConsigneGSTNo = Convert.ToString(dr["ConsigneGSTNo"]),
                            InvoiceNo = Convert.ToString(dr["InvoiceNo"]),
                            InvoiceDate = Convert.ToString(dr["InvoiceDate"]),
                            InvoiceValue = Convert.ToDecimal(dr["InvoiceValue"]),
                            PlaceOfSupply = Convert.ToString(dr["PlaceOfSupply"]),
                            ReverseCharge = Convert.ToString(dr["ReverseCharge"]),
                            InvoiceType = Convert.ToString(dr["InvoiceType"]),
                            EcommerceGSTNo = Convert.ToString(dr["EcommerceGSTNo"]),
                            Rate = Convert.ToDecimal(dr["Rate"]),
                            TaxableValue = Convert.ToDecimal(dr["TaxableValue"]),
                            CessValue = Convert.ToDecimal(dr["CessValue"])
                        });

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return gSTR1s;
        }
        public DataTable GetGSTR1B2BDataTable(string fromDate, string toDate, int companyId)
        {
            DataTable dtGSTR1B2Bs = new DataTable();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                dtGSTR1B2Bs = sqlDbInterface.GetGSTR1B2B(Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId);

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtGSTR1B2Bs;
        }

        public DataTable GetGSTR1B2CLDataTable(string fromDate, string toDate, int companyId)
        {
            DataTable dtGSTR1B2Bs = new DataTable();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                dtGSTR1B2Bs = sqlDbInterface.GetGSTR1B2CL(Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId);

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtGSTR1B2Bs;
        }
        public DataTable GetGSTR1B2CSDataTable(string fromDate, string toDate, int companyId)
        {
            DataTable dtGSTR1B2Bs = new DataTable();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                dtGSTR1B2Bs = sqlDbInterface.GetGSTR1B2CS(Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId);

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtGSTR1B2Bs;
        }

        public DataTable GetGSTR1CDNRDataTable(string fromDate, string toDate, int companyId)
        {
            DataTable dtGSTR1CDNRs = new DataTable();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                dtGSTR1CDNRs = sqlDbInterface.GetGSTR1CDNR(Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId);

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtGSTR1CDNRs;
        }

        public DataTable GetGSTR1CDNURDataTable(string fromDate, string toDate, int companyId)
        {
            DataTable dtGSTR1CDNURs = new DataTable();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                dtGSTR1CDNURs = sqlDbInterface.GetGSTR1CDNUR(Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId);

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtGSTR1CDNURs;
        }

        public List<SOProductViewModel> GetSISOProductList(long soId)
        {
            List<SOProductViewModel> soProducts = new List<SOProductViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCustomers = sqlDbInterface.GetSISOProductList(soId);
                if (dtCustomers != null && dtCustomers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCustomers.Rows)
                    {
                        soProducts.Add(new SOProductViewModel
                        {
                            SOProductDetailId = Convert.ToInt32(dr["SOProductDetailId"]),
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
                            IsSerializedProduct = Convert.ToString(dr["IsSerializedProduct"]),
                            IsThirdPartyProduct = Convert.ToString(dr["IsThirdPartyProduct"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return soProducts;
        }

        public DataTable GetChassisNoSoldDetailsDataTable(string customerName, long productId, int productSubGroupId, int companyId, string chassisNo,string invoiceNo, DateTime fromDate, DateTime toDate,int companyBranchId)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtChassisNoSoldDetails = new DataTable();
            try
            {
                dtChassisNoSoldDetails = sqlDbInterface.GetChassisNoSoldDetails(customerName, productId, productSubGroupId, companyId, chassisNo, invoiceNo, fromDate, toDate, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtChassisNoSoldDetails;
        }

        public DataTable GetSubGroupWiseChassisNoSoldDetailsDataTable(int productSubGroupId, int companyId, DateTime fromDate, DateTime toDate,int companyBranchId)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtChassisNoSoldDetails = new DataTable();
            try
            {
                dtChassisNoSoldDetails = sqlDbInterface.GetSubGroupdWiseChassisNoSoldDetails(productSubGroupId, companyId, fromDate, toDate, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtChassisNoSoldDetails;
        }

        public DataTable GetGSTR3BDataTable(string fromDate, string toDate, int companyId)
        {
            DataTable dtGSTR1CDNRs = new DataTable();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                dtGSTR1CDNRs = sqlDbInterface.GetGSTR3B(Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId);

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtGSTR1CDNRs;
        }

        public DataTable GetGSTR3BTempDataTable(string fromDate, string toDate, int companyId)
        {
            DataTable dtGSTR3 = new DataTable();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                dtGSTR3 = sqlDbInterface.GetGSTR3BTemp(Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId);

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtGSTR3;
        }

        public DataTable GetGSTR3BITCDataTable(string fromDate, string toDate, int companyId)
        {
            DataTable dtGSTR3 = new DataTable();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                dtGSTR3 = sqlDbInterface.GetGSTR3BITC(Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId);

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtGSTR3;
        }

        public DataTable GetGSTR3NONGSTInwardSupplies(string fromDate, string toDate, int companyId)
        {
            DataTable dtGSTR3 = new DataTable();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                dtGSTR3 = sqlDbInterface.GetGSTR3NONGSTInwardSupplies(Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId);

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtGSTR3;
        }
        public List<SaleInvoiceViewModel> GetPrpductSaleList(long productID,string companyBranch)
        {
            List<SaleInvoiceViewModel> silist = new List<SaleInvoiceViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtPOs = sqlDbInterface.GetPrpductSaleList(productID, companyBranch);
                if (dtPOs != null && dtPOs.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPOs.Rows)
                    {
                        silist.Add(new SaleInvoiceViewModel
                        {

                            InvoiceNo = Convert.ToString(dr["InvoiceNo"]),
                            InvoiceDate = Convert.ToString(dr["InvoiceDate"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
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
            return silist;
        }

        public List<SaleInvoiceViewModel> GetChallanSaleInvoiceList(string saleinvoiceNo, string customerName, string challanNo, string fromDate , int companyBranchId, string toDate = "", string saleType = "", string displayType = "", string approvalStatus = "", string customerCode = "")
        {
            List<SaleInvoiceViewModel> saleinvoices = new List<SaleInvoiceViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtSaleInvoices = sqlDbInterface.GetChallanSaleInvoiceList(saleinvoiceNo, customerName, challanNo, Convert.ToDateTime(fromDate), companyBranchId,Convert.ToDateTime(toDate),  saleType, displayType, approvalStatus, customerCode);
                if (dtSaleInvoices != null && dtSaleInvoices.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtSaleInvoices.Rows)
                    {
                        saleinvoices.Add(new SaleInvoiceViewModel
                        {
                           InvoiceId = Convert.ToInt32(dr["InvoiceId"]),
                            InvoiceNo = Convert.ToString(dr["InvoiceNo"]),
                            ChallanNo = Convert.ToString(dr["ChallanNo"]),
                            //InvoiceDate = Convert.ToString(dr["InvoiceDate"]),
                            //    SONo = Convert.ToString(dr["SONo"]),
                            //InvoiceType = Convert.ToString(dr["InvoiceType"]),
                            //CustomerId = Convert.ToInt32(dr["CustomerId"]),
                            //CustomerCode = Convert.ToString(dr["CustomerCode"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            //ConsigneeId = Convert.ToInt32(dr["ConsigneeId"]),
                            //ConsigneeName = Convert.ToString(dr["ConsigneeName"]),
                            //ConsigneeCode = Convert.ToString(dr["ConsigneeCode"]),
                           // CompanyBranchName = Convert.ToString(dr["BranchName"]),
                            //City = Convert.ToString(dr["City"]),
                            //StateName = Convert.ToString(dr["StateName"]),
                            //SaleType = Convert.ToString(dr["SaleType"]),
                            //RefNo = Convert.ToString(dr["RefNo"]),
                            //RefDate = Convert.ToString(dr["RefDate"]),
                            //BasicValue = Convert.ToDecimal(dr["BasicValue"]),
                            //BasicAmt = Convert.ToDecimal(dr["BasicAmt"]),
                            //TotalValue = Convert.ToDecimal(dr["TotalValue"]),
                            //GrossValue = Convert.ToDecimal(dr["GrossValue"]),
                            //RoundOfValue = Convert.ToDecimal(dr["RoundOfValue"]),
                           // ApprovalStatus = Convert.ToString(dr["ApprovalStatus"]),
                            //CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                           // CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            //ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            //ModifiedDate = Convert.ToString(dr["ModifiedDate"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return saleinvoices;
        }


        public DataTable GetCancelSaleInvoices(string fromDate, string toDate, string companyBranch, string customerName, int companyId)
        {
            DataTable dtCancelSaleInvoices = new DataTable();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                dtCancelSaleInvoices = sqlDbInterface.GetCancelSaleInvoices(Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate),companyBranch,customerName, companyId);

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtCancelSaleInvoices;
        }

        public DataTable GetCancelSaleOrders(string fromDate, string toDate,string companyBranch, string customerName, int companyId)
        {
            DataTable dtCancelSaleOrders = new DataTable();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                dtCancelSaleOrders = sqlDbInterface.GetCancelSaleOrders(Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyBranch, customerName, companyId);

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtCancelSaleOrders;
        }

        public List<SaleInvoiceViewModel> GetSIList(string saleinvoiceNo, string customerName, string refNo, string fromDate, string toDate, int companyId, string displayType = "", string approvalStatus = "", int companyBranchId = 0, string CreatedByUserName = "")
        {
            List<SaleInvoiceViewModel> saleinvoices = new List<SaleInvoiceViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtSaleInvoices = sqlDbInterface.GetSIList(saleinvoiceNo, customerName, refNo, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, displayType, approvalStatus, companyBranchId, CreatedByUserName);
                if (dtSaleInvoices != null && dtSaleInvoices.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtSaleInvoices.Rows)
                    {
                        saleinvoices.Add(new SaleInvoiceViewModel
                        {
                            InvoiceId = Convert.ToInt32(dr["InvoiceId"]),
                            InvoiceNo = Convert.ToString(dr["InvoiceNo"]),
                            InvoiceDate = Convert.ToString(dr["InvoiceDate"]),
                            SONo = Convert.ToString(dr["SONo"]),
                            InvoiceType = Convert.ToString(dr["InvoiceType"]),
                            CustomerId = Convert.ToInt32(dr["CustomerId"]),
                            CustomerCode = Convert.ToString(dr["CustomerCode"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            ConsigneeId = Convert.ToInt32(dr["ConsigneeId"]),
                            ConsigneeName = Convert.ToString(dr["ConsigneeName"]),
                            ConsigneeCode = Convert.ToString(dr["ConsigneeCode"]),
                            CompanyBranchName = Convert.ToString(dr["BranchName"]),
                            City = Convert.ToString(dr["City"]),
                            StateName = Convert.ToString(dr["StateName"]),
                            SaleType = Convert.ToString(dr["SaleType"]),
                            RefNo = Convert.ToString(dr["RefNo"]),
                            RefDate = Convert.ToString(dr["RefDate"]),
                            BasicValue = Convert.ToDecimal(dr["BasicValue"]),
                            BasicAmt = Convert.ToDecimal(dr["BasicAmt"]),
                            TotalValue = Convert.ToDecimal(dr["TotalValue"]),
                            GrossValue = Convert.ToDecimal(dr["GrossValue"]),
                            RoundOfValue = Convert.ToDecimal(dr["RoundOfValue"]),
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
            return saleinvoices;
        }
        public List<SaleInvoiceProductViewModel> GetSIProductList(long saleinvoiceId)
        {
            List<SaleInvoiceProductViewModel> saleinvoiceProducts = new List<SaleInvoiceProductViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCustomers = sqlDbInterface.GetSaleInvoiceProductList(saleinvoiceId);
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
                            IsSerializedProduct = Convert.ToString(dr["IsSerializedProduct"]),
                            IsThirdPartyProduct = Convert.ToString(dr["IsThirdPartyProduct"]),
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

        public List<SaleInvoiceProductViewModel> GetSaleInvoiceJobCardProductList(long jobcardId)
        {
            List<SaleInvoiceProductViewModel> saleinvoiceProducts = new List<SaleInvoiceProductViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCustomers = sqlDbInterface.GetSaleInvoiceJobCardProductList(jobcardId);
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
                            IsSerializedProduct = Convert.ToString(dr["IsSerializedProduct"]),
                            IsThirdPartyProduct = Convert.ToString(dr["IsThirdPartyProduct"]),
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
        public DataTable GetSaleInvoiceDocumentList(long siId)
        {
            DataTable dtSaleInvoiceProductSerials;
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                dtSaleInvoiceProductSerials = sqlDbInterface.GetSISupportingDocumentList(siId);

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtSaleInvoiceProductSerials;
        }
        public DataTable GetSIServiceItmeList(long siId)
        {
            DataTable dtSaleInvoiceProductSerials;
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                dtSaleInvoiceProductSerials = sqlDbInterface.GetSIServiceItmeList(siId);

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtSaleInvoiceProductSerials;
        }
    }
}
