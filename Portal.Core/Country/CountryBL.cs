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
    public class CountryBL
    {
        DBInterface dbInterface;
        public CountryBL()
        {
            dbInterface = new DBInterface();
        }
        public List<CountryViewModel> GetCountryList()
        {
            List<CountryViewModel> countryList = new List<CountryViewModel>();
            try
            {
              List<Portal.DAL.Country> countries = dbInterface.GetCountryList();
                if (countries!=null && countries.Count>0)
                {
                    foreach(Portal.DAL.Country country in countries)
                    {
                        countryList.Add(new CountryViewModel { CountryId = country.CountryId, CountryCode = country.CountryCode, CountryName = country.CountryName });
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
        public ResponseOut AddEditCountry(CountryViewModel countryViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                Country country = new Country
                {
                    CountryId = countryViewModel.CountryId,
                    CountryName = countryViewModel.CountryName,
                    CountryCode = countryViewModel.CountryCode,
                    Status = countryViewModel.Country_Status
                };
                responseOut = dbInterface.AddEditCountry(country);
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

        public List<CountryViewModel> GetCountryList(string countryName = "", string countryCode = "", string Status = "")
        {
            List<CountryViewModel> countries = new List<CountryViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
               DataTable dtCountries = sqlDbInterface.GetCountryList(countryName, countryCode, Status);
                if (dtCountries != null && dtCountries.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCountries.Rows)
                    {
                        countries.Add(new CountryViewModel
                        {
                            CountryId = Convert.ToInt32(dr["CountryId"]),
                            CountryName = Convert.ToString(dr["CountryName"]),
                            CountryCode = Convert.ToString(dr["CountryCode"]),
                            Country_Status = Convert.ToBoolean(dr["Status"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return countries;
        }
         
        public CountryViewModel GetCountryDetail(int countryId = 0)
        {
            CountryViewModel country = new CountryViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCountries= sqlDbInterface.GetCountryDetail(countryId);
                if (dtCountries != null && dtCountries.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCountries.Rows)
                    {
                        country = new CountryViewModel
                        {
                            //CountryId = Convert.ToInt32(dr["CompanyId"]),
                            CountryName = Convert.ToString(dr["CountryName"]),
                            CountryCode = Convert.ToString(dr["CountryCode"]),
                            Country_Status = Convert.ToBoolean(dr["Status"]), 
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return country;
        }



    }
}
