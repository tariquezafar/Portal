using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;

namespace Portal.Core.ViewModel
{
    public class DashboardInterfaceViewModel : IModel
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public string ModuleName { get; set; }
        public string ContainerNo { get; set; }
        public string ContainerName { get; set; }
        public bool Status { get; set; }
        public int CompanyBranchId { get; set; }
        public int SequenceNo { get; set; }
        public string CompanyBranchName { get; set; }
    }
}
