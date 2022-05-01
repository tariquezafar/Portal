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
    public class StateBL
    {
        DBInterface dbInterface;
        public StateBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditState (StateViewModel stateViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                State state = new State
                {
                    Stateid= stateViewModel.StateId,
                    StateName = stateViewModel.StateName,
                    StateCode = stateViewModel.StateCode,
                    CountryId = stateViewModel.CountryId,
              
                };
                responseOut = dbInterface.AddEditState(state);
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

        public List<StateViewModel> GetStateList(int countryId = 0)
        {
            List<StateViewModel> stateList = new List<StateViewModel>();
            try
            {
                List<Portal.DAL.State> states = dbInterface.GetStateList(countryId);
                if (states != null && states.Count > 0)
                {
                    foreach (Portal.DAL.State state in states)
                    {
                        stateList.Add(new StateViewModel { StateId = state.Stateid, StateCode = state.StateCode, StateName = state.StateName });
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
        public List<StateViewModel> GetStateList(string stateName = "", string stateCode = "", int countryId = 0)
        {
            List<StateViewModel> states = new List<StateViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtStates = sqlDbInterface.GetStateList(stateName, stateCode, countryId);
                if (dtStates != null && dtStates.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtStates.Rows)
                    {
                        states.Add(new StateViewModel
                        {
                            StateId = Convert.ToInt32(dr["StateId"]),
                            StateName = Convert.ToString(dr["StateName"]),
                            StateCode = Convert.ToString(dr["StateCode"]),
                            CountryId = dr["CountryId"] == DBNull.Value ? 0 : Convert.ToInt16(dr["CountryId"]),
                            CountryName = Convert.ToString(dr["CountryName"]),
                            CountryCode = Convert.ToString(dr["CountryCode"])
                        });
                    }
                }
            }
            catch (Exception ex) 
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return states;
        }
    
        public StateViewModel GetStateDetail(int stateId = 0)
        {
            StateViewModel state = new StateViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtStates = sqlDbInterface.GetStateDetail(stateId);
                if (dtStates != null && dtStates.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtStates.Rows)
                    {
                        state = new StateViewModel
                        {
                            StateId = Convert.ToInt32(dr["StateId"]),
                            StateName = Convert.ToString(dr["StateName"]),
                            StateCode = Convert.ToString(dr["StateCode"]),
                            CountryId = dr["CountryId"] == DBNull.Value ? 0 : Convert.ToInt16(dr["CountryId"]),
                            CountryName = Convert.ToString(dr["CountryName"]),
                            CountryCode = Convert.ToString(dr["CountryCode"]),

                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return state;
        }

        public List<StateViewModel> GetStateList()
        {
            List<StateViewModel> stateList = new List<StateViewModel>();
            try
            {
                List<Portal.DAL.State> states = dbInterface.GetStateList();
                if (states != null && states.Count > 0)
                {
                    foreach (Portal.DAL.State state in states)
                    {
                        stateList.Add(new StateViewModel { StateId = state.Stateid, StateCode = state.StateCode, StateName = state.StateName });
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


    }
}
