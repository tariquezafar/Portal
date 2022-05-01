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
  public  class CityBL
    {
        DBInterface dbInterface;
        public CityBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditCity(CityViewModel cityViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {

                City city = new City {

                    CityId = cityViewModel.CityId,
                    CityName=cityViewModel.CityName,
                    StateId = cityViewModel.StateId,
                    Population= cityViewModel.Population,
                    Area= cityViewModel.Area,
                    Railway = cityViewModel.Railway,
                    Airport = cityViewModel.Airport,
                    PointOfInterest = cityViewModel.PointOfInterest,
                    CityClass= cityViewModel.CityClass,
                    PopFactor = cityViewModel.PopFactor,
                    EffectivePopulation= cityViewModel.EffectivePopulation,
                    Vehicles = cityViewModel.Vehicles,
                    PerDealar =cityViewModel.PerDealar,
                    DealershipsNos= cityViewModel.DealershipsNos,
                    Status = cityViewModel.CityStatus 

                };
                responseOut = dbInterface.AddEditCity(city);
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

        public List<CityViewModel> GetCityList(int stateId = 0)
        {
            List<CityViewModel> cityList = new List<CityViewModel>();
            try
            {
                List<Portal.DAL.City> cities = dbInterface.GetCityList(stateId);
                if (cities != null && cities.Count > 0)
                {
                    foreach (Portal.DAL.City city in cities)
                    {
                        cityList.Add(new CityViewModel {CityId = city.CityId,CityName=city.CityName });
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
        public List<CityViewModel> GetCityList(string cityName = "",int stateId = 0,int countryId=0)
        {
            List<CityViewModel> cities = new List<CityViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                
                DataTable dtCities = sqlDbInterface.GetCityList(cityName,stateId,countryId);
                if (dtCities != null && dtCities.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCities.Rows)
                    {

                        cities.Add(new CityViewModel {
                            CityId = Convert.ToInt32(dr["CityId"]),
                            CityName = Convert.ToString(dr["CityName"]),
                            CityStatus= Convert.ToBoolean(dr["Status"]),
                            StateId = dr["Stateid"] == DBNull.Value ? 0 : Convert.ToInt16(dr["Stateid"]),
                            StateName = Convert.ToString(dr["StateName"]),
                            CountryName=Convert.ToString(dr["CountryName"]),
                            Population= Convert.ToDecimal(dr["Population"]),
                            Area = Convert.ToDecimal(dr["Area"]),
                            Railway= Convert.ToInt32(dr["Railway"]),
                            Airport= Convert.ToInt32(dr["Airport"]),
                            PointOfInterest= Convert.ToInt32(dr["PointOfInterest"]),
                            CityClass = Convert.ToString(dr["CityClass"]),
                            PopFactor= Convert.ToDecimal(dr["PopFactor"]),
                            EffectivePopulation= Convert.ToDecimal(dr["EffectivePopulation"]),
                            Vehicles=Convert.ToInt32(dr["Vehicles"]),
                            PerDealar = Convert.ToInt32(dr["PerDealar"]),
                            DealershipsNos= Convert.ToInt32(dr["DealershipsNos"]),


                        });
                       
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return cities;
        }

        public CityViewModel GetCityDetail(int cityId = 0)
        {
           
            CityViewModel city = new CityViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtcities = sqlDbInterface.GetCityDetail(cityId);
                if (dtcities != null && dtcities.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtcities.Rows)
                    {
                        city = new CityViewModel
                        {
                            CityId = Convert.ToInt32(dr["CityId"]),
                            CityName = Convert.ToString(dr["CityName"]),
                            Population= Convert.ToDecimal(dr["Population"]),
                            Area = Convert.ToDecimal(dr["Area"]),
                            Railway= Convert.ToInt32(dr["Railway"]),
                            Airport = Convert.ToInt32(dr["Airport"]),
                            PointOfInterest= Convert.ToInt32(dr["PointOfInterest"]),
                            CityClass = Convert.ToString(dr["CityClass"]),
                            PopFactor = Convert.ToDecimal(dr["PopFactor"]),
                            EffectivePopulation = Convert.ToDecimal(dr["EffectivePopulation"]),
                            Vehicles = Convert.ToInt32(dr["Vehicles"]),
                            PerDealar = Convert.ToInt32(dr["PerDealar"]),
                            DealershipsNos = Convert.ToInt32(dr["DealershipsNos"]),
                            CityStatus = Convert.ToBoolean(dr["Status"]),
                            StateId = dr["Stateid"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Stateid"]),
                            StateName = Convert.ToString(dr["StateName"]),
                            StateCode = Convert.ToString(dr["StateCode"]),
                            CountryId =dr["CountryId"]==DBNull.Value?0: Convert.ToInt32(dr["CountryId"])
                            
                        };

                       
                    }
                }
            
                
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return city;
        }



    }
}
