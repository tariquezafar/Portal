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
    
    public partial class PurchaseIndent
    {
        public long IndentId { get; set; }
        public string IndentNo { get; set; }
        public Nullable<System.DateTime> IndentDate { get; set; }
        public Nullable<long> RequisitionId { get; set; }
        public string RequisitionNo { get; set; }
        public Nullable<int> FinYearId { get; set; }
        public string IndentType { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> CompanyBranchId { get; set; }
        public Nullable<int> LocationId { get; set; }
        public Nullable<int> IndentByUserId { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public Nullable<int> CustomerBranchId { get; set; }
        public string Remarks1 { get; set; }
        public string Remarks2 { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string IndentStatus { get; set; }
        public string ApprovalStatus { get; set; }
        public Nullable<int> ApprovedBy { get; set; }
        public Nullable<System.DateTime> ApprovedDate { get; set; }
        public string RejectionStatus { get; set; }
        public Nullable<int> RejectedBy { get; set; }
        public Nullable<System.DateTime> RejectedDate { get; set; }
        public string RejectedReason { get; set; }
        public Nullable<int> IndentSequence { get; set; }
    }
}
