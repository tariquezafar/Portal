using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
    public class DashboardItemMappingViewModel
    {
        public long DashboardItemMappingID { get; set; }
        public Int64 DashboardItemId { get; set; }
        public string ModuleName { get; set; }
        public int ContainerID { get; set; }
        public int RoleId { get; set; }      
        public string message { get; set; }
        public string status { get; set; }
        public int CompanyBranchId { get; set; }
        public string CompanyBranchName { get; set; }
        public string SequenceNo { get; set; }


        public string ItemName { get; set; }
        public string ItemDisplayName { get; set; }
        public string ContainerName { get; set; }
        public int ContainerNo { get; set; }

        public bool MappingStatus { get; set; }
        

    }


}
