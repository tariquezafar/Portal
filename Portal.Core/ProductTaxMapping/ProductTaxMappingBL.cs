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
    
    public class ProductTaxMappingBL
    {
        DBInterface dbInterface;
        public ProductTaxMappingBL()
        {
            dbInterface = new DBInterface();
        }
        
        public ResponseOut AddEditProductTaxMapping(ProductSubCategoryStateTaxMappingViewModel productSubCategoryStateTaxMappingViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                ProductSubCategoryStateTaxMapping productSubCategoryStateTaxMappingg = new ProductSubCategoryStateTaxMapping
                {
                    MappingId = productSubCategoryStateTaxMappingViewModel.MappingId,
                    ProductSubGroupId = productSubCategoryStateTaxMappingViewModel.ProductSubGroupId,
                    StateId = productSubCategoryStateTaxMappingViewModel.StateId,
                    TaxId = productSubCategoryStateTaxMappingViewModel.TaxId,
                    CompanyId = productSubCategoryStateTaxMappingViewModel.CompanyId,
                    Status = Convert.ToBoolean(productSubCategoryStateTaxMappingViewModel.MappingStatus),
                    CreatedBy = productSubCategoryStateTaxMappingViewModel.CreatedBy
                };
                responseOut = dbInterface.AddEditProductTaxMapping(productSubCategoryStateTaxMappingg);
            }
            catch(Exception ex)
            {
                responseOut.status = ActionStatus.Fail;
                responseOut.message = ActionMessage.ApplicationException;
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return responseOut;
        }

        public List<ProductSubCategoryStateTaxMappingViewModel> GetProductTaxMappingList(int productSubGroupId, int stateId, int taxId)
        {
            List<ProductSubCategoryStateTaxMappingViewModel> products = new List<ProductSubCategoryStateTaxMappingViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {

                DataTable dtProducts = sqlDbInterface .GetProductTaxMappingList(productSubGroupId, stateId, taxId);
                if (dtProducts != null && dtProducts.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtProducts.Rows)
                    {
                        products.Add(new ProductSubCategoryStateTaxMappingViewModel {
                            MappingId = Convert.ToInt32(dr["MappingId"]),
                            ProductSubGroupName = Convert.ToString(dr["ProductSubGroupName"]),
                            TaxName = Convert.ToString(dr["TaxName"]),
                            StateName =Convert.ToString(dr["StateName"]),
                            CreatedByName=Convert.ToString(dr["UserName"]),
                            MappingStatus = Convert.ToInt32(dr["Status"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"])
                        });

                       

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return products;
        }

        public ProductSubCategoryStateTaxMappingViewModel GetProductStateTaxDetail(int MappingId = 0)
        {

            ProductSubCategoryStateTaxMappingViewModel productTaxt = new ProductSubCategoryStateTaxMappingViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtproducts = sqlDbInterface.GetProductStateTaxDetail(MappingId);
                if (dtproducts != null && dtproducts.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtproducts.Rows)
                    {
                        productTaxt = new ProductSubCategoryStateTaxMappingViewModel
                        {
                            MappingId = Convert.ToInt32(dr["MappingId"]),
                            ProductSubGroupId = Convert.ToInt32(dr["ProductSubGroupId"]),
                            ProductSubGroupName = Convert.ToString(dr["ProductSubGroupName"]),
                            ProductMainGroupId = Convert.ToInt32(dr["ProductMainGroupId"]),
                            TaxId = Convert.ToInt32(dr["TaxId"]),
                            TaxName = Convert.ToString(dr["TaxName"]),
                            StateId = Convert.ToInt32(dr["StateId"]),
                            StateName = Convert.ToString(dr["StateName"]),
                            MappingStatus = Convert.ToInt32(dr["Status"])
                          
                           

                        };


                    }
                }


            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return productTaxt;
        }
    }
}
