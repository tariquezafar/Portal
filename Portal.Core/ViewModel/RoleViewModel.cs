using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class RoleViewModel:IModel
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleDesc { get; set; }
        public bool IsAdmin { get; set; }
        public int CompanyId { get; set; }
        public bool Role_Status { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public int CompanyBranchId { get; set; }
        public string CompanyBranchName { get; set; }
        public int UserTypeId { get; internal set; }
    }
    
}
