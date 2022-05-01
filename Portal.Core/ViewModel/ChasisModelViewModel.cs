using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class ChasisModelViewModel: IModel
    {
        public long ChasisModelID { get; set; }
        public int ProductSubGroupId { get; set; }
        public string ProductSubGroupName { get; set; }
        public string ChasisModelName { get; set; }    
        public string ChasisModelCode { get; set; }
        public string MotorModelCode { get; set; }
        public bool ChasisModelStatus { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public int CompanyBranchId { get; set; }
        public string CompanyBranchName { get; set; }

    }
    
}
