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
    
    public partial class POProductSchedule
    {
        public int PoProductScheduleId { get; set; }
        public Nullable<long> POId { get; set; }
        public Nullable<int> CompanyBranchId { get; set; }
        public string LocationName { get; set; }
        public Nullable<long> ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string UOMName { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public Nullable<System.DateTime> DeliveryDate { get; set; }
        public Nullable<decimal> SchQuantity { get; set; }
        public Nullable<System.DateTime> ConDeliveryDate { get; set; }
    }
}
