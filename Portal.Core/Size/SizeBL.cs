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
    public class SizeBL
    {
        DBInterface dbInterface;
        public SizeBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditSize(SizeViewModel sizeViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                Size size = new Size
                {
                    SizeId = sizeViewModel.SizeId,
                    SizeCode = sizeViewModel.SizeCode,
                    SizeDesc = sizeViewModel.SizeDesc,
                    ProductMainGroupId = sizeViewModel.ProductMainGroupId,
                    ProductSubGroupId = sizeViewModel.ProductSubGroupId,
                    Status = sizeViewModel.Size_Status,

                };
                responseOut = dbInterface.AddEditSize(size);
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
         

        public List<SizeViewModel> GetSizeList(string sizeDesc, string sizeCode, int productSubGroupId, int productMainGroupId, string sizeStatus)
        {
            List<SizeViewModel> sizes = new List<SizeViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtProducts = sqlDbInterface.GetSizeList(sizeDesc, sizeCode, productSubGroupId, productMainGroupId, sizeStatus);
                if (dtProducts != null && dtProducts.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtProducts.Rows)
                    {
                        sizes.Add(new SizeViewModel
                        {
                            SizeId = Convert.ToInt32(dr["SizeId"]),
                            SizeCode = Convert.ToString(dr["SizeCode"]),
                            SizeDesc = Convert.ToString(dr["SizeDesc"]),
                            ProductMainGroupId = Convert.ToInt16(dr["ProductMainGroupId"]),
                            ProductMainGroupName = Convert.ToString(dr["ProductMainGroupName"]),
                            ProductSubGroupId = Convert.ToInt16(dr["ProductSubGroupId"]),
                            ProductSubGroupName = Convert.ToString(dr["ProductSubGroupName"]),
                            Size_Status = Convert.ToBoolean(dr["Status"])

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return sizes;
        }
        public SizeViewModel GetSizeDetail(long sizeId = 0)
        {
            SizeViewModel size = new SizeViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtProducts = sqlDbInterface.GetSizeDetail(sizeId);
                if (dtProducts != null && dtProducts.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtProducts.Rows)
                    {
                        size = new SizeViewModel
                        {
                            SizeId = Convert.ToInt32(dr["SizeId"]),
                            SizeCode = Convert.ToString(dr["SizeCode"]),
                            SizeDesc = Convert.ToString(dr["SizeDesc"]),
                            ProductMainGroupId = Convert.ToInt16(dr["ProductMainGroupId"]),
                            ProductMainGroupName = Convert.ToString(dr["ProductMainGroupName"]),
                            ProductSubGroupId = Convert.ToInt16(dr["ProductSubGroupId"]),
                            ProductSubGroupName = Convert.ToString(dr["ProductSubGroupName"]),
                            Size_Status = Convert.ToBoolean(dr["Status"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return size;
        }
         

        public ResponseOut ImportSize(SizeViewModel sizeViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                Size size = new Size
                {
                    SizeId = sizeViewModel.SizeId,
                    SizeCode = sizeViewModel.SizeCode,
                    SizeDesc = sizeViewModel.SizeDesc,
                    ProductMainGroupId = sizeViewModel.ProductMainGroupId,
                    ProductSubGroupId = sizeViewModel.ProductSubGroupId,
                    Status = sizeViewModel.Size_Status,
                };
                responseOut = dbInterface.AddEditSize(size);
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




        public List<ProductMainGroupViewModel> GetProductMainGroupList()
        {
            List<ProductMainGroupViewModel> productMainGroups = new List<ProductMainGroupViewModel>();
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
        public List<SizeViewModel> GetSizeAutoCompleteList(string searchTerm, int productMainGroupId, int productSubGroupId)
        {
            List<SizeViewModel> sizes = new List<SizeViewModel>();
            try
            {
                List<Size> sizeList = dbInterface.GetSizeAutoCompleteList(searchTerm, productMainGroupId, productSubGroupId);

                if (sizeList != null && sizeList.Count > 0)
                {
                    foreach (Size size in sizeList)
                    {
                        sizes.Add(new SizeViewModel { SizeId = size.SizeId, SizeCode = size.SizeCode, SizeDesc = size.SizeDesc });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return sizes;
        }

    }
}
