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
    
    public partial class JobCardDetail
    {
        public long JobCardDetailID { get; set; }
        public Nullable<long> JobCardID { get; set; }
        public Nullable<long> ProductID { get; set; }
        public long ServiceItemID { get; set; }
        public string ServiceItemName { get; set; }
        public string CustComplaintObservation { get; set; }
        public string SupervisorAdvice { get; set; }
        public Nullable<decimal> AmountEstimated { get; set; }
    }
}