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
    public class ProductBL
    {
        DBInterface dbInterface;

        public ProductBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditProduct(ProductViewModel productViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                Product product = new Product
                {
                    Productid = productViewModel.Productid,
                    ProductName = productViewModel.ProductName,
                    ProductCode = productViewModel.ProductCode,
                    ProductShortDesc = productViewModel.ProductShortDesc,
                    ProductFullDesc = productViewModel.ProductFullDesc,
                    ProductTypeId = productViewModel.ProductTypeId,
                    ProductMainGroupId = productViewModel.ProductMainGroupId,
                    ProductSubGroupId = productViewModel.ProductSubGroupId,
                    AssemblyType = productViewModel.AssemblyType,
                    UOMId = productViewModel.UOMId,
                    PurchaseUOMId = productViewModel.PurchaseUOMId,
                    PurchasePrice = productViewModel.PurchasePrice,
                    SalePrice = productViewModel.SalePrice,
                    LocalTaxRate = 0,
                    CentralTaxRate = 0,
                    OtherTaxRate = 0,
                    IsSerializedProduct = productViewModel.IsSerializedProduct,
                    BrandName = productViewModel.BrandName,
                    ReOrderQty = productViewModel.ReOrderQty,
                    MinOrderQty = productViewModel.MinOrderQty,
                    CreatedBy = productViewModel.CreatedBy,
                    Status = productViewModel.Product_Status,
                    CompanyId = productViewModel.CompanyId,
                    CGST_Perc = productViewModel.CGST_Perc,
                    SGST_Perc = productViewModel.SGST_Perc,
                    IGST_Perc = productViewModel.IGST_Perc,
                    HSN_Code = productViewModel.HSN_Code,
                    GST_Exempt = productViewModel.GST_Exempt,
                    SizeId = productViewModel.SizeId,
                    Length = productViewModel.Length,
                    ManufacturerId = productViewModel.ManufacturerId,
                    ColourCode = productViewModel.ColourCode,
                    IsNonGST = productViewModel.IsNonGST,
                    IsWarrantyProduct = productViewModel.IsWarrantyProduct,
                    WarrantyInMonth = productViewModel.WarrantyInMonth,
                    IsNilRated = productViewModel.IsNilRated,
                    IsZeroRated = productViewModel.IsZeroRated,
                    IsThirdPartyProduct = productViewModel.IsThirdPartyProduct,
                    IsOnline = productViewModel.OnlineProduct,
                    RackNo = productViewModel.RackNo,
                    ModelName = productViewModel.ModelName,
                    CC = productViewModel.CC,
                    HSNID = productViewModel.HSNID,
                    VendorId = productViewModel.VendorId,
                    VehicleType = productViewModel.VehicleType,
                    LocalName = productViewModel.LocalName,
                    Compatibility = productViewModel.Compatibility,
                    CompanyBranchId = productViewModel.CompanyBranchId,
                    ProductSequence = 1,

                };
                responseOut = dbInterface.AddEditProduct(product);
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
        public ResponseOut UpdateProductPic(ProductViewModel productViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                Product product = new Product
                {
                    Productid = productViewModel.Productid,
                    UserPicName = productViewModel.ProductPicName,
                    UserPicPath = productViewModel.ProductPicPath
                };
                responseOut = dbInterface.UpdateProductPic(product);
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
        public List<ProductViewModel> GetProductList(string productName, int companyid, string productCode, string productShortDesc, string productFullDesc, int productTypeId, int productMainGroupId, int productSubGroupId, string assemblyType, string brandName, string fromDate, string toDate,string modelName,string hsnCode,string cc, string vendorName, string vendorCode,string localName, string compatibility,int companyBranchId)
        {
            List<ProductViewModel> products = new List<ProductViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtProducts = sqlDbInterface.GetProductList(productName, companyid, productCode, productShortDesc, productFullDesc, productTypeId, productMainGroupId, productSubGroupId, assemblyType, brandName,Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), modelName, hsnCode, cc, vendorName,  vendorCode, localName, compatibility, companyBranchId);
                if (dtProducts != null && dtProducts.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtProducts.Rows)
                    {
                        products.Add(new ProductViewModel
                        {
                            Productid = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ProductCode = Convert.ToString(dr["ProductCode"]),
                            ProductShortDesc = Convert.ToString(dr["ProductShortDesc"]),
                            ProductFullDesc = Convert.ToString(dr["ProductFullDesc"]),
                            ProductTypeId = Convert.ToInt16(dr["ProductTypeId"]),
                            ProductTypeName = Convert.ToString(dr["ProductTypeName"]),
                            ProductMainGroupId = Convert.ToInt16(dr["ProductMainGroupId"]),
                            ProductMainGroupName = Convert.ToString(dr["ProductMainGroupName"]),
                            ProductSubGroupId = Convert.ToInt16(dr["ProductSubGroupId"]),
                            ProductSubGroupName = Convert.ToString(dr["ProductSubGroupName"]),
                            AssemblyType = Convert.ToString(dr["AssemblyType"]),
                            UOMId = Convert.ToInt16(dr["UOMId"]),
                            UOMName = Convert.ToString(dr["UOMName"]),
                            PurchaseUOMId = Convert.ToInt16(dr["PurchaseUOMId"]),
                            PurchaseUOMName = Convert.ToString(dr["PurchaseUOMName"]),
                            PurchasePrice = Convert.ToDecimal(dr["PurchasePrice"]),
                            SalePrice = Convert.ToDecimal(dr["SalePrice"]),
                            IsSerializedProduct = Convert.ToBoolean(dr["IsSerializedProduct"]),

                            OnlineProduct = Convert.ToBoolean(dr["OnlineProduct"]),
                            BrandName = Convert.ToString(dr["BrandName"]),
                            ReOrderQty = Convert.ToDecimal(dr["ReOrderQty"]),
                            MinOrderQty = Convert.ToDecimal(dr["MinOrderQty"]),
                            Product_Status = Convert.ToBoolean(dr["Status"]),
                            SizeDesc = Convert.ToString(dr["SizeDesc"]),
                            Length = Convert.ToString(dr["Length"]),
                            ManufacturerName = Convert.ToString(dr["ManufacturerName"]),
                            CreatedByName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            ModelName = Convert.ToString(dr["ModelName"]),
                            CC = Convert.ToString(dr["CC"]),
                            HSN_Code = Convert.ToString(dr["HSN_Code"]),            
                            VendorName = Convert.ToString(dr["VendorName"]),
                            VendorCode = Convert.ToString(dr["VendorCode"]),
                            VehicleType = Convert.ToString(dr["VehicleType"]),
                            LocalName = Convert.ToString(dr["LocalName"]),
                            Compatibility = Convert.ToString(dr["Compatibility"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                            CompanyBranchName = Convert.ToString(dr["CompanyBranchName"]),

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
        public ProductViewModel GetProductDetail(long productId = 0)
        {
            ProductViewModel product = new ProductViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtProducts = sqlDbInterface.GetProductDetail(productId);
                if (dtProducts != null && dtProducts.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtProducts.Rows)
                    {
                        product = new ProductViewModel
                        {
                            Productid = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ProductCode = Convert.ToString(dr["ProductCode"]),
                            ProductShortDesc = Convert.ToString(dr["ProductShortDesc"]),
                            ProductFullDesc = Convert.ToString(dr["ProductFullDesc"]),
                            ProductTypeId = Convert.ToInt16(dr["ProductTypeId"]),
                            ProductTypeName = Convert.ToString(dr["ProductTypeName"]),
                            ProductMainGroupId = Convert.ToInt16(dr["ProductMainGroupId"]),
                            ProductMainGroupName = Convert.ToString(dr["ProductMainGroupName"]),
                            ProductSubGroupId = Convert.ToInt16(dr["ProductSubGroupId"]),
                            ProductSubGroupName = Convert.ToString(dr["ProductSubGroupName"]),
                            AssemblyType = Convert.ToString(dr["AssemblyType"]),
                            UOMId = Convert.ToInt16(dr["UOMId"]),
                            UOMName = Convert.ToString(dr["UOMName"]),
                            PurchaseUOMId = Convert.ToInt16(dr["PurchaseUOMId"]),
                            PurchaseUOMName = Convert.ToString(dr["PurchaseUOMName"]),
                            PurchasePrice = Convert.ToDecimal(dr["PurchasePrice"]),
                            SalePrice = Convert.ToDecimal(dr["SalePrice"]),
                            IsSerializedProduct = Convert.ToBoolean(dr["IsSerializedProduct"]),
                            BrandName = Convert.ToString(dr["BrandName"]),
                            ReOrderQty = Convert.ToDecimal(dr["ReOrderQty"]),
                            MinOrderQty = Convert.ToDecimal(dr["MinOrderQty"]),
                            Product_Status = Convert.ToBoolean(dr["Status"]),
                            ProductPicPath = Convert.ToString(dr["UserPicPath"]),
                            ProductPicName = Convert.ToString(dr["UserPicName"]),
                            CGST_Perc = Convert.ToDecimal(dr["CGST_Perc"]),
                            SGST_Perc = Convert.ToDecimal(dr["SGST_Perc"]),
                            IGST_Perc = Convert.ToDecimal(dr["IGST_Perc"]),
                            HSN_Code = Convert.ToString(dr["HSN_Code"]),
                            GST_Exempt = Convert.ToBoolean(dr["GST_Exempt"]),
                            IsNonGST = Convert.ToBoolean(dr["IsNonGST"]),
                            SizeId = Convert.ToInt32(dr["SizeId"]),
                            SizeCode = Convert.ToString(dr["SizeCode"]),
                            SizeDesc = Convert.ToString(dr["SizeDesc"]),
                            Length = Convert.ToString(dr["Length"]),
                            ManufacturerId = Convert.ToInt32(dr["ManufacturerId"]),
                            ManufacturerCode = Convert.ToString(dr["ManufacturerCode"]),
                            ManufacturerName = Convert.ToString(dr["ManufacturerName"]),
                            ColourCode = Convert.ToString(dr["ColourCode"]),
                            IsWarrantyProduct = Convert.ToBoolean(dr["IsWarrantyProduct"]),
                            WarrantyInMonth = Convert.ToInt32(dr["WarrantyInMonth"]),
                            IsZeroRated=Convert.ToBoolean(dr["IsZeroRated"]),
                            IsNilRated = Convert.ToBoolean(dr["IsNilRated"]),
                            IsThirdPartyProduct = Convert.ToBoolean(dr["IsThirdPartyProduct"]),
                            OnlineProduct = Convert.ToBoolean(dr["OnlineProduct"]),
                            RackNo = Convert.ToString(dr["RackNo"]),
                            ModelName = Convert.ToString(dr["ModelName"]),
                            CC = Convert.ToString(dr["CC"]),
                            HSNID = Convert.ToInt32(dr["HSNID"]),
                            VendorId = Convert.ToInt32(dr["VendorId"]),
                            VendorName = Convert.ToString(dr["VendorName"]),
                            VendorCode =Convert.ToString(dr["VendorCode"]),
                            VehicleType = Convert.ToString(dr["VehicleType"]),
                            LocalName = Convert.ToString(dr["LocalName"]),
                            Compatibility = Convert.ToString(dr["Compatibility"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return product;
        }

        public List<ProductViewModel> GetProductAutoCompleteList(string searchTerm,int companyId)
        {
            List<ProductViewModel> products = new List<ProductViewModel>();
            try
            {
                List<Product> productList = dbInterface.GetProductAutoCompleteList(searchTerm, companyId);
                if (productList != null && productList.Count > 0)
                {
                    foreach (Product product in productList)
                    {
                        products.Add(new ProductViewModel { Productid = product.Productid,
                             ProductName = product.ProductName,
                            ProductCode =product.ProductCode,
                            ProductShortDesc =product.ProductShortDesc,
                            CGST_Perc = Convert.ToDecimal(product.CGST_Perc),
                            SGST_Perc = Convert.ToDecimal(product.SGST_Perc),
                            IGST_Perc = Convert.ToDecimal(product.IGST_Perc),
                            HSN_Code = product.HSN_Code ,
                            IsSerializedProduct=Convert.ToBoolean(product.IsSerializedProduct),
                            IsThirdPartyProduct = Convert.ToBoolean(product.IsThirdPartyProduct),
                            IsWarrantyProduct = Convert.ToBoolean(product.IsWarrantyProduct),
                            WarrantyInMonth =Convert.ToInt32(product.WarrantyInMonth),
                            SalePrice = Convert.ToInt32(product.SalePrice)
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
        public List<ProductViewModel> GetProductAutoCompleteList(string searchTerm, int companyId, string assemblyType)
        {
            List<ProductViewModel> products = new List<ProductViewModel>();
            try
            {
                List<Product> productList = dbInterface.GetProductAutoCompleteList(searchTerm, companyId, assemblyType);
                if (productList != null && productList.Count > 0)
                {
                    foreach (Product product in productList)
                    {
                        products.Add(new ProductViewModel { Productid = product.Productid, ProductName = product.ProductName, ProductCode = product.ProductCode, ProductShortDesc = product.ProductShortDesc });
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

        public List<ProductSubGroupViewModel> GetProductSubGroupAutoCompleteList(string searchTerm, int companyId)
        {
            List<ProductSubGroupViewModel> productSubGroups = new List<ProductSubGroupViewModel>();
            try
            {
                List<ProductSubGroup> productSubGroupList = dbInterface.GetProductSubGroupAutoCompleteList(searchTerm, companyId);
                if (productSubGroupList != null && productSubGroupList.Count > 0)
                {
                    foreach (ProductSubGroup product in productSubGroupList)
                    {
                        productSubGroups.Add(new ProductSubGroupViewModel { ProductSubGroupId = product.ProductSubGroupId, ProductSubGroupName = product.ProductSubGroupName, ProductSubGroupCode = product.ProductSubGroupCode});
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
        public List<ProductViewModel> GetSubAssemblyAutoCompleteList(string searchTerm, int companyId)
        {
            List<ProductViewModel> products = new List<ProductViewModel>();
            try
            {
                List<Product> productList = dbInterface.GetSubAssemblyAutoCompleteList(searchTerm, companyId);
                if (productList != null && productList.Count > 0)
                {
                    foreach (Product product in productList)
                    {
                        products.Add(new ProductViewModel { Productid = product.Productid, ProductName = product.ProductName, ProductCode = product.ProductCode, ProductShortDesc = product.ProductShortDesc });
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

        public ResponseOut ImportProduct(ProductViewModel productViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                Product product = new Product
                {
                    Productid = productViewModel.Productid,
                    ProductName = productViewModel.ProductName,
                    ProductCode = productViewModel.ProductCode,
                    ProductShortDesc = productViewModel.ProductShortDesc,
                    ProductFullDesc = productViewModel.ProductFullDesc,
                    ProductTypeId = productViewModel.ProductTypeId,
                    ProductMainGroupId = productViewModel.ProductMainGroupId,
                    ProductSubGroupId = productViewModel.ProductSubGroupId,
                    AssemblyType = productViewModel.AssemblyType,
                    UOMId = productViewModel.UOMId,
                    PurchasePrice = productViewModel.PurchasePrice,
                    SalePrice = productViewModel.SalePrice,
                    LocalTaxRate = productViewModel.LocalTaxRate,
                    CentralTaxRate = productViewModel.CentralTaxRate,
                    OtherTaxRate = productViewModel.OtherTaxRate,
                    IsSerializedProduct = productViewModel.IsSerializedProduct,
                    BrandName = productViewModel.BrandName,
                    ReOrderQty = productViewModel.ReOrderQty,
                    MinOrderQty = productViewModel.MinOrderQty,

                    UserPicName = productViewModel.ProductPicName,
                    UserPicPath = productViewModel.ProductPicPath, 
                    SizeId = productViewModel.SizeId, 
                    Length = productViewModel.Length,
                    ManufacturerId = productViewModel.ManufacturerId,

                    CGST_Perc = productViewModel.CGST_Perc,
                    SGST_Perc = productViewModel.SGST_Perc,
                    IGST_Perc = productViewModel.IGST_Perc,
                    HSN_Code = productViewModel.HSN_Code,
                    GST_Exempt = productViewModel.GST_Exempt,
                    CreatedBy = productViewModel.CreatedBy,
                    Status = productViewModel.Product_Status,
                    CompanyId = productViewModel.CompanyId
                };
                responseOut = dbInterface.AddEditProduct(product);
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

        public List<ReorderPointProductCountViewModel> GetReorderPointProductCountList(string productName , string productCode , string productShortDesc, string productFullDesc, int companyId, int finYearId)
        {
            List<ReorderPointProductCountViewModel> products = new List<ReorderPointProductCountViewModel>();
            
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtProducts = sqlDbInterface.GetReorderPointProductCountList(productName, productCode, productShortDesc, productFullDesc, companyId,finYearId);
                if (dtProducts != null && dtProducts.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtProducts.Rows)
                    {
                        products.Add(new ReorderPointProductCountViewModel
                        {
                            
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ProductCode= Convert.ToString(dr["ProductCode"]),
                            ProductShortDesc = Convert.ToString(dr["ProductShortDesc"]),
                            ProductFullDesc = Convert.ToString(dr["ProductFullDesc"]),
                            ReorderQty = Convert.ToInt32(dr["ReOrderQty"]),
                            AvailableStock = Convert.ToInt32(dr["AvailableStock"])
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

        public ProductViewModel GetProductTaxPercentage(long productId)
        {
            ProductViewModel productViewModel = new ProductViewModel();
            try
            {
                Product productList = dbInterface.GetProductTaxPercentage(productId);
                if (productList != null)
                {
                    productViewModel = new ProductViewModel
                    {                       
                        CGST_Perc = Convert.ToDecimal(productList.CGST_Perc),
                        SGST_Perc = Convert.ToDecimal(productList.SGST_Perc),
                        IGST_Perc = Convert.ToDecimal(productList.IGST_Perc),
                       
                    };

                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return productViewModel;
        }


        public ResponseOut RemoveImage(long productId)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                responseOut = dbInterface.RemoveImage(productId);
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

        public DataTable GenerateProductsOpeningStockReports(int companyId, int companyBranchId,int currentFinYear)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtProductsOpeningStock = new DataTable();
            try
            {
                dtProductsOpeningStock = sqlDbInterface.GenerateProductsOpeningStockReports(companyId, companyBranchId, currentFinYear);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtProductsOpeningStock;
        }
        public DataTable GeneratePhysicalStock(int companyId)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtProductsOpeningStock = new DataTable();
            try
            {
                dtProductsOpeningStock = sqlDbInterface.GeneratePhysicalStock(companyId );
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtProductsOpeningStock;
        }

        public ProductViewModel GetAutoCompleteProductDetail(long productId = 0)
        {
            ProductViewModel product = new ProductViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtProducts = sqlDbInterface.GetAutoCompleteProductDetail(productId);
                if (dtProducts != null && dtProducts.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtProducts.Rows)
                    {
                        product = new ProductViewModel
                        {
                            Productid = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ProductCode = Convert.ToString(dr["ProductCode"]),
                            UOMId = Convert.ToInt16(dr["UOMId"]),
                            UOMName = Convert.ToString(dr["UOMName"]),
                            SalePrice = Convert.ToDecimal(dr["SalePrice"]),
                            BrandName = Convert.ToString(dr["BrandName"]),
                            CGST_Perc = Convert.ToDecimal(dr["CGST_Perc"]),
                            SGST_Perc = Convert.ToDecimal(dr["SGST_Perc"]),
                            HSN_Code = Convert.ToString(dr["HSN_Code"])
                           
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return product;
        }

        public List<ProductViewModel> GetComplaintProductAutoCompleteList(string searchTerm)
        {
            
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            List<ProductViewModel> products = new List<ProductViewModel>();
            try
            {
                DataTable productList = sqlDbInterface.GetComplaintProductAutoCompleteList(searchTerm);
                if (productList != null && productList.Rows.Count > 0)
                {
                    foreach (DataRow dr in productList.Rows)
                    {
                        products.Add(new ProductViewModel
                        {
                            Productid = Convert.ToInt32(dr["Productid"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ProductCode = Convert.ToString(dr["ProductCode"]),
                            WarrantyStartDate = Convert.ToString(dr["WarrantyStartDate"]),
                            WarrantyEndDate = Convert.ToString(dr["WarrantyEndDate"]),
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

        public List<ProductViewModel> GetProductTypeBYProductAutoCompleteList(string searchTerm, int companyId, int productTypeid)
        {
            List<ProductViewModel> products = new List<ProductViewModel>();
            try
            {
                List<Product> productList = dbInterface.GetProductTypeBYProductAutoCompleteList(searchTerm, companyId, productTypeid);
                if (productList != null && productList.Count > 0)
                {
                    foreach (Product product in productList)
                    {
                        products.Add(new ProductViewModel
                        {
                            Productid = product.Productid,
                            ProductName = product.ProductName,
                            ProductCode = product.ProductCode                          
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
        public string UpdateAndCancelOnlineCode(long productId = 0,string status="")
        {
            ProductViewModel product = new ProductViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            string str = "";
            try
            {
                str = sqlDbInterface.UpdateAndCancelOnlineCode(productId,status);
               
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
