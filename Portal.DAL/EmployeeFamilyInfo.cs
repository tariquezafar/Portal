//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Portal.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class EmployeeFamilyInfo
    {
        public long FamilyInfoId { get; set; }
        public Nullable<int> EmployeeId { get; set; }
        public Nullable<int> RelationId { get; set; }
        public string RelativeName { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
    }
}
