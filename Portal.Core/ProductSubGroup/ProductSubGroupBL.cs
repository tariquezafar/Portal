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
    public class ProductSubGroupBL
    {
        DBInterface dbInterface;
        public ProductSubGroupBL()
        {
            dbInterface = new DBInterface();
        }

        public ResponseOut AddEditProductSubGroup(ProductSubGroupViewModel productsubgroupViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                ProductSubGroup productsubgroup = new ProductSubGroup
                {
                    ProductSubGroupId = productsubgroupViewModel.ProductSubGroupId,
                    ProductSubGroupName = productsubgroupViewModel.ProductSubGroupName,
                    ProductSubGroupCode = productsubgroupViewModel.ProductSubGroupCode,
                    ProductMainGroupId = productsubgroupViewModel.ProductMainGroupId,
                    Status = productsubgroupViewModel.ProductSubGroup_Status
                };
                responseOut = dbInterface.AddEditProductSubGroup(productsubgroup);
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

        public List<ProductSubGroupViewModel> GetProductSubGroupList(string productsubgroupName = "", string productsubgroupCode = "", int productmaingroupId = 0, string Status = "")
        {
            List<ProductSubGroupViewModel> productsubgroups = new List<ProductSubGroupViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtProductSubGroups = sqlDbInterface.GetProductSubGroupList(productsubgroupName,productsubgroupCode, productmaingroupId, Status);
                if (dtProductSubGroups != null && dtProductSubGroups.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtProductSubGroups.Rows)
                    {
                        productsubgroups.Add(new ProductSubGroupViewModel
                        {
                            ProductSubGroupId = Convert.ToInt32(dr["ProductSubGroupId"]),
                            ProductSubGroupName = Convert.ToString(dr["ProductSubGroupName"]),
                            ProductSubGroupCode = Convert.ToString(dr["ProductSubGroupCode"]),
                            ProductMainGroupId = dr["ProductMainGroupId"] == DBNull.Value ? 0 : Convert.ToInt16(dr["ProductMainGroupId"]),
                            ProductMainGroupName = Convert.ToString(dr["ProductMainGroupName"]),
                            ProductSubGroup_Status = Convert.ToBoolean(dr["Status"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return productsubgroups;
        }

        public ProductSubGroupViewModel GetProductSubGroupDetail(int productsubgroupId = 0)
        {
            ProductSubGroupViewModel productsubgroup = new ProductSubGroupViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtProductSubGroups = sqlDbInterface.GetProductSubGroupDetail(productsubgroupId);
                if (dtProductSubGroups != null && dtProductSubGroups.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtProductSubGroups.Rows)
                    {
                        productsubgroup = new ProductSubGroupViewModel
                        {
                            ProductSubGroupId = Convert.ToInt32(dr["ProductSubGroupId"]),
                            ProductSubGroupName = Convert.ToString(dr["ProductSubGroupName"]),
                            ProductSubGroupCode = Convert.ToString(dr["ProductSubGroupCode"]),
                            ProductMainGroupId = Convert.ToInt16(dr["ProductMainGroupId"]),
                            ProductMainGroupName = Convert.ToString(dr["ProductMainGroupName"]),
                            ProductSubGroup_Status = Convert.ToBoolean(dr["Status"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return productsubgroup;
        }


        public List<ProductSubGroupViewModel> GetProductSubGroupList(int productMainGroupId)
        {
            List<ProductSubGroupViewModel> productSubGroups= new List<ProductSubGroupViewModel>();
            try
            {
                List<ProductSubGroup> productSubGroupList = dbInterface.GetProductSubGroupList(productMainGroupId);
                if (productSubGroupList != null && productSubGroupList.Count > 0)
                {
                    foreach (ProductSubGroup productSubGroup in productSubGroupList)
                    {
                        productSubGroups.Add(new ProductSubGroupViewModel { ProductSubGroupId = productSubGroup.ProductSubGroupId, ProductSubGroupName = productSubGroup.ProductSubGroupName,ProductSubGroupCode=productSubGroup.ProductSubGroupCode });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return productSubGroups;
        }

        public List<ProductSubGroupViewModel> GetChasisModelSubGroupList()
        {
            List<ProductSubGroupViewModel> productSubGroups = new List<ProductSubGroupViewModel>();
            try
            {
                List<ProductSubGroup> productSubGroupList = dbInterface.GetChasisModelSubGroupList();
                if (productSubGroupList != null && productSubGroupList.Count > 0)
                {
                    foreach (ProductSubGroup productSubGroup in productSubGroupList)
                    {
                        productSubGroups.Add(new ProductSubGroupViewModel { ProductSubGroupId = productSubGroup.ProductSubGroupId, ProductSubGroupName = productSubGroup.ProductSubGroupName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return productSubGroups;
        }
        public ResponseOut ImportProductSubGroup(ProductSubGroupViewModel productsubgroupViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                ProductSubGroup productsubgroup = new ProductSubGroup
                {
                    ProductSubGroupId = productsubgroupViewModel.ProductSubGroupId,
                    ProductSubGroupName = productsubgroupViewModel.ProductSubGroupName,
                    ProductSubGroupCode = productsubgroupViewModel.ProductSubGroupCode,
                    ProductMainGroupId = productsubgroupViewModel.ProductMainGroupId,
                    Status = productsubgroupViewModel.ProductSubGroup_Status
                };
                responseOut = dbInterface.AddEditProductSubGroup(productsubgroup);
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
    }
}
