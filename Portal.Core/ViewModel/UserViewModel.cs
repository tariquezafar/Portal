using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class UserViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string OldPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public string ModifiedByName { get; set; }
        public string ModifiedDate { get; set; }
        public string ExpiryDate { get; set; }
        public bool UserStatus { get; set; }
        public string UserPicPath { get; set; }
        public string UserPicName { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public string CompanyBranchName { get; set; }
        public long CompanyBranchId { get; set; }
        public int FK_CustomerId { get; set; }
      


    }
    public class ParentMenu
    {
        public int InterfaceId { get; set; }
        public string InterfaceName { get; set; }
        public string InterfaceURL { get; set; }
        public string InterfaceType { get; set; }
        public string InterfaceSubType { get; set; }
        public List<ChildMenu> childMenuList { get; set; }
    }
    public class ChildMenu
    {
      public int InterfaceId { get; set; }
      public string InterfaceName { get; set; }
      public string InterfaceURL { get; set; }
      public string InterfaceType { get; set; }
      public string InterfaceSubType { get; set; }
    }
}
