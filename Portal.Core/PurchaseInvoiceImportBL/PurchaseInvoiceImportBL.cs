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
  
  public  class PurchaseInvoiceImportBL
    {
        DBInterface dbInterface;
        public PurchaseInvoiceImportBL()
        {
            dbInterface = new DBInterface();
        }
        public List<PurchaseInvoiceImportProductDetailViewModel> GetPurchaseInvoiceImportProductList(long InvoiceId)
        {
            List<PurchaseInvoiceImportProductDetailViewModel> piProducts = new List<PurchaseInvoiceImportProductDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtProducts = sqlDbInterface.GetPurchaseInvoiceImportProductList(InvoiceId);
                if (dtProducts != null && dtProducts.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtProducts.Rows)
                    {
                        piProducts.Add(new PurchaseInvoiceImportProductDetailViewModel
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
                            IGST_Perc = Convert.ToDecimal(dr["IGST_Perc"]),
                            IGST_Amount = Convert.ToDecimal(dr["IGST_Amount"]),
                            HSN_Code = Convert.ToString(dr["HSN_Code"]),
                            Total = Convert.ToDecimal(dr["Total"]),
                            Currency= Convert.ToString(dr["Currency"]),
                            RITC = Convert.ToString(dr["RITC"]),
                            AssValue = Convert.ToDecimal(dr["AssValue"]),
                            CTH = Convert.ToDecimal(dr["CTH"]),
                            CETH = Convert.ToString(dr["CETH"]),
                            CNotn = Convert.ToString(dr["CNotn"]),
                            ENotn = Convert.ToString(dr["ENotn"]),
                            RSP = Convert.ToString(dr["RSP"]),
                            LoadProv = Convert.ToDecimal(dr["LoadProv"]),
                            BCDAmtRs = Convert.ToDecimal(dr["BCDAmtRs"]),
                            CVDAmtRs = Convert.ToDecimal(dr["CVDAmtRs"]),
                            CustomDutyPerc= Convert.ToDecimal(dr["CustomDutyPerc"]),
                            CustomDutyRate = Convert.ToDecimal(dr["CustomDutyRate"]),
                            ExciseDutyPerc = Convert.ToDecimal(dr["ExciseDutyPerc"]),
                            ExciseDutyRate = Convert.ToDecimal(dr["ExciseDutyRate"]),
                            GSTCessPerc = Convert.ToDecimal(dr["GSTCessPerc"]),
                            GSTCessAmt = Convert.ToDecimal(dr["GSTCessAmt"]),
                            Cnsno= Convert.ToString(dr["Cnsno"]),
                            Ensno= Convert.ToString(dr["Ensno"]),
                            EducationalCessOnCVDPerc = Convert.ToDecimal(dr["EducationalCessOnCVDPerc"]),
                            EducationalCessOnCVDAmt = Convert.ToDecimal(dr["EducationalCessOnCVDAmt"]),
                            SecHigherEduCessOnCVDPerc = Convert.ToDecimal(dr["SecHigherEduCessOnCVDPerc"]),
                            SecHigherEduCessOnCVDAmt = Convert.ToDecimal(dr["SecHigherEduCessOnCVDAmt"]),
                            CustomSecHigherEduCessPerc = Convert.ToDecimal(dr["CustomSecHigherEduCessPerc"]),
                            CustomSecHigherEduCessAmt = Convert.ToDecimal(dr["CustomSecHigherEduCessAmt"]),
                            CustomEducationalCessPerc = Convert.ToDecimal(dr["CustomEducationalCessPerc"]),
                            CustomEducationalCessAmt = Convert.ToDecimal(dr["CustomEducationalCessAmt"]),
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

        public List<PurchaseInvoiceImportTermsDetailViewModel> GetPIImportTermList(long InvoiceId)
        {
            List<PurchaseInvoiceImportTermsDetailViewModel> piTerms = new List<PurchaseInvoiceImportTermsDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtTerms = sqlDbInterface.GetPITermList(InvoiceId);
                if (dtTerms != null && dtTerms.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtTerms.Rows)
                    {
                        piTerms.Add(new PurchaseInvoiceImportTermsDetailViewModel
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

        public ResponseOut AddEditPurchaseInvoiceImport(PurchaseInvoiceImportViewModel piViewModel, List<PurchaseInvoiceImportProductDetailViewModel> piProducts, List<PurchaseInvoiceImportTermsDetailViewModel> piTerms)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                PurchaseInvoiceImport pi = new PurchaseInvoiceImport
                {
                    InvoiceId = piViewModel.InvoiceId,
                    InvoiceDate = Convert.ToDateTime(piViewModel.InvoiceDate),
                    CustomStn=piViewModel.CustomStn,
                    CHA = piViewModel.CHA,
                    BENo=piViewModel.BENo,
                    BEDate=Convert.ToDateTime(piViewModel.BEDate),
                    CC = piViewModel.CC,
                    Type=piViewModel.Type,
                    POId = piViewModel.POId,
                    PONo = piViewModel.PONo,
                    CustomerInvoiceNo= piViewModel.CustomerInvoiceNo,
                    CustomerInvoiceDate = Convert.ToDateTime(piViewModel.CustomerInvoiceDate),
                    ConsigneeId = piViewModel.ConsigneeId,
                    ConsigneeName = piViewModel.ConsigneeName,
                    ShippingAddress = piViewModel.ShippingAddress,
                    ShippingCity = piViewModel.ShippingCity,
                    ShippingStateId = piViewModel.ShippingStateId,
                    ShippingCountryId = piViewModel.ShippingCountryId,
                    ShippingPinCode = piViewModel.ShippingPinCode,
                    ConsigneeGSTNo = piViewModel.ConsigneeGSTNo,

                    VendorId = piViewModel.VendorId,
                    VendorName = piViewModel.VendorName,
                    BillingAddress = piViewModel.BillingAddress,
                    CountryId=piViewModel.CountryId,
                    PinCode=piViewModel.PinCode,
                    PANNo=piViewModel.PANNo,
                    GSTNo=piViewModel.GSTNo,
                    ExciseNo=piViewModel.ExciseNo,
                    LocalIGMNo=piViewModel.LocalIGMNo,
                    LocalIGMDate =Convert.ToDateTime(piViewModel.LocalIGMDate),
                    GatewayIGMNo=piViewModel.GatewayIGMNo,
                    GatewayIGMDate=Convert.ToDateTime(piViewModel.GatewayIGMDate),
                    PortOfLoading=piViewModel.PortOfLoading,
                    PortOfReporing=piViewModel.PortOfReporing,
                    CountryOfOrigin= piViewModel.CountryOfOrigin,
                    CountryOfConsignee=piViewModel.CountryOfConsignee,
                    BLNo=piViewModel.BLNo,
                    BLDate=Convert.ToDateTime(piViewModel.BLDate),
                    HBLNo=piViewModel.HBLNo,
                    HBLDate = Convert.ToDateTime(piViewModel.HBLDate),
                    NoOfPkgs= piViewModel.NoOfPkgs,
                    NoOfPkgsUnit=piViewModel.NoOfPkgsUnit,
                    GrossWt=piViewModel.GrossWt,
                    GrossWtUnit=piViewModel.GrossWtUnit,
                    Mark=piViewModel.Mark,
                    SupplierInvoiceNo=piViewModel.SupplierInvoiceNo,
                    SupplierInvoiceDate=Convert.ToDateTime(piViewModel.SupplierInvoiceDate),
                    InvoiceValue=piViewModel.InvoiceValue,
                    InvoiceValueCurrency=piViewModel.InvoiceValueCurrency,
                    TOI=piViewModel.TOI,
                    Freight=piViewModel.Freight,
                    FreightCurrency=piViewModel.FreightCurrency,
                    Insurence=piViewModel.Insurence,
                    InsurenceCurrency=piViewModel.InsurenceCurrency,
                    SVBLoadingASS=piViewModel.SVBLoadingASS,
                    SVBLoadingDty=piViewModel.SVBLoadingDty,
                    CustHouseNo=piViewModel.CustHouseNo,
                    HSSLoadRate=piViewModel.HSSLoadRate,
                    HSSAmount=piViewModel.HSSAmount,
                    MiscCharges=piViewModel.MiscCharges,
                    EDD= piViewModel.EDD,
                    ThirdParty=piViewModel.ThirdParty,
                    XBEDutyFGInt=piViewModel.XBEDutyFGInt,
                    BuyerSellarRelted=piViewModel.BuyerSellarRelted,
                    PaymentMethod=piViewModel.PaymentMethod,
                    RefNo=piViewModel.RefNo,
                    RefDate=Convert.ToDateTime(piViewModel.RefDate),
                    CompanyBranchId=piViewModel.CompanyBranchId,
                    Remarks =piViewModel.Remarks,
                    FinYearId=piViewModel.FinYearId,
                    InvoiceStatus=piViewModel.InvoiceStatus,
                    EducationalCessOnCVDPerc=piViewModel.EducationalCessOnCVDPerc,
                    EducationalCessOnCVDAmt=piViewModel.EducationalCessOnCVDAmt,
                    SecHigherEduCessOnCVDPerc=piViewModel.SecHigherEduCessOnCVDPerc,
                    SecHigherEduCessOnCVDAmt=piViewModel.SecHigherEduCessOnCVDAmt,
                    CustomSecHigherEduCessPerc=piViewModel.CustomSecHigherEduCessPerc,
                    CustomSecHigherEduCessAmt=piViewModel.CustomSecHigherEduCessAmt,
                    CustomEducationalCessPerc=piViewModel.CustomEducationalCessPerc,
                    CustomEducationalCessAmt=piViewModel.CustomEducationalCessAmt,
                    BasicValue = piViewModel.BasicValue,
                    TotalValue = piViewModel.TotalValue,
                    RoundOfValue=piViewModel.RoundOfValue,
                    GrossValue=piViewModel.GrossValue,
                    CompanyId=piViewModel.CompanyId,
                    CreatedBy = piViewModel.CreatedBy,
                    ModifiedBy=piViewModel.ModifiedBy,
                    ADCode= piViewModel.AdCode,
                    ExchangeRate = piViewModel.ExchangeRate,
                    ExchangeRatecurrencyName = piViewModel.ExchangeRatecurrencyName,
                    ForeigncurrencyValue= piViewModel.ForeigncurrencyValue,
                    ForeigncurrencyName= piViewModel.ForeigncurrencyName


                };

                List<PurchaseInvoiceImportProductDetail> piProductList = new List<PurchaseInvoiceImportProductDetail>();
                if (piProducts != null && piProducts.Count > 0)
                {
                    foreach (PurchaseInvoiceImportProductDetailViewModel item in piProducts)
                    {
                        piProductList.Add(new PurchaseInvoiceImportProductDetail
                        {
                            ProductId = item.ProductId,
                            ProductShortDesc = item.ProductShortDesc,
                            Price = item.Price,
                            Quantity = item.Quantity,
                            Currency=item.Currency,
                            DiscountPercentage = item.DiscountAmount,
                            DiscountAmount = item.DiscountAmount,
                            IGST_Perc=item.IGST_Perc,
                            IGST_Amount=item.IGST_Amount,
                            HSN_Code=item.HSN_Code,
                            Total = item.Total,
                            RITC=item.RITC,
                            AssValue=item.AssValue,
                            CTH=item.CTH,
                            CETH=item.CETH,
                            CNotn=item.CNotn,
                            ENotn=item.ENotn,
                            RSP=item.RSP,
                            LoadProv=item.LoadProv,
                            BCDAmtRs=item.BCDAmtRs,
                            CVDAmtRs=item.CVDAmtRs,
                            CustomDutyPerc=item.CustomDutyPerc,
                            CustomDutyRate=item.CustomDutyRate,
                            ExciseDutyPerc=item.ExciseDutyPerc,
                            ExciseDutyRate=item.ExciseDutyRate,
                            GSTCessPerc=item.GSTCessPerc,
                            GSTCessAmt=item.GSTCessAmt,
                            CNSNO= item.Cnsno,
                            ENSNO=item.Ensno,
                            EducationalCessOnCVDPerc = item.EducationalCessOnCVDPerc,
                            EducationalCessOnCVDAmt = item.EducationalCessOnCVDAmt,
                            SecHigherEduCessOnCVDPerc = item.SecHigherEduCessOnCVDPerc,
                            SecHigherEduCessOnCVDAmt = item.SecHigherEduCessOnCVDAmt,
                            CustomSecHigherEduCessPerc = item.CustomSecHigherEduCessPerc,
                            CustomSecHigherEduCessAmt = item.CustomSecHigherEduCessAmt,
                            CustomEducationalCessPerc = item.CustomEducationalCessPerc,
                            CustomEducationalCessAmt = item.CustomEducationalCessAmt,
                        });
                    }
                }
                
                List<PurchaseInvoiceImportTermsDetail> piTermList = new List<PurchaseInvoiceImportTermsDetail>();
                if (piTerms != null && piTerms.Count > 0)
                {
                    foreach (PurchaseInvoiceImportTermsDetailViewModel item in piTerms)
                    {
                        piTermList.Add(new PurchaseInvoiceImportTermsDetail
                        {
                            TermDesc = item.TermDesc,
                            TermSequence = item.TermSequence
                        });
                    }
                }

               responseOut = sqlDbInterface.AddEditPurchaseInvoiceImport(pi, piProductList, piTermList);
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

        public PurchaseInvoiceImportViewModel GetPurchaseInvoiceImportDetail(long invoiceId = 0)
        {
            PurchaseInvoiceImportViewModel pi = new PurchaseInvoiceImportViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtpos = sqlDbInterface.GetPurchaseInvoiceImportDetail(invoiceId);
                if (dtpos != null && dtpos.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtpos.Rows)
                    {
                        pi = new PurchaseInvoiceImportViewModel
                        {
                            InvoiceId = Convert.ToInt32(dr["InvoiceId"]),
                            InvoiceNo = Convert.ToString(dr["InvoiceNo"]),
                            InvoiceDate = Convert.ToString(dr["InvoiceDate"]),
                            CustomStn= Convert.ToString(dr["CustomStn"]),
                            CHA = Convert.ToString(dr["CHA"]),
                            BENo = Convert.ToString(dr["BENo"]),
                            BEDate = Convert.ToString(dr["BEDate"]),
                            CC = Convert.ToString(dr["CC"]),
                            Type = Convert.ToString("Type"),
                            POId = Convert.ToInt32(dr["POId"]),
                            PONo = Convert.ToString(dr["PONo"]),
                            CustomerInvoiceNo = Convert.ToString(dr["CustomerInvoiceNo"]),
                            CustomerInvoiceDate = Convert.ToString(dr["CustomerInvoiceDate"]),

                            ConsigneeId = Convert.ToInt32(dr["ConsigneeId"]),
                            ConsigneeName = Convert.ToString(dr["ConsigneeName"]),
                            ConsigneeCode = Convert.ToString(dr["ConsigneeCode"]),
                            ShippingAddress = Convert.ToString(dr["ShippingAddress"]),
                            CountryId = Convert.ToInt32(dr["CountryId"]),
                            VendorId = Convert.ToInt32(dr["VendorId"]),
                            VendorCode = Convert.ToString(dr["VendorCode"]),
                            VendorName = Convert.ToString(dr["VendorName"]),
                            BillingAddress = Convert.ToString(dr["BillingAddress"]),
                            PANNo = Convert.ToString(dr["PANNo"]),
                            GSTNo = Convert.ToString(dr["GSTNo"]),
                            PinCode = Convert.ToString(dr["PinCode"]),
                            RefNo = Convert.ToString(dr["RefNo"]),
                            RefDate = Convert.ToString(dr["RefDate"]),
                            BasicValue = Convert.ToDecimal(dr["BasicValue"]),
                            TotalValue = Convert.ToDecimal(dr["TotalValue"]),
                            GrossValue = Convert.ToDecimal(dr["GrossValue"]),
                            RoundOfValue = Convert.ToDecimal(dr["RoundOfValue"]),
                            ShippingCity = Convert.ToString(dr["ShippingCity"]),
                            ShippingStateId = Convert.ToInt32(dr["ShippingStateId"]),
                            ShippingCountryId = Convert.ToInt32(dr["ShippingCountryId"]),
                            ShippingPinCode = Convert.ToString(dr["ShippingPinCode"]),
                            ConsigneeGSTNo = Convert.ToString(dr["ConsigneeGSTNo"]),
                            SupplierInvoiceNo = Convert.ToString(dr["SupplierInvoiceNo"]),
                            SupplierInvoiceDate = Convert.ToString(dr["SupplierInvoiceDate"]),
                            LocalIGMNo = Convert.ToString(dr["LocalIGMNo"]),
                            LocalIGMDate = Convert.ToString(dr["LocalIGMDate"]),
                            GatewayIGMNo = Convert.ToString(dr["GatewayIGMNo"]),
                            GatewayIGMDate = Convert.ToString(dr["GatewayIGMDate"]),
                            PortOfLoading = Convert.ToString(dr["PortOfLoading"]),
                            PortOfReporing = Convert.ToString(dr["PortOfReporing"]),
                            CountryOfOrigin = Convert.ToInt32(dr["CountryOfOrigin"]),
                            CountryOfConsignee = Convert.ToInt32(dr["CountryOfConsignee"]),
                            BLNo = Convert.ToString(dr["BLNo"]),
                            BLDate = Convert.ToString(dr["BLDate"]),
                            HBLNo = Convert.ToString(dr["HBLNo"]),
                            HBLDate = Convert.ToString(dr["HBLDate"]),
                            NoOfPkgs = Convert.ToDecimal(dr["NoOfPkgs"]),
                            NoOfPkgsUnit = Convert.ToInt32(dr["NoOfPkgsUnit"]),
                            GrossWt = Convert.ToInt32(dr["GrossWt"]),
                            GrossWtUnit = Convert.ToInt32(dr["GrossWtUnit"]),
                            Mark = Convert.ToString(dr["Mark"]),
                            InvoiceValue = Convert.ToDecimal(dr["InvoiceValue"]),
                            InvoiceValueCurrency = Convert.ToString(dr["InvoiceValueCurrency"]),
                            Freight = Convert.ToDecimal(dr["Freight"]),
                            FreightCurrency = Convert.ToString(dr["FreightCurrency"]),
                            TOI= Convert.ToString(dr["TOI"]),
                            Insurence = Convert.ToDecimal(dr["Insurence"]),
                            InsurenceCurrency = Convert.ToString(dr["InsurenceCurrency"]),
                            SVBLoadingASS = Convert.ToString(dr["SVBLoadingASS"]),
                            SVBLoadingDty = Convert.ToString(dr["SVBLoadingDty"]),
                            CustHouseNo = Convert.ToString(dr["CustHouseNo"]),
                            HSSLoadRate = Convert.ToDecimal(dr["HSSLoadRate"]),
                            HSSAmount = Convert.ToDecimal(dr["HSSAmount"]),
                            MiscCharges = Convert.ToDecimal(dr["MiscCharges"]),
                            EDD = Convert.ToString(dr["EDD"]),
                            ThirdParty = Convert.ToString(dr["ThirdParty"]),
                            XBEDutyFGInt = Convert.ToDecimal(dr["XBEDutyFGInt"]),
                            BuyerSellarRelted = Convert.ToString(dr["BuyerSellarRelted"]),
                            PaymentMethod = Convert.ToString(dr["PaymentMethod"]),
                            ExchangeRate = Convert.ToDecimal(dr["ExchangeRate"]),
                            Remarks = Convert.ToString(dr["Remarks"]),
                            InvoiceStatus = Convert.ToString(dr["InvoiceStatus"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                            CompanyBranchName = Convert.ToString(dr["BranchName"]),
                            EducationalCessOnCVDPerc = Convert.ToDecimal(dr["EducationalCessOnCVDPerc"]),
                            EducationalCessOnCVDAmt=Convert.ToDecimal(dr["EducationalCessOnCVDAmt"]),
                            SecHigherEduCessOnCVDPerc= Convert.ToDecimal(dr["SecHigherEduCessOnCVDPerc"]),
                            SecHigherEduCessOnCVDAmt= Convert.ToDecimal(dr["SecHigherEduCessOnCVDAmt"]),
                            CustomSecHigherEduCessPerc= Convert.ToDecimal(dr["CustomSecHigherEduCessPerc"]),
                            CustomSecHigherEduCessAmt= Convert.ToDecimal(dr["CustomSecHigherEduCessAmt"]),
                            CustomEducationalCessPerc = Convert.ToDecimal(dr["CustomEducationalCessPerc"]),
                            CustomEducationalCessAmt= Convert.ToDecimal(dr["CustomEducationalCessAmt"]),
                            AdCode=Convert.ToString(dr["ADCode"]),
                            ExchangeRatecurrencyName = Convert.ToString(dr["ExchangeRatecurrencyName"]),
                            ForeigncurrencyName = Convert.ToString(dr["ForeigncurrencyName"]),
                            ForeigncurrencyValue = Convert.ToDecimal(dr["ForeigncurrencyValue"])
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

        public List<PurchaseInvoiceImportViewModel> GetPurchaseInvoiceImportList(string invoiceNo="", string vendorName="", string fromDate="", string toDate="", int companyId=0, string invoiceStatus = "", string displayType = "", string CreatedByUserName = "", string companyBranch = "")
        {
            List<PurchaseInvoiceImportViewModel> pos = new List<PurchaseInvoiceImportViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtPIs = sqlDbInterface.GetPurchaseInvoiceImportList(invoiceNo, vendorName, fromDate, toDate, companyId, invoiceStatus, displayType, CreatedByUserName,companyBranch);
                if (dtPIs != null && dtPIs.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPIs.Rows)
                    {
                        pos.Add(new PurchaseInvoiceImportViewModel
                        {
                            InvoiceId = Convert.ToInt32(dr["InvoiceId"]),
                            InvoiceNo = Convert.ToString(dr["InvoiceNo"]),
                            InvoiceDate = Convert.ToString(dr["InvoiceDate"]),
                            CustomStn = Convert.ToString(dr["CustomStn"]),
                            CHA = Convert.ToString(dr["CHA"]),
                            BENo = Convert.ToString(dr["BENo"]),
                            BEDate = Convert.ToString(dr["BEDate"]),
                            CC = Convert.ToString(dr["CC"]),
                            Type = Convert.ToString("Type"),
                            POId = Convert.ToInt32(dr["POId"]),
                            PONo = Convert.ToString(dr["PONo"]),
                            ConsigneeId = Convert.ToInt32(dr["ConsigneeId"]),
                            ConsigneeName = Convert.ToString(dr["ConsigneeName"]),
                            ConsigneeCode = Convert.ToString(dr["ConsigneeCode"]),
                            ShippingAddress = Convert.ToString(dr["ShippingAddress"]),
                            CountryId = Convert.ToInt32(dr["CountryId"]),
                            VendorId = Convert.ToInt32(dr["VendorId"]),
                            VendorCode = Convert.ToString(dr["VendorCode"]),
                            VendorName = Convert.ToString(dr["VendorName"]),
                            BillingAddress = Convert.ToString(dr["BillingAddress"]),
                            PANNo = Convert.ToString(dr["PANNo"]),
                            GSTNo = Convert.ToString(dr["GSTNo"]),
                            PinCode = Convert.ToString(dr["PinCode"]),
                            RefNo = Convert.ToString(dr["RefNo"]),
                            RefDate = Convert.ToString(dr["RefDate"]),
                            BasicValue = Convert.ToDecimal(dr["BasicValue"]),
                            TotalValue = Convert.ToDecimal(dr["TotalValue"]),
                            GrossValue = Convert.ToDecimal(dr["GrossValue"]),
                            RoundOfValue = Convert.ToDecimal(dr["RoundOfValue"]),
                            ShippingCity = Convert.ToString(dr["ShippingCity"]),
                            ShippingStateId = Convert.ToInt32(dr["ShippingStateId"]),
                            ShippingCountryId = Convert.ToInt32(dr["ShippingCountryId"]),
                            ShippingPinCode = Convert.ToString(dr["ShippingPinCode"]),
                            ConsigneeGSTNo = Convert.ToString(dr["ConsigneeGSTNo"]),
                            SupplierInvoiceNo = Convert.ToString(dr["SupplierInvoiceNo"]),
                            SupplierInvoiceDate = Convert.ToString(dr["SupplierInvoiceDate"]),
                            LocalIGMNo = Convert.ToString(dr["LocalIGMNo"]),
                            LocalIGMDate = Convert.ToString(dr["LocalIGMDate"]),
                            GatewayIGMNo = Convert.ToString(dr["GatewayIGMNo"]),
                            GatewayIGMDate = Convert.ToString(dr["GatewayIGMDate"]),
                            PortOfLoading = Convert.ToString(dr["PortOfLoading"]),
                            PortOfReporing = Convert.ToString(dr["PortOfReporing"]),
                            CountryOfOrigin = Convert.ToInt32(dr["CountryOfOrigin"]),
                            CountryOfConsignee = Convert.ToInt32(dr["CountryOfConsignee"]),
                            BLNo = Convert.ToString(dr["BLNo"]),
                            BLDate = Convert.ToString(dr["BLDate"]),
                            HBLNo = Convert.ToString(dr["HBLNo"]),
                            HBLDate = Convert.ToString(dr["HBLDate"]),
                            NoOfPkgs = Convert.ToDecimal(dr["NoOfPkgs"]),
                            NoOfPkgsUnit = Convert.ToInt32(dr["NoOfPkgsUnit"]),
                            GrossWt = Convert.ToInt32(dr["GrossWt"]),
                            GrossWtUnit = Convert.ToInt32(dr["GrossWtUnit"]),
                            Mark = Convert.ToString(dr["Mark"]),
                            InvoiceValue = Convert.ToDecimal(dr["InvoiceValue"]),
                            InvoiceValueCurrency = Convert.ToString(dr["InvoiceValueCurrency"]),
                            Freight = Convert.ToDecimal(dr["Freight"]),
                            FreightCurrency = Convert.ToString(dr["FreightCurrency"]),
                            TOI = Convert.ToString(dr["TOI"]),
                            Insurence = Convert.ToDecimal(dr["Insurence"]),
                            InsurenceCurrency = Convert.ToString(dr["InsurenceCurrency"]),
                            SVBLoadingASS = Convert.ToString(dr["SVBLoadingASS"]),
                            SVBLoadingDty = Convert.ToString(dr["SVBLoadingDty"]),
                            CustHouseNo = Convert.ToString(dr["CustHouseNo"]),
                            HSSLoadRate = Convert.ToDecimal(dr["HSSLoadRate"]),
                            HSSAmount = Convert.ToDecimal(dr["HSSAmount"]),
                            MiscCharges = Convert.ToDecimal(dr["MiscCharges"]),
                            EDD = Convert.ToString(dr["EDD"]),
                            ThirdParty = Convert.ToString(dr["ThirdParty"]),
                            XBEDutyFGInt = Convert.ToDecimal(dr["XBEDutyFGInt"]),
                            BuyerSellarRelted = Convert.ToString(dr["BuyerSellarRelted"]),
                            PaymentMethod = Convert.ToString(dr["PaymentMethod"]),
                            ExchangeRate = Convert.ToDecimal(dr["ExchangeRate"]),
                            Remarks = Convert.ToString(dr["Remarks"]),
                            InvoiceStatus = Convert.ToString(dr["InvoiceStatus"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                            CompanyBranchName = Convert.ToString(dr["BranchName"]),
                            EducationalCessOnCVDPerc = Convert.ToDecimal(dr["EducationalCessOnCVDPerc"]),
                            EducationalCessOnCVDAmt = Convert.ToDecimal(dr["EducationalCessOnCVDAmt"]),
                            SecHigherEduCessOnCVDPerc = Convert.ToDecimal(dr["SecHigherEduCessOnCVDPerc"]),
                            SecHigherEduCessOnCVDAmt = Convert.ToDecimal(dr["SecHigherEduCessOnCVDAmt"]),
                            CustomSecHigherEduCessPerc = Convert.ToDecimal(dr["CustomSecHigherEduCessPerc"]),
                            CustomSecHigherEduCessAmt = Convert.ToDecimal(dr["CustomSecHigherEduCessAmt"]),
                            CustomEducationalCessPerc = Convert.ToDecimal(dr["CustomEducationalCessPerc"]),
                            CustomEducationalCessAmt = Convert.ToDecimal(dr["CustomEducationalCessAmt"])
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


        public DataTable GetPurchaseInvoiceImportDataTable(long piId = 0)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtPurchaseInvoiceImport = new DataTable();
            try
            {
                dtPurchaseInvoiceImport = sqlDbInterface.GetPurchaseInvoiceImportDetail(piId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtPurchaseInvoiceImport;
        }
        public DataTable GetPurchaseInvoiceImportProductDataTable(long piId)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtProducts = new DataTable();
            try
            {
                dtProducts = sqlDbInterface.GetPurchaseInvoiceImportProductList(piId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtProducts;
        }

        public List<PurchaseInvoiceImportProductDetailViewModel> GetPIPOProductList(long poId)
        {
            List<PurchaseInvoiceImportProductDetailViewModel> quotationProducts = new List<PurchaseInvoiceImportProductDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtProducts = sqlDbInterface.GetPIPOImportProductList(poId);
                if (dtProducts != null && dtProducts.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtProducts.Rows)
                    {
                        quotationProducts.Add(new PurchaseInvoiceImportProductDetailViewModel
                        {
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
                            IGST_Perc = Convert.ToDecimal(dr["IGST_Perc"]),
                            IGST_Amount = Convert.ToDecimal(dr["IGST_Amount"]),
                            HSN_Code = Convert.ToString(dr["HSN_Code"]),
                            AssValue = Convert.ToDecimal(dr["AssValue"]),
                            Total = Convert.ToDecimal(dr["TotalPrice"]),
                            Currency = Convert.ToString(dr["Currency"]),
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
    }
}
