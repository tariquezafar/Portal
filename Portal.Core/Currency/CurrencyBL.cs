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
    public class CurrencyBL
    {
        DBInterface dbInterface;
        public CurrencyBL()
        {
            dbInterface = new DBInterface();
        }
        public List<CurrencyViewModel> GetCurrencyList()
        {
            List<CurrencyViewModel> currencyList = new List<CurrencyViewModel>();
            try
            {
                List<Portal.DAL.Currency> currencies = dbInterface.GetCurrencyList();
                if (currencies != null && currencies.Count > 0)
                {
                    foreach (Portal.DAL.Currency currency in currencies)
                    {
                        currencyList.Add(new CurrencyViewModel { CurrencyId = currency.CurrencyId, CurrencyCode = currency.CurrencyCode, CurrencyName = currency.CurrencyName });
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

    }
}
