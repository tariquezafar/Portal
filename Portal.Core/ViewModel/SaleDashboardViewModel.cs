using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
 public   class SaleDashboardViewModel
    {
        public List<QuatationCountViewModel> bookBalanceList { get; set; }
    }
    public class QuatationCountViewModel
    {       
        public string Count_Head { get; set; }            
        public string TotalCount { get; set; }
    }

    public class SOCountViewModel
    {
        public string sOCount_Head { get; set; }
        public string sOTotalCount { get; set; }
    }
    public class SICountViewModel
    {
        public string SICount_Head { get; set; }
        public string SITotalCount { get; set; }
        public string SITotalAmountSum { get; set; }
        
    }
    
    public class SaleDashboardCountViewModel
    {
        public string TotalInvoicePackingCount { get; set; }
        public string TotalSaleReturn { get; set; }
        public string TotalSaleTarget { get; set; }
        public string TotalSaleAmount { get; set; }
    }

    public class SalePendingPaymentCountViewModel
    {
        public string salePendingInvoiceCount { get; set; }
        public string salePendingInvoiceAmount { get; set; }
    }

    public class SaleDashboardTargetAmountViewModel
    {
        public string TargetAmount { get; set; }
        public string TotalInvoiceAmount { get; set; }
    }
    public class Container9ViewModel
    {
        public string ContainerKey { get; set; }
        public string ContainerValue { get; set; }
    }
}


