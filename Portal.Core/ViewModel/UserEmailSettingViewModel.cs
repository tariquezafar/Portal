using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
   public class UserEmailSettingViewModel
    {
        public long SettingId { get; set; }
        public int UserId { get; set; }

        public string FullName { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPass { get; set; }
        public string SmtpServer { get; set; }
        public bool EnableSsl { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpDisplayName { get; set; }
        public bool Status { get; set; }
        public bool UserStatus { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
        public string ModifiedByName { get; set; }
        public int CompanyBranchId { get; set; }
        public string CompanyBranchName { get; set; }
    }
}
