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
    
    public partial class HR_EmployeeAttendance
    {
        public long EmployeeAttendanceId { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<long> EmployeeId { get; set; }
        public Nullable<System.DateTime> AttendanceDate { get; set; }
        public string PresentAbsent { get; set; }
        public Nullable<System.DateTime> TrnDateTime { get; set; }
        public string AttendanceStatus { get; set; }
        public Nullable<int> AttendanceApprovedBy { get; set; }
        public Nullable<System.DateTime> AttendanceApproveDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string InOut { get; set; }
        public Nullable<int> CompanyBranchId { get; set; }
    }
}