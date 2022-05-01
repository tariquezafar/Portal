using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class ProductMainGroupViewModel:IModel
    {
        public int ProductMainGroupId { get; set; }
        public string ProductMainGroupName { get; set; }
        public string ProductMainGroupCode { get; set; }
        public bool ProductMainGroup_Status { get; set; }
        public string message { get; set; }
        public string status { get; set; }

    }
    
}
