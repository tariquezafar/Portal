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
    
    public partial class Return
    {
        public long ReturnedID { get; set; }
        public string ReturnedNo { get; set; }
        public Nullable<System.DateTime> ReturnedDate { get; set; }
        public Nullable<long> InvoiceID { get; set; }
        public string InvoiceNo { get; set; }
        public Nullable<int> FinYearId { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<long> CompanyBranchId { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ApprovalStatus { get; set; }
        public Nullable<int> ReturnedSequence { get; set; }
        public string Warranty { get; set; }
    }
}