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
    
    public class ProductGLMappingBL
    {
        DBInterface dbInterface;
        public ProductGLMappingBL()
        {
            dbInterface = new DBInterface();
        }
        
        public ResponseOut AddEditProductGLMapping(ProductGLMappingViewModel productGLMappingViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                ProductGLMapping productGLMapping = new ProductGLMapping
                {
                    MappingId = productGLMappingViewModel.MappingId,
                    ProductSubGroupId = productGLMappingViewModel.ProductSubGroupId, 
                    GLId = productGLMappingViewModel.GLId,
                    CompanyId = productGLMappingViewModel.CompanyId,
                    Status = Convert.ToBoolean(productGLMappingViewModel.MappingStatus),
                    GLType=productGLMappingViewModel.GLType,
                    CreatedBy = productGLMappingViewModel.CreatedBy
                };
                responseOut = dbInterface.AddEditProductGLMapping(productGLMapping);
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

        public List<ProductGLMappingViewModel> GetProductGLMappingList(int productSubGroupId,int glId)
        {
            List<ProductGLMappingViewModel> products = new List<ProductGLMappingViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {

                DataTable dtProducts = sqlDbInterface.GetProductGLMappingList(productSubGroupId, glId);
                if (dtProducts != null && dtProducts.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtProducts.Rows)
                    {
                        products.Add(new ProductGLMappingViewModel
                        {
                            MappingId = Convert.ToInt32(dr["MappingId"]),
                            ProductSubGroupName = Convert.ToString(dr["ProductSubGroupName"]),
                            GLHead= Convert.ToString(dr["GLHead"]),
                            GLType=Convert.ToString(dr["GLType"]),
                            CreatedByName = Convert.ToString(dr["UserName"]),
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

        public ProductGLMappingViewModel GetProductGLDetail(int MappingId = 0)
        {

            ProductGLMappingViewModel productTaxt = new ProductGLMappingViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtproducts = sqlDbInterface.GetProductGLDetail(MappingId);
                if (dtproducts != null && dtproducts.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtproducts.Rows)
                    {
                        productTaxt = new ProductGLMappingViewModel
                        {
                            MappingId = Convert.ToInt32(dr["MappingId"]),
                            ProductSubGroupId = Convert.ToInt32(dr["ProductSubGroupId"]),
                            ProductSubGroupName = Convert.ToString(dr["ProductSubGroupName"]),                         
                            GLId = Convert.ToInt32(dr["GLId"]),
                            GLHead = Convert.ToString(dr["GLHead"]),
                            GLType = Convert.ToString(dr["GLType"]),
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
