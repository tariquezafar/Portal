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
    public class PackingMaterialBOMBL
    {
        DBInterface dbInterface;
        public PackingMaterialBOMBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditPackingMaterialBOM(PackingMaterialBOMViewModel packingMaterialBOMViewModel, List<PackingMaterialBOMProductViewModel> packingMaterialBOMProducts)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                PackingMaterialBOM packingMaterialBOM = new PackingMaterialBOM
                {
                    PMBId = packingMaterialBOMViewModel.PMBId,
                    PMBDate = Convert.ToDateTime(packingMaterialBOMViewModel.PMBDate),
                    CompanyId = packingMaterialBOMViewModel.CompanyId,
                    PackingListTypeId = packingMaterialBOMViewModel.PackingListTypeId,
                    ProductSubGroupId = packingMaterialBOMViewModel.ProductSubGroupid,
                    CreatedBy = packingMaterialBOMViewModel.CreatedBy,
                    CompanyBranchId= packingMaterialBOMViewModel.CompanyBranchId
                    
                };
                List<PackingMaterialBOMProductDetail> packingMaterialBOMProductList = new List<PackingMaterialBOMProductDetail>();
                if(packingMaterialBOMProducts != null && packingMaterialBOMProducts.Count>0)
                {
                    foreach(PackingMaterialBOMProductViewModel item in packingMaterialBOMProducts)
                    {
                        packingMaterialBOMProductList.Add(new PackingMaterialBOMProductDetail
                        {
                            ProductId=item.ProductId,
                            Quantity=item.Quantity
                        });
                    }
                }
                responseOut = sqlDbInterface.AddEditPackingMaterialBOM(packingMaterialBOM, packingMaterialBOMProductList);
             
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


        public List<PackingMaterialBOMViewModel> GetPackingMaterialBOMList(string pMBNo, int packingListTypeId, int productSubGroupId, string fromDate, string toDate, int companyId,int companyBranchId)
        {
            List<PackingMaterialBOMViewModel> packingMaterialBOMs = new List<PackingMaterialBOMViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtPackingMaterialBOMs = sqlDbInterface.GetPackingMaterialBOMList(pMBNo, packingListTypeId, productSubGroupId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, companyBranchId);
                if (dtPackingMaterialBOMs != null && dtPackingMaterialBOMs.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPackingMaterialBOMs.Rows)
                    {
                        packingMaterialBOMs.Add(new PackingMaterialBOMViewModel
                        {
                            PMBId = Convert.ToInt32(dr["PMBId"]),
                            PMBNo = Convert.ToString(dr["PMBNo"]),
                            PMBDate = Convert.ToString(dr["PMBDate"]),
                            PackingListTypeId = Convert.ToInt32(dr["PackingListTypeId"]),
                            PackingListTypeName = Convert.ToString(dr["PackingListTypeName"]),
                            ProductSubGroupid = Convert.ToInt32(dr["ProductSubGroupId"]),
                            ProductSubGroupName = Convert.ToString(dr["ProductSubGroupName"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                            CompanyBranchName = Convert.ToString(dr["CompanyBranchName"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return packingMaterialBOMs;
        }
        public PackingMaterialBOMViewModel GetPackingMaterialBOMDetail(long pMBId = 0)
        {
            PackingMaterialBOMViewModel packingMaterialBOM = new PackingMaterialBOMViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtPackingMaterialBOM = sqlDbInterface.GetPackingMaterialBOMDetail(pMBId);
                if (dtPackingMaterialBOM != null && dtPackingMaterialBOM.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPackingMaterialBOM.Rows)
                    {
                        packingMaterialBOM = new PackingMaterialBOMViewModel
                        {
                            PMBId = Convert.ToInt32(dr["PMBId"]),
                            PMBNo = Convert.ToString(dr["PMBNo"]),
                            PMBDate = Convert.ToString(dr["PMBDate"]),
                            PackingListTypeId = Convert.ToInt32(dr["PackingListTypeId"]),
                            ProductSubGroupid = Convert.ToInt64(dr["ProductSubGroupid"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                            CompanyBranchName = Convert.ToString(dr["CompanyBranchName"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return packingMaterialBOM;
        }


        public List<PackingMaterialBOMProductViewModel> GetPackingMaterialBOMProductList(long pMBId)
        {
            List<PackingMaterialBOMProductViewModel> packingMaterialBOMProducts = new List<PackingMaterialBOMProductViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtPMBProducts = sqlDbInterface.GetPackingMaterialBOMProductList(pMBId);
                if (dtPMBProducts != null && dtPMBProducts.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPMBProducts.Rows)
                    {
                        packingMaterialBOMProducts.Add(new PackingMaterialBOMProductViewModel
                        {
                            PMBProductDetailId = Convert.ToInt32(dr["PMBProductDetailId"]),
                            SequenceNo = Convert.ToInt32(dr["SNo"]),
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ProductCode = Convert.ToString(dr["ProductCode"]),
                            ProductShortDesc = Convert.ToString(dr["ProductShortDesc"]),
                            UOMName = Convert.ToString(dr["UOMName"]),
                            Quantity = Convert.ToDecimal(dr["Quantity"])

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return packingMaterialBOMProducts;
        }

        public DataTable GetPackingMaterialBOMProductListDataTable(long pMBId)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtProducts = new DataTable();
            try
            {
                dtProducts = sqlDbInterface.GetPackingMaterialBOMProductList(pMBId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtProducts;
        }

        public DataTable GetPackingMaterialBOMDataTable(long pMBId = 0)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtPackingMaterialBOM = new DataTable();
            try
            {
                dtPackingMaterialBOM = sqlDbInterface.GetPackingMaterialBOMDetail(pMBId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtPackingMaterialBOM;
        }

        public List<ProductSubGroupViewModel> GetProductSubGroupListForPMB()
        {
            List<ProductSubGroupViewModel> allProductSubGroup = new List<ProductSubGroupViewModel>();
            try
            {
                List<ProductSubGroup> productSubGroupList = dbInterface.GetProductSubGroupListForPMB();
                if (productSubGroupList != null && productSubGroupList.Count > 0)
                {
                    foreach (ProductSubGroup productSubGroup in productSubGroupList)
                    {
                        allProductSubGroup.Add(new ProductSubGroupViewModel { ProductSubGroupId = productSubGroup.ProductSubGroupId, ProductSubGroupName = productSubGroup.ProductSubGroupName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return allProductSubGroup;
        }

    }
}
