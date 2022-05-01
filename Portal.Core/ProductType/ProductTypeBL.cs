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
    public class ProductTypeBL
    {
        DBInterface dbInterface;
        public ProductTypeBL()
        {
            dbInterface = new DBInterface();
        }
       
        public ResponseOut AddEditProductType(ProductTypeViewModel producttypeViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                ProductType producttype = new ProductType
                {
                    ProductTypeId = producttypeViewModel.ProductTypeId,
                    ProductTypeName = producttypeViewModel.ProductTypeName,
                    ProductTypeCode = producttypeViewModel.ProductTypeCode,
                    Status = producttypeViewModel.ProductType_Status,
                    CompanyBranchId= producttypeViewModel.CompanyBranchId
                };
                responseOut = dbInterface.AddEditProductType(producttype);
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

        public List<ProductTypeViewModel> GetProductTypeList(string producttypeName = "", string producttypeCode = "", string Status = "",int companyBranchId=0)
        {
            List<ProductTypeViewModel> producttypes = new List<ProductTypeViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtProductTypes = sqlDbInterface.GetProductTypeList(producttypeName, producttypeCode, Status, companyBranchId);
                if (dtProductTypes != null && dtProductTypes.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtProductTypes.Rows)
                    {
                        producttypes.Add(new ProductTypeViewModel
                        {
                            ProductTypeId = Convert.ToInt32(dr["ProductTypeId"]),
                            ProductTypeName = Convert.ToString(dr["ProductTypeName"]),
                            ProductTypeCode= Convert.ToString(dr["ProductTypeCode"]),
                            ProductType_Status = Convert.ToBoolean(dr["Status"]),
                            CompanyBranchName= Convert.ToString(dr["CompanyBranchName"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return producttypes;
        }

        public ProductTypeViewModel GetProductTypeDetail(int producttypeId = 0)
        {
            ProductTypeViewModel producttype = new ProductTypeViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtProductTypes = sqlDbInterface.GetProductTypeDetail(producttypeId);
                if (dtProductTypes != null && dtProductTypes.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtProductTypes.Rows)
                    {
                        producttype = new ProductTypeViewModel
                        {
                            ProductTypeId = Convert.ToInt32(dr["ProductTypeId"]),
                            ProductTypeName = Convert.ToString(dr["ProductTypeName"]),
                            ProductTypeCode = Convert.ToString(dr["ProductTypeCode"]),
                            ProductType_Status = Convert.ToBoolean(dr["Status"]),
                            CompanyBranchId=Convert.ToInt32(dr["CompanyBranchId"]),
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return producttype;
        }

        public List<ProductTypeViewModel> GetProductTypeList()
        {
            List<ProductTypeViewModel> productTypes = new List<ProductTypeViewModel>();
            try
            {
                List<ProductType> productTypeList = dbInterface.GetProductTypeList();
                if (productTypeList != null && productTypeList.Count > 0)
                {
                    foreach (ProductType productType in productTypeList)
                    {
                        productTypes.Add(new ProductTypeViewModel { ProductTypeId = productType.ProductTypeId, ProductTypeName = productType.ProductTypeName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return productTypes;
        }


    }
}
