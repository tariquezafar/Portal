using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class ProductTypeViewModel:IModel
    {
        public int ProductTypeId { get; set; }
        public string ProductTypeName { get; set; }
        public string ProductTypeCode { get; set; }
        public bool ProductType_Status { get; set; }
        public string message { get; set; }
        public string status { get; set; }

        public int CompanyBranchId { get; set; }
        public string CompanyBranchName { get; set; }


    }

}
