using System.Text;
using System.Threading.Tasks;
using Portal.Core.ViewModel;
using Portal.DAL;
using Portal.Common;
using System.Reflection;
using System.Data;
using System.Collections.Generic;
using System;

namespace Portal.Core
{
    public class HRDashboardBL
    {
        DBInterface dbInterface;
        public HRDashboardBL()
        {
            dbInterface = new DBInterface();
        }
        public List<InventoryDashboardItemsViewModel> GetHRDashboardItems(int roleId, int companyId, int companyBranchId, int finYearId)
        {
            List<InventoryDashboardItemsViewModel> inventoryDashboardItems = new List<InventoryDashboardItemsViewModel>();
            SQLDbInterface sqldbinterface = new SQLDbInterface();
            try
            {
                DataTable dtInventoryDashboardItemList = sqldbinterface.HRDashboardItems(roleId, companyId, companyBranchId, finYearId);

                if (dtInventoryDashboardItemList != null && dtInventoryDashboardItemList.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtInventoryDashboardItemList.Rows)
                    {
                        inventoryDashboardItems.Add(new InventoryDashboardItemsViewModel
                        {
                            SrNo = Convert.ToInt32(dr["SrNo"]),
                            ContainerItemKey = Convert.ToString(dr["ContainerItemKey"]),
                            ContainerItemValue = Convert.ToString(dr["ContainerItemValue"]),
                            BoxNumber = Convert.ToString(dr["BoxNumber"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return inventoryDashboardItems;
        }
    }
}
