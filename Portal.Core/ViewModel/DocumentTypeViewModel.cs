using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
   public class DocumentTypeViewModel:IModel
    {  
        public int DocumentTypeId { get; set; }
        public string DocumentTypeDesc { get; set; }
        public bool DocumentType_Status { get; set; }
        public int CompanyId { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public int CompanyBranchId { get; set; }
        public string CompanyBranchName { get; set; }
    }
}
