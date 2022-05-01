using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{

    public partial class PMS_PerformanceCycleViewModel
    {
        public int PerformanceCycleId { get; set; }
        public string PerformanceCycleName { get; set; }
        public bool PerformanceCycle_Status { get; set; }
        public string message { get; set; }
        public string status { get; set; }
    }
    
}
