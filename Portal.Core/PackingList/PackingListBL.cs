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
   public class PackingListBL
    {
        DBInterface dbInterface;
        public PackingListBL()
        {
            dbInterface = new DBInterface();
        }

        public ResponseOut AddEditPackingList(PackingListViewModel packingListViewModel, List<PackingListDetailViewModel> packingListProducts)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                PackingList packingList = new PackingList
                {
                    PackingListID= packingListViewModel.PackingListID,
                    PackingListName= packingListViewModel.PackingListName,
                    PackingListDescription= packingListViewModel.PackingListDescription,
                    PackingListStatus=packingListViewModel.PackingListStatus,
                    PackingListTypeID=packingListViewModel.PackingListTypeID,
                    ProductSubGroupId=packingListViewModel.ProductSubGroupId,
                    CreatedBy=packingListViewModel.CreatedBy,
                    ModifiedBy=packingListViewModel.ModifiedBy,
                    CompanyBranchId = packingListViewModel.CompanyBranchId,
                };
                List<PackingListDetail> packingListProductList = new List<PackingListDetail>();
                if (packingListProducts != null && packingListProducts.Count > 0)
                {
                    foreach (PackingListDetailViewModel item in packingListProducts)
                    {
                        packingListProductList.Add(new PackingListDetail
                        {
                            ProductID = item.ProductId,
                            Quantity = item.Quantity,
                            IsComplimentary=item.IsComplimentary
                        });
                    }
                }
                responseOut = sqlDbInterface.AddEditPackingList(packingList, packingListProductList);

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
        public List<PackingListDetailViewModel> GetPackingListProductList(long packingListId)
        {
            List<PackingListDetailViewModel> packingListProducts = new List<PackingListDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtPackingList = sqlDbInterface.GetPackingListProductList(packingListId);
                if (dtPackingList != null && dtPackingList.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPackingList.Rows)
                    {
                        packingListProducts.Add(new PackingListDetailViewModel
                        {
                            PackingListDetailedID = Convert.ToInt32(dr["PackingListDetailedID"]),
                            SequenceNo = Convert.ToInt32(dr["SequenceNo"]),
                            PackingListID = Convert.ToInt32(dr["PackingListID"]),
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ProductCode = Convert.ToString(dr["ProductCode"]),
                            ProductShortDesc = Convert.ToString(dr["ProductShortDesc"]),
                            UOMName = Convert.ToString(dr["UOMName"]),
                            Quantity = Convert.ToDecimal(dr["Quantity"]),
                            IsComplimentary=Convert.ToString(dr["IsComplimentary"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return packingListProducts;
        }

        public List<PackingListViewModel> GetProductSubGroup()
        {
            List<PackingListViewModel> productSubGroups = new List<PackingListViewModel>();
            try
            {
                List<ProductSubGroup> productSubGroupList = dbInterface.GetAllProductSubGroupList();
                if (productSubGroupList != null && productSubGroupList.Count > 0)
                {
                    foreach (ProductSubGroup productSubGroup in productSubGroupList)
                    {
                        productSubGroups.Add(new PackingListViewModel { ProductSubGroupId = productSubGroup.ProductSubGroupId, ProductSubGroupName = productSubGroup.ProductSubGroupName });
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
        public List<PackingListViewModel> GetAllPackingListType()
        {
            List<PackingListViewModel> allPackingListType = new List<PackingListViewModel>();
            try
            {
                List<PackingListType> productPackingListTypeList = dbInterface.GetPackingTypeList();
                if (productPackingListTypeList != null && productPackingListTypeList.Count > 0)
                {
                    foreach (PackingListType productPackingType in productPackingListTypeList)
                    {
                        allPackingListType.Add(new PackingListViewModel { PackingListTypeID = productPackingType.PackingListTypeID, PackingListTypeName = productPackingType.PackingListTypeName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return allPackingListType;
        }

        // Get Packing BOM Product List  from SubGrouptype 
        public List<PackingListDetailViewModel> GetPackingListBOMProductList(int productSubGroupId)
        {
            List<PackingListDetailViewModel> packingListBomProducts = new List<PackingListDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtPackingList = sqlDbInterface.GetPackingListBOMProductList(productSubGroupId);
                if (dtPackingList != null && dtPackingList.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPackingList.Rows)
                    {
                        packingListBomProducts.Add(new PackingListDetailViewModel
                        {
                            PackingListDetailedID = Convert.ToInt32(dr["PackingListDetailedID"]),
                            SequenceNo = Convert.ToInt32(dr["SNo"]),
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ProductCode = Convert.ToString(dr["ProductCode"]),
                            ProductShortDesc = Convert.ToString(dr["ProductShortDesc"]),
                            UOMName = Convert.ToString(dr["UOMName"]),
                            Quantity = Convert.ToDecimal(dr["Quantity"]),
                            IsComplimentary = Convert.ToString(dr["IsComplimentary"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return packingListBomProducts;
        }

        public PackingListViewModel GetPackingListDetail(long packingListId = 0)
        {
            PackingListViewModel packingListViewModel = new PackingListViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtPackingList = sqlDbInterface.GetPackingListDetail(packingListId);
                if (dtPackingList != null && dtPackingList.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPackingList.Rows)
                    {
                        packingListViewModel = new PackingListViewModel
                        {
                           // SequenceNo = Convert.ToInt32(dr["SequenceNo"]),
                            PackingListID = Convert.ToInt32(dr["PackingListId"]),
                            PackingListName = Convert.ToString(dr["PackingListName"]),
                            PackingListDescription = Convert.ToString(dr["PackingListDescription"]),
                            PackingListTypeID = Convert.ToInt32(dr["PackingListTypeID"]),
                            ProductSubGroupId = Convert.ToInt32(dr["ProductSubGroupId"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                            PackingListStatus = Convert.ToString(dr["PackingListStatus"]),
                           
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return packingListViewModel;
        }

        public List<PackingListViewModel> GetPackingList(string packingListName, string packingListStatus,int companyBranchId)
        {
            List<PackingListViewModel> packingListViewModel = new List<PackingListViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtPackingList = sqlDbInterface.GetPackingList(packingListName, packingListStatus, companyBranchId);
                if (dtPackingList != null && dtPackingList.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPackingList.Rows)
                    {
                        packingListViewModel.Add(new PackingListViewModel
                        {
                            PackingListID = Convert.ToInt32(dr["PackingListID"]),
                            PackingListName = Convert.ToString(dr["PackingListName"]),                      
                            PackingListStatus= Convert.ToString(dr["PackingListStatus"]),
                            PackingListTypeName=Convert.ToString(dr["PackingListTypeName"]),
                            ProductSubGroupName = Convert.ToString(dr["ProductSubGroupName"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            CompanyBranchName=Convert.ToString(dr["CompanyBranchName"])

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return packingListViewModel;
        }
    }
}
