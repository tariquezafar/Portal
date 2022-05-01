using Portal.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
    public class ServiceViewModel:IModel
    {
        public int SequenceNo { get; set; }
        public long ServiceItemId { get; set; }
        public string ServiceItemName { get; set; }       
        public string ProductName { get; set; }
        public long ProductId { get; set; }
        public long ProductTypeID { get; set; }
        public string Notes { get; set; }
    }

    public class ServicViewModel : IModel
    {
        public long ServiceId { get; set; }      
        public string ServiceNo { get; set; }

        public string ServiceDate { get; set; }
        public string ApprovalStatus { get; set; }
        public int CompanyId { get; set; }
        public string CreatedByUserName { get; set; }
        public string CreatedDate { get; set; }

        public int CreatedBy { get; set; }
        
        public int Modifiedby { get; set; }
        public string ModifiedByUserName { get; set; }
        public string ModifiedDate { get; set; }

    }
}
