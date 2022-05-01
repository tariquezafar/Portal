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
    public class ProductPurchasePIBL
    {

        public ProductPurchasePIBL()
        {

        }

        public List<ProductPurchasePIViewModel> GetProductPurchasePIList(long productId, string vendorName, string invoiceFromDate, string invoiceToDate,string companyBranch)
        {
            List<ProductPurchasePIViewModel> productPurchaseViewModels = new List<ProductPurchasePIViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtProductPurchase = sqlDbInterface.GetProductPurchasePIList(productId,vendorName,invoiceFromDate,invoiceToDate, companyBranch);
                if (dtProductPurchase != null && dtProductPurchase.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtProductPurchase.Rows)
                    {
                        productPurchaseViewModels.Add(new ProductPurchasePIViewModel
                        {
                            InvoiceId = Convert.ToInt32(dr["InvoiceId"]),
                            InvoiceNo = Convert.ToString(dr["InvoiceNo"]),
                            InvoiceDate = Convert.ToString(dr["InvoiceDate"]),
                            PONo = Convert.ToString(dr["PONo"]),
                            VendorName = Convert.ToString(dr["VendorName"]),
                            VendorCode = Convert.ToString(dr["VendorCode"]),
                            BillingAddress = Convert.ToString(dr["BillingAddress"]),
                            ConsigneeName = Convert.ToString(dr["ConsigneeName"]),
                            ShippingAddress = Convert.ToString(dr["ShippingAddress"]),
                            ShippingCity = Convert.ToString(dr["ShippingCity"]),
                            ConsigneeGSTNo = Convert.ToString(dr["ConsigneeGSTNo"]),
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

        public DataTable PIProductReport(long productId,string companyBranch)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtProductPurchase = new DataTable();
            try
            {
                dtProductPurchase = sqlDbInterface.GetProductPurchasePIList(productId, "", "", "", companyBranch);
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