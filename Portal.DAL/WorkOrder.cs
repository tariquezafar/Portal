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
    
    public partial class WorkOrder
    {
        public long WorkOrderId { get; set; }
        public string WorkOrderNo { get; set; }
        public Nullable<System.DateTime> WorkOrderDate { get; set; }
        public Nullable<System.DateTime> TargetFromDate { get; set; }
        public Nullable<System.DateTime> TargetToDate { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> CompanyBranchId { get; set; }
        public string Remarks1 { get; set; }
        public string Remarks2 { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string WorkOrderStatus { get; set; }
        public Nullable<int> WorkOrderSequence { get; set; }
        public Nullable<long> SOId { get; set; }
        public string SONo { get; set; }
        public string CancelStatus { get; set; }
        public Nullable<int> CancelBy { get; set; }
        public Nullable<System.DateTime> CancelDate { get; set; }
        public string CancelReason { get; set; }
        public Nullable<int> LocationId { get; set; }
    }
}
