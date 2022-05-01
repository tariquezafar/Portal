using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class ProductOpeningViewModel:IModel
    {
        
        public long OpeningTrnId { get; set; }
        public int FinYearId { get; set; }
        public string FinYearDesc { get; set; }
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ProductShortDesc { get; set; }
        public decimal OpeningQty { get; set; }       
        public int CompanyId { get; set; }
        public int CompanyBranchId { get; set; }
        public int CreatedBy { get; set; }
        public string BranchName { get; set; }
        public string CreatedByName { get; set; }
        public string CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public string ModifiedByName { get; set; }
        public string ModifiedDate { get; set; }
        public string message { get; set; }
        public string status { get; set; }

    }
    
}
