//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Portal.DAL.Infrastructure
{
    using System;
    using System.Collections.Generic;
    
    public partial class HR_EmployeeClearanceProcessDetail
    {
        public long EmployeeClearanceDetailId { get; set; }
        public Nullable<long> EmployeeClearanceId { get; set; }
        public Nullable<int> SeparationClearListId { get; set; }
        public Nullable<int> ClearanceByUserId { get; set; }
        public string ClearanceStatus { get; set; }
        public string ClearanceRemarks { get; set; }
    }
}
