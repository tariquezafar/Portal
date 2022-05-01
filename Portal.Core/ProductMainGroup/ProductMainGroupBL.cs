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
    public class ProductMainGroupBL
    {
        DBInterface dbInterface;
        public ProductMainGroupBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditProductMainGroup(ProductMainGroupViewModel productmaingroupViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
               ProductMainGroup productmaingroup = new ProductMainGroup
               {
                   ProductMainGroupId = productmaingroupViewModel.ProductMainGroupId,
                   ProductMainGroupName = productmaingroupViewModel.ProductMainGroupName,
                   ProductMainGroupCode = productmaingroupViewModel.ProductMainGroupCode,
                   Status = productmaingroupViewModel.ProductMainGroup_Status
               };
                responseOut = dbInterface.AddEditProductMainGroup(productmaingroup);
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
        public List<ProductMainGroupViewModel> GetProductMainGroupList(string productmaingroupName = "", string productmaingroupCode = "", string Status = "")
        {
            List<ProductMainGroupViewModel> productmaingroups = new List<ProductMainGroupViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtProductMainGroups = sqlDbInterface.GetProductMainGroupList(productmaingroupName, productmaingroupCode, Status);
                if (dtProductMainGroups != null && dtProductMainGroups.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtProductMainGroups.Rows)
                    {
                        productmaingroups.Add(new ProductMainGroupViewModel
                        {
                            ProductMainGroupId = Convert.ToInt32(dr["ProductMainGroupId"]),
                            ProductMainGroupName = Convert.ToString(dr["ProductMainGroupName"]),
                            ProductMainGroupCode = Convert.ToString(dr["ProductMainGroupCode"]),
                            ProductMainGroup_Status = Convert.ToBoolean(dr["Status"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return productmaingroups;
        }
        public ProductMainGroupViewModel GetProductMainGroupDetail(int productmaingroupId = 0)
        {
            ProductMainGroupViewModel productmaingroup = new ProductMainGroupViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtProductMainGroups = sqlDbInterface.GetProductMainGroupDetail(productmaingroupId);
                if (dtProductMainGroups != null && dtProductMainGroups.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtProductMainGroups.Rows)
                    {
                        productmaingroup = new ProductMainGroupViewModel
                        {
                            ProductMainGroupId = Convert.ToInt32(dr["ProductMainGroupId"]),
                            ProductMainGroupName = Convert.ToString(dr["ProductMainGroupName"]),
                            ProductMainGroupCode = Convert.ToString(dr["ProductMainGroupCode"]),
                            ProductMainGroup_Status = Convert.ToBoolean(dr["Status"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return productmaingroup;
        }
        public List<ProductMainGroupViewModel> GetProductMainGroupList()
        {
            List<ProductMainGroupViewModel> productMainGroups= new List<ProductMainGroupViewModel>();
            try
            {
                List<ProductMainGroup> productMainGroupList = dbInterface.GetProductMainGroupList();
                if (productMainGroupList != null && productMainGroupList.Count > 0)
                {
                    foreach (ProductMainGroup productMainGroup in productMainGroupList)
                    {
                        productMainGroups.Add(new ProductMainGroupViewModel { ProductMainGroupId = productMainGroup.ProductMainGroupId, ProductMainGroupName = productMainGroup.ProductMainGroupName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return productMainGroups;
        }
        public ResponseOut ImportProductMainGroup(ProductMainGroupViewModel productmaingroupViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                ProductMainGroup productmaingroup = new ProductMainGroup
                {
                    ProductMainGroupId = productmaingroupViewModel.ProductMainGroupId,
                    ProductMainGroupName = productmaingroupViewModel.ProductMainGroupName,
                    ProductMainGroupCode = productmaingroupViewModel.ProductMainGroupCode,
                    Status = productmaingroupViewModel.ProductMainGroup_Status
                };
                responseOut = dbInterface.AddEditProductMainGroup(productmaingroup);
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
