using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class ProductSubGroupViewModel:IModel
    {
        public int ProductSubGroupId { get; set; }
        public string ProductSubGroupName { get; set; }
        public string ProductSubGroupCode { get; set; }
        public int ProductMainGroupId { get; set; }
        public string ProductMainGroupName { get; set; }
        public bool ProductSubGroup_Status { get; set; }
        public string message { get; set; }
        public string status { get; set; }

    }
    
}
