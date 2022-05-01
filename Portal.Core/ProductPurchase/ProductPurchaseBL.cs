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
    public class ProductPurchaseBL
    {

        public ProductPurchaseBL()
        {

        }

        public List<ProductPurchaseViewModel> GetProductPurchaseList(long productId, string vendorName, string invoiceFromDate, string invoiceToDate,string companyBranch)
        {
            List<ProductPurchaseViewModel> productPurchaseViewModels = new List<ProductPurchaseViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtProductPurchase = sqlDbInterface.GetProductPurchaseList(productId, vendorName, invoiceFromDate,invoiceToDate, companyBranch);
                if (dtProductPurchase != null && dtProductPurchase.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtProductPurchase.Rows)
                    {
                        productPurchaseViewModels.Add(new ProductPurchaseViewModel
                        {
                            POId = Convert.ToInt32(dr["POId"]),
                            PONo = Convert.ToString(dr["PONo"]),
                            PODate = Convert.ToString(dr["PODate"]),
                            VendorName = Convert.ToString(dr["VendorName"]),
                            VendorCode = Convert.ToString(dr["VendorCode"]),
                            BillingAddress = Convert.ToString(dr["BillingAddress"]),
                            City = Convert.ToString(dr["City"]),
                            StateName = Convert.ToString(dr["StateName"]),
                            GSTNO = Convert.ToString(dr["GSTNO"]),
                            NetAmount = Convert.ToDecimal(dr["NetAmount"]),
                            CGST_Amount = Convert.ToDecimal(dr["CGST_Amount"]),
                            SGST_Amount = Convert.ToDecimal(dr["SGST_Amount"]),
                            IGST_Amount = Convert.ToDecimal(dr["IGST_Amount"]),
                            GrossAmount = Convert.ToDecimal(dr["GrossAmount"]),
                            Discount = Convert.ToDecimal(dr["DiscountAmount"]),
                            Price = Convert.ToDecimal(dr["Price"]),
                            Quantity = Convert.ToDecimal(dr["Quantity"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            companyBranch = Convert.ToString(dr["BranchName"]),

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return productPurchaseViewModels;
        }

        public DataTable GetProductPurchaseReports(long productId,string companyBranch)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtProductPurchase = new DataTable();
            try
            {
                dtProductPurchase = sqlDbInterface.GetProductPurchaseList(productId, "", "", "", companyBranch);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtProductPurchase;
        }

        public string GetProductName(long productID)
        {
            string str = "";
            SQLDbInterface sqldbinterface = new SQLDbInterface();
            try
            {
                str = sqldbinterface.GetProductName(productID);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return str;
        }


    }
}