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
  public  class VendorBL
    {
        DBInterface dbInterface;
        public VendorBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditVendor(VendorViewModel vendorViewModel, List<VendorProductViewModel> vendorProducts)
        {
            ResponseOut responseOut = new ResponseOut();
            ResponseOut responseOutProduct = new ResponseOut();
            ResponseOut responseOutSL = new ResponseOut();
            using (TransactionScope transactionscope = new TransactionScope())
            {
                try
                {
                    Vendor vendor = new Vendor
                    {
                        VendorId = vendorViewModel.VendorId,
                        VendorCode = vendorViewModel.VendorCode,
                        VendorName = vendorViewModel.VendorName,
                        ContactPersonName = vendorViewModel.ContactPersonName,
                        Email = vendorViewModel.Email,
                        MobileNo = vendorViewModel.MobileNo,
                        ContactNo = vendorViewModel.ContactNo,
                        Fax = vendorViewModel.Fax,
                        Address = vendorViewModel.Address,
                        City = vendorViewModel.City,
                        StateId = vendorViewModel.StateId,
                        CountryId = vendorViewModel.CountryId,
                        PinCode = vendorViewModel.PinCode,
                        CSTNo = vendorViewModel.CSTNo,
                        TINNo = vendorViewModel.TINNo,
                        PANNo = vendorViewModel.PANNo,
                        GSTNo = vendorViewModel.GSTNo,
                        ExciseNo = vendorViewModel.ExciseNo,
                        CompanyId = vendorViewModel.CompanyId,
                        CreditLimit = vendorViewModel.CreditLimit,
                        CreditDays = vendorViewModel.CreditDays,
                        CreatedBy = vendorViewModel.CreatedBy,
                        Status = vendorViewModel.Vendor_Status,
                        AnnualTurnover = vendorViewModel.AnnualTurnover,
                        GST_Exempt = vendorViewModel.GST_Exempt,
                        IsComposition = vendorViewModel.IsComposition,
                        CompanyBranchId= vendorViewModel.CompanyBranchId
                    };


                    int VendorId = 0;
                    responseOut = dbInterface.AddEditVendor(vendor, out VendorId);
                    if (responseOut.status == ActionStatus.Success)
                    {
                        if (vendorProducts != null && vendorProducts.Count > 0)
                        {
                            foreach (VendorProductViewModel vendorProductViewModel in vendorProducts)
                            {


                                VendorProductMapping vendorProduct = new VendorProductMapping
                                {
                                    VendorId = VendorId,
                                    MappingId = vendorProductViewModel.MappingId,
                                    ProductId = vendorProductViewModel.ProductId,
                                    Status = true
                                };
                                responseOutProduct = dbInterface.AddEditVendorProduct(vendorProduct);
                            }
                        }
                    }

                    SL sl = new SL
                    {
                        SLId = 0,
                        SLCode = vendorViewModel.VendorCode,
                        SLHead = vendorViewModel.VendorName,
                        RefCode = vendorViewModel.VendorCode,
                        SLTypeId = 1,
                        CostCenterId = 0,
                        SubCostCenterId = 0,
                        CompanyId = vendorViewModel.CompanyId,
                        CreatedBy = vendorViewModel.CreatedBy,
                        Status = vendorViewModel.Vendor_Status,
                    };

                    responseOutSL = dbInterface.AddEditVendorSL(sl, vendorViewModel.VendorId==0?"Add":"Edit");
                    transactionscope.Complete();
                }
                catch (TransactionException ex1)
                {
                    transactionscope.Dispose();
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.ApplicationException;
                    Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex1);
                    throw ex1;
                }


                catch (Exception ex)
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.ApplicationException;
                    Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                    throw ex;
                }
            }
            return responseOut;
        }






        public List<VendorProductViewModel> GetVendorProductList(int vendorId)
        {           
            List<VendorProductViewModel> vendorProducts = new List<VendorProductViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCustomers = sqlDbInterface.GetVendorProductList(vendorId);
                if (dtCustomers != null && dtCustomers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCustomers.Rows)
                    {
                        vendorProducts.Add(new VendorProductViewModel
                        {
                            MappingId = Convert.ToInt32(dr["MappingId"]),
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ProductCode = Convert.ToString(dr["ProductCode"]),
                            ProductShortDesc = Convert.ToString(dr["ProductShortDesc"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return vendorProducts;
        }

        public List<VendorViewModel> GetVendorList(string vendorName, string vendorCode,string city, string state, string mobileNo,  int companyId, string vendorStatus,string companyBranch)
        {
            List<VendorViewModel> vendors = new List<VendorViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtVendors = sqlDbInterface.GetVendorList(vendorName, vendorCode, city,state, mobileNo, companyId, vendorStatus,companyBranch);
                if (dtVendors != null && dtVendors.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtVendors.Rows)
                    {
                        vendors.Add(new VendorViewModel
                        {
                            VendorId = Convert.ToInt32(dr["VendorId"]),
                            VendorCode = Convert.ToString(dr["VendorCode"]),
                            VendorName = Convert.ToString(dr["VendorName"]),
                            ContactPersonName = Convert.ToString(dr["ContactPersonName"]),
                            Email = Convert.ToString(dr["Email"]),
                            MobileNo = Convert.ToString(dr["MobileNo"]),
                            Address = Convert.ToString(dr["Address"]),
                            City = Convert.ToString(dr["City"]),
                            ExciseNo = Convert.ToString(dr["ExciseNo"]),
                            StateName = Convert.ToString(dr["StateName"]),
                            CreditLimit = Convert.ToDecimal(dr["CreditLimit"].ToString() == "" ? "0" : dr["CreditLimit"]),
                            CreditDays = Convert.ToInt32(dr["CreditDays"].ToString() == "" ? "0" : dr["CreditDays"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),  
                            Vendor_Status = Convert.ToBoolean(dr["Status"]),
                            AnnualTurnover = Convert.ToDecimal(dr["AnnualTurnover"]),
                            CompanyBranchName= Convert.ToString(dr["BranchName"])

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return vendors;
        }
        public VendorViewModel GetVendorDetail(int vendorId = 0)
        {
            VendorViewModel vendor = new VendorViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtVendors = sqlDbInterface.GetVendorDetail(vendorId);
                if (dtVendors != null && dtVendors.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtVendors.Rows)
                    {
                        vendor = new VendorViewModel
                        {
                            VendorId = Convert.ToInt32(dr["VendorId"]),
                            VendorCode = Convert.ToString(dr["VendorCode"]),
                            VendorName = Convert.ToString(dr["VendorName"]),
                            ContactPersonName = Convert.ToString(dr["ContactPersonName"]), 
                            Email = Convert.ToString(dr["Email"]),
                            MobileNo = Convert.ToString(dr["MobileNo"]),
                            ContactNo = Convert.ToString(dr["ContactNo"]),
                            Fax = Convert.ToString(dr["Fax"]),
                            Address = Convert.ToString(dr["Address"]),
                            City = Convert.ToString(dr["City"]),
                            StateId = Convert.ToInt32(dr["StateId"]),
                            CountryId = Convert.ToInt32(dr["CountryId"]),
                            PinCode = Convert.ToString(dr["PinCode"]),
                            CSTNo = Convert.ToString(dr["CSTNo"]),
                            TINNo = Convert.ToString(dr["TINNo"]),
                            PANNo = Convert.ToString(dr["PANNo"]),
                            GSTNo = Convert.ToString(dr["GSTNo"]),
                            ExciseNo = Convert.ToString(dr["ExciseNo"]),
                            CreditLimit = Convert.ToDecimal(dr["CreditLimit"].ToString() == "" ? "0" : dr["CreditLimit"]),
                            CreditDays = Convert.ToInt32(dr["CreditDays"].ToString() == "" ? "0" : dr["CreditDays"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            Vendor_Status = Convert.ToBoolean(dr["Status"]),
                            GST_Exempt = Convert.ToBoolean(dr["GST_Exempt"]),
                            AnnualTurnover = Convert.ToDecimal(dr["AnnualTurnover"]),
                            IsComposition = Convert.ToBoolean(dr["IsComposition"]),
                            CompanyBranchId= Convert.ToInt64(dr["CompanyBranchId"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return vendor;
        }

        public List<VendorViewModel> GetVendorAutoCompleteList(string searchTerm, int companyId)
        {
            List<VendorViewModel> vendors = new List<VendorViewModel>();
            try
            {
                List<Vendor> vendorList = dbInterface.GetVendorAutoCompleteList(searchTerm, companyId);

                if (vendorList != null && vendorList.Count > 0)
                {
                    foreach (Vendor vendor in vendorList)
                    {
                        vendors.Add(new VendorViewModel { VendorId = vendor.VendorId, VendorName = vendor.VendorName, VendorCode = vendor.VendorCode, Address = vendor.Address });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return vendors;
        }

        public ResponseOut ImportVendor(VendorViewModel vendorViewModel)
        {

            ResponseOut responseOut = new ResponseOut();
            ResponseOut responseOutSL = new ResponseOut(); 
            using (TransactionScope transactionscope = new TransactionScope())
            {
                try
                {

                    Vendor vendor = new Vendor
                    {
                        VendorId = vendorViewModel.VendorId,
                        VendorCode = vendorViewModel.VendorCode,
                        VendorName = vendorViewModel.VendorName,
                        ContactPersonName = vendorViewModel.ContactPersonName, 
                        Email = vendorViewModel.Email,
                        MobileNo = vendorViewModel.MobileNo,
                        ContactNo = vendorViewModel.ContactNo,
                        Fax = vendorViewModel.Fax, 
                        Address = vendorViewModel.Address,
                        City = vendorViewModel.City,
                        StateId = vendorViewModel.StateId,
                        CountryId = vendorViewModel.CountryId,
                        PinCode = vendorViewModel.PinCode,
                        CSTNo = vendorViewModel.CSTNo,
                        TINNo = vendorViewModel.TINNo,
                        PANNo = vendorViewModel.PANNo,
                        GSTNo = vendorViewModel.GSTNo,
                        ExciseNo = vendorViewModel.ExciseNo, 
                        CompanyId = vendorViewModel.CompanyId,
                        CreatedBy = vendorViewModel.CreatedBy,
                        CreditLimit = vendorViewModel.CreditLimit,
                        CreditDays = vendorViewModel.CreditDays,
                        Status = vendorViewModel.Vendor_Status,
                        GST_Exempt = vendorViewModel.GST_Exempt
                    };


                    int vendorId = 0;
                    responseOut = dbInterface.AddEditVendor(vendor, out vendorId);

                    SL sl = new SL
                    {
                        SLId = 0,
                        SLCode = vendorViewModel.VendorCode,
                        SLHead = vendorViewModel.VendorName,
                        RefCode = vendorViewModel.VendorCode,
                        SLTypeId = 1,
                        CostCenterId = 0,
                        SubCostCenterId = 0,
                        CompanyId = vendorViewModel.CompanyId,
                        CreatedBy = vendorViewModel.CreatedBy,
                        Status = true
                    };

                    responseOutSL = dbInterface.AddEditVendorSL(sl, vendorViewModel.VendorId == 0 ? "Add" : "Edit");


                    transactionscope.Complete();

                }
                catch (TransactionException ex1)
                {
                    transactionscope.Dispose();
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.ApplicationException;
                    Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex1);
                    throw ex1;
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.ApplicationException;
                    Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                    throw ex;
                }
            }
            return responseOut;
        }



        public DataTable VendorExport(string vendorName, string vendorCode, string mobileNo, string city, string state, int companyId, string vendorStatus)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtVendorExport = new DataTable();
            try
            {
                dtVendorExport = sqlDbInterface.VendorExport(vendorName, vendorCode, mobileNo,city,state,companyId, vendorStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtVendorExport;
        }

        public ResponseOut AddEditVendorMaster(VendorViewModel vendorViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            ResponseOut responseOutProduct = new ResponseOut();
            ResponseOut responseOutSL = new ResponseOut();
            using (TransactionScope transactionscope = new TransactionScope())
            {
                try
                {
                    Vendor vendor = new Vendor
                    {
                        VendorId = vendorViewModel.VendorId,
                        VendorCode = vendorViewModel.VendorCode,
                        VendorName = vendorViewModel.VendorName,
                        ContactPersonName = vendorViewModel.ContactPersonName,
                        MobileNo = vendorViewModel.MobileNo,
                        Address = vendorViewModel.Address,
                        StateId = vendorViewModel.StateId,
                        CountryId = vendorViewModel.CountryId,
                        GSTNo = vendorViewModel.GSTNo,
                        Status = vendorViewModel.Vendor_Status,
                        CompanyId = vendorViewModel.CompanyId,
                        City = vendorViewModel.City,
                        CreatedBy = vendorViewModel.CreatedBy,
                        IsComposition= vendorViewModel.IsComposition,
                        GST_Exempt= vendorViewModel.GST_Exempt,
                    };
                    responseOut = dbInterface.AddEditVendorMaster(vendor);
                    SL sl = new SL
                    {
                        SLId = 0,
                        SLCode = vendorViewModel.VendorCode,
                        SLHead = vendorViewModel.VendorName,
                        RefCode = vendorViewModel.VendorCode,
                        SLTypeId = 1,
                        CostCenterId = 0,
                        SubCostCenterId = 0,
                        CompanyId = vendorViewModel.CompanyId,
                        CreatedBy = vendorViewModel.CreatedBy,
                        Status = true
                    };
                    responseOutSL = dbInterface.AddEditVendorMasterSL(sl, "Add");
                    transactionscope.Complete();
                }
                catch (TransactionException ex1)
                {
                    transactionscope.Dispose();
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.ApplicationException;
                    Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex1);
                    throw ex1;
                }


                catch (Exception ex)
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.ApplicationException;
                    Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                    throw ex;
                }
            }
            return responseOut;
        }


        public List<VendorViewModel> GetVendorDetailsById(int vendorId, int companyId)
        {
            List<VendorViewModel> vendors = new List<VendorViewModel>();
            try
            {
                List<Vendor> vendorList = dbInterface.GetVendorDetailsById(vendorId, companyId);

                if (vendorList != null && vendorList.Count > 0)
                {
                    foreach (Vendor vendor in vendorList)
                    {
                        vendors.Add(new VendorViewModel { VendorId = vendor.VendorId, VendorName = vendor.VendorName, VendorCode = vendor.VendorCode, Address = vendor.Address });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return vendors;
        }


    }
}
