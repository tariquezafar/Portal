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
    
    public partial class FabricationBOMDetail
    {
        public long FabricationBOMId { get; set; }
        public Nullable<long> FabricationId { get; set; }
        public Nullable<long> AssemblyId { get; set; }
        public Nullable<long> ProductId { get; set; }
        public Nullable<decimal> BOMQty { get; set; }
        public string ProcessType { get; set; }
        public Nullable<decimal> ScrapPercentage { get; set; }
    }
}