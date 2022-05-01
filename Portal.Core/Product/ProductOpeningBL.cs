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
    public class ProductOpeningBL
    {
        DBInterface dbInterface;
        public ProductOpeningBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditProductOpening(ProductOpeningViewModel productOpeningViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                ProductOpeningStock productOpening = new ProductOpeningStock
                {
                    OpeningTrnId= productOpeningViewModel.OpeningTrnId,
                    ProductId = productOpeningViewModel.ProductId,
                    FinYearId= productOpeningViewModel.FinYearId,
                    CompanyBranchId = productOpeningViewModel.CompanyBranchId,
                    OpeningQty =productOpeningViewModel.OpeningQty,
                    CreatedBy = productOpeningViewModel.CreatedBy,
                    CompanyId= productOpeningViewModel.CompanyId
                };
                responseOut = dbInterface.AddEditProductOpening(productOpening);
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
        public List<ProductOpeningViewModel> GetProductOpeningList(string productName, int companyid, int finYearId,int companyBranchId)
        {
            List<ProductOpeningViewModel> productOpenings = new List<ProductOpeningViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtProducts = sqlDbInterface.GetProductOpeningList(productName, companyid,finYearId, companyBranchId);
                if (dtProducts != null && dtProducts.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtProducts.Rows)
                    {
                        productOpenings.Add(new ProductOpeningViewModel
                        {
                            OpeningTrnId = Convert.ToInt32(dr["OpeningTrnId"]),
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ProductCode = Convert.ToString(dr["ProductCode"]),
                            ProductShortDesc = Convert.ToString(dr["ProductShortDesc"]),
                            FinYearId = Convert.ToInt16(dr["FinYearId"]),
                            CompanyBranchId = Convert.ToInt16(dr["CompanyBranchId"]),
                            BranchName = Convert.ToString(dr["BranchName"]),
                            FinYearDesc = Convert.ToString(dr["FinYearDesc"]),
                            OpeningQty = Convert.ToDecimal(dr["OpeningQty"])

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return productOpenings;
        }

        public ProductOpeningViewModel GetProductOpeningDetail(long openingTrnId = 0)
        {
            ProductOpeningViewModel productOpening = new ProductOpeningViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtProducts = sqlDbInterface.GetProductOpeningDetail(openingTrnId);
                if (dtProducts != null && dtProducts.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtProducts.Rows)
                    {
                        productOpening = new ProductOpeningViewModel
                        {
                            OpeningTrnId = Convert.ToInt32(dr["OpeningTrnId"]),
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ProductCode = Convert.ToString(dr["ProductCode"]),
                            ProductShortDesc = Convert.ToString(dr["ProductShortDesc"]),
                            FinYearId = Convert.ToInt16(dr["FinYearId"]),
                            FinYearDesc = Convert.ToString(dr["FinYearDesc"]),
                            CompanyBranchId = Convert.ToInt16(dr["CompanyBranchId"]),
                            BranchName = Convert.ToString(dr["BranchName"]),
                            OpeningQty = Convert.ToDecimal(dr["OpeningQty"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return productOpening;
        }
        public ResponseOut ImportProductOpening(ProductOpeningViewModel productOpeningViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                ProductOpeningStock productOpening = new ProductOpeningStock
                {
                    OpeningTrnId = productOpeningViewModel.OpeningTrnId,
                    ProductId = productOpeningViewModel.ProductId,
                    FinYearId = productOpeningViewModel.FinYearId,
                    CompanyBranchId = productOpeningViewModel.CompanyBranchId,
                    OpeningQty = productOpeningViewModel.OpeningQty,
                    CreatedBy = productOpeningViewModel.CreatedBy,
                    CompanyId = productOpeningViewModel.CompanyId
                };
                responseOut = dbInterface.AddEditProductOpening(productOpening);
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
