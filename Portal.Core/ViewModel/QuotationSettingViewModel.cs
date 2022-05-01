using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class QuotationSettingViewModel : IModel
    {   

        public int QuotationSettingId { get; set; }
        public bool NormalApprovalRequired { get; set; }
        public int NormalApprovalByUserId { get; set; }
        public string NormalApprovalByUserName { get; set; }
        public bool RevisedApprovalRequired { get; set; }
        public int RevisedApprovalByUserId { get; set; }
        public string RevisedApprovalByUserName { get; set; }
        public int CompanyId { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedName { get; set; }
        public string CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public string ModifiedName { get; set; }
        public string ModifiedDate { get; set; }
        public bool QuotationSetting_Status { get; set; }
        public string message { get; set; }
        public Nullable<bool> status { get; set; }



    }
    
}
