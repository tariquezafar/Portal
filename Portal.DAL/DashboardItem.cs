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
    
    public partial class DashboardItem
    {
        public long DashboardItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemDisplayName { get; set; }
        public string ModuleName { get; set; }
        public Nullable<bool> Status { get; set; }
    }
}