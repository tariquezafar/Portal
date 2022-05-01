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
    public class AdditionalTaxBL
    {
        DBInterface dbInterface;
        public AdditionalTaxBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditAdditionalTax(AdditionalTaxViewModel additionaltaxViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                AdditionalTax additionaltax = new AdditionalTax
                {
                    AddTaxId = additionaltaxViewModel.AddTaxId,
                    AddTaxName = additionaltaxViewModel.AddTaxName,
                    CompanyId = additionaltaxViewModel.CompanyId, 
                    GLId = additionaltaxViewModel.GLId,
                    GLCode = additionaltaxViewModel.GLCode,
                    SLId = additionaltaxViewModel.SLId,
                    SLCode = additionaltaxViewModel.SLCode, 
                    Status = additionaltaxViewModel.AdditionalTax_Status
                };
                responseOut = dbInterface.AddEditAdditionalTax(additionaltax);

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



        public List<AdditionalTaxViewModel> GetAdditionalTaxList(string taxName = "", int companyId = 0, string status = "")
        {
            List<AdditionalTaxViewModel> additionaltaxes = new List<AdditionalTaxViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtTaxes = sqlDbInterface.GetAdditionalTaxList(taxName, companyId, status);
                if (dtTaxes != null && dtTaxes.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtTaxes.Rows)
                    {
                        additionaltaxes.Add(new AdditionalTaxViewModel
                        {
                            AddTaxId = Convert.ToInt32(dr["AddTaxId"]),
                            AddTaxName = Convert.ToString(dr["AddTaxName"]),  
                            AdditionalTax_Status = Convert.ToBoolean(dr["Status"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return additionaltaxes;
        }

        public AdditionalTaxViewModel GetAddtionalTaxDetail(int taxId = 0)
        {
            AdditionalTaxViewModel tax = new AdditionalTaxViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtTaxes = sqlDbInterface.GetAdditionalTaxDetail(taxId);
                if (dtTaxes != null && dtTaxes.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtTaxes.Rows)
                    {
                        tax = new AdditionalTaxViewModel
                        {
                            AddTaxId = Convert.ToInt32(dr["AddTaxId"]),
                            AddTaxName = Convert.ToString(dr["AddTaxName"]),
                            GLCode = Convert.ToString(dr["GLCode"]),
                            SLCode = Convert.ToString(dr["SLCode"]),
                            GLId = Convert.ToInt32(dr["GLId"]),
                            SLId = Convert.ToInt32(dr["SLId"]),
                            TaxGLHead = Convert.ToString(dr["TaxGLHead"]),
                            TaxSLHead = Convert.ToString(dr["TaxSLHead"]), 
                            AdditionalTax_Status = Convert.ToBoolean(dr["Status"]),
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return tax;
        }
        public List<TaxViewModel> GetTaxAutoCompleteList(string searchTerm, string taxtaxSubType, int companyId)
        {
            List<TaxViewModel> taxes = new List<TaxViewModel>();
            try
            {
                List<Tax> taxList = dbInterface.GetTaxAutoCompleteList(searchTerm, taxtaxSubType, companyId);

                if (taxList != null && taxList.Count > 0)
                {
                    foreach (Tax tax in taxList)
                    {
                        taxes.Add(new TaxViewModel { TaxId = tax.TaxId, TaxName = tax.TaxName, TaxPercentage = Convert.ToDecimal(tax.TaxPercentage) });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return taxes;
        }


    }
}








