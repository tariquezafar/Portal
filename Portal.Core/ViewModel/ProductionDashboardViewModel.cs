using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
    public class ProductionDashboardViewModel
    {
    }
    public class SOPendingViewModel
    {
        public int SOId { get; set; }
        public string SONo { get; set; }
        public string SODate { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public string QuotationNo { get; set; }
        public string ApprovalStatus { get; set; }
        public string CreatedByName { get; set; }
        public string ModifiedByName { get; set; }
    }
    public class ProductionSummaryReportViewModel
    {
        public int Sno { get; set; }
        public string Nature { get; set; }
        public int TotalValue { get; set; }
    }
    public class ProductionCommanCountViewModel
    {
        public string TodayCount { get; set; }
        public string TotalCount { get; set; }
    }

    public class ProductionFullCommanCountViewModel
    {

        public string TodayProductBOMCount { get; set; }
        public string TotalProductBOMCount { get; set; }
        public string TodayPendingWorkOrderCount { get; set; }
        public string TotalPendingWorkOrderCount { get; set; }
        public string TodayFinishedGoodCount { get; set; }
        public string TotalFinishedGoodCount { get; set; }
        public string TodayFabricationCount { get; set; }
        public string TotalFabricationCount { get; set; }
        public string Total_no_of_so_rec_sdept { get; set; }
        public string Total_no_of_work_order_gen_so { get; set; }
        public string Total_no_of_work_order_gen_with_so { get; set; }
        public string  Total_no_of_incom_vej_online { get; set; }
        public string Total_no_of_Work_Order_Pending_ForSo { get; set; }
        public string Total_no_of_Work_Order_Pending { get; set; }
        public string  Total_no_of_veh_in_fin_good { get; set; }

    }
}
