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
    
    public partial class AdditionalTax
    {
        public int AddTaxId { get; set; }
        public string AddTaxName { get; set; }
        public Nullable<int> GLId { get; set; }
        public string GLCode { get; set; }
        public Nullable<long> SLId { get; set; }
        public string SLCode { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<bool> Status { get; set; }
    }
}
