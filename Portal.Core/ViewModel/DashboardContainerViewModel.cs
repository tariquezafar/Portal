using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
    public class DashboardContainerViewModel
    {
        public long DashboardContainterID { get; set; }        
        public string ContainerName { get; set; }
        public string ContainerDisplayName { get; set; }
        public int ContainterNo { get; set; }
        public int TotalItem { get; set; }
        public string ModuleName { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public int CompanyBranchId { get; set; }
        public string CompanyBranchName { get; set; }

    }
}
