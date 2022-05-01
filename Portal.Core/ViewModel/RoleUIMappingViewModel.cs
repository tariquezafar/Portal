using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class RoleUIMappingViewModel:IModel
    {
        public long RoleUIMappingId { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int InterfaceId { get; set; }
        public string InterfaceName { get; set; }
        public bool AddAccess { get; set; }
        public bool EditAccess { get; set; }
        public bool ViewAccess { get; set; }
        public bool Mapping_Status { get; set; }

        public string message { get; set; }
        public string status { get; set; }
        public bool CancelAccess { get; set; }
        public bool ReviseAccess { get; set; }

        public string ParentName { get; set; }
        public int CompanyBranchId { get; set; }
        public string CompanyBranchName { get; set; }
    }
    
}
