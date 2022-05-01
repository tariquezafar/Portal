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
    public class ManufacturerBL
    {
        DBInterface dbInterface;
        public ManufacturerBL()
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




        public ResponseOut ImportManufacturer(ManufacturerViewModel manufacturerViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                Manufacturer manufacturer = new Manufacturer
                {
                    ManufacturerId = manufacturerViewModel.ManufacturerId,
                   ManufacturerCode = manufacturerViewModel.ManufacturerCode,
                    ManufacturerName = manufacturerViewModel.ManufacturerName, 
                    CreatedBy = manufacturerViewModel.CreatedBy,
                    Status = manufacturerViewModel.Manufacturer_Status,
                };
                responseOut = dbInterface.AddEditManufacturer(manufacturer);
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
         


        public List<ManufacturerViewModel> GetManufacturerAutoCompleteList(string searchTerm)
        {
            List<ManufacturerViewModel> manufacturers = new List<ManufacturerViewModel>();
            try
            {
                List<Manufacturer> manufacturerList = dbInterface.GetManufacturerAutoCompleteList(searchTerm);

                if (manufacturerList != null && manufacturerList.Count > 0)
                {
                    foreach (Manufacturer manufacturer in manufacturerList)
                    {
                        manufacturers.Add(new ManufacturerViewModel { ManufacturerId = manufacturer.ManufacturerId, ManufacturerCode = manufacturer.ManufacturerCode, ManufacturerName = manufacturer.ManufacturerName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return manufacturers;
        }

        public ResponseOut AddEditManufacturer(ManufacturerViewModel manufacturerViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                Manufacturer Manufacturer = new Manufacturer
                {
                    ManufacturerId = manufacturerViewModel.ManufacturerId,
                    ManufacturerName = manufacturerViewModel.ManufacturerName,
                    ManufacturerCode = manufacturerViewModel.ManufacturerCode,
                    Status = manufacturerViewModel.Manufacturer_Status,
                    CreatedBy=manufacturerViewModel.CreatedBy,
                };
                responseOut = dbInterface.AddEditManufacturer(Manufacturer);
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

        public List<ManufacturerViewModel> GetManufacturerList(string manufacturerName = "", string manufacturerCode = "", string Status = "")
        {
            List<ManufacturerViewModel> manufacturerViewModel = new List<ManufacturerViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCLaimTypes = sqlDbInterface.GetManufacturerList(manufacturerName, manufacturerCode, Status);
                if (dtCLaimTypes != null && dtCLaimTypes.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCLaimTypes.Rows)
                    {
                        manufacturerViewModel.Add(new ManufacturerViewModel
                        {
                            ManufacturerId = Convert.ToInt32(dr["ManufacturerId"]),
                            ManufacturerName = Convert.ToString(dr["ManufacturerName"]),
                            ManufacturerCode = Convert.ToString(dr["ManufacturerCode"]),
                            Manufacturer_Status = Convert.ToBoolean(dr["Status"]),
                            CreatedByUserName=Convert.ToString(dr["CreatedByName"]),
                            CreatedDate=Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return manufacturerViewModel;
        }

        public ManufacturerViewModel GetManufacturereDetail(int manufacturerId = 0)
        {
            ManufacturerViewModel manufacturerViewModel = new ManufacturerViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtClaimTypes = sqlDbInterface.GetManufacturereDetail(manufacturerId);
                if (dtClaimTypes != null && dtClaimTypes.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtClaimTypes.Rows)
                    {
                        manufacturerViewModel = new ManufacturerViewModel
                        {
                            ManufacturerId = Convert.ToInt32(dr["ManufacturerId"]),
                            ManufacturerName = Convert.ToString(dr["ManufacturerName"]),
                            ManufacturerCode = Convert.ToString(dr["ManufacturerCode"]),
                            Manufacturer_Status = Convert.ToBoolean(dr["Status"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return manufacturerViewModel;
        }


    }
}
