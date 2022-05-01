using Portal.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Portal.DAL.Infrastructure;
using System.Data.Entity.SqlServer;

namespace Portal.DAL
{
    /// <summary>
    /// Class to Provide Services of DB
    /// </summary>
    public partial class DBInterface : IDisposable
    {
        private readonly ERPEntities entities = new ERPEntities();

        public DBInterface()
        {

        }
        #region Dispose Methods
        public void Dispose()
        {
            try
            {
                entities.Dispose();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
        }
        #endregion
        public User AuthenticateUser(string userName, string password)
        {
            User user = new User();
            try
            {
                if (entities.Users.Any(x => x.UserName == userName && x.Password == password))
                {
                    user = entities.Users.Where(u => u.UserName == userName && u.Password == password).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return user;
        }
        public List<proc_GetRoleWiseParentUI_Result> GetRoleWiseParentUI(int roleId)
        {
            List<proc_GetRoleWiseParentUI_Result> roleWiseParentUI = new List<proc_GetRoleWiseParentUI_Result>();
            try
            {
                roleWiseParentUI = entities.proc_GetRoleWiseParentUI(roleId).ToList();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return roleWiseParentUI;
        }
        public List<proc_GetRoleWiseChildUI_Result> GetRoleWiseChildUI(int roleId, int parentId)
        {
            List<proc_GetRoleWiseChildUI_Result> roleWiseChildUI = new List<proc_GetRoleWiseChildUI_Result>();
            try
            {
                roleWiseChildUI = entities.proc_GetRoleWiseChildUI(parentId, roleId).ToList();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return roleWiseChildUI;
        }
        public bool AuthorizeUser(int roleId, int interfaceId, int accessMode)
        {
            bool isAuthorized = false;
            try
            {
                switch (accessMode)
                {
                    case ((int)AccessMode.AddAccess):
                        {
                            if (entities.RoleUIActionMappings.Any(x => x.RoleId == roleId && x.InterfaceId == interfaceId && x.AddAccess == true && x.Status == true))
                            { isAuthorized = true; }
                            else
                            { isAuthorized = false; }
                            break;
                        }
                    case ((int)AccessMode.EditAccess):
                        {
                            if (entities.RoleUIActionMappings.Any(x => x.RoleId == roleId && x.InterfaceId == interfaceId && x.EditAccess == true && x.Status == true))
                            { isAuthorized = true; }
                            else
                            { isAuthorized = false; }
                            break;
                        }
                    case ((int)AccessMode.ViewAccess):
                        {
                            if (entities.RoleUIActionMappings.Any(x => x.RoleId == roleId && x.InterfaceId == interfaceId && x.ViewAccess == true && x.Status == true))
                            { isAuthorized = true; }
                            else
                            { isAuthorized = false; }
                            break;
                        }
                    case ((int)AccessMode.CancelAccess):
                        {
                            if (roleId == 2)
                            {
                                isAuthorized = true;
                            }
                            else if (entities.RoleUIActionMappings.Any(x => x.RoleId == roleId && x.InterfaceId == interfaceId && x.CancelAccess == true && x.Status == true))
                            {
                                isAuthorized = true;
                            }
                            else
                            {
                                isAuthorized = false;
                            }
                            break;
                        }
                    case ((int)AccessMode.ReviseAccess):
                        {
                            if (roleId == 2)
                            {
                                isAuthorized = true;
                            }

                            else if (entities.RoleUIActionMappings.Any(x => x.RoleId == roleId && x.InterfaceId == interfaceId && x.ReviseAccess == true && x.Status == true))
                            {
                                isAuthorized = true;
                            }
                            else
                            {
                                isAuthorized = false;
                            }
                            break;
                        }
                    default:
                        {
                            isAuthorized = false;
                            break;
                        }
                }


            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                isAuthorized = false;
                throw ex;
            }
            return isAuthorized;

        }



        public List<Country> GetCountryList()
        {
            List<Country> countryList = new List<Country>();
            try
            {
                var countries = entities.Countries.Where(x => x.Status == true).OrderBy(x => SqlFunctions.IsNumeric(x.CountryName)).ThenBy(x => x.CountryName).Select(s => new
                {
                    CountryId = s.CountryId,
                    CountryName = s.CountryName,
                    CountryCode = s.CountryCode
                }).ToList();
                if (countries != null && countries.Count > 0)
                {
                    foreach (var item in countries)
                    {
                        countryList.Add(new Country { CountryId = item.CountryId, CountryCode = item.CountryCode, CountryName = item.CountryName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return countryList;
        }



        public List<State> GetStateList(int countryId)
        {
            List<State> stateList = new List<State>();
            try
            {
                var states = entities.States.Where(x => x.CountryId == countryId).OrderBy(x => SqlFunctions.IsNumeric(x.StateName)).ThenBy(x => x.StateName).Select(s => new
                {
                    StateId = s.Stateid,
                    StateName = s.StateName,
                    StateCode = s.StateCode
                }).ToList();
                if (states != null && states.Count > 0)
                {
                    foreach (var item in states)
                    {
                        stateList.Add(new State { Stateid = item.StateId, StateCode = item.StateCode, StateName = item.StateName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return stateList;
        }
        public List<PaymentMode> GetPaymentModeList(int paymentModeId)
        {
            List<PaymentMode> paymentModeList = new List<PaymentMode>();
            try
            {
                var paymentModes = entities.PaymentModes.Where(x => x.Status == true).Select(s => new
                {
                    PaymentModeId = s.PaymentModeId,
                    PaymentModeName = s.PaymentModeName
                }).ToList();
                if (paymentModes != null && paymentModes.Count > 0)
                {
                    foreach (var item in paymentModes)
                    {
                        paymentModeList.Add(new PaymentMode { PaymentModeId = item.PaymentModeId, PaymentModeName = item.PaymentModeName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return paymentModeList;
        }

        public List<State> GetStateList()
        {
            List<State> stateList = new List<State>();
            try
            {
                var states = entities.States.Select(s => new
                {
                    StateId = s.Stateid,
                    StateName = s.StateName,
                    StateCode = s.StateCode
                }).ToList();
                if (states != null && states.Count > 0)
                {
                    foreach (var item in states)
                    {
                        stateList.Add(new State { Stateid = item.StateId, StateCode = item.StateCode, StateName = item.StateName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return stateList;
        }

        #region Company
        public ResponseOut AddEditCompany(Company company)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.Companies.Any(x => x.CompanyCode == company.CompanyCode && x.CompanyId != company.CompanyId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateCompanyCode;
                }
                else if (entities.Companies.Any(x => x.CompanyName == company.CompanyName && x.Email == company.Email && x.CompanyId != company.CompanyId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateCompanyName;
                }
                else
                {
                    if (company.CompanyId == 0)
                    {
                        company.CreatedDate = DateTime.Now;
                        entities.Companies.Add(company);
                        responseOut.message = ActionMessage.CompanyCreatedSuccess;
                    }
                    else
                    {
                        entities.Companies.Where(a => a.CompanyId == company.CompanyId).ToList().ForEach(a =>
                        {
                            a.CompanyName = company.CompanyName;
                            a.ContactPersonName = company.ContactPersonName;
                            a.Phone = company.Phone;
                            a.Email = company.Email;
                            a.Fax = company.Fax;
                            a.Logo = company.Logo;
                            a.Website = company.Website;
                            a.Address = company.Address;
                            a.City = company.City;
                            a.State = company.State;
                            a.CountryId = company.CountryId;
                            a.ZipCode = company.ZipCode;
                            a.CompanyDesc = company.CompanyDesc;
                            a.PANNo = company.PANNo;
                            a.TanNo = company.TanNo;
                            a.TINNo = company.TINNo;
                            a.ServiceTaxNo = company.ServiceTaxNo;
                            a.CompanyCode = company.CompanyCode;
                            a.AnnualTurnover = company.AnnualTurnover;
                            a.ModifiedDate = DateTime.Now;
                        });
                        responseOut.message = ActionMessage.CompanyUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;

                }
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
        public List<Company> GetCompanyList()
        {
            List<Company> companyList = new List<Company>();
            try
            {
                var companies = entities.Companies.Select(s => new
                {
                    CompanyId = s.CompanyId,
                    CompanyName = s.CompanyName
                }).ToList();
                if (companies != null && companies.Count > 0)
                {
                    foreach (var item in companies)
                    {
                        companyList.Add(new Company { CompanyId = item.CompanyId, CompanyName = item.CompanyName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return companyList;
        }
        public ComapnyBranch GetCompanyBranchDetails(int companyBranchID)
        {
            ComapnyBranch ComapnyBranchList = new ComapnyBranch();
            try
            {
                var companies = entities.ComapnyBranches.Where(x => x.CompanyBranchId == companyBranchID && x.Status == true).FirstOrDefault();
                if (companies != null)
                {
                    ComapnyBranchList = new ComapnyBranch
                    {
                        StateId = companies.StateId,
                        FreightCGST_Perc = companies.FreightCGST_Perc,
                        FreightSGST_Perc = companies.FreightSGST_Perc,
                        FreightIGST_Perc = companies.FreightIGST_Perc,
                        LoadingCGST_Perc = companies.LoadingCGST_Perc,
                        LoadingSGST_Perc = companies.LoadingSGST_Perc,
                        LoadingIGST_Perc = companies.LoadingIGST_Perc,
                        InsuranceCGST_Perc = companies.InsuranceCGST_Perc,
                        InsuranceSGST_Perc = companies.InsuranceSGST_Perc,
                        InsuranceIGST_Perc = companies.InsuranceIGST_Perc
                    };

                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return ComapnyBranchList;
        }
        #endregion
        #region CompanyBranch
        public ResponseOut AddEditCompanyBranch(ComapnyBranch comapnyBranch)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.ComapnyBranches.Any(x => x.GSTNo == comapnyBranch.GSTNo && x.BranchName == comapnyBranch.BranchName && x.PrimaryAddress == comapnyBranch.PrimaryAddress && x.CompanyBranchId != comapnyBranch.CompanyBranchId && x.StateId == comapnyBranch.StateId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.CompanyBranchDuplicateBranch;
                }

                if (entities.ComapnyBranches.Any(x => x.GSTNo == comapnyBranch.GSTNo && x.BranchName == comapnyBranch.BranchName && x.CompanyBranchId != comapnyBranch.CompanyBranchId && x.StateId != comapnyBranch.StateId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.CompanyBranchDuplicateGSTNo;
                }

                else
                {
                    if (comapnyBranch.CompanyBranchId == 0)
                    {
                        if (entities.ComapnyBranches.Any(x => x.GSTNo == comapnyBranch.GSTNo && x.BranchName == comapnyBranch.BranchName && x.PrimaryAddress == comapnyBranch.PrimaryAddress && x.CompanyBranchId != comapnyBranch.CompanyBranchId && x.StateId == comapnyBranch.StateId && x.CompanyBranchCode == comapnyBranch.CompanyBranchCode))
                        {
                            responseOut.status = ActionStatus.Fail;
                            responseOut.message = ActionMessage.CompanyBranchDuplicateBranch;
                        }

                        else if (entities.ComapnyBranches.Any(x => x.GSTNo == comapnyBranch.GSTNo && x.BranchName == comapnyBranch.BranchName && x.CompanyBranchId != comapnyBranch.CompanyBranchId && x.StateId != comapnyBranch.StateId && x.CompanyBranchCode != comapnyBranch.CompanyBranchCode))
                        {
                            responseOut.status = ActionStatus.Fail;
                            responseOut.message = ActionMessage.CompanyBranchDuplicateGSTNo;
                        }
                        else
                        {
                            entities.ComapnyBranches.Add(comapnyBranch);
                            responseOut.message = ActionMessage.CompanyBranchCreatedSuccess;
                        }
                    }

                    else
                    {
                        entities.ComapnyBranches.Where(a => a.CompanyBranchId == comapnyBranch.CompanyBranchId).ToList().ForEach(a =>
                        {
                            a.BranchName = comapnyBranch.BranchName;
                            a.ContactPersonName = comapnyBranch.ContactPersonName;
                            a.Designation = comapnyBranch.Designation;
                            a.Email = comapnyBranch.Email;
                            a.MobileNo = comapnyBranch.MobileNo;
                            a.ContactNo = comapnyBranch.ContactNo;
                            a.Fax = comapnyBranch.Fax;
                            a.PrimaryAddress = comapnyBranch.PrimaryAddress;
                            a.City = comapnyBranch.City;
                            a.StateId = comapnyBranch.StateId;
                            a.CountryId = comapnyBranch.CountryId;
                            a.PinCode = comapnyBranch.PinCode;
                            a.CSTNo = comapnyBranch.CSTNo;
                            a.TINNo = comapnyBranch.TINNo;
                            a.PANNo = comapnyBranch.PANNo;
                            a.GSTNo = comapnyBranch.GSTNo;
                            a.CompanyId = comapnyBranch.CompanyId;
                            a.AnnualTurnover = comapnyBranch.AnnualTurnover;
                            a.Status = comapnyBranch.Status;
                            a.CompanyBranchCode = comapnyBranch.CompanyBranchCode;
                            a.ManufactorLocationCode = comapnyBranch.ManufactorLocationCode;
                        });
                        responseOut.message = ActionMessage.CompanyBranchUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;

                }
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
        public List<ComapnyBranch> GetCompanyBranchList(int companyId)
        {
            List<ComapnyBranch> ComapnyBranchList = new List<ComapnyBranch>();
            try
            {
                var companies = entities.ComapnyBranches.Where(x => x.CompanyId == companyId && x.Status == true).OrderBy(x => SqlFunctions.IsNumeric(x.BranchName)).ThenBy(x => x.BranchName).Select(s => new
                {
                    CompanyBranchId = s.CompanyBranchId,
                    BranchName = s.BranchName
                }).ToList();
                if (companies != null && companies.Count > 0)
                {
                    foreach (var item in companies)
                    {
                        ComapnyBranchList.Add(new ComapnyBranch { CompanyBranchId = item.CompanyBranchId, BranchName = item.BranchName });

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return ComapnyBranchList;

            //return ComapnyBranchList.Where(x => x.CompanyId == companyId).OrderBy(x => SqlFunctions.IsNumeric(x.BranchName)).ThenBy(x => x.BranchName);
        }
        public ComapnyBranch GetCompanyBranchByStateIdList(int companyBranchID)
        {
            ComapnyBranch ComapnyBranchList = new ComapnyBranch();
            try
            {
                var companies = entities.ComapnyBranches.Where(x => x.CompanyBranchId == companyBranchID && x.Status == true).FirstOrDefault();
                if (companies != null)
                {
                    ComapnyBranchList = new ComapnyBranch
                    {
                        StateId = companies.StateId,
                        FreightCGST_Perc = companies.FreightCGST_Perc,
                        FreightSGST_Perc = companies.FreightSGST_Perc,
                        FreightIGST_Perc = companies.FreightIGST_Perc,
                        LoadingCGST_Perc = companies.LoadingCGST_Perc,
                        LoadingSGST_Perc = companies.LoadingSGST_Perc,
                        LoadingIGST_Perc = companies.LoadingIGST_Perc,
                        InsuranceCGST_Perc = companies.InsuranceCGST_Perc,
                        InsuranceSGST_Perc = companies.InsuranceSGST_Perc,
                        InsuranceIGST_Perc = companies.InsuranceIGST_Perc
                    };

                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return ComapnyBranchList;
        }

        public List<ComapnyBranch> GetCompanyBranchsList(int companyBranchID)
        {
            List<ComapnyBranch> BranchList = new List<ComapnyBranch>();
            try
            {
                var companies = entities.ComapnyBranches.Where(x => x.CompanyBranchId == companyBranchID && x.Status == true).Select(s => new
                {
                    CompanyBranchId = s.CompanyBranchId,
                    BranchName = s.BranchName
                }).ToList();
                if (companies != null && companies.Count > 0)
                {
                    foreach (var item in companies)
                    {
                        BranchList.Add(new ComapnyBranch { CompanyBranchId = item.CompanyBranchId, BranchName = item.BranchName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return BranchList;
        }


        #endregion

        #region CompanyForm
        public ResponseOut AddEditCustomerForm(CustomerForm customerForm)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (customerForm.CustomerFormTrnId == 0)
                {

                    customerForm.CreatedDate = DateTime.Now;
                    entities.CustomerForms.Add(customerForm);
                    responseOut.message = ActionMessage.CustomerFormCreatedSuccess;

                }
                else
                {
                    entities.CustomerForms.Where(a => a.CustomerFormTrnId == customerForm.CustomerFormTrnId).ToList().ForEach(a =>
                    {
                        a.CustomerId = customerForm.CustomerId;
                        a.InvoiceId = customerForm.InvoiceId;
                        a.FormTypeId = customerForm.FormTypeId;
                        a.RefNo = customerForm.RefNo;
                        a.RefDate = customerForm.RefDate;
                        a.Amount = customerForm.Amount;
                        a.Remarks = customerForm.Remarks;
                        a.ModifiedBy = customerForm.CreatedBy;
                        a.ModifiedDate = DateTime.Now;
                        a.FormStatus = customerForm.FormStatus;
                        a.CompanyId = customerForm.CompanyId;
                        a.Status = customerForm.Status;
                    });
                    responseOut.message = ActionMessage.CustomerFormUpdatedSuccess;
                }
                entities.SaveChanges();
                responseOut.status = ActionStatus.Success;

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

        #region User
        public ResponseOut AddEditUser(User user)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.Users.Any(x => x.UserName == user.UserName && x.UserId != user.UserId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateUsername;
                }

                else
                {
                    if (user.UserId == 0)
                    {
                        user.CreatedDate = DateTime.Now;
                        entities.Users.Add(user);
                        responseOut.message = ActionMessage.UserCreatedSuccess;

                    }
                    else
                    {
                        user.ModifiedBy = user.CreatedBy;
                        user.ModifiedDate = DateTime.Now;

                        entities.Users.Where(a => a.UserId == user.UserId).ToList().ForEach(a =>
                        {
                            a.UserName = user.UserName;
                            a.FullName = user.FullName;
                            a.MobileNo = user.MobileNo;
                            a.Email = user.Email;
                            a.Password = user.Password;
                            a.RoleId = user.RoleId;
                            a.CompanyId = user.CompanyId;
                            a.ModifiedBy = user.ModifiedBy;
                            a.ModifiedDate = user.ModifiedDate;
                            a.ExpiryDate = user.ExpiryDate;
                            a.Status = user.Status;
                            a.CompanyBranchId = user.CompanyBranchId;
                            a.FK_CustomerId = user.FK_CustomerId;
                        });
                        responseOut.message = ActionMessage.UserUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.trnId = user.UserId;
                    responseOut.status = ActionStatus.Success;

                }
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
        public ResponseOut UpdateUserPic(User user)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {


                entities.Users.Where(a => a.UserId == user.UserId).ToList().ForEach(a =>
                {
                    a.UserPicName = user.UserPicName;
                    a.UserPicPath = user.UserPicPath;
                });
                entities.SaveChanges();
                responseOut.status = ActionStatus.Success;


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



        public List<User> GetUserAutoCompleteList(string searchTerm, int companyId)
        {
            List<User> userList = new List<User>();
            try
            {
                var users = (from p in entities.Users
                             where (p.FullName.ToLower().Contains(searchTerm.ToLower())) && p.CompanyId == companyId && p.Status == true
                             select new
                             {
                                 UserId = p.UserId,
                                 FullName = p.FullName

                             }).ToList();


                if (users != null && users.Count > 0)
                {
                    foreach (var item in users)
                    {
                        userList.Add(new User { UserId = item.UserId, FullName = item.FullName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return userList;
        }
        public ResponseOut RemoveImageUser(long userId)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                entities.Users.Where(a => a.UserId == userId).ToList().ForEach(a =>
                 {
                     a.UserPicPath = null;
                     a.UserPicName = null;
                 });
                responseOut.message = ActionMessage.UserUpdatedSuccess;
                entities.SaveChanges();
                responseOut.status = ActionStatus.Success;

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

        public int GetEmployeeIdByEmailId(string emailId)
        {
            int employeeId = 0;
            try
            {
                employeeId = entities.Employees.Where(s => s.Email.Trim().ToUpper() == emailId.Trim().ToUpper()).Select(x => x.EmployeeId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return employeeId;
        }


        #endregion

        #region Country
        public ResponseOut AddEditCountry(Country country)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.Countries.Any(x => x.CountryCode == country.CountryCode && x.CountryId != country.CountryId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateCountryCode;
                }
                else if (entities.Countries.Any(x => x.CountryName == country.CountryName && x.CountryId != country.CountryId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateCountryName;
                }
                else
                {
                    if (country.CountryId == 0)
                    {

                        entities.Countries.Add(country);
                        responseOut.message = ActionMessage.CountryCreatedSuccess;
                    }
                    else
                    {
                        entities.Countries.Where(a => a.CountryId == country.CountryId).ToList().ForEach(a =>
                        {
                            a.CountryName = country.CountryName;
                            a.CountryCode = country.CountryCode;
                            a.Status = country.Status;
                        });
                        responseOut.message = ActionMessage.CountryUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;

                }
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

        #region FinYear
        public ResponseOut AddEditFinYear(FinancialYear finyear)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.FinancialYears.Any(x => x.FinYearCode == finyear.FinYearCode && x.FinYearId != finyear.FinYearId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateFinYear;
                }

                else
                {
                    if (!entities.FinancialYears.Any(x => x.FinYearId == finyear.FinYearId))
                    {
                        entities.FinancialYears.Add(finyear);
                        responseOut.message = ActionMessage.FinYearCreatedSuccess;
                    }
                    else
                    {
                        entities.FinancialYears.Where(a => a.FinYearId == finyear.FinYearId).ToList().ForEach(a =>
                        {
                            a.StartDate = finyear.StartDate;
                            a.EndDate = finyear.EndDate;
                            a.FinYearCode = finyear.FinYearCode;
                            a.FinYearDesc = finyear.FinYearDesc;
                            a.Status = finyear.Status;
                        });
                        responseOut.message = ActionMessage.FinYearUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;

                }
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
        public List<FinancialYear> GetFinYearList()
        {
            List<FinancialYear> finYearList = new List<FinancialYear>();
            try
            {
                var finYears = entities.FinancialYears.Where(x => x.Status == true).Select(s => new
                {
                    FinYearId = s.FinYearId,
                    FinYearDesc = s.FinYearDesc
                }).ToList();
                if (finYears != null && finYears.Count > 0)
                {
                    foreach (var item in finYears)
                    {
                        finYearList.Add(new FinancialYear { FinYearId = item.FinYearId, FinYearDesc = item.FinYearDesc });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return finYearList;
        }

        public FinancialYear GetCurrentFinYear(int finYearId = 0)
        {
            FinancialYear currentfinYear = new FinancialYear();
            try
            {
                if (finYearId == 0)
                {
                    var finYears = entities.FinancialYears.Where(x => x.Status == true).OrderByDescending(o => o.FinYearId).Select(s => new
                    {
                        FinYearId = s.FinYearId,
                        FinYearDesc = s.FinYearDesc,
                        StartDate = s.StartDate,
                        EndDate = s.EndDate,
                        FinYearCode = s.FinYearCode
                    }).FirstOrDefault();
                    if (finYears != null)
                    {
                        currentfinYear.FinYearId = finYears.FinYearId;
                        currentfinYear.FinYearDesc = finYears.FinYearDesc;
                        currentfinYear.StartDate = finYears.StartDate;
                        currentfinYear.EndDate = finYears.EndDate;
                        currentfinYear.FinYearCode = finYears.FinYearCode;
                    }
                }
                else
                {
                    var finYears = entities.FinancialYears.Where(x => x.Status == true && x.FinYearId == finYearId).Select(s => new
                    {
                        FinYearId = s.FinYearId,
                        FinYearDesc = s.FinYearDesc,
                        StartDate = s.StartDate,
                        EndDate = s.EndDate,
                        FinYearCode = s.FinYearCode
                    }).FirstOrDefault();
                    if (finYears != null)
                    {
                        currentfinYear.FinYearId = finYears.FinYearId;
                        currentfinYear.FinYearDesc = finYears.FinYearDesc;
                        currentfinYear.StartDate = finYears.StartDate;
                        currentfinYear.EndDate = finYears.EndDate;
                        currentfinYear.FinYearCode = finYears.FinYearCode;
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return currentfinYear;
        }


        public List<FinancialYear> GetFinancialYearForEmployeeAppraisalTemplateList()
        {
            List<FinancialYear> finyear = new List<FinancialYear>();
            try
            {
                var finYearList = entities.FinancialYears.Where(x => x.Status == true).Select(s => new
                {
                    FinYearId = s.FinYearId,
                    FinYearDesc = s.FinYearDesc

                }).ToList();
                if (finYearList != null && finYearList.Count > 0)
                {
                    foreach (var item in finYearList)
                    {
                        finyear.Add(new FinancialYear { FinYearId = item.FinYearId, FinYearDesc = item.FinYearDesc });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return finyear;
        }

        #endregion

        #region State
        public ResponseOut AddEditState(State state)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.States.Any(x => x.Stateid != state.Stateid && x.StateCode == state.StateCode))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateStateCode;
                }
                else if (entities.States.Any(x => x.StateName == state.StateName && x.Stateid != state.Stateid))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateStateName;
                }
                else
                {
                    if (state.Stateid == 0)
                    {
                        entities.States.Add(state);
                        responseOut.message = ActionMessage.StateCreatedSuccess;
                    }
                    else
                    {
                        entities.States.Where(a => a.Stateid == state.Stateid).ToList().ForEach(a =>
                        {
                            a.StateName = state.StateName;
                            a.StateCode = state.StateCode;
                            a.CountryId = state.CountryId;
                        });
                        responseOut.message = ActionMessage.StateUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;

                }
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

        #region PaymentMode
        public ResponseOut AddEditPaymentMode(PaymentMode paymentMode)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                //if (entities.PaymentModes.Any(x => x.PaymentModeId != paymentMode.PaymentModeId && x.PaymentModeName == paymentMode.PaymentModeName))
                //{
                //    responseOut.status = ActionStatus.Fail;
                //    responseOut.message = ActionMessage.DuplicateStateCode;
                //}
                //else
                if (entities.PaymentModes.Any(x => x.PaymentModeName == paymentMode.PaymentModeName && x.PaymentModeId != paymentMode.PaymentModeId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicatePaymentModeName;
                }
                else
                {
                    if (paymentMode.PaymentModeId == 0)
                    {
                        entities.PaymentModes.Add(paymentMode);
                        responseOut.message = ActionMessage.PaymentModeCreatedSuccess;
                    }
                    else
                    {
                        entities.PaymentModes.Where(a => a.PaymentModeId == paymentMode.PaymentModeId).ToList().ForEach(a =>
                        {
                            a.PaymentModeName = paymentMode.PaymentModeName;
                            a.Status = paymentMode.Status;

                        });
                        responseOut.message = ActionMessage.PaymentModeUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;

                }
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
        public List<PaymentMode> GetPaymentModeList()
        {
            List<PaymentMode> paymentmodeList = new List<PaymentMode>();
            try
            {
                var paymentmodes = entities.PaymentModes.Where(x => x.Status == true).Select(s => new
                {
                    PaymentModeId = s.PaymentModeId,
                    PaymentModeName = s.PaymentModeName
                }).ToList();
                if (paymentmodes != null && paymentmodes.Count > 0)
                {
                    foreach (var item in paymentmodes)
                    {
                        paymentmodeList.Add(new PaymentMode { PaymentModeId = item.PaymentModeId, PaymentModeName = item.PaymentModeName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return paymentmodeList;
        }
        #endregion
        #region FormType
        public List<FormType> GetFormTypeList()
        {
            List<FormType> FormTypeList = new List<FormType>();
            try
            {
                var FormType = entities.FormTypes.Where(x => x.Status == true).Select(s => new
                {
                    FormTypeId = s.FormTypeId,
                    FormTypeDesc = s.FormTypeDesc
                }).ToList();
                if (FormType != null && FormType.Count > 0)
                {
                    foreach (var item in FormType)
                    {
                        FormTypeList.Add(new FormType { FormTypeId = item.FormTypeId, FormTypeDesc = item.FormTypeDesc });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return FormTypeList;
        }
        #endregion

        #region Role
        public ResponseOut AddEditRole(Role role)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.Roles.Any(x => x.RoleName == role.RoleName && x.CompanyId == role.CompanyId && x.RoleId != role.RoleId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateRoleName;
                }
                else
                {
                    if (role.RoleId == 0)
                    {
                        entities.Roles.Add(role);
                        responseOut.message = ActionMessage.RoleCreatedSuccess;
                    }
                    else
                    {
                        entities.Roles.Where(a => a.RoleId == role.RoleId).ToList().ForEach(a =>
                        {
                            a.RoleId = role.RoleId;
                            a.RoleName = role.RoleName;
                            a.RoleDesc = role.RoleDesc;
                            a.CompanyId = role.CompanyId;
                            a.IsAdmin = role.IsAdmin;
                            a.Status = role.Status;
                            a.CompanyBranchId = role.CompanyBranchId;
                            a.UserTypeId = role.UserTypeId;
                        });
                        responseOut.message = ActionMessage.RoleUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                }
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
        public List<Role> GetRoleList(int companyId)
        {
            List<Role> roleList = new List<Role>();
            try
            {
                var roles = entities.Roles.Where(x => x.CompanyId == companyId && (x.RoleId != (int)Roles.SuperAdmin && x.RoleId != (int)Roles.Admin) && x.Status == true).Select(s => new
                {
                    RoleId = s.RoleId,
                    RoleName = s.RoleName
                }).OrderBy(x => x.RoleName).ToList();
                if (roles != null && roles.Count > 0)
                {
                    foreach (var item in roles)
                    {
                        roleList.Add(new Role { RoleId = item.RoleId, RoleName = item.RoleName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return roleList;
        }

        public bool GetIsAdminByRoleId(int RoleId)
        {
            bool IsAdmin;
            try
            {
                IsAdmin = Convert.ToBoolean(entities.Roles.Where(s => s.RoleId == RoleId).Select(x => x.IsAdmin).SingleOrDefault());
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return IsAdmin;
        }
        public string GetCompanyBranchName(long companyBranchId)
        {
            string companyName = "";
            try
            {
                companyName = Convert.ToString(entities.ComapnyBranches.Where(s => s.CompanyBranchId == companyBranchId).Select(x => x.BranchName).SingleOrDefault());
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return companyName;
        }
        #endregion

        #region Role UI Mapping
        public ResponseOut AddEditRoleUIMapping(RoleUIActionMapping roleUIActionMapping)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.RoleUIActionMappings.Any(x => x.RoleId == roleUIActionMapping.RoleId && x.InterfaceId == roleUIActionMapping.InterfaceId))
                {
                    entities.RoleUIActionMappings.Where(a => a.RoleId == roleUIActionMapping.RoleId && a.InterfaceId == roleUIActionMapping.InterfaceId).ToList().ForEach(a =>
                    {
                        a.AddAccess = roleUIActionMapping.AddAccess;
                        a.EditAccess = roleUIActionMapping.EditAccess;
                        a.ViewAccess = roleUIActionMapping.ViewAccess;
                        a.CancelAccess = roleUIActionMapping.CancelAccess;
                        a.ReviseAccess = roleUIActionMapping.ReviseAccess;
                        a.Status = roleUIActionMapping.Status;
                        a.CompanyBranchId = roleUIActionMapping.CompanyBranchId;

                    });
                }
                else
                {
                    entities.RoleUIActionMappings.Add(roleUIActionMapping);
                }

                entities.SaveChanges();
                responseOut.message = ActionMessage.RoleUIMappingSuccessful;
                responseOut.status = ActionStatus.Success;
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

        //

        public ResponseOut DeleteRoleUIMapping(RoleUIActionMapping roleUIActionMapping)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                var itemToRemove = entities.RoleUIActionMappings.SingleOrDefault(x =>
                x.RoleId == roleUIActionMapping.RoleId && x.InterfaceId == roleUIActionMapping.InterfaceId); //returns a single item.

                if (itemToRemove != null)
                {
                    entities.RoleUIActionMappings.Remove(itemToRemove);
                    entities.SaveChanges();
                }


                responseOut.message = ActionMessage.RoleUIMappingSuccessful;
                responseOut.status = ActionStatus.Success;
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

        #region LeadStatus
        public ResponseOut AddEditLeadStatus(LeadStatu leadstatus)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.LeadStatus.Any(x => x.LeadStatusName == leadstatus.LeadStatusName && x.CompanyId == leadstatus.CompanyId && x.LeadStatusId != leadstatus.LeadStatusId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateLeadStatusName;
                }
                else
                {
                    if (leadstatus.LeadStatusId == 0)
                    {
                        entities.LeadStatus.Add(leadstatus);
                        responseOut.message = ActionMessage.LeadStatusCreatedSuccess;
                    }
                    else
                    {
                        entities.LeadStatus.Where(a => a.LeadStatusId == leadstatus.LeadStatusId).ToList().ForEach(a =>
                        {
                            a.LeadStatusId = leadstatus.LeadStatusId;
                            a.LeadStatusName = leadstatus.LeadStatusName;
                            a.CompanyId = leadstatus.CompanyId;
                            a.Status = leadstatus.Status;
                        });
                        responseOut.message = ActionMessage.LeadStatusUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                }
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


        #region LeadSource
        public ResponseOut AddEditLeadSource(LeadSource leadsource)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.LeadSources.Any(x => x.LeadSourceName == leadsource.LeadSourceName && x.CompanyId == leadsource.CompanyId && x.CompanyBranchId == leadsource.CompanyBranchId && x.LeadSourceId != leadsource.LeadSourceId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateLeadStatusName;
                }
                else
                {
                    if (leadsource.LeadSourceId == 0)
                    {
                        entities.LeadSources.Add(leadsource);
                        responseOut.message = ActionMessage.LeadSourceCreatedSuccess;
                    }
                    else
                    {
                        entities.LeadSources.Where(a => a.LeadSourceId == leadsource.LeadSourceId).ToList().ForEach(a =>
                        {
                            a.LeadSourceId = leadsource.LeadSourceId;
                            a.LeadSourceName = leadsource.LeadSourceName;
                            a.CompanyId = leadsource.CompanyId;
                            a.Status = leadsource.Status;
                            a.CompanyBranchId = leadsource.CompanyBranchId;
                        });
                        responseOut.message = ActionMessage.LeadSourceUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                }
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


        #region ProductType
        public ResponseOut AddEditProductType(ProductType producttype)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.ProductTypes.Any(x => x.ProductTypeName == producttype.ProductTypeName && x.ProductTypeId != producttype.ProductTypeId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateProductTypeName;
                }
                else if (entities.ProductTypes.Any(x => x.ProductTypeCode == producttype.ProductTypeCode && x.ProductTypeId != producttype.ProductTypeId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateProductTypeCode;
                }
                else
                {
                    if (producttype.ProductTypeId == 0)
                    {
                        entities.ProductTypes.Add(producttype);
                        responseOut.message = ActionMessage.ProductTypeCreatedSuccess;
                    }
                    else
                    {
                        entities.ProductTypes.Where(a => a.ProductTypeId == producttype.ProductTypeId).ToList().ForEach(a =>
                        {
                            a.ProductTypeId = producttype.ProductTypeId;
                            a.ProductTypeName = producttype.ProductTypeName;
                            a.ProductTypeCode = producttype.ProductTypeCode;
                            a.Status = producttype.Status;
                            a.CompanyBranchId = producttype.CompanyBranchId;
                        });
                        responseOut.message = ActionMessage.ProductTypeUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                }
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
        public List<ProductType> GetProductTypeList()
        {
            List<ProductType> productTypeList = new List<ProductType>();
            try
            {
                var productTypes = entities.ProductTypes.Where(x => x.Status == true).OrderBy(x => SqlFunctions.IsNumeric(x.ProductTypeName)).ThenBy(x => x.ProductTypeName).Select(s => new
                {
                    ProductTypeId = s.ProductTypeId,
                    ProductTypeName = s.ProductTypeName
                }).ToList();
                if (productTypes != null && productTypes.Count > 0)
                {
                    foreach (var item in productTypes)
                    {
                        productTypeList.Add(new ProductType { ProductTypeId = item.ProductTypeId, ProductTypeName = item.ProductTypeName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return productTypeList;
        }
        #endregion

        #region Product Main Group 

        public ResponseOut AddEditProductMainGroup(ProductMainGroup productmaingroup)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.ProductMainGroups.Any(x => x.ProductMainGroupName == productmaingroup.ProductMainGroupName && x.ProductMainGroupId != productmaingroup.ProductMainGroupId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateProductMainGroupName;
                }
                else if (entities.ProductMainGroups.Any(x => x.ProductMainGroupCode == productmaingroup.ProductMainGroupCode && x.ProductMainGroupId != productmaingroup.ProductMainGroupId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateProductMainGroupCode;
                }
                else
                {
                    if (productmaingroup.ProductMainGroupId == 0)
                    {
                        entities.ProductMainGroups.Add(productmaingroup);
                        responseOut.message = ActionMessage.ProductMainGroupCreatedSuccess;
                    }
                    else
                    {
                        entities.ProductMainGroups.Where(a => a.ProductMainGroupId == productmaingroup.ProductMainGroupId).ToList().ForEach(a =>
                        {
                            a.ProductMainGroupId = productmaingroup.ProductMainGroupId;
                            a.ProductMainGroupName = productmaingroup.ProductMainGroupName;
                            a.ProductMainGroupCode = productmaingroup.ProductMainGroupCode;
                            a.Status = productmaingroup.Status;
                        });
                        responseOut.message = ActionMessage.ProductMainGroupUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                }
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


        public List<ProductMainGroup> GetProductMainGroupList()
        {
            List<ProductMainGroup> productMainGroupList = new List<ProductMainGroup>();
            try
            {
                var productMainGroups = entities.ProductMainGroups.Where(x => x.Status == true).Select(s => new
                {
                    ProductMainGroupId = s.ProductMainGroupId,
                    ProductMainGroupName = s.ProductMainGroupName
                }).ToList();
                if (productMainGroups != null && productMainGroups.Count > 0)
                {
                    foreach (var item in productMainGroups)
                    {
                        productMainGroupList.Add(new ProductMainGroup { ProductMainGroupId = item.ProductMainGroupId, ProductMainGroupName = item.ProductMainGroupName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return productMainGroupList;
        }
        #endregion

        #region Product Sub Group

        public ResponseOut AddEditProductSubGroup(ProductSubGroup productsubgroup)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.ProductSubGroups.Any(x => x.ProductSubGroupName == productsubgroup.ProductSubGroupName && x.ProductSubGroupId != productsubgroup.ProductSubGroupId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateProductSubGroupName;
                }
                else if (entities.ProductSubGroups.Any(x => x.ProductSubGroupCode == productsubgroup.ProductSubGroupCode && x.ProductSubGroupId != productsubgroup.ProductSubGroupId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateProductSubGroupCode;
                }
                else
                {
                    if (productsubgroup.ProductSubGroupId == 0)
                    {
                        entities.ProductSubGroups.Add(productsubgroup);
                        responseOut.message = ActionMessage.ProductSubGroupCreatedSuccess;
                    }
                    else
                    {
                        entities.ProductSubGroups.Where(a => a.ProductSubGroupId == productsubgroup.ProductSubGroupId).ToList().ForEach(a =>
                        {
                            a.ProductSubGroupId = productsubgroup.ProductSubGroupId;
                            a.ProductSubGroupName = productsubgroup.ProductSubGroupName;
                            a.ProductSubGroupCode = productsubgroup.ProductSubGroupCode;
                            a.ProductMainGroupId = productsubgroup.ProductMainGroupId;
                            a.Status = productsubgroup.Status;
                        });
                        responseOut.message = ActionMessage.ProductSubGroupUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                }
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


        public List<ProductSubGroup> GetProductSubGroupList(int productMainGroupId)
        {
            List<ProductSubGroup> productSubGroupList = new List<ProductSubGroup>();
            try
            {
                var productSubGroups = entities.ProductSubGroups.Where(x => x.Status == true && x.ProductMainGroupId == productMainGroupId).Select(s => new
                {
                    ProductSubGroupId = s.ProductSubGroupId,
                    ProductSubGroupName = s.ProductSubGroupName
                }).ToList();
                if (productSubGroups != null && productSubGroups.Count > 0)
                {
                    foreach (var item in productSubGroups)
                    {
                        productSubGroupList.Add(new ProductSubGroup { ProductSubGroupId = item.ProductSubGroupId, ProductSubGroupName = item.ProductSubGroupName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return productSubGroupList;
        }

        public List<ProductSubGroup> GetAllProductSubGroupList()
        {
            List<ProductSubGroup> productSubGroupList = new List<ProductSubGroup>();
            try
            {
                var products = (from p in entities.ProductMainGroups
                                join ps in entities.ProductSubGroups on p.ProductMainGroupId equals ps.ProductMainGroupId
                                where (p.ProductMainGroupName == "FinishedGood" && ps.Status == true)
                                select new
                                {
                                    ProductSubGroupId = ps.ProductSubGroupId,
                                    ProductSubGroupName = ps.ProductSubGroupName

                                }).ToList();

                if (products != null && products.Count > 0)
                {
                    foreach (var item in products)
                    {
                        productSubGroupList.Add(new ProductSubGroup { ProductSubGroupId = item.ProductSubGroupId, ProductSubGroupName = item.ProductSubGroupName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return productSubGroupList;
        }

        public List<ProductSubGroup> GetProductSubGroupListForPMB()
        {
            List<ProductSubGroup> productSubGroupList = new List<ProductSubGroup>();
            try
            {
                var products = (from p in entities.ProductMainGroups
                                join ps in entities.ProductSubGroups on p.ProductMainGroupId equals ps.ProductMainGroupId
                                where (ps.Status == true)
                                select new
                                {
                                    ProductSubGroupId = ps.ProductSubGroupId,
                                    ProductSubGroupName = ps.ProductSubGroupName

                                }).ToList();

                if (products != null && products.Count > 0)
                {
                    foreach (var item in products)
                    {
                        productSubGroupList.Add(new ProductSubGroup { ProductSubGroupId = item.ProductSubGroupId, ProductSubGroupName = item.ProductSubGroupName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return productSubGroupList;
        }

        public List<ProductSubGroup> GetChasisModelSubGroupList()
        {
            List<ProductSubGroup> productSubGroupList = new List<ProductSubGroup>();
            try
            {
                var productSubGroups = entities.ProductSubGroups.Where(x => x.Status == true).OrderBy(x => SqlFunctions.IsNumeric(x.ProductSubGroupName)).ThenBy(x => x.ProductSubGroupName).Select(s => new
                {
                    ProductSubGroupId = s.ProductSubGroupId,
                    ProductSubGroupName = s.ProductSubGroupName
                }).ToList();
                if (productSubGroups != null && productSubGroups.Count > 0)
                {
                    foreach (var item in productSubGroups)
                    {
                        productSubGroupList.Add(new ProductSubGroup { ProductSubGroupId = item.ProductSubGroupId, ProductSubGroupName = item.ProductSubGroupName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return productSubGroupList;
        }
        #endregion

        #region UOM 
        public ResponseOut AddEditUOM(UOM uom)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.UOMs.Any(x => x.UOMName == uom.UOMName && x.UOMId != uom.UOMId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateUOMName;
                }
                else if (entities.UOMs.Any(x => x.UOMDesc == uom.UOMDesc && x.UOMId != uom.UOMId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateUOMDesc;
                }
                else
                {
                    if (uom.UOMId == 0)
                    {
                        entities.UOMs.Add(uom);
                        responseOut.message = ActionMessage.UOMCreatedSuccess;
                    }
                    else
                    {
                        entities.UOMs.Where(a => a.UOMId == uom.UOMId).ToList().ForEach(a =>
                        {
                            a.UOMId = uom.UOMId;
                            a.UOMName = uom.UOMName;
                            a.UOMDesc = uom.UOMDesc;
                            a.Status = uom.Status;
                        });
                        responseOut.message = ActionMessage.UOMUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                }
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

        public List<UOM> GetUOMList()
        {
            List<UOM> uomList = new List<UOM>();
            try
            {
                var uoms = entities.UOMs.Where(x => x.Status == true).Select(s => new
                {
                    UOMId = s.UOMId,
                    UOMName = s.UOMName
                }).ToList();
                if (uoms != null && uoms.Count > 0)
                {
                    foreach (var item in uoms)
                    {
                        uomList.Add(new UOM { UOMId = item.UOMId, UOMName = item.UOMName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return uomList;
        }
        #endregion

        #region Product
        public ResponseOut AddEditProduct(Product product)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {

                if (entities.Products.Any(x => x.ProductName == product.ProductName && x.CompanyId == product.CompanyId && x.Productid != product.Productid && x.CompanyBranchId==product.CompanyBranchId && x.ProductShortDesc==product.ProductShortDesc))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateProductName;
                }
                //else if (entities.Products.Any(x => x.ProductCode == product.ProductCode && x.CompanyId == product.CompanyId && x.Productid != product.Productid))
                //{
                //    responseOut.status = ActionStatus.Fail;
                //    responseOut.message = ActionMessage.DuplicateProductCode;
                //}
                else
                {
                    if (product.Productid == 0)
                    {
                        product.CreatedDate = DateTime.Now;
                        entities.Products.Add(product);
                        responseOut.message = ActionMessage.ProductCreatedSuccess;
                    }
                    else
                    {
                        product.ModifiedBy = product.CreatedBy;
                        product.ModifiedDate = DateTime.Now;

                        entities.Products.Where(a => a.Productid == product.Productid).ToList().ForEach(a =>
                        {

                            a.ProductName = product.ProductName;
                            a.ProductCode = product.ProductCode;
                            a.ProductShortDesc = product.ProductShortDesc;
                            a.ProductFullDesc = product.ProductFullDesc;
                            a.ProductTypeId = product.ProductTypeId;
                            a.ProductMainGroupId = product.ProductMainGroupId;
                            a.ProductSubGroupId = product.ProductSubGroupId;
                            a.AssemblyType = product.AssemblyType;
                            a.UOMId = product.UOMId;
                            a.PurchaseUOMId = product.PurchaseUOMId;
                            a.PurchasePrice = product.PurchasePrice;
                            a.SalePrice = product.SalePrice;
                            a.IsSerializedProduct = product.IsSerializedProduct;
                            a.BrandName = product.BrandName;
                            a.ReOrderQty = product.ReOrderQty;
                            a.MinOrderQty = product.MinOrderQty;
                            a.ModifiedBy = product.ModifiedBy;
                            a.ModifiedDate = product.ModifiedDate;
                            a.Status = product.Status;
                            a.CGST_Perc = product.CGST_Perc;
                            a.SGST_Perc = product.SGST_Perc;
                            a.IGST_Perc = product.IGST_Perc;
                            a.HSN_Code = product.HSN_Code;
                            a.GST_Exempt = product.GST_Exempt;
                            a.SizeId = product.SizeId;
                            a.Length = product.Length;
                            a.ManufacturerId = product.ManufacturerId;
                            a.ColourCode = product.ColourCode;
                            a.IsNonGST = product.IsNonGST;
                            a.IsWarrantyProduct = product.IsWarrantyProduct;
                            a.WarrantyInMonth = product.WarrantyInMonth;
                            a.IsNilRated = product.IsNilRated;
                            a.IsZeroRated = product.IsZeroRated;
                            a.IsThirdPartyProduct = product.IsThirdPartyProduct;
                            a.RackNo = product.RackNo;
                            a.ModelName = product.ModelName;
                            a.CC = product.CC;
                            a.HSNID = product.HSNID;
                            a.VendorId = product.VendorId;
                            a.Compatibility = product.Compatibility;
                            a.VehicleType = product.VehicleType;
                            a.CompanyBranchId = product.CompanyBranchId;
                            a.IsOnline = product.IsOnline;

                        });
                        responseOut.message = ActionMessage.ProductUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.trnId = product.Productid;
                    responseOut.status = ActionStatus.Success;

                }
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
        public ResponseOut UpdateProductPic(Product product)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                entities.Products.Where(a => a.Productid == product.Productid).ToList().ForEach(a =>
                  {
                      a.UserPicPath = product.UserPicPath;
                      a.UserPicName = product.UserPicName;
                  });

                entities.SaveChanges();
                responseOut.status = ActionStatus.Success;


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
        public List<Product> GetProductAutoCompleteList(string searchTerm, int companyId)
        {
            List<Product> productList = new List<Product>();
            try
            {
                var products = (from p in entities.Products
                                where (p.ProductName.ToLower().Contains(searchTerm.ToLower()) || p.ProductCode.ToLower().Contains(searchTerm.ToLower()) || p.ProductShortDesc.ToLower().Contains(searchTerm.ToLower())) && p.CompanyId == companyId && p.Status == true
                                select new
                                {
                                    ProductId = p.Productid,
                                    ProductName = p.ProductName,
                                    ProductCode = p.ProductCode,
                                    ProductShortDesc = p.ProductShortDesc,
                                    CGST_Perc = p.CGST_Perc,
                                    SGST_Perc = p.SGST_Perc,
                                    IGST_Perc = p.IGST_Perc,
                                    HSN_Code = p.HSN_Code,
                                    IsSerializedProduct = p.IsSerializedProduct,
                                    IsThirdPartyProduct = p.IsThirdPartyProduct,

                                    IsWarrantyProduct = p.IsWarrantyProduct,
                                    WarrantyInMonth = p.WarrantyInMonth,
                                    SalePrice = p.SalePrice

                                }).ToList();


                if (products != null && products.Count > 0)
                {
                    foreach (var item in products)
                    {
                        productList.Add(new Product
                        {
                            Productid = item.ProductId,
                            ProductName = item.ProductName,
                            ProductCode = item.ProductCode,
                            ProductShortDesc = item.ProductShortDesc,
                            CGST_Perc = item.CGST_Perc,
                            SGST_Perc = item.SGST_Perc,
                            IGST_Perc = item.IGST_Perc,
                            HSN_Code = item.HSN_Code,
                            IsSerializedProduct = item.IsSerializedProduct,
                            IsThirdPartyProduct = item.IsThirdPartyProduct,
                            IsWarrantyProduct = item.IsWarrantyProduct,
                            WarrantyInMonth = item.WarrantyInMonth,
                            SalePrice = item.SalePrice
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return productList;
        }
        public List<Product> GetProductTypeBYProductAutoCompleteList(string searchTerm, int companyId, int productTytpeId)
        {
            List<Product> productList = new List<Product>();
            try
            {
                var products = (from p in entities.Products
                                where (p.ProductName.ToLower().Contains(searchTerm.ToLower()) || p.ProductCode.ToLower().Contains(searchTerm.ToLower()) || p.ProductShortDesc.ToLower().Contains(searchTerm.ToLower())) && p.CompanyId == companyId && p.ProductTypeId == productTytpeId && p.Status == true
                                select new
                                {
                                    ProductId = p.Productid,
                                    ProductName = p.ProductName,
                                    ProductCode = p.ProductCode,
                                   

                                }).ToList();


                if (products != null && products.Count > 0)
                {
                    foreach (var item in products)
                    {
                        productList.Add(new Product
                        {
                            Productid = item.ProductId,
                            ProductName = item.ProductName,
                            ProductCode = item.ProductCode,
                          
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return productList;
        }
        public List<Product> GetProductAutoCompleteList(string searchTerm, int companyId, string assemblyType)
        {
            List<Product> productList = new List<Product>();
            try
            {
                if (assemblyType != "")
                {
                    var products = (from p in entities.Products
                                    where (p.ProductName.ToLower().Contains(searchTerm.ToLower()) || p.ProductCode.ToLower().Contains(searchTerm.ToLower()) || p.ProductShortDesc.ToLower().Contains(searchTerm.ToLower())) && p.CompanyId == companyId && p.AssemblyType == assemblyType && p.Status == true
                                    select new
                                    {
                                        ProductId = p.Productid,
                                        ProductName = p.ProductName,
                                        ProductCode = p.ProductCode,
                                        ProductShortDesc = p.ProductShortDesc
                                    }).ToList();
                    if (products != null && products.Count > 0)
                    {
                        foreach (var item in products)
                        {
                            productList.Add(new Product { Productid = item.ProductId, ProductName = item.ProductName, ProductCode = item.ProductCode, ProductShortDesc = item.ProductShortDesc });
                        }
                    }

                }
                else
                {
                    var products = (from p in entities.Products
                                    where (p.ProductName.ToLower().Contains(searchTerm.ToLower()) || p.ProductCode.ToLower().Contains(searchTerm.ToLower()) || p.ProductShortDesc.ToLower().Contains(searchTerm.ToLower())) && p.CompanyId == companyId && (p.AssemblyType == "MA" || p.AssemblyType == "SA") && p.Status == true
                                    select new
                                    {
                                        ProductId = p.Productid,
                                        ProductName = p.ProductName,
                                        ProductCode = p.ProductCode,
                                        ProductShortDesc = p.ProductShortDesc
                                    }).ToList();

                    if (products != null && products.Count > 0)
                    {
                        foreach (var item in products)
                        {
                            productList.Add(new Product { Productid = item.ProductId, ProductName = item.ProductName, ProductCode = item.ProductCode, ProductShortDesc = item.ProductShortDesc });
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return productList;
        }
        public List<Product> GetSubAssemblyAutoCompleteList(string searchTerm, int companyId)
        {
            List<Product> productList = new List<Product>();
            try
            {
                var products = (from p in entities.Products
                                where (p.ProductName.ToLower().Contains(searchTerm.ToLower()) || p.ProductCode.ToLower().Contains(searchTerm.ToLower()) || p.ProductShortDesc.ToLower().Contains(searchTerm.ToLower())) && p.CompanyId == companyId && (p.AssemblyType == "RC" || p.AssemblyType == "SA") && p.Status == true
                                select new
                                {
                                    ProductId = p.Productid,
                                    ProductName = p.ProductName,
                                    ProductCode = p.ProductCode,
                                    ProductShortDesc = p.ProductShortDesc
                                }).ToList();


                if (products != null && products.Count > 0)
                {
                    foreach (var item in products)
                    {
                        productList.Add(new Product { Productid = item.ProductId, ProductName = item.ProductName, ProductCode = item.ProductCode, ProductShortDesc = item.ProductShortDesc });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return productList;
        }

        public List<ProductSubGroup> GetProductSubGroupAutoCompleteList(string searchTerm, int companyId)
        {
            List<ProductSubGroup> productSubGroupList = new List<ProductSubGroup>();
            try
            {
                var productSubGroups = (from p in entities.ProductSubGroups
                                        where (p.ProductSubGroupName.ToLower().Contains(searchTerm.ToLower()) || p.ProductSubGroupCode.ToLower().Contains(searchTerm.ToLower())) && p.Status == true
                                        select new
                                        {
                                            ProductSubGroupId = p.ProductSubGroupId,
                                            ProductSubGroupName = p.ProductSubGroupName,
                                            ProductSubGroupCode = p.ProductSubGroupCode

                                        }).ToList();


                if (productSubGroups != null && productSubGroups.Count > 0)
                {
                    foreach (var item in productSubGroups)
                    {
                        productSubGroupList.Add(new ProductSubGroup { ProductSubGroupId = item.ProductSubGroupId, ProductSubGroupName = item.ProductSubGroupName, ProductSubGroupCode = item.ProductSubGroupCode });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return productSubGroupList;
        }

        public Product GetProductTaxPercentage(long productId)
        {
            Product ProductList = new Product();
            try
            {
                var products = entities.Products.Where(x => x.Productid == productId && x.Status == true).FirstOrDefault();
                if (products != null)
                {
                    ProductList = new Product
                    {
                        CGST_Perc = products.CGST_Perc,
                        SGST_Perc = products.SGST_Perc,
                        IGST_Perc = products.IGST_Perc,


                    };

                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return ProductList;
        }

        public int GetIdBySizeDesc(string sizeDesc)
        {
            int sizeId = 0;
            try
            {
                sizeId = entities.Sizes.Where(s => s.SizeDesc.Trim().ToUpper() == sizeDesc.Trim().ToUpper()).Select(x => x.SizeId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return sizeId;
        }
        public int GetIdByManufacturerName(string manufacturerName)
        {
            int manfId = 0;
            try
            {
                manfId = entities.Manufacturers.Where(s => s.ManufacturerName.Trim().ToUpper() == manufacturerName.Trim().ToUpper()).Select(x => x.ManufacturerId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return manfId;
        }

        public ResponseOut RemoveImage(long productId)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                entities.Products.Where(a => a.Productid == productId).ToList().ForEach(a =>
                {
                    a.UserPicName = null;
                    a.UserPicPath = null;
                });
                responseOut.message = ActionMessage.ProductUpdatedSuccess;
                entities.SaveChanges();
                responseOut.status = ActionStatus.Success;

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


        public List<ChasisSerialMapping> GetChasisSerialNoAutoCompleteList(string searchTerm)
        {

            List<ChasisSerialMapping> chasisSerialMappings = new List<ChasisSerialMapping>();
            try
            {
                var chasisSerials = (from c in entities.ChasisSerialMappings
                                     where (c.ChasisSerialNo.ToLower().Contains(searchTerm.ToLower())) && c.Status == true && c.ProductSerialStatus != "SOLD" && c.ProductSerialStatus != "TRANSFERRED"
                                     select new
                                     {
                                         ProductId = c.ProductId,
                                         ChasisSerialNo = c.ChasisSerialNo,
                                         MotorNo = c.MotorNo,
                                         ControllerNo = c.ControllerNo
                                     }).ToList();


                if (chasisSerials != null && chasisSerials.Count > 0)
                {
                    foreach (var item in chasisSerials)
                    {
                        chasisSerialMappings.Add(new ChasisSerialMapping
                        {
                            ProductId = item.ProductId,
                            ChasisSerialNo = item.ChasisSerialNo,
                            MotorNo = item.MotorNo,
                            ControllerNo = item.ControllerNo
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return chasisSerialMappings;
        }

        public List<ChasisSerialMapping> GetChasisSerialNoList()
        {

            List<ChasisSerialMapping> chasisSerialMappings = new List<ChasisSerialMapping>();
            try
            {
                var chasisSerials = (from c in entities.ChasisSerialMappings
                                     where (c.Status == true && c.ProductSerialStatus != "SOLD")
                                     select new
                                     {
                                         ProductId = c.ProductId,
                                         ChasisSerialNo = c.ChasisSerialNo,
                                         MotorNo = c.MotorNo,
                                         ControllerNo = c.ControllerNo,
                                         ComapnyBranch = c.CompanyBranchId
                                     }).ToList();


                if (chasisSerials != null && chasisSerials.Count > 0)
                {
                    foreach (var item in chasisSerials)
                    {
                        chasisSerialMappings.Add(new ChasisSerialMapping
                        {
                            ProductId = item.ProductId,
                            ChasisSerialNo = item.ChasisSerialNo,
                            MotorNo = item.MotorNo,
                            ControllerNo = item.ControllerNo,
                            CompanyBranchId = item.ComapnyBranch
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return chasisSerialMappings;
        }
        #endregion

        #region PaymentTerm
        public ResponseOut AddEditPaymentTerm(PaymentTerm paymentterm)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.PaymentTerms.Any(x => x.PaymentTermDesc == paymentterm.PaymentTermDesc && x.CompanyId != paymentterm.CompanyId && x.PaymentTermId == paymentterm.PaymentTermId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicatePaymentTermDesc;
                }
                else
                {
                    if (paymentterm.PaymentTermId == 0)
                    {
                        entities.PaymentTerms.Add(paymentterm);
                        responseOut.message = ActionMessage.PaymentTermCreatedSuccess;
                    }
                    else
                    {
                        entities.PaymentTerms.Where(a => a.PaymentTermId == paymentterm.PaymentTermId).ToList().ForEach(a =>
                        {
                            a.PaymentTermId = paymentterm.PaymentTermId;
                            a.PaymentTermDesc = paymentterm.PaymentTermDesc;
                            a.Status = paymentterm.Status;
                        });
                        responseOut.message = ActionMessage.PaymentTermUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                }
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

        #region Department
        public ResponseOut AddEditDepartment(Department department)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.Departments.Any(x => x.DepartmentName == department.DepartmentName && x.CompanyId == department.CompanyId && x.DepartmentId != department.DepartmentId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateDepartmentName;
                }
                else if (entities.Departments.Any(x => x.DepartmentCode == department.DepartmentCode && x.CompanyId == department.CompanyId && x.DepartmentId != department.DepartmentId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateDepartmentCode;
                }
                else
                {
                    if (department.DepartmentId == 0)
                    {
                        entities.Departments.Add(department);
                        responseOut.message = ActionMessage.DepartmentCreatedSuccess;
                    }
                    else
                    {
                        entities.Departments.Where(a => a.DepartmentId == department.DepartmentId).ToList().ForEach(a =>
                        {
                            a.DepartmentId = department.DepartmentId;
                            a.DepartmentName = department.DepartmentName;
                            a.DepartmentCode = department.DepartmentCode;
                            a.Status = department.Status;
                            a.CompanyBranchId = department.CompanyBranchId;
                        });
                        responseOut.message = ActionMessage.DepartmentUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                }
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

        public List<Department> GetDepartmentList(int companyId)
        {
            List<Department> departmentList = new List<Department>();
            try
            {
                var departments = entities.Departments.Where(x => x.CompanyId == companyId && x.Status == true).OrderBy(x => SqlFunctions.IsNumeric(x.DepartmentName)).ThenBy(x => x.DepartmentName).Select(s => new
                {
                    DepartmentId = s.DepartmentId,
                    DepartmentName = s.DepartmentName,
                    DepartmentCode = s.DepartmentCode
                }).ToList();
                if (departments != null && departments.Count > 0)
                {
                    foreach (var item in departments)
                    {
                        departmentList.Add(new Department { DepartmentId = item.DepartmentId, DepartmentName = item.DepartmentName, DepartmentCode = item.DepartmentCode });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return departmentList;
        }
        public List<Department> GetDepartmentsList(int departmentID)
        {
            List<Department> departmentList = new List<Department>();
            try
            {
                var companies = entities.Departments.Where(x => x.DepartmentId == departmentID && x.Status == true).Select(s => new
                {
                    DepartmentId = s.DepartmentId,
                    DepartmentName = s.DepartmentName
                }).ToList();
                if (companies != null && companies.Count > 0)
                {
                    foreach (var item in companies)
                    {
                        departmentList.Add(new Department { DepartmentId = item.DepartmentId, DepartmentName = item.DepartmentName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return departmentList;
        }
        #endregion

        #region Designation
        public ResponseOut AddEditDesignation(Designation designation)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.Designations.Any(x => x.DesignationName == designation.DesignationName && x.DesignationId != designation.DesignationId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateDesignationName;
                }
                else if (entities.Designations.Any(x => x.DesignationCode == designation.DesignationCode && x.DesignationId != designation.DesignationId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateDesignationCode;
                }
                else
                {
                    if (designation.DesignationId == 0)
                    {
                        entities.Designations.Add(designation);
                        responseOut.message = ActionMessage.DesignationCreatedSuccess;
                    }
                    else
                    {
                        entities.Designations.Where(a => a.DesignationId == designation.DesignationId).ToList().ForEach(a =>
                        {
                            a.DesignationId = designation.DesignationId;
                            a.DesignationName = designation.DesignationName;
                            a.DesignationCode = designation.DesignationCode;
                            a.DepartmentId = designation.DepartmentId;
                            a.Status = designation.Status;
                        });
                        responseOut.message = ActionMessage.DesignationUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                }
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

        public List<Designation> GetDesignationList(int departmentId)
        {
            List<Designation> designationList = new List<Designation>();
            try
            {
                var designations = entities.Designations.Where(x => x.DepartmentId == departmentId && x.Status == true).OrderBy(x => SqlFunctions.IsNumeric(x.DesignationName)).ThenBy(x => x.DesignationName).Select(s => new
                {
                    DesignationId = s.DesignationId,
                    DesignationName = s.DesignationName,
                    DesignationCode = s.DesignationCode
                }).ToList();
                if (designations != null && designations.Count > 0)
                {
                    foreach (var item in designations)
                    {
                        designationList.Add(new Designation { DesignationId = item.DesignationId, DesignationName = item.DesignationName, DesignationCode = item.DesignationCode });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return designationList;
        }

        public List<Designation> GetAllDesignationList()
        {
            List<Designation> designationList = new List<Designation>();
            try
            {
                var designations = entities.Designations.Where(x => x.Status == true).Select(s => new
                {
                    DesignationId = s.DesignationId,
                    DesignationName = s.DesignationName,
                    DesignationCode = s.DesignationCode
                }).ToList();
                if (designations != null && designations.Count > 0)
                {
                    foreach (var item in designations)
                    {
                        designationList.Add(new Designation { DesignationId = item.DesignationId, DesignationName = item.DesignationName, DesignationCode = item.DesignationCode });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return designationList;
        }
        public List<Designation> GetDesignationList()
        {
            List<Designation> designationList = new List<Designation>();
            try
            {
                var designations = entities.Designations.Where(x => x.Status == true).OrderBy(x => SqlFunctions.IsNumeric(x.DesignationName)).ThenBy(x => x.DesignationName).Select(s => new
                {
                    DesignationId = s.DesignationId,
                    DesignationName = s.DesignationName,
                    DesignationCode = s.DesignationCode
                }).ToList();
                if (designations != null && designations.Count > 0)
                {
                    foreach (var item in designations)
                    {
                        designationList.Add(new Designation { DesignationId = item.DesignationId, DesignationName = item.DesignationName, DesignationCode = item.DesignationCode });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return designationList;
        }
        #endregion

        #region Product Opening Stock
        public ResponseOut AddEditProductOpening(ProductOpeningStock productOpeningStock)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.ProductOpeningStocks.Any(x => x.ProductId == productOpeningStock.ProductId && x.CompanyId == productOpeningStock.CompanyId && x.CompanyBranchId == productOpeningStock.CompanyBranchId && x.FinYearId == productOpeningStock.FinYearId && x.OpeningTrnId != productOpeningStock.OpeningTrnId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateProductOpening;
                }

                else
                {
                    if (productOpeningStock.OpeningTrnId == 0)
                    {
                        productOpeningStock.CreatedDate = DateTime.Now;
                        entities.ProductOpeningStocks.Add(productOpeningStock);
                        responseOut.message = ActionMessage.ProductOpeningCreatedSuccess;
                    }
                    else
                    {
                        productOpeningStock.ModifiedBy = productOpeningStock.CreatedBy;
                        productOpeningStock.ModifiedDate = DateTime.Now;

                        entities.ProductOpeningStocks.Where(a => a.OpeningTrnId == productOpeningStock.OpeningTrnId).ToList().ForEach(a =>
                        {
                            a.ProductId = productOpeningStock.ProductId;
                            a.FinYearId = productOpeningStock.FinYearId;
                            a.CompanyBranchId = productOpeningStock.CompanyBranchId;
                            a.OpeningQty = productOpeningStock.OpeningQty;
                            a.ModifiedBy = productOpeningStock.ModifiedBy;
                            a.ModifiedDate = productOpeningStock.ModifiedDate;

                        });
                        responseOut.message = ActionMessage.ProductOpeningUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;

                }
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

        #region Lead


        public List<LeadSource> GetLeadSourceList()
        {
            List<LeadSource> leadsourceList = new List<LeadSource>();
            try
            {
                var leadsources = entities.LeadSources.Where(x => x.Status == true).OrderBy(x => SqlFunctions.IsNumeric(x.LeadSourceName)).ThenBy(x => x.LeadSourceName).Select(s => new
                {
                    LeadSourceId = s.LeadSourceId,
                    LeadSourceName = s.LeadSourceName,
                    CompanyBranchId = s.CompanyBranchId

                }).ToList();
                if (leadsources != null && leadsources.Count > 0)
                {
                    foreach (var item in leadsources)
                    {
                        leadsourceList.Add(new LeadSource { LeadSourceId = item.LeadSourceId, LeadSourceName = item.LeadSourceName, CompanyBranchId = item.CompanyBranchId });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return leadsourceList;
        }

        public List<LeadMaster> GetAllLeadTypeList()
        {

            List<LeadMaster> AllLeadTypeList = new List<LeadMaster>();
            try
            {
                var LeadTypeList = entities.LeadMasters.Where(x => x.status == true).OrderBy(x => SqlFunctions.IsNumeric(x.LeadTypeName)).ThenBy(x => x.LeadTypeName).Select(s => new
                {
                    LeadTypeId = s.LeadTypeId,
                    LeadTypeName = s.LeadTypeName
                }).ToList();
                if (LeadTypeList != null && LeadTypeList.Count > 0)
                {
                    foreach (var item in LeadTypeList)
                    {
                        AllLeadTypeList.Add(new LeadMaster { LeadTypeId = item.LeadTypeId, LeadTypeName = item.LeadTypeName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return AllLeadTypeList;
        }


        public List<FollowUpActivityType> GetFollowUpActivityTypeList()
        {

            List<FollowUpActivityType> followupactivitytypelist = new List<FollowUpActivityType>();
            try
            {
                var followupactivitys = entities.FollowUpActivityTypes.Where(x => x.Status == true).Select(s => new
                {
                    FollowUpActivityTypeId = s.FollowUpActivityTypeId,
                    FollowUpActivityTypeName = s.FollowUpActivityTypeName
                }).ToList();
                if (followupactivitys != null && followupactivitys.Count > 0)
                {
                    foreach (var item in followupactivitys)
                    {
                        followupactivitytypelist.Add(new FollowUpActivityType { FollowUpActivityTypeId = item.FollowUpActivityTypeId, FollowUpActivityTypeName = item.FollowUpActivityTypeName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return followupactivitytypelist;
        }


        public List<LeadStatu> GetLeadStatusList()
        {
            List<LeadStatu> leadstatusList = new List<LeadStatu>();
            try
            {
                var leadstatuses = entities.LeadStatus.Where(x => x.Status == true).OrderBy(x => SqlFunctions.IsNumeric(x.LeadStatusName)).ThenBy(x => x.LeadStatusName).Select(s => new
                {
                    LeadStatusId = s.LeadStatusId,
                    LeadStatusName = s.LeadStatusName
                }).ToList();
                if (leadstatuses != null && leadstatuses.Count > 0)
                {
                    foreach (var item in leadstatuses)
                    {
                        leadstatusList.Add(new LeadStatu { LeadStatusId = item.LeadStatusId, LeadStatusName = item.LeadStatusName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return leadstatusList;
        }

        public ResponseOut AddEditLead(Lead lead, out int leadId)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                leadId = lead.LeadId;
                if (entities.Leads.Any(x => x.LeadCode == lead.LeadCode && x.CompanyId == lead.CompanyId && x.LeadId != lead.LeadId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateLeadCode;
                }
                else if (entities.Leads.Any(x => x.CompanyName == lead.CompanyName && x.ContactNo == lead.ContactNo && x.CompanyId == lead.CompanyId && x.CompanyBranchId == lead.CompanyBranchId && x.LeadStatusId != 4 && x.LeadId != lead.LeadId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateLead;
                }
                else
                {
                    if (lead.LeadId == 0)
                    {
                        lead.CreatedDate = DateTime.Now;
                        entities.Leads.Add(lead);
                        responseOut.message = ActionMessage.LeadCreatedSuccess;
                    }
                    else
                    {
                        lead.ModifiedBy = lead.CreatedBy;
                        lead.ModifiedDate = DateTime.Now;
                        entities.Leads.Where(a => a.LeadId == lead.LeadId).ToList().ForEach(a =>
                        {
                            a.LeadCode = lead.LeadCode;
                            a.CompanyId = lead.CompanyId;
                            a.CompanyName = lead.CompanyName;
                            a.ContactPersonName = lead.ContactPersonName;
                            a.Email = lead.Email;
                            a.AlternateEmail = lead.AlternateEmail;
                            a.ContactNo = lead.ContactNo;
                            a.AlternateContactNo = lead.AlternateContactNo;
                            a.Fax = lead.Fax;
                            a.Designation = lead.Designation;
                            a.CompanyAddress = lead.CompanyAddress;
                            a.BranchAddress = lead.BranchAddress;
                            a.City = lead.City;
                            a.StateId = lead.StateId;
                            a.CountryId = lead.CountryId;
                            a.PinCode = lead.PinCode;
                            a.CompanyCity = lead.CompanyCity;
                            a.CompanyStateId = lead.CompanyStateId;
                            a.CompanyCountryId = lead.CompanyCountryId;
                            a.CompanyPinCode = lead.CompanyPinCode;
                            a.LeadStatusId = lead.LeadStatusId;
                            a.LeadSourceId = lead.LeadSourceId;
                            a.OtherLeadSourceDescription = lead.OtherLeadSourceDescription;
                            a.ModifiedBy = lead.ModifiedBy;
                            a.ModifiedDate = lead.ModifiedDate;
                            a.Status = lead.Status;
                            a.LeadTypeId = lead.LeadTypeId;
                            a.CompanyBranchId = lead.CompanyBranchId;
                        });
                        responseOut.message = ActionMessage.LeadUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    leadId = lead.LeadId;
                    responseOut.status = ActionStatus.Success;

                }
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
        public ResponseOut AddEditLeadFollowUp(LeadFollowUp leadFollowUp)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {

                if (leadFollowUp.LeadFollowUpId == 0)
                {
                    leadFollowUp.CreatedDate = DateTime.Now;
                    entities.LeadFollowUps.Add(leadFollowUp);

                }
                else
                {
                    leadFollowUp.ModifiedDate = DateTime.Now;
                    entities.LeadFollowUps.Where(a => a.LeadFollowUpId == leadFollowUp.LeadFollowUpId).ToList().ForEach(a =>
                     {

                         a.FollowUpActivityTypeId = leadFollowUp.FollowUpActivityTypeId;
                         a.FollowUpDueDateTime = leadFollowUp.FollowUpDueDateTime;
                         a.FollowUpReminderDateTime = leadFollowUp.FollowUpReminderDateTime;
                         a.FollowUpRemarks = leadFollowUp.FollowUpRemarks;
                         a.Priority = leadFollowUp.Priority;
                         a.LeadStatusId = leadFollowUp.LeadStatusId;
                         a.LeadStatusReason = leadFollowUp.LeadStatusReason;
                         a.FollowUpByUserId = leadFollowUp.FollowUpByUserId;
                         a.ModifiedBy = leadFollowUp.CreatedBy;
                         a.ModifiedDate = DateTime.Now;
                     });
                }
                entities.SaveChanges();
                responseOut.status = ActionStatus.Success;
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
        public void UpdateLastLeadStatus(Int32 leadid, int leadStatusId, int modifiedBy = 0)
        {
            Lead lead = (from s in entities.Leads
                         where s.LeadId == leadid
                         select s).FirstOrDefault();

            LeadStatu leadStatus = (from s in entities.LeadStatus
                                    where s.LeadStatusId == leadStatusId
                                    select s).FirstOrDefault();

            lead.LeadStatusId = leadStatusId;
            lead.ModifiedBy = modifiedBy;
            lead.ModifiedDate = DateTime.Now;


            if (leadStatus.LeadStatusName.ToUpper() == "QUOTATION")
            {
                ResponseOut responseOut = new ResponseOut();

                if (entities.Customers.Any(x => x.CustomerCode == lead.LeadCode))
                {

                }
                else if (entities.Customers.Any(x => x.CustomerName == lead.CompanyName && x.MobileNo == lead.ContactNo))
                {

                }
                else
                {
                    Customer customer = new Customer
                    {
                        CustomerId = 0,
                        LeadId = lead.LeadId,
                        CustomerCode = lead.LeadCode,
                        CustomerName = lead.CompanyName,
                        ContactPersonName = lead.ContactPersonName,
                        Designation = lead.Designation,
                        Email = lead.Email,
                        MobileNo = lead.ContactNo,
                        ContactNo = lead.ContactNo,
                        Fax = lead.Fax,
                        PrimaryAddress = lead.CompanyAddress,
                        City = lead.CompanyCity,
                        StateId = lead.CompanyStateId,
                        CountryId = lead.CompanyCountryId,
                        PinCode = lead.PinCode,
                        CSTNo = "",
                        TINNo = "",
                        PANNo = "",
                        GSTNo = "",
                        ExciseNo = "",
                        EmployeeId = 0,
                        CustomerTypeId = 5,
                        CompanyId = lead.CompanyId,
                        CreatedBy = lead.CreatedBy,
                        CreatedDate = DateTime.Now,
                        CreditLimit = 0,
                        CreditDays = 0,
                        Status = true,
                        GST_Exempt = false
                    };
                    entities.Customers.Add(customer);

                    SL sl = new SL
                    {
                        SLId = 0,
                        SLCode = lead.LeadCode,
                        SLHead = lead.CompanyName,
                        RefCode = lead.LeadCode,
                        SLTypeId = 2,
                        CostCenterId = 0,
                        SubCostCenterId = 0,
                        CompanyId = lead.CompanyId,
                        CreatedBy = lead.CreatedBy,
                        CreatedDate = DateTime.Now,
                        Status = true
                    };
                    entities.SLs.Add(sl);
                }
            }

            entities.SaveChanges();
        }

        public List<User> GetUserAutoCompleteList(string searchTerm, long companyBranchID)
        {
            List<User> userList = new List<User>();
            try
            {

                if (companyBranchID != 0)
                {
                    var allusers = (from user in entities.Users
                                    where ((user.FullName.ToLower().Contains(searchTerm.ToLower()) || user.UserName.ToLower().Contains(searchTerm.ToLower()) || user.MobileNo.Contains(searchTerm)) && user.CompanyBranchId == companyBranchID && (user.Status == true && (user.RoleId != (int)Roles.Admin && user.RoleId != (int)Roles.SuperAdmin)))
                                    select new
                                    {

                                        FullName = user.FullName,
                                        UserName = user.UserName,
                                        UserId = user.UserId,
                                        MobileNo = user.MobileNo
                                    }).ToList();
                    if (allusers != null && allusers.Count > 0)
                    {
                        foreach (var item in allusers)
                        {

                            userList.Add(new User
                            {

                                FullName = item.FullName,
                                UserName = item.UserName,
                                UserId = item.UserId,
                                MobileNo = item.MobileNo

                            });

                        }
                    }
                }

                else
                {
                    var allusers = (from user in entities.Users
                                    where ((user.FullName.ToLower().Contains(searchTerm.ToLower()) || user.UserName.ToLower().Contains(searchTerm.ToLower()) || user.MobileNo.Contains(searchTerm)) && (user.Status == true && (user.RoleId != (int)Roles.Admin && user.RoleId != (int)Roles.SuperAdmin)))
                                    select new
                                    {

                                        FullName = user.FullName,
                                        UserName = user.UserName,
                                        UserId = user.UserId,
                                        MobileNo = user.MobileNo
                                    }).ToList();
                    if (allusers != null && allusers.Count > 0)
                    {
                        foreach (var item in allusers)
                        {

                            userList.Add(new User
                            {

                                FullName = item.FullName,
                                UserName = item.UserName,
                                UserId = item.UserId,
                                MobileNo = item.MobileNo

                            });

                        }
                    }
                }
            }

            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return userList;
        }

        public List<User> GetUserAutoCompleteList(string searchTerm)
        {
            List<User> userList = new List<User>();
            try
            {
                var allusers = (from user in entities.Users
                                where ((user.FullName.ToLower().Contains(searchTerm.ToLower()) || user.UserName.ToLower().Contains(searchTerm.ToLower()) || user.MobileNo.Contains(searchTerm)) && (user.Status == true && (user.RoleId != (int)Roles.Admin && user.RoleId != (int)Roles.SuperAdmin)))
                                select new
                                {

                                    FullName = user.FullName,
                                    UserName = user.UserName,
                                    UserId = user.UserId,
                                    MobileNo = user.MobileNo
                                }).ToList();
                if (allusers != null && allusers.Count > 0)
                {
                    foreach (var item in allusers)
                    {

                        userList.Add(new User
                        {

                            FullName = item.FullName,
                            UserName = item.UserName,
                            UserId = item.UserId,
                            MobileNo = item.MobileNo

                        });

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return userList;
        }
        #endregion

        #region Product BOM


        public ResponseOut AddEditProductBOM(ProductBOM productBOM)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.ProductBOMs.Any(x => x.AssemblyId == productBOM.AssemblyId && x.ProductId == productBOM.ProductId && x.BOMId != productBOM.BOMId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateBOM;
                }

                else
                {
                    if (productBOM.BOMId == 0)
                    {
                        productBOM.CreatedDate = DateTime.Now;
                        entities.ProductBOMs.Add(productBOM);
                        responseOut.message = ActionMessage.ProductBOMCreatedSuccess;
                    }
                    else
                    {
                        productBOM.ModifiedBy = productBOM.CreatedBy;
                        productBOM.ModifiedDate = DateTime.Now;

                        entities.ProductBOMs.Where(a => a.BOMId == productBOM.BOMId).ToList().ForEach(a =>
                        {
                            a.AssemblyId = productBOM.AssemblyId;
                            a.ProductId = productBOM.ProductId;
                            a.BOMQty = productBOM.BOMQty;
                            a.ProcessType = productBOM.ProcessType;
                            a.CompanyId = productBOM.CompanyId;
                            a.ScrapPercentage = productBOM.ScrapPercentage;
                            a.ModifiedBy = productBOM.ModifiedBy;
                            a.ModifiedDate = productBOM.ModifiedDate;
                            a.Status = productBOM.Status;
                            a.CompanyBranchId = productBOM.CompanyBranchId;

                        });

                        responseOut.message = ActionMessage.ProductBOMUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;

                }
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



        public ResponseOut AddEditProductBOM(ProductBOM productBOM, ProductBomManufacturingExpense productBomManufacturingExpense)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.ProductBOMs.Any(x => x.AssemblyId == productBOM.AssemblyId && x.ProductId == productBOM.ProductId && x.BOMId != productBOM.BOMId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateBOM;
                }

                else
                {
                    if (productBOM.BOMId == 0)
                    {

                        if (entities.ProductBomManufacturingExpenses.Any(b => b.AssemblyId == productBomManufacturingExpense.AssemblyId))
                        {

                            entities.ProductBomManufacturingExpenses.Where(a => a.AssemblyId == productBOM.AssemblyId).ToList().ForEach(a =>
                            {
                                a.Electricityexpenses = productBomManufacturingExpense.Electricityexpenses;
                                a.LabourExpense = productBomManufacturingExpense.LabourExpense;
                                a.OtherExpense = productBomManufacturingExpense.OtherExpense;
                            });
                        }
                        else
                        {
                            entities.ProductBomManufacturingExpenses.Add(productBomManufacturingExpense);
                        }


                        productBOM.CreatedDate = DateTime.Now;
                        entities.ProductBOMs.Add(productBOM);
                        responseOut.message = ActionMessage.ProductBOMCreatedSuccess;
                    }
                    else
                    {
                        productBOM.ModifiedBy = productBOM.CreatedBy;
                        productBOM.ModifiedDate = DateTime.Now;

                        entities.ProductBOMs.Where(a => a.BOMId == productBOM.BOMId).ToList().ForEach(a =>
                        {
                            a.AssemblyId = productBOM.AssemblyId;
                            a.ProductId = productBOM.ProductId;
                            a.BOMQty = productBOM.BOMQty;
                            a.ProcessType = productBOM.ProcessType;
                            a.CompanyId = productBOM.CompanyId;
                            a.ScrapPercentage = productBOM.ScrapPercentage;
                            a.ModifiedBy = productBOM.ModifiedBy;
                            a.ModifiedDate = productBOM.ModifiedDate;
                            a.Status = productBOM.Status;
                            a.CompanyBranchId = productBOM.CompanyBranchId;

                        });

                        if (entities.ProductBomManufacturingExpenses.Any(b => b.AssemblyId == productBomManufacturingExpense.AssemblyId))
                        {
                            entities.ProductBomManufacturingExpenses.Where(a => a.AssemblyId == productBOM.AssemblyId).ToList().ForEach(a =>
                            {
                                a.Electricityexpenses = productBomManufacturingExpense.Electricityexpenses;
                                a.LabourExpense = productBomManufacturingExpense.LabourExpense;
                                a.OtherExpense = productBomManufacturingExpense.OtherExpense;
                            });
                        }
                        else
                        {
                            entities.ProductBomManufacturingExpenses.Add(productBomManufacturingExpense);
                        }


                        responseOut.message = ActionMessage.ProductBOMUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;

                }
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
        public ResponseOut CopyProductBOM(long copyFromAssemblyId, long copyToAssemblyId, int createdBy)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.ProductBOMs.Any(x => x.AssemblyId == copyToAssemblyId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateBOM;
                }

                else
                {
                    int rowsInserted = entities.proc_CopyAssemblyBOM(copyFromAssemblyId, copyToAssemblyId, createdBy);
                    if (rowsInserted > 0)
                    {
                        responseOut.message = ActionMessage.ProductBOMCopySuccess;
                        responseOut.status = ActionStatus.Success;
                    }
                    else
                    {
                        responseOut.status = ActionStatus.Fail;
                        responseOut.message = ActionMessage.ProductBOMCopyFail;
                    }
                }
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

        #region SLType
        public ResponseOut AddEditSLType(SLType sLType)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.SLTypes.Any(x => x.SLTypeName == sLType.SLTypeName && x.SLTypeId != sLType.SLTypeId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateSLTypeName;
                }
                else
                {
                    if (sLType.SLTypeId == 0)
                    {
                        entities.SLTypes.Add(sLType);
                        responseOut.message = ActionMessage.SLTypeCreatedSuccess;
                    }
                    else
                    {
                        entities.SLTypes.Where(a => a.SLTypeId == sLType.SLTypeId).ToList().ForEach(a =>
                        {
                            a.SLTypeName = sLType.SLTypeName;
                            a.Status = sLType.Status;

                        });
                        responseOut.message = ActionMessage.SLTypeUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;

                }
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
        public List<SLType> GetSLTypeList()
        {
            List<SLType> slTypeList = new List<SLType>();
            try
            {
                var slTypes = entities.SLTypes.Where(x => x.Status == true).Select(s => new
                {
                    SLTypeId = s.SLTypeId,
                    SLTypeName = s.SLTypeName
                }).ToList();
                if (slTypes != null && slTypes.Count > 0)
                {
                    foreach (var item in slTypes)
                    {
                        slTypeList.Add(new SLType { SLTypeId = item.SLTypeId, SLTypeName = item.SLTypeName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return slTypeList;
        }
        #endregion

        #region Services
        public ResponseOut AddEditServices(Service services)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                //if (entities.PaymentModes.Any(x => x.PaymentModeId != paymentMode.PaymentModeId && x.PaymentModeName == paymentMode.PaymentModeName))
                //{
                //    responseOut.status = ActionStatus.Fail;
                //    responseOut.message = ActionMessage.DuplicateStateCode;
                //}
                //else
                if (entities.Services.Any(x => x.ServicesName == services.ServicesName && x.ServicesId != services.ServicesId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateServicesName;
                }
                else
                {
                    if (services.ServicesId == 0)
                    {
                        entities.Services.Add(services);
                        responseOut.message = ActionMessage.ServicesCreatedSuccess;
                    }
                    else
                    {
                        entities.Services.Where(a => a.ServicesId == services.ServicesId).ToList().ForEach(a =>
                        {
                            a.ServicesName = services.ServicesName;
                            a.Status = services.Status;

                        });
                        responseOut.message = ActionMessage.ServicesUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;

                }
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

        #region City
        public ResponseOut AddEditCity(City city)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {

                if (entities.Cities.Any(x => x.CityName == city.CityName && x.StateId == city.StateId && city.CityId == 0))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateCityName;
                }
                else
                {
                    if (city.CityId == 0)
                    {
                        entities.Cities.Add(city);
                        responseOut.message = ActionMessage.CityCreatedSuccess;
                    }
                    else
                    {
                        entities.Cities.Where(a => a.CityId == city.CityId).ToList().ForEach(a =>
                        {
                            a.CityName = city.CityName;
                            a.StateId = city.StateId;
                            a.Population = city.Population;
                            a.Area = city.Area;
                            a.Railway = city.Railway;
                            a.Airport = city.Airport;
                            a.PointOfInterest = city.PointOfInterest;
                            a.Vehicles = city.Vehicles;
                            a.PerDealar = city.PerDealar;
                            a.DealershipsNos = city.DealershipsNos;
                            a.Status = city.Status;

                        });
                        responseOut.message = ActionMessage.CityUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                }
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
        public List<City> GetCityList(int stateId)
        {
            List<City> cityList = new List<City>();
            try
            {
                var cities = entities.Cities.Where(x => x.StateId == stateId).Select(s => new
                {
                    CityId = s.CityId,
                    CityName = s.CityName
                }).ToList();
                if (cities != null && cities.Count > 0)
                {
                    foreach (var item in cities)
                    {
                        cityList.Add(new City { CityId = item.CityId, CityName = item.CityName });

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return cityList;
        }

        #endregion

        #region Employee State Mapping
        public ResponseOut AddEditEmpStateMapping(EmployeeStateMapping employeeStateMapping)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {

                if (entities.EmployeeStateMappings.Any(x => x.EmployeeStateMappingId == employeeStateMapping.EmployeeStateMappingId))
                {
                    entities.EmployeeStateMappings.Where(a => a.EmployeeStateMappingId == employeeStateMapping.EmployeeStateMappingId).ToList().ForEach(a =>
                        {
                            a.EmployeeStateMappingId = employeeStateMapping.EmployeeStateMappingId;
                            a.EmployeeId = employeeStateMapping.EmployeeId;
                            a.StateId = employeeStateMapping.StateId;
                            a.ModifiedBy = employeeStateMapping.CreatedBy;
                            a.ModifiedDate = DateTime.Now;
                            a.Status = employeeStateMapping.Status;
                        });
                }
                else
                {
                    entities.EmployeeStateMappings.Add(employeeStateMapping);
                }
                entities.SaveChanges();
                responseOut.message = ActionMessage.EmployeeStateMappingSuccessful;
                responseOut.status = ActionStatus.Success;
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

        public List<Employee> GetEmployeeAutoCompleteList(string searchTerm, long companyBranchId)
        {
            List<Employee> employeeList = new List<Employee>();
            try
            {
                // && (emp.Division == "Sale" || emp.Division == "Marketing")
                if (companyBranchId != 0)
                {
                    var employees = (from emp in entities.Employees
                                     where ((emp.FirstName.ToLower().Contains(searchTerm.ToLower()) || emp.EmployeeCode.ToLower().Contains(searchTerm.ToLower()) || emp.MobileNo.Contains(searchTerm)) && emp.CompanyBranchId == companyBranchId && emp.Status == true)
                                     select new
                                     {
                                         EmployeeId = emp.EmployeeId,
                                         FirstName = emp.FirstName + " " + emp.LastName,
                                         EmployeeCode = emp.EmployeeCode,
                                         MobileNo = emp.MobileNo,
                                         DepartmentId = emp.DepartmentId,
                                         Designation = emp.DesignationId
                                      


                                     }).ToList();
                    if (employees != null && employees.Count > 0)
                    {
                        foreach (var item in employees)
                        {

                            employeeList.Add(new Employee
                            {
                                EmployeeId = item.EmployeeId,
                                FirstName = item.FirstName,
                                EmployeeCode = item.EmployeeCode,
                                MobileNo = item.MobileNo,
                                DepartmentId = item.DepartmentId,
                                DesignationId = item.Designation
                            });

                        }
                    }
                }

                else
                {
                    var employees = (from emp in entities.Employees
                                     where ((emp.FirstName.ToLower().Contains(searchTerm.ToLower()) || emp.EmployeeCode.ToLower().Contains(searchTerm.ToLower()) || emp.MobileNo.Contains(searchTerm)) && emp.Status == true)
                                     select new
                                     {
                                         EmployeeId = emp.EmployeeId,
                                         FirstName = emp.FirstName + " " + emp.LastName,
                                         EmployeeCode = emp.EmployeeCode,
                                         MobileNo = emp.MobileNo,
                                         DepartmentId = emp.DepartmentId,
                                         Designation = emp.DesignationId

                                     }).ToList();
                    if (employees != null && employees.Count > 0)
                    {
                        foreach (var item in employees)
                        {

                            employeeList.Add(new Employee
                            {
                                EmployeeId = item.EmployeeId,
                                FirstName = item.FirstName,
                                EmployeeCode = item.EmployeeCode,
                                MobileNo = item.MobileNo,
                                DepartmentId = item.DepartmentId,
                                DesignationId = item.Designation
                            });

                        }
                    }

                }


            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return employeeList;
        }
        #endregion

        #region Employee
        public ResponseOut AddEditEmployee(Employee employee, EmployeeReportingInfo employeeReportinginfo, EmployeePayInfo employeePayInfo)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.Employees.Any(x => x.EmployeeCode == employee.EmployeeCode && x.EmployeeId != employee.EmployeeId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateEmployeeCode;
                }
                else if (entities.Employees.Any(x => x.FirstName == employee.FirstName && x.LastName == employee.LastName && x.MobileNo == employee.MobileNo && x.EmployeeId != employee.EmployeeId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateEmployeeDetail;
                }

                else if (entities.Employees.Any(x => x.UANNo == employee.UANNo && x.EmployeeId != employee.EmployeeId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateUANNoDetail;
                }
                else
                {
                    if (employee.EmployeeId == 0)
                    {
                        employee.CreatedDate = DateTime.Now;
                        entities.Employees.Add(employee);
                        entities.SaveChanges();
                        int employeeId = employee.EmployeeId;
                        responseOut.trnId = employeeId;
                        if (employeePayInfo != null && employeePayInfo.BasicPay != 0)
                        {
                            employeePayInfo.EmployeeId = employeeId;
                            entities.EmployeePayInfoes.Add(employeePayInfo);
                            entities.SaveChanges();
                        }
                        if (employeeReportinginfo != null && employeeReportinginfo.ReportingEmployeeId != 0)
                        {
                            employeeReportinginfo.EmployeeId = employeeId;
                            entities.EmployeeReportingInfoes.Add(employeeReportinginfo);
                            entities.SaveChanges();
                        }

                        responseOut.message = ActionMessage.EmployeeCreatedSuccess;
                    }
                    else
                    {
                        employee.Modifiedby = employee.CreatedBy;
                        employee.ModifiedDate = DateTime.Now;
                        entities.Employees.Where(a => a.EmployeeId == employee.EmployeeId).ToList().ForEach(a =>
                        {
                            a.ApplicantId = employee.ApplicantId;
                            a.CompanyBranchId = employee.CompanyBranchId;
                            a.EmployeeCode = employee.EmployeeCode;
                            a.FirstName = employee.FirstName;
                            a.LastName = employee.LastName;
                            a.FatherSpouseName = employee.FatherSpouseName;
                            a.Gender = employee.Gender;
                            a.DateOfBirth = employee.DateOfBirth;
                            a.MaritalStatus = employee.MaritalStatus;
                            a.BloodGroup = employee.BloodGroup;
                            a.Email = employee.Email;
                            a.AlternateEmail = employee.AlternateEmail;
                            a.ContactNo = employee.ContactNo;
                            a.AlternateContactno = employee.AlternateContactno;
                            a.MobileNo = employee.MobileNo;
                            a.CAddress = employee.CAddress;
                            a.CCity = employee.CCity;
                            a.CStateId = employee.CStateId;
                            a.CCountryId = employee.CCountryId;
                            a.CPinCode = employee.CPinCode;
                            a.PAddress = employee.PAddress;
                            a.PCity = employee.PCity;
                            a.PStateId = employee.PStateId;
                            a.PCountryId = employee.PCountryId;
                            a.PPinCode = employee.PPinCode;
                            a.DateOfJoin = employee.DateOfJoin;
                            a.DateOfLeave = employee.DateOfLeave;
                            a.PANNo = employee.PANNo;
                            a.AadharNo = employee.AadharNo;
                            a.BankDetail = employee.BankDetail;
                            a.BankAccountNo = employee.BankAccountNo;
                            a.PFApplicable = employee.PFApplicable;
                            a.PFNo = employee.PFNo;
                            a.ESIApplicable = employee.ESIApplicable;
                            a.ESINo = employee.ESINo;
                            a.CompanyId = employee.CompanyId;
                            a.Division = employee.Division;
                            a.DepartmentId = employee.DepartmentId;
                            a.DesignationId = employee.DesignationId;
                            a.EmploymentType = employee.EmploymentType;
                            a.EmployeeCurrentStatus = employee.EmployeeCurrentStatus;
                            a.EmployeeStatusPeriod = employee.EmployeeStatusPeriod;
                            a.EmployeeStatusStartDate = employee.EmployeeStatusStartDate;
                            a.Modifiedby = employee.Modifiedby;
                            a.ModifiedDate = employee.ModifiedDate;
                            a.UANNo = employee.UANNo;
                            a.Status = employee.Status;

                        });
                        if (employeePayInfo != null && employeePayInfo.BasicPay != 0)
                        {
                            if (entities.EmployeePayInfoes.Any(x => x.EmployeeId == employee.EmployeeId))
                            {
                                entities.EmployeePayInfoes.Where(a => a.EmployeeId == employee.EmployeeId).ToList().ForEach(a =>
                                {
                                    a.OTRate = employeePayInfo.OTRate;
                                    a.BasicPay = employeePayInfo.BasicPay;
                                    a.HRA = employeePayInfo.HRA;
                                    a.ConveyanceAllow = employeePayInfo.ConveyanceAllow;
                                    a.SpecialAllow = employeePayInfo.SpecialAllow;
                                    a.OtherAllow = employeePayInfo.OtherAllow;
                                    a.OtherDeduction = employeePayInfo.OtherDeduction;
                                    a.MedicalAllow = employeePayInfo.MedicalAllow;
                                    a.ChildEduAllow = employeePayInfo.ChildEduAllow;
                                    a.LTA = employeePayInfo.LTA;
                                    a.EmployeePF = employeePayInfo.EmployeePF;
                                    a.EmployeeESI = employeePayInfo.EmployeeESI;
                                    a.EmployerPF = employeePayInfo.EmployerPF;
                                    a.EmployerESI = employeePayInfo.EmployerESI;
                                    a.ProfessinalTax = employeePayInfo.ProfessinalTax;
                                    a.HRAPerc = employeePayInfo.HRAPerc;
                                    a.SpecialAllowPerc = employeePayInfo.SpecialAllowPerc;
                                    a.LTAPerc = employeePayInfo.LTAPerc;
                                    a.OtherAllowPerc = employeePayInfo.OtherAllowPerc;
                                    a.EmployeePFPerc = employeePayInfo.EmployeePFPerc;
                                    a.EmployeeESIPerc = employeePayInfo.EmployeeESIPerc;
                                    a.EmployerPFPerc = employeePayInfo.EmployerPFPerc;
                                    a.EmployerESIPerc = employeePayInfo.EmployerESIPerc;
                                    a.EmployerEPS = employeePayInfo.EmployerEPS;
                                    a.EmployerEPSPerc = employeePayInfo.EmployerEPSPerc;

                                });
                            }
                            else
                            {
                                employeePayInfo.EmployeeId = employee.EmployeeId;
                                entities.EmployeePayInfoes.Add(employeePayInfo);

                            }
                        }

                        if (employeeReportinginfo != null && employeeReportinginfo.ReportingEmployeeId != 0)
                        {
                            if (entities.EmployeeReportingInfoes.Any(x => x.EmployeeId == employee.EmployeeId))
                            {
                                entities.EmployeeReportingInfoes.Where(a => a.EmployeeId == employee.EmployeeId).ToList().ForEach(a =>
                                {
                                    a.ReportingEmployeeId = employeeReportinginfo.ReportingEmployeeId;
                                });
                            }
                            else
                            {
                                employeeReportinginfo.EmployeeId = employee.EmployeeId;
                                entities.EmployeeReportingInfoes.Add(employeeReportinginfo);

                            }
                        }



                        entities.SaveChanges();
                        responseOut.message = ActionMessage.EmployeeUpdatedSuccess;
                        responseOut.trnId = employee.EmployeeId;
                    }

                    responseOut.status = ActionStatus.Success;
                }
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
        public ResponseOut UpdateEmployeePic(Employee employee)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                entities.Employees.Where(a => a.EmployeeId == employee.EmployeeId).ToList().ForEach(a =>
                 {
                     a.EmployeePicPath = employee.EmployeePicPath;
                     a.EmployeePicName = employee.EmployeePicName;
                 });
                entities.SaveChanges();
                responseOut.status = ActionStatus.Success;


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
        public List<Employee> GetEmployeeAutoCompleteList(string searchTerm, int departmentId, int designationId)
        {
            List<Employee> employeeList = new List<Employee>();
            try
            {
                var employees = (from emp in entities.Employees
                                 where ((emp.FirstName.ToLower().Contains(searchTerm.ToLower()) || emp.EmployeeCode.ToLower().Contains(searchTerm.ToLower()) || emp.MobileNo.Contains(searchTerm)) && emp.DepartmentId == (departmentId == 0 ? emp.DepartmentId : departmentId) && emp.DesignationId == (designationId == 0 ? emp.DesignationId : designationId) && emp.Status == true)
                                 select new
                                 {
                                     EmployeeId = emp.EmployeeId,
                                     FirstName = emp.FirstName + " " + emp.LastName,
                                     EmployeeCode = emp.EmployeeCode,
                                     MobileNo = emp.MobileNo,
                                     DepartmentId = emp.DepartmentId,
                                     CompanyBranchId = emp.CompanyBranchId,
                                     DesignationId = emp.DesignationId

                                 }).ToList();
                if (employees != null && employees.Count > 0)
                {
                    foreach (var item in employees)
                    {

                        employeeList.Add(new Employee
                        {
                            EmployeeId = item.EmployeeId,
                            FirstName = item.FirstName,
                            EmployeeCode = item.EmployeeCode,
                            MobileNo = item.MobileNo,
                            DepartmentId = item.DepartmentId,
                            CompanyBranchId = item.CompanyBranchId,
                            DesignationId = item.DesignationId
                        });

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return employeeList;
        }
        public ResponseOut AddEditEmployeeSL(SL sl, string action = "Add")
        {

            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.SLs.Any(x => x.SLCode == sl.SLCode && action == "Add"))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateSLCode;
                }
                else if (entities.SLs.Any(x => x.SLCode == sl.SLCode && action == "Edit"))
                {
                    entities.SLs.Where(a => a.SLCode == sl.SLCode).ToList().ForEach(a =>
                    {
                        a.SLHead = sl.SLHead;
                        a.ModifiedBy = sl.CreatedBy;
                        a.ModifiedDate = DateTime.Now;
                    });
                }
                else
                {
                    sl.CreatedDate = DateTime.Now;
                    entities.SLs.Add(sl);
                }
                entities.SaveChanges();
                responseOut.status = ActionStatus.Success;
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


        public ResponseOut ImportEmployee(Employee employee)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.Employees.Any(x => x.EmployeeCode == employee.EmployeeCode && x.EmployeeId != employee.EmployeeId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateEmployeeCode;
                }
                else if (entities.Employees.Any(x => x.FirstName == employee.FirstName && x.LastName == employee.LastName && x.MobileNo == employee.MobileNo && x.EmployeeId != employee.EmployeeId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateEmployeeDetail;
                }
                else
                {
                    if (employee.EmployeeId == 0)
                    {
                        employee.CreatedDate = DateTime.Now;
                        entities.Employees.Add(employee);
                        entities.SaveChanges();
                        responseOut.message = ActionMessage.EmployeeCreatedSuccess;
                    }
                    else
                    {
                        employee.Modifiedby = employee.CreatedBy;
                        employee.ModifiedDate = DateTime.Now;
                        entities.Employees.Where(a => a.EmployeeId == employee.EmployeeId).ToList().ForEach(a =>
                        {
                            a.EmployeeCode = employee.EmployeeCode;
                            a.FirstName = employee.FirstName;
                            a.LastName = employee.LastName;
                            a.FatherSpouseName = employee.FatherSpouseName;
                            a.Gender = employee.Gender;
                            a.DateOfBirth = employee.DateOfBirth;
                            a.MaritalStatus = employee.MaritalStatus;
                            a.BloodGroup = employee.BloodGroup;
                            a.Email = employee.Email;
                            a.AlternateEmail = employee.AlternateEmail;
                            a.ContactNo = employee.ContactNo;
                            a.AlternateContactno = employee.AlternateContactno;
                            a.MobileNo = employee.MobileNo;
                            a.CAddress = employee.CAddress;
                            a.CCity = employee.CCity;
                            a.CStateId = employee.CStateId;
                            a.CCountryId = employee.CCountryId;
                            a.CompanyBranchId = employee.CompanyBranchId;
                            a.CPinCode = employee.CPinCode;
                            a.PAddress = employee.PAddress;
                            a.PCity = employee.PCity;
                            a.PStateId = employee.PStateId;
                            a.PCountryId = employee.PCountryId;
                            a.PPinCode = employee.PPinCode;
                            a.DateOfJoin = employee.DateOfJoin;
                            a.DateOfLeave = employee.DateOfLeave;
                            a.PANNo = employee.PANNo;
                            a.AadharNo = employee.AadharNo;
                            a.BankDetail = employee.BankDetail;
                            a.BankAccountNo = employee.BankAccountNo;
                            a.PFApplicable = employee.PFApplicable;
                            a.PFNo = employee.PFNo;
                            a.ESIApplicable = employee.ESIApplicable;
                            a.ESINo = employee.ESINo;
                            a.CompanyId = employee.CompanyId;
                            a.Division = employee.Division;
                            a.DepartmentId = employee.DepartmentId;
                            a.DesignationId = employee.DesignationId;
                            a.EmploymentType = employee.EmploymentType;
                            a.EmployeeCurrentStatus = employee.EmployeeCurrentStatus;
                            a.EmployeeStatusPeriod = employee.EmployeeStatusPeriod;
                            a.EmployeeStatusStartDate = employee.EmployeeStatusStartDate;
                            a.EmployeePicName = employee.EmployeePicName;
                            a.EmployeePicPath = employee.EmployeePicPath;
                            a.Modifiedby = employee.Modifiedby;
                            a.ModifiedDate = employee.ModifiedDate;
                            a.Status = employee.Status;

                        });

                        entities.SaveChanges();
                        responseOut.message = ActionMessage.EmployeeUpdatedSuccess;
                    }

                    responseOut.status = ActionStatus.Success;
                }
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


        public ResponseOut RemoveImageEmployee(long employeeId)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                entities.Employees.Where(a => a.EmployeeId == employeeId).ToList().ForEach(a =>
                {
                    a.EmployeePicName = null;
                    a.EmployeePicPath = null;
                });
                responseOut.message = ActionMessage.EmployeeUpdatedSuccess;
                entities.SaveChanges();
                responseOut.status = ActionStatus.Success;

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

        #region Tax
        public ResponseOut AddEditTax(Tax tax)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                //if (entities.Taxes.Any(x => x.TaxName == tax.TaxName && x.CompanyId == tax.CompanyId && x.TaxId != tax.TaxId))
                //{
                //  responseOut.status = ActionStatus.Fail;
                //     responseOut.message = ActionMessage.DuplicateTaxName;
                //  }


                if (tax.TaxId == 0)
                {
                    tax.CreatedDate = DateTime.Now;
                    entities.Taxes.Add(tax);
                    responseOut.message = ActionMessage.TaxCreatedSuccess;
                }
                else
                {
                    tax.ModifiedBy = tax.CreatedBy;
                    tax.ModifiedDate = DateTime.Now;

                    entities.Taxes.Where(a => a.TaxId == tax.TaxId).ToList().ForEach(a =>
                    {
                        a.TaxId = tax.TaxId;
                        a.CompanyId = tax.CompanyId;
                        a.TaxName = tax.TaxName;
                        a.TaxType = tax.TaxType;
                        a.TaxSubType = tax.TaxSubType;
                        a.TaxGLId = tax.TaxGLId;
                        a.TaxSLId = tax.TaxSLId;
                        a.TaxGLCode = tax.TaxGLCode;
                        a.TaxSLCode = tax.TaxSLCode;
                        a.FormTypeId = tax.FormTypeId;
                        a.WithOutCFormTaxPercentae = tax.WithOutCFormTaxPercentae;
                        a.CFormApplicable = tax.CFormApplicable;
                        a.TaxPercentage = tax.TaxPercentage;
                        a.ModifiedBy = tax.ModifiedBy;
                        a.ModifiedDate = tax.ModifiedDate;
                        a.Status = tax.Status;
                    });
                    responseOut.message = ActionMessage.TaxUpdatedSuccess;
                }
                entities.SaveChanges();
                responseOut.status = ActionStatus.Success;


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

        public List<Tax> GetTaxAutoCompleteList(string searchTerm, string taxSubType, int companyId)
        {
            List<Tax> taxList = new List<Tax>();
            try
            {
                var taxes = (from tax in entities.Taxes
                             where (tax.TaxName.ToLower().Contains(searchTerm.ToLower()) && tax.CompanyId == companyId && tax.TaxSubType.ToUpper() == taxSubType.ToUpper() && tax.Status == true)
                             select new
                             {
                                 TaxId = tax.TaxId,
                                 TaxName = tax.TaxName,
                                 TaxPercentage = tax.TaxPercentage,
                                 SurchargeName_1 = tax.SurchargeName_1,
                                 SurchargePercentage_1 = tax.SurchargePercentage_1,
                                 SurchargeName_2 = tax.SurchargeName_2,
                                 SurchargePercentage_2 = tax.SurchargePercentage_2,
                                 SurchargeName_3 = tax.SurchargeName_3,
                                 SurchargePercentage_3 = tax.SurchargePercentage_3
                             }).ToList();
                if (taxes != null && taxes.Count > 0)
                {
                    foreach (var item in taxes)
                    {

                        taxList.Add(new Tax
                        {
                            TaxId = item.TaxId,
                            TaxName = item.TaxName,
                            TaxPercentage = item.TaxPercentage,
                            SurchargeName_1 = item.SurchargeName_1,
                            SurchargePercentage_1 = item.SurchargePercentage_1,
                            SurchargeName_2 = item.SurchargeName_2,
                            SurchargePercentage_2 = item.SurchargePercentage_2,
                            SurchargeName_3 = item.SurchargeName_3,
                            SurchargePercentage_3 = item.SurchargePercentage_3
                        });

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return taxList;
        }


        #endregion


        #region Additional Tax
        public ResponseOut AddEditAdditionalTax(AdditionalTax addtionaltax)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.AdditionalTaxes.Any(x => x.AddTaxName == addtionaltax.AddTaxName && x.CompanyId == addtionaltax.CompanyId && x.AddTaxId != addtionaltax.AddTaxId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateTaxName;
                }
                else
                {
                    if (addtionaltax.AddTaxId == 0)
                    {
                        entities.AdditionalTaxes.Add(addtionaltax);
                        responseOut.message = ActionMessage.TaxCreatedSuccess;
                    }
                    else
                    {

                        entities.AdditionalTaxes.Where(a => a.AddTaxId == addtionaltax.AddTaxId).ToList().ForEach(a =>
                        {
                            a.AddTaxId = addtionaltax.AddTaxId;
                            a.CompanyId = addtionaltax.CompanyId;
                            a.AddTaxName = addtionaltax.AddTaxName;
                            a.GLId = addtionaltax.GLId;
                            a.SLId = addtionaltax.SLId;
                            a.GLCode = addtionaltax.GLCode;
                            a.SLCode = addtionaltax.SLCode;

                            a.Status = addtionaltax.Status;
                        });
                        responseOut.message = ActionMessage.TaxUpdatedSuccess;
                    }
                }
                entities.SaveChanges();
                responseOut.status = ActionStatus.Success;


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

        public List<Tax> GetTaxAutoCompleteList(string searchTerm, int companyId)
        {
            List<Tax> taxList = new List<Tax>();
            try
            {
                var taxes = (from tax in entities.Taxes
                             where (tax.TaxName.ToLower().Contains(searchTerm.ToLower()) && tax.CompanyId == companyId && tax.Status == true)
                             select new
                             {
                                 TaxId = tax.TaxId,
                                 TaxName = tax.TaxName,
                                 TaxPercentage = tax.TaxPercentage
                             }).ToList();
                if (taxes != null && taxes.Count > 0)
                {
                    foreach (var item in taxes)
                    {

                        taxList.Add(new Tax
                        {
                            TaxId = item.TaxId,
                            TaxName = item.TaxName,
                            TaxPercentage = item.TaxPercentage
                        });

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return taxList;
        }


        #endregion


        #region Customer
        public ResponseOut AddEditCustomer(Customer customer, out int customerId)
        {

            ResponseOut responseOut = new ResponseOut();
            try
            {
                customerId = customer.CustomerId;
                if (entities.Customers.Any(x => x.CustomerCode == customer.CustomerCode && x.CustomerId != customer.CustomerId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateCustomerCode;
                }
                else if (entities.Customers.Any(x => x.CustomerName == customer.CustomerName && x.MobileNo == customer.MobileNo && x.CustomerTypeId == customer.CustomerTypeId && x.CustomerId != customer.CustomerId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateCustomerDetail;
                }
                else
                {
                    if (customer.CustomerId == 0)
                    {
                        customer.CreatedDate = DateTime.Now;
                        entities.Customers.Add(customer);
                        responseOut.message = ActionMessage.CustomerCreatedSuccess;
                    }
                    else
                    {
                        customer.Modifiedby = customer.CreatedBy;
                        customer.ModifiedDate = DateTime.Now;
                        entities.Customers.Where(a => a.CustomerId == customer.CustomerId).ToList().ForEach(a =>
                        {
                            a.CustomerCode = customer.CustomerCode;
                            a.CustomerName = customer.CustomerName;
                            a.ContactPersonName = customer.ContactPersonName;
                            a.Designation = customer.Designation;
                            a.Email = customer.Email;
                            a.MobileNo = customer.MobileNo;
                            a.ContactNo = customer.ContactNo;
                            a.Fax = customer.Fax;
                            a.PrimaryAddress = customer.PrimaryAddress;
                            a.City = customer.City;
                            a.StateId = customer.StateId;
                            a.CountryId = customer.CountryId;
                            a.PinCode = customer.PinCode;
                            a.CSTNo = customer.CSTNo;
                            a.TINNo = customer.TINNo;
                            a.PANNo = customer.PANNo;
                            a.GSTNo = customer.GSTNo;
                            a.ExciseNo = customer.ExciseNo;
                            a.EmployeeId = customer.EmployeeId;
                            a.CustomerTypeId = customer.CustomerTypeId;
                            a.CreditDays = customer.CreditDays;
                            a.CreditLimit = customer.CreditLimit;
                            a.CompanyId = customer.CompanyId;
                            a.Modifiedby = customer.Modifiedby;
                            a.ModifiedDate = customer.ModifiedDate;
                            a.Status = customer.Status;
                            a.AnnualTurnover = customer.AnnualTurnover;
                            a.GST_Exempt = customer.GST_Exempt;
                            a.IsComposition = customer.IsComposition;
                            a.IsUIN = customer.IsUIN;
                            a.UINNo = customer.UINNo;
                            a.CustomerId = customer.CustomerId;
                            a.SaleEmpId = customer.SaleEmpId;
                        });
                        responseOut.message = ActionMessage.CustomerUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    customerId = customer.CustomerId;
                    responseOut.status = ActionStatus.Success;
                }
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
        public ResponseOut AddEditCustomerBranch(CustomerBranch customerBranch)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (customerBranch.CustomerBranchId == 0)
                {
                    entities.CustomerBranches.Add(customerBranch);
                }
                else
                {
                    entities.CustomerBranches.Where(a => a.CustomerBranchId == customerBranch.CustomerBranchId).ToList().ForEach(a =>
                    {
                        a.BranchName = customerBranch.BranchName;
                        a.ContactPersonName = customerBranch.ContactPersonName;
                        a.Designation = customerBranch.Designation;
                        a.Email = customerBranch.Email;
                        a.MobileNo = customerBranch.MobileNo;
                        a.ContactNo = customerBranch.ContactNo;
                        a.Fax = customerBranch.Fax;
                        a.PrimaryAddress = customerBranch.PrimaryAddress;
                        a.City = customerBranch.City;
                        a.StateId = customerBranch.StateId;
                        a.CountryId = customerBranch.CountryId;
                        a.PinCode = customerBranch.PinCode;
                        a.CSTNo = customerBranch.CSTNo;
                        a.TINNo = customerBranch.TINNo;
                        a.PANNo = customerBranch.PANNo;
                        a.AnnualTurnover = customerBranch.AnnualTurnover;
                        a.GSTNo = customerBranch.GSTNo;
                    });

                }
                entities.SaveChanges();
                responseOut.status = ActionStatus.Success;

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



        public ResponseOut AddEditCustomerProduct(CustomerProductMapping customerProduct)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (customerProduct.MappingId == 0)
                {
                    entities.CustomerProductMappings.Add(customerProduct);
                }
                else
                {
                    entities.CustomerProductMappings.Where(a => a.MappingId == customerProduct.MappingId).ToList().ForEach(a =>
                    {
                        a.ProductId = customerProduct.ProductId;
                    });

                }
                entities.SaveChanges();
                responseOut.status = ActionStatus.Success;

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
                if (customerBranchId == 0)
                {

                }
                else
                {
                    entities.CustomerBranches.Where(a => a.CustomerBranchId == customerBranchId).ToList().ForEach(a =>
                    {
                        a.Status = false;

                    });
                    entities.SaveChanges();
                }

                responseOut.status = ActionStatus.Success;
                responseOut.message = ActionMessage.CustomerBranchRemovedSuccess;

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
                if (mappingId == 0)
                {

                }
                else
                {
                    entities.CustomerProductMappings.Where(a => a.MappingId == mappingId).ToList().ForEach(a =>
                    {
                        a.Status = false;

                    });
                    entities.SaveChanges();
                }

                responseOut.status = ActionStatus.Success;
                responseOut.message = ActionMessage.CustomerProductRemovedSuccess;

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
                if (mappingId == 0)
                {

                }
                else
                {
                    entities.VendorProductMappings.Where(a => a.MappingId == mappingId).ToList().ForEach(a =>
                    {
                        a.Status = false;

                    });
                    entities.SaveChanges();
                }

                responseOut.status = ActionStatus.Success;
                responseOut.message = ActionMessage.VendorProductRemovedSuccess;

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

        public List<Customer> GetCustomerAutoCompleteList(string searchTerm, int companyId)
        {
            List<Customer> customerList = new List<Customer>();
            try
            {
                var customers = (from cust in entities.Customers
                                 where ((cust.CustomerName.ToLower().Contains(searchTerm.ToLower()) || cust.CustomerCode.ToLower().Contains(searchTerm.ToLower())) && cust.CompanyId == companyId && cust.Status == true)
                                 select new
                                 {
                                     CustomerId = cust.CustomerId,
                                     CustomerName = cust.CustomerName,
                                     CustomerCode = cust.CustomerCode,
                                     PrimaryAddress = cust.PrimaryAddress,
                                     GSTNo = cust.GSTNo,
                                     PANNo = cust.PANNo
                                 }).ToList();
                if (customers != null && customers.Count > 0)
                {
                    foreach (var item in customers)
                    {

                        customerList.Add(new Customer
                        {
                            CustomerId = item.CustomerId,
                            CustomerName = item.CustomerName,
                            CustomerCode = item.CustomerCode,
                            PrimaryAddress = item.PrimaryAddress,
                            GSTNo = item.GSTNo,
                            PANNo = item.PANNo

                        });

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return customerList;
        }
        public List<CustomerBranch> GetCustomerBranchList(int customerId)
        {
            List<CustomerBranch> customerList = new List<CustomerBranch>();
            try
            {
                var customerBranches = entities.CustomerBranches.Where(x => x.CustomerId == customerId && x.Status == true).Select(s => new
                {
                    CustomerBranchId = s.CustomerBranchId,
                    BranchName = s.BranchName
                }).ToList();
                if (customerBranches != null && customerBranches.Count > 0)
                {
                    foreach (var item in customerBranches)
                    {
                        customerList.Add(new CustomerBranch { CustomerBranchId = item.CustomerBranchId, BranchName = item.BranchName });

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return customerList;
        }
        public CustomerBranch GetCustomerBranchDetail(long customerBranchId)
        {
            CustomerBranch customerBranch = new CustomerBranch();
            try
            {
                var customerBranches = entities.CustomerBranches.Where(x => x.CustomerBranchId == customerBranchId).Select(s => new
                {
                    PrimaryAddress = s.PrimaryAddress,
                    City = s.City,
                    StateId = s.StateId,
                    CountryId = s.CountryId,
                    PinCode = s.PinCode,
                    TINNo = s.TINNo,
                    ContactPersonName = s.ContactPersonName,
                    Email = s.Email,
                    MobileNo = s.MobileNo,
                    ContactNo = s.ContactNo,
                    Fax = s.Fax,
                    GSTNo = s.GSTNo
                }).FirstOrDefault();
                if (customerBranches != null)
                {
                    customerBranch = new CustomerBranch
                    {
                        PrimaryAddress = customerBranches.PrimaryAddress,
                        City = customerBranches.City,
                        StateId = customerBranches.StateId,
                        CountryId = customerBranches.CountryId,
                        PinCode = customerBranches.PinCode,
                        TINNo = customerBranches.TINNo,
                        ContactPersonName = customerBranches.ContactPersonName,
                        Email = customerBranches.Email,
                        MobileNo = customerBranches.MobileNo,
                        ContactNo = customerBranches.ContactNo,
                        Fax = customerBranches.Fax,
                        GSTNo = customerBranches.GSTNo
                    };

                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return customerBranch;
        }


        public List<Customer> GetCustomerAutoCompleteForPaymentModeList(string searchTerm, int companyId)
        {
            List<Customer> customerList = new List<Customer>();
            try
            {
                var customers = (from cust in entities.Customers
                                 where ((cust.CustomerName.ToLower().Contains(searchTerm.ToLower()) || cust.CustomerCode.ToLower().Contains(searchTerm.ToLower())) && cust.CompanyId == companyId && cust.Status == true)
                                 select new
                                 {
                                     CustomerId = cust.CustomerId,
                                     CustomerName = cust.CustomerName,
                                 }).ToList();
                if (customers != null && customers.Count > 0)
                {
                    foreach (var item in customers)
                    {

                        customerList.Add(new Customer
                        {
                            CustomerId = item.CustomerId,
                            CustomerName = item.CustomerName,


                        });

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return customerList;
        }

        public ResponseOut AddEditCustomerSL(SL sl, string action = "Add")
        {

            ResponseOut responseOut = new ResponseOut();
            try
            {

                if (entities.SLs.Any(x => x.SLCode == sl.SLCode && action == "Add"))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateSLCode;
                }

                else if (entities.SLs.Any(x => x.SLCode == sl.SLCode && action == "Edit"))
                {
                    sl.ModifiedBy = sl.CreatedBy;
                    sl.ModifiedDate = DateTime.Now;
                    entities.SLs.Where(a => a.SLCode == sl.SLCode).ToList().ForEach(a =>
                     {

                         a.SLHead = sl.SLHead;

                         a.ModifiedBy = sl.ModifiedBy;
                         a.ModifiedDate = sl.ModifiedDate;

                     });
                }
                else
                {
                    sl.CreatedDate = DateTime.Now;
                    entities.SLs.Add(sl);
                }
                entities.SaveChanges();
                responseOut.status = ActionStatus.Success;
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

        public ResponseOut AddEditCustomerFollowUp(CustomerFollowUp customerFollowUp)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (customerFollowUp.CustomerFollowUpId == 0)
                {
                    customerFollowUp.CreatedDate = DateTime.Now;
                    entities.CustomerFollowUps.Add(customerFollowUp);

                }
                else
                {
                    customerFollowUp.ModifiedDate = DateTime.Now;
                    entities.CustomerFollowUps.Where(a => a.CustomerFollowUpId == customerFollowUp.CustomerFollowUpId).ToList().ForEach(a =>
                    {
                        a.FollowUpActivityTypeId = customerFollowUp.FollowUpActivityTypeId;
                        a.FollowUpDueDateTime = customerFollowUp.FollowUpDueDateTime;
                        a.FollowUpReminderDateTime = customerFollowUp.FollowUpReminderDateTime;
                        a.FollowUpRemarks = customerFollowUp.FollowUpRemarks;
                        a.Priority = customerFollowUp.Priority;
                        a.FollowUpStatusId = customerFollowUp.FollowUpStatusId;
                        a.FollowUpStatusReason = customerFollowUp.FollowUpStatusReason;
                        a.FollowUpByUserId = customerFollowUp.FollowUpByUserId;
                        a.CreatedBy = customerFollowUp.CreatedBy;
                        a.CreatedDate = customerFollowUp.CreatedDate;
                        a.ModifiedBy = customerFollowUp.ModifiedBy;
                        a.ModifiedDate = customerFollowUp.ModifiedDate;
                    });
                }
                entities.SaveChanges();
                responseOut.status = ActionStatus.Success;
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
        public ResponseOut AddEditCustomerMaster(Customer customer)
        {

            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.Customers.Any(x => x.CustomerCode == customer.CustomerCode))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateCustomerCode;
                }
                //else if (entities.Customers.Any(x => x.CustomerName == customer.CustomerName && x.MobileNo == customer.MobileNo && x.CustomerTypeId == customer.CustomerTypeId))
                //{
                //    responseOut.status = ActionStatus.Fail;
                //    responseOut.message = ActionMessage.DuplicateCustomerDetail;
                //}
                else
                {
                    customer.CreatedDate = DateTime.Now;
                    entities.Customers.Add(customer);
                    responseOut.message = ActionMessage.CustomerCreatedSuccess;
                    entities.SaveChanges();
                    responseOut.trnId = customer.CustomerId;
                    responseOut.status = ActionStatus.Success;
                }
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

        public ResponseOut AddEditCustomerMasterSL(SL sl, string action = "Add")
        {

            ResponseOut responseOut = new ResponseOut();
            try
            {

                if (entities.SLs.Any(x => x.SLCode == sl.SLCode && action == "Add"))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateSLCode;
                }
                else
                {
                    sl.CreatedDate = DateTime.Now;
                    entities.SLs.Add(sl);
                }
                entities.SaveChanges();
                responseOut.status = ActionStatus.Success;
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

        public List<Customer> GetCustomerDetailsById(int customerId, int companyId)
        {
            List<Customer> customerList = new List<Customer>();
            try
            {
                var customers = entities.Customers.Where(x => x.CustomerId == customerId && x.CompanyId == companyId && x.Status == true)
                               .Select(cust => new
                               {
                                   CustomerId = cust.CustomerId,
                                   CustomerName = cust.CustomerName,
                                   CustomerCode = cust.CustomerCode,
                                   PrimaryAddress = cust.PrimaryAddress
                               }).ToList();

                if (customers != null && customers.Count > 0)
                {
                    foreach (var item in customers)
                    {

                        customerList.Add(new Customer
                        {
                            CustomerId = item.CustomerId,
                            CustomerName = item.CustomerName,
                            CustomerCode = item.CustomerCode,
                            PrimaryAddress = item.PrimaryAddress
                        });

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return customerList;
        }

        public List<Customer> GetCustomerByType(int CustomerTypeId)
        {
            List<Customer> customerList = new List<Customer>();
            try
            {
                var customers = (from cust in entities.Customers
                                 where (cust.CustomerTypeId == CustomerTypeId && cust.Status == true)
                                 select new
                                 {
                                     CustomerId = cust.CustomerId,
                                     CustomerName = cust.CustomerName,
                                     CustomerCode = cust.CustomerCode,
                                     PrimaryAddress = cust.PrimaryAddress,
                                     GSTNo = cust.GSTNo,
                                     PANNo = cust.PANNo
                                 }).ToList();
                if (customers != null && customers.Count > 0)
                {
                    foreach (var item in customers)
                    {

                        customerList.Add(new Customer
                        {
                            CustomerId = item.CustomerId,
                            CustomerName = item.CustomerName,
                            CustomerCode = item.CustomerCode,
                            PrimaryAddress = item.PrimaryAddress,
                            GSTNo = item.GSTNo,
                            PANNo = item.PANNo

                        });

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return customerList;
        }
        #endregion

        #region Customer Type
        public List<CustomerType> GetCustomerTypeList()
        {
            List<CustomerType> customerTypeList = new List<CustomerType>();
            try
            {
                var customerTypes = entities.CustomerTypes.Where(x => x.Status == true).Select(s => new
                {
                    CustomerTypeId = s.CustomerTypeId,
                    CustomerTypeDesc = s.CustomerTypeDesc
                }).ToList();
                if (customerTypes != null && customerTypes.Count > 0)
                {
                    foreach (var item in customerTypes)
                    {
                        customerTypeList.Add(new CustomerType { CustomerTypeId = item.CustomerTypeId, CustomerTypeDesc = item.CustomerTypeDesc });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return customerTypeList;
        }

        public ResponseOut AddEditCustomerType(CustomerType customertype)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.CustomerTypes.Any(x => x.CustomerTypeId != customertype.CustomerTypeId && x.CustomerTypeDesc == customertype.CustomerTypeDesc))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateCustomerTypeDesc;
                }
                else
                {
                    if (customertype.CustomerTypeId == 0)
                    {
                        entities.CustomerTypes.Add(customertype);
                        responseOut.message = ActionMessage.CustomerTypeCreatedSuccess;
                    }
                    else
                    {
                        entities.CustomerTypes.Where(a => a.CustomerTypeId == customertype.CustomerTypeId).ToList().ForEach(a =>
                        {
                            a.CustomerTypeId = customertype.CustomerTypeId;
                            a.CustomerTypeDesc = customertype.CustomerTypeDesc;
                            a.Status = customertype.Status;
                            a.CompanyBranchId = customertype.CompanyBranchId;

                        });
                        responseOut.message = ActionMessage.CustomerTypeUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;

                }
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

        #region Book
        public ResponseOut AddEditBook(Book book)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.Books.Any(x => x.BookName == book.BookName && x.CompanyId == book.CompanyId && x.BookId != book.BookId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateBookName;
                }
                else if (entities.Books.Any(x => x.BookCode == book.BookCode && x.CompanyId == book.CompanyId && x.BookId != book.BookId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateBookCode;
                }
                else if (entities.Books.Any(x => x.GLCode == book.GLCode && x.CompanyId == book.CompanyId && x.BookId != book.BookId && (book.BookType == "B" || book.BookType == "C")))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateBookGLCode;
                }

                else
                {
                    if (book.BookId == 0)
                    {
                        book.CreatedDate = DateTime.Now;
                        entities.Books.Add(book);
                        responseOut.message = ActionMessage.BookCreatedSuccess;
                    }
                    else
                    {
                        book.Modifiedby = book.CreatedBy;
                        book.ModifiedDate = DateTime.Now;

                        entities.Books.Where(a => a.BookId == book.BookId).ToList().ForEach(a =>
                        {
                            a.BookId = book.BookId;
                            a.CompanyId = book.CompanyId;
                            a.BookName = book.BookName;
                            a.BookType = book.BookType;
                            a.BookCode = book.BookCode;
                            a.BankBranch = book.BankBranch;
                            a.BankAccountNo = book.BankAccountNo;
                            a.GLCode = book.GLCode;
                            a.IFSC = book.IFSC;
                            a.OverDraftLimit = book.OverDraftLimit;
                            a.Modifiedby = book.Modifiedby;
                            a.ModifiedDate = book.ModifiedDate;
                            a.CompanyBranchId = book.CompanyBranchId;
                            a.Status = book.Status;
                            a.AdCode = book.AdCode;
                        });
                        responseOut.message = ActionMessage.BookUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                    responseOut.trnId = book.BookId;

                }
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


        public List<Book> GetBookList(int companyId)
        {
            List<Book> bookList = new List<Book>();
            try
            {
                var books = entities.Books.Where(x => x.Status == true && x.CompanyId == companyId && (x.BookType == "C" || x.BookType == "B")).Select(s => new
                {
                    BookId = s.BookId,
                    BookName = s.BookName,
                    BankAccountNo = s.BankAccountNo,
                    BankBranch = s.BankBranch

                }).ToList();
                if (books != null && books.Count > 0)
                {
                    foreach (var item in books)
                    {
                        bookList.Add(new Book { BookId = item.BookId, BookName = item.BookName, BankAccountNo = item.BankAccountNo, BankBranch = item.BankBranch });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return bookList;
        }
        public List<Book> GetBookList(string bookType, int companyId, int companyBranchId)
        {
            List<Book> bookList = new List<Book>();
            try
            {
                if (companyBranchId > 0)
                {
                    var books = entities.Books.Where(x => x.Status == true && x.BookType == bookType && x.CompanyId == companyId && x.CompanyBranchId == companyBranchId).Select(s => new
                    {
                        BookId = s.BookId,
                        BookName = s.BookName,
                        BankAccountNo = s.BankAccountNo,
                        BankBranch = s.BankBranch

                    }).ToList();
                    if (books != null && books.Count > 0)
                    {
                        foreach (var item in books)
                        {
                            bookList.Add(new Book { BookId = item.BookId, BookName = item.BookName, BankAccountNo = item.BankAccountNo, BankBranch = item.BankBranch });
                        }
                    }
                }
                else
                {
                    var books = entities.Books.Where(x => x.Status == true && x.BookType == bookType && x.CompanyId == companyId).Select(s => new
                    {
                        BookId = s.BookId,
                        BookName = s.BookName,
                        BankAccountNo = s.BankAccountNo,
                        BankBranch = s.BankBranch

                    }).ToList();
                    if (books != null && books.Count > 0)
                    {
                        foreach (var item in books)
                        {
                            bookList.Add(new Book { BookId = item.BookId, BookName = item.BookName, BankAccountNo = item.BankAccountNo, BankBranch = item.BankBranch });
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return bookList;
        }

        public List<Book> GetJVBookList(string bookType, int companyId)
        {
            List<Book> bookList = new List<Book>();
            try
            {
                var books = entities.Books.Where(x => x.Status == true && x.BookType == bookType && x.CompanyId == companyId).Select(s => new
                {
                    BookId = s.BookId,
                    BookName = s.BookName,
                    BankAccountNo = s.BankAccountNo,
                    BankBranch = s.BankBranch

                }).ToList();
                if (books != null && books.Count > 0)
                {
                    foreach (var item in books)
                    {
                        bookList.Add(new Book { BookId = item.BookId, BookName = item.BookName, BankAccountNo = item.BankAccountNo, BankBranch = item.BankBranch });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return bookList;
        }
        public List<Book> GetDebitNoteBookList(string bookType, int companyId)
        {
            List<Book> bookList = new List<Book>();
            try
            {
                var books = entities.Books.Where(x => x.Status == true && x.BookType == bookType && x.CompanyId == companyId).Select(s => new
                {
                    BookId = s.BookId,
                    BookName = s.BookName,
                    BankAccountNo = s.BankAccountNo,
                    BankBranch = s.BankBranch

                }).ToList();
                if (books != null && books.Count > 0)
                {
                    foreach (var item in books)
                    {
                        bookList.Add(new Book { BookId = item.BookId, BookName = item.BookName, BankAccountNo = item.BankAccountNo, BankBranch = item.BankBranch });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return bookList;
        }



        public List<Book> GetCreditNoteBookList(string bookType, int companyId)
        {
            List<Book> bookList = new List<Book>();
            try
            {
                var books = entities.Books.Where(x => x.Status == true && x.BookType == bookType && x.CompanyId == companyId).Select(s => new
                {
                    BookId = s.BookId,
                    BookName = s.BookName,
                    BankAccountNo = s.BankAccountNo,
                    BankBranch = s.BankBranch

                }).ToList();
                if (books != null && books.Count > 0)
                {
                    foreach (var item in books)
                    {
                        bookList.Add(new Book { BookId = item.BookId, BookName = item.BookName, BankAccountNo = item.BankAccountNo, BankBranch = item.BankBranch });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return bookList;
        }



        public List<Book> GetBookAutoCompleteList(string searchTerm, string bookType, int companyId, int companyBranchId)
        {
            List<Book> bookList = new List<Book>();
            try
            {
                if (companyBranchId > 0)
                {
                    var books = (from cust in entities.Books
                                 where ((cust.BookName.ToLower().Contains(searchTerm.ToLower()) || cust.BankBranch.ToLower().Contains(searchTerm.ToLower()) || cust.IFSC.ToLower().Contains(searchTerm.ToLower())) && cust.CompanyId == companyId && cust.BookType == bookType && cust.Status == true && cust.CompanyBranchId == companyBranchId)
                                 select new
                                 {
                                     BookId = cust.BookId,
                                     BookCode = cust.BookCode,
                                     BookName = cust.BookName,
                                     BankBranch = cust.BankBranch,
                                     IFSC = cust.IFSC
                                 }).ToList();
                    if (books != null && books.Count > 0)
                    {
                        foreach (var item in books)
                        {

                            bookList.Add(new Book
                            {
                                BookId = item.BookId,
                                BookCode = item.BookCode,
                                BookName = item.BookName,
                                BankBranch = item.BankBranch,
                                IFSC = item.IFSC
                            });
                        }
                    }
                }
                else
                {
                    var books = (from cust in entities.Books
                                 where ((cust.BookName.ToLower().Contains(searchTerm.ToLower()) || cust.BankBranch.ToLower().Contains(searchTerm.ToLower()) || cust.IFSC.ToLower().Contains(searchTerm.ToLower())) && cust.CompanyId == companyId && cust.BookType == bookType && cust.Status == true)
                                 select new
                                 {
                                     BookId = cust.BookId,
                                     BookCode = cust.BookCode,
                                     BookName = cust.BookName,
                                     BankBranch = cust.BankBranch,
                                     IFSC = cust.IFSC
                                 }).ToList();
                    if (books != null && books.Count > 0)
                    {
                        foreach (var item in books)
                        {

                            bookList.Add(new Book
                            {
                                BookId = item.BookId,
                                BookCode = item.BookCode,
                                BookName = item.BookName,
                                BankBranch = item.BankBranch,
                                IFSC = item.IFSC
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return bookList;
        }

        public List<GL> GetBookGLAutoCompleteList(string searchTerm, int companyId)
        {
            List<GL> glList = new List<GL>();
            try
            {
                var gls = (from p in entities.GLs
                           where (p.GLCode.ToLower().Contains(searchTerm.ToLower()) || p.GLHead.ToLower().Contains(searchTerm.ToLower())) && p.IsBookGL == true && p.CompanyId == companyId && p.Status == true
                           select new
                           {
                               GLId = p.GLId,
                               GLHead = p.GLHead,
                               GLCode = p.GLCode,
                               SLTypeId = p.SLTypeId
                           }).ToList();


                if (gls != null && gls.Count > 0)
                {
                    foreach (var item in gls)
                    {
                        glList.Add(new GL { GLId = item.GLId, GLHead = item.GLHead, GLCode = item.GLCode, SLTypeId = item.SLTypeId });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return glList;
        }

        public List<GL> GetAllGLAutoCompleteList(string searchTerm, int companyId)
        {
            List<GL> glList = new List<GL>();
            try
            {
                var gls = (from p in entities.GLs
                           where (p.GLCode.ToLower().Contains(searchTerm.ToLower()) || p.GLHead.ToLower().Contains(searchTerm.ToLower())) && p.CompanyId == companyId
                           select new
                           {
                               GLId = p.GLId,
                               GLHead = p.GLHead,
                               GLCode = p.GLCode,
                               SLTypeId = p.SLTypeId
                           }).ToList();


                if (gls != null && gls.Count > 0)
                {
                    foreach (var item in gls)
                    {
                        glList.Add(new GL { GLId = item.GLId, GLHead = item.GLHead, GLCode = item.GLCode, SLTypeId = item.SLTypeId });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return glList;
        }

        #endregion

        #region Vendor
        public ResponseOut AddEditVendor(Vendor vendor, out int vendorId)
        {

            ResponseOut responseOut = new ResponseOut();
            try
            {
                vendorId = vendor.VendorId;
                if (entities.Vendors.Any(x => x.VendorCode == vendor.VendorCode && x.VendorId != vendor.VendorId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateVendorCode;
                }

                else if (entities.Vendors.Any(x => x.VendorName == vendor.VendorName && x.MobileNo == vendor.MobileNo && x.VendorId != vendor.VendorId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateVendorDetail;
                }
                else
                {
                    if (vendor.VendorId == 0)
                    {
                        vendor.CreatedDate = DateTime.Now;
                        entities.Vendors.Add(vendor);
                        responseOut.message = ActionMessage.VendorCreatedSuccess;
                    }
                    else
                    {
                        vendor.Modifiedby = vendor.CreatedBy;
                        vendor.ModifiedDate = DateTime.Now;
                        entities.Vendors.Where(a => a.VendorId == vendor.VendorId).ToList().ForEach(a =>
                        {
                            a.VendorCode = vendor.VendorCode;
                            a.VendorName = vendor.VendorName;
                            a.ContactPersonName = vendor.ContactPersonName;
                            a.Email = vendor.Email;
                            a.MobileNo = vendor.MobileNo;
                            a.ContactNo = vendor.ContactNo;
                            a.Fax = vendor.Fax;
                            a.Address = vendor.Address;
                            a.City = vendor.City;
                            a.StateId = vendor.StateId;
                            a.CountryId = vendor.CountryId;
                            a.PinCode = vendor.PinCode;
                            a.CSTNo = vendor.CSTNo;
                            a.TINNo = vendor.TINNo;
                            a.PANNo = vendor.PANNo;
                            a.GSTNo = vendor.GSTNo;
                            a.ExciseNo = vendor.ExciseNo;
                            a.AnnualTurnover = vendor.AnnualTurnover;
                            a.CompanyId = vendor.CompanyId;
                            a.CreditDays = vendor.CreditDays;
                            a.CreditLimit = vendor.CreditLimit;
                            a.Modifiedby = vendor.Modifiedby;
                            a.ModifiedDate = vendor.ModifiedDate;
                            a.Status = vendor.Status;
                            a.GST_Exempt = vendor.GST_Exempt;
                            a.IsComposition = vendor.IsComposition;
                            a.CompanyBranchId = vendor.CompanyBranchId;

                        });
                        responseOut.message = ActionMessage.VendorUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    vendorId = vendor.VendorId;
                    responseOut.status = ActionStatus.Success;
                }
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
        public ResponseOut AddEditVendorProduct(VendorProductMapping vendorProduct)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (vendorProduct.MappingId == 0)
                {
                    entities.VendorProductMappings.Add(vendorProduct);
                }
                else
                {
                    entities.VendorProductMappings.Where(a => a.MappingId == vendorProduct.MappingId).ToList().ForEach(a =>
                    {
                        a.ProductId = vendorProduct.ProductId;
                    });

                }
                entities.SaveChanges();
                responseOut.status = ActionStatus.Success;

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
        public List<Vendor> GetVendorAutoCompleteList(string searchTerm, int companyId)
        {
            List<Vendor> vendorList = new List<Vendor>();
            try
            {
                var vendors = (from vend in entities.Vendors
                               where ((vend.VendorName.ToLower().Contains(searchTerm.ToLower()) || vend.VendorCode.ToLower().Contains(searchTerm.ToLower())) && vend.CompanyId == companyId && vend.Status == true)
                               select new
                               {
                                   VendorId = vend.VendorId,
                                   VendorName = vend.VendorName,
                                   VendorCode = vend.VendorCode,
                                   Address = vend.Address

                               }).ToList();
                if (vendors != null && vendors.Count > 0)
                {
                    foreach (var item in vendors)
                    {

                        vendorList.Add(new Vendor
                        {
                            VendorId = item.VendorId,
                            VendorName = item.VendorName,
                            VendorCode = item.VendorCode,
                            Address = item.Address
                        });

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return vendorList;
        }

        public ResponseOut AddEditVendorSL(SL sl, string action = "Add")
        {

            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.SLs.Any(x => x.SLCode == sl.SLCode && action == "Add"))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateSLCode;
                }
                else if (entities.SLs.Any(x => x.SLCode == sl.SLCode && action == "Edit"))
                {
                    entities.SLs.Where(a => a.SLCode == sl.SLCode).ToList().ForEach(a =>
                    {
                        a.SLHead = sl.SLHead;
                        a.ModifiedBy = sl.CreatedBy;
                        a.ModifiedDate = DateTime.Now;
                    });
                }
                else
                {
                    sl.CreatedDate = DateTime.Now;
                    entities.SLs.Add(sl);
                }
                entities.SaveChanges();
                responseOut.status = ActionStatus.Success;
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

        public ResponseOut AddEditVendorMaster(Vendor vendor)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.Vendors.Any(x => x.VendorCode == vendor.VendorCode))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateVendorCode;
                }

                else if (entities.Vendors.Any(x => x.VendorName == vendor.VendorName && x.MobileNo == vendor.MobileNo))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateVendorDetail;
                }
                else
                {
                    vendor.CreatedDate = DateTime.Now;
                    entities.Vendors.Add(vendor);
                    responseOut.message = ActionMessage.VendorCreatedSuccess;
                }
                entities.SaveChanges();
                responseOut.trnId = vendor.VendorId;
                responseOut.status = ActionStatus.Success;
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
        public ResponseOut AddEditVendorMasterSL(SL sl, string action = "Add")
        {

            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.SLs.Any(x => x.SLCode == sl.SLCode && action == "Add"))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateSLCode;
                }
                else
                {
                    sl.CreatedDate = DateTime.Now;
                    entities.SLs.Add(sl);
                }
                entities.SaveChanges();
                responseOut.status = ActionStatus.Success;
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
        public List<Vendor> GetVendorDetailsById(int vendorId, int companyId)
        {
            List<Vendor> vendorList = new List<Vendor>();
            try
            {
                var vendors = entities.Vendors.Where(x => x.VendorId == vendorId && x.CompanyId == companyId && x.Status == true)
                               .Select(vend => new
                               {
                                   VendorId = vend.VendorId,
                                   VendorName = vend.VendorName,
                                   VendorCode = vend.VendorCode,
                                   Address = vend.Address
                               }).ToList();

                if (vendors != null && vendors.Count > 0)
                {
                    foreach (var item in vendors)
                    {

                        vendorList.Add(new Vendor
                        {
                            VendorId = item.VendorId,
                            VendorName = item.VendorName,
                            VendorCode = item.VendorCode,
                            Address = item.Address
                        });

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return vendorList;
        }

        #endregion

        #region GL Main Group 

        public ResponseOut AddEditGLMainGroup(GLMainGroup glmaingroup)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.GLMainGroups.Any(x => x.GLMainGroupName == glmaingroup.GLMainGroupName && x.CompanyId == glmaingroup.CompanyId && x.GLMainGroupId != glmaingroup.GLMainGroupId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateGLMainGroupName;
                }
                else if (entities.GLMainGroups.Any(x => x.GLType == glmaingroup.GLType && x.CompanyId == glmaingroup.CompanyId && x.GLMainGroupName == glmaingroup.GLMainGroupName && x.GLMainGroupId != glmaingroup.GLMainGroupId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateGLType;
                }
                else
                {
                    if (glmaingroup.GLMainGroupId == 0)
                    {
                        glmaingroup.CreatedDate = DateTime.Now;
                        entities.GLMainGroups.Add(glmaingroup);
                        responseOut.message = ActionMessage.GLMainGroupCreatedSuccess;
                    }
                    else
                    {
                        glmaingroup.ModifiedBy = glmaingroup.CreatedBy;
                        glmaingroup.ModifiedDate = DateTime.Now;
                        entities.GLMainGroups.Where(a => a.GLMainGroupId == glmaingroup.GLMainGroupId).ToList().ForEach(a =>
                        {
                            a.GLMainGroupId = glmaingroup.GLMainGroupId;
                            a.GLMainGroupName = glmaingroup.GLMainGroupName;
                            a.GLType = glmaingroup.GLType;
                            a.SequenceNo = glmaingroup.SequenceNo;
                            a.CompanyId = glmaingroup.CompanyId;
                            a.Status = glmaingroup.Status;
                        });
                        responseOut.message = ActionMessage.GLMainGroupUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                }
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

        #region GL Sub Group 
        public List<GLMainGroup> GetGLMainGroupList()
        {
            List<GLMainGroup> glmaingroupList = new List<GLMainGroup>();
            try
            {
                var glmaingroups = entities.GLMainGroups.Where(x => x.Status == true).Select(s => new
                {
                    GLMainGroupId = s.GLMainGroupId,
                    GLMainGroupName = s.GLMainGroupName,
                    GLType = s.GLType,
                    SequenceNo = s.SequenceNo
                }).ToList();
                if (glmaingroups != null && glmaingroups.Count > 0)
                {
                    foreach (var item in glmaingroups)
                    {
                        glmaingroupList.Add(new GLMainGroup { GLMainGroupId = item.GLMainGroupId, GLMainGroupName = item.GLMainGroupName, GLType = item.GLType, SequenceNo = item.SequenceNo });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return glmaingroupList;
        }


        public List<GLMainGroup> GetGLMainGroupTypeList(string glType)
        {
            List<GLMainGroup> glmaingroupList = new List<GLMainGroup>();
            try
            {
                var glmaingroups = entities.GLMainGroups.Where(x => x.Status == true && x.GLType == glType).Select(s => new
                {
                    GLMainGroupId = s.GLMainGroupId,
                    GLMainGroupName = s.GLMainGroupName,
                    GLType = s.GLType,
                    SequenceNo = s.SequenceNo
                }).ToList();
                if (glmaingroups != null && glmaingroups.Count > 0)
                {
                    foreach (var item in glmaingroups)
                    {
                        glmaingroupList.Add(new GLMainGroup { GLMainGroupId = item.GLMainGroupId, GLMainGroupName = item.GLMainGroupName, GLType = item.GLType, SequenceNo = item.SequenceNo });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return glmaingroupList;
        }




        public ResponseOut AddEditGLSubGroup(GLSubGroup glSubgroup)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.GLSubGroups.Any(x => x.GLSubGroupName == glSubgroup.GLSubGroupName && x.CompanyId == glSubgroup.CompanyId && x.GLSubGroupId != glSubgroup.GLSubGroupId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateGLSubGroupName;
                }
                else if (entities.GLSubGroups.Any(x => x.GLMainGroupId == glSubgroup.GLMainGroupId && x.CompanyId == glSubgroup.CompanyId && x.GLSubGroupName == glSubgroup.GLSubGroupName && x.GLMainGroupId != glSubgroup.GLMainGroupId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateGLMainGroupId;
                }
                else
                {
                    if (glSubgroup.GLSubGroupId == 0)
                    {
                        glSubgroup.CreatedDate = DateTime.Now;
                        entities.GLSubGroups.Add(glSubgroup);
                        responseOut.message = ActionMessage.GLSubGroupCreatedSuccess;
                    }
                    else
                    {
                        glSubgroup.ModifiedBy = glSubgroup.CreatedBy;
                        glSubgroup.ModifiedDate = DateTime.Now;
                        entities.GLSubGroups.Where(a => a.GLSubGroupId == glSubgroup.GLSubGroupId).ToList().ForEach(a =>
                        {
                            a.GLSubGroupId = glSubgroup.GLSubGroupId;
                            a.GLSubGroupName = glSubgroup.GLSubGroupName;
                            a.GLMainGroupId = glSubgroup.GLMainGroupId;
                            a.ScheduleId = glSubgroup.ScheduleId;
                            a.SequenceNo = glSubgroup.SequenceNo;
                            a.CompanyId = glSubgroup.CompanyId;
                            a.Status = glSubgroup.Status;
                        });
                        responseOut.message = ActionMessage.GLSubGroupUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                }
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

        #region QuotationSetting

        //public List<User> GetUserAutoCompleteList(string searchTerm)
        //{
        //    List<User> userList = new List<User>();
        //    try
        //    {
        //        var users = (from usr in entities.Users
        //                     where ((usr.UserName.ToLower().Contains(searchTerm.ToLower()) || usr.FullName.ToLower().Contains(searchTerm.ToLower()) || usr.MobileNo.Contains(searchTerm))  && usr.Status == true)
        //                         select new
        //                         {
        //                             UserId = usr.UserId,
        //                             UserName = usr.UserName,
        //                             FullName = usr.FullName,
        //                             MobileNo = usr.MobileNo
        //                         }).ToList();
        //        if (users != null && users.Count > 0)
        //        {
        //            foreach (var item in users)
        //            {

        //                userList.Add(new User
        //                {
        //                    UserId = item.UserId,
        //                    UserName = item.UserName,
        //                    FullName = item.FullName,
        //                    MobileNo = item.MobileNo

        //                });

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
        //        throw ex;
        //    }
        //    return userList;
        //}

        public ResponseOut AddEditQuotationSetting(QuotationSetting quotationsetting)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.QuotationSettings.Any(x => x.Status == true && x.CompanyId == quotationsetting.CompanyId && x.QuotationSettingId != quotationsetting.QuotationSettingId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.QuotationSettingExist;
                }
                else
                {
                    if (quotationsetting.QuotationSettingId == 0)
                    {
                        quotationsetting.CreatedDate = DateTime.Now;
                        entities.QuotationSettings.Add(quotationsetting);
                        responseOut.message = ActionMessage.QuotationSettingCreatedSuccess;
                    }
                    else
                    {
                        quotationsetting.ModifiedBy = quotationsetting.CreatedBy;
                        quotationsetting.ModifiedDate = DateTime.Now;

                        entities.QuotationSettings.Where(a => a.QuotationSettingId == quotationsetting.QuotationSettingId).ToList().ForEach(a =>
                        {
                            a.QuotationSettingId = quotationsetting.QuotationSettingId;
                            a.CompanyId = quotationsetting.CompanyId;
                            a.NormalApprovalRequired = quotationsetting.NormalApprovalRequired;
                            a.NormalApprovalByUserId = quotationsetting.NormalApprovalByUserId;
                            a.RevisedApprovalRequired = quotationsetting.RevisedApprovalRequired;
                            a.RevisedApprovalByUserId = quotationsetting.RevisedApprovalByUserId;
                            a.ModifiedBy = quotationsetting.ModifiedBy;
                            a.ModifiedDate = quotationsetting.ModifiedDate;
                            a.Status = quotationsetting.Status;
                        });
                        responseOut.message = ActionMessage.QuotationSettingUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;


                }
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

        #region Term Template
        public ResponseOut RemoveTermTemplate(long trnId)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (trnId == 0)
                {

                }
                else
                {
                    entities.TermTemplateDetails.Where(a => a.TrnId == trnId).ToList().ForEach(a =>
                    {
                        a.Status = false;

                    });
                    entities.SaveChanges();
                }

                responseOut.status = ActionStatus.Success;
                responseOut.message = ActionMessage.TermTemplateRemovedSuccess;

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


        public ResponseOut AddEditTermTemplate(TermsTemplate termtemplate, out int termtemplateId)
        {

            ResponseOut responseOut = new ResponseOut();
            try
            {
                termtemplateId = termtemplate.TermTemplateId;
                if (entities.TermsTemplates.Any(x => x.TermTempalteName == termtemplate.TermTempalteName && x.TermTemplateId != termtemplate.TermTemplateId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateTermTempalteName;
                }

                else
                {
                    if (termtemplate.TermTemplateId == 0)
                    {
                        termtemplate.CreatedDate = DateTime.Now;
                        entities.TermsTemplates.Add(termtemplate);
                        responseOut.message = ActionMessage.TermTemplateCreatedSuccess;
                    }
                    else
                    {
                        termtemplate.ModifiedBy = termtemplate.CreatedBy;
                        termtemplate.ModifiedDate = DateTime.Now;
                        entities.TermsTemplates.Where(a => a.TermTemplateId == termtemplate.TermTemplateId).ToList().ForEach(a =>
                        {
                            a.TermTempalteName = termtemplate.TermTempalteName;
                            a.CompanyId = termtemplate.CompanyId;
                            a.ModifiedBy = termtemplate.ModifiedBy;
                            a.ModifiedDate = termtemplate.ModifiedDate;
                            a.Status = termtemplate.Status;
                            a.CompanyBranchId = termtemplate.CompanyBranchId;
                        });
                        responseOut.message = ActionMessage.TermTemplateUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    termtemplateId = termtemplate.TermTemplateId;
                    responseOut.status = ActionStatus.Success;
                }
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


        public ResponseOut AddEditTermTemplateDetail(TermTemplateDetail termTemplate)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (termTemplate.TrnId == 0)
                {
                    entities.TermTemplateDetails.Add(termTemplate);
                }
                else
                {
                    entities.TermTemplateDetails.Where(a => a.TrnId == termTemplate.TrnId).ToList().ForEach(a =>
                    {
                        a.TermTemplateId = termTemplate.TermTemplateId;
                        a.TermsDesc = termTemplate.TermsDesc;
                        a.Status = termTemplate.Status;
                    });

                }
                entities.SaveChanges();
                responseOut.status = ActionStatus.Success;

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

        public List<TermsTemplate> GetTermTemplateList(int companyId)
        {
            List<TermsTemplate> termList = new List<TermsTemplate>();
            try
            {
                var terms = entities.TermsTemplates.Where(x => x.Status == true && x.CompanyId == companyId).Select(s => new
                {
                    TermTemplateId = s.TermTemplateId,
                    TermTempalteName = s.TermTempalteName
                }).ToList();
                if (terms != null && terms.Count > 0)
                {
                    foreach (var item in terms)
                    {
                        termList.Add(new TermsTemplate { TermTemplateId = item.TermTemplateId, TermTempalteName = item.TermTempalteName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return termList;
        }
        public List<TermTemplateDetail> GetTermTemplateDetailList(int termTemplateId)
        {
            List<TermTemplateDetail> termDescList = new List<TermTemplateDetail>();
            try
            {
                var termDesc = entities.TermTemplateDetails.Where(x => x.Status == true && x.TermTemplateId == termTemplateId).Select(s => new
                {
                    TrnId = s.TrnId,
                    TermsDesc = s.TermsDesc
                }).ToList();
                if (termDesc != null && termDesc.Count > 0)
                {
                    foreach (var item in termDesc)
                    {
                        termDescList.Add(new TermTemplateDetail { TrnId = item.TrnId, TermsDesc = item.TermsDesc });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return termDescList;
        }
        #endregion

        #region Document Type
        public ResponseOut AddEditDocumentType(DocumentType documenttype)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.DocumentTypes.Any(x => x.DocumentTypeId != documenttype.DocumentTypeId && x.DocumentTypeDesc == documenttype.DocumentTypeDesc && x.CompanyId == documenttype.CompanyId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateDocumentTypeDesc;
                }
                else
                {
                    if (documenttype.DocumentTypeId == 0)
                    {
                        entities.DocumentTypes.Add(documenttype);
                        responseOut.message = ActionMessage.DocumentTypeCreatedSuccess;
                    }
                    else
                    {
                        entities.DocumentTypes.Where(a => a.DocumentTypeId == documenttype.DocumentTypeId).ToList().ForEach(a =>
                        {
                            a.DocumentTypeId = documenttype.DocumentTypeId;
                            a.DocumentTypeDesc = documenttype.DocumentTypeDesc;
                            a.CompanyId = documenttype.CompanyId;
                            a.Status = documenttype.Status;
                            a.CompanyBranchId = documenttype.CompanyBranchId;

                        });
                        responseOut.message = ActionMessage.DocumentTypeUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;

                }
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
        public List<DocumentType> GetDocumentTypeList(int companyId)
        {
            List<DocumentType> documentList = new List<DocumentType>();
            try
            {
                var documents = entities.DocumentTypes.Where(x => x.Status == true && x.CompanyId == companyId).Select(s => new
                {
                    DocumentTypeId = s.DocumentTypeId,
                    DocumentTypeDesc = s.DocumentTypeDesc
                }).ToList();
                if (documents != null && documents.Count > 0)
                {
                    foreach (var item in documents)
                    {
                        documentList.Add(new DocumentType { DocumentTypeId = item.DocumentTypeId, DocumentTypeDesc = item.DocumentTypeDesc });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return documentList;
        }

        #endregion

        #region SO Setting



        public ResponseOut AddEditSOSetting(SOSetting sosetting)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.SOSettings.Any(x => x.Status == true && x.CompanyId == sosetting.CompanyId && x.SOSettingId != sosetting.SOSettingId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.SOSettingExist;
                }
                else
                {
                    if (sosetting.SOSettingId == 0)
                    {
                        sosetting.CreatedDate = DateTime.Now;
                        entities.SOSettings.Add(sosetting);
                        responseOut.message = ActionMessage.SOSettingCreatedSuccess;
                    }
                    else
                    {
                        sosetting.ModifiedBy = sosetting.CreatedBy;
                        sosetting.ModifiedDate = DateTime.Now;

                        entities.SOSettings.Where(a => a.SOSettingId == sosetting.SOSettingId).ToList().ForEach(a =>
                        {
                            a.SOSettingId = sosetting.SOSettingId;
                            a.CompanyId = sosetting.CompanyId;
                            a.NormalApprovalRequired = sosetting.NormalApprovalRequired;
                            a.NormalApprovalByUserId = sosetting.NormalApprovalByUserId;
                            a.RevisedApprovalRequired = sosetting.RevisedApprovalRequired;
                            a.RevisedApprovalByUserId = sosetting.RevisedApprovalByUserId;
                            a.ModifiedBy = sosetting.ModifiedBy;
                            a.ModifiedDate = sosetting.ModifiedDate;
                            a.Status = sosetting.Status;
                        });
                        responseOut.message = ActionMessage.SOSettingUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;


                }
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

        #region Product State Tax Mapping
        public ResponseOut AddEditProductTaxMapping(ProductSubCategoryStateTaxMapping productSubCategoryStateTaxMapping)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {

                if (productSubCategoryStateTaxMapping.MappingId == 0)
                {
                    productSubCategoryStateTaxMapping.CreatedDate = DateTime.Now;
                    entities.ProductSubCategoryStateTaxMappings.Add(productSubCategoryStateTaxMapping);
                    responseOut.message = ActionMessage.ProductStateTaxMappingCreatedSuccessful;
                }
                else
                {
                    entities.ProductSubCategoryStateTaxMappings.Where(a => a.ProductSubGroupId == productSubCategoryStateTaxMapping.ProductSubGroupId).ToList().ForEach(a =>
                    {

                        a.ProductSubGroupId = productSubCategoryStateTaxMapping.ProductSubGroupId;
                        a.StateId = productSubCategoryStateTaxMapping.StateId;
                        a.TaxId = productSubCategoryStateTaxMapping.TaxId;
                        a.CompanyId = productSubCategoryStateTaxMapping.CompanyId;
                        a.Status = productSubCategoryStateTaxMapping.Status;
                        a.CreatedBy = productSubCategoryStateTaxMapping.CreatedBy;
                        a.CreatedDate = DateTime.Now;

                    });
                    responseOut.message = ActionMessage.ProductStateTaxMappingUpdateSuccessful;

                }
                entities.SaveChanges();
                responseOut.status = ActionStatus.Success;
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

        #region FollowUpActivityType
        public ResponseOut AddEditFollowUpActivityType(FollowUpActivityType followUpActivityType)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                //if (entities.PaymentModes.Any(x => x.PaymentModeId != paymentMode.PaymentModeId && x.PaymentModeName == paymentMode.PaymentModeName))
                //{
                //    responseOut.status = ActionStatus.Fail;
                //    responseOut.message = ActionMessage.DuplicateStateCode;
                //}
                //else
                if (entities.FollowUpActivityTypes.Any(x => x.FollowUpActivityTypeName == followUpActivityType.FollowUpActivityTypeName && x.FollowUpActivityTypeId != followUpActivityType.FollowUpActivityTypeId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateFollowUpActivityTypeName;
                }
                else
                {
                    if (followUpActivityType.FollowUpActivityTypeId == 0)
                    {
                        entities.FollowUpActivityTypes.Add(followUpActivityType);
                        responseOut.message = ActionMessage.FollowUpActivityTypeCreatedSuccess;
                    }
                    else
                    {
                        entities.FollowUpActivityTypes.Where(a => a.FollowUpActivityTypeId == followUpActivityType.FollowUpActivityTypeId).ToList().ForEach(a =>
                        {
                            a.FollowUpActivityTypeName = followUpActivityType.FollowUpActivityTypeName;
                            a.Status = followUpActivityType.Status;

                        });
                        responseOut.message = ActionMessage.FollowUpActivityTypeUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;

                }
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

        #region Schedule
        public ResponseOut AddEditSchedule(Schedule schedule)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                //if (entities.Schedules.Any(x => x.ScheduleName == schedule.ScheduleName && x.CompanyId == schedule.CompanyId && x.ScheduleId != schedule.ScheduleId))
                //{
                //    responseOut.status = ActionStatus.Fail;
                //    responseOut.message = ActionMessage.DuplicateScheduleName;
                //}
                if (entities.Schedules.Any(x => x.ScheduleNo == schedule.ScheduleNo && x.CompanyId == schedule.CompanyId && x.ScheduleId != schedule.ScheduleId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateScheduleNo;
                }
                else if (entities.Schedules.Any(x => x.CompanyId == schedule.CompanyId && x.ScheduleName == schedule.ScheduleName && x.ScheduleId != schedule.ScheduleId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateScheduleName;
                }
                else
                {
                    if (schedule.ScheduleId == 0)
                    {
                        schedule.CreatedDate = DateTime.Now;
                        entities.Schedules.Add(schedule);
                        responseOut.message = ActionMessage.ScheduleCreatedSuccess;
                    }
                    else
                    {
                        schedule.ModifiedBy = schedule.CreatedBy;
                        schedule.ModifiedDate = DateTime.Now;
                        entities.Schedules.Where(a => a.ScheduleId == schedule.ScheduleId).ToList().ForEach(a =>
                        {
                            a.ScheduleId = schedule.ScheduleId;
                            a.ScheduleName = schedule.ScheduleName;
                            a.ScheduleNo = schedule.ScheduleNo;
                            a.CompanyId = schedule.CompanyId;
                            a.Status = schedule.Status;
                        });
                        responseOut.message = ActionMessage.ScheduleUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                }
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
        public List<Schedule> GetScheduleList()
        {
            // List<GLMainGroup> glmaingroupList = new List<GLMainGroup>();
            List<Schedule> scheduleList = new List<Schedule>();
            try
            {
                var schedule = entities.Schedules.Where(x => x.Status == true).Select(s => new
                {
                    ScheduleId = s.ScheduleId,
                    ScheduleName = s.ScheduleName,
                    //GLType = s.GLType,
                    //SequenceNo = s.SequenceNo
                }).ToList();
                if (schedule != null && schedule.Count > 0)
                {
                    foreach (var item in schedule)
                    {
                        scheduleList.Add(new Schedule { ScheduleId = item.ScheduleId, ScheduleName = item.ScheduleName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return scheduleList;
        }
        public List<Schedule> GetGLScheduleList()
        {
            List<Schedule> scheduleList = new List<Schedule>();
            try
            {
                var Schedules = entities.Schedules.Where(x => x.Status == true).Select(s => new
                {
                    ScheduleId = s.ScheduleId,
                    ScheduleName = s.ScheduleName
                }).ToList();
                if (Schedules != null && Schedules.Count > 0)
                {
                    foreach (var item in Schedules)
                    {
                        scheduleList.Add(new Schedule { ScheduleId = item.ScheduleId, ScheduleName = item.ScheduleName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return scheduleList;
        }
        #endregion

        #region Form Type
        public ResponseOut AddEditFormType(FormType formType)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.FormTypes.Any(x => x.FormTypeDesc == formType.FormTypeDesc && x.CompanyId == formType.CompanyId && x.FormTypeId != formType.FormTypeId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateFormType;
                }
                else if (entities.FormTypes.Any(x => x.CompanyId == formType.CompanyId && x.FormTypeDesc == formType.FormTypeDesc && x.FormTypeId != formType.FormTypeId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateFormType;
                }
                else
                {
                    if (formType.FormTypeId == 0)
                    {

                        entities.FormTypes.Add(formType);
                        responseOut.message = ActionMessage.FormTypeCreatedSuccess;
                    }
                    else
                    {

                        entities.FormTypes.Where(a => a.FormTypeId == formType.FormTypeId).ToList().ForEach(a =>
                        {
                            a.FormTypeId = formType.FormTypeId;
                            a.FormTypeDesc = formType.FormTypeDesc;
                            a.CompanyId = formType.CompanyId;
                            a.Status = formType.Status;
                        });
                        responseOut.message = ActionMessage.FormTypeUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                }
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

        #region Change Password
        public ResponseOut VerifyOldPassword(int userId, string oldPassword)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.Users.Any(x => x.UserId == userId && x.Password != oldPassword))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.PasswordDoesNotMatch;
                }

                else
                {
                    responseOut.status = ActionStatus.Success;
                }
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
        public ResponseOut ChangePassword(User user)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {

                user.ModifiedBy = user.CreatedBy;
                user.ModifiedDate = DateTime.Now;

                entities.Users.Where(a => a.UserId == user.UserId).ToList().ForEach(a =>
                {
                    a.Password = user.Password;
                    a.ModifiedBy = user.ModifiedBy;
                    a.ModifiedDate = user.ModifiedDate;

                });
                responseOut.message = ActionMessage.PasswordChangeSuccessfully;

                entities.SaveChanges();
                responseOut.status = ActionStatus.Success;


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

        #region UserEmailSetting
        public ResponseOut AddEditUserEmailSetting(UserEmailSetting userEmailSetting)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.UserEmailSettings.Any(x => x.SmtpUser == userEmailSetting.SmtpUser && x.SettingId != userEmailSetting.SettingId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateUserEmailSetting;
                }

                else
                {
                    if (userEmailSetting.SettingId == 0)
                    {
                        userEmailSetting.CreatedDate = DateTime.Now;
                        entities.UserEmailSettings.Add(userEmailSetting);
                        responseOut.message = ActionMessage.UserEmailSettingCreatedSuccess;
                    }
                    else
                    {
                        userEmailSetting.ModifiedBy = userEmailSetting.CreatedBy;
                        userEmailSetting.ModifiedDate = DateTime.Now;

                        entities.UserEmailSettings.Where(a => a.SettingId == userEmailSetting.SettingId).ToList().ForEach(a =>
                        {
                            a.SmtpUser = userEmailSetting.SmtpUser;
                            a.SmtpDisplayName = userEmailSetting.SmtpDisplayName;
                            a.SmtpPass = userEmailSetting.SmtpPass;
                            a.SmtpServer = userEmailSetting.SmtpServer;
                            a.EnableSsl = userEmailSetting.EnableSsl;
                            a.SmtpPort = userEmailSetting.SmtpPort;
                            a.SmtpDisplayName = userEmailSetting.SmtpDisplayName;
                            a.Status = userEmailSetting.Status;
                            a.CompanyBranchId = userEmailSetting.CompanyBranchId;
                        });
                        responseOut.message = ActionMessage.UserEmailSettingUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;

                }
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
        public List<User> GetUserEmailAutoCompleteList(string searchTerm)
        {
            List<User> userList = new List<User>();
            try
            {
                var allusers = (from user in entities.Users
                                where ((user.FullName.ToLower().Contains(searchTerm.ToLower()) || user.UserName.ToLower().Contains(searchTerm.ToLower()) || user.MobileNo.Contains(searchTerm)) && user.Status == true)
                                select new
                                {

                                    FullName = user.FullName,
                                    UserName = user.UserName,
                                    UserId = user.UserId,
                                    MobileNo = user.MobileNo,
                                    Email = user.Email

                                }).ToList();
                if (allusers != null && allusers.Count > 0)
                {
                    foreach (var item in allusers)
                    {

                        userList.Add(new User
                        {

                            FullName = item.FullName,
                            UserName = item.UserName,
                            UserId = item.UserId,
                            MobileNo = item.MobileNo,
                            Email = item.Email

                        });

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return userList;
        }
        #endregion

        #region VendorForm
        public ResponseOut AddEditVendorForm(VendorForm vendorForm)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (vendorForm.VendorFormTrnId == 0)
                {
                    vendorForm.CreatedDate = DateTime.Now;
                    entities.VendorForms.Add(vendorForm);
                    responseOut.message = ActionMessage.VendorFormCreatedSuccess;

                }
                else
                {
                    entities.VendorForms.Where(a => a.VendorFormTrnId == vendorForm.VendorFormTrnId).ToList().ForEach(a =>
                    {
                        a.VendorId = vendorForm.VendorId;
                        a.InvoiceId = vendorForm.InvoiceId;
                        a.FormTypeId = vendorForm.FormTypeId;
                        a.RefNo = vendorForm.RefNo;
                        a.RefDate = vendorForm.RefDate;
                        a.Amount = vendorForm.Amount;
                        a.Remarks = vendorForm.Remarks;
                        a.CreatedBy = vendorForm.CreatedBy;
                        a.FormStatus = vendorForm.FormStatus;
                        a.CompanyId = vendorForm.CompanyId;
                        a.ModifiedBy = vendorForm.CreatedBy;
                        a.ModifiedDate = DateTime.Now;
                    });
                    responseOut.message = ActionMessage.VendorUpdatedSuccess;
                }
                entities.SaveChanges();
                responseOut.status = ActionStatus.Success;

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

        #region GL
        public List<GLSubGroup> GetGLSubGroupList(int mainGroupId)
        {
            List<GLSubGroup> glsubgroupList = new List<GLSubGroup>();
            try
            {
                var glsubgroup = entities.GLSubGroups.Where(x => x.GLMainGroupId == mainGroupId).Select(s => new
                {
                    GLSubGroupId = s.GLSubGroupId,
                    GLSubGroupName = s.GLSubGroupName

                }).ToList();

                if (glsubgroup != null && glsubgroup.Count > 0)
                {
                    foreach (var item in glsubgroup)
                    {
                        glsubgroupList.Add(new GLSubGroup { GLSubGroupId = item.GLSubGroupId, GLSubGroupName = item.GLSubGroupName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return glsubgroupList;
        }

        public List<GL> GetGLAutoCompleteList(string searchTerm, int companyId)
        {
            List<GL> glList = new List<GL>();
            try
            {
                var gls = (from p in entities.GLs
                           where (p.GLCode.ToLower().Contains(searchTerm.ToLower()) || p.GLHead.ToLower().Contains(searchTerm.ToLower())) && p.CompanyId == companyId
                           select new
                           {
                               GLId = p.GLId,
                               GLHead = p.GLHead,
                               GLCode = p.GLCode,
                               SLTypeId = p.SLTypeId
                           }).ToList();


                if (gls != null && gls.Count > 0)
                {
                    foreach (var item in gls)
                    {
                        glList.Add(new GL { GLId = item.GLId, GLHead = item.GLHead, GLCode = item.GLCode, SLTypeId = item.SLTypeId });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return glList;
        }

        public List<GL> GetPostingGLAutoCompleteList(string searchTerm, int slTypeId, int companyId)
        {
            List<GL> glList = new List<GL>();
            try
            {
                var gls = (from p in entities.GLs
                           where (p.GLCode.ToLower().Contains(searchTerm.ToLower()) || p.GLHead.ToLower().Contains(searchTerm.ToLower())) && p.CompanyId == companyId && p.Status == true && p.IsPostGL == true && p.SLTypeId == slTypeId
                           select new
                           {
                               GLId = p.GLId,
                               GLHead = p.GLHead,
                               GLCode = p.GLCode,
                           }).ToList();


                if (gls != null && gls.Count > 0)
                {
                    foreach (var item in gls)
                    {
                        glList.Add(new GL { GLId = item.GLId, GLHead = item.GLHead, GLCode = item.GLCode });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return glList;
        }



        public List<GL> GetGLAutoCompleteListsForTax(string searchTerm, int companyId)
        {
            List<GL> glList = new List<GL>();
            try
            {
                var gls = (from p in entities.GLs
                           where (p.GLCode.ToLower().Contains(searchTerm.ToLower()) || p.GLHead.ToLower().Contains(searchTerm.ToLower())) && p.CompanyId == companyId && p.Status == true && p.IsTaxGL == true && p.SLTypeId == 5
                           select new
                           {
                               GLId = p.GLId,
                               GLHead = p.GLHead,
                               GLCode = p.GLCode,
                           }).ToList();


                if (gls != null && gls.Count > 0)
                {
                    foreach (var item in gls)
                    {
                        glList.Add(new GL { GLId = item.GLId, GLHead = item.GLHead, GLCode = item.GLCode });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return glList;
        }

        public List<GL> GetGLAutoCompleteListForProductGLMapping(string searchTerm, int companyId)
        {
            List<GL> glList = new List<GL>();
            try
            {
                var gls = (from p in entities.GLs
                           where (p.GLCode.ToLower().Contains(searchTerm.ToLower()) || p.GLHead.ToLower().Contains(searchTerm.ToLower())) && p.CompanyId == companyId && p.Status == true && p.SLTypeId == 0 && p.IsBookGL == false && p.IsDebtorGL == false && p.IsTaxGL == false && p.IsBranchGL == false && p.IsCreditorGL == false && p.IsPostGL == false
                           select new
                           {
                               GLId = p.GLId,
                               GLHead = p.GLHead,
                               GLCode = p.GLCode,
                           }).ToList();


                if (gls != null && gls.Count > 0)
                {
                    foreach (var item in gls)
                    {
                        glList.Add(new GL { GLId = item.GLId, GLHead = item.GLHead, GLCode = item.GLCode });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return glList;
        }

        public List<GL> GetGLAutoCompleteList(string searchTerm, int slTypeId, int companyId)
        {
            List<GL> glList = new List<GL>();
            try
            {
                var sls = (from p in entities.GLs
                           where ((p.GLCode.ToLower().Contains(searchTerm.ToLower()) || p.GLHead.ToLower().Contains(searchTerm.ToLower())) && p.CompanyId == companyId && p.SLTypeId == slTypeId && p.Status == true)
                           select new
                           {
                               GLId = p.GLId,
                               GLHead = p.GLHead,
                               GLCode = p.GLCode
                           }).ToList();


                if (sls != null && sls.Count > 0)
                {
                    foreach (var item in sls)
                    {
                        glList.Add(new GL { GLId = item.GLId, GLHead = item.GLHead, GLCode = item.GLCode });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return glList;
        }
        #endregion

        #region Forgot Password

        public User GetUserFromEmail(string userEmail)
        {
            User user = new User();
            try
            {
                if (entities.Users.Any(x => x.Email == userEmail))
                {
                    user = entities.Users.Where(u => u.Email == userEmail).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return user;
        }
        #endregion


        #region SL
        public ResponseOut AddEditSL(SL sL)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.SLs.Any(x => x.SLCode == sL.SLCode && x.SLId != sL.SLId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateSLCode;
                }
                else
                {
                    if (sL.SLId == 0)
                    {
                        sL.CreatedDate = DateTime.Now;
                        entities.SLs.Add(sL);
                        responseOut.message = ActionMessage.SLCreatedSuccess;
                    }
                    else
                    {
                        sL.ModifiedBy = sL.CreatedBy;
                        sL.ModifiedDate = DateTime.Now;
                        entities.SLs.Where(a => a.SLId == sL.SLId).ToList().ForEach(a =>
                        {
                            a.SLCode = sL.SLCode;
                            a.SLHead = sL.SLHead;
                            a.RefCode = sL.RefCode;
                            a.SLTypeId = sL.SLTypeId;
                            a.PostingGLId = sL.PostingGLId;
                            a.CostCenterId = sL.CostCenterId;
                            a.ModifiedBy = sL.ModifiedBy;
                            a.ModifiedDate = sL.ModifiedDate;
                            a.Status = sL.Status;
                            a.CompanyBranchId = sL.CompanyBranchId;

                        });
                        responseOut.message = ActionMessage.SLUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;

                }
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


        public List<SLType> GetSLTList()
        {
            List<SLType> slTypeList = new List<SLType>();
            try
            {
                var slTypes = entities.SLTypes.Where(x => x.Status == true && x.SLTypeId != 0).Select(s => new
                {
                    SLTypeId = s.SLTypeId,
                    SLTypeName = s.SLTypeName
                }).ToList();
                if (slTypes != null && slTypes.Count > 0)
                {
                    foreach (var item in slTypes)
                    {
                        slTypeList.Add(new SLType { SLTypeId = item.SLTypeId, SLTypeName = item.SLTypeName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return slTypeList;
        }

        public List<CostCenter> GetCostCenterList()
        {
            List<CostCenter> costCenterList = new List<CostCenter>();
            try
            {
                var costCenters = entities.CostCenters.Where(x => x.Status == true).Select(s => new
                {
                    CostCenterId = s.CostCenterId,
                    CostCenterName = s.CostCenterName

                }).ToList();

                if (costCenters != null && costCenters.Count > 0)
                {
                    foreach (var item in costCenters)
                    {
                        costCenterList.Add(new CostCenter { CostCenterId = item.CostCenterId, CostCenterName = item.CostCenterName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return costCenterList;
        }

        public List<SL> GetSLAutoCompleteList(string searchTerm, int slTypeId, int companyId)
        {
            List<SL> slList = new List<SL>();
            try
            {
                var sls = (from p in entities.SLs
                           where (p.SLCode.ToLower().Contains(searchTerm.ToLower()) || p.SLHead.ToLower().Contains(searchTerm.ToLower()) || p.RefCode.ToLower().Contains(searchTerm.ToLower()))
                           && p.CompanyId == companyId && p.SLTypeId == slTypeId && p.Status == true
                           select new
                           {
                               SLId = p.SLId,
                               SLHead = p.SLHead,
                               SLCode = p.SLCode
                           }).ToList();


                if (sls != null && sls.Count > 0)
                {
                    foreach (var item in sls)
                    {
                        slList.Add(new SL { SLId = item.SLId, SLHead = item.SLHead, SLCode = item.SLCode });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return slList;
        }

        public List<SL> GetSLAutoCompleteListForTax(string term, int companyId)
        {
            List<SL> slList = new List<SL>();
            try
            {
                var sls = (from p in entities.SLs
                           where (p.SLCode.ToLower().Contains(term.ToLower()) || p.SLHead.ToLower().Contains(term.ToLower()) || p.RefCode.ToLower().Contains(term.ToLower())) && p.CompanyId == companyId && p.Status == true && p.SLTypeId == 5
                           select new
                           {
                               SLId = p.SLId,
                               SLHead = p.SLHead,
                               SLCode = p.SLCode
                           }).ToList();


                if (sls != null && sls.Count > 0)
                {
                    foreach (var item in sls)
                    {
                        slList.Add(new SL { SLId = item.SLId, SLHead = item.SLHead, SLCode = item.SLCode });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return slList;
        }






        #endregion

        #region Cot Center
        public List<CostCenter> GetCostCenterList(int companyId)
        {
            List<CostCenter> costCenterList = new List<CostCenter>();
            try
            {
                var costCenters = entities.CostCenters.Where(x => x.CompanyId == companyId && x.Status == true).Select(s => new
                {
                    CostCenterId = s.CostCenterId,
                    CostCenterName = s.CostCenterName
                }).ToList();

                if (costCenters != null && costCenters.Count > 0)
                {
                    foreach (var item in costCenters)
                    {
                        costCenterList.Add(new CostCenter { CostCenterId = item.CostCenterId, CostCenterName = item.CostCenterName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return costCenterList;
        }
        #endregion



        #region Sale Invoice
        public ResponseOut CancelSaleInvoice(SaleInvoice saleinvoice)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {

                entities.SaleInvoices.Where(a => a.InvoiceId == saleinvoice.InvoiceId).ToList().ForEach(a =>
                {
                    a.ApprovalStatus = saleinvoice.ApprovalStatus;
                    a.CancelStatus = saleinvoice.CancelStatus;
                    a.CancelBy = saleinvoice.CreatedBy;
                    a.CancelDate = DateTime.Now;
                    a.CancelReason = saleinvoice.CancelReason;
                });
                responseOut.message = ActionMessage.SaleInvoiceCancelSuccess;

                entities.SaveChanges();
                responseOut.status = ActionStatus.Success;

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



        #region Purchase Invoice
        public ResponseOut CancelPI(PurchaseInvoice purchaseinvoice)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                entities.PurchaseInvoices.Where(a => a.InvoiceId == purchaseinvoice.InvoiceId).ToList().ForEach(a =>
                {
                    a.ApprovalStatus = purchaseinvoice.ApprovalStatus;
                    a.CancelStatus = purchaseinvoice.CancelStatus;
                    a.CancelBy = purchaseinvoice.CreatedBy;
                    a.CancelDate = DateTime.Now;
                    a.CancelReason = purchaseinvoice.CancelReason;
                });
                responseOut.message = ActionMessage.PurchaseInvoiceCancelSuccess;

                entities.SaveChanges();
                responseOut.status = ActionStatus.Success;

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


        #region PO
        public ResponseOut CancelPO(PO po)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                entities.POes.Where(a => a.POId == po.POId).ToList().ForEach(a =>
                {
                    a.POStatus = po.POStatus;
                    a.CancelStatus = po.CancelStatus;
                    a.CancelBy = po.CreatedBy;
                    a.CancelDate = DateTime.Now;
                    a.CancelReason = po.CancelReason;
                });
                responseOut.message = ActionMessage.POCancelSuccess;

                entities.SaveChanges();
                responseOut.status = ActionStatus.Success;

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

        #region Cost Center
        public ResponseOut AddEditCostCenter(CostCenter costcenter)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.CostCenters.Any(x => x.CostCenterName == costcenter.CostCenterName && x.CompanyId == costcenter.CompanyId && x.CostCenterId != costcenter.CostCenterId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateCostCenterName;
                }

                else
                {
                    if (costcenter.CostCenterId == 0)
                    {
                        costcenter.CreatedDate = DateTime.Now;
                        entities.CostCenters.Add(costcenter);
                        responseOut.message = ActionMessage.CostCenterCreatedSuccess;
                    }
                    else
                    {
                        costcenter.Modifiedby = costcenter.CreatedBy;
                        costcenter.ModifiedDate = DateTime.Now;
                        entities.CostCenters.Where(a => a.CostCenterId == costcenter.CostCenterId).ToList().ForEach(a =>
                        {
                            a.CostCenterId = costcenter.CostCenterId;
                            a.CostCenterName = costcenter.CostCenterName;
                            a.CompanyId = costcenter.CompanyId;
                            a.Status = costcenter.Status;
                            a.CompanyBranchId = costcenter.CompanyBranchId;
                        });
                        responseOut.message = ActionMessage.CostCenterUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                }
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

        #region SLDetails
        public ResponseOut AddEditSLDetail(SLDetail sLDetail)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {

                if (entities.SLDetails.Any(x => x.SLDetailId == sLDetail.SLDetailId))
                {
                    entities.SLDetails.Where(a => a.SLDetailId == sLDetail.SLDetailId).ToList().ForEach(a =>
                    {
                        a.SLDetailId = sLDetail.SLDetailId;
                        a.GLId = sLDetail.GLId;
                        a.SLId = sLDetail.SLId;
                        a.CompanyId = sLDetail.CompanyId;
                        a.FinYearId = sLDetail.FinYearId;
                        a.OpeningBalance = sLDetail.OpeningBalance;
                        a.OpeningBalanceDebit = sLDetail.OpeningBalanceDebit;
                        a.OpeningBalanceCredit = sLDetail.OpeningBalanceCredit;
                        a.ModifiedBy = sLDetail.CreatedBy;
                        a.ModifiedDate = DateTime.Now;
                        a.Status = sLDetail.Status;
                        a.CompanyBranchId = sLDetail.CompanyBranchId;
                    });
                    //if(entities.GLDetails.Any(x => x.GLId == sLDetail.GLId && x.CompanyId == sLDetail.CompanyId && x.CompanyBranchId == sLDetail.CompanyBranchId && x.FinYearId == sLDetail.FinYearId))
                    //{ 
                    //    entities.GLDetails.Where(a => a.GLId == sLDetail.GLId && a.CompanyId== sLDetail.CompanyId && a.CompanyBranchId== sLDetail.CompanyBranchId && a.FinYearId==sLDetail.FinYearId).ToList().ForEach(a =>
                    //    {                                                                                          
                    //        a.OpeningBalance = sLDetail.OpeningBalance;
                    //        a.OpeningBalanceDebit = sLDetail.OpeningBalanceDebit;
                    //        a.OpeningBalanceCredit = sLDetail.OpeningBalanceCredit;
                    //        a.ModifiedBy = sLDetail.CreatedBy;
                    //        a.ModifiedDate = DateTime.Now;
                    //        a.Status = sLDetail.Status;                       
                    //    });

                    //}
                    //    else
                    //    {
                    //        GLDetail gLDetail = new GLDetail {
                    //            GLId= sLDetail.GLId,
                    //            CompanyId = sLDetail.CompanyId,
                    //            FinYearId = sLDetail.FinYearId,
                    //            CompanyBranchId = sLDetail.CompanyBranchId,
                    //            OpeningBalance =sLDetail.OpeningBalance,                            
                    //            OpeningBalanceDebit = sLDetail.OpeningBalanceDebit,
                    //            OpeningBalanceCredit= sLDetail.OpeningBalanceCredit,
                    //            CreatedBy = sLDetail.CreatedBy,
                    //            CreatedDate = DateTime.Now,
                    //            Status = sLDetail.Status,
                    //    };
                    //        entities.GLDetails.Add(gLDetail);
                    //  }
                }
                else
                {


                    //if (entities.GLDetails.Any(x => x.GLId == sLDetail.GLId && x.CompanyId == sLDetail.CompanyId && x.CompanyBranchId == sLDetail.CompanyBranchId && x.FinYearId == sLDetail.FinYearId))
                    //{
                    //    entities.GLDetails.Where(a => a.GLId == sLDetail.GLId && a.CompanyId == sLDetail.CompanyId && a.CompanyBranchId == sLDetail.CompanyBranchId && a.FinYearId == sLDetail.FinYearId).ToList().ForEach(a =>
                    //    {
                    //        a.OpeningBalance = sLDetail.OpeningBalance;
                    //        a.OpeningBalanceDebit = sLDetail.OpeningBalanceDebit;
                    //        a.OpeningBalanceCredit = sLDetail.OpeningBalanceCredit;
                    //        a.ModifiedBy = sLDetail.CreatedBy;
                    //        a.ModifiedDate = DateTime.Now;
                    //        a.Status = sLDetail.Status;
                    //    });

                    //}
                    //else
                    //{
                    //    GLDetail gLDetail = new GLDetail
                    //    {
                    //        GLId = sLDetail.GLId,
                    //        CompanyId = sLDetail.CompanyId,
                    //        FinYearId = sLDetail.FinYearId,
                    //        CompanyBranchId = sLDetail.CompanyBranchId,
                    //        OpeningBalance = sLDetail.OpeningBalance,
                    //        OpeningBalanceDebit = sLDetail.OpeningBalanceDebit,
                    //        OpeningBalanceCredit = sLDetail.OpeningBalanceCredit,
                    //        CreatedBy = sLDetail.CreatedBy,
                    //        CreatedDate = DateTime.Now,
                    //        Status = sLDetail.Status,
                    //    };
                    //    entities.GLDetails.Add(gLDetail);
                    //}
                    entities.SLDetails.Add(sLDetail);

                }
                entities.SaveChanges();
                responseOut.message = ActionMessage.SLDetailSuccessful;
                responseOut.status = ActionStatus.Success;
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



        #region Bank Voucher
        public ResponseOut CancelApprovedBankVoucher(Voucher voucher)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {



                if (voucher.VoucherStatus == "Cancel")
                {
                    entities.Vouchers.Where(a => a.VoucherId == voucher.VoucherId).ToList().ForEach(a =>
                    {
                        a.VoucherStatus = voucher.VoucherStatus;
                        a.CancelledBy = voucher.CreatedBy;
                        a.CancelledDate = DateTime.Now;
                        a.CancelReason = voucher.CancelReason;
                    });
                    entities.Vouchers.Where(a => a.ContraVoucherId == voucher.VoucherId).ToList().ForEach(a =>
                    {
                        a.VoucherStatus = voucher.VoucherStatus;
                        a.CancelledBy = voucher.CreatedBy;
                        a.CancelledDate = DateTime.Now;
                        a.CancelReason = voucher.CancelReason;
                    });

                    responseOut.message = ActionMessage.BankVoucherCancelSuccess;
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;

                }


                if (voucher.VoucherStatus == "Approved")

                {
                    entities.Vouchers.Where(a => a.VoucherId == voucher.VoucherId).ToList().ForEach(a =>
                    {
                        a.VoucherStatus = voucher.VoucherStatus;
                        a.ApprovedBy = voucher.CreatedBy;
                        a.ApprovedDate = DateTime.Now;
                    });
                    entities.Vouchers.Where(a => a.ContraVoucherId == voucher.VoucherId).ToList().ForEach(a =>
                    {
                        a.VoucherStatus = voucher.VoucherStatus;
                        a.ApprovedBy = voucher.CreatedBy;
                        a.ApprovedDate = DateTime.Now;
                    });
                    responseOut.message = ActionMessage.BankVoucherApprovedSuccess;
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;

                }

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

        #region Cash Voucher
        public ResponseOut CancelApprovedCashVoucher(Voucher voucher)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (voucher.VoucherStatus == "Cancel")
                {
                    entities.Vouchers.Where(a => a.VoucherId == voucher.VoucherId).ToList().ForEach(a =>
                    {
                        a.VoucherStatus = voucher.VoucherStatus;
                        a.CancelledBy = voucher.CreatedBy;
                        a.CancelledDate = DateTime.Now;
                        a.CancelReason = voucher.CancelReason;
                    });
                    entities.Vouchers.Where(a => a.ContraVoucherId == voucher.VoucherId).ToList().ForEach(a =>
                    {
                        a.VoucherStatus = voucher.VoucherStatus;
                        a.CancelledBy = voucher.CreatedBy;
                        a.CancelledDate = DateTime.Now;
                        a.CancelReason = voucher.CancelReason;
                    });
                    responseOut.message = ActionMessage.CashVoucherCancelSuccess;
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;

                }


                if (voucher.VoucherStatus == "Approved")

                {
                    entities.Vouchers.Where(a => a.VoucherId == voucher.VoucherId).ToList().ForEach(a =>
                    {
                        a.VoucherStatus = voucher.VoucherStatus;
                        a.ApprovedBy = voucher.CreatedBy;
                        a.ApprovedDate = DateTime.Now;
                    });
                    entities.Vouchers.Where(a => a.ContraVoucherId == voucher.VoucherId).ToList().ForEach(a =>
                    {
                        a.VoucherStatus = voucher.VoucherStatus;
                        a.ApprovedBy = voucher.CreatedBy;
                        a.ApprovedDate = DateTime.Now;
                    });
                    responseOut.message = ActionMessage.CashVoucherApprovedSuccess;
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;

                }

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

        #region Journal Voucher
        public ResponseOut CancelApprovedJournalVoucher(Voucher voucher)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (voucher.VoucherStatus == "Cancel")
                {
                    entities.Vouchers.Where(a => a.VoucherId == voucher.VoucherId).ToList().ForEach(a =>
                    {
                        a.VoucherStatus = voucher.VoucherStatus;
                        a.CancelledBy = voucher.CreatedBy;
                        a.CancelledDate = DateTime.Now;
                        a.CancelReason = voucher.CancelReason;
                    });
                    responseOut.message = ActionMessage.JournalVoucherCancelSuccess;
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;

                }

                if (voucher.VoucherStatus == "Approved")
                {
                    entities.Vouchers.Where(a => a.VoucherId == voucher.VoucherId).ToList().ForEach(a =>
                    {
                        a.VoucherStatus = voucher.VoucherStatus;
                        a.ApprovedBy = voucher.CreatedBy;
                        a.ApprovedDate = DateTime.Now;
                    });
                    responseOut.message = ActionMessage.JournalVoucherApprovedSuccess;
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;

                }


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

        #region DebitNoteVoucher
        public ResponseOut CancelApprovedDebitNoteVoucher(Voucher voucher)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {

                if (voucher.VoucherStatus == "Cancel")
                {
                    entities.Vouchers.Where(a => a.VoucherId == voucher.VoucherId).ToList().ForEach(a =>
                    {
                        a.VoucherStatus = voucher.VoucherStatus;
                        a.CancelledBy = voucher.CreatedBy;
                        a.CancelledDate = DateTime.Now;
                        a.CancelReason = voucher.CancelReason;
                    });

                    responseOut.message = ActionMessage.DebitNoteVoucherCancelSuccess;
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;

                }

                if (voucher.VoucherStatus == "Approved")
                {

                    entities.Vouchers.Where(a => a.VoucherId == voucher.VoucherId).ToList().ForEach(a =>
                    {
                        a.VoucherStatus = voucher.VoucherStatus;
                        a.ApprovedBy = voucher.CreatedBy;
                        a.ApprovedDate = DateTime.Now;

                    });
                    responseOut.message = ActionMessage.DebitNoteVoucherApprovedSuccess;
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;

                }

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

        #region Credit Note Voucher
        public ResponseOut CancelApprovedCreditNoteVoucher(Voucher voucher)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {

                if (voucher.VoucherStatus == "Cancel")
                {
                    entities.Vouchers.Where(a => a.VoucherId == voucher.VoucherId).ToList().ForEach(a =>
                    {
                        a.VoucherStatus = voucher.VoucherStatus;
                        a.CancelledBy = voucher.CreatedBy;
                        a.CancelledDate = DateTime.Now;
                        a.CancelReason = voucher.CancelReason;
                    });
                    responseOut.message = ActionMessage.CreditNoteVoucherCancelSuccess;
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;

                }


                if (voucher.VoucherStatus == "Approved")

                {
                    entities.Vouchers.Where(a => a.VoucherId == voucher.VoucherId).ToList().ForEach(a =>
                    {
                        a.VoucherStatus = voucher.VoucherStatus;
                        a.ApprovedBy = voucher.CreatedBy;
                        a.ApprovedDate = DateTime.Now;
                    });
                    responseOut.message = ActionMessage.CreditNoteVoucherApprovedSuccess;
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;

                }

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

        #region Product GL Mapping
        public ResponseOut AddEditProductGLMapping(ProductGLMapping productGLMapping)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.ProductGLMappings.Any(x => x.ProductSubGroupId == productGLMapping.ProductSubGroupId && x.GLType == productGLMapping.GLType && x.MappingId != productGLMapping.MappingId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateProductSubGroup;
                }

                else
                {

                    if (productGLMapping.MappingId == 0)
                    {
                        productGLMapping.CreatedDate = DateTime.Now;
                        entities.ProductGLMappings.Add(productGLMapping);
                        responseOut.message = ActionMessage.ProductGLMappingCreatedSuccessful;
                    }
                    else
                    {
                        entities.ProductGLMappings.Where(a => a.ProductSubGroupId == productGLMapping.ProductSubGroupId).ToList().ForEach(a =>
                        {

                            a.ProductSubGroupId = productGLMapping.ProductSubGroupId;
                            a.GLId = productGLMapping.GLId;
                            a.CompanyId = productGLMapping.CompanyId;
                            a.Status = productGLMapping.Status;
                            a.CreatedBy = productGLMapping.CreatedBy;
                            a.GLType = productGLMapping.GLType;
                            a.CreatedDate = DateTime.Now;

                        });
                        responseOut.message = ActionMessage.ProductGLMappingUpdateSuccessful;

                    }
                }
                entities.SaveChanges();
                responseOut.status = ActionStatus.Success;
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

        #region Upload Utility
        public int GetIdByStateName(string stateName)
        {
            int stateId = 0;
            try
            {
                stateId = entities.States.Where(s => s.StateName.Trim().ToUpper() == stateName.Trim().ToUpper()).Select(x => x.Stateid).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return stateId;
        }
        public int GetIdByCountryName(string countryName)
        {
            int countryId = 0;
            try
            {
                countryId = entities.Countries.Where(s => s.CountryName.Trim().ToUpper() == countryName.Trim().ToUpper()).Select(x => x.CountryId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return countryId;
        }

        public int GetIdByCustomerName(string customerName)
        {
            int customerId = 0;
            try
            {
                customerId = entities.Customers.Where(s => s.CustomerName.Trim().ToUpper() == customerName.Trim().ToUpper()).Select(x => x.CustomerId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return customerId;
        }


        public int GetIdByLeadSourceName(string leadSourceName)
        {
            int leadSourceId = 0;
            try
            {
                leadSourceId = entities.LeadSources.Where(s => s.LeadSourceName.Trim().ToUpper() == leadSourceName.Trim().ToUpper()).Select(x => x.LeadSourceId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return leadSourceId;
        }
        public int GetIdByLeadStatusName(string leadStatusName)
        {
            int leadStatusId = 0;
            try
            {
                leadStatusId = entities.LeadStatus.Where(s => s.LeadStatusName.Trim().ToUpper() == leadStatusName.Trim().ToUpper()).Select(x => x.LeadStatusId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return leadStatusId;
        }
        public int GetIdByFollowUpActivityTypeName(string followUpActivityTypeName)
        {
            int followUpActivityTypeId = 0;
            try
            {
                followUpActivityTypeId = entities.FollowUpActivityTypes.Where(s => s.FollowUpActivityTypeName.Trim().ToUpper() == followUpActivityTypeName.Trim().ToUpper()).Select(x => x.FollowUpActivityTypeId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return followUpActivityTypeId;
        }

        public int GetIdByGLMainGroupName(string gLMainGroupName)
        {
            int glMainGroupId = 0;
            try
            {
                glMainGroupId = entities.GLMainGroups.Where(s => s.GLMainGroupName.Trim().ToUpper() == gLMainGroupName.Trim().ToUpper()).Select(x => x.GLMainGroupId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return glMainGroupId;
        }
        public int GetIdByScheduleName(string scheduleName)
        {
            int scheduleID = 0;
            try
            {
                scheduleID = entities.Schedules.Where(s => s.ScheduleName.Trim().ToUpper() == scheduleName.Trim().ToUpper()).Select(x => x.ScheduleId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return scheduleID;
        }
        public int GetIdBySLTypeName(string sLTypeName)
        {
            int sLTypeId = 0;
            try
            {
                sLTypeId = entities.SLTypes.Where(s => s.SLTypeName.Trim().ToUpper() == sLTypeName.Trim().ToUpper()).Select(x => x.SLTypeId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return sLTypeId;
        }



        public int GetIdByGLSubGroupName(string gLSubGroupName)
        {
            int gLSubGroupId = 0;
            try
            {
                gLSubGroupId = entities.GLSubGroups.Where(s => s.GLSubGroupName.Trim().ToUpper() == gLSubGroupName.Trim().ToUpper()).Select(x => x.GLSubGroupId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return gLSubGroupId;
        }







        public int GetIdByCostCenterName(string costCenterName)
        {
            int costCenterId = 0;
            try
            {
                costCenterId = entities.CostCenters.Where(s => s.CostCenterName.Trim().ToUpper() == costCenterName.Trim().ToUpper()).Select(x => x.CostCenterId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return costCenterId;
        }
        public int GetIdBySubCostCenterName(string subcostCenterName)
        {
            int subcostCenterId = 0;
            try
            {
                subcostCenterId = entities.SubCostCenters.Where(s => s.SubCostCenterName.Trim().ToUpper() == subcostCenterName.Trim().ToUpper()).Select(x => x.SubCostCenterId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return subcostCenterId;
        }
        public int GetIdByGLHead(string gLHead)
        {
            int postingGLId = 0;
            try
            {
                postingGLId = entities.GLs.Where(s => s.GLHead.Trim().ToUpper() == gLHead.Trim().ToUpper()).Select(x => x.GLId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return postingGLId;
        }

        public int GetIdByEmployeeName(string employeeName)
        {
            int employeeId = 0;
            try
            {
                employeeId = entities.Employees.Where(s => s.FirstName.Trim().ToUpper() == employeeName.Trim().ToUpper()).Select(x => x.EmployeeId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return employeeId;
        }
        public int GetIdByCustomerTypeDesc(string customerTypeDesc)
        {
            int customerTypeId = 0;
            try
            {
                customerTypeId = entities.CustomerTypes.Where(s => s.CustomerTypeDesc.Trim().ToUpper() == customerTypeDesc.Trim().ToUpper()).Select(x => x.CustomerTypeId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return customerTypeId;
        }
        public int GetIdByDepartmentName(string departmentName)
        {
            int departmentId = 0;
            try
            {
                departmentId = entities.Departments.Where(s => s.DepartmentName.Trim().ToUpper() == departmentName.Trim().ToUpper()).Select(x => x.DepartmentId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return departmentId;
        }
        public int GetIdByDesignationName(string designationName)
        {
            int designationId = 0;
            try
            {
                designationId = entities.Designations.Where(s => s.DesignationName.Trim().ToUpper() == designationName.Trim().ToUpper()).Select(x => x.DesignationId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return designationId;
        }
        public int GetIdByProductMainGroupName(string mainGroupName)
        {
            int productMainGroupID = 0;
            try
            {
                productMainGroupID = entities.ProductMainGroups.Where(s => s.ProductMainGroupName.Trim().ToUpper() == mainGroupName.Trim().ToUpper()).Select(x => x.ProductMainGroupId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return productMainGroupID;
        }
        public int GetIdByProductTypeName(string productTypeName)
        {
            int productTypeId = 0;
            try
            {
                productTypeId = entities.ProductTypes.Where(s => s.ProductTypeName.Trim().ToUpper() == productTypeName.Trim().ToUpper()).Select(x => x.ProductTypeId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return productTypeId;
        }
        public int GetIdByProductSubGroupName(string subGroupName)
        {
            int productSubGroupID = 0;
            try
            {
                productSubGroupID = entities.ProductSubGroups.Where(s => s.ProductSubGroupName.Trim().ToUpper() == subGroupName.Trim().ToUpper()).Select(x => x.ProductSubGroupId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return productSubGroupID;
        }
        public int GetIdByUOMName(string uOMName)
        {
            int UOMId = 0;
            try
            {
                UOMId = entities.UOMs.Where(s => s.UOMName.Trim().ToUpper() == uOMName.Trim().ToUpper()).Select(x => x.UOMId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return UOMId;
        }






        public long GetIdByProductName(string productName)
        {
            long productId = 0;
            try
            {
                productId = entities.Products.Where(s => s.ProductName.Trim().ToUpper() == productName.Trim().ToUpper() && (s.AssemblyType.Trim().ToUpper() == "SA" || s.AssemblyType.Trim().ToUpper() == "RC")).Select(x => x.Productid).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return productId;
        }
        public long GetIdByAllProductName(string productName)
        {
            long productId = 0;
            try
            {
                productId = entities.Products.Where(s => s.ProductName.Trim().ToUpper() == productName.Trim().ToUpper()).Select(x => x.Productid).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return productId;
        }
        public long GetIdByProductNameForChessis(string productName)
        {
            long productId = 0;
            try
            {
                productId = entities.Products.Where(s => s.ProductName.Trim().ToUpper() == productName.Trim().ToUpper() && (s.AssemblyType.Trim().ToUpper() == "MA")).Select(x => x.Productid).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return productId;
        }

        public long GetProductIdByProductName(string productName)
        {
            long productId = 0;
            try
            {
                productId = entities.Products.Where(s => s.ProductName.Trim().ToUpper() == productName.Trim().ToUpper()).Select(x => x.Productid).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return productId;
        }
        public long GetIdByAssemblyName(string assemblyName)
        {
            long assemblyId = 0;
            try
            {
                assemblyId = entities.Products.Where((s => s.ProductName.Replace("\r\n", "").Trim().ToUpper() == assemblyName && s.AssemblyType.Trim().ToUpper() == "MA")).Select(x => x.Productid).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return assemblyId;
        }

        public long GetIdByCompanyBranchName(string companyBranchName)
        {
            long companyBranchId = 0;
            try
            {
                companyBranchId = entities.ComapnyBranches.Where(s => s.BranchName.Trim().ToUpper() == companyBranchName).Select(x => x.CompanyBranchId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return companyBranchId;
        }







        public long GetIdByProductNameForOpening(string productName)
        {
            long productId = 0;
            try
            {
                productId = entities.Products.Where(s => s.ProductName.Trim().ToUpper() == productName.Trim().ToUpper()).Select(x => x.Productid).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return productId;
        }
        public int GetIdByGLName(string gLHead)
        {
            int productId = 0;
            try
            {
                productId = entities.GLs.Where(s => s.GLHead.Trim().ToUpper() == gLHead.Trim().ToUpper()).Select(x => x.GLId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return productId;
        }


        public long GetIdBySLHead(string sLHead)
        {
            long sLId = 0;
            try
            {
                sLId = entities.SLs.Where(s => s.SLHead.Trim().ToUpper() == sLHead.Trim().ToUpper()).Select(x => x.SLId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return sLId;
        }
        public long GetIdByChasisSerialNo(string chasisSerialNo)
        {
            long productId = 0;
            try
            {
                productId = Convert.ToInt32(entities.ChasisSerialMappings.Where(s => s.ChasisSerialNo.Trim().ToUpper() == chasisSerialNo.Trim().ToUpper()).Select(x => x.ProductId).FirstOrDefault());
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return productId;
        }

        #endregion
        #region Currency
        public List<Currency> GetCurrencyList()
        {
            List<Currency> currencyList = new List<Currency>();
            try
            {
                var currencies = entities.Currencies.Where(x => x.Status == true).Select(s => new
                {
                    CurrencyId = s.CurrencyId,
                    CurrencyCode = s.CurrencyCode,
                    CurrencyName = s.CurrencyName
                }).ToList();
                if (currencies != null && currencies.Count > 0)
                {
                    foreach (var item in currencies)
                    {
                        currencyList.Add(new Currency { CurrencyId = item.CurrencyId, CurrencyCode = item.CurrencyCode, CurrencyName = item.CurrencyName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return currencyList;
        }
        #endregion

        #region ChasisSerialMapping
        public ResponseOut AddEditChasisSerialMapping(ChasisSerialMapping chasisSerialMapping)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.ChasisSerialMappings.Any(x => x.ChasisSerialNo == chasisSerialMapping.ChasisSerialNo && x.CompanyBranchId == chasisSerialMapping.CompanyBranchId && x.MappingId != chasisSerialMapping.MappingId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateChasisSerialNo;
                }
                else if (entities.ChasisSerialMappings.Any(x => x.MotorNo == chasisSerialMapping.MotorNo && x.CompanyBranchId == chasisSerialMapping.CompanyBranchId && x.MappingId != chasisSerialMapping.MappingId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateMotorNo;
                }              
                else
                {
                    if (chasisSerialMapping.MappingId == 0)
                    {
                        chasisSerialMapping.CreatedDate = DateTime.Now;
                        entities.ChasisSerialMappings.Add(chasisSerialMapping);
                        entities.SaveChanges();
                        responseOut.status = ActionStatus.Success;
                        responseOut.message = ActionMessage.ChasisSerialMappingCreatedSuccess;

                    }
                    else
                    {
                        chasisSerialMapping.ModifiedBy = chasisSerialMapping.CreatedBy;
                        chasisSerialMapping.ModifiedDate = DateTime.Now;

                        entities.ChasisSerialMappings.Where(a => a.MappingId == chasisSerialMapping.MappingId).ToList().ForEach(a =>
                        {
                            a.ProductId = chasisSerialMapping.ProductId;
                            a.ChasisSerialNo = chasisSerialMapping.ChasisSerialNo;
                            a.MotorNo = chasisSerialMapping.MotorNo;
                            a.ControllerNo = chasisSerialMapping.ControllerNo;
                            a.Color = chasisSerialMapping.Color;
                            a.BatteryPower = chasisSerialMapping.BatteryPower;
                            a.BatterySerialNo1 = chasisSerialMapping.BatterySerialNo1;
                            a.BatterySerialNo2 = chasisSerialMapping.BatterySerialNo2;
                            a.BatterySerialNo3 = chasisSerialMapping.BatterySerialNo3;
                            a.BatterySerialNo4 = chasisSerialMapping.BatterySerialNo4;
                            a.Tier = chasisSerialMapping.Tier;
                            a.FrontGlassAvailable = chasisSerialMapping.FrontGlassAvailable;
                            a.ViperAvailable = chasisSerialMapping.ViperAvailable;
                            a.RearShockerAvailable = chasisSerialMapping.RearShockerAvailable;
                            a.ChargerAvailable = chasisSerialMapping.ChargerAvailable;
                            a.FM = chasisSerialMapping.FM;
                            a.ModifiedDate = chasisSerialMapping.ModifiedDate;
                            a.Status = chasisSerialMapping.Status;
                            a.CompanyBranchId = chasisSerialMapping.CompanyBranchId;


                        });
                        responseOut.message = ActionMessage.ChasisSerialMappingUpdatedSuccess;

                        entities.SaveChanges();
                        // responseOut.trnId = product.Productid;
                        responseOut.status = ActionStatus.Success;

                    }
                }
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
        public ResponseOut AddEditChasisSerialMappingForUploadUtility(ChasisSerialMapping chasisSerialMapping)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.ChasisSerialMappings.Any(x => x.ChasisSerialNo == chasisSerialMapping.ChasisSerialNo && x.MappingId != chasisSerialMapping.MappingId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateChasisSerialNo;
                }
                else if (entities.ChasisSerialMappings.Any(x => x.MotorNo == chasisSerialMapping.MotorNo && x.MappingId != chasisSerialMapping.MappingId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateMotorNo;
                }
                else if ((chasisSerialMapping.ControllerNo != "") && (entities.ChasisSerialMappings.Any(x => x.ControllerNo == chasisSerialMapping.ControllerNo && x.MappingId != chasisSerialMapping.MappingId)))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateControllerNo;
                }
                else
                {
                    if (chasisSerialMapping.MappingId == 0)
                    {
                        chasisSerialMapping.CreatedDate = DateTime.Now;
                        entities.ChasisSerialMappings.Add(chasisSerialMapping);
                        entities.SaveChanges();
                        responseOut.status = ActionStatus.Success;
                        responseOut.message = ActionMessage.ChasisSerialMappingCreatedSuccess;

                    }
                    else
                    {
                        chasisSerialMapping.ModifiedBy = chasisSerialMapping.CreatedBy;
                        chasisSerialMapping.ModifiedDate = DateTime.Now;

                        entities.ChasisSerialMappings.Where(a => a.MappingId == chasisSerialMapping.MappingId).ToList().ForEach(a =>
                        {
                            a.ProductId = chasisSerialMapping.ProductId;
                            a.ChasisSerialNo = chasisSerialMapping.ChasisSerialNo;
                            a.MotorNo = chasisSerialMapping.MotorNo;
                            a.ControllerNo = chasisSerialMapping.ControllerNo;
                            a.Color = chasisSerialMapping.Color;
                            a.BatteryPower = chasisSerialMapping.BatteryPower;
                            a.BatterySerialNo1 = chasisSerialMapping.BatterySerialNo1;
                            a.BatterySerialNo2 = chasisSerialMapping.BatterySerialNo2;
                            a.BatterySerialNo3 = chasisSerialMapping.BatterySerialNo3;
                            a.BatterySerialNo4 = chasisSerialMapping.BatterySerialNo4;
                            a.Tier = chasisSerialMapping.Tier;
                            a.FrontGlassAvailable = chasisSerialMapping.FrontGlassAvailable;
                            a.ViperAvailable = chasisSerialMapping.ViperAvailable;
                            a.RearShockerAvailable = chasisSerialMapping.RearShockerAvailable;
                            a.ChargerAvailable = chasisSerialMapping.ChargerAvailable;
                            a.FM = chasisSerialMapping.FM;
                            a.ModifiedDate = chasisSerialMapping.ModifiedDate;
                            a.Status = chasisSerialMapping.Status;
                            a.CompanyBranchId = chasisSerialMapping.CompanyBranchId;
                        });
                        responseOut.message = ActionMessage.ChasisSerialMappingUpdatedSuccess;

                        entities.SaveChanges();
                        // responseOut.trnId = product.Productid;
                        responseOut.status = ActionStatus.Success;

                    }
                }
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
        #endregion ChasisSerialMapping

        #region EmployeeProfile
        public Employee GetEmployee(string email)
        {
            Employee employee = new Employee();
            try
            {
                if (entities.Employees.Any(x => x.Email == email))
                {
                    employee = entities.Employees.Where(u => u.Email == email).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return employee;
        }
        #endregion

        #region Thought
        public ResponseOut AddEditThought(Thought thought)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.Thoughts.Any(x => x.ThoughtMessage == thought.ThoughtMessage && x.ThoughtId != thought.ThoughtId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateThoughtName;
                }
                else
                {
                    if (thought.ThoughtId == 0)
                    {
                        entities.Thoughts.Add(thought);
                        responseOut.message = ActionMessage.ThoughtCreatedSuccess;
                    }
                    else
                    {
                        entities.Thoughts.Where(a => a.ThoughtId == thought.ThoughtId).ToList().ForEach(a =>
                        {
                            a.ThoughtId = thought.ThoughtId;
                            a.ThoughtMessage = thought.ThoughtMessage;
                            a.ThoughtType = thought.ThoughtType;
                            a.Status = thought.Status;
                        });
                        responseOut.message = ActionMessage.ThoughtUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                }
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

        #region Email Template
        public ResponseOut AddEditEmailTemplate(EmailTemplate emailTemplate)
        {

            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.EmailTemplates.Any(x => x.EmailTemplateId == emailTemplate.EmailTemplateId && x.EmailTemplateId != emailTemplate.EmailTemplateId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateEmailTempalteName;
                }
                else
                {
                    if (emailTemplate.EmailTemplateId == 0)
                    {
                        emailTemplate.CreatedDate = DateTime.Now;
                        entities.EmailTemplates.Add(emailTemplate);
                        responseOut.message = ActionMessage.EmailTemplateCreatedSuccess;
                    }
                    else
                    {
                        emailTemplate.ModifiedBy = emailTemplate.CreatedBy;
                        emailTemplate.ModifiedDate = DateTime.Now;
                        entities.EmailTemplates.Where(a => a.EmailTemplateId == emailTemplate.EmailTemplateId).ToList().ForEach(a =>
                        {
                            a.EmailTemplateSubject = emailTemplate.EmailTemplateSubject;
                            a.EmailTemplateTypeId = emailTemplate.EmailTemplateTypeId;
                            a.EmailTemplateDesc = emailTemplate.EmailTemplateDesc;
                            a.CompanyId = emailTemplate.CompanyId;
                            a.ModifiedBy = emailTemplate.ModifiedBy;
                            a.ModifiedDate = emailTemplate.ModifiedDate;
                            a.Status = emailTemplate.Status;
                            a.CompanyBranchId = emailTemplate.CompanyBranchId;
                        });
                        responseOut.message = ActionMessage.EmailTemplateUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                }
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

        public List<EmailTemplateType> GetEmailTemplateType()
        {
            List<EmailTemplateType> emailTemplateTypeList = new List<EmailTemplateType>();
            try
            {
                var emailTemplateTypes = entities.EmailTemplateTypes.Where(x => x.Status == true).OrderBy(x => SqlFunctions.IsNumeric(x.EmailTemplateName)).ThenBy(x => x.EmailTemplateName).Select(s => new
                {
                    EmailTemplateTypeId = s.EmailTemplateTypeId,
                    EmailTemplateName = s.EmailTemplateName,
                    Status = s.Status

                }).ToList();

                if (emailTemplateTypes != null && emailTemplateTypes.Count > 0)
                {
                    foreach (var item in emailTemplateTypes)
                    {
                        emailTemplateTypeList.Add(new EmailTemplateType
                        {
                            EmailTemplateTypeId = item.EmailTemplateTypeId,
                            EmailTemplateName = item.EmailTemplateName
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return emailTemplateTypeList;
        }
        #endregion

        #region Location
        public ResponseOut AddEditLocation(Location location)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (location.IsStoreLocation == true)
                {
                    if (entities.Locations.Any(x => x.IsStoreLocation == true && x.CompanyBranchId == location.CompanyBranchId && x.LocationId != location.LocationId))
                    {
                        responseOut.message = ActionMessage.DuplicateIsStoreLocation;
                        responseOut.status = ActionStatus.Fail;
                        return responseOut;
                    }
                }

                if (entities.Locations.Any(x => x.LocationName == location.LocationName && x.CompanyBranchId == location.CompanyBranchId && x.LocationId != location.LocationId))
                {
                    responseOut.message = ActionMessage.DuplicateLocation;
                }
                else
                {

                    if (location.LocationId == 0)
                    {

                        location.CreatedDate = DateTime.Now;
                        entities.Locations.Add(location);
                        responseOut.message = ActionMessage.LocationCreatedSuccess;

                    }
                    else
                    {
                        location.Modifiedby = location.CreatedBy;
                        location.ModifiedDate = DateTime.Now;
                        entities.Locations.Where(a => a.LocationId == location.LocationId).ToList().ForEach(a =>
                        {
                            a.LocationCode = location.LocationCode;
                            a.LocationName = location.LocationName;
                            a.IsStoreLocation = location.IsStoreLocation;
                            a.CompanyBranchId = location.CompanyBranchId;
                            a.CompanyId = location.CompanyId;
                            a.Modifiedby = location.Modifiedby;
                            a.ModifiedDate = location.ModifiedDate;
                            a.Status = location.Status;


                        });
                        responseOut.message = ActionMessage.LocationUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;

                }
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

        public List<Location> GetFromLocationList(int companyBranchID)
        {
            List<Location> locationList = new List<Location>();
            try
            {
                var companies = entities.Locations.Where(x => x.CompanyBranchId == companyBranchID && x.Status == true).Select(s => new
                {
                    LocationId = s.LocationId,
                    LocationName = s.LocationName
                }).ToList();
                if (companies != null && companies.Count > 0)
                {
                    foreach (var item in companies)
                    {
                        locationList.Add(new Location { LocationId = item.LocationId, LocationName = item.LocationName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return locationList;
        }
        #endregion

        #region StickyNotes
        public ResponseOut AddEditStickyNotes(StickyNote stickyNote)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                //if (entities.StickyNotes.Any(x => x.UserId == stickyNote.UserId && x.StickyNoteId != stickyNote.StickyNoteId))
                //{
                //    responseOut.status = ActionStatus.Fail;
                //    responseOut.message = ActionMessage.DuplicateStickyNotesName;
                //}
                //else
                //{
                if (stickyNote.StickyNoteId == 0)
                {
                    entities.StickyNotes.Add(stickyNote);
                    responseOut.message = ActionMessage.StickyNotesCreatedSuccess;
                }
                else
                {
                    entities.StickyNotes.Where(a => a.StickyNoteId == stickyNote.StickyNoteId).ToList().ForEach(a =>
                    {
                        a.StickyNoteId = stickyNote.StickyNoteId;
                        a.StickyNoteMessage = stickyNote.StickyNoteMessage;
                        a.UserId = stickyNote.UserId;
                        a.LastModifyDateTime = stickyNote.LastModifyDateTime;
                    });
                    responseOut.message = ActionMessage.StickyNotesUpdatedSuccess;
                }
                entities.SaveChanges();
                responseOut.status = ActionStatus.Success;
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
        #endregion

        #region  Mail Log

        public ResponseOut AddCustomMailLog(MailLog mailLog)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (mailLog.MailLogId == 0)
                {
                    entities.MailLogs.Add(mailLog);
                    //responseOut.message = ActionMessage.MailCreatedSuccess;
                }
                entities.SaveChanges();
                responseOut.status = ActionStatus.Success;

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

        #region Manufacturer
        public ResponseOut AddEditManufacturer(Manufacturer manufacturer)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.Manufacturers.Any(x => x.ManufacturerName == manufacturer.ManufacturerName && x.ManufacturerId != manufacturer.ManufacturerId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateManufacturer;
                }
                else if (entities.Manufacturers.Any(x => x.ManufacturerCode == manufacturer.ManufacturerCode && x.ManufacturerId != manufacturer.ManufacturerId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateManufacturercode;
                }
                else
                {
                    if (manufacturer.ManufacturerId == 0)
                    {
                        manufacturer.CreatedDate = DateTime.Now;
                        entities.Manufacturers.Add(manufacturer);
                        responseOut.message = ActionMessage.ManufacturerCreatedSuccess;
                    }
                    else
                    {
                        entities.Manufacturers.Where(a => a.ManufacturerId == manufacturer.ManufacturerId).ToList().ForEach(a =>
                        {
                            a.ManufacturerId = manufacturer.ManufacturerId;
                            a.ManufacturerName = manufacturer.ManufacturerName;
                            a.ManufacturerCode = manufacturer.ManufacturerCode;
                            a.Modifiedby = manufacturer.CreatedBy;
                            a.ModifiedDate = DateTime.Now;
                            a.Status = manufacturer.Status;
                        });
                        responseOut.message = ActionMessage.ManufacturerUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                }
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
        public List<Manufacturer> GetManufacturerAutoCompleteList(string searchTerm)
        {
            List<Manufacturer> manufacturerList = new List<Manufacturer>();
            try
            {
                var manufacturers = (from man in entities.Manufacturers
                                     where ((man.ManufacturerCode.ToLower().Contains(searchTerm.ToLower()) || man.ManufacturerName.ToLower().Contains(searchTerm.ToLower())) && man.Status == true)
                                     select new
                                     {
                                         ManufacturerId = man.ManufacturerId,
                                         ManufacturerCode = man.ManufacturerCode,
                                         ManufacturerName = man.ManufacturerName
                                     }).ToList();
                if (manufacturers != null && manufacturers.Count > 0)
                {
                    foreach (var item in manufacturers)
                    {
                        manufacturerList.Add(new Manufacturer { ManufacturerId = item.ManufacturerId, ManufacturerCode = item.ManufacturerCode, ManufacturerName = item.ManufacturerName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return manufacturerList;
        }

        public List<Manufacturer> GetManufacturerList()
        {
            List<Manufacturer> manufacturerList = new List<Manufacturer>();
            try
            {
                var manufacturers = entities.Manufacturers.Where(x => x.Status == true).Select(s => new
                {
                    ManufacturerId = s.ManufacturerId,
                    ManufacturerCode = s.ManufacturerCode,
                    ManufacturerName = s.ManufacturerName
                }).ToList();
                if (manufacturers != null && manufacturers.Count > 0)
                {
                    foreach (var item in manufacturers)
                    {
                        manufacturerList.Add(new Manufacturer { ManufacturerId = item.ManufacturerId, ManufacturerCode = item.ManufacturerCode, ManufacturerName = item.ManufacturerName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return manufacturerList;
        }
        #endregion
        #region Size
        public List<Size> GetSizeAutoCompleteList(string searchTerm, int productMainGroupId, int productSubGroupId)
        {
            List<Size> sizeList = new List<Size>();
            try
            {
                var sizes = (from siz in entities.Sizes
                             where ((siz.SizeCode.ToLower().Contains(searchTerm.ToLower()) || siz.SizeDesc.ToLower().Contains(searchTerm.ToLower())) && siz.ProductMainGroupId == productMainGroupId && siz.ProductSubGroupId == productSubGroupId && siz.Status == true)
                             select new
                             {
                                 SizeId = siz.SizeId,
                                 SizeCode = siz.SizeCode,
                                 SizeDesc = siz.SizeDesc
                             }).ToList();
                if (sizes != null && sizes.Count > 0)
                {
                    foreach (var item in sizes)
                    {
                        sizeList.Add(new Size { SizeId = item.SizeId, SizeCode = item.SizeCode, SizeDesc = item.SizeDesc });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return sizeList;
        }
        public List<Size> GetSizeList(int productMainGroupId, int productSubGroupId)
        {
            List<Size> sizeList = new List<Size>();
            try
            {
                var sizes = entities.Sizes.Where(x => x.Status == true && x.ProductMainGroupId == productMainGroupId && x.ProductSubGroupId == productSubGroupId).Select(s => new
                {
                    SizeId = s.SizeId,
                    SizeCode = s.SizeCode,
                    SizeDesc = s.SizeDesc
                }).ToList();
                if (sizes != null && sizes.Count > 0)
                {
                    foreach (var item in sizes)
                    {
                        sizeList.Add(new Size { SizeId = item.SizeId, SizeCode = item.SizeCode, SizeDesc = item.SizeDesc });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return sizeList;
        }

        public ResponseOut AddEditSize(Size size)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.Sizes.Any(x => x.SizeCode == size.SizeCode && x.SizeId != size.SizeId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateSizeCode;
                }

                else if (entities.Sizes.Any(x => x.SizeDesc == size.SizeDesc && x.SizeId != size.SizeId && x.ProductMainGroupId == size.ProductMainGroupId && x.ProductSubGroupId == size.ProductSubGroupId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateSizeDesc;
                }
                else
                {
                    if (size.SizeId == 0)
                    {
                        entities.Sizes.Add(size);
                        responseOut.message = ActionMessage.SizeCreatedSuccess;
                    }
                    else
                    {

                        entities.Sizes.Where(a => a.SizeId == size.SizeId).ToList().ForEach(a =>
                        {
                            a.SizeCode = size.SizeCode;
                            a.SizeDesc = size.SizeDesc;
                            a.ProductMainGroupId = size.ProductMainGroupId;
                            a.ProductSubGroupId = size.ProductSubGroupId;
                            a.Status = size.Status;

                        });
                        responseOut.message = ActionMessage.SizeUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.trnId = size.SizeId;
                    responseOut.status = ActionStatus.Success;

                }
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

        #region Product Serial
        public ResponseOut AddEditProductSerial(ProductSerialDetail productSerialDetail)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.ProductSerialDetails.Any(x => x.Serial1 == productSerialDetail.Serial1 && x.ProductId == productSerialDetail.ProductId && x.ProductSerialId != productSerialDetail.ProductSerialId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateProductSerialName;
                }
                else
                {
                    if (productSerialDetail.ProductSerialId == 0)
                    {
                        entities.ProductSerialDetails.Add(productSerialDetail);
                        responseOut.message = ActionMessage.ProductSerialCreatedSuccess;
                    }
                    else
                    {
                        entities.ProductSerialDetails.Where(a => a.ProductSerialId == productSerialDetail.ProductSerialId).ToList().ForEach(a =>
                        {
                            a.ProductId = productSerialDetail.ProductId;
                            a.ProductSerialNo = productSerialDetail.ProductSerialNo;
                            a.Serial1 = productSerialDetail.Serial1;
                            a.Serial2 = productSerialDetail.Serial2;
                            a.Serial3 = productSerialDetail.Serial3;
                            a.ProductSerialStatus = productSerialDetail.ProductSerialStatus;
                        });
                        responseOut.message = ActionMessage.ProductSerialUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;

                }
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

        #region Approval StoreRequisition
        public ResponseOut ApproveRejectStoreRequisition(StoreRequisition storeRequisition)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (storeRequisition.ApprovalStatus == "Rejected")
                {
                    entities.StoreRequisitions.Where(a => a.RequisitionId == storeRequisition.RequisitionId).ToList().ForEach(a =>
                    {
                        a.ApprovalStatus = storeRequisition.ApprovalStatus;
                        a.RejectedBy = storeRequisition.ApprovedBy;
                        a.RejectedDate = DateTime.Now;
                        a.RejectedReason = storeRequisition.RejectedReason;
                    });
                    responseOut.message = ActionMessage.StoreRequisitionRejectionCreatedSuccess;
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;

                }
                else if (storeRequisition.ApprovalStatus == "Approved")
                {
                    entities.StoreRequisitions.Where(a => a.RequisitionId == storeRequisition.RequisitionId).ToList().ForEach(a =>
                    {
                        a.ApprovalStatus = storeRequisition.ApprovalStatus;
                        a.ApprovedBy = storeRequisition.ApprovedBy;
                        a.ApprovedDate = DateTime.Now;
                    });
                    responseOut.message = ActionMessage.StoreRequisitionApproveUpdatedSuccess;
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                }

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

        #region Approval Purchase Indent
        public ResponseOut ApproveRejectPurchaseIndent(PurchaseIndent purchaseIndent)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (purchaseIndent.ApprovalStatus == "Rejected")
                {
                    entities.PurchaseIndents.Where(a => a.IndentId == purchaseIndent.IndentId).ToList().ForEach(a =>
                    {
                        a.ApprovalStatus = purchaseIndent.ApprovalStatus;
                        a.RejectedBy = purchaseIndent.ApprovedBy;
                        a.RejectedDate = DateTime.Now;
                        a.RejectedReason = purchaseIndent.RejectedReason;
                    });
                    responseOut.message = ActionMessage.PurchaseIndentRejectionCreatedSuccess;
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;

                }
                else if (purchaseIndent.ApprovalStatus == "Approved")
                {
                    entities.PurchaseIndents.Where(a => a.IndentId == purchaseIndent.IndentId).ToList().ForEach(a =>
                    {
                        a.ApprovalStatus = purchaseIndent.ApprovalStatus;
                        a.ApprovedBy = purchaseIndent.ApprovedBy;
                        a.ApprovedDate = DateTime.Now;
                    });
                    responseOut.message = ActionMessage.PurchaseIndentApproveUpdatedSuccess;
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                }

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

        #region Approval PO
        public ResponseOut ApproveRejectPO(PO po)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (po.ApprovalStatus == "Draft")
                {
                    entities.POes.Where(a => a.POId == po.POId).ToList().ForEach(a =>
                    {
                        a.RejectionStatus = po.ApprovalStatus;
                        a.RejectedBy = po.ApprovedBy;
                        a.RejectedDate = DateTime.Now;

                    });
                    responseOut.message = ActionMessage.PORecommendedSuccess;
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;

                }
                else if (po.ApprovalStatus == "Approved")
                {
                    entities.POes.Where(a => a.POId == po.POId).ToList().ForEach(a =>
                    {
                        a.ApprovalStatus = po.ApprovalStatus;
                        a.ApprovedBy = po.ApprovedBy;
                        a.ApprovedDate = DateTime.Now;
                    });
                    responseOut.message = ActionMessage.POUpdatedSuccess;
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                }
                else if (po.ApprovalStatus == "Canceled")
                {
                    entities.POes.Where(a => a.POId == po.POId).ToList().ForEach(a =>
                    {
                        a.CancelBy = po.ApprovedBy;
                        a.CancelReason = po.RejectedReason;
                        a.ApprovalStatus = po.ApprovalStatus;
                        a.CancelDate = DateTime.Now;
                    });

                    //Update purchase indent quantity on PO Cancel--------
                    //entities.proc_UpdateIndentQtyOnCancelPO(po.POId);

                    responseOut.message = ActionMessage.PORejectionCreatedSuccess;
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                }

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

        #region Project
        public ResponseOut AddEditProject(Project project)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                //&& x.CityId!=city.CityId
                if (entities.Projects.Any(x => x.ProjectCode == project.ProjectCode && x.ProjectId != project.ProjectId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateCityName;
                }
                else
                {
                    if (project.ProjectId == 0)
                    {
                        project.CreatedDate = DateTime.Now;
                        entities.Projects.Add(project);
                        responseOut.message = ActionMessage.ProjectCreatedSuccess;
                    }
                    else
                    {
                        project.Modifiedby = project.CreatedBy;
                        project.ModifiedDate = DateTime.Now;
                        entities.Projects.Where(a => a.ProjectId == project.ProjectId).ToList().ForEach(a =>
                        {
                            a.ProjectName = project.ProjectName;
                            a.ProjectCode = project.ProjectCode;
                            a.CustomerId = project.CustomerId;
                            a.CustomerBranchId = project.CustomerBranchId;
                            a.Modifiedby = project.Modifiedby;
                            a.ModifiedDate = project.ModifiedDate;
                            a.ProjectStatus = project.ProjectStatus;
                            a.CompanyBranchId = project.CompanyBranchId;


                        });
                        responseOut.message = ActionMessage.ProjectUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                }
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
        #region PackingList
        public List<PackingListType> GetPackingTypeList()
        {
            List<PackingListType> productPackingTypeList = new List<PackingListType>();
            try
            {
                var productPackingType = entities.PackingListTypes.Where(x => x.PackingListTypeStatus == "Final").Select(s => new
                {
                    PackingListTypeID = s.PackingListTypeID,
                    PackingListTypeName = s.PackingListTypeName
                }).ToList();
                if (productPackingType != null && productPackingType.Count > 0)
                {
                    foreach (var item in productPackingType)
                    {
                        productPackingTypeList.Add(new PackingListType { PackingListTypeID = item.PackingListTypeID, PackingListTypeName = item.PackingListTypeName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return productPackingTypeList;
        }
        #endregion

        #region Chasis Model
        public ResponseOut AddEditChasisModel(ChasisModel chasisModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.ChasisModels.Any(x => x.ChasisModelName == chasisModel.ChasisModelName && x.ChasisModelID != chasisModel.ChasisModelID))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateChasisModelSubGroupName;
                }
                else
                {
                    if (chasisModel.ChasisModelID == 0)
                    {
                        entities.ChasisModels.Add(chasisModel);
                        responseOut.message = ActionMessage.ChasisModelCreatedSuccess;
                    }
                    else
                    {
                        entities.ChasisModels.Where(a => a.ChasisModelID == chasisModel.ChasisModelID).ToList().ForEach(a =>
                        {
                            a.ChasisModelID = chasisModel.ChasisModelID;
                            a.ProductSubGroupId = chasisModel.ProductSubGroupId;
                            a.ChasisModelName = chasisModel.ChasisModelName;
                            a.ChasisModelCode = chasisModel.ChasisModelCode;
                            a.MotorModelCode = chasisModel.MotorModelCode;
                            a.ChasisModelStatus = chasisModel.ChasisModelStatus;
                            a.CompanyBranchId = chasisModel.CompanyBranchId;
                        });
                        responseOut.message = ActionMessage.ChasisModelUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                }
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

        #region Target Type
        public ResponseOut AddEditTargetType(TargetType targettype)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.TargetTypes.Any(x => x.TargetName == targettype.TargetName && x.TargetTypeId != targettype.TargetTypeId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateTargetTypeName;
                }

                else
                {
                    if (targettype.TargetTypeId == 0)
                    {

                        entities.TargetTypes.Add(targettype);
                        responseOut.message = ActionMessage.TargetTypeCreatedSuccess;
                    }
                    else
                    {
                        entities.TargetTypes.Where(a => a.TargetTypeId == targettype.TargetTypeId).ToList().ForEach(a =>
                        {
                            a.TargetName = targettype.TargetName;
                            a.TargetDesc = targettype.TargetDesc;
                            a.Status = targettype.Status;
                            a.CreatedBy = targettype.CreatedBy;
                            a.CreatedDate = targettype.CreatedDate;
                            a.Modifiedby = targettype.Modifiedby;
                            a.ModifiedDate = targettype.ModifiedDate;
                            a.CompanyBranchId = targettype.CompanyBranchId;
                        });
                        responseOut.message = ActionMessage.TargetTypeUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return responseOut;
        }
        public List<TargetType> GetTargetTypeList()
        {
            List<TargetType> targetTypeList = new List<TargetType>();
            try
            {
                var targetTypes = entities.TargetTypes.Where(x => x.Status == true).Select(s => new
                {
                    TargetTypeId = s.TargetTypeId,
                    TargetTypeName = s.TargetName,
                }).ToList();
                if (targetTypes != null && targetTypes.Count > 0)
                {
                    foreach (var item in targetTypes)
                    {
                        targetTypeList.Add(new TargetType { TargetTypeId = item.TargetTypeId, TargetName = item.TargetTypeName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return targetTypeList;
        }
        #endregion

        #region TDS Section
        public ResponseOut AddEditTDSSection(TDSSection tdsSection)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.TDSSections.Any(x => x.SectionName == tdsSection.SectionName && x.SectionDesc != tdsSection.SectionDesc))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateTDSSectionName;
                }

                else
                {
                    if (tdsSection.SectionId == 0)
                    {

                        entities.TDSSections.Add(tdsSection);
                        responseOut.message = ActionMessage.TDSSectionCreatedSuccess;
                    }
                    else
                    {
                        entities.TDSSections.Where(a => a.SectionId == tdsSection.SectionId).ToList().ForEach(a =>
                        {
                            a.SectionName = tdsSection.SectionName;
                            a.SectionDesc = tdsSection.SectionDesc;
                            a.SectionMaxValue = tdsSection.SectionMaxValue;
                            a.Status = tdsSection.Status;
                            a.CompanyBranchId = tdsSection.CompanyBranchId;


                        });
                        responseOut.message = ActionMessage.TDSSectionUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;

                }
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

        #region Employee Pay Info
        public ResponseOut AddEditEmployeePayInfo(EmployeePayInfo employeePayInfo)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.EmployeePayInfoes.Any(x => x.EmployeeId == employeePayInfo.EmployeeId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateEmployeePayInfo;
                }


                else
                {
                    if (employeePayInfo.EmployeeId == 0)
                    {

                        entities.EmployeePayInfoes.Add(employeePayInfo);
                        entities.SaveChanges();
                        responseOut.status = ActionStatus.Success;
                        responseOut.message = ActionMessage.EmployeePayInfoCreatedSuccess;

                    }
                    else
                    {

                        entities.EmployeePayInfoes.Where(a => a.EmployeeId == employeePayInfo.EmployeeId).ToList().ForEach(a =>
                        {
                            a.EmployeeId = employeePayInfo.EmployeeId;
                            a.OTRate = employeePayInfo.OTRate;
                            a.BasicPay = employeePayInfo.BasicPay;
                            a.HRA = employeePayInfo.HRA;
                            a.ConveyanceAllow = employeePayInfo.ConveyanceAllow;
                            a.SpecialAllow = employeePayInfo.SpecialAllow;
                            a.OtherAllow = employeePayInfo.OtherAllow;
                            a.OtherDeduction = employeePayInfo.OtherDeduction;
                            a.MedicalAllow = employeePayInfo.MedicalAllow;
                            a.ChildEduAllow = employeePayInfo.ChildEduAllow;
                            a.EmployeePF = employeePayInfo.EmployeePF;
                            a.EmployeeESI = employeePayInfo.EmployeeESI;
                            a.EmployerPF = employeePayInfo.EmployerPF;
                            a.EmployerESI = employeePayInfo.EmployerESI;
                            a.LTA = employeePayInfo.LTA;
                            a.ProfessinalTax = employeePayInfo.ProfessinalTax;


                        });
                        responseOut.message = ActionMessage.EmployeePayInfoUpdatedSuccess;

                        entities.SaveChanges();
                        responseOut.status = ActionStatus.Success;

                    }
                }
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
        public ResponseOut AddEditEmployeePayInfoForUploadUtility(EmployeePayInfo employeePayInfo)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.EmployeePayInfoes.Any(x => x.EmployeeId == employeePayInfo.EmployeeId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateEmployeePayInfo;
                }

                else
                {
                    if (employeePayInfo.EmployeeId == 0)
                    {

                        entities.EmployeePayInfoes.Add(employeePayInfo);
                        entities.SaveChanges();
                        responseOut.status = ActionStatus.Success;
                        responseOut.message = ActionMessage.EmployeePayInfoCreatedSuccess;

                    }
                    else
                    {

                        entities.EmployeePayInfoes.Where(a => a.EmployeeId == employeePayInfo.EmployeeId).ToList().ForEach(a =>
                        {
                            a.EmployeeId = employeePayInfo.EmployeeId;
                            a.OTRate = employeePayInfo.OTRate;
                            a.BasicPay = employeePayInfo.BasicPay;
                            a.HRA = employeePayInfo.HRA;
                            a.ConveyanceAllow = employeePayInfo.ConveyanceAllow;
                            a.SpecialAllow = employeePayInfo.SpecialAllow;
                            a.OtherAllow = employeePayInfo.OtherAllow;
                            a.OtherDeduction = employeePayInfo.OtherDeduction;
                            a.MedicalAllow = employeePayInfo.MedicalAllow;
                            a.ChildEduAllow = employeePayInfo.ChildEduAllow;
                            a.EmployeePF = employeePayInfo.EmployeePF;
                            a.EmployeeESI = employeePayInfo.EmployeeESI;
                            a.EmployerPF = employeePayInfo.EmployerPF;
                            a.EmployerESI = employeePayInfo.EmployerESI;
                            a.LTA = employeePayInfo.LTA;
                            a.ProfessinalTax = employeePayInfo.ProfessinalTax;
                        });
                        responseOut.message = ActionMessage.EmployeePayInfoUpdatedSuccess;

                        entities.SaveChanges();
                        responseOut.status = ActionStatus.Success;

                    }
                }
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

        #region Sale Target
        public List<City> GetCityAutoCompleteList(string searchTerm, int stateId)
        {

            List<City> cityList = new List<City>();
            try
            {
                var cities = (from city in entities.Cities
                              where ((city.CityName.ToLower().StartsWith(searchTerm.ToLower())) && city.Status == true && city.StateId == stateId)
                              select new
                              {
                                  CityId = city.CityId,
                                  CityName = city.CityName,
                                  Vehicles = city.Vehicles,
                                  PerDealar = city.PerDealar,
                                  DealershipsNos = city.DealershipsNos,
                              }).ToList();

                if (cities != null && cities.Count > 0)
                {
                    foreach (var item in cities)
                    {

                        cityList.Add(new City
                        {
                            CityId = item.CityId,
                            CityName = item.CityName,
                            Vehicles = item.Vehicles,
                            PerDealar = item.PerDealar,
                            DealershipsNos = item.DealershipsNos
                        });

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return cityList;
        }
        #endregion


        #region Complaint Service
        public List<Customer> GetCustomerMobileAutoCompleteList(string searchTerm, int companyId)
        {
            List<Customer> customerList = new List<Customer>();
            try
            {

                var customers = (from cust in entities.Customers
                                 where ((cust.MobileNo.ToLower().Contains(searchTerm.ToLower())) && cust.CompanyId == companyId && cust.Status == true)
                                 select new
                                 {
                                     MobileNo = cust.MobileNo,
                                     CustomerId = cust.CustomerId,
                                     CustomerName = cust.CustomerName,
                                     Email = cust.Email,
                                     PrimaryAddress = cust.PrimaryAddress,

                                 }).ToList();
                if (customers != null && customers.Count > 0)
                {
                    foreach (var item in customers)
                    {

                        customerList.Add(new Customer
                        {
                            MobileNo = item.MobileNo,
                            CustomerId = item.CustomerId,
                            CustomerName = item.CustomerName,
                            Email = item.Email,
                            PrimaryAddress = item.PrimaryAddress,

                        });

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return customerList;
        }

        public List<Product> GetComplaintProductAutoCompleteList(string searchTerm, int companyId)
        {
            List<Product> productList = new List<Product>();
            try
            {
                var products = (from p in entities.Products
                                where (p.ProductName.ToLower().Contains(searchTerm.ToLower())) && p.CompanyId == companyId && p.Status == true
                                select new
                                {
                                    ProductId = p.Productid,
                                    ProductName = p.ProductName,
                                    ProductCode = p.ProductCode,

                                }).ToList();


                if (products != null && products.Count > 0)
                {
                    foreach (var item in products)
                    {
                        productList.Add(new Product
                        {
                            Productid = item.ProductId,
                            ProductName = item.ProductName,
                            ProductCode = item.ProductCode,

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return productList;
        }
        #endregion


       

        public ResponseOut AddEditDashboardContainer(DashboardContainer dashboardContainer)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.DashboardContainers.Any(x => x.ContainerName == dashboardContainer.ContainerName && x.DashboardContainterID != dashboardContainer.DashboardContainterID && x.ModuleName == dashboardContainer.ModuleName))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateDashboardContainerName;
                }
                else if (entities.DashboardContainers.Any(x => x.ContainerDisplayName == dashboardContainer.ContainerDisplayName && x.DashboardContainterID != dashboardContainer.DashboardContainterID && x.ModuleName == dashboardContainer.ModuleName))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateDashboardContainerDisplayName;
                }
                else if (entities.DashboardContainers.Any(x => x.ContainterNo == dashboardContainer.ContainterNo && x.ModuleName == dashboardContainer.ModuleName && x.DashboardContainterID != dashboardContainer.DashboardContainterID && x.ModuleName == dashboardContainer.ModuleName) )
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateDashboardContainerNo;
                }

                else
                {
                    if (dashboardContainer.DashboardContainterID == 0)
                    {
                        //dashboardContainer.CreatedDate = DateTime.Now;
                        entities.DashboardContainers.Add(dashboardContainer);
                        responseOut.message = ActionMessage.DashboardContainerCreatedSuccess;
                    }
                    else
                    {
                        //dashboardContainer.Modifiedby = book.CreatedBy;
                        //dashboardContainer.ModifiedDate = DateTime.Now;

                        entities.DashboardContainers.Where(a => a.DashboardContainterID == dashboardContainer.DashboardContainterID).ToList().ForEach(a =>
                      {
                          a.DashboardContainterID = dashboardContainer.DashboardContainterID;
                          a.ContainerName = dashboardContainer.ContainerName;
                          a.ContainerDisplayName = dashboardContainer.ContainerDisplayName;
                          a.ContainterNo = dashboardContainer.ContainterNo;
                          a.TotalItem = dashboardContainer.TotalItem;
                          a.ModuleName = dashboardContainer.ModuleName;
                      });
                        responseOut.message = ActionMessage.BookUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                    responseOut.trnId = dashboardContainer.DashboardContainterID;

                }
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








       

        public ResponseOut AddEditDashDashboardItemMapping(DashboardItemMapping dashboardItemMapping)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.DashboardItemMappings.Any(x => x.RoleId == dashboardItemMapping.RoleId && x.DashboardItemId == dashboardItemMapping.DashboardItemId && x.ModuleName == dashboardItemMapping.ModuleName && x.ContainerID == dashboardItemMapping.ContainerID && x.CompanyBranchId == dashboardItemMapping.CompanyBranchId))
                {
                    entities.DashboardItemMappings.Where(a => a.RoleId == dashboardItemMapping.RoleId && a.DashboardItemId == dashboardItemMapping.DashboardItemId && a.ModuleName == dashboardItemMapping.ModuleName && a.ContainerID == dashboardItemMapping.ContainerID && a.CompanyBranchId == dashboardItemMapping.CompanyBranchId).ToList().ForEach(a =>
                      {
                        //a.st= roleUIActionMapping.AddAccess;
                        //a.EditAccess = roleUIActionMapping.EditAccess;
                        //a.ViewAccess = roleUIActionMapping.ViewAccess;
                        //a.CancelAccess = roleUIActionMapping.CancelAccess;
                        //a.ReviseAccess = roleUIActionMapping.ReviseAccess;
                        //a.Status = roleUIActionMapping.Status;
                        //a.CompanyBranchId = roleUIActionMapping.CompanyBranchId;

                    });
                }
                else
                {
                    entities.DashboardItemMappings.Add(dashboardItemMapping);
                }

                entities.SaveChanges();
                responseOut.message = ActionMessage.RoleDashboardMappingSuccessful;
                responseOut.status = ActionStatus.Success;
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



        public ResponseOut DeleteDashDashboardItemMapping(DashboardItemMapping dashboardItemMapping)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                var itemToRemove = entities.DashboardItemMappings.SingleOrDefault(x => x.RoleId == dashboardItemMapping.RoleId && x.DashboardItemId == dashboardItemMapping.DashboardItemId && x.ModuleName == dashboardItemMapping.ModuleName  && x.CompanyBranchId == dashboardItemMapping.CompanyBranchId);

                if (itemToRemove != null)
                {
                    entities.DashboardItemMappings.Remove(itemToRemove);
                    entities.SaveChanges();
                }


                responseOut.message = ActionMessage.RoleUIMappingSuccessful;
                responseOut.status = ActionStatus.Success;
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


        #region HSN
        public ResponseOut AddEditHSN(HSNCode hSNCode)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.HSNCodes.Any(x => x.HSNCode1 ==hSNCode.HSNCode1 && x.HSNID != hSNCode.HSNID))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.HSNDublicate;
                }
               
                else
                {
                    if (hSNCode.HSNID == 0)
                    {
                       
                        entities.HSNCodes.Add(hSNCode);
                        responseOut.message = ActionMessage.HSNCreatedSuccess;
                    }
                    else
                    {
                        entities.HSNCodes.Where(a => a.HSNID == hSNCode.HSNID).ToList().ForEach(a =>
                        {
                            a.HSNCode1 = hSNCode.HSNCode1;
                            a.CGST_Perc = hSNCode.CGST_Perc;
                            a.SGST_Perc = hSNCode.SGST_Perc;
                            a.IGST_Perc =hSNCode.IGST_Perc;
                            a.Status = hSNCode.Status;
                        });
                        responseOut.message = ActionMessage.HSNUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                }
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
        public List<HSNCode> GetHSNAutoCompleteList(string searchTerm)
        {
            List<HSNCode> hSNList = new List<HSNCode>();
            try
            {
                var hsns = (from man in entities.HSNCodes
                                     where ((man.HSNCode1.ToLower().Contains(searchTerm.ToLower())) && man.Status == true)
                                     select new
                                     {
                                         HSNID = man.HSNID,
                                         HSNCode1 = man.HSNCode1,
                                         CGST_Perc = man.CGST_Perc,
                                         SGST_Perc = man.SGST_Perc,
                                         IGST_Perc = man.IGST_Perc
                                     }).ToList();
                if (hsns != null && hsns.Count > 0)
                {
                    foreach (var item in hsns)
                    {
                        hSNList.Add(new HSNCode
                        {
                            HSNID = item.HSNID,
                            HSNCode1 = item.HSNCode1,
                            CGST_Perc = item.CGST_Perc,
                            SGST_Perc = item.SGST_Perc,
                            IGST_Perc = item.IGST_Perc
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return hSNList;
        }

       
        #endregion

    }



}