using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
   public class CustomerTypeViewModel:IModel
    {
        
        public int CustomerTypeId { get; set; }
        public string CustomerTypeDesc { get; set; }
        public bool CustomerType_Status { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public int CompanyBranchId { get; set; }
        public string CompanyBranchName { get; set; }
    }
}
