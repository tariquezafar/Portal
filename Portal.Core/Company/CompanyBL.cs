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

namespace Portal.Core
{
    public class CompanyBL
    {
        DBInterface dbInterface;
        public CompanyBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditCompany(CompanyViewModel companyViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                Company company = new Company
                {
                    CompanyId=companyViewModel.CompanyId,
                    CompanyName = companyViewModel.CompanyName,
                    ContactPersonName = companyViewModel.ContactPersonName,
                    Phone = companyViewModel.Phone,
                    Email = companyViewModel.Email,
                    Fax = companyViewModel.Fax,
                    Logo = companyViewModel.Logo,
                    Website = companyViewModel.Website,
                    Address = companyViewModel.Address,
                    City = companyViewModel.City,
                    State = companyViewModel.State == 0 ? null : companyViewModel.State,
                    CountryId = companyViewModel.CountryId == 0 ? null : companyViewModel.CountryId,
                    ZipCode = companyViewModel.ZipCode,
                    CompanyDesc = companyViewModel.CompanyDesc,
                    PANNo = companyViewModel.PANNo,
                    TINNo = companyViewModel.TINNo,
                    TanNo = companyViewModel.TanNo,
                    ServiceTaxNo = companyViewModel.ServiceTaxNo, 
                    CompanyCode = companyViewModel.CompanyCode,
                    AnnualTurnover = companyViewModel.AnnualTurnover,
                };
                responseOut = dbInterface.AddEditCompany(company);
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


        public ResponseOut AddEditCompanyBranch(CompanyBranchViewModel companyBranchViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                ComapnyBranch comapnyBranch = new ComapnyBranch               
                {                 
                    CompanyBranchId = companyBranchViewModel.CompanyBranchId,
                    BranchName = companyBranchViewModel.BranchName,
                    ContactPersonName = companyBranchViewModel.ContactPersonName,
                    Designation = companyBranchViewModel.Designation,
                    Email = companyBranchViewModel.Email,
                    MobileNo = companyBranchViewModel.MobileNo,
                    ContactNo = companyBranchViewModel.ContactNo,
                    Fax = companyBranchViewModel.Fax,
                    PrimaryAddress = companyBranchViewModel.PrimaryAddress,
                    City = companyBranchViewModel.City,
                    StateId = companyBranchViewModel.StateId,
                    CountryId = companyBranchViewModel.CountryId,
                    PinCode = companyBranchViewModel.PinCode,
                    CSTNo = companyBranchViewModel.CSTNo,
                    TINNo = companyBranchViewModel.TINNo,
                    PANNo = companyBranchViewModel.PANNo, 
                    GSTNo = companyBranchViewModel.GSTNo,
                    AnnualTurnover = companyBranchViewModel.AnnualTurnover,
                    CompanyId = companyBranchViewModel.CompanyId,
                    Status = companyBranchViewModel.CompanyBranch_Status,
                    CompanyBranchCode= companyBranchViewModel.CompanyBranchCode,
                    ManufactorLocationCode= companyBranchViewModel.ManufactorLocationCode

                };
                
                    responseOut = dbInterface.AddEditCompanyBranch(comapnyBranch);
               
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

        public List<CompanyViewModel> GetCompanyList(string companyName = "", string cityName = "", string panNo = "", string phoneNo = "", string tinNo = "")
        {
            List<CompanyViewModel> companies = new List<CompanyViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCompanies = sqlDbInterface.GetCompanyList(companyName, cityName, panNo, phoneNo, tinNo);
                if (dtCompanies!=null && dtCompanies.Rows.Count>0)
                {
                    foreach (DataRow dr in dtCompanies.Rows)
                    {
                        companies.Add(new CompanyViewModel
                        {
                            CompanyId =Convert.ToInt32(dr["CompanyId"]),
                            CompanyName=Convert.ToString(dr["CompanyName"]),
                            ContactPersonName = Convert.ToString(dr["ContactPersonName"]),
                            Phone = Convert.ToString(dr["Phone"]),
                            Email = Convert.ToString(dr["Email"]),
                            Fax = Convert.ToString(dr["Fax"]),
                            Logo = Convert.ToString(dr["Logo"]),
                            Website = Convert.ToString(dr["Website"]),
                            Address = Convert.ToString(dr["Address"]),
                            City = Convert.ToString(dr["City"]),
                            State = dr["State"]==DBNull.Value?0:Convert.ToInt16(dr["State"]) ,
                            StateName = Convert.ToString(dr["StateName"]),
                            CountryId = dr["CountryId"] == DBNull.Value ? 0 : Convert.ToInt16(dr["CountryId"]),
                            CountryName = Convert.ToString(dr["CountryName"]),
                            ZipCode = Convert.ToString(dr["ZipCode"]),
                            CompanyDesc = Convert.ToString(dr["CompanyDesc"]),
                            PANNo = Convert.ToString(dr["PANNo"]),
                            TINNo = Convert.ToString(dr["TINNo"]),
                            TanNo = Convert.ToString(dr["TanNo"]),
                            ServiceTaxNo = Convert.ToString(dr["ServiceTaxNo"]), 
                            CompanyCode = Convert.ToString(dr["CompanyCode"]),
                            AnnualTurnover = Convert.ToDecimal(dr["AnnualTurnover"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return companies;
        }
        public List<CompanyViewModel> GetCompanyList()
        {
            List<CompanyViewModel> companies = new List<CompanyViewModel>();
            try
            {
                List<Company> companyList = dbInterface.GetCompanyList();
                if (companyList != null && companyList.Count > 0)
                {
                    foreach (Company company in companyList)
                    {
                        companies.Add(new CompanyViewModel {CompanyId = company.CompanyId,CompanyName = company.CompanyName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return companies;
        }
        public CompanyViewModel GetCompanyDetail(int companyId = 0)
        {
            CompanyViewModel company = new CompanyViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCompanies = sqlDbInterface.GetCompanyDetail(companyId);
                if (dtCompanies != null && dtCompanies.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCompanies.Rows)
                    {
                        company=new CompanyViewModel
                        {
                            CompanyId = Convert.ToInt32(dr["CompanyId"]),
                            CompanyName = Convert.ToString(dr["CompanyName"]),
                            ContactPersonName = Convert.ToString(dr["ContactPersonName"]),
                            Phone = Convert.ToString(dr["Phone"]),
                            Email = Convert.ToString(dr["Email"]),
                            Fax = Convert.ToString(dr["Fax"]),
                            Logo = Convert.ToString(dr["Logo"]),
                            Website = Convert.ToString(dr["Website"]),
                            Address = Convert.ToString(dr["Address"]),
                            City = Convert.ToString(dr["City"]),
                            State = dr["State"] == DBNull.Value ? 0 : Convert.ToInt16(dr["State"]),
                            StateName = Convert.ToString(dr["StateName"]),
                            CountryId = dr["CountryId"] == DBNull.Value ? 0 : Convert.ToInt16(dr["CountryId"]),
                            CountryName = Convert.ToString(dr["CountryName"]),
                            ZipCode = Convert.ToString(dr["ZipCode"]),
                            CompanyDesc = Convert.ToString(dr["CompanyDesc"]),
                            PANNo = Convert.ToString(dr["PANNo"]),
                            TINNo = Convert.ToString(dr["TINNo"]),
                            TanNo = Convert.ToString(dr["TanNo"]),
                            ServiceTaxNo = Convert.ToString(dr["ServiceTaxNo"]),
                            AnnualTurnover = Convert.ToDecimal(dr["AnnualTurnover"]),
                            CompanyCode = Convert.ToString(dr["CompanyCode"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return company;
        }


        public List<CompanyBranchViewModel> GetCompanyBranchTypeList(int companyId = 0,int companyBranchId=0)
        {
            List<CompanyBranchViewModel> companyBranchViewModel = new List<CompanyBranchViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCompanies = sqlDbInterface.GetCompanyBranchTypeList(companyBranchId,companyId);
                if (dtCompanies != null && dtCompanies.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCompanies.Rows)
                    {
                        companyBranchViewModel.Add(new CompanyBranchViewModel
                        {


                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                            BranchType= Convert.ToString(dr["BranchType"])
                           

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return companyBranchViewModel;
        }

        public List<CompanyBranchViewModel> GetCompanyBranchList(string BranchName = "", string cityName = "", string panNo = "", string phoneNo = "", string tinNo = "", string companyBranchCode="",string manufactorLocationCode="")
        {          
            List<CompanyBranchViewModel> companyBranchViewModel = new List<CompanyBranchViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCompanies = sqlDbInterface.GetCompanyBranchList(BranchName, cityName, panNo, phoneNo, tinNo,companyBranchCode, manufactorLocationCode);
                if (dtCompanies != null && dtCompanies.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCompanies.Rows)
                    {
                        companyBranchViewModel.Add(new CompanyBranchViewModel
                        {
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                            BranchName = Convert.ToString(dr["BranchName"]),
                            ContactPersonName = Convert.ToString(dr["ContactPersonName"]),
                            MobileNo = Convert.ToString(dr["MobileNo"]),
                            ContactNo = Convert.ToString(dr["ContactNo"]),
                            Email = Convert.ToString(dr["Email"]),
                            Fax = Convert.ToString(dr["Fax"]),
                            PrimaryAddress = Convert.ToString(dr["PrimaryAddress"]),
                            City = Convert.ToString(dr["City"]),
                            StateId = dr["StateId"] == DBNull.Value ? 0 : Convert.ToInt16(dr["StateId"]),
                            StateName = Convert.ToString(dr["StateName"]),
                            CountryId = dr["CountryId"] == DBNull.Value ? 0 : Convert.ToInt16(dr["CountryId"]),
                            CountryName = Convert.ToString(dr["CountryName"]),
                            CSTNo = Convert.ToString(dr["CSTNo"]),
                            GSTNo = Convert.ToString(dr["GSTNo"]),
                            PANNo = Convert.ToString(dr["PANNo"]),
                            TINNo = Convert.ToString(dr["TINNo"]),
                            AnnualTurnover = Convert.ToDecimal(dr["AnnualTurnover"]),
                            Designation = Convert.ToString(dr["Designation"]),
                            CompanyBranch_Status = Convert.ToBoolean(dr["Status"]),
                            CompanyBranchCode=Convert.ToString(dr["CompanyBranchCode"]),
                            ManufactorLocationCode = Convert.ToString(dr["ManufactorLocationCode"])

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return companyBranchViewModel;
        }
        public List<CompanyBranchViewModel> GetCompanyBranchList(int companyId)
        {
            List<CompanyBranchViewModel> companyBranches = new List<CompanyBranchViewModel>();
            try
            {
                List<ComapnyBranch> companyBranchList = dbInterface.GetCompanyBranchList(companyId);
                if (companyBranchList != null && companyBranchList.Count > 0)
                {
                    foreach (ComapnyBranch companyBranch in companyBranchList)
                    {
                        companyBranches.Add(new CompanyBranchViewModel { CompanyBranchId = companyBranch.CompanyBranchId, BranchName = companyBranch.BranchName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return companyBranches;
        }
        public CompanyBranchViewModel GetCompanyBracnhDetail(int companyBranchId = 0)
        {
            CompanyBranchViewModel companyBranchViewModel = new CompanyBranchViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCompanies = sqlDbInterface.GetCompanyBranchDetail(companyBranchId);
                if (dtCompanies != null && dtCompanies.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCompanies.Rows)
                    {
                        companyBranchViewModel = new CompanyBranchViewModel
                        {
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                            BranchName = Convert.ToString(dr["BranchName"]),
                            ContactPersonName = Convert.ToString(dr["ContactPersonName"]),
                            MobileNo = Convert.ToString(dr["MobileNo"]),
                            ContactNo = Convert.ToString(dr["ContactNo"]),
                            Email = Convert.ToString(dr["Email"]),
                            Fax = Convert.ToString(dr["Fax"]),
                            PrimaryAddress = Convert.ToString(dr["PrimaryAddress"]),
                            City = Convert.ToString(dr["City"]),
                            StateId = dr["StateId"] == DBNull.Value ? 0 : Convert.ToInt16(dr["StateId"]),
                            StateName = Convert.ToString(dr["StateName"]),
                            CountryId = dr["CountryId"] == DBNull.Value ? 0 : Convert.ToInt16(dr["CountryId"]),
                            CountryName = Convert.ToString(dr["CountryName"]),
                            PinCode = Convert.ToString(dr["PinCode"]),
                            CSTNo = Convert.ToString(dr["CSTNo"]),
                            GSTNo = Convert.ToString(dr["GSTNo"]),
                            PANNo = Convert.ToString(dr["PANNo"]),
                            TINNo = Convert.ToString(dr["TINNo"]),
                            AnnualTurnover = Convert.ToDecimal(dr["AnnualTurnover"]),
                            Designation = Convert.ToString(dr["Designation"]),
                            CompanyBranch_Status = Convert.ToBoolean(dr["Status"]),
                            CompanyBranchCode = Convert.ToString(dr["CompanyBranchCode"]),
                            ManufactorLocationCode= Convert.ToString(dr["ManufactorLocationCode"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return companyBranchViewModel;
        }
        public CompanyBranchViewModel GetCompanyBranchByStateIdList(int companyBranchID)
        {
            CompanyBranchViewModel companyBranches = new CompanyBranchViewModel();
            try
            {
                ComapnyBranch companyBranchList = dbInterface.GetCompanyBranchByStateIdList(companyBranchID);
                if (companyBranchList != null)
                {
                    companyBranches = new CompanyBranchViewModel
                    {
                        StateId = companyBranchList.StateId,
                        FreightCGST_Perc = Convert.ToDecimal(companyBranchList.FreightCGST_Perc),
                        FreightSGST_Perc = Convert.ToDecimal(companyBranchList.FreightSGST_Perc),
                        FreightIGST_Perc = Convert.ToDecimal(companyBranchList.FreightIGST_Perc),
                        LoadingCGST_Perc = Convert.ToDecimal(companyBranchList.LoadingCGST_Perc),
                        LoadingSGST_Perc = Convert.ToDecimal(companyBranchList.LoadingSGST_Perc),
                        LoadingIGST_Perc = Convert.ToDecimal(companyBranchList.LoadingIGST_Perc),
                        InsuranceCGST_Perc = Convert.ToDecimal(companyBranchList.InsuranceCGST_Perc),
                        InsuranceSGST_Perc = Convert.ToDecimal(companyBranchList.InsuranceSGST_Perc),
                        InsuranceIGST_Perc = Convert.ToDecimal(companyBranchList.InsuranceIGST_Perc)
                    };

                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return companyBranches;
        }

        public CompanyBranchViewModel GetCompanyBranchDetails(int companyBranchID = 0)
        {
            CompanyBranchViewModel companyBranches = new CompanyBranchViewModel();
            try
            {
                ComapnyBranch companyBranch = dbInterface.GetCompanyBranchDetails(companyBranchID);
                if (companyBranch != null)
                {

                    companyBranches = new CompanyBranchViewModel
                    {
                        StateId = companyBranch.StateId,
                        FreightCGST_Perc = Convert.ToDecimal(companyBranch.FreightCGST_Perc),
                        FreightSGST_Perc = Convert.ToDecimal(companyBranch.FreightSGST_Perc),
                        FreightIGST_Perc = Convert.ToDecimal(companyBranch.FreightIGST_Perc),
                        LoadingCGST_Perc = Convert.ToDecimal(companyBranch.LoadingCGST_Perc),
                        LoadingSGST_Perc = Convert.ToDecimal(companyBranch.LoadingSGST_Perc),
                        LoadingIGST_Perc = Convert.ToDecimal(companyBranch.LoadingIGST_Perc),
                        InsuranceCGST_Perc = Convert.ToDecimal(companyBranch.InsuranceCGST_Perc),
                        InsuranceSGST_Perc = Convert.ToDecimal(companyBranch.InsuranceSGST_Perc),
                        InsuranceIGST_Perc = Convert.ToDecimal(companyBranch.InsuranceIGST_Perc),
                    };
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return companyBranches;
        }

       public List<CompanyBranchViewModel> GetCompanyBranchLists(int companyBranchID)
          {
            List<CompanyBranchViewModel> branchViewModel = new List<CompanyBranchViewModel>();
            try
            {
                List<ComapnyBranch> BranchList = dbInterface.GetCompanyBranchsList(companyBranchID);
                if (BranchList != null && BranchList.Count > 0)
                {
                    foreach (ComapnyBranch location in BranchList)
                    {
                        branchViewModel.Add(new CompanyBranchViewModel { CompanyBranchId = location.CompanyBranchId, BranchName = location.BranchName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return branchViewModel;
        }
       
    }
}
