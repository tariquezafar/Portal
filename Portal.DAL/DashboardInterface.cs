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
    
    public partial class DashboardInterface
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public string ModuleName { get; set; }
        public string ContainerNo { get; set; }
        public string ContainerName { get; set; }
        public bool Status { get; set; }
        public Nullable<int> CompanyBranchId { get; set; }
        public Nullable<int> SequenceNo { get; set; }
    }
}