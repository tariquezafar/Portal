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
    
    public partial class ProductInternalStock
    {
        public long TrnId { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> CompanyBranchId { get; set; }
        public Nullable<int> LocationId { get; set; }
        public Nullable<long> ProductId { get; set; }
        public string TrnType { get; set; }
        public Nullable<System.DateTime> TrnDate { get; set; }
        public Nullable<decimal> TrnQty { get; set; }
        public string TrnReferenceNo { get; set; }
        public Nullable<System.DateTime> TrnReferenceDate { get; set; }
    }
}
