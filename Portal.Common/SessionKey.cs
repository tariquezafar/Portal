using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Common
{
    public class SessionKey
    {
        public const String CurrentUser = "CurrentUser";
        public const String MyMenu = "MyMenu";
        public const String IsAuthenticated = "IsAuthenticated";
        public const String UserFullName = "UserFullName";
        public const String RoleWiseUIMatrix = "RoleWiseUIMatrix";
        public const String CurrentFinYear = "CurrentFinYear";
        public const String UserPicName = "UserPicName";
        public const String UserEmail = "UserEmail";
        public const String CurrentEmployee = "CurrentEmployee";
        public const String EmployeeFullName = "EmployeeFullName";
        public const String EmployeeId = "EmployeeId";
        public const String IsAdmin = "IsAdmin";
        public const String CompanyBranchId = "CompanyBranchId";
        public const String UserId = "UserId";
        public const String CompanyBranchList = "CompanyBranchList";
        public const String CompanyBranchName = "CompanyBranchName";
        public const String CustomerId = "CustomerId";
    }
    public class CookieKey
    {
        public const String Username = "Username";
        public const String Password = "Password";
        public const String StaySignedIn = "StaySignedIn";
    }
    public class CacheKey
    {
        public const String CityCache = "City_Cache";
    }
}
