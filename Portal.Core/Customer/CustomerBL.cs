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
using Portal.Common.ViewModel;

namespace Portal.Core
{
    public class CustomerBL
    {
        DBInterface dbInterface;
        public CustomerBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditCustomer(CustomerViewModel customerViewModel, List<CustomerBranchViewModel> customerBranchs, List<CustomerProductViewModel> customerProducts,List<CustomerFollowUpViewModel> customerFollowUps)
        {
            ResponseOut responseOutBranch = new ResponseOut();
            ResponseOut responseOutProduct = new ResponseOut();
            ResponseOut responseOut = new ResponseOut();
            ResponseOut responseOutSL = new ResponseOut();
            ResponseOut responseOutFollowUp = new ResponseOut(); 
            using (TransactionScope transactionscope = new TransactionScope())
            {
                try
                { 
                    Customer customer = new Customer
                    {
                        CustomerId = customerViewModel.CustomerId,
                        CustomerCode =string.IsNullOrEmpty( customerViewModel.CustomerCode) ? this.GenerateCustomerCode(customerViewModel.StateId,customerViewModel.StateCode,customerViewModel.CustomerTypeId,customerViewModel.CustomerTypeCode):customerViewModel.CustomerCode,
                        CustomerName = customerViewModel.CustomerName,
                        ContactPersonName = customerViewModel.ContactPersonName,
                        Designation = customerViewModel.Designation,
                        Email = customerViewModel.Email,
                        MobileNo = customerViewModel.MobileNo,
                        ContactNo = customerViewModel.ContactNo,
                        Fax = customerViewModel.Fax,
                        PrimaryAddress = customerViewModel.PrimaryAddress,
                        City = customerViewModel.City,
                        StateId = customerViewModel.StateId,
                        CountryId = customerViewModel.CountryId,
                        PinCode = customerViewModel.PinCode,
                        CSTNo = customerViewModel.CSTNo,
                        TINNo = customerViewModel.TINNo,
                        PANNo = customerViewModel.PANNo,
                        GSTNo = customerViewModel.GSTNo,
                        ExciseNo = customerViewModel.ExciseNo,
                        EmployeeId = customerViewModel.EmployeeId,
                        CustomerTypeId = customerViewModel.CustomerTypeId,
                        CompanyId = customerViewModel.CompanyId,
                        CreatedBy = customerViewModel.CreatedBy,
                        CreditLimit=customerViewModel.CreditLimit,
                        CreditDays=customerViewModel.CreditDays,
                        Status = customerViewModel.Customer_Status,
                        AnnualTurnover = customerViewModel.AnnualTurnover,
                        GST_Exempt = customerViewModel.GST_Exempt,
                        IsComposition= customerViewModel.IsComposition,
                        IsUIN = customerViewModel.IsUIN,
                        UINNo = customerViewModel.UINNo,
                        CompanyBranchId= customerViewModel.CompanyBranchId,
                        SaleEmpId= customerViewModel.SaleEmpId
                    };


                    int customerId = 0;
                    responseOut = dbInterface.AddEditCustomer(customer, out customerId);


                    if (responseOut.status == ActionStatus.Success)
                    {
                        if (customerBranchs != null && customerBranchs.Count > 0)
                        {
                            foreach (CustomerBranchViewModel customerBranchViewModel in customerBranchs)
                            {
                                CustomerBranch customerBranch = new CustomerBranch
                                {
                                    CustomerId = customerId,
                                    CustomerBranchId = customerBranchViewModel.CustomerBranchId,
                                    BranchName = customerBranchViewModel.BranchName,
                                    ContactPersonName = customerBranchViewModel.ContactPersonName,
                                    Designation = customerBranchViewModel.Designation,
                                    Email = customerBranchViewModel.Email,
                                    MobileNo = customerBranchViewModel.MobileNo,
                                    ContactNo = customerBranchViewModel.ContactNo,
                                    Fax = customerBranchViewModel.Fax,
                                    PrimaryAddress = customerBranchViewModel.PrimaryAddress,
                                    City = customerBranchViewModel.City,
                                    StateId = customerBranchViewModel.StateId,
                                    CountryId = customerBranchViewModel.CountryId,
                                    PinCode = customerBranchViewModel.PinCode,
                                    CSTNo = customerBranchViewModel.CSTNo,
                                    TINNo = customerBranchViewModel.TINNo,
                                    PANNo = customerBranchViewModel.PANNo,
                                    GSTNo = customerBranchViewModel.GSTNo,
                                    AnnualTurnover = customerBranchViewModel.AnnualTurnover,
                                    Status = true
                                };
                                responseOutBranch = dbInterface.AddEditCustomerBranch(customerBranch);
                            }
                        }
                    }
                        if (customerProducts != null && customerProducts.Count > 0)
                        {
                            foreach (CustomerProductViewModel customerProductViewModel in customerProducts)
                            {
                                CustomerProductMapping customerProduct = new CustomerProductMapping
                                {
                                    CustomerId = customerId,
                                    MappingId = customerProductViewModel.MappingId,
                                    ProductId = customerProductViewModel.ProductId,
                                    Status = true
                                };
                                responseOutProduct = dbInterface.AddEditCustomerProduct(customerProduct);
                            }
                        }
                        if(customerFollowUps!=null && customerFollowUps.Count>0)
                        {
                            foreach(CustomerFollowUpViewModel customerFollowUpViewModel in customerFollowUps)
                            {
                                CustomerFollowUp customerFollowUp = new CustomerFollowUp {

                                    CustomerFollowUpId= customerFollowUpViewModel.CustomerFollowUpId,
                                    CustomerId= customerId,
                                    FollowUpActivityTypeId = customerFollowUpViewModel.FollowUpActivityTypeId,
                                    FollowUpDueDateTime=Convert.ToDateTime(customerFollowUpViewModel.FollowUpDueDateTime),
                                    FollowUpReminderDateTime= Convert.ToDateTime(customerFollowUpViewModel.FollowUpReminderDateTime),
                                    FollowUpRemarks= customerFollowUpViewModel.FollowUpRemarks,
                                    Priority= Convert.ToByte(customerFollowUpViewModel.Priority),
                                    FollowUpStatusId= customerFollowUpViewModel.FollowUpStatusId,
                                    FollowUpStatusReason= customerFollowUpViewModel.FollowUpStatusReason,
                                    FollowUpByUserId= customerFollowUpViewModel.FollowUpByUserId,
                                    CreatedBy= customerViewModel.CreatedBy,
                                    CreatedDate=Convert.ToDateTime(customerFollowUpViewModel.CreatedDate)

                                };
                            responseOutFollowUp = dbInterface.AddEditCustomerFollowUp(customerFollowUp);
                        }
                        
                        }

                        SL sl = new SL
                        {
                            SLId = 0,
                            SLCode = customerViewModel.CustomerCode,
                            SLHead = customerViewModel.CustomerName,
                            RefCode = customerViewModel.CustomerCode,
                            SLTypeId = 2,
                            CostCenterId = 0,
                            SubCostCenterId = 0,
                            CompanyId = customerViewModel.CompanyId,
                            CreatedBy = customerViewModel.CreatedBy,
                            Status = customerViewModel.Customer_Status
                        }; 

                   
                    responseOutSL = dbInterface.AddEditCustomerSL(sl, customerViewModel.CustomerId == 0 ? "Add" : "Edit");
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

        public string GenerateCustomerCode(int StateId, string StateCode, int CustomerTypeId, string CustomerTypeCode)
        {
            string CustomerCode = string.Empty;
            try
            {
                List<Customer> objLstCustomer = dbInterface.GetCustomerByFilter(StateId,CustomerTypeId);
                if (objLstCustomer != null && objLstCustomer.Any())
                {
                    int MaxSequence = objLstCustomer.Max(x => x.CustomerId);
                    CustomerCode = "GEMPL" + CustomerTypeCode + StateCode + (MaxSequence+1).ToString().PadLeft(4, '0');
                }
                else
                {
                    CustomerCode = "GEMPL" + CustomerTypeCode + StateCode + "0001";
                }

            }
            catch (Exception ex)
            {
                
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
              
            }
            return CustomerCode;
            
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


        public ResponseOut RemoveVendorProduct(long mappingId)
        {

            ResponseOut responseOut = new ResponseOut();
            try
            {
                responseOut = dbInterface.RemoveVendorProduct(mappingId);
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
         


        public List<CustomerViewModel> GetCustomerList(string customerName, string customerCode, string mobileNo, int customerTypeid, string city, string state , int companyId, string customerStatus,int companyBranchId,int UserId=0)
        {
            List<CustomerViewModel> customers = new List<CustomerViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCustomers = sqlDbInterface.GetCustomerList(customerName, customerCode, mobileNo, customerTypeid,city,state, companyId, customerStatus, companyBranchId,UserId);
                if (dtCustomers != null && dtCustomers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCustomers.Rows)
                    {
                        customers.Add(new CustomerViewModel
                        {
                            CustomerId = Convert.ToInt32(dr["CustomerId"]),
                            CustomerCode = Convert.ToString(dr["CustomerCode"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            ContactPersonName = Convert.ToString(dr["ContactPersonName"]),
                            Email = Convert.ToString(dr["Email"]),
                            MobileNo = Convert.ToString(dr["MobileNo"]),
                            ContactNo = Convert.ToString(dr["ContactNo"]),
                            PrimaryAddress = Convert.ToString(dr["PrimaryAddress"]),
                            City = Convert.ToString(dr["City"]),
                            ExciseNo = Convert.ToString(dr["ExciseNo"]),
                            StateName = Convert.ToString(dr["StateName"]),
                            CountryName = Convert.ToString(dr["CountryName"]),
                            PinCode = Convert.ToString(dr["PinCode"]),
                            TINNo = Convert.ToString(dr["TINNo"]),
                            PANNo = Convert.ToString(dr["PANNo"]),
                            GSTNo = Convert.ToString(dr["GSTNo"]),
                            CustomerTypeDesc = Convert.ToString(dr["CustomerTypeDesc"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            Customer_Status = Convert.ToBoolean(dr["Status"]),
                            GST_Exempt = Convert.ToBoolean(dr["GST_Exempt"]),
                            IsComposition = Convert.ToBoolean(dr["IsComposition"]),
                            IsUIN = Convert.ToBoolean(dr["IsUIN"]),
                            UINNo = Convert.ToString(dr["UINNo"]),
                            AnnualTurnover = Convert.ToDecimal(dr["AnnualTurnover"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                            CompanyBranchName = Convert.ToString(dr["CompanyBranchName"])
                        });
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
        public CustomerViewModel GetCustomerDetail(int customerId = 0)
        {
            CustomerViewModel customer = new CustomerViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCompanies = sqlDbInterface.GetCustomerDetail(customerId);
                if (dtCompanies != null && dtCompanies.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCompanies.Rows)
                    {
                        customer = new CustomerViewModel
                        {
                            CustomerId = Convert.ToInt32(dr["CustomerId"]),
                            CustomerCode = Convert.ToString(dr["CustomerCode"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            ContactPersonName = Convert.ToString(dr["ContactPersonName"]),
                            Designation = Convert.ToString(dr["Designation"]),
                            Email = Convert.ToString(dr["Email"]),
                            MobileNo = Convert.ToString(dr["MobileNo"]),
                            ContactNo = Convert.ToString(dr["ContactNo"]),
                            Fax = Convert.ToString(dr["Fax"]),
                            PrimaryAddress = Convert.ToString(dr["PrimaryAddress"]),
                            City = Convert.ToString(dr["City"]),
                            StateId = Convert.ToInt32(dr["StateId"]),
                            CountryId = Convert.ToInt32(dr["CountryId"]),
                            PinCode = Convert.ToString(dr["PinCode"]),
                            CSTNo = Convert.ToString(dr["CSTNo"]),
                            TINNo = Convert.ToString(dr["TINNo"]),
                            PANNo = Convert.ToString(dr["PANNo"]),
                            GSTNo = Convert.ToString(dr["GSTNo"]),
                            ExciseNo = Convert.ToString(dr["ExciseNo"]),
                            EmployeeId = Convert.ToInt32(dr["EmployeeId"].ToString() == "" ? "0" : dr["EmployeeId"]),
                            EmployeeName = Convert.ToString(dr["EmployeeName"]),
                            CreditLimit = Convert.ToDecimal(dr["CreditLimit"].ToString() == "" ? "0" : dr["CreditLimit"]),
                            CreditDays= Convert.ToInt16(dr["CreditDays"].ToString() == "" ? "0" : dr["CreditDays"]),
                            CustomerTypeId = Convert.ToInt32(dr["CustomerTypeId"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            Customer_Status = Convert.ToBoolean(dr["Status"]),
                            AnnualTurnover = Convert.ToDecimal(dr["AnnualTurnover"]),
                            GST_Exempt = Convert.ToBoolean(dr["GST_Exempt"]),
                            IsComposition = Convert.ToBoolean(dr["IsComposition"]),
                            IsUIN = Convert.ToBoolean(dr["IsUIN"]),
                            UINNo = Convert.ToString(dr["UINNo"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                            CompanyBranchName = Convert.ToString(dr["CompanyBranchName"]),
                            SaleEmpId = Convert.ToInt32(dr["SaleEmpId"]),
                            SaleEmployeeName = Convert.ToString(dr["SaleEmpName"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return customer;
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
                            SequenceNo = Convert.ToInt32(dr["SNo"]),
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
                            AnnualTurnover = Convert.ToDecimal(dr["AnnualTurnover"]),
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

        public List<CustomerProductViewModel> GetCustomerProductList(int customerId)
        {
            List<CustomerProductViewModel> customerProducts = new List<CustomerProductViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCustomers = sqlDbInterface.GetCustomerProductList(customerId);
                if (dtCustomers != null && dtCustomers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCustomers.Rows)
                    {
                        customerProducts.Add(new CustomerProductViewModel
                        {
                            MappingId = Convert.ToInt32(dr["MappingId"]),
                            SequenceNo = Convert.ToInt32(dr["SNo"]),
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
            return customerProducts;
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
                        customers.Add(new CustomerViewModel { CustomerId = customer.CustomerId, CustomerName = customer.CustomerName, CustomerCode = customer.CustomerCode, PrimaryAddress = customer.PrimaryAddress,GSTNo=customer.GSTNo,PANNo= customer.PANNo });
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
        public List<CustomerBranchViewModel> GetCustomerBranches( int customerId)
        {
            List<CustomerBranchViewModel> customers = new List<CustomerBranchViewModel>();
            try
            {
                List<CustomerBranch> customerList = dbInterface.GetCustomerBranchList(customerId);

                if (customerList != null && customerList.Count > 0)
                {
                    foreach (CustomerBranch customer in customerList)
                    {
                        customers.Add(new CustomerBranchViewModel { CustomerBranchId = customer.CustomerBranchId, BranchName = customer.BranchName });
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
        public CustomerBranchViewModel GetCustomerBranchDetail(long customerBranchId)
        {
            CustomerBranchViewModel customers = new CustomerBranchViewModel();
            try
            {
                CustomerBranch customerList = dbInterface.GetCustomerBranchDetail(customerBranchId);

                if (customerList != null)
                {
                    customers = new CustomerBranchViewModel {
                        PrimaryAddress = customerList.PrimaryAddress,
                        City = customerList.City,
                        StateId =Convert.ToInt32(customerList.StateId),
                        CountryId =Convert.ToInt32(customerList.CountryId),
                        PinCode = customerList.PinCode,
                        TINNo = customerList.TINNo,
                        ContactPersonName = customerList.ContactPersonName,
                        Email = customerList.Email,
                        MobileNo = customerList.MobileNo,
                        ContactNo = customerList.ContactNo,
                        Fax = customerList.Fax,
                        GSTNo=customerList.GSTNo

                    };
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return customers;
        }

        public List<CustomerViewModel> GetCustomerAutoCompleteForPaymentModeList(string searchTerm, int companyId)
        {
            List<CustomerViewModel> customers = new List<CustomerViewModel>();
            try
            {
                List<Customer> customerList = dbInterface.GetCustomerAutoCompleteForPaymentModeList(searchTerm, companyId);

                if (customerList != null && customerList.Count > 0)
                {
                    foreach (Customer customer in customerList)
                    {
                        customers.Add(new CustomerViewModel { CustomerId = customer.CustomerId, CustomerName = customer.CustomerName });
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

        public ResponseOut ImportCustomer(CustomerViewModel customerViewModel)
        {
           
            ResponseOut responseOut = new ResponseOut();
            ResponseOut responseOutSL = new ResponseOut();
            using (TransactionScope transactionscope = new TransactionScope())
            {
                try
                {

                    Customer customer = new Customer
                    {
                        CustomerId = customerViewModel.CustomerId,
                        LeadId=customerViewModel.LeadID,
                        CustomerCode = customerViewModel.CustomerCode,
                        CustomerName = customerViewModel.CustomerName,
                        ContactPersonName = customerViewModel.ContactPersonName,
                        Designation = customerViewModel.Designation,
                        Email = customerViewModel.Email,
                        MobileNo = customerViewModel.MobileNo,
                        ContactNo = customerViewModel.ContactNo,
                        Fax = customerViewModel.Fax,
                        PrimaryAddress = customerViewModel.PrimaryAddress,
                        City = customerViewModel.City,
                        StateId = customerViewModel.StateId,
                        CountryId = customerViewModel.CountryId,
                        PinCode = customerViewModel.PinCode,
                        CSTNo = customerViewModel.CSTNo,
                        TINNo = customerViewModel.TINNo,
                        PANNo = customerViewModel.PANNo,
                        GSTNo = customerViewModel.GSTNo,
                        ExciseNo = customerViewModel.ExciseNo,
                        EmployeeId = customerViewModel.EmployeeId,
                        CustomerTypeId = customerViewModel.CustomerTypeId,
                        CompanyId = customerViewModel.CompanyId,
                        CreatedBy = customerViewModel.CreatedBy,
                        CreditLimit = customerViewModel.CreditLimit,
                        CreditDays = customerViewModel.CreditDays,
                        Status = customerViewModel.Customer_Status,
                        GST_Exempt = customerViewModel.GST_Exempt
                };


                    int customerId = 0;
                    responseOut = dbInterface.AddEditCustomer(customer, out customerId);


                    SL sl = new SL
                    {
                        SLId = 0,
                        SLCode = customerViewModel.CustomerCode,
                        SLHead = customerViewModel.CustomerName,
                        RefCode = customerViewModel.CustomerCode,
                        SLTypeId = 2,
                        CostCenterId = 0,
                        SubCostCenterId = 0,
                        CompanyId = customerViewModel.CompanyId,
                        CreatedBy = customerViewModel.CreatedBy,
                        Status = true
                    };


                    responseOutSL = dbInterface.AddEditCustomerSL(sl, customerViewModel.CustomerId == 0 ? "Add" : "Edit");
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


        public ResponseOut ImportCustomerBranch(CustomerBranchViewModel customerbranchViewModel)
        {

            ResponseOut responseOut = new ResponseOut();
            using (TransactionScope transactionscope = new TransactionScope())
            {
                try
                {

                    CustomerBranch customerbranch = new CustomerBranch
                    {
                        CustomerBranchId = customerbranchViewModel.CustomerBranchId, 
                        BranchName = customerbranchViewModel.BranchName,
                        ContactPersonName = customerbranchViewModel.ContactPersonName,
                        Designation = customerbranchViewModel.Designation,
                        Email = customerbranchViewModel.Email,
                        MobileNo = customerbranchViewModel.MobileNo,
                        ContactNo = customerbranchViewModel.ContactNo,
                        Fax = customerbranchViewModel.Fax,
                        PrimaryAddress = customerbranchViewModel.PrimaryAddress,
                        City = customerbranchViewModel.City,
                        StateId = customerbranchViewModel.StateId,
                        CustomerId = customerbranchViewModel.CustomerId,
                        CountryId = customerbranchViewModel.CountryId, 
                        PinCode = customerbranchViewModel.PinCode,
                        CSTNo = customerbranchViewModel.CSTNo,
                        TINNo = customerbranchViewModel.TINNo,
                        PANNo = customerbranchViewModel.PANNo,
                        GSTNo = customerbranchViewModel.GSTNo,
                        Status = customerbranchViewModel.CustomerBranch_Status,


                    };
                   
                    responseOut = dbInterface.AddEditCustomerBranch(customerbranch);


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

        public ResponseOut CustomerFollowUpValidation(CustomerFollowUpViewModel customerFollowUps)
        {
            ResponseOut responseOut = new ResponseOut();
            if(customerFollowUps != null)
            {
                DateTime dueDate;
                DateTime RemDate;
                DateTime.TryParse(customerFollowUps.FollowUpDueDateTime, out dueDate);
                DateTime.TryParse(customerFollowUps.FollowUpReminderDateTime, out RemDate);
                if (RemDate > dueDate)
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.CustomerFollowUpsDateCheck;
                }
                else
                {
                    responseOut.status = ActionStatus.Success;   
                }
            }
            return responseOut;
        }

        public List<CustomerFollowUpViewModel> GetCustomerFollowUpList(int CustomerId)
        {
            List<CustomerFollowUpViewModel> followUps = new List<CustomerFollowUpViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();

            try
            {
                DataTable dtFollowUps = sqlDbInterface.GetCustomerFollowUpList(CustomerId);
                if (dtFollowUps != null && dtFollowUps.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtFollowUps.Rows)
                    {
                        followUps.Add(new CustomerFollowUpViewModel
                        {
                            CustomerFollowUpId = Convert.ToInt32(dr["CustomerFollowUpId"]),
                            FollowUpSequenceNo = Convert.ToInt32(dr["SNo"]),
                            FollowUpActivityTypeId = Convert.ToInt32(dr["FollowUpActivityTypeId"]),
                            FollowUpActivityTypeName = dr["FollowUpActivityTypeName"].ToString(),
                            FollowUpDueDateTime = Convert.ToString(dr["FollowUpDueDateTime"]),
                            FollowUpReminderDateTime = Convert.ToString(dr["FollowUpReminderDateTime"]),
                            FollowUpRemarks = dr["FollowUpRemarks"].ToString(),
                            Priority = Convert.ToInt16(dr["Priority"]),
                            PriorityName = ((Convert.ToInt16(dr["Priority"].ToString()) == 1) ? "Urgent" : (Convert.ToInt16(dr["Priority"].ToString()) == 2) ? "High" : (Convert.ToInt16(dr["Priority"].ToString()) == 3) ? "Medium" : "Low"),
                            FollowUpStatusId = Convert.ToInt32(dr["FollowUpStatusId"]),
                            FollowUpStatusName = dr["FollowUpStatusName"].ToString(),
                            FollowUpStatusReason = dr["FollowUpStatusReason"].ToString(),
                            FollowUpByUserName = dr["FollowUpByUserName"].ToString(),
                            CreatedByUserName = dr["FullName"].ToString(),
                            CreatedBy = Convert.ToInt32(dr["CreatedBy"]) == 0 ? 0 : Convert.ToInt32(dr["CreatedBy"]),
                            CreatedDate = dr["CreatedDate"].ToString() == "" ? "" : dr["CreatedDate"].ToString(),
                            ModifiedBy = (Convert.ToInt16(dr["ModifiedBy"]) == 0) ? 0 : Convert.ToInt16(dr["ModifiedBy"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]) == "" ? "" : Convert.ToString(dr["ModifiedDate"]),

                        });

                    }

                }
            }

            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return followUps;
        }

        public CustomerCountViewModel GetNewCustomerCount(int CompanyId)
        {
            CustomerCountViewModel customers = new CustomerCountViewModel();
            SQLDbInterface sqldbinterface = new SQLDbInterface();
            try
            {
                DataTable dtcustomerList = sqldbinterface.GetNewCustomerCount(CompanyId);

                if (dtcustomerList != null && dtcustomerList.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtcustomerList.Rows)
                    {
                        customers = new CustomerCountViewModel
                        {
                            NewCustomer = Convert.ToInt32(dr["NewCustomers"])
                        };
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

        public CustomerCountViewModel GetTotalCustomerCount(int CompanyId)
        {
            CustomerCountViewModel customers = new CustomerCountViewModel();
            SQLDbInterface sqldbinterface = new SQLDbInterface();
            try
            {
                DataTable dtcustomerList = sqldbinterface.GetTotalCustomerCount(CompanyId);

                if (dtcustomerList != null && dtcustomerList.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtcustomerList.Rows)
                    {
                        customers = new CustomerCountViewModel
                        {
                            TotalCustomer = Convert.ToInt32(dr["TotalCustomers"])
                        };
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

        public List<CustomerViewModel> GetTodayNewCustomerList(int companyID)
        {
            List<CustomerViewModel> customers = new List<CustomerViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCustomers = sqlDbInterface.GetTodayNewCustomer(companyID);
                if (dtCustomers != null && dtCustomers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCustomers.Rows)
                    {
                        customers.Add(new CustomerViewModel
                        {
                            CustomerId = Convert.ToInt32(dr["CustomerId"]),
                            CustomerCode = Convert.ToString(dr["CustomerCode"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            ContactPersonName = Convert.ToString(dr["ContactPersonName"]),
                            Email = Convert.ToString(dr["Email"]),
                            MobileNo = Convert.ToString(dr["MobileNo"]),
                            PrimaryAddress = Convert.ToString(dr["PrimaryAddress"]),
                            City = Convert.ToString(dr["City"]),
                            ExciseNo = Convert.ToString(dr["ExciseNo"]),
                            StateName = Convert.ToString(dr["StateName"]),
                            CustomerTypeDesc = Convert.ToString(dr["CustomerTypeDesc"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            Customer_Status = Convert.ToBoolean(dr["Status"])
                        });
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

        public DataTable CustomerExport(string customerName, string customerCode, string mobileNo, int customerTypeid, int companyId,string city,string state, string customerStatus)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtCustomerExport = new DataTable();
            try
            {
                dtCustomerExport = sqlDbInterface.CustomerExport(customerName, customerCode, mobileNo, customerTypeid, companyId,city,state, customerStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtCustomerExport;
        }

        //Customer Pop Up Master------------
        public ResponseOut AddEditCustomerMaster(CustomerViewModel customerViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            ResponseOut responseOutSL = new ResponseOut();
            using (TransactionScope transactionscope = new TransactionScope())
            {
                try
                {
                    Customer customer = new Customer
                    {
                        CustomerCode = string.IsNullOrEmpty( customerViewModel.CustomerCode) ?GenerateCustomerCode(customerViewModel.StateId,customerViewModel.StateCode,customerViewModel.CustomerTypeId,customerViewModel.CustomerTypeCode): customerViewModel.CustomerCode,
                        CustomerName = customerViewModel.CustomerName,
                        ContactPersonName = customerViewModel.ContactPersonName,
                        MobileNo = customerViewModel.MobileNo,
                        PrimaryAddress = customerViewModel.PrimaryAddress,
                        StateId = customerViewModel.StateId,
                        CountryId = customerViewModel.CountryId,
                        City = customerViewModel.City,
                        GSTNo = customerViewModel.GSTNo,
                        CustomerTypeId = customerViewModel.CustomerTypeId,
                        CompanyId = customerViewModel.CompanyId,
                        CreatedBy = customerViewModel.CreatedBy,
                        Status = customerViewModel.Customer_Status
                    };
                    responseOut = dbInterface.AddEditCustomerMaster(customer);
                    if (responseOut.status == ActionStatus.Success)
                    {
                        SL sl = new SL
                        {
                            SLId = 0,
                            SLCode = customerViewModel.CustomerCode,
                            SLHead = customerViewModel.CustomerName,
                            RefCode = customerViewModel.CustomerCode,
                            SLTypeId = 2,
                            CostCenterId = 0,
                            SubCostCenterId = 0,
                            CompanyId = customerViewModel.CompanyId,
                            CreatedBy = customerViewModel.CreatedBy,
                            Status = true
                        };
                        responseOutSL = dbInterface.AddEditCustomerMasterSL(sl, "Add");
                        transactionscope.Complete();
                    }
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

        public List<CustomerViewModel> GetCustomerDetailsById(int customerId, int companyId)
        {
            List<CustomerViewModel> customers = new List<CustomerViewModel>();
            try
            {
                List<Customer> customerList = dbInterface.GetCustomerDetailsById(customerId, companyId);

                if (customerList != null && customerList.Count > 0)
                {
                    foreach (Customer customer in customerList)
                    {
                        customers.Add(new CustomerViewModel { CustomerId = customer.CustomerId, CustomerName = customer.CustomerName, CustomerCode = customer.CustomerCode, PrimaryAddress = customer.PrimaryAddress ,MobileNo=customer.MobileNo});
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

        public List<CustomerViewModel> GetCustomerMobileAutoCompleteList(string searchTerm, int companyId)
        {
            List<CustomerViewModel> customers = new List<CustomerViewModel>();
            try
            {
                List<Customer> customerList = dbInterface.GetCustomerMobileAutoCompleteList(searchTerm, companyId);

                if (customerList != null && customerList.Count > 0)
                {
                    foreach (Customer customer in customerList)
                    {
                        customers.Add(new CustomerViewModel {
                            MobileNo = customer.MobileNo,
                            CustomerName = customer.CustomerName,
                            Email = customer.Email,
                            PrimaryAddress=customer.PrimaryAddress

                        });
                       
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

        public List<CustomerViewModel> GetTodayCustomerList(int companyId)
        {
            List<CustomerViewModel> customers = new List<CustomerViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCustomers = sqlDbInterface.GetTodayCustomerList(companyId);
                if (dtCustomers != null && dtCustomers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCustomers.Rows)
                    {
                        customers.Add(new CustomerViewModel
                        {
                            CustomerId = Convert.ToInt32(dr["CustomerId"]),
                            CustomerCode = Convert.ToString(dr["CustomerCode"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            ContactPersonName = Convert.ToString(dr["ContactPersonName"]),
                            Email = Convert.ToString(dr["Email"]),
                            MobileNo = Convert.ToString(dr["MobileNo"]),
                            ContactNo = Convert.ToString(dr["ContactNo"]),
                            PrimaryAddress = Convert.ToString(dr["PrimaryAddress"]),
                            City = Convert.ToString(dr["City"]),
                            ExciseNo = Convert.ToString(dr["ExciseNo"]),
                            StateName = Convert.ToString(dr["StateName"]),
                            CountryName = Convert.ToString(dr["CountryName"]),
                            PinCode = Convert.ToString(dr["PinCode"]),
                            TINNo = Convert.ToString(dr["TINNo"]),
                            PANNo = Convert.ToString(dr["PANNo"]),
                            GSTNo = Convert.ToString(dr["GSTNo"]),
                            CustomerTypeDesc = Convert.ToString(dr["CustomerTypeDesc"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            Customer_Status = Convert.ToBoolean(dr["Status"]),
                            GST_Exempt = Convert.ToBoolean(dr["GST_Exempt"]),
                            IsComposition = Convert.ToBoolean(dr["IsComposition"]),
                            IsUIN = Convert.ToBoolean(dr["IsUIN"]),
                            UINNo = Convert.ToString(dr["UINNo"]),
                            AnnualTurnover = Convert.ToDecimal(dr["AnnualTurnover"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                            CompanyBranchName = Convert.ToString(dr["CompanyBranchName"])
                        });
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


        public List<SaleDashboardItemsViewModel> GetSaleDashboardItems(int roleId, int companyId, int companyBranchId, int finYearId)
        {
           List<SaleDashboardItemsViewModel> saleDashboardItems = new List<SaleDashboardItemsViewModel>();
            SQLDbInterface sqldbinterface = new SQLDbInterface();
            try
            {
                DataTable dtSaleDashboardItemList = sqldbinterface.SaleDashboardItems(roleId, companyId, companyBranchId,finYearId);

                if (dtSaleDashboardItemList != null && dtSaleDashboardItemList.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtSaleDashboardItemList.Rows)
                    {
                        saleDashboardItems.Add(new SaleDashboardItemsViewModel
                        {
                          SrNo=Convert.ToInt32(dr["SrNo"]),
                          ContainerItemKey=Convert.ToString(dr["ContainerItemKey"]),
                          ContainerItemValue = Convert.ToString(dr["ContainerItemValue"]),
                          BoxNumber=Convert.ToString(dr["BoxNumber"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return saleDashboardItems;
        }

        public List<InventoryDashboardItemsViewModel> GetInventoryDashboardItems(int roleId, int companyId, int companyBranchId, int finYearId)
        {
            List<InventoryDashboardItemsViewModel> inventoryDashboardItems = new List<InventoryDashboardItemsViewModel>();
            SQLDbInterface sqldbinterface = new SQLDbInterface();
            try
            {
                DataTable dtInventoryDashboardItemList = sqldbinterface.InventoryDashboardItems(roleId, companyId, companyBranchId, finYearId);

                if (dtInventoryDashboardItemList != null && dtInventoryDashboardItemList.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtInventoryDashboardItemList.Rows)
                    {
                        inventoryDashboardItems.Add(new InventoryDashboardItemsViewModel
                        {
                            SrNo = Convert.ToInt32(dr["SrNo"]),
                            ContainerItemKey = Convert.ToString(dr["ContainerItemKey"]),
                            ContainerItemValue = Convert.ToString(dr["ContainerItemValue"]),
                            BoxNumber = Convert.ToString(dr["BoxNumber"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return inventoryDashboardItems;
        }

        public List<CustomerFormViewModel> GetCustomerByTypeId(int CustomerTypeId)
        {
            List<CustomerFormViewModel> objCustomers = new List<CustomerFormViewModel>();
            List<Customer> customers = new List<Customer>();
            DBInterface sqlDbInterface = new DBInterface();
            try
            {
                customers = sqlDbInterface.GetCustomerByType(CustomerTypeId);
                if (customers.Any())
                {
                    foreach (Customer item in customers)
                    {
                        objCustomers.Add(new CustomerFormViewModel {
                            CustomerId = item.CustomerId,
                            CustomerName = item.CustomerName,
                            CustomerCode = item.CustomerCode,
                        });
                    }
                }
               
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return objCustomers;
        }

        public List<SelectListModel> GetCustomerAutoCompletewithSaleOrderList(string searchTerm, int companyId)
        {
            List<SelectListModel> lstSelectListModel = new List<SelectListModel>();
            try
            {
                lstSelectListModel = dbInterface.GetCustomerAutoCompletewithSaleOrderList(searchTerm, companyId);

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return lstSelectListModel;
        }
    }
}
