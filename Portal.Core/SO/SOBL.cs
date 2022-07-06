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
    public class SOBL
    {
        DBInterface dbInterface;
        public SOBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditSO(SOViewModel soViewModel, List<SOProductViewModel> soProducts, List<SOTaxViewModel> soTaxes, List<SOTermViewModel> soTerms,List<SOSupportingDocumentViewModel> soDocuments)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                SO so = new SO
                {
                    SOId = soViewModel.SOId,
                    SODate = Convert.ToDateTime(soViewModel.SODate),
                    CompanyBranchId = soViewModel.CompanyBranchId,
                    CurrencyCode = soViewModel.CurrencyCode,
                    QuotationNo = soViewModel.QuotationNo,
                    QuotationId = soViewModel.QuotationId,
                    CustomerId = soViewModel.CustomerId,
                    CustomerName = soViewModel.CustomerName,
                    ContactPerson = soViewModel.ContactPerson,
                    BillingAddress = soViewModel.BillingAddress,
                    City = soViewModel.City,
                    StateId = soViewModel.StateId,
                    CountryId = soViewModel.CountryId,
                    PinCode = soViewModel.PinCode,
                    CSTNo = soViewModel.CSTNo,
                    TINNo = soViewModel.TINNo,
                    PANNo = soViewModel.PANNo,
                    GSTNo = soViewModel.GSTNo,
                    ExciseNo = soViewModel.ExciseNo,
                    Email = soViewModel.Email,
                    MobileNo = soViewModel.MobileNo,
                    ContactNo = soViewModel.ContactNo,
                    Fax = soViewModel.Fax,
                    ConsigneeId = soViewModel.ConsigneeId,
                    ConsigneeName = soViewModel.ConsigneeName,
                    ShippingContactPerson = soViewModel.ShippingContactPerson,
                    ShippingBillingAddress = soViewModel.ShippingBillingAddress,
                    ShippingCity = soViewModel.ShippingCity,
                    ShippingStateId = soViewModel.ShippingStateId,
                    ShippingCountryId = soViewModel.ShippingCountryId,
                    ShippingPinCode = soViewModel.ShippingPinCode,
                    ShippingTINNo = soViewModel.ShippingTINNo,
                    ShippingEmail = soViewModel.ShippingEmail,
                    ShippingMobileNo = soViewModel.ShippingMobileNo,
                    ShippingContactNo = soViewModel.ShippingContactNo,
                    ShippingFax = soViewModel.ShippingFax,
                    RefNo = soViewModel.RefNo,
                    RefDate = string.IsNullOrEmpty(soViewModel.RefDate) ? Convert.ToDateTime("01-01-1900") : Convert.ToDateTime(soViewModel.RefDate),
                    BasicValue = soViewModel.BasicValue,
                    TotalValue = soViewModel.TotalValue,
                    FreightValue = soViewModel.FreightValue,
                    LoadingValue = soViewModel.LoadingValue,
                    PayToBookId = soViewModel.PayToBookId,
                    Remarks1 = string.IsNullOrEmpty(soViewModel.Remarks1) ? "" : soViewModel.Remarks1,
                    Remarks2 = string.IsNullOrEmpty(soViewModel.Remarks2) ? "": soViewModel.Remarks2,
                    FinYearId = soViewModel.FinYearId,
                    CompanyId = soViewModel.CompanyId,
                    ApprovalStatus=soViewModel.ApprovalStatus,
                    CreatedBy = soViewModel.CreatedBy,
                    ReverseChargeApplicable= Convert.ToBoolean(soViewModel.ReverseChargeApplicable),
                    ReverseChargeAmount= soViewModel.RevserseChargeAmount,
                    InsuranceValue= soViewModel.InsuranceValue,
                    FreightCGST_Amt=soViewModel.FreightCGST_Amt,
                    FreightSGST_Amt=soViewModel.FreightSGST_Amt,
                    FreightIGST_Amt=soViewModel.FreightIGST_Amt,
                    LoadingCGST_Amt=soViewModel.LoadingCGST_Amt,
                    LoadingSGST_Amt= soViewModel.LoadingSGST_Amt,
                    LoadingIGST_Amt=soViewModel.LoadingIGST_Amt,
                    InsuranceCGST_Amt=soViewModel.InsuranceCGST_Amt,
                    InsuranceSGST_Amt=soViewModel.InsuranceSGST_Amt,
                    InsuranceIGST_Amt=soViewModel.InsuranceIGST_Amt,
                    FreightCGST_Perc=soViewModel.FreightCGST_Perc,
                    FreightSGST_Perc=soViewModel.FreightSGST_Perc,
                    FreightIGST_Perc = soViewModel.FreightIGST_Perc,
                    LoadingCGST_Perc= soViewModel.LoadingCGST_Perc,
                    LoadingSGST_Perc=soViewModel.LoadingSGST_Perc,
                    LoadingIGST_Perc= soViewModel.LoadingIGST_Perc,
                    InsuranceCGST_Perc=soViewModel.InsuranceCGST_Perc,
                    InsuranceSGST_Perc=soViewModel.InsuranceSGST_Perc,
                    InsuranceIGST_Perc=soViewModel.InsuranceIGST_Perc,
                    AdharcardNo= soViewModel.AdharcardNo,
                    Pancard= soViewModel.Pancard,
                    IdtypeName= soViewModel.IdtypeName,
                    IdtypeValue= soViewModel.IdtypeValue,
                    RtoRegsValue = soViewModel.RtoRegsValue,
                    RtoRegsCGST_Amt = soViewModel.RtoRegsCGST_Amt,
                    RtoRegsSGST_Amt = soViewModel.RtoRegsSGST_Amt,
                    RtoRegsIGST_Amt = soViewModel.RtoRegsIGST_Amt,
                    RtoRegsCGST_Perc = soViewModel.RtoRegsCGST_Perc,
                    RtoRegsSGST_Perc = soViewModel.RtoRegsSGST_Perc,
                    RtoRegsIGST_Perc = soViewModel.RtoRegsIGST_Perc,
                    VehicleInsuranceValue = soViewModel.VehicleInsuranceValue,
                    HypothecationBy=soViewModel.HypothecationBy,
                    LocationId=soViewModel.LocationId

                };
                List<SOProductDetail> soProductList = new List<SOProductDetail>();
                if (soProducts != null && soProducts.Count > 0)
                {
                    foreach (SOProductViewModel item in soProducts)
                    {
                        soProductList.Add(new SOProductDetail
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
                            SurchargeName_1 = item.SurchargeName_1,
                            SurchargePercentage_1 = item.SurchargePercentage_1,
                            SurchargeAmount_1 = item.SurchargeAmount_1,
                            SurchargeName_2 = item.SurchargeName_2,
                            SurchargePercentage_2 = item.SurchargePercentage_2,
                            SurchargeAmount_2 = item.SurchargeAmount_2,
                            SurchargeName_3 = item.SurchargeName_3,
                            SurchargePercentage_3 = item.SurchargePercentage_3,
                            SurchargeAmount_3 = item.SurchargeAmount_3,
                            CGST_Perc=item.CGST_Perc,
                            CGST_Amount=item.CGST_Amount,
                            SGST_Perc=item.SGST_Perc,
                            SGST_Amount=item.SGST_Amount,
                            IGST_Perc=item.IGST_Perc,
                            IGST_Amount=item.IGST_Amount,
                            HSN_Code=item.HSN_Code,
                            TotalPrice = item.TotalPrice,
                        });
                    }
                }

                List<SOTaxDetail> soTaxList = new List<SOTaxDetail>();
                if (soTaxes != null && soTaxes.Count > 0)
                {
                    foreach (SOTaxViewModel item in soTaxes)
                    {
                        soTaxList.Add(new SOTaxDetail
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
                List<SOTermsDetail> soTermList = new List<SOTermsDetail>();
                if (soTerms != null && soTerms.Count > 0)
                {
                    foreach (SOTermViewModel item in soTerms)
                    {
                        soTermList.Add(new SOTermsDetail
                        {
                            TermDesc = item.TermDesc,
                            TermSequence = item.TermSequence
                        });
                    }
                }


                List<SOSupportingDocument> soDocumentList = new List<SOSupportingDocument>();
                if (soDocuments != null && soDocuments.Count > 0)
                {
                    foreach (SOSupportingDocumentViewModel item in soDocuments)
                    {
                        soDocumentList.Add(new SOSupportingDocument
                        {
                            DocumentTypeId = item.DocumentTypeId,
                            DocumentName = item.DocumentName,
                            DocumentPath = item.DocumentPath
                        });
                    }
                }
                responseOut = sqlDbInterface.AddEditSO(so, soProductList, soTaxList, soTermList, soDocumentList);




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

        public List<SOViewModel> GetSOList(string soNo, string customerName, string refNo, string fromDate, string toDate, int companyId, string approvalStatus = "",string displayType="",string CreatedByUserName="", int companyBranchId = 0, string dashboardList = "",int CustomerId=0,int LocationId=0)
        {
            List<SOViewModel> sos = new List<SOViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtSOs = sqlDbInterface.GetSOList(soNo, customerName, refNo, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, approvalStatus , displayType, CreatedByUserName, companyBranchId, dashboardList, CustomerId, LocationId);
                if (dtSOs != null && dtSOs.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtSOs.Rows)
                    {
                        sos.Add(new SOViewModel
                        {
                            SOId = Convert.ToInt32(dr["SOId"]),
                            SONo = Convert.ToString(dr["SONo"]),
                            SODate = Convert.ToString(dr["SODate"]),
                            CustomerId= Convert.ToInt32(dr["CustomerId"]),
                            CustomerCode= Convert.ToString(dr["CustomerCode"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            CustomerGSTNo = Convert.ToString(dr["CustomerGSTNo"]),

                            ConsigneeId = Convert.ToInt32(dr["ConsigneeId"]),
                            ConsigneeName = Convert.ToString(dr["ConsigneeName"]),
                            ConsigneeCode= Convert.ToString(dr["ConsigneeCode"]),

                            ConsigneeGSTNo = Convert.ToString(dr["ConsigneeGSTNo"]),
                            City = Convert.ToString(dr["City"]),
                            StateName = Convert.ToString(dr["StateName"]),
                            BasicAmt=Convert.ToDecimal(dr["BasicAmt"]),
                            QuotationNo = Convert.ToString(dr["QuotationNo"]),
                            RefNo = Convert.ToString(dr["RefNo"]),
                            RefDate = Convert.ToString(dr["RefDate"]),
                            BasicValue = Convert.ToDecimal(dr["BasicValue"]),
                            TotalValue = Convert.ToDecimal(dr["TotalValue"]),
                            ApprovalStatus=Convert.ToString(dr["ApprovalStatus"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            CompanyBranchId= Convert.ToInt32(dr["CompanyBranchId"]),
                            CompanyBranchName = Convert.ToString(dr["CompanyBranchName"]),
                            HypothecationBy = Convert.ToString(dr["HypothecationBy"]),
                            LocationId=Convert.ToInt32(dr["LocationId"]),
                            LocationName=Convert.ToString(dr["LocationName"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return sos;
        }
        public SOViewModel GetSODetail(long soId = 0)
        {
            SOViewModel so = new SOViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCompanies = sqlDbInterface.GetSODetail(soId);
                if (dtCompanies != null && dtCompanies.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCompanies.Rows)
                    {
                        so = new SOViewModel
                        {
                            SOId = Convert.ToInt32(dr["SOId"]),
                            SONo = Convert.ToString(dr["SONo"]),
                            SODate = Convert.ToString(dr["SODate"]),
                            QuotationId = Convert.ToInt32(dr["QuotationId"]),
                            QuotationNo = Convert.ToString(dr["QuotationNo"]),
                            QuotationDate = Convert.ToString(dr["QuotationDate"]),
                            CurrencyCode = Convert.ToString(dr["CurrencyCode"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                            CustomerId = Convert.ToInt32(dr["CustomerId"]),
                            CustomerCode = Convert.ToString(dr["CustomerCode"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),

                            ConsigneeId = Convert.ToInt32(dr["ConsigneeId"]),
                            ConsigneeName = Convert.ToString(dr["ConsigneeName"]),
                            ConsigneeCode = Convert.ToString(dr["ConsigneeCode"]),

                            ContactPerson = Convert.ToString(dr["ContactPerson"]),
                            BillingAddress = Convert.ToString(dr["BillingAddress"]),
                            City = Convert.ToString(dr["City"]),
                            StateId = Convert.ToInt32(dr["StateId"]),
                            CountryId = Convert.ToInt32(dr["CountryId"]),
                            PinCode = Convert.ToString(dr["PinCode"]),
                            TINNo = Convert.ToString(dr["TINNo"]),

                            Email = Convert.ToString(dr["Email"]),
                            MobileNo = Convert.ToString(dr["MobileNo"]),
                            ContactNo = Convert.ToString(dr["ContactNo"]),
                            Fax = Convert.ToString(dr["Fax"]),


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
                            ReverseChargeApplicable = Convert.ToBoolean(dr["ReverseChargeApplicable"]),
                            RevserseChargeAmount = Convert.ToDecimal(dr["ReverseChargeAmount"]),
                            BasicValue = Convert.ToDecimal(dr["BasicValue"]),
                            TotalValue = Convert.ToDecimal(dr["TotalValue"]),

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


             
                            PayToBookId = Convert.ToInt32(dr["PayToBookId"]),
                            PayToBookName= Convert.ToString(dr["PayToBookName"]),
                            PayToBookBranch= Convert.ToString(dr["PayToBookBranch"]),
                            
                            Remarks1 = Convert.ToString(dr["Remarks1"]),
                            Remarks2 = Convert.ToString(dr["Remarks2"]),
                            ApprovalStatus = Convert.ToString(dr["ApprovalStatus"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),

                            AdharcardNo = Convert.ToString(dr["AdharcardNo"]),
                            Pancard = Convert.ToString(dr["Pancard"]),
                            IdtypeName = Convert.ToString(dr["IdtypeName"]),
                            IdtypeValue = Convert.ToString(dr["IdtypeValue"]),
                            HypothecationBy = Convert.ToString(dr["HypothecationBy"]),

                            RtoRegsValue = Convert.ToDecimal(dr["RtoRegsValue"]),
                            RtoRegsCGST_Amt = Convert.ToDecimal(dr["RtoRegsCGST_Amt"]),
                            RtoRegsSGST_Amt = Convert.ToDecimal(dr["RtoRegsSGST_Amt"]),
                            RtoRegsIGST_Amt = Convert.ToDecimal(dr["RtoRegsIGST_Amt"]),
                            RtoRegsCGST_Perc = Convert.ToDecimal(dr["RtoRegsCGST_Perc"]),
                            RtoRegsIGST_Perc = Convert.ToDecimal(dr["RtoRegsSGST_Perc"]),
                            RtoRegsSGST_Perc = Convert.ToDecimal(dr["RtoRegsIGST_Perc"]),
                            VehicleInsuranceValue = Convert.ToDecimal(dr["VehicleInsuranceValue"]),
                            BranchType= Convert.ToString(dr["BranchType"]),
                            LocationId=Convert.ToInt32(dr["LocationId"])

                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return so;
        }
        public DataTable GetSODetailDataTable(long soId = 0)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtQuotation = new DataTable();
            try
            {
                dtQuotation = sqlDbInterface.GetSODetail(soId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtQuotation;
        }
        public DataTable GetSOProductListDataTable(long soId)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtProducts = new DataTable();
            try
            {
                dtProducts = sqlDbInterface.GetSOProductList(soId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtProducts;
        }
        public DataTable GetSOTermListDataTable(long soId)
        {

            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtTerms = new DataTable();
            try
            {
                dtTerms = sqlDbInterface.GetSOTermList(soId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtTerms;
        }
        public DataTable GetSOTaxListDataTable(long soId)
        {

            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtTerms = new DataTable();
            try
            {
                dtTerms = sqlDbInterface.GetSOTaxList(soId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtTerms;
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

        public List<SOProductViewModel> GetSOProductList(long soId)
        {
            List<SOProductViewModel> soProducts = new List<SOProductViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCustomers = sqlDbInterface.GetSOProductList(soId);
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
            return soProducts;
        }

        public List<SOTaxViewModel> GetSOTaxList(long soId)
        {
            List<SOTaxViewModel> soTaxes = new List<SOTaxViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCustomers = sqlDbInterface.GetSOTaxList(soId);
                if (dtCustomers != null && dtCustomers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCustomers.Rows)
                    {
                        soTaxes.Add(new SOTaxViewModel
                        {
                            SOTaxDetailId = Convert.ToInt32(dr["SOTaxDetailId"]),
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
            return soTaxes;
        }
        public List<SOTermViewModel> GetSOTermList(long soId)
        {
            List<SOTermViewModel> soTerms = new List<SOTermViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCustomers = sqlDbInterface.GetSOTermList(soId);
                if (dtCustomers != null && dtCustomers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCustomers.Rows)
                    {
                        soTerms.Add(new SOTermViewModel
                        {
                            SOTermDetailId = Convert.ToInt32(dr["SOTermDetailId"]),
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
            return soTerms;
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

        public List<SOSupportingDocumentViewModel> GetSOSupportingDocumentList(long soId)
        {
            List<SOSupportingDocumentViewModel> soDocuments = new List<SOSupportingDocumentViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtDocument = sqlDbInterface.GetSOSupportingDocumentList(soId);
                if (dtDocument != null && dtDocument.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtDocument.Rows)
                    {
                        soDocuments.Add(new SOSupportingDocumentViewModel
                        {
                            SODocId = Convert.ToInt32(dr["SODocId"]),
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
            return soDocuments;
        }

        public SOCountViewModel GetSOCount(int CompanyId, int FinYearId,int companyBranchId)
        {
            SOCountViewModel SO = new SOCountViewModel();
            SQLDbInterface sqldbinterface = new SQLDbInterface();
            try
            {
                DataTable dtSOList = sqldbinterface.GetDashboard_TotalSaleCount(CompanyId, FinYearId, companyBranchId);

                if (dtSOList != null && dtSOList.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtSOList.Rows)
                    {
                        SO = new SOCountViewModel
                        {
                            sOTotalCount = Convert.ToString(dr["SOCount"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return SO;
        }

      

        public SICountViewModel GetSITotalAmountSumByUser(int CompanyId, int FinYearId, int userId, int reportingUserId, int reportingRoleId)
        {
            SICountViewModel SI = new SICountViewModel();
            SQLDbInterface sqldbinterface = new SQLDbInterface();
            try
            {
                DataTable dtSIList = sqldbinterface.GetSITotalAmountSumByUser(CompanyId, FinYearId, userId,reportingUserId,reportingRoleId);

                if (dtSIList != null && dtSIList.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtSIList.Rows)
                    {
                        SI = new SICountViewModel
                        {
                            SITotalAmountSum = Convert.ToString(dr["SITotalAmountSum"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return SI;
        }

        public List<QuotationProductViewModel> GetSOQuotationProductList(long quotationId)
        {
            List<QuotationProductViewModel> soQuotationProducts = new List<QuotationProductViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCustomers = sqlDbInterface.GetSOQuotationProductList(quotationId);
                if (dtCustomers != null && dtCustomers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCustomers.Rows)
                    {
                        soQuotationProducts.Add(new QuotationProductViewModel
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
            return soQuotationProducts;
        }

        public ResponseOut CancelSO(SOViewModel sOViewModel)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            ResponseOut responseOut = new ResponseOut();
            try
            {
                SO so = new SO
                {
                    SOId = sOViewModel.SOId,
                    SONo = sOViewModel.SONo,
                    CancelStatus = "Cancel",
                    ApprovalStatus = "Cancelled",
                    CreatedBy = sOViewModel.CreatedBy,
                    CancelReason = sOViewModel.CancelReason
                };
                responseOut = sqlDbInterface.CancelSO(so);
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

    }
}
