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
    
    public partial class ComplaintServiceProductDetail
    {
        public long ComplaintProductDetailID { get; set; }
        public long ComplaintId { get; set; }
        public Nullable<long> ProductId { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> Quantity { get; set; }
    }
}
